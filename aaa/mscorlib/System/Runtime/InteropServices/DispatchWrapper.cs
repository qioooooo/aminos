using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000509 RID: 1289
	[ComVisible(true)]
	[Serializable]
	public sealed class DispatchWrapper
	{
		// Token: 0x0600328A RID: 12938 RVA: 0x000AB754 File Offset: 0x000AA754
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public DispatchWrapper(object obj)
		{
			if (obj != null)
			{
				IntPtr idispatchForObject = Marshal.GetIDispatchForObject(obj);
				Marshal.Release(idispatchForObject);
			}
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x000AB77F File Offset: 0x000AA77F
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019AA RID: 6570
		private object m_WrappedObject;
	}
}
