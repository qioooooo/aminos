using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000550 RID: 1360
	public class ActiveDesignSurfaceChangedEventArgs : EventArgs
	{
		// Token: 0x06002FA3 RID: 12195 RVA: 0x0010FC4B File Offset: 0x0010EC4B
		public ActiveDesignSurfaceChangedEventArgs(DesignSurface oldSurface, DesignSurface newSurface)
		{
			this._oldSurface = oldSurface;
			this._newSurface = newSurface;
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x0010FC61 File Offset: 0x0010EC61
		public DesignSurface OldSurface
		{
			get
			{
				return this._oldSurface;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x0010FC69 File Offset: 0x0010EC69
		public DesignSurface NewSurface
		{
			get
			{
				return this._newSurface;
			}
		}

		// Token: 0x04002057 RID: 8279
		private DesignSurface _oldSurface;

		// Token: 0x04002058 RID: 8280
		private DesignSurface _newSurface;
	}
}
