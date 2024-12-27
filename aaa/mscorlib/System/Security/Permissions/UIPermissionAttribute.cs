using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062D RID: 1581
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class UIPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600398E RID: 14734 RVA: 0x000C24DE File Offset: 0x000C14DE
		public UIPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x000C24E7 File Offset: 0x000C14E7
		// (set) Token: 0x06003990 RID: 14736 RVA: 0x000C24EF File Offset: 0x000C14EF
		public UIPermissionWindow Window
		{
			get
			{
				return this.m_windowFlag;
			}
			set
			{
				this.m_windowFlag = value;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003991 RID: 14737 RVA: 0x000C24F8 File Offset: 0x000C14F8
		// (set) Token: 0x06003992 RID: 14738 RVA: 0x000C2500 File Offset: 0x000C1500
		public UIPermissionClipboard Clipboard
		{
			get
			{
				return this.m_clipboardFlag;
			}
			set
			{
				this.m_clipboardFlag = value;
			}
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x000C2509 File Offset: 0x000C1509
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new UIPermission(PermissionState.Unrestricted);
			}
			return new UIPermission(this.m_windowFlag, this.m_clipboardFlag);
		}

		// Token: 0x04001DC3 RID: 7619
		private UIPermissionWindow m_windowFlag;

		// Token: 0x04001DC4 RID: 7620
		private UIPermissionClipboard m_clipboardFlag;
	}
}
