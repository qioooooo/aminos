using System;

namespace System.IO.Ports
{
	// Token: 0x020007AC RID: 1964
	public class SerialErrorReceivedEventArgs : EventArgs
	{
		// Token: 0x06003C37 RID: 15415 RVA: 0x00101502 File Offset: 0x00100502
		internal SerialErrorReceivedEventArgs(SerialError eventCode)
		{
			this.errorType = eventCode;
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x00101511 File Offset: 0x00100511
		public SerialError EventType
		{
			get
			{
				return this.errorType;
			}
		}

		// Token: 0x0400351F RID: 13599
		private SerialError errorType;
	}
}
