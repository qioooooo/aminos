using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200045C RID: 1116
	[ComVisible(true)]
	public class KeyPressEventArgs : EventArgs
	{
		// Token: 0x060041CD RID: 16845 RVA: 0x000EB309 File Offset: 0x000EA309
		public KeyPressEventArgs(char keyChar)
		{
			this.keyChar = keyChar;
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x000EB318 File Offset: 0x000EA318
		// (set) Token: 0x060041CF RID: 16847 RVA: 0x000EB320 File Offset: 0x000EA320
		public char KeyChar
		{
			get
			{
				return this.keyChar;
			}
			set
			{
				this.keyChar = value;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x000EB329 File Offset: 0x000EA329
		// (set) Token: 0x060041D1 RID: 16849 RVA: 0x000EB331 File Offset: 0x000EA331
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

		// Token: 0x04001FB1 RID: 8113
		private char keyChar;

		// Token: 0x04001FB2 RID: 8114
		private bool handled;
	}
}
