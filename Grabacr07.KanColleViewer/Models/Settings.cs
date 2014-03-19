﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grabacr07.KanColleViewer.Models.Data.Xml;
using Livet;
using MetroRadiance.Core;

namespace Grabacr07.KanColleViewer.Models
{
	public class Settings : NotificationObject
	{
		#region static members

		private static readonly string filePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"grabacr.net",
			"KanColleViewer",
			"Settings.xml");

        private static readonly string CurrentSettingsVersion = "1.2";

		public static Settings Current { get; set; }

		public static void Load()
		{
			try
			{
				Current = filePath.ReadXml<Settings>();
                if (Current.SettingsVersion != CurrentSettingsVersion)
                    Current = GetInitialSettings();
			}
			catch (Exception ex)
			{
				Current = GetInitialSettings();
				Debug.WriteLine(ex);
			}
		}

		public static Settings GetInitialSettings()
		{
			return new Settings
			{
				SettingsVersion = CurrentSettingsVersion,
                ScreenshotFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
				ScreenshotFilename = "KanColle-{0:d04}.png",
				ScreenshotImageFormat = SupportedImageFormat.Png,
				CanDisplayBuildingShipName = false,
				EnableLogging = false,
				EnableTranslations = true,
				EnableAddUntranslated = true,
                EnableCriticalNotify = true,
                EnableCriticalAccent = true,
                EnableUpdateNotification = true,
                EnableUpdateTransOnStart = true
			};
		}

		#endregion

        #region SettingsVersion 変更通知プロパティ
        
        private string _SettingsVersion;

        public string SettingsVersion
        {
            get { return this._SettingsVersion; }
            set
            {
                if (this._SettingsVersion != value)
                {
                    this._SettingsVersion = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region ScreenshotFolder 変更通知プロパティ

        private string _ScreenshotFolder;

		/// <summary>
		/// スクリーンショットの保存先フォルダーを取得または設定します。
		/// </summary>
		public string ScreenshotFolder
		{
			get { return this._ScreenshotFolder; }
			set
			{
				if (this._ScreenshotFolder != value)
				{
					this._ScreenshotFolder = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ScreenshotFilename 変更通知プロパティ

		private string _ScreenshotFilename;

		/// <summary>
		/// スクリーンショットのファイル名を取得または設定します。
		/// </summary>
		public string ScreenshotFilename
		{
			get { return this._ScreenshotFilename; }
			set
			{
				if (this._ScreenshotFilename != value)
				{
					this._ScreenshotFilename = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ScreenshotImageFormat 変更通知プロパティ

		private SupportedImageFormat _ScreenshotImageFormat;

		/// <summary>
		/// スクリーンショットのイメージ形式を取得または設定します。
		/// </summary>
		public SupportedImageFormat ScreenshotImageFormat
		{
			get
			{
				switch (this._ScreenshotImageFormat)
				{
					case SupportedImageFormat.Png:
					case SupportedImageFormat.Jpeg:
						break;
					default:
						this._ScreenshotImageFormat = SupportedImageFormat.Png;
						break;
				}
				return this._ScreenshotImageFormat;
			}
			set
			{
				if (this._ScreenshotImageFormat != value)
				{
					this._ScreenshotImageFormat = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region CanDisplayBuildingShipName 変更通知プロパティ

		private bool _CanDisplayBuildingShipName;

		/// <summary>
		/// 建造中の艦の名前を表示するかどうかを示す値を取得または設定します。
		/// </summary>
		public bool CanDisplayBuildingShipName
		{
			get { return this._CanDisplayBuildingShipName; }
			set
			{
				if (this._CanDisplayBuildingShipName != value)
				{
					this._CanDisplayBuildingShipName = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyBuildingComplete 変更通知プロパティ

		private bool _NotifyBuildingCompleted;

		public bool NotifyBuildingCompleted
		{
			get { return this._NotifyBuildingCompleted; }
			set
			{
				if (this._NotifyBuildingCompleted != value)
				{
					this._NotifyBuildingCompleted = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyExpeditionReturned 変更通知プロパティ

		private bool _NotifyExpeditionReturned;

		public bool NotifyExpeditionReturned
		{
			get { return this._NotifyExpeditionReturned; }
			set
			{
				if (this._NotifyExpeditionReturned != value)
				{
					this._NotifyExpeditionReturned = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region NotifyRepairingCompleted 変更通知プロパティ

		private bool _NotifyRepairingCompleted;

		public bool NotifyRepairingCompleted
		{
			get { return this._NotifyRepairingCompleted; }
			set
			{
				if (this._NotifyRepairingCompleted != value)
				{
					this._NotifyRepairingCompleted = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableProxy 変更通知プロパティ

		private bool _EnableProxy;

		/// <summary>
		/// プロキシサーバーを使用して通信をするかどうかを取得または設定します。
		/// </summary>
		public bool EnableProxy
		{
			get { return this._EnableProxy; }
			set
			{
				if (this._EnableProxy != value)
				{
					this._EnableProxy = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableSSLProxy 変更通知プロパティ

		private bool _EnableSSLProxy;

		/// <summary>
		/// プロキシサーバーを使用して SSL 通信をするかどうかを取得または設定します。
		/// </summary>
		public bool EnableSSLProxy
		{
			get { return this._EnableSSLProxy; }
			set
			{
				if (this._EnableSSLProxy != value)
				{
					this._EnableSSLProxy = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ProxyHost 変更通知プロパティ

		private string _ProxyHost;

		/// <summary>
		/// プロキシサーバーのホスト名を取得または設定します。
		/// </summary>
		public string ProxyHost
		{
			get { return this._ProxyHost; }
			set
			{
				if (this._ProxyHost != value)
				{
					this._ProxyHost = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ProxyPort 変更通知プロパティ

		private UInt16 _ProxyPort;

		/// <summary>
		/// プロキシサーバーのポート番号を取得または設定します。
		/// </summary>
		public UInt16 ProxyPort
		{
			get { return this._ProxyPort; }
			set
			{
				if (this._ProxyPort != value)
				{
					this._ProxyPort = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region TopMost 変更通知プロパティ

		private bool _TopMost;

		/// <summary>
		/// メイン ウィンドウを常に最前面に表示するかどうかを示す値を取得または設定します。
		/// </summary>
		public bool TopMost
		{
			get { return this._TopMost; }
			set
			{
				if (this._TopMost != value)
				{
					this._TopMost = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Culture 変更通知プロパティ

		private string _Culture;

		/// <summary>
		/// カルチャを取得または設定します。
		/// </summary>
		public string Culture
		{
			get { return this._Culture; }
			set
			{
				if (this._Culture != value)
				{
					this._Culture = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region BrowserZoomFactor 変更通知プロパティ

		private int _BrowserZoomFactorPercentage = 100;
		private double? _BrowserZoomFactor;

		/// <summary>
		/// ブラウザーの拡大率 (パーセンテージ) を取得または設定します。
		/// </summary>
		public int BrowserZoomFactorPercentage
		{
			get { return this._BrowserZoomFactorPercentage; }
			set { this._BrowserZoomFactorPercentage = value; }
		}

		/// <summary>
		/// ブラウザーの拡大率を取得または設定します。
		/// </summary>
		[XmlIgnore]
		public double BrowserZoomFactor
		{
			get { return this._BrowserZoomFactor ?? (this._BrowserZoomFactor = this.BrowserZoomFactorPercentage / 100.0).Value; }
			set
			{
				if (this._BrowserZoomFactor != value)
				{
					this._BrowserZoomFactor = value;
					this._BrowserZoomFactorPercentage = (int)(value * 100);
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ReSortieCondition 変更通知プロパティ

		private ushort _ReSortieCondition = 40;

		/// <summary>
		/// 艦隊が再出撃可能と判断する基準となるコンディション値を取得または設定します。
		/// </summary>
		public ushort ReSortieCondition
		{
			get { return this._ReSortieCondition; }
			set
			{
				if (this._ReSortieCondition != value)
				{
					this._ReSortieCondition = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableLogging 変更通知プロパティ

		private bool _EnableLogging;

		public bool EnableLogging
		{
			get { return this._EnableLogging; }
			set
			{
				if (this._EnableLogging != value)
				{
					this._EnableLogging = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region EnableTranslations 変更通知プロパティ

		private bool _EnableTranslations;

		public bool EnableTranslations
		{ 
			get { return this._EnableTranslations; }
			set
			{
				if (this._EnableTranslations != value)
				{
					this._EnableTranslations = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableAddUntranslated 変更通知プロパティ

		private bool _EnableAddUntranslated;

		public bool EnableAddUntranslated
		{
			get { return this._EnableAddUntranslated; }
			set
			{
				if (this._EnableAddUntranslated != value)
				{
					this._EnableAddUntranslated = value;
					this.RaisePropertyChanged();
				}
			}
		}
        #endregion

        #region EnableCriticalNotify 変更通知プロパティ

        private bool _EnableCriticalNotify;

        public bool EnableCriticalNotify
        {
            get { return this._EnableCriticalNotify; }
            set
            {
                if (this._EnableCriticalNotify != value)
                {
                    this._EnableCriticalNotify = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region EnableCriticalAccent 変更通知プロパティ

        private bool _EnableCriticalAccent;

        public bool EnableCriticalAccent
        {
            get { return this._EnableCriticalAccent; }
            set
            {
                if (this._EnableCriticalAccent != value)
                {
                    this._EnableCriticalAccent = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region EnableUpdateNotification 変更通知プロパティ

        private bool _EnableUpdateNotification;

        public bool EnableUpdateNotification
        {
            get { return this._EnableUpdateNotification; }
            set
            {
                if (this._EnableUpdateNotification != value)
                {
                    this._EnableUpdateNotification = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region EnableUpdateTransOnStart 変更通知プロパティ

        private bool _EnableUpdateTransOnStart;

        public bool EnableUpdateTransOnStart
        {
            get { return this._EnableUpdateTransOnStart; }
            set
            {
                if (this._EnableUpdateTransOnStart != value)
                {
                    this._EnableUpdateTransOnStart = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public void Save()
		{
			try
			{
				this.WriteXml(filePath);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
