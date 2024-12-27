using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000104 RID: 260
	[ComVisible(true)]
	public struct RuntimeArgumentHandle
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x0002C4F3 File Offset: 0x0002B4F3
		internal IntPtr Value
		{
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x040004F2 RID: 1266
		private IntPtr m_ptr;
	}
}
