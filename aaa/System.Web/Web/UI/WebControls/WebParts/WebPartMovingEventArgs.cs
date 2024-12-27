using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200073C RID: 1852
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartMovingEventArgs : WebPartCancelEventArgs
	{
		// Token: 0x060059EA RID: 23018 RVA: 0x0016B489 File Offset: 0x0016A489
		public WebPartMovingEventArgs(WebPart webPart, WebPartZoneBase zone, int zoneIndex)
			: base(webPart)
		{
			this._zone = zone;
			this._zoneIndex = zoneIndex;
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x060059EB RID: 23019 RVA: 0x0016B4A0 File Offset: 0x0016A4A0
		// (set) Token: 0x060059EC RID: 23020 RVA: 0x0016B4A8 File Offset: 0x0016A4A8
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

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x060059ED RID: 23021 RVA: 0x0016B4B1 File Offset: 0x0016A4B1
		// (set) Token: 0x060059EE RID: 23022 RVA: 0x0016B4B9 File Offset: 0x0016A4B9
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

		// Token: 0x04003069 RID: 12393
		private WebPartZoneBase _zone;

		// Token: 0x0400306A RID: 12394
		private int _zoneIndex;
	}
}
