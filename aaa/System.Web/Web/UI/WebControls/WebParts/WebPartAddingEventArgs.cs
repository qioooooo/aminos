using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000706 RID: 1798
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartAddingEventArgs : WebPartCancelEventArgs
	{
		// Token: 0x060057AB RID: 22443 RVA: 0x00161425 File Offset: 0x00160425
		public WebPartAddingEventArgs(WebPart webPart, WebPartZoneBase zone, int zoneIndex)
			: base(webPart)
		{
			this._zone = zone;
			this._zoneIndex = zoneIndex;
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x0016143C File Offset: 0x0016043C
		// (set) Token: 0x060057AD RID: 22445 RVA: 0x00161444 File Offset: 0x00160444
		public WebPartZoneBase Zone
		{
			get
			{
				return this._zone;
			}
			set
			{
				this._zone = value;
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x0016144D File Offset: 0x0016044D
		// (set) Token: 0x060057AF RID: 22447 RVA: 0x00161455 File Offset: 0x00160455
		public int ZoneIndex
		{
			get
			{
				return this._zoneIndex;
			}
			set
			{
				this._zoneIndex = value;
			}
		}

		// Token: 0x04002FAE RID: 12206
		private WebPartZoneBase _zone;

		// Token: 0x04002FAF RID: 12207
		private int _zoneIndex;
	}
}
