using System;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000506 RID: 1286
	[ComVisible(true)]
	[Serializable]
	public sealed class BStrWrapper
	{
		// Token: 0x06003285 RID: 12933 RVA: 0x000AB6F4 File Offset: 0x000AA6F4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BStrWrapper(string value)
		{
			this.m_WrappedObject = value;
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x000AB703 File Offset: 0x000AA703
		public string WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019A4 RID: 6564
		private string m_WrappedObject;
	}
}
