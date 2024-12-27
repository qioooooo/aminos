using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000057 RID: 87
	internal class ADValue
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00007AAC File Offset: 0x00006AAC
		public ADValue()
		{
			this.IsBinary = false;
			this.BinaryVal = null;
		}

		// Token: 0x04000195 RID: 405
		public bool IsBinary;

		// Token: 0x04000196 RID: 406
		public string StringVal;

		// Token: 0x04000197 RID: 407
		public byte[] BinaryVal;
	}
}
