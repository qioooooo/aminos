using System;
using System.Drawing;

namespace System.Web.UI.Design
{
	// Token: 0x02000360 RID: 864
	public sealed class DesignerRegionMouseEventArgs : EventArgs
	{
		// Token: 0x0600208A RID: 8330 RVA: 0x000B6B91 File Offset: 0x000B5B91
		public DesignerRegionMouseEventArgs(DesignerRegion region, Point location)
		{
			this._location = location;
			this._region = region;
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600208B RID: 8331 RVA: 0x000B6BA7 File Offset: 0x000B5BA7
		public Point Location
		{
			get
			{
				return this._location;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000B6BAF File Offset: 0x000B5BAF
		public DesignerRegion Region
		{
			get
			{
				return this._region;
			}
		}

		// Token: 0x040017DE RID: 6110
		private Point _location;

		// Token: 0x040017DF RID: 6111
		private DesignerRegion _region;
	}
}
