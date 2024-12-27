using System;

namespace System.Windows.Forms
{
	// Token: 0x020005A1 RID: 1441
	internal sealed class NoneExcludedImageIndexConverter : ImageIndexConverter
	{
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x0010E54E File Offset: 0x0010D54E
		protected override bool IncludeNoneAsStandardValue
		{
			get
			{
				return false;
			}
		}
	}
}
