using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;

//DEBUG
using System.Windows.Forms;

namespace Grabacr07.KanColleViewer.ViewModels.Contents.Fleets
{
	/// <summary>
	/// 単一の艦隊情報を提供します。
	/// </summary>
    ///******************** 其中一个舰队的信息************************************

	public class FleetViewModel : ItemViewModel
	{
		public Fleet Source { get; private set; }

		public int Id
		{
			get { return this.Source.Id; }
		}

		public string Name
		{
			get { return string.IsNullOrEmpty(this.Source.Name.Trim()) ? "(第 " + this.Source.Id + " 艦隊)" : this.Source.Name; }
		}

		/// <summary>
		/// 艦隊に所属している艦娘のコレクションを取得します。
		/// </summary>
        /// ***********************全部舰娘信息************************
		public ShipViewModel[] Ships
		{
			get { return this.Source.Ships.Select(x => new ShipViewModel(x)).ToArray(); }
		}

		public FleetStateViewModel State { get; private set; }

		public ExpeditionViewModel Expedition { get; private set; }

		public ViewModel QuickStateView
		{
			get
			{
				var situation = this.Source.State.Situation;
				if (situation == FleetSituation.Empty)
				{
					return NullViewModel.Instance;
				}
				if (situation.HasFlag(FleetSituation.Sortie))
				{
					return this.State.Sortie;
				}
				if (situation.HasFlag(FleetSituation.Expedition))
				{
					return this.Expedition;
				}

				return this.State.Homeport;
			}
		}


		public FleetViewModel(Fleet fleet)
		{
			this.Source = fleet;

			this.CompositeDisposable.Add(new PropertyChangedEventListener(fleet)
			{
				(sender, args) => this.RaisePropertyChanged(args.PropertyName),
			});
			this.CompositeDisposable.Add(new PropertyChangedEventListener(fleet.State)
			{
				{ "Situation", (sender, args) => this.RaisePropertyChanged("QuickStateView") },
			});

			this.State = new FleetStateViewModel(fleet.State);
			this.CompositeDisposable.Add(this.State);

			this.Expedition = new ExpeditionViewModel(fleet.Expedition);
			this.CompositeDisposable.Add(this.Expedition);

           // this.debug();
		}

        public void debug()
        {
            int len = this.Ships.Length;
            for (int i = 0; i < len; i++)
            {
                String debugStr = "Fleet=" + this.Name
                    + ";ShipName=" + Ships[i].Ship.Info.Name
                    + ";HP=" + Ships[i].Ship.HP.Current
                    + ";Condition=" + Ships[i].Ship.Condition
                    + ";Situation=" + Ships[i].Ship.Situation;
                MessageBox.Show(debugStr);
            }
        }

	}
}
