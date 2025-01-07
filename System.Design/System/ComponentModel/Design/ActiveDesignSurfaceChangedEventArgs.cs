using System;

namespace System.ComponentModel.Design
{
	public class ActiveDesignSurfaceChangedEventArgs : EventArgs
	{
		public ActiveDesignSurfaceChangedEventArgs(DesignSurface oldSurface, DesignSurface newSurface)
		{
			this._oldSurface = oldSurface;
			this._newSurface = newSurface;
		}

		public DesignSurface OldSurface
		{
			get
			{
				return this._oldSurface;
			}
		}

		public DesignSurface NewSurface
		{
			get
			{
				return this._newSurface;
			}
		}

		private DesignSurface _oldSurface;

		private DesignSurface _newSurface;
	}
}
