using System;

namespace System.IO.Ports
{
	// Token: 0x020007AF RID: 1967
	public class SerialPinChangedEventArgs : EventArgs
	{
		// Token: 0x06003C3D RID: 15421 RVA: 0x00101519 File Offset: 0x00100519
		internal SerialPinChangedEventArgs(SerialPinChange eventCode)
		{
			this.pinChanged = eventCode;
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x00101528 File Offset: 0x00100528
		public SerialPinChange EventType
		{
			get
			{
				return this.pinChanged;
			}
		}

		// Token: 0x04003526 RID: 13606
		private SerialPinChange pinChanged;
	}
}
