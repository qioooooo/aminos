using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FA RID: 1274
	[ComVisible(true)]
	public struct HandleRef
	{
		// Token: 0x0600318A RID: 12682 RVA: 0x000A9E45 File Offset: 0x000A8E45
		public HandleRef(object wrapper, IntPtr handle)
		{
			this.m_wrapper = wrapper;
			this.m_handle = handle;
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x000A9E55 File Offset: 0x000A8E55
		public object Wrapper
		{
			get
			{
				return this.m_wrapper;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600318C RID: 12684 RVA: 0x000A9E5D File Offset: 0x000A8E5D
		public IntPtr Handle
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000A9E65 File Offset: 0x000A8E65
		public static explicit operator IntPtr(HandleRef value)
		{
			return value.m_handle;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000A9E6E File Offset: 0x000A8E6E
		public static IntPtr ToIntPtr(HandleRef value)
		{
			return value.m_handle;
		}

		// Token: 0x0400197B RID: 6523
		internal object m_wrapper;

		// Token: 0x0400197C RID: 6524
		internal IntPtr m_handle;
	}
}
