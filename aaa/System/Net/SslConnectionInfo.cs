using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000546 RID: 1350
	[StructLayout(LayoutKind.Sequential)]
	internal class SslConnectionInfo
	{
		// Token: 0x0600291B RID: 10523 RVA: 0x000AB890 File Offset: 0x000AA890
		internal unsafe SslConnectionInfo(byte[] nativeBuffer)
		{
			fixed (void* ptr = nativeBuffer)
			{
				IntPtr intPtr = new IntPtr(ptr);
				this.Protocol = Marshal.ReadInt32(intPtr);
				this.DataCipherAlg = Marshal.ReadInt32(intPtr, 4);
				this.DataKeySize = Marshal.ReadInt32(intPtr, 8);
				this.DataHashAlg = Marshal.ReadInt32(intPtr, 12);
				this.DataHashKeySize = Marshal.ReadInt32(intPtr, 16);
				this.KeyExchangeAlg = Marshal.ReadInt32(intPtr, 20);
				this.KeyExchKeySize = Marshal.ReadInt32(intPtr, 24);
			}
		}

		// Token: 0x04002812 RID: 10258
		public readonly int Protocol;

		// Token: 0x04002813 RID: 10259
		public readonly int DataCipherAlg;

		// Token: 0x04002814 RID: 10260
		public readonly int DataKeySize;

		// Token: 0x04002815 RID: 10261
		public readonly int DataHashAlg;

		// Token: 0x04002816 RID: 10262
		public readonly int DataHashKeySize;

		// Token: 0x04002817 RID: 10263
		public readonly int KeyExchangeAlg;

		// Token: 0x04002818 RID: 10264
		public readonly int KeyExchKeySize;
	}
}
