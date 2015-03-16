using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Grabacr07.KanColleWrapper
{
	public class Translations : NotificationObject
	{
		private XDocument ShipsXML;
		private XDocument ShipTypesXML;
		private XDocument EquipmentXML;
		private XDocument OperationsXML;
		private XDocument QuestsXML;
        private XDocument ExpeditionsXML;
		private string CurrentCulture;

		public bool EnableTranslations { get; set; }
		public bool EnableAddUntranslated { get; set; }
		
		#region EquipmentVersion 変更通知プロパティ

		private string _EquipmentVersion;

		public string EquipmentVersion
		{
			get { return _EquipmentVersion; }
			set
			{
				if (_EquipmentVersion != value)
				{
					_EquipmentVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region OperationsVersion 変更通知プロパティ

		private string _OperationsVersion;

		public string OperationsVersion
		{
			get { return _OperationsVersion; }
			set
			{
				if (_OperationsVersion != value)
				{
					_OperationsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region QuestsVersion 変更通知プロパティ

		private string _QuestsVersion;

		public string QuestsVersion
		{
			get { return _QuestsVersion; }
			set
			{
				if (_QuestsVersion != value)
				{
					_QuestsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipsVersion 変更通知プロパティ

		private string _ShipsVersion;

		public string ShipsVersion
		{
			get { return _ShipsVersion; }
			set
			{
				if (_ShipsVersion != value)
				{
					_ShipsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ShipTypesVersion 変更通知プロパティ

		private string _ShipTypesVersion;

		public string ShipTypesVersion
		{
			get { return _ShipTypesVersion; }
			set
			{
				if (_ShipTypesVersion != value)
				{
					_ShipTypesVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

        #region ExpeditionsVersion 変更通知プロパティ

        private string _ExpeditionsVersion;

        public string ExpeditionsVersion
        {
            get { return _ExpeditionsVersion; }
            set
            {
                if (_ExpeditionsVersion != value)
                {
                    _ExpeditionsVersion = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

		internal Translations()
		{
			try
			{
				if (File.Exists("Translations\\Ships.xml")) ShipsXML = XDocument.Load("Translations\\Ships.xml");
				if (File.Exists("Translations\\ShipTypes.xml")) ShipTypesXML = XDocument.Load("Translations\\ShipTypes.xml");
				if (File.Exists("Translations\\Equipment.xml")) EquipmentXML = XDocument.Load("Translations\\Equipment.xml");
				if (File.Exists("Translations\\Operations.xml")) OperationsXML = XDocument.Load("Translations\\Operations.xml");
				if (File.Exists("Translations\\Quests.xml")) QuestsXML = XDocument.Load("Translations\\Quests.xml");
                if (File.Exists("Translations\\Expeditions.xml")) ExpeditionsXML = XDocument.Load("Translations\\Expeditions.xml");

				GetVersions();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		public void ChangeCulture(string Culture)
		{
			CurrentCulture = Culture == "en-US" || Culture == "en" ? "" : (Culture == "ja-JP" || Culture == "ja" ? "ja-JP" : (Culture + "\\"));

			ShipsXML = null;
			ShipTypesXML = null;
			EquipmentXML = null;
			OperationsXML = null;
			QuestsXML = null;
            ExpeditionsXML = null;

			if (!EnableTranslations || CurrentCulture == "ja-JP")
			{
				this.RaisePropertyChanged("CurrentCulture");
				return;
			}

			try
			{
				if (!Directory.Exists("Translations")) Directory.CreateDirectory("Translations");
				if (File.Exists("Translations\\Ships.xml")) ShipsXML = XDocument.Load("Translations\\Ships.xml");
				if (File.Exists("Translations\\ShipTypes.xml")) ShipTypesXML = XDocument.Load("Translations\\ShipTypes.xml");
				if (File.Exists("Translations\\Equipment.xml")) EquipmentXML = XDocument.Load("Translations\\Equipment.xml");
				if (File.Exists("Translations\\Operations.xml")) OperationsXML = XDocument.Load("Translations\\Operations.xml");
				if (File.Exists("Translations\\Quests.xml")) QuestsXML = XDocument.Load("Translations\\Quests.xml");
                if (File.Exists("Translations\\Expeditions.xml")) ExpeditionsXML = XDocument.Load("Translations\\Expeditions.xml");

				GetVersions();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

			this.RaisePropertyChanged("CurrentCulture");
		}

		private void GetVersions()
		{
			if (ShipsXML != null)
				ShipsVersion = ShipsXML.Root.Attribute("Version").Value;
			else
				ShipsVersion = "0.0.0";
			if (ShipTypesXML != null)
				ShipTypesVersion = ShipTypesXML.Root.Attribute("Version").Value;
			else
				ShipTypesVersion = "0.0.0";
			if (EquipmentXML != null)
				EquipmentVersion = EquipmentXML.Root.Attribute("Version").Value;
			else
				EquipmentVersion = "0.0.0";
			if (OperationsXML != null)
				OperationsVersion = OperationsXML.Root.Attribute("Version").Value;
			else
				OperationsVersion = "0.0.0";
			if (QuestsXML != null)
				QuestsVersion = QuestsXML.Root.Attribute("Version").Value;
			else
				QuestsVersion = "0.0.0";
            ExpeditionsVersion = ExpeditionsXML != null ? ExpeditionsXML.Root.Attribute("Version").Value : "0.0.0";
		}

		private IEnumerable<XElement> GetTranslationList(TranslationType Type)
		{
			switch(Type)
			{
				case TranslationType.Ships:
					if (ShipsXML != null) 
						return ShipsXML.Descendants("Ship");
					break;
				case TranslationType.ShipTypes:
					if (ShipTypesXML != null) 
						return ShipTypesXML.Descendants("Type");
					break;
				case TranslationType.Equipment:
					if (EquipmentXML != null) 
						return EquipmentXML.Descendants("Item");
					break;
				case TranslationType.OperationMaps:
					if (OperationsXML != null) 
						return OperationsXML.Descendants("Map");
					break;
				case TranslationType.OperationSortie:
					if (OperationsXML != null) 
						return OperationsXML.Descendants("Sortie");
					break;
				case TranslationType.Quests:
				case TranslationType.QuestTitle:
				case TranslationType.QuestDetail:
					if (QuestsXML != null) 
						return QuestsXML.Descendants("Quest");
					break;
                case TranslationType.Expeditions:
                case TranslationType.ExpeditionTitle:
                case TranslationType.ExpeditionDetail:
                    if (ExpeditionsXML != null)
                        return ExpeditionsXML.Descendants("Expedition");
                    break;
			}

			return null;
		}

		public string GetTranslation(string JPString, TranslationType Type, Object RawData, int ID = -1)
		{
			if (!EnableTranslations || CurrentCulture == "ja-JP")
				return JPString;

			try
			{
				IEnumerable<XElement> TranslationList = GetTranslationList(Type);

				if (TranslationList == null)
				{
					AddTranslation(RawData, Type);
					return JPString;
				}

				string JPChildElement = "JP-Name";
				string TRChildElement = "TR-Name";

				if (Type == TranslationType.QuestDetail || Type == TranslationType.ExpeditionDetail)
				{
					JPChildElement = "JP-Detail";
					TRChildElement = "TR-Detail";
				}

				string translate = JPString;
				if (GetTranslation(JPString, TranslationList, JPChildElement, TRChildElement, ID, ref translate))
				{
					return translate;
				}
#if DEBUG
				Debug.WriteLine(string.Format("Can't find Translation: {0,-20} {1}", JPString, ID));
#endif
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

			AddTranslation(RawData, Type);

			return JPString;
		}

		public bool GetTranslation(string JPString, IEnumerable<XElement> TranslationList, String JPChildElement, String TRChildElement, int ID , ref string translate)
		{
			IEnumerable<XElement> FoundTranslation = TranslationList.Where(el =>
			{
				if (el.Element(JPChildElement).Value.Equals(JPString)) return true;
				if (el.Attribute("mode") != null)
				{
					if (el.Attribute("mode").Value.Equals("suffix")) {
						int sl = el.Element(JPChildElement).Value.Length;
						if (JPString.Length > sl)
						{
							if (el.Element(JPChildElement).Value.Equals(JPString.Substring(JPString.Length - sl))) return true;
						}
					}
				}
				return false;
			});

			bool FoundWrongId = false;
			int n;
			foreach (XElement el in FoundTranslation)
			{
				// #if DEBUG
				// 					if (ID >= 0 && el.Element("ID") != null && Convert.ToInt32(el.Element("ID").Value) == ID)
				// 						Debug.WriteLine(string.Format("Translation: {0,-20} {1,-20} {2}", JPString, el.Element(TRChildElement).Value, ID));
				// #endif

				if (el.Attribute("mode") != null && !el.Attribute("mode").Equals("normal"))
				{
					if (el.Attribute("mode").Value.Equals("suffix"))
					{
						string t = JPString.Substring(0, JPString.Length - el.Element(JPChildElement).Value.Length);
						if (GetTranslation(t, TranslationList, JPChildElement, TRChildElement, -1, ref t))
						{
							if ((el.Attribute("suffixType") != null) && el.Attribute("suffixType").Value.Equals("pre")) translate = el.Element(TRChildElement).Value + t;
							else translate = t + el.Element(TRChildElement).Value;
							return true;
						}
					}
					continue;
				}

				if (ID >= 0)
				{
					if (!Int32.TryParse(el.Element("ID").Value, out n))
					{
						FoundWrongId = true;
						translate = el.Element(TRChildElement).Value;
					}
					else
					{
						if (ID >= 0 && el.Element("ID") != null && Convert.ToInt32(el.Element("ID").Value) == ID)
						{
							translate = el.Element(TRChildElement).Value;
							return true;
						}
					}
				}				
				else
				{
					translate = el.Element(TRChildElement).Value;
					return true;
				}
			}

			if (FoundWrongId)
			{
				// #if DEBUG
				// 					Debug.WriteLine(string.Format("Wrong ID: {0,-20} {1,-20} {2}", JPString, translate, ID));
				// #endif
				return true;
			}
						
			return false;
		}

		public void AddTranslation(Object RawData, TranslationType Type)
		{
			if (RawData == null || !EnableAddUntranslated)
				return;
            
			try
			{
				switch (Type)
				{
					case TranslationType.Ships:
						if (ShipsXML == null)
						{
							ShipsXML = new XDocument();
							ShipsXML.Add(new XElement("Ships"));
							ShipsXML.Root.SetAttributeValue("Version", "0.0.0");
							ShipsVersion = "0.0.0";
						}

						kcsapi_mst_ship ShipData = RawData as kcsapi_mst_ship;

						if (ShipData == null)
							return;

						ShipsXML.Root.Add(new XElement("Ship",
							new XElement("JP-Name", ShipData.api_name),
							new XElement("TR-Name", ShipData.api_name)
						));

						ShipsXML.Save("Translations\\Ships.xml");
						break;

					case TranslationType.ShipTypes:
						if (ShipTypesXML == null)
						{
							ShipTypesXML = new XDocument();
							ShipTypesXML.Add(new XElement("ShipTypes"));
							ShipTypesXML.Root.SetAttributeValue("Version", "0.0.0");
							ShipTypesVersion = "0.0.0";
						}

						kcsapi_mst_stype TypeData = RawData as kcsapi_mst_stype;

						if (TypeData == null)
							return;

						ShipTypesXML.Root.Add(new XElement("Type",
							new XElement("ID", TypeData.api_id),
							new XElement("JP-Name", TypeData.api_name),
							new XElement("TR-Name", TypeData.api_name)
							));

						ShipTypesXML.Save("Translations\\ShipTypes.xml");
						break;

					case TranslationType.Equipment:
						if (EquipmentXML == null)
						{
							EquipmentXML = new XDocument();
							EquipmentXML.Add(new XElement("Equipment"));
							EquipmentXML.Root.SetAttributeValue("Version", "0.0.0");
							EquipmentVersion = "0.0.0";
						}

						kcsapi_mst_slotitem EqiupData = RawData as kcsapi_mst_slotitem;

						if (EqiupData == null)
							return;

						EquipmentXML.Root.Add(new XElement("Item",
							new XElement("JP-Name", EqiupData.api_name),
							new XElement("TR-Name", EqiupData.api_name)
							));

						EquipmentXML.Save("Translations\\Equipment.xml");
						break;

					case TranslationType.OperationMaps:
					case TranslationType.OperationSortie:
						if (OperationsXML == null)
						{
							OperationsXML = new XDocument();
							OperationsXML.Add(new XElement("Operations"));
							OperationsXML.Root.SetAttributeValue("Version", "0.0.0");
							OperationsVersion = "0.0.0";
						}

						kcsapi_battleresult OperationsData = RawData as kcsapi_battleresult;

						if (OperationsData == null)
							return;

						if (Type == TranslationType.OperationMaps)
						{
							OperationsXML.Root.Add(new XElement("Map",
								new XElement("JP-Name", OperationsData.api_quest_name),
								new XElement("TR-Name", OperationsData.api_quest_name)
								));
						}
						else
						{
							OperationsXML.Root.Add(new XElement("Sortie",
								new XElement("JP-Name", OperationsData.api_enemy_info.api_deck_name),
								new XElement("TR-Name", OperationsData.api_enemy_info.api_deck_name)
								));
						}

						OperationsXML.Save("Translations\\Operations.xml");
						break;

					case TranslationType.Quests:
					case TranslationType.QuestTitle:
					case TranslationType.QuestDetail:
						if (QuestsXML == null)
						{
							QuestsXML = new XDocument();
							QuestsXML.Add(new XElement("Quests"));
							QuestsXML.Root.SetAttributeValue("Version", "0.0.0");
							QuestsVersion = "0.0.0";
						}

						kcsapi_quest QuestData = RawData as kcsapi_quest;

						if (QuestData == null)
							return;

						IEnumerable<XElement> FoundTranslation = QuestsXML.Descendants("Quest").Where(b => b.Element("ID").Value.Equals(QuestData.api_no.ToString()));

						if (FoundTranslation != null && FoundTranslation.Any())
						{
							foreach (XElement el in FoundTranslation)
							{
								if (el.Element("JP-Name") == null) el.Add(new XElement("JP-Name", QuestData.api_title));
								else el.Element("JP-Name").Value = QuestData.api_title;
								if (el.Element("JP-Detail") == null) el.Add(new XElement("JP-Detail", QuestData.api_detail));
								else el.Element("JP-Detail").Value = QuestData.api_detail;
							}
						}
						else
						{
							int n;
							bool need_add = true;
							IEnumerable<XElement> FoundTranslationDetail = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Detail").Value.Equals(QuestData.api_detail));
							IEnumerable<XElement> FoundTranslationTitle = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Name").Value.Equals(QuestData.api_title));

							// Check the current list for any errors and fix them before writing a whole new element.
							if (Type == TranslationType.QuestTitle && FoundTranslationDetail != null && FoundTranslationDetail.Any())
							{
								// The title is wrong, but the detail is right. Fix the title.
								foreach (XElement el in FoundTranslationDetail)
								{
									if (!Int32.TryParse(el.Element("ID").Value, out n))
									{
										if (el.Element("JP-Name") == null) el.Add(new XElement("JP-Name", QuestData.api_title));
										else el.Element("JP-Name").Value = QuestData.api_title;
										need_add = false;
									}
								}

							}
							else if (Type == TranslationType.QuestDetail && FoundTranslationTitle != null && FoundTranslationTitle.Any())
							{
								// We found an existing detail, the title must be broken. Fix it.
								foreach (XElement el in FoundTranslationTitle)
								{
									if (!Int32.TryParse(el.Element("ID").Value, out n))
									{
										if (el.Element("JP-Detail") == null) el.Add(new XElement("JP-Detail", QuestData.api_detail));
										else el.Element("JP-Detail").Value = QuestData.api_detail;
										need_add = false;
									}
								}									
							}
							
							if (need_add)
							{
								// The quest doesn't exist at all. Add it.
								QuestsXML.Root.Add(new XElement("Quest",
									new XElement("ID", QuestData.api_no),
									new XElement("JP-Name", QuestData.api_title),
									new XElement("TR-Name", QuestData.api_title),
									new XElement("JP-Detail", QuestData.api_detail),
									new XElement("TR-Detail", QuestData.api_detail)
									));
							}
						}

						QuestsXML.Save("Translations\\Quests.xml");
						break;
                    case TranslationType.Expeditions:
                    case TranslationType.ExpeditionTitle:
                    case TranslationType.ExpeditionDetail:
                        if (ExpeditionsXML == null)
                        {
                            ExpeditionsXML = new XDocument();
                            ExpeditionsXML.Add(new XElement("Expeditions"));
                            ExpeditionsXML.Root.SetAttributeValue("Version", "0.0.0");
                            ExpeditionsVersion = "0.0.0";
                        }

                        kcsapi_mission ExpeditionData = RawData as kcsapi_mission;

                        if (ExpeditionData == null)
							return;

                        IEnumerable<XElement> FoundTranslationExpedition = ExpeditionsXML.Descendants("Expedition").Where(b => b.Element("ID").Value.Equals(ExpeditionData.api_id.ToString()));
                        
                        // Check the current list for any errors and fix them before writing a whole new element.
						if (FoundTranslationExpedition != null && FoundTranslationExpedition.Any())
                        {
							foreach (XElement el in FoundTranslationExpedition)
							{
								if (el.Element("JP-Name") == null) el.Add(new XElement("JP-Name", ExpeditionData.api_name));
								else el.Element("JP-Name").Value = ExpeditionData.api_name;
								if (el.Element("JP-Detail") == null) el.Add(new XElement("JP-Detail", ExpeditionData.api_details));
								else el.Element("JP-Detail").Value = ExpeditionData.api_details;
							}

                        }
                        else
                        {
                            // The quest doesn't exist at all. Add it.
                            ExpeditionsXML.Root.Add(new XElement("Expedition",
                                new XElement("ID", ExpeditionData.api_id),
                                new XElement("JP-Name", ExpeditionData.api_name),
                                new XElement("TR-Name", ExpeditionData.api_name),
                                new XElement("JP-Detail", ExpeditionData.api_details),
                                new XElement("TR-Detail", ExpeditionData.api_details)
                                ));
                        }

                        ExpeditionsXML.Save("Translations\\Expeditions.xml");
                        break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

	}
}
