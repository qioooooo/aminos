using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000151 RID: 337
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class ReaderWriterLock : CriticalFinalizerObject
	{
		// Token: 0x06001290 RID: 4752 RVA: 0x0003420C File Offset: 0x0003320C
		public ReaderWriterLock()
		{
			this.PrivateInitialize();
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0003421C File Offset: 0x0003321C
		~ReaderWriterLock()
		{
			this.PrivateDestruct();
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00034248 File Offset: 0x00033248
		public bool IsReaderLockHeld
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.PrivateGetIsReaderLockHeld();
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00034250 File Offset: 0x00033250
		public bool IsWriterLockHeld
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.PrivateGetIsWriterLockHeld();
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x00034258 File Offset: 0x00033258
		public int WriterSeqNum
		{
			get
			{
				return this.PrivateGetWriterSeqNum();
			}
		}

		// Token: 0x06001295 RID: 4757
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AcquireReaderLockInternal(int millisecondsTimeout);

		// Token: 0x06001296 RID: 4758 RVA: 0x00034260 File Offset: 0x00033260
		public void AcquireReaderLock(int millisecondsTimeout)
		{
			this.AcquireReaderLockInternal(millisecondsTimeout);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003426C File Offset: 0x0003326C
		public void AcquireReaderLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			this.AcquireReaderLockInternal((int)num);
		}

		// Token: 0x06001298 RID: 4760
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AcquireWriterLockInternal(int millisecondsTimeout);

		// Token: 0x06001299 RID: 4761 RVA: 0x000342AD File Offset: 0x000332AD
		public void AcquireWriterLock(int millisecondsTimeout)
		{
			this.AcquireWriterLockInternal(millisecondsTimeout);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000342B8 File Offset: 0x000332B8
		public void AcquireWriterLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			this.AcquireWriterLockInternal((int)num);
		}

		// Token: 0x0600129B RID: 4763
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseReaderLockInternal();

		// Token: 0x0600129C RID: 4764 RVA: 0x000342F9 File Offset: 0x000332F9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void ReleaseReaderLock()
		{
			this.ReleaseReaderLockInternal();
		}

		// Token: 0x0600129D RID: 4765
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseWriterLockInternal();

		// Token: 0x0600129E RID: 4766 RVA: 0x00034301 File Offset: 0x00033301
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void ReleaseWriterLock()
		{
			this.ReleaseWriterLockInternal();
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0003430C File Offset: 0x0003330C
		public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
		{
			LockCookie lockCookie = default(LockCookie);
			this.FCallUpgradeToWriterLock(ref lockCookie, millisecondsTimeout);
			return lockCookie;
		}

		// Token: 0x060012A0 RID: 4768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FCallUpgradeToWriterLock(ref LockCookie result, int millisecondsTimeout);

		// Token: 0x060012A1 RID: 4769 RVA: 0x0003432C File Offset: 0x0003332C
		public LockCookie UpgradeToWriterLock(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return this.UpgradeToWriterLock((int)num);
		}

		// Token: 0x060012A2 RID: 4770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DowngradeFromWriterLockInternal(ref LockCookie lockCookie);

		// Token: 0x060012A3 RID: 4771 RVA: 0x0003436D File Offset: 0x0003336D
		public void DowngradeFromWriterLock(ref LockCookie lockCookie)
		{
			this.DowngradeFromWriterLockInternal(ref lockCookie);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00034378 File Offset: 0x00033378
		public LockCookie ReleaseLock()
		{
			LockCookie lockCookie = default(LockCookie);
			this.FCallReleaseLock(ref lockCookie);
			return lockCookie;
		}

		// Token: 0x060012A5 RID: 4773
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FCallReleaseLock(ref LockCookie result);

		// Token: 0x060012A6 RID: 4774
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RestoreLockInternal(ref LockCookie lockCookie);

		// Token: 0x060012A7 RID: 4775 RVA: 0x00034396 File Offset: 0x00033396
		public void RestoreLock(ref LockCookie lockCookie)
		{
			this.RestoreLockInternal(ref lockCookie);
		}

		// Token: 0x060012A8 RID: 4776
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool PrivateGetIsReaderLockHeld();

		// Token: 0x060012A9 RID: 4777
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool PrivateGetIsWriterLockHeld();

		// Token: 0x060012AA RID: 4778
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int PrivateGetWriterSeqNum();

		// Token: 0x060012AB RID: 4779
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool AnyWritersSince(int seqNum);

		// Token: 0x060012AC RID: 4780
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PrivateInitialize();

		// Token: 0x060012AD RID: 4781
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PrivateDestruct();

		// Token: 0x0400063D RID: 1597
		private IntPtr _hWriterEvent;

		// Token: 0x0400063E RID: 1598
		private IntPtr _hReaderEvent;

		// Token: 0x0400063F RID: 1599
		private IntPtr _hObjectHandle;

		// Token: 0x04000640 RID: 1600
		private int _dwState;

		// Token: 0x04000641 RID: 1601
		private int _dwULockID;

		// Token: 0x04000642 RID: 1602
		private int _dwLLockID;

		// Token: 0x04000643 RID: 1603
		private int _dwWriterID;

		// Token: 0x04000644 RID: 1604
		private int _dwWriterSeqNum;

		// Token: 0x04000645 RID: 1605
		private short _wWriterLevel;
	}
}
