using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000703 RID: 1795
	[Serializable]
	public class SettingsAttributeDictionary : Hashtable
	{
		// Token: 0x0600373D RID: 14141 RVA: 0x000EAF29 File Offset: 0x000E9F29
		public SettingsAttributeDictionary()
		{
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000EAF31 File Offset: 0x000E9F31
		public SettingsAttributeDictionary(SettingsAttributeDictionary attributes)
			: base(attributes)
		{
		}
	}
}
