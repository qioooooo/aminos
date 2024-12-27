using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x02000749 RID: 1865
	internal class SwitchesDictionarySectionHandler : DictionarySectionHandler
	{
		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000F029C File Offset: 0x000EF29C
		protected override string KeyAttributeName
		{
			get
			{
				return "name";
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x060038DA RID: 14554 RVA: 0x000F02A3 File Offset: 0x000EF2A3
		internal override bool ValueRequired
		{
			get
			{
				return true;
			}
		}
	}
}
