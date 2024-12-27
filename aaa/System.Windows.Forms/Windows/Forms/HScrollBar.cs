using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000427 RID: 1063
	[SRDescription("DescriptionHScrollBar")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class HScrollBar : ScrollBar
	{
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06003F47 RID: 16199 RVA: 0x000E5C14 File Offset: 0x000E4C14
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				CreateParams createParams2 = createParams;
				createParams2.Style = createParams2.Style;
				return createParams;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x000E5C35 File Offset: 0x000E4C35
		protected override Size DefaultSize
		{
			get
			{
				return new Size(80, SystemInformation.HorizontalScrollBarHeight);
			}
		}
	}
}
