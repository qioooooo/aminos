using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200045A RID: 1114
	[ComVisible(true)]
	public class KeyEventArgs : EventArgs
	{
		// Token: 0x060041BD RID: 16829 RVA: 0x000EB239 File Offset: 0x000EA239
		public KeyEventArgs(Keys keyData)
		{
			this.keyData = keyData;
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x000EB248 File Offset: 0x000EA248
		public virtual bool Alt
		{
			get
			{
				return (this.keyData & Keys.Alt) == Keys.Alt;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x000EB25D File Offset: 0x000EA25D
		public bool Control
		{
			get
			{
				return (this.keyData & Keys.Control) == Keys.Control;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x000EB272 File Offset: 0x000EA272
		// (set) Token: 0x060041C1 RID: 16833 RVA: 0x000EB27A File Offset: 0x000EA27A
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

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x000EB284 File Offset: 0x000EA284
		public Keys KeyCode
		{
			get
			{
				Keys keys = this.keyData & Keys.KeyCode;
				if (!Enum.IsDefined(typeof(Keys), (int)keys))
				{
					return Keys.None;
				}
				return keys;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x000EB2B8 File Offset: 0x000EA2B8
		public int KeyValue
		{
			get
			{
				return (int)(this.keyData & Keys.KeyCode);
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x000EB2C6 File Offset: 0x000EA2C6
		public Keys KeyData
		{
			get
			{
				return this.keyData;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x000EB2CE File Offset: 0x000EA2CE
		public Keys Modifiers
		{
			get
			{
				return this.keyData & Keys.Modifiers;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x000EB2DC File Offset: 0x000EA2DC
		public virtual bool Shift
		{
			get
			{
				return (this.keyData & Keys.Shift) == Keys.Shift;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x000EB2F1 File Offset: 0x000EA2F1
		// (set) Token: 0x060041C8 RID: 16840 RVA: 0x000EB2F9 File Offset: 0x000EA2F9
		public bool SuppressKeyPress
		{
			get
			{
				return this.suppressKeyPress;
			}
			set
			{
				this.suppressKeyPress = value;
				this.handled = value;
			}
		}

		// Token: 0x04001FAE RID: 8110
		private readonly Keys keyData;

		// Token: 0x04001FAF RID: 8111
		private bool handled;

		// Token: 0x04001FB0 RID: 8112
		private bool suppressKeyPress;
	}
}
