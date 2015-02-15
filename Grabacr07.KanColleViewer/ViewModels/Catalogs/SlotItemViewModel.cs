using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleViewer.Properties;
using Livet;

namespace Grabacr07.KanColleViewer.ViewModels.Catalogs
{
	public class SlotItemViewModel : ViewModel
	{
		private int count;
		public List<Counter> Ships { get; private set; }

		public class Counter
		{
			public Ship Ship { get; set; }
			public int Count { get; set; }

			public string ShipName
			{
			    get { return this.Ship.Info.Name; }
			}

		    public string ShipLevel
		    {
		        get { return "Lv." + this.Ship.Level; }
		    }

		    public string CountString
		    {
		        get { return this.Count == 1 ? "" : " x " + this.Count + " "; }
		    }

			public string StatsToolTip
			{
				get
				{
					string AddDetail = "";
					if (this.Ship.Info.UntranslatedName != "")
						AddDetail += this.Ship.Info.UntranslatedName + "\n";

					foreach (ShipSlot s in this.Ship.EquippedSlots) {
						AddDetail += String.Format("{0}{1}\n", s.Item.Info.Name, s.Item.Level > 0 ? " +" + s.Item.Level : "");
					}

					return AddDetail.TrimEnd('\n');
				}
			}
		}


		public SlotItemInfo SlotItem { get; set; }
		public int Level { get; set; }

		public int Count
		{
			get { return this.count; }
			set { this.count = this.Remainder = value; }
		}

		public int Remainder { get; set; }


		public SlotItemViewModel()
		{
			this.Ships = new List<Counter>();
		}

		public SlotItemViewModel(SlotItemInfo item)
		{
			this.Ships = new List<Counter>();
			this.SlotItem = item;
			this.Level = 0;
		}

		public void AddShip(Ship ship)
		{
			var target = this.Ships.FirstOrDefault(x => x.Ship.Id == ship.Id);
			if (target == null)
			{
				this.Ships.Add(new Counter { Ship = ship, Count = 1 });
			}
			else
			{
				target.Count++;
			}

			this.Remainder--;
		}

		public string DetailedToolTip
		{
			get
			{
				string _Detail = this.Detail;
				return this.SlotItem.Name + (this.Level > 0 ? " +" + this.Level : "") + (_Detail != "" ? "\n" + _Detail : "");
			}
		}

		public string Detail
		{
			get
			{
				List<string> details = new List<string>();

				if (this.SlotItem.Firepower != 0) details.Add(StatFormat(SlotItem.Firepower, Resources.Stats_Firepower));
				if (this.SlotItem.AA != 0) details.Add(StatFormat(SlotItem.AA, Resources.Stats_AntiAir));
				if (this.SlotItem.Torpedo != 0) details.Add(StatFormat(SlotItem.Torpedo, Resources.Stats_Torpedo));
				if (this.SlotItem.AntiSub != 0) details.Add(StatFormat(SlotItem.AntiSub, Resources.Stats_AntiSub));
				if (this.SlotItem.SightRange != 0) details.Add(StatFormat(SlotItem.SightRange, Resources.Stats_SightRange));
				if (this.SlotItem.Speed != 0) details.Add(StatFormat(SlotItem.Speed, Resources.Stats_Speed));
				if (this.SlotItem.Armor != 0) details.Add(StatFormat(SlotItem.Armor, Resources.Stats_Armor));
				if (this.SlotItem.Health != 0) details.Add(StatFormat(SlotItem.Health, Resources.Stats_Health));
				if (this.SlotItem.Luck != 0) details.Add(StatFormat(SlotItem.Luck, Resources.Stats_Luck));
				if (this.SlotItem.Evasion != 0) details.Add(StatFormat(SlotItem.Evasion, Resources.Stats_Evasion));
				if (this.SlotItem.Accuracy != 0) details.Add(StatFormat(SlotItem.Accuracy, Resources.Stats_Accuracy));
				if (this.SlotItem.DiveBomb != 0) details.Add(StatFormat(SlotItem.DiveBomb, Resources.Stats_DiveBomb));
				if (this.SlotItem.AttackRange > 0) details.Add(String.Format(" {1}({0})", this.SlotItem.AttackRange, Resources.Stats_AttackRange));
				//if (this.SlotItem.RawData.api_raik > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_raik + " api_raik";
				//if (this.SlotItem.RawData.api_raim > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_raim + " api_raim";
				//if (this.SlotItem.RawData.api_sakb > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_sakb + " api_sakb";
				//if (this.SlotItem.RawData.api_atap > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_atap + " api_atap";
				//if (this.SlotItem.RawData.api_rare > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_rare + " api_rare";
				//if (this.SlotItem.RawData.api_bakk > 0) AddDetail += (AddDetail != "" ? "\n" : "") + " +" + this.SlotItem.RawData.api_bakk + " api_bakk";

				return String.Join("\n", details);
			}
		}

		private string StatFormat(int stat, string name)
		{
			return String.Format(" {0:+#;-#} {1}", stat, name);
		}
	}
}
