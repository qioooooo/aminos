using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;

namespace System.EnterpriseServices
{
	// Token: 0x02000039 RID: 57
	internal class SCUnMarshaler
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00005052 File Offset: 0x00004052
		internal SCUnMarshaler(Type _servertype, byte[] _buffer)
		{
			this.buffer = _buffer;
			this.servertype = _servertype;
			this._rp = null;
			this._fUnMarshaled = false;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005078 File Offset: 0x00004078
		private RealProxy UnmarshalRemoteReference()
		{
			IntPtr intPtr = IntPtr.Zero;
			RealProxy realProxy = null;
			try
			{
				this._fUnMarshaled = true;
				if (this.buffer != null)
				{
					intPtr = Proxy.UnmarshalObject(this.buffer);
				}
				realProxy = new RemoteServicedComponentProxy(this.servertype, intPtr, false);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
				this.buffer = null;
			}
			return realProxy;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000050E8 File Offset: 0x000040E8
		internal RealProxy GetRealProxy()
		{
			if (this._rp == null && !this._fUnMarshaled)
			{
				this._rp = this.UnmarshalRemoteReference();
			}
			return this._rp;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000510C File Offset: 0x0000410C
		internal void Dispose()
		{
			if (!this._fUnMarshaled && this.buffer != null)
			{
				Proxy.ReleaseMarshaledObject(this.buffer);
			}
		}

		// Token: 0x0400007C RID: 124
		private byte[] buffer;

		// Token: 0x0400007D RID: 125
		private Type servertype;

		// Token: 0x0400007E RID: 126
		private RealProxy _rp;

		// Token: 0x0400007F RID: 127
		private bool _fUnMarshaled;
	}
}
