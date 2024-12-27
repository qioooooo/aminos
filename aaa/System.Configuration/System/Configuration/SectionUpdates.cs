using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000098 RID: 152
	internal class SectionUpdates
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0001C12A File Offset: 0x0001B12A
		internal SectionUpdates(string name)
		{
			this._name = name;
			this._groups = new Hashtable();
			this._sections = new Hashtable();
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0001C14F File Offset: 0x0001B14F
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x0001C157 File Offset: 0x0001B157
		internal bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				this._isNew = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0001C160 File Offset: 0x0001B160
		internal bool IsEmpty
		{
			get
			{
				return this._groups.Count == 0 && this._sections.Count == 0;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001C180 File Offset: 0x0001B180
		private SectionUpdates FindSectionUpdates(string configKey, bool isGroup)
		{
			string text;
			if (isGroup)
			{
				text = configKey;
			}
			else
			{
				string text2;
				BaseConfigurationRecord.SplitConfigKey(configKey, out text, out text2);
			}
			SectionUpdates sectionUpdates = this;
			if (text.Length != 0)
			{
				string[] array = text.Split(BaseConfigurationRecord.ConfigPathSeparatorParams);
				foreach (string text3 in array)
				{
					SectionUpdates sectionUpdates2 = (SectionUpdates)sectionUpdates._groups[text3];
					if (sectionUpdates2 == null)
					{
						sectionUpdates2 = new SectionUpdates(text3);
						sectionUpdates._groups[text3] = sectionUpdates2;
					}
					sectionUpdates = sectionUpdates2;
				}
			}
			return sectionUpdates;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001C208 File Offset: 0x0001B208
		internal void CompleteUpdates()
		{
			bool flag = true;
			foreach (object obj in this._groups.Values)
			{
				SectionUpdates sectionUpdates = (SectionUpdates)obj;
				sectionUpdates.CompleteUpdates();
				if (!sectionUpdates.IsNew)
				{
					flag = false;
				}
			}
			this._isNew = flag && this._cMoved == this._sections.Count;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001C290 File Offset: 0x0001B290
		internal void AddSection(Update update)
		{
			SectionUpdates sectionUpdates = this.FindSectionUpdates(update.ConfigKey, false);
			sectionUpdates._sections.Add(update.ConfigKey, update);
			sectionUpdates._cUnretrieved++;
			if (update.Moved)
			{
				sectionUpdates._cMoved++;
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001C2E4 File Offset: 0x0001B2E4
		internal void AddSectionGroup(Update update)
		{
			SectionUpdates sectionUpdates = this.FindSectionUpdates(update.ConfigKey, true);
			sectionUpdates._sectionGroupUpdate = update;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001C308 File Offset: 0x0001B308
		private Update GetUpdate(string configKey)
		{
			Update update = (Update)this._sections[configKey];
			if (update != null)
			{
				if (update.Retrieved)
				{
					update = null;
				}
				else
				{
					update.Retrieved = true;
					this._cUnretrieved--;
					if (update.Moved)
					{
						this._cMoved--;
					}
				}
			}
			return update;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001C362 File Offset: 0x0001B362
		internal DeclarationUpdate GetSectionGroupUpdate()
		{
			if (this._sectionGroupUpdate != null && !this._sectionGroupUpdate.Retrieved)
			{
				this._sectionGroupUpdate.Retrieved = true;
				return (DeclarationUpdate)this._sectionGroupUpdate;
			}
			return null;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001C392 File Offset: 0x0001B392
		internal DefinitionUpdate GetDefinitionUpdate(string configKey)
		{
			return (DefinitionUpdate)this.GetUpdate(configKey);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001C3A0 File Offset: 0x0001B3A0
		internal DeclarationUpdate GetDeclarationUpdate(string configKey)
		{
			return (DeclarationUpdate)this.GetUpdate(configKey);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001C3AE File Offset: 0x0001B3AE
		internal SectionUpdates GetSectionUpdatesForGroup(string group)
		{
			return (SectionUpdates)this._groups[group];
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001C3C4 File Offset: 0x0001B3C4
		internal bool HasUnretrievedSections()
		{
			if (this._cUnretrieved > 0 || (this._sectionGroupUpdate != null && !this._sectionGroupUpdate.Retrieved))
			{
				return true;
			}
			foreach (object obj in this._groups.Values)
			{
				SectionUpdates sectionUpdates = (SectionUpdates)obj;
				if (sectionUpdates.HasUnretrievedSections())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001C44C File Offset: 0x0001B44C
		internal bool HasNewSectionGroups()
		{
			foreach (object obj in this._groups.Values)
			{
				SectionUpdates sectionUpdates = (SectionUpdates)obj;
				if (sectionUpdates.IsNew)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001C4B4 File Offset: 0x0001B4B4
		internal string[] GetUnretrievedSectionNames()
		{
			if (this._cUnretrieved == 0)
			{
				return null;
			}
			string[] array = new string[this._cUnretrieved];
			int num = 0;
			foreach (object obj in this._sections.Values)
			{
				Update update = (Update)obj;
				if (!update.Retrieved)
				{
					array[num] = update.ConfigKey;
					num++;
				}
			}
			Array.Sort<string>(array);
			return array;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001C544 File Offset: 0x0001B544
		internal string[] GetMovedSectionNames()
		{
			if (this._cMoved == 0)
			{
				return null;
			}
			string[] array = new string[this._cMoved];
			int num = 0;
			foreach (object obj in this._sections.Values)
			{
				Update update = (Update)obj;
				if (update.Moved && !update.Retrieved)
				{
					array[num] = update.ConfigKey;
					num++;
				}
			}
			Array.Sort<string>(array);
			return array;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001C5DC File Offset: 0x0001B5DC
		internal string[] GetUnretrievedGroupNames()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this._groups)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				SectionUpdates sectionUpdates = (SectionUpdates)dictionaryEntry.Value;
				if (sectionUpdates.HasUnretrievedSections())
				{
					arrayList.Add(text);
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array);
			Array.Sort<string>(array);
			return array;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001C690 File Offset: 0x0001B690
		internal string[] GetNewGroupNames()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this._groups)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				SectionUpdates sectionUpdates = (SectionUpdates)dictionaryEntry.Value;
				if (sectionUpdates.IsNew && sectionUpdates.HasUnretrievedSections())
				{
					arrayList.Add(text);
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array);
			Array.Sort<string>(array);
			return array;
		}

		// Token: 0x040003C6 RID: 966
		private string _name;

		// Token: 0x040003C7 RID: 967
		private Hashtable _groups;

		// Token: 0x040003C8 RID: 968
		private Hashtable _sections;

		// Token: 0x040003C9 RID: 969
		private int _cUnretrieved;

		// Token: 0x040003CA RID: 970
		private int _cMoved;

		// Token: 0x040003CB RID: 971
		private Update _sectionGroupUpdate;

		// Token: 0x040003CC RID: 972
		private bool _isNew;
	}
}
