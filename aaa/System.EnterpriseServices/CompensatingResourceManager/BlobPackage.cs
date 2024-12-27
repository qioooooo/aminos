using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AB RID: 171
	internal class BlobPackage
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x0000CF67 File Offset: 0x0000BF67
		internal BlobPackage(_BLOB b)
		{
			this.Blob = b;
			this._bits = null;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000CF80 File Offset: 0x0000BF80
		internal byte[] GetBits()
		{
			if (this._bits != null)
			{
				return this._bits;
			}
			byte[] array = new byte[this.Blob.cbSize];
			Marshal.Copy(this.Blob.pBlobData, array, 0, this.Blob.cbSize);
			return array;
		}

		// Token: 0x040001DA RID: 474
		private byte[] _bits;

		// Token: 0x040001DB RID: 475
		internal _BLOB Blob;
	}
}
