using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020003D1 RID: 977
	[Serializable]
	public class HttpListenerException : Win32Exception
	{
		// Token: 0x06001ED9 RID: 7897 RVA: 0x000777B0 File Offset: 0x000767B0
		public HttpListenerException()
			: base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000777BD File Offset: 0x000767BD
		public HttpListenerException(int errorCode)
			: base(errorCode)
		{
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000777C6 File Offset: 0x000767C6
		public HttpListenerException(int errorCode, string message)
			: base(errorCode, message)
		{
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000777D0 File Offset: 0x000767D0
		protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x000777DA File Offset: 0x000767DA
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
