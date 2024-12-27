using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000020 RID: 32
	internal class ConfigDefinitionUpdates
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x0000C638 File Offset: 0x0000B638
		internal ConfigDefinitionUpdates()
		{
			this._locationUpdatesList = new ArrayList();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C64C File Offset: 0x0000B64C
		internal LocationUpdates FindLocationUpdates(OverrideModeSetting overrideMode, bool inheritInChildApps)
		{
			foreach (object obj in this._locationUpdatesList)
			{
				LocationUpdates locationUpdates = (LocationUpdates)obj;
				if (OverrideModeSetting.CanUseSameLocationTag(locationUpdates.OverrideMode, overrideMode) && locationUpdates.InheritInChildApps == inheritInChildApps)
				{
					return locationUpdates;
				}
			}
			return null;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000C6BC File Offset: 0x0000B6BC
		internal DefinitionUpdate AddUpdate(OverrideModeSetting overrideMode, bool inheritInChildApps, bool moved, string updatedXml, SectionRecord sectionRecord)
		{
			LocationUpdates locationUpdates = this.FindLocationUpdates(overrideMode, inheritInChildApps);
			if (locationUpdates == null)
			{
				locationUpdates = new LocationUpdates(overrideMode, inheritInChildApps);
				this._locationUpdatesList.Add(locationUpdates);
			}
			DefinitionUpdate definitionUpdate = new DefinitionUpdate(sectionRecord.ConfigKey, moved, updatedXml, sectionRecord);
			locationUpdates.SectionUpdates.AddSection(definitionUpdate);
			return definitionUpdate;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C70C File Offset: 0x0000B70C
		internal void CompleteUpdates()
		{
			foreach (object obj in this._locationUpdatesList)
			{
				LocationUpdates locationUpdates = (LocationUpdates)obj;
				locationUpdates.CompleteUpdates();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C764 File Offset: 0x0000B764
		internal ArrayList LocationUpdatesList
		{
			get
			{
				return this._locationUpdatesList;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C76C File Offset: 0x0000B76C
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000C774 File Offset: 0x0000B774
		internal bool RequireLocation
		{
			get
			{
				return this._requireLocationWritten;
			}
			set
			{
				this._requireLocationWritten = value;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C77D File Offset: 0x0000B77D
		internal void FlagLocationWritten()
		{
			this._requireLocationWritten = false;
		}

		// Token: 0x04000210 RID: 528
		private ArrayList _locationUpdatesList;

		// Token: 0x04000211 RID: 529
		private bool _requireLocationWritten;
	}
}
