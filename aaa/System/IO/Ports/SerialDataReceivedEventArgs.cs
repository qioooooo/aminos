using System;

namespace System.IO.Ports
{
	// Token: 0x020007B3 RID: 1971
	public class SerialDataReceivedEventArgs : EventArgs
	{
		// Token: 0x06003C95 RID: 15509 RVA: 0x00102F71 File Offset: 0x00101F71
		internal SerialDataReceivedEventArgs(SerialData eventCode)
		{
			this.receiveType = eventCode;
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x00102F80 File Offset: 0x00101F80
		public SerialData EventType
		{
			get
			{
				return this.receiveType;
			}
		}

		// Token: 0x0400355B RID: 13659
		internal SerialData receiveType;
	}
}
