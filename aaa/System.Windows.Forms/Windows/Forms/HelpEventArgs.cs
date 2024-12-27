using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000420 RID: 1056
	[ComVisible(true)]
	public class HelpEventArgs : EventArgs
	{
		// Token: 0x06003ECF RID: 16079 RVA: 0x000E4D2C File Offset: 0x000E3D2C
		public HelpEventArgs(Point mousePos)
		{
			this.mousePos = mousePos;
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x000E4D3B File Offset: 0x000E3D3B
		public Point MousePos
		{
			get
			{
				return this.mousePos;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x000E4D43 File Offset: 0x000E3D43
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x000E4D4B File Offset: 0x000E3D4B
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x04001EF6 RID: 7926
		private readonly Point mousePos;

		// Token: 0x04001EF7 RID: 7927
		private bool handled;
	}
}
