using System;

namespace System.Web.Util
{
	// Token: 0x02000773 RID: 1907
	internal class CaseInsensitiveStringSet : StringSet
	{
		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x06005C65 RID: 23653 RVA: 0x00172603 File Offset: 0x00171603
		protected override bool CaseInsensitive
		{
			get
			{
				return true;
			}
		}
	}
}
