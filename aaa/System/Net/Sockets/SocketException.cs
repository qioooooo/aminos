using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	// Token: 0x0200043F RID: 1087
	[Serializable]
	public class SocketException : Win32Exception
	{
		// Token: 0x06002218 RID: 8728 RVA: 0x00086AEF File Offset: 0x00085AEF
		public SocketException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00086AFC File Offset: 0x00085AFC
		internal SocketException(EndPoint endPoint)
			: base(Marshal.GetLastWin32Error())
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00086B10 File Offset: 0x00085B10
		public SocketException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00086B19 File Offset: 0x00085B19
		internal SocketException(int errorCode, EndPoint endPoint)
			: base(errorCode)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00086B29 File Offset: 0x00085B29
		internal SocketException(SocketError socketError)
			: base((int)socketError)
		{
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00086B32 File Offset: 0x00085B32
		protected SocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x00086B3C File Offset: 0x00085B3C
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x00086B44 File Offset: 0x00085B44
		public override string Message
		{
			get
			{
				if (this.m_EndPoint == null)
				{
					return base.Message;
				}
				return base.Message + " " + this.m_EndPoint.ToString();
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x00086B70 File Offset: 0x00085B70
		public SocketError SocketErrorCode
		{
			get
			{
				return (SocketError)base.NativeErrorCode;
			}
		}

		// Token: 0x0400220A RID: 8714
		[NonSerialized]
		private EndPoint m_EndPoint;
	}
}
