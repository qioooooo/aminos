using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000618 RID: 1560
	[Serializable]
	public class NetworkInformationException : Win32Exception
	{
		// Token: 0x0600300E RID: 12302 RVA: 0x000CFA34 File Offset: 0x000CEA34
		public NetworkInformationException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000CFA41 File Offset: 0x000CEA41
		public NetworkInformationException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000CFA4A File Offset: 0x000CEA4A
		internal NetworkInformationException(SocketError socketError)
			: base((int)socketError)
		{
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000CFA53 File Offset: 0x000CEA53
		protected NetworkInformationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000CFA5D File Offset: 0x000CEA5D
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
