using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Win32;
using Livet;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using System.Net;

namespace Grabacr07.KanColleWrapper
{
	public partial class KanColleProxy
	{
		private readonly IConnectableObservable<SessionData> connectableSessionSource;
		private readonly IConnectableObservable<SessionData> apiSource;
		private readonly LivetCompositeDisposable compositeDisposable;

		public IObservable<SessionData> SessionSource
		{
			get { return this.connectableSessionSource.AsObservable(); }
		}

		public IObservable<SessionData> ApiSessionSource
		{
			get { return this.apiSource.AsObservable(); }
		}

		public IProxySettings UpstreamProxySettings { get; set; }


		public KanColleProxy()
		{
			this.compositeDisposable = new LivetCompositeDisposable();

			this.connectableSessionSource = Observable
				.FromEvent<EventHandler<SessionEventArgs>, SessionData>(
					h => (sender, e) => h(new SessionData(e)),
					h => ProxyServer.BeforeResponse += h,
					h => ProxyServer.BeforeResponse -= h)
				.Publish();

			this.apiSource = this.connectableSessionSource
				.Where(s => s.RequestUri.AbsolutePath.StartsWith("/kcsapi"))
				.Where(s => s.ContentType.Equals("text/plain"))
			#region .Do(debug)
#if DEBUG
.Do(session =>
				{
					Debug.WriteLine("==================================================");
					Debug.WriteLine("Fiddler session: ");
					Debug.WriteLine(session);
					Debug.WriteLine("");
				})
#endif
			#endregion
			.Publish();
		}


		public void Startup(int proxy = 37564)
		{
			ProxyServer.Start(IPAddress.Parse("127.0.0.1"), proxy);
			ProxyServer.BeforeRequest += this.SetUpstreamProxyHandler;

			SetIESettings("127.0.0.1:" + proxy);

			this.compositeDisposable.Add(this.connectableSessionSource.Connect());
			this.compositeDisposable.Add(this.apiSource.Connect());
		}

		public void Shutdown()
		{
			this.compositeDisposable.Dispose();

			ProxyServer.BeforeRequest -= this.SetUpstreamProxyHandler;
			ProxyServer.Stop();
		}


		private static void SetIESettings(string proxyUri)
		{
			// ReSharper disable InconsistentNaming
			const int INTERNET_OPTION_PROXY = 38;
			const int INTERNET_OPEN_TYPE_PROXY = 3;
			// ReSharper restore InconsistentNaming

			INTERNET_PROXY_INFO proxyInfo;
			proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
			proxyInfo.proxy = Marshal.StringToHGlobalAnsi(proxyUri);
			proxyInfo.proxyBypass = Marshal.StringToHGlobalAnsi("local");

			var proxyInfoSize = Marshal.SizeOf(proxyInfo);
			var proxyInfoPtr = Marshal.AllocCoTaskMem(proxyInfoSize);
			Marshal.StructureToPtr(proxyInfo, proxyInfoPtr, true);

			NativeMethods.InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, proxyInfoPtr, proxyInfoSize);
		}

		/// <summary>
		/// Fiddler からのリクエスト発行時にプロキシを挟む設定を行います。
		/// </summary>
		/// <param name="requestingSession">通信を行おうとしているセッション。</param>
		private void SetUpstreamProxyHandler(object sender, SessionEventArgs requestingSession)
		{
			var settings = this.UpstreamProxySettings;
			if (settings == null) return;

			var useGateway = !string.IsNullOrEmpty(settings.Host) && settings.IsEnabled;
			if (!useGateway || (IsSessionSSL(requestingSession) && !settings.IsEnabledOnSSL)) return;

			requestingSession.UpstreamProxy = new ProxySetting(settings.Host, settings.Port);
		}

		/// <summary>
		/// セッションが SSL 接続を使用しているかどうかを返します。
		/// </summary>
		/// <param name="session">セッション。</param>
		/// <returns>セッションが SSL 接続を使用する場合は true、そうでない場合は false。</returns>
		internal static bool IsSessionSSL(SessionEventArgs session)
		{
			// 「http://www.dmm.com:433/」の場合もあり、これは Session.isHTTPS では判定できない
			return session.IsSecure ||
				session.ProxyRequest.RequestUri.OriginalString.StartsWith("https:") ||
				session.ProxyRequest.RequestUri.OriginalString.Contains(":443");
		}
	}
}
