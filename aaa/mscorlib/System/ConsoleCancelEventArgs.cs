using System;

namespace System
{
	// Token: 0x02000090 RID: 144
	[Serializable]
	public sealed class ConsoleCancelEventArgs : EventArgs
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x0001A23B File Offset: 0x0001923B
		internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
		{
			this._type = type;
			this._cancel = false;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001A251 File Offset: 0x00019251
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0001A259 File Offset: 0x00019259
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				if (this._type == ConsoleSpecialKey.ControlBreak && value)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CantCancelCtrlBreak"));
				}
				this._cancel = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001A27E File Offset: 0x0001927E
		public ConsoleSpecialKey SpecialKey
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x040002B5 RID: 693
		private ConsoleSpecialKey _type;

		// Token: 0x040002B6 RID: 694
		private bool _cancel;
	}
}
