using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050A RID: 1290
	[ComVisible(true)]
	[Serializable]
	public sealed class ErrorWrapper
	{
		// Token: 0x0600328C RID: 12940 RVA: 0x000AB787 File Offset: 0x000AA787
		public ErrorWrapper(int errorCode)
		{
			this.m_ErrorCode = errorCode;
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000AB796 File Offset: 0x000AA796
		public ErrorWrapper(object errorCode)
		{
			if (!(errorCode is int))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"), "errorCode");
			}
			this.m_ErrorCode = (int)errorCode;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000AB7C7 File Offset: 0x000AA7C7
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ErrorWrapper(Exception e)
		{
			this.m_ErrorCode = Marshal.GetHRForException(e);
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x000AB7DB File Offset: 0x000AA7DB
		public int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
		}

		// Token: 0x040019AB RID: 6571
		private int m_ErrorCode;
	}
}
