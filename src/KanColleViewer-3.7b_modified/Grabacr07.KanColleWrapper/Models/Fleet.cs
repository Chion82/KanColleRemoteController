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

//JSON OUTPUT
using Codeplex.Data;

//DEBUG
using System.Diagnostics;
using System.Windows.Forms;

namespace Grabacr07.KanColleWrapper.Models
{
	/// <summary>
	/// 複数の艦娘によって編成される、単一の常設艦隊を表します。
	/// </summary>
	public class Fleet : DisposableNotifier, IIdentifiable
	{
		private readonly Homeport homeport;
		private Ship[] originalShips; // null も含んだやつ

		#region Id 変更通知プロパティ

		private int _Id;

		public int Id
		{
			get { return this._Id; }
			private set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Name 変更通知プロパティ

		private string _Name;

		public string Name
		{
			get { return this._Name; }
			internal set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.State.Condition.Name = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Ships 変更通知プロパティ

		private Ship[] _Ships = new Ship[0];

		/// <summary>
		/// 艦隊に所属している艦娘の配列を取得します。
		/// </summary>
		public Ship[] Ships
		{
			get { return this._Ships; }
			private set
			{
				if (this._Ships != value)
				{
					this._Ships = value;
					this.RaisePropertyChanged();
				}
                for (int i = 0; i < this.Ships.Length;i++ )
                {
                    this.Ships[i].SetFleet(this);
                }
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		public bool IsInSortie { get; private set; }

		public FleetState State { get; private set; }

		/// <summary>
		/// 艦隊の遠征に関するステータスを取得します。
		/// </summary>
		public Expedition Expedition { get; private set; }


		internal Fleet(Homeport parent, kcsapi_deck rawData)
		{
			this.homeport = parent;

			this.State = new FleetState(parent, this);
			this.Expedition = new Expedition(this);
			this.CompositeDisposable.Add(this.State);
			this.CompositeDisposable.Add(this.Expedition);

			this.Update(rawData);
		}


		/// <summary>
		/// 指定した <see cref="kcsapi_deck"/> を使用して艦隊の情報をすべて更新します。
		/// </summary>
		/// <param name="rawData">エンド ポイントから取得したデータ。</param>
		internal void Update(kcsapi_deck rawData)
		{
			this.Id = rawData.api_id;
			this.Name = rawData.api_name;

			this.Expedition.Update(rawData.api_mission);
			this.UpdateShips(rawData.api_ship.Select(id => this.homeport.Organization.Ships[id]).ToArray());
		
            //Export data to json file
            this.ExportToFile();
        }

		#region 艦の編成 (Change, Unset)

		/// <summary>
		/// 艦隊の編成を変更します。
		/// </summary>
		/// <param name="index">編成を変更する艦のインデックス。通常は 0 ～ 5、旗艦以外をすべて外す場合は -1。</param>
		/// <param name="ship">艦隊の <paramref name="index"/> 番目に新たに編成する艦。<paramref name="index"/> 番目から艦を外す場合は null。</param>
		/// <returns>このメソッドを呼び出した時点で <paramref name="index"/> に配置されていた艦。</returns>
		internal Ship Change(int index, Ship ship)
		{
			var current = this.originalShips[index];

			List<Ship> list;
			if (index == -1)
			{
				list = this.originalShips.Take(1).ToList();
			}
			else
			{
				list = this.originalShips.ToList();
				list[index] = ship;
				list.RemoveAll(x => x == null);
			}

			var ships = new Ship[this.originalShips.Length];
			Array.Copy(list.ToArray(), ships, list.Count);

			this.UpdateShips(ships);

            //Export data to JSON file
            this.ExportToFile();

			return current;
		}

		/// <summary>
		/// 指定したインデックスの艦を艦隊から外します。
		/// </summary>
		/// <param name="index">艦隊から外す艦のインデックス。</param>
		internal void Unset(int index)
		{
			var list = this.originalShips.ToList();
			list[index] = null;
			list.RemoveAll(x => x == null);

			var ships = new Ship[this.originalShips.Length];
			Array.Copy(list.ToArray(), ships, list.Count);

			this.UpdateShips(ships);
		}

		/// <summary>
		/// 旗艦以外のすべての艦を艦隊から外します。
		/// </summary>
		internal void UnsetAll()
		{
			var list = this.originalShips.Take(1).ToList();
			var ships = new Ship[this.originalShips.Length];
			Array.Copy(list.ToArray(), ships, list.Count);

			this.UpdateShips(ships);
		}

		#endregion

		#region 出撃 (Sortie, Homing)

		internal void Sortie()
		{
			if (!this.IsInSortie)
			{
				this.IsInSortie = true;
				this.State.Update();
			}
            //Export data to JSON file
            this.ExportToFile();
		}

		internal void Homing()
		{
			if (this.IsInSortie)
			{
				this.IsInSortie = false;
				this.State.Update();
			}
            //Export data to JSON file
            this.ExportToFile();
		}

		#endregion

		private void UpdateShips(Ship[] ships)
		{
			this.originalShips = ships;
			this.Ships = ships.Where(x => x != null).ToArray();

			this.State.Calculate();
			this.State.Update();
		}

		public override string ToString()
		{
			return string.Format("ID = {0}, Name = \"{1}\", Ships = {2}", this.Id, this.Name, this.Ships.Select(s => "\"" + s.Info.Name + "\"").ToString(","));
		}

        /// <summary>
        /// Export fleet info to JSON file
        /// Added by @Chion82
        /// </summary>
        public void ExportToFile()
        {
            if (this.State == null)
                MessageBox.Show("state=null!");
            try
            {
                var jsonObj = new
                {
                    fleetId = this.Id,
                    fleetName = this.Name,
                    expedition = FleetInfoUtils.GetExpeditionInfo(this.Expedition),
                    isInSortie = this.IsInSortie,
                    state = FleetInfoUtils.GetFleetConditionInfo(this.State),
                    ships = FleetInfoUtils.GetShipArray(this.Ships)
                };
                if (this.Id<=10)
                {
                    fleets[this.Id-1] = jsonObj;
                }
                String jsonStr = DynamicJson.Serialize(fleets);
                DataExportUtils.WriteStringToFile("fleets.json", jsonStr);
            }
            catch (Exception e) 
            {
                MessageBox.Show(e.ToString());
            }

        }

        static public Object[] fleets = new Object[10];

	}
}
