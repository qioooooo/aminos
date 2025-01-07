using System;

namespace System.ComponentModel.Design
{
	public class DesignSurfaceEventArgs : EventArgs
	{
		public DesignSurfaceEventArgs(DesignSurface surface)
		{
			if (surface == null)
			{
				throw new ArgumentNullException("surface");
			}
			this._surface = surface;
		}

		public DesignSurface Surface
		{
			get
			{
				return this._surface;
			}
		}

		private DesignSurface _surface;
	}
}
