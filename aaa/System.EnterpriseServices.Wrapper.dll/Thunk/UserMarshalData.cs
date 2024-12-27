using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	internal class UserMarshalData
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x000027A8 File Offset: 0x00001BA8
		public static UserMarshalData Get(IntPtr pinned)
		{
			return (UserMarshalData)((GCHandle)pinned).Target;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000027C8 File Offset: 0x00001BC8
		public UserMarshalData(IntPtr pUnk)
		{
			this.pUnk = pUnk;
			this.buffer = null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000027EC File Offset: 0x00001BEC
		public IntPtr Pin()
		{
			return (IntPtr)GCHandle.Alloc(this, GCHandleType.Normal);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00002808 File Offset: 0x00001C08
		public void Unpin(IntPtr pinned)
		{
			((GCHandle)pinned).Free();
		}

		// Token: 0x040000AB RID: 171
		public IntPtr pUnk;

		// Token: 0x040000AC RID: 172
		public byte[] buffer;
	}
}
