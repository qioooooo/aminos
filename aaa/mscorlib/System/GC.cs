using System;
using System.Globalization;
using System.Reflection.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x020000BC RID: 188
	public static class GC
	{
		// Token: 0x06000AB6 RID: 2742
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetGCLatencyMode();

		// Token: 0x06000AB7 RID: 2743
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nativeSetGCLatencyMode(int newLatencyMode);

		// Token: 0x06000AB8 RID: 2744
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGenerationWR(IntPtr handle);

		// Token: 0x06000AB9 RID: 2745
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long nativeGetTotalMemory();

		// Token: 0x06000ABA RID: 2746
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nativeCollectGeneration(int generation, int mode);

		// Token: 0x06000ABB RID: 2747
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeGetMaxGeneration();

		// Token: 0x06000ABC RID: 2748
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeCollectionCount(int generation);

		// Token: 0x06000ABD RID: 2749
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeIsServerGC();

		// Token: 0x06000ABE RID: 2750
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nativeAddMemoryPressure(ulong bytesAllocated);

		// Token: 0x06000ABF RID: 2751
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nativeRemoveMemoryPressure(ulong bytesAllocated);

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00020E34 File Offset: 0x0001FE34
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void AddMemoryPressure(long bytesAllocated)
		{
			if (bytesAllocated <= 0L)
			{
				throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (4 == IntPtr.Size && bytesAllocated > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("pressure", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegInt32"));
			}
			GC.nativeAddMemoryPressure((ulong)bytesAllocated);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00020E88 File Offset: 0x0001FE88
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void RemoveMemoryPressure(long bytesAllocated)
		{
			if (bytesAllocated <= 0L)
			{
				throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (4 == IntPtr.Size && bytesAllocated > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegInt32"));
			}
			GC.nativeRemoveMemoryPressure((ulong)bytesAllocated);
		}

		// Token: 0x06000AC2 RID: 2754
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetGeneration(object obj);

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00020EDB File Offset: 0x0001FEDB
		public static void Collect(int generation)
		{
			GC.Collect(generation, GCCollectionMode.Default);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00020EE4 File Offset: 0x0001FEE4
		public static void Collect()
		{
			GC.nativeCollectGeneration(-1, 0);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00020EED File Offset: 0x0001FEED
		public static void Collect(int generation, GCCollectionMode mode)
		{
			if (generation < 0)
			{
				throw new ArgumentOutOfRangeException("generation", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (mode < GCCollectionMode.Default || mode > GCCollectionMode.Optimized)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			GC.nativeCollectGeneration(generation, (int)mode);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00020F27 File Offset: 0x0001FF27
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int CollectionCount(int generation)
		{
			if (generation < 0)
			{
				throw new ArgumentOutOfRangeException("generation", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			return GC.nativeCollectionCount(generation);
		}

		// Token: 0x06000AC7 RID: 2759
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void KeepAlive(object obj);

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00020F48 File Offset: 0x0001FF48
		public static int GetGeneration(WeakReference wo)
		{
			int generationWR = GC.GetGenerationWR(wo.m_handle);
			GC.KeepAlive(wo);
			return generationWR;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00020F6A File Offset: 0x0001FF6A
		public static int MaxGeneration
		{
			get
			{
				return GC.nativeGetMaxGeneration();
			}
		}

		// Token: 0x06000ACA RID: 2762
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WaitForPendingFinalizers();

		// Token: 0x06000ACB RID: 2763
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nativeSuppressFinalize(object o);

		// Token: 0x06000ACC RID: 2764 RVA: 0x00020F71 File Offset: 0x0001FF71
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void SuppressFinalize(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			GC.nativeSuppressFinalize(obj);
		}

		// Token: 0x06000ACD RID: 2765
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nativeReRegisterForFinalize(object o);

		// Token: 0x06000ACE RID: 2766 RVA: 0x00020F87 File Offset: 0x0001FF87
		public static void ReRegisterForFinalize(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			GC.nativeReRegisterForFinalize(obj);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00020FA0 File Offset: 0x0001FFA0
		public static long GetTotalMemory(bool forceFullCollection)
		{
			long num = GC.nativeGetTotalMemory();
			if (!forceFullCollection)
			{
				return num;
			}
			int num2 = 20;
			long num3 = num;
			float num4;
			do
			{
				GC.WaitForPendingFinalizers();
				GC.Collect();
				num = num3;
				num3 = GC.nativeGetTotalMemory();
				num4 = (float)(num3 - num) / (float)num;
			}
			while (num2-- > 0 && (-0.05 >= (double)num4 || (double)num4 >= 0.05));
			return num3;
		}

		// Token: 0x06000AD0 RID: 2768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeRegisterForFullGCNotification(int maxGenerationPercentage, int largeObjectHeapPercentage);

		// Token: 0x06000AD1 RID: 2769
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeCancelFullGCNotification();

		// Token: 0x06000AD2 RID: 2770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeWaitForFullGCApproach(int millisecondsTimeout);

		// Token: 0x06000AD3 RID: 2771
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeWaitForFullGCComplete(int millisecondsTimeout);

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00020FFC File Offset: 0x0001FFFC
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RegisterForFullGCNotification(int maxGenerationThreshold, int largeObjectHeapThreshold)
		{
			if (maxGenerationThreshold <= 0 || maxGenerationThreshold >= 100)
			{
				throw new ArgumentOutOfRangeException("maxGenerationThreshold", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { 1, 99 }));
			}
			if (largeObjectHeapThreshold <= 0 || largeObjectHeapThreshold >= 100)
			{
				throw new ArgumentOutOfRangeException("largeObjectHeapThreshold", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { 1, 99 }));
			}
			if (!GC.nativeRegisterForFullGCNotification(maxGenerationThreshold, largeObjectHeapThreshold))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotWithConcurrentGC"));
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000210A8 File Offset: 0x000200A8
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void CancelFullGCNotification()
		{
			if (!GC.nativeCancelFullGCNotification())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotWithConcurrentGC"));
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000210C1 File Offset: 0x000200C1
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static GCNotificationStatus WaitForFullGCApproach()
		{
			return (GCNotificationStatus)GC.nativeWaitForFullGCApproach(-1);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000210C9 File Offset: 0x000200C9
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static GCNotificationStatus WaitForFullGCApproach(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return (GCNotificationStatus)GC.nativeWaitForFullGCApproach(millisecondsTimeout);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000210EA File Offset: 0x000200EA
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static GCNotificationStatus WaitForFullGCComplete()
		{
			return (GCNotificationStatus)GC.nativeWaitForFullGCComplete(-1);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x000210F2 File Offset: 0x000200F2
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static GCNotificationStatus WaitForFullGCComplete(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return (GCNotificationStatus)GC.nativeWaitForFullGCComplete(millisecondsTimeout);
		}

		// Token: 0x06000ADA RID: 2778
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCleanupCache();

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000ADB RID: 2779 RVA: 0x00021114 File Offset: 0x00020114
		// (remove) Token: 0x06000ADC RID: 2780 RVA: 0x00021160 File Offset: 0x00020160
		internal static event ClearCacheHandler ClearCache
		{
			add
			{
				lock (GC.locker)
				{
					GC.m_cacheHandler = (ClearCacheHandler)Delegate.Combine(GC.m_cacheHandler, value);
					GC.SetCleanupCache();
				}
			}
			remove
			{
				lock (GC.locker)
				{
					GC.m_cacheHandler = (ClearCacheHandler)Delegate.Remove(GC.m_cacheHandler, value);
				}
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000211A8 File Offset: 0x000201A8
		internal static void FireCacheEvent()
		{
			ClearCacheHandler clearCacheHandler = Interlocked.Exchange<ClearCacheHandler>(ref GC.m_cacheHandler, null);
			if (clearCacheHandler != null)
			{
				clearCacheHandler(null, null);
			}
		}

		// Token: 0x040003E7 RID: 999
		private static ClearCacheHandler m_cacheHandler;

		// Token: 0x040003E8 RID: 1000
		private static readonly object locker = new object();
	}
}
