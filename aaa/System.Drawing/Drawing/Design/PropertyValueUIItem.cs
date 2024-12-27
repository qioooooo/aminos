using System;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x020000FA RID: 250
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertyValueUIItem
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002817C File Offset: 0x0002717C
		public PropertyValueUIItem(Image uiItemImage, PropertyValueUIItemInvokeHandler handler, string tooltip)
		{
			this.itemImage = uiItemImage;
			this.handler = handler;
			if (this.itemImage == null)
			{
				throw new ArgumentNullException("uiItemImage");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.tooltip = tooltip;
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000281BA File Offset: 0x000271BA
		public virtual Image Image
		{
			get
			{
				return this.itemImage;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x000281C2 File Offset: 0x000271C2
		public virtual PropertyValueUIItemInvokeHandler InvokeHandler
		{
			get
			{
				return this.handler;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x000281CA File Offset: 0x000271CA
		public virtual string ToolTip
		{
			get
			{
				return this.tooltip;
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000281D2 File Offset: 0x000271D2
		public virtual void Reset()
		{
		}

		// Token: 0x04000B6A RID: 2922
		private Image itemImage;

		// Token: 0x04000B6B RID: 2923
		private PropertyValueUIItemInvokeHandler handler;

		// Token: 0x04000B6C RID: 2924
		private string tooltip;
	}
}
