using System;
using System.Runtime.InteropServices;

namespace System.Web.Services.Interop
{
	// Token: 0x02000020 RID: 32
	[StructLayout(LayoutKind.Sequential)]
	internal class UserThread
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00002E29 File Offset: 0x00001E29
		internal UserThread()
		{
			this.pSidBuffer = 0;
			this.dwSidLen = 0;
			this.dwTid = 0;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002E48 File Offset: 0x00001E48
		public override bool Equals(object obj)
		{
			if (!(obj is UserThread))
			{
				return false;
			}
			UserThread userThread = (UserThread)obj;
			return userThread.dwTid == this.dwTid && userThread.pSidBuffer == this.pSidBuffer && userThread.dwSidLen == this.dwSidLen;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002E93 File Offset: 0x00001E93
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000234 RID: 564
		internal int pSidBuffer;

		// Token: 0x04000235 RID: 565
		internal int dwSidLen;

		// Token: 0x04000236 RID: 566
		internal int dwTid;
	}
}
