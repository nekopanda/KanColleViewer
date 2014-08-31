using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleWrapper;
using Livet.EventListeners;

namespace Grabacr07.KanColleViewer.Plugins.ViewModels
{
    public class RankingsViewModel : ViewModel
	{
		private readonly ConcurrentDictionary<int, RankingViewModel[]> CurrentData;

		private int MyRankingsPage;

        #region Rankings

        private RankingViewModel[] _Rankings;

        public RankingViewModel[] Rankings
        {
            get { return this._Rankings; }
            set
            {
                if (this._Rankings != value)
                {
                    this._Rankings = value;
                    this.RaisePropertyChanged();

                    this.HasNoRankings = !(this._Rankings != null && this._Rankings.Length > 0);
                }
            }
        }

        #endregion

        #region TotalRanked

        private int _TotalRanked;

        public int TotalRanked
        {
            get { return this._TotalRanked; }
            set
            {
                if (this._TotalRanked != value)
                {
                    this._TotalRanked = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region TotalPages

        private int _TotalPages;

        public int TotalPages
        {
            get { return this._TotalPages; }
            set
            {
                if (this._TotalPages != value)
                {
                    this._TotalPages = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region CurrentPage

        private int _CurrentPage;

        public int CurrentPage
        {
            get { return this._CurrentPage; }
            set
            {
                if (this._CurrentPage != value)
                {
                    this._CurrentPage = value;
                    this.RaisePropertyChanged();

					this.CanGoNextPage = value < this.TotalPages;
					this.CanGoPreviousPage = value > 1 && value - 1 <= this.TotalPages;
                }
            }
        }

        #endregion

        #region HasNoRankings

        private bool _HasNoRankings;

        public bool HasNoRankings
        {
            get { return this._HasNoRankings; }
            set
            {
                if (this._HasNoRankings != value)
                {
                    this._HasNoRankings = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

		#region CanGoPreviousPage

		private bool _CanGoPreviousPage;

		public bool CanGoPreviousPage
		{
			get { return this._CanGoPreviousPage; }
			set
			{
				if (this._CanGoPreviousPage != value)
				{
					this._CanGoPreviousPage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		
		#region CanGoNextPage

		private bool _CanGoNextPage;

		public bool CanGoNextPage
		{
			get { return this._CanGoNextPage; }
			set
			{
				if (this._CanGoNextPage != value)
				{
					this._CanGoNextPage = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

        private void Update()
        {
            this.Rankings = KanColleClient.Current.Homeport.Rankings.Current.Select(x => new RankingViewModel(x)).ToArray();
            this.TotalRanked = KanColleClient.Current.Homeport.Rankings.TotalRanked;
            this.TotalPages = KanColleClient.Current.Homeport.Rankings.TotalPages;
            this.CurrentPage = KanColleClient.Current.Homeport.Rankings.CurrentPage;

			if (this.CurrentData.Keys.Any(x => x == this.CurrentPage)) this.CurrentData.Clear();
			this.CurrentData.GetOrAdd(this.CurrentPage, this.Rankings);

			if (this.Rankings.Any(x => x.NickName.Equals(KanColleClient.Current.Homeport.Admiral.Nickname)))
			{
				this.MyRankingsPage = this.CurrentPage;
			}
        }

        public RankingsViewModel()
        {
			this.CurrentData = new ConcurrentDictionary<int, RankingViewModel[]>();
			this.MyRankingsPage = 0;
			
			this.Update();
            this.CompositeDisposable.Add(new PropertyChangedEventListener(KanColleClient.Current.Homeport.Rankings)
            {
                {
                    () => KanColleClient.Current.Homeport.Rankings.Current,
                    (sender, args) => Update()
                },
            });
        }

		public void ToMyRank()
		{
			this.ToPage(this.MyRankingsPage);
		}

		public void ToTop1()
		{
			this.ToPage(1);
		}

		public void ToTop501()
		{
			this.ToPage(51);
		}

		public void ToNextPage()
		{
			this.ToPage(this.CurrentPage + 1);
		}

		public void ToPreviousPage()
		{
			this.ToPage(this.CurrentPage - 1);
		}

		public void ToPage(int page)
		{
			try
			{
				this.Rankings = this.CurrentData.First(x => x.Key == page).Value;
			}
			catch
			{
				this.Rankings = new RankingViewModel[0];
				this.HasNoRankings = true;
			}
			this.CurrentPage = page;
		}
	}
}
