using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000057 RID: 87
	internal class UserCallData
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00002708 File Offset: 0x00001B08
		public static UserCallData Get(IntPtr pinned)
		{
			return (UserCallData)((GCHandle)pinned).Target;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002728 File Offset: 0x00001B28
		public UserCallData(object otp, IMessage msg, IntPtr ctx, [MarshalAs(UnmanagedType.U1)] bool fIsAutoDone, MemberInfo mb)
		{
			this.otp = otp;
			this.msg = msg;
			this.pDestCtx = ctx.ToInt32();
			this.fIsAutoDone = fIsAutoDone;
			this.mb = mb;
			this.except = null;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002770 File Offset: 0x00001B70
		public IntPtr Pin()
		{
			return (IntPtr)GCHandle.Alloc(this, GCHandleType.Normal);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000278C File Offset: 0x00001B8C
		public void Unpin(IntPtr pinned)
		{
			((GCHandle)pinned).Free();
		}

		// Token: 0x040000A5 RID: 165
		public object otp;

		// Token: 0x040000A6 RID: 166
		public object except;

		// Token: 0x040000A7 RID: 167
		public MemberInfo mb;

		// Token: 0x040000A8 RID: 168
		public IMessage msg;

		// Token: 0x040000A9 RID: 169
		public unsafe IUnknown* pDestCtx;

		// Token: 0x040000AA RID: 170
		public bool fIsAutoDone;
	}
}
