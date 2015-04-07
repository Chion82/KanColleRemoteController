/*
    This source file is modified by @Chion82
    for the purpose of exporting real-time game data.
    Original code is written by @Grabacr07
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper.Internal;

//JSON OUTPUT
using Codeplex.Data;

namespace Grabacr07.KanColleWrapper.Models
{
	/// <summary>
	/// プレイヤー (提督) を表します。
	/// </summary>
	public class Admiral : RawDataWrapper<kcsapi_basic>
	{
		public string MemberId
		{
		    get { return this.RawData.api_member_id; }
		}

	    public string Nickname
	    {
	        get { return this.RawData.api_nickname; }
	    }

	    #region Comment 変更通知プロパティ

		private string _Comment;

		public string Comment
		{
			get { return this._Comment; }
			set
			{
				if (this._Comment != value)
				{
					this._Comment = value;
					this.RaisePropertyChanged();
				}
                //Export data to JSON file
                this.ExportToFile();
			}
		}

		#endregion


		/// <summary>
		/// 提督経験値を取得します。
		/// </summary>
		public int Experience
		{
		    get { return this.RawData.api_experience; }
		}

	    /// <summary>
		/// 次のレベルに上がるために必要な提督経験値を取得します。
		/// </summary>
		public int ExperienceForNexeLevel
	    {
	        get { return Models.Experience.GetAdmiralExpForNextLevel(this.RawData.api_level, this.RawData.api_experience); }
	    }

	    /// <summary>
		/// 艦隊司令部レベルを取得します。
		/// </summary>
		public int Level
	    {
	        get { return this.RawData.api_level; }
	    }

	    /// <summary>
		/// 提督のランク名 (元帥, 大将, 中将, ...) を取得します。
		/// </summary>
		public string Rank
	    {
	        get { return Models.Rank.GetName(this.RawData.api_rank); }
	    }

	    /// <summary>
		/// 出撃時の勝利数を取得します。
		/// </summary>
		public int SortieWins
	    {
	        get { return this.RawData.api_st_win; }
	    }

	    /// <summary>
		/// 出撃時の敗北数を取得します。
		/// </summary>
		public int SortieLoses
	    {
	        get { return this.RawData.api_st_lose; }
	    }

	    /// <summary>
		/// 出撃時の勝率を取得します。
		/// </summary>
		public double SortieWinningRate
		{
			get
			{
				var battleCount = this.RawData.api_st_win + this.RawData.api_st_lose;
				if (battleCount == 0) return 0.0;
				return this.RawData.api_st_win / (double)battleCount;
			}
		}

		/// <summary>
		/// 司令部に所属できる艦娘の最大値を取得します。
		/// </summary>
		public int MaxShipCount
		{
		    get { return this.RawData.api_max_chara; }
		}

	    /// <summary>
		/// 司令部が保有できる装備アイテムの最大値を取得します。
		/// </summary>
		public int MaxSlotItemCount
	    {
	        get { return this.RawData.api_max_slotitem; }
	    }


	    internal Admiral(kcsapi_basic rawData)
			: base(rawData)
		{
			this.Comment = this.RawData.api_comment;

            //Export data to JSON file
            this.ExportToFile();
		}

		public override string ToString()
		{
			return string.Format("ID = {0}, Nickname = \"{1}\", Level = {2}, Rank = \"{3}\"", this.MemberId, this.Nickname, this.Level, this.Rank);
		}

        /// <summary>
        /// Export admiral info to JSON file
        /// Added by @Chion82
        /// </summary>
        public void ExportToFile()
        {
            var jsonObj = new
            {
                memberId = this.MemberId,
                nickname = this.Nickname,
                experience = this.Experience,
                experienceForNextLevel = this.ExperienceForNexeLevel,
                level = this.Level,
                maxShipCount = this.MaxShipCount,
                maxSlotItemCount = this.MaxSlotItemCount,
                comment = this.Comment,
                rank = this.Rank,
                sortieLoses = this.SortieLoses,
                sortieWins = this.SortieWins,
                sortieWinningRate = this.SortieWinningRate
            };
            String jsonStr = DynamicJson.Serialize(jsonObj);
            DataExportUtils.WriteStringToFile("admiral.json", jsonStr);
        }

	}
}
