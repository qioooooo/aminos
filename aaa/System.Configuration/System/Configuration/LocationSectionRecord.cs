using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000076 RID: 118
	[DebuggerDisplay("LocationSectionRecord {ConfigKey}")]
	internal class LocationSectionRecord
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x000143CF File Offset: 0x000133CF
		internal LocationSectionRecord(SectionXmlInfo sectionXmlInfo, List<ConfigurationException> errors)
		{
			this._sectionXmlInfo = sectionXmlInfo;
			this._errors = errors;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000143E5 File Offset: 0x000133E5
		internal string ConfigKey
		{
			get
			{
				return this._sectionXmlInfo.ConfigKey;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000143F2 File Offset: 0x000133F2
		internal SectionXmlInfo SectionXmlInfo
		{
			get
			{
				return this._sectionXmlInfo;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x000143FA File Offset: 0x000133FA
		internal ICollection<ConfigurationException> Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00014402 File Offset: 0x00013402
		internal List<ConfigurationException> ErrorsList
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001440A File Offset: 0x0001340A
		internal bool HasErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errors);
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00014417 File Offset: 0x00013417
		internal void AddError(ConfigurationException e)
		{
			ErrorsHelper.AddError(ref this._errors, e);
		}

		// Token: 0x0400032F RID: 815
		private SectionXmlInfo _sectionXmlInfo;

		// Token: 0x04000330 RID: 816
		private List<ConfigurationException> _errors;
	}
}
