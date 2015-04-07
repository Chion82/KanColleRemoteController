/*
    This source file is modified by @Chion82
    for the purpose of exporting real-time game data.
    Original code is written by @Grabacr07
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper.Internal;
using Livet;

//JSON OUTPUT
using Codeplex.Data;

namespace Grabacr07.KanColleWrapper.Models
{
	/// <summary>
	/// 建造ドックを表します。
	/// </summary>
	public class BuildingDock : TimerNotifier, IIdentifiable
	{
		private bool notificated;

		#region Id 変更通知プロパティ

		private int _Id;

		public int Id
		{
			get { return this._Id; }
			private set
			{
				this._Id = value;
				this.RaisePropertyChanged();
			}
		}

		#endregion

		#region State 変更通知プロパティ

		private BuildingDockState _State;

		public BuildingDockState State
		{
			get { return this._State; }
			private set
			{
				if (this._State != value)
				{
					this._State = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Ship 変更通知プロパティ

		private ShipInfo _Ship;

		/// <summary>
		/// 建造中の艦娘の情報を取得します。
		/// </summary>
		public ShipInfo Ship
		{
			get { return this._Ship; }
			private set
			{
				if (this._Ship != value)
				{
					this._Ship = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region CompleteTime 変更通知プロパティ

		private DateTimeOffset? _CompleteTime;

		/// <summary>
		/// 建造完了時刻を取得します。
		/// </summary>
		public DateTimeOffset? CompleteTime
		{
			get { return this._CompleteTime; }
			private set
			{
				if (this._CompleteTime != value)
				{
					this._CompleteTime = value;
					this.notificated = false;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Remaining 変更通知プロパティ

		private TimeSpan? _Remaining;

		/// <summary>
		/// 建造が完了するまでの残り時間を取得します。1 秒ごとに更新されます。
		/// </summary>
		public TimeSpan? Remaining
		{
			get { return this._Remaining; }
			private set
			{
				if (this._Remaining != value)
				{
					this._Remaining = value;
					this.RaisePropertyChanged();
				}
                
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		/// <summary>
		/// 建造が完了したときに発生しまう。
		/// </summary>
		public event EventHandler<BuildingCompletedEventArgs> Completed;


		internal BuildingDock(kcsapi_kdock rawData)
		{
			this.Update(rawData);
		}


		internal void Update(kcsapi_kdock rawData)
		{
			this.Id = rawData.api_id;
			this.State = (BuildingDockState)rawData.api_state;
			this.Ship = this.State == BuildingDockState.Building || this.State == BuildingDockState.Completed
				? KanColleClient.Current.Master.Ships[rawData.api_created_ship_id]
				: null;
			this.CompleteTime = this.State == BuildingDockState.Building
				? (DateTimeOffset?)Definitions.UnixEpoch.AddMilliseconds(rawData.api_complete_time)
				: null;

            //Export data to JSON file
            this.ExportToFile();
		}

		internal void Finish()
		{
			this.State = BuildingDockState.Completed;
			this.CompleteTime = null;
		}


		protected override void Tick()
		{
			base.Tick();

			if (this.CompleteTime.HasValue)
			{
				var remaining = this.CompleteTime.Value.Subtract(DateTimeOffset.Now);
				if (remaining.Ticks < 0) remaining = TimeSpan.Zero;

				this.Remaining = remaining;

				if (!this.notificated && this.Completed != null && remaining.Ticks <= 0)
				{
					this.Completed(this, new BuildingCompletedEventArgs(this.Id, this.Ship));
					this.notificated = true;
				}
			}
			else
			{
				this.Remaining = null;
			}
		}

        /// <summary>
        /// Export building dock data to JSON file
        /// Added by @Chion82
        /// </summary>
        public void ExportToFile()
        {
            Object jsonObj = null;
            if (this.Ship!=null && this.Remaining!=null)
            {
                jsonObj = new
                {
                    id = this.Id,
                    state = this.State.ToString(),
                    //completeTime = DataExportUtils.GetUnixTimeStramp(this.CompleteTime.Value.LocalDateTime),
                    remaining = this.Remaining.Value.TotalSeconds,
                    ship = new
                    {
                        HP = this.Ship.HP,
                        id = this.Ship.Id,
                        maxAA = this.Ship.MaxAA,
                        maxArmor = this.Ship.MaxArmer,
                        // maxASW = ship.Info.MaxASW,
                        // maxEvasion = ship.Info.MaxEvasion,
                        maxFirePower = this.Ship.MaxFirepower,
                        // maxLOS = ship.Info.MaxLOS,
                        // maxTorpedo = ship.Info.MaxTorpedo,
                        name = this.Ship.Name,
                        // nextRemodelingLevel = ship.Info.NextRemodelingLevel,
                        shipType = this.Ship.ShipType.Name,
                        //slots = ship.Info.Slots.ToString(),
                        speed = this.Ship.Speed.ToString()
                    }
                };
            }
            else
            {
                jsonObj = new
                {
                    id = this.Id,
                    state = this.State.ToString(),
                    //completeTime = 0,
                    remaining = 0,
                    ship = ""
                };
            }
            if (this.Id <= 10)
            {
                buildingDocks[this.Id-1] = jsonObj;
            }
            String jsonStr = DynamicJson.Serialize(buildingDocks);
            DataExportUtils.WriteStringToFile("buildingdocks.json", jsonStr);
        }

        static public Object[] buildingDocks = new Object[10];
	}
}
