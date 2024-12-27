using System;

namespace System.Net
{
	// Token: 0x02000545 RID: 1349
	[Flags]
	internal enum Alg
	{
		// Token: 0x040027FE RID: 10238
		Any = 0,
		// Token: 0x040027FF RID: 10239
		ClassSignture = 8192,
		// Token: 0x04002800 RID: 10240
		ClassEncrypt = 24576,
		// Token: 0x04002801 RID: 10241
		ClassHash = 32768,
		// Token: 0x04002802 RID: 10242
		ClassKeyXch = 40960,
		// Token: 0x04002803 RID: 10243
		TypeRSA = 1024,
		// Token: 0x04002804 RID: 10244
		TypeBlock = 1536,
		// Token: 0x04002805 RID: 10245
		TypeStream = 2048,
		// Token: 0x04002806 RID: 10246
		TypeDH = 2560,
		// Token: 0x04002807 RID: 10247
		NameDES = 1,
		// Token: 0x04002808 RID: 10248
		NameRC2 = 2,
		// Token: 0x04002809 RID: 10249
		Name3DES = 3,
		// Token: 0x0400280A RID: 10250
		NameAES_128 = 14,
		// Token: 0x0400280B RID: 10251
		NameAES_192 = 15,
		// Token: 0x0400280C RID: 10252
		NameAES_256 = 16,
		// Token: 0x0400280D RID: 10253
		NameAES = 17,
		// Token: 0x0400280E RID: 10254
		NameRC4 = 1,
		// Token: 0x0400280F RID: 10255
		NameMD5 = 3,
		// Token: 0x04002810 RID: 10256
		NameSHA = 4,
		// Token: 0x04002811 RID: 10257
		NameDH_Ephem = 2
	}
}
