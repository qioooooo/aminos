using System;

namespace System.Web.Configuration
{
	// Token: 0x02000208 RID: 520
	internal sealed class ImpersonateTokenRef : IDisposable
	{
		// Token: 0x06001C27 RID: 7207 RVA: 0x00081011 File Offset: 0x00080011
		internal ImpersonateTokenRef(IntPtr token)
		{
			this._handle = token;
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00081020 File Offset: 0x00080020
		internal IntPtr Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00081028 File Offset: 0x00080028
		~ImpersonateTokenRef()
		{
			if (this._handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.CloseHandle(this._handle);
				this._handle = IntPtr.Zero;
			}
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00081078 File Offset: 0x00080078
		void IDisposable.Dispose()
		{
			if (this._handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.CloseHandle(this._handle);
				this._handle = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040018BA RID: 6330
		private IntPtr _handle;
	}
}
