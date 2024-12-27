using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200014D RID: 333
	[ComVisible(true)]
	public class Overlapped
	{
		// Token: 0x0600126E RID: 4718 RVA: 0x00033CE7 File Offset: 0x00032CE7
		public Overlapped()
		{
			this.m_overlappedData = OverlappedDataCache.GetOverlappedData(this);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00033CFC File Offset: 0x00032CFC
		public Overlapped(int offsetLo, int offsetHi, IntPtr hEvent, IAsyncResult ar)
		{
			this.m_overlappedData = OverlappedDataCache.GetOverlappedData(this);
			this.m_overlappedData.m_nativeOverlapped.OffsetLow = offsetLo;
			this.m_overlappedData.m_nativeOverlapped.OffsetHigh = offsetHi;
			this.m_overlappedData.UserHandle = hEvent;
			this.m_overlappedData.m_asyncResult = ar;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00033D56 File Offset: 0x00032D56
		[Obsolete("This constructor is not 64-bit compatible.  Use the constructor that takes an IntPtr for the event handle.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public Overlapped(int offsetLo, int offsetHi, int hEvent, IAsyncResult ar)
			: this(offsetLo, offsetHi, new IntPtr(hEvent), ar)
		{
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x00033D68 File Offset: 0x00032D68
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x00033D75 File Offset: 0x00032D75
		public IAsyncResult AsyncResult
		{
			get
			{
				return this.m_overlappedData.m_asyncResult;
			}
			set
			{
				this.m_overlappedData.m_asyncResult = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x00033D83 File Offset: 0x00032D83
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x00033D95 File Offset: 0x00032D95
		public int OffsetLow
		{
			get
			{
				return this.m_overlappedData.m_nativeOverlapped.OffsetLow;
			}
			set
			{
				this.m_overlappedData.m_nativeOverlapped.OffsetLow = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00033DA8 File Offset: 0x00032DA8
		// (set) Token: 0x06001276 RID: 4726 RVA: 0x00033DBA File Offset: 0x00032DBA
		public int OffsetHigh
		{
			get
			{
				return this.m_overlappedData.m_nativeOverlapped.OffsetHigh;
			}
			set
			{
				this.m_overlappedData.m_nativeOverlapped.OffsetHigh = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00033DD0 File Offset: 0x00032DD0
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x00033DF0 File Offset: 0x00032DF0
		[Obsolete("This property is not 64-bit compatible.  Use EventHandleIntPtr instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public int EventHandle
		{
			get
			{
				return this.m_overlappedData.UserHandle.ToInt32();
			}
			set
			{
				this.m_overlappedData.UserHandle = new IntPtr(value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00033E03 File Offset: 0x00032E03
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x00033E10 File Offset: 0x00032E10
		[ComVisible(false)]
		public IntPtr EventHandleIntPtr
		{
			get
			{
				return this.m_overlappedData.UserHandle;
			}
			set
			{
				this.m_overlappedData.UserHandle = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00033E1E File Offset: 0x00032E1E
		internal _IOCompletionCallback iocbHelper
		{
			get
			{
				return this.m_overlappedData.m_iocbHelper;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00033E2B File Offset: 0x00032E2B
		internal IOCompletionCallback UserCallback
		{
			get
			{
				return this.m_overlappedData.m_iocb;
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00033E38 File Offset: 0x00032E38
		[CLSCompliant(false)]
		[Obsolete("This method is not safe.  Use Pack (iocb, userData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb)
		{
			return this.Pack(iocb, null);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00033E42 File Offset: 0x00032E42
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb, object userData)
		{
			return this.m_overlappedData.Pack(iocb, userData);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00033E51 File Offset: 0x00032E51
		[CLSCompliant(false)]
		[Obsolete("This method is not safe.  Use UnsafePack (iocb, userData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb)
		{
			return this.UnsafePack(iocb, null);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00033E5B File Offset: 0x00032E5B
		[CLSCompliant(false)]
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlEvidence = true, ControlPolicy = true)]
		public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb, object userData)
		{
			return this.m_overlappedData.UnsafePack(iocb, userData);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00033E6C File Offset: 0x00032E6C
		[CLSCompliant(false)]
		public unsafe static Overlapped Unpack(NativeOverlapped* nativeOverlappedPtr)
		{
			if (nativeOverlappedPtr == null)
			{
				throw new ArgumentNullException("nativeOverlappedPtr");
			}
			return OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr).m_overlapped;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00033E98 File Offset: 0x00032E98
		[CLSCompliant(false)]
		public unsafe static void Free(NativeOverlapped* nativeOverlappedPtr)
		{
			if (nativeOverlappedPtr == null)
			{
				throw new ArgumentNullException("nativeOverlappedPtr");
			}
			Overlapped overlapped = OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr).m_overlapped;
			OverlappedData.FreeNativeOverlapped(nativeOverlappedPtr);
			OverlappedData overlappedData = overlapped.m_overlappedData;
			overlapped.m_overlappedData = null;
			OverlappedDataCache.CacheOverlappedData(overlappedData);
		}

		// Token: 0x0400062F RID: 1583
		private OverlappedData m_overlappedData;
	}
}
