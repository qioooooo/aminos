using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005AD RID: 1453
	internal sealed class PinnedBufferMemoryStream : UnmanagedMemoryStream
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x000B919C File Offset: 0x000B819C
		internal unsafe PinnedBufferMemoryStream(byte[] array)
		{
			int num = array.Length;
			if (num == 0)
			{
				array = new byte[1];
				num = 0;
			}
			this._array = array;
			this._pinningHandle = new GCHandle(array, GCHandleType.Pinned);
			fixed (byte* array2 = this._array)
			{
				base.Initialize(array2, (long)num, (long)num, FileAccess.Read, true);
			}
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000B9204 File Offset: 0x000B8204
		~PinnedBufferMemoryStream()
		{
			this.Dispose(false);
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000B9234 File Offset: 0x000B8234
		protected override void Dispose(bool disposing)
		{
			if (this._isOpen)
			{
				this._pinningHandle.Free();
				this._isOpen = false;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001C56 RID: 7254
		private byte[] _array;

		// Token: 0x04001C57 RID: 7255
		private GCHandle _pinningHandle;
	}
}
