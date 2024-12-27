using System;

namespace System.DirectoryServices.Interop
{
	// Token: 0x0200004C RID: 76
	internal struct AdsSearchColumn
	{
		// Token: 0x0400021E RID: 542
		public IntPtr pszAttrName;

		// Token: 0x0400021F RID: 543
		public int dwADsType;

		// Token: 0x04000220 RID: 544
		public unsafe AdsValue* pADsValues;

		// Token: 0x04000221 RID: 545
		public int dwNumValues;

		// Token: 0x04000222 RID: 546
		public IntPtr hReserved;
	}
}
