using System;

namespace System.Windows.Forms
{
	// Token: 0x02000333 RID: 819
	public class HandledMouseEventArgs : MouseEventArgs
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x000B98AD File Offset: 0x000B88AD
		public HandledMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
			: this(button, clicks, x, y, delta, false)
		{
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000B98BD File Offset: 0x000B88BD
		public HandledMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta, bool defaultHandledValue)
			: base(button, clicks, x, y, delta)
		{
			this.handled = defaultHandledValue;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x000B98D4 File Offset: 0x000B88D4
		// (set) Token: 0x06003478 RID: 13432 RVA: 0x000B98DC File Offset: 0x000B88DC
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

		// Token: 0x04001B2A RID: 6954
		private bool handled;
	}
}
