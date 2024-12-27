using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000633 RID: 1587
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x000C2792 File Offset: 0x000C1792
		protected IsolatedStoragePermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000C27A4 File Offset: 0x000C17A4
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x000C279B File Offset: 0x000C179B
		public long UserQuota
		{
			get
			{
				return this.m_userQuota;
			}
			set
			{
				this.m_userQuota = value;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000C27B5 File Offset: 0x000C17B5
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000C27AC File Offset: 0x000C17AC
		public IsolatedStorageContainment UsageAllowed
		{
			get
			{
				return this.m_allowed;
			}
			set
			{
				this.m_allowed = value;
			}
		}

		// Token: 0x04001DCE RID: 7630
		internal long m_userQuota;

		// Token: 0x04001DCF RID: 7631
		internal IsolatedStorageContainment m_allowed;
	}
}
