using System;

namespace System.Web.Configuration
{
	// Token: 0x02000217 RID: 535
	internal class NullRuntimeConfig : RuntimeConfig
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x0008345E File Offset: 0x0008245E
		internal NullRuntimeConfig()
			: base(null, true)
		{
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00083468 File Offset: 0x00082468
		protected override object GetSectionObject(string sectionName)
		{
			return null;
		}
	}
}
