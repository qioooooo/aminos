using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000B8 RID: 184
	[Guid("ef24f689-14f8-4d92-b4af-d7b1f0e70fd4")]
	public class AppDomainHelper : IAppDomainHelper
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x0000D940 File Offset: 0x0000C940
		void IAppDomainHelper.Initialize(IntPtr pUnkAD, IntPtr pfnShutdownCB, IntPtr punkPool)
		{
			this._ad = (AppDomain)Marshal.GetObjectForIUnknown(pUnkAD);
			this._pfnShutdownCB = pfnShutdownCB;
			this._punkPool = punkPool;
			Marshal.AddRef(this._punkPool);
			this._ad.DomainUnload += this.OnDomainUnload;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000D990 File Offset: 0x0000C990
		void IAppDomainHelper.DoCallback(IntPtr pUnkAD, IntPtr pfnCallbackCB, IntPtr data)
		{
			AppDomainHelper.CallbackWrapper callbackWrapper = new AppDomainHelper.CallbackWrapper(pfnCallbackCB, data);
			if (this._ad != AppDomain.CurrentDomain)
			{
				this._ad.DoCallBack(new CrossAppDomainDelegate(callbackWrapper.ReceiveCallback));
				return;
			}
			callbackWrapper.ReceiveCallback();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000D9D0 File Offset: 0x0000C9D0
		private void OnDomainUnload(object sender, EventArgs e)
		{
			if (this._pfnShutdownCB != IntPtr.Zero)
			{
				Proxy.CallFunction(this._pfnShutdownCB, this._punkPool);
				this._pfnShutdownCB = IntPtr.Zero;
				Marshal.Release(this._punkPool);
				this._punkPool = IntPtr.Zero;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000DA24 File Offset: 0x0000CA24
		~AppDomainHelper()
		{
			if (this._punkPool != IntPtr.Zero)
			{
				Marshal.Release(this._punkPool);
				this._punkPool = IntPtr.Zero;
			}
		}

		// Token: 0x040001F0 RID: 496
		private AppDomain _ad;

		// Token: 0x040001F1 RID: 497
		private IntPtr _pfnShutdownCB;

		// Token: 0x040001F2 RID: 498
		private IntPtr _punkPool;

		// Token: 0x020000B9 RID: 185
		private class CallbackWrapper
		{
			// Token: 0x0600045F RID: 1119 RVA: 0x0000DA74 File Offset: 0x0000CA74
			public CallbackWrapper(IntPtr pfnCB, IntPtr pv)
			{
				this._pfnCB = pfnCB;
				this._pv = pv;
			}

			// Token: 0x06000460 RID: 1120 RVA: 0x0000DA8C File Offset: 0x0000CA8C
			public void ReceiveCallback()
			{
				int num = Proxy.CallFunction(this._pfnCB, this._pv);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}

			// Token: 0x040001F3 RID: 499
			private IntPtr _pfnCB;

			// Token: 0x040001F4 RID: 500
			private IntPtr _pv;
		}
	}
}
