using System;

namespace System.Configuration
{
	// Token: 0x0200005C RID: 92
	internal class DefinitionUpdate : Update
	{
		// Token: 0x06000396 RID: 918 RVA: 0x000128CA File Offset: 0x000118CA
		internal DefinitionUpdate(string configKey, bool moved, string updatedXml, SectionRecord sectionRecord)
			: base(configKey, moved, updatedXml)
		{
			this._sectionRecord = sectionRecord;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000397 RID: 919 RVA: 0x000128DD File Offset: 0x000118DD
		internal SectionRecord SectionRecord
		{
			get
			{
				return this._sectionRecord;
			}
		}

		// Token: 0x040002E6 RID: 742
		private SectionRecord _sectionRecord;
	}
}
