/*
    This source file is modified by @Chion82
    for the purpose of exporting real-time game data.
    Original code is written by @Grabacr07
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

//DEBUG
using System.Windows.Forms;

//JSON OUTPUT
using Codeplex.Data;

namespace Grabacr07.KanColleWrapper
{
	/// <summary>
	/// 資源および資材の保有状況を表します。
	/// </summary>
	public class Materials : NotificationObject
	{
		#region Fuel 変更通知プロパティ

		private int _Fuel;

		/// <summary>
		/// 所有している燃料数を取得します。
		/// </summary>
		public int Fuel
		{
			get { return this._Fuel; }
			private set
			{
				if (this._Fuel != value)
				{
					this._Fuel = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Ammunition 変更通知プロパティ

		private int _Ammunition;

		/// <summary>
		/// 所有している弾薬数を取得します。
		/// </summary>
		public int Ammunition
		{
			get { return this._Ammunition; }
			private set
			{
				if (this._Ammunition != value)
				{
					this._Ammunition = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Steel 変更通知プロパティ

		private int _Steel;

		/// <summary>
		/// 所有している鉄鋼数を取得します。
		/// </summary>
		public int Steel
		{
			get { return this._Steel; }
			private set
			{
				if (this._Steel != value)
				{
					this._Steel = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region Bauxite 変更通知プロパティ

		private int _Bauxite;

		/// <summary>
		/// 所有しているボーキサイト数を取得します。
		/// </summary>
		public int Bauxite
		{
			get { return this._Bauxite; }
			private set
			{
				if (this._Bauxite != value)
				{
					this._Bauxite = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region DevelopmentMaterials 変更通知プロパティ

		private int _DevelopmentMaterials;

		/// <summary>
		/// 所有している開発資材の数を取得します。
		/// </summary>
		public int DevelopmentMaterials
		{
			get { return this._DevelopmentMaterials; }
			private set
			{
				if (this._DevelopmentMaterials != value)
				{
					this._DevelopmentMaterials = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion

		#region InstantRepairMaterials 変更通知プロパティ

		private int _InstantRepairMaterials;

		/// <summary>
		/// 所有している高速修復材の数を取得します。
		/// </summary>
		public int InstantRepairMaterials
		{
			get { return this._InstantRepairMaterials; }
			private set
			{
				if (this._InstantRepairMaterials != value)
				{
					this._InstantRepairMaterials = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged("Bucket");
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		/// <summary>
		/// バケツ！！！ ぶっかけ！！！！
		/// </summary>
		public int Bucket
		{
			get { return this.InstantRepairMaterials; }
		}

		#endregion

		#region InstantBuildMaterials 変更通知プロパティ

		private int _InstantBuildMaterials;

		/// <summary>
		/// 所有している高速建造材の数を取得します。
		/// </summary>
		public int InstantBuildMaterials
		{
			get { return this._InstantBuildMaterials; }
			private set
			{
				if (this._InstantBuildMaterials != value)
				{
					this._InstantBuildMaterials = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion


		internal Materials(KanColleProxy proxy)
		{
			proxy.api_get_member_material.TryParse<kcsapi_material[]>().Subscribe(x => this.Update(x.Data));
			proxy.api_req_hokyu_charge.TryParse<kcsapi_charge>().Subscribe(x => this.Update(x.Data.api_material));
			proxy.api_req_kousyou_destroyship.TryParse<kcsapi_destroyship>().Subscribe(x => this.Update(x.Data.api_material));
		}


		internal void Update(kcsapi_material[] source)
		{
			if (source != null && source.Length >= 7)
			{
				this.Fuel = source[0].api_value;
				this.Ammunition = source[1].api_value;
				this.Steel = source[2].api_value;
				this.Bauxite = source[3].api_value;
				this.DevelopmentMaterials = source[6].api_value;
				this.InstantRepairMaterials = source[5].api_value;
				this.InstantBuildMaterials = source[4].api_value;
			}

            //DEBUG
            //debug();

            //Export data to JSON file
            this.ExportToFile();
		}

		private void Update(int[] source)
		{
			if (source != null && source.Length == 4)
			{
				this.Fuel = source[0];
				this.Ammunition = source[1];
				this.Steel = source[2];
				this.Bauxite = source[3];
			}
		}

        
        /// <summary>
        /// Debug method. Test JSON result.
        /// </summary>
        public void debug()
        {
            var jsonObj = new
            {
                fuel = this.Fuel,
                ammunition = this.Ammunition,
                steel = this.Steel,
                bauxite = this.Bauxite,
                developmentmaterials = this.DevelopmentMaterials,
                instantRepairMaterials = this.InstantRepairMaterials,
                instantBuildMaterials = this.InstantBuildMaterials
            };
            String debugStr = DynamicJson.Serialize(jsonObj);
            MessageBox.Show(debugStr);
        }

        /// <summary>
        /// Export current material info to JSON file
        /// Added by @Chion82
        /// </summary>
        public void ExportToFile()
        {
            var jsonObj = new
            {
                fuel = this.Fuel,
                ammunition = this.Ammunition,
                steel = this.Steel,
                bauxite = this.Bauxite,
                developmentMaterials = this.DevelopmentMaterials,
                instantRepairMaterials = this.InstantRepairMaterials,
                instantBuildMaterials = this.InstantBuildMaterials
            };
            String jsonStr = DynamicJson.Serialize(jsonObj);
            DataExportUtils.WriteStringToFile("materials.json", jsonStr);
        }

	}
}
