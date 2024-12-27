using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020003A9 RID: 937
	[Serializable]
	public abstract class EndPoint
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x0007010B File Offset: 0x0006F10B
		public virtual AddressFamily AddressFamily
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00070112 File Offset: 0x0006F112
		public virtual SocketAddress Serialize()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00070119 File Offset: 0x0006F119
		public virtual EndPoint Create(SocketAddress socketAddress)
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}
	}
}
