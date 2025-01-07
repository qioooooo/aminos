using System;
using System.Drawing;

namespace System.Web.UI.Design
{
	public sealed class DesignerRegionMouseEventArgs : EventArgs
	{
		public DesignerRegionMouseEventArgs(DesignerRegion region, Point location)
		{
			this._location = location;
			this._region = region;
		}

		public Point Location
		{
			get
			{
				return this._location;
			}
		}

		public DesignerRegion Region
		{
			get
			{
				return this._region;
			}
		}

		private Point _location;

		private DesignerRegion _region;
	}
}
