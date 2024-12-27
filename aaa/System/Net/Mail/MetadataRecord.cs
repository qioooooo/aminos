using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x0200068F RID: 1679
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct MetadataRecord
	{
		// Token: 0x04002FDC RID: 12252
		internal uint Identifier;

		// Token: 0x04002FDD RID: 12253
		internal uint Attributes;

		// Token: 0x04002FDE RID: 12254
		internal uint UserType;

		// Token: 0x04002FDF RID: 12255
		internal uint DataType;

		// Token: 0x04002FE0 RID: 12256
		internal uint DataLen;

		// Token: 0x04002FE1 RID: 12257
		internal IntPtr DataBuf;

		// Token: 0x04002FE2 RID: 12258
		internal uint DataTag;
	}
}
