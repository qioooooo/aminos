using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x02000087 RID: 135
	internal class DtcTransactionManager
	{
		// Token: 0x0600036C RID: 876 RVA: 0x00033ED0 File Offset: 0x000332D0
		internal DtcTransactionManager(string nodeName, OletxTransactionManager oletxTm)
		{
			this.nodeName = nodeName;
			this.oletxTm = oletxTm;
			this.initialized = false;
			this.proxyShimFactory = OletxTransactionManager.proxyShimFactory;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00033F04 File Offset: 0x00033304
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			OletxInternalResourceManager internalResourceManager = this.oletxTm.internalResourceManager;
			IntPtr intPtr = IntPtr.Zero;
			IResourceManagerShim resourceManagerShim = null;
			bool flag = false;
			CoTaskMemHandle coTaskMemHandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				intPtr = HandleTable.AllocHandle(internalResourceManager);
				this.proxyShimFactory.ConnectToProxy(this.nodeName, internalResourceManager.Identifier, intPtr, out flag, out this.whereaboutsSize, out coTaskMemHandle, out resourceManagerShim);
				if (!flag)
				{
					throw new NotSupportedException(SR.GetString("ProxyCannotSupportMultipleNodeNames"));
				}
				if (coTaskMemHandle != null && this.whereaboutsSize != 0U)
				{
					this.whereabouts = new byte[this.whereaboutsSize];
					Marshal.Copy(coTaskMemHandle.DangerousGetHandle(), this.whereabouts, 0, Convert.ToInt32(this.whereaboutsSize));
				}
				internalResourceManager.resourceManagerShim = resourceManagerShim;
				internalResourceManager.CallReenlistComplete();
				this.initialized = true;
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_NOTSUPPORTED == ex.ErrorCode)
				{
					throw new NotSupportedException(SR.GetString("CannotSupportNodeNameSpecification"));
				}
				OletxTransactionManager.ProxyException(ex);
				throw TransactionManagerCommunicationException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionManagerCommunicationException"), ex);
			}
			finally
			{
				if (coTaskMemHandle != null)
				{
					coTaskMemHandle.Close();
				}
				if (!this.initialized)
				{
					if (intPtr != IntPtr.Zero && resourceManagerShim == null)
					{
						HandleTable.FreeHandle(intPtr);
					}
					if (this.whereabouts != null)
					{
						this.whereabouts = null;
						this.whereaboutsSize = 0U;
					}
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00034080 File Offset: 0x00033480
		internal IDtcProxyShimFactory ProxyShimFactory
		{
			get
			{
				if (!this.initialized)
				{
					lock (this)
					{
						this.Initialize();
					}
				}
				return this.proxyShimFactory;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000340D0 File Offset: 0x000334D0
		internal void ReleaseProxy()
		{
			lock (this)
			{
				this.whereabouts = null;
				this.whereaboutsSize = 0U;
				this.initialized = false;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00034120 File Offset: 0x00033520
		internal byte[] Whereabouts
		{
			get
			{
				if (!this.initialized)
				{
					lock (this)
					{
						this.Initialize();
					}
				}
				return this.whereabouts;
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00034170 File Offset: 0x00033570
		internal static uint AdjustTimeout(TimeSpan timeout)
		{
			uint num = 0U;
			try
			{
				num = Convert.ToUInt32(timeout.TotalMilliseconds, CultureInfo.CurrentCulture);
			}
			catch (OverflowException ex)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
				num = uint.MaxValue;
			}
			return num;
		}

		// Token: 0x040001C6 RID: 454
		private string nodeName;

		// Token: 0x040001C7 RID: 455
		private OletxTransactionManager oletxTm;

		// Token: 0x040001C8 RID: 456
		private IDtcProxyShimFactory proxyShimFactory;

		// Token: 0x040001C9 RID: 457
		private uint whereaboutsSize;

		// Token: 0x040001CA RID: 458
		private byte[] whereabouts;

		// Token: 0x040001CB RID: 459
		private bool initialized;
	}
}
