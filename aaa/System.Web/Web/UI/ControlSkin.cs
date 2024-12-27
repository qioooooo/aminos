using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003C7 RID: 967
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ControlSkin
	{
		// Token: 0x06002F4A RID: 12106 RVA: 0x000D2D8B File Offset: 0x000D1D8B
		public ControlSkin(Type controlType, ControlSkinDelegate themeDelegate)
		{
			this._controlType = controlType;
			this._controlSkinDelegate = themeDelegate;
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002F4B RID: 12107 RVA: 0x000D2DA1 File Offset: 0x000D1DA1
		public Type ControlType
		{
			get
			{
				return this._controlType;
			}
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000D2DA9 File Offset: 0x000D1DA9
		public void ApplySkin(Control control)
		{
			this._controlSkinDelegate(control);
		}

		// Token: 0x040021CF RID: 8655
		private Type _controlType;

		// Token: 0x040021D0 RID: 8656
		private ControlSkinDelegate _controlSkinDelegate;
	}
}
