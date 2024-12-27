using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200014C RID: 332
	internal sealed class OverlappedData : CriticalFinalizerObject
	{
		// Token: 0x06001263 RID: 4707 RVA: 0x00033AE4 File Offset: 0x00032AE4
		internal OverlappedData(OverlappedDataCacheLine cacheLine)
		{
			this.m_cacheLine = cacheLine;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00033AF4 File Offset: 0x00032AF4
		~OverlappedData()
		{
			if (this.m_cacheLine != null && !this.m_cacheLine.Removed && !Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				OverlappedDataCache.CacheOverlappedData(this);
				GC.ReRegisterForFinalize(this);
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00033B50 File Offset: 0x00032B50
		internal void ReInitialize()
		{
			this.m_asyncResult = null;
			this.m_iocb = null;
			this.m_iocbHelper = null;
			this.m_overlapped = null;
			this.m_userObject = null;
			this.m_pinSelf = (IntPtr)0;
			this.m_userObjectInternal = (IntPtr)0;
			this.m_AppDomainId = 0;
			this.m_nativeOverlapped.EventHandle = (IntPtr)0;
			this.m_isArray = 0;
			this.m_nativeOverlapped.InternalHigh = (IntPtr)0;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00033BC8 File Offset: 0x00032BC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal unsafe NativeOverlapped* Pack(IOCompletionCallback iocb, object userData)
		{
			if (!this.m_pinSelf.IsNull())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_Overlapped_Pack"));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (iocb != null)
			{
				this.m_iocbHelper = new _IOCompletionCallback(iocb, ref stackCrawlMark);
				this.m_iocb = iocb;
			}
			else
			{
				this.m_iocbHelper = null;
				this.m_iocb = null;
			}
			this.m_userObject = userData;
			if (this.m_userObject != null)
			{
				if (this.m_userObject.GetType() == typeof(object[]))
				{
					this.m_isArray = 1;
				}
				else
				{
					this.m_isArray = 0;
				}
			}
			return this.AllocateNativeOverlapped();
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00033C58 File Offset: 0x00032C58
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		internal unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb, object userData)
		{
			if (!this.m_pinSelf.IsNull())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_Overlapped_Pack"));
			}
			this.m_userObject = userData;
			if (this.m_userObject != null)
			{
				if (this.m_userObject.GetType() == typeof(object[]))
				{
					this.m_isArray = 1;
				}
				else
				{
					this.m_isArray = 0;
				}
			}
			this.m_iocb = iocb;
			this.m_iocbHelper = null;
			return this.AllocateNativeOverlapped();
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00033CCC File Offset: 0x00032CCC
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00033CD9 File Offset: 0x00032CD9
		[ComVisible(false)]
		internal IntPtr UserHandle
		{
			get
			{
				return this.m_nativeOverlapped.EventHandle;
			}
			set
			{
				this.m_nativeOverlapped.EventHandle = value;
			}
		}

		// Token: 0x0600126A RID: 4714
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern NativeOverlapped* AllocateNativeOverlapped();

		// Token: 0x0600126B RID: 4715
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void FreeNativeOverlapped(NativeOverlapped* nativeOverlappedPtr);

		// Token: 0x0600126C RID: 4716
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern OverlappedData GetOverlappedFromNative(NativeOverlapped* nativeOverlappedPtr);

		// Token: 0x0600126D RID: 4717
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void CheckVMForIOPacket(out NativeOverlapped* pOVERLAP, out uint errorCode, out uint numBytes);

		// Token: 0x04000622 RID: 1570
		internal IAsyncResult m_asyncResult;

		// Token: 0x04000623 RID: 1571
		internal IOCompletionCallback m_iocb;

		// Token: 0x04000624 RID: 1572
		internal _IOCompletionCallback m_iocbHelper;

		// Token: 0x04000625 RID: 1573
		internal Overlapped m_overlapped;

		// Token: 0x04000626 RID: 1574
		private object m_userObject;

		// Token: 0x04000627 RID: 1575
		internal OverlappedDataCacheLine m_cacheLine;

		// Token: 0x04000628 RID: 1576
		private IntPtr m_pinSelf;

		// Token: 0x04000629 RID: 1577
		private IntPtr m_userObjectInternal;

		// Token: 0x0400062A RID: 1578
		private int m_AppDomainId;

		// Token: 0x0400062B RID: 1579
		internal short m_slot;

		// Token: 0x0400062C RID: 1580
		private byte m_isArray;

		// Token: 0x0400062D RID: 1581
		private byte m_toBeCleaned;

		// Token: 0x0400062E RID: 1582
		internal NativeOverlapped m_nativeOverlapped;
	}
}
