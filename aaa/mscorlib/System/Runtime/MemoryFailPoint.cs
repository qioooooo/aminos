using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Runtime
{
	// Token: 0x020005F8 RID: 1528
	public sealed class MemoryFailPoint : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x060037C1 RID: 14273 RVA: 0x000BBAA9 File Offset: 0x000BAAA9
		static MemoryFailPoint()
		{
			MemoryFailPoint.GetMemorySettings(out MemoryFailPoint.GCSegmentSize, out MemoryFailPoint.TopOfMemory);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000BBABC File Offset: 0x000BAABC
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public unsafe MemoryFailPoint(int sizeInMegabytes)
		{
			if (sizeInMegabytes <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeInMegabytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			ulong num = (ulong)((ulong)((long)sizeInMegabytes) << 20);
			this._reservedMemory = num;
			ulong num2 = (ulong)(Math.Ceiling((double)(num / MemoryFailPoint.GCSegmentSize)) * MemoryFailPoint.GCSegmentSize);
			if (num2 >= MemoryFailPoint.TopOfMemory)
			{
				throw new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_TooBig"));
			}
			ulong num3 = 0UL;
			ulong num4 = 0UL;
			int i = 0;
			while (i < 3)
			{
				MemoryFailPoint.CheckForAvailableMemory(out num3, out num4);
				ulong memoryFailPointReservedMemory = SharedStatics.MemoryFailPointReservedMemory;
				ulong num5 = num2 + memoryFailPointReservedMemory;
				bool flag = num5 < num2 || num5 < memoryFailPointReservedMemory;
				bool flag2 = num3 < num5 + 16777216UL || flag;
				bool flag3 = num4 < num5 || flag;
				long num6 = (long)Environment.TickCount;
				if (num6 > MemoryFailPoint.LastTimeCheckingAddressSpace + 10000L || num6 < MemoryFailPoint.LastTimeCheckingAddressSpace || MemoryFailPoint.LastKnownFreeAddressSpace < (long)num2)
				{
					MemoryFailPoint.CheckForFreeAddressSpace(num2, false);
				}
				bool flag4 = MemoryFailPoint.LastKnownFreeAddressSpace < (long)num2;
				if (!flag2 && !flag3 && !flag4)
				{
					break;
				}
				switch (i)
				{
				case 0:
					GC.Collect();
					break;
				case 1:
					if (flag2)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							break;
						}
						finally
						{
							UIntPtr uintPtr = new UIntPtr(num2);
							void* ptr = Win32Native.VirtualAlloc(null, uintPtr, 4096, 4);
							if (ptr != null && !Win32Native.VirtualFree(ptr, UIntPtr.Zero, 32768))
							{
								__Error.WinIOError();
							}
						}
						goto IL_0166;
					}
					break;
				case 2:
					goto IL_0166;
				}
				IL_019A:
				i++;
				continue;
				IL_0166:
				if (flag2 || flag3)
				{
					InsufficientMemoryException ex = new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint"));
					throw ex;
				}
				if (flag4)
				{
					InsufficientMemoryException ex2 = new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_VAFrag"));
					throw ex2;
				}
				goto IL_019A;
			}
			Interlocked.Add(ref MemoryFailPoint.LastKnownFreeAddressSpace, (long)(-(long)num));
			if (MemoryFailPoint.LastKnownFreeAddressSpace < 0L)
			{
				MemoryFailPoint.CheckForFreeAddressSpace(num2, true);
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SharedStatics.AddMemoryFailPointReservation((long)num);
				this._mustSubtractReservation = true;
			}
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000BBCC4 File Offset: 0x000BACC4
		private static void CheckForAvailableMemory(out ulong availPageFile, out ulong totalAddressSpaceFree)
		{
			if (Environment.IsWin9X())
			{
				Win32Native.MEMORYSTATUS memorystatus = new Win32Native.MEMORYSTATUS();
				if (!Win32Native.GlobalMemoryStatus(memorystatus))
				{
					__Error.WinIOError();
				}
				availPageFile = (ulong)memorystatus.availPageFile;
				totalAddressSpaceFree = (ulong)memorystatus.availVirtual;
				return;
			}
			Win32Native.MEMORYSTATUSEX memorystatusex = new Win32Native.MEMORYSTATUSEX();
			if (!Win32Native.GlobalMemoryStatusEx(memorystatusex))
			{
				__Error.WinIOError();
			}
			availPageFile = memorystatusex.availPageFile;
			totalAddressSpaceFree = memorystatusex.availVirtual;
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000BBD28 File Offset: 0x000BAD28
		private static bool CheckForFreeAddressSpace(ulong size, bool shouldThrow)
		{
			ulong num = MemoryFailPoint.MemFreeAfterAddress(null, size);
			MemoryFailPoint.LastKnownFreeAddressSpace = (long)num;
			MemoryFailPoint.LastTimeCheckingAddressSpace = (long)Environment.TickCount;
			if (num < size && shouldThrow)
			{
				throw new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_VAFrag"));
			}
			return num >= size;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000BBD70 File Offset: 0x000BAD70
		private unsafe static ulong MemFreeAfterAddress(void* address, ulong size)
		{
			if (size >= MemoryFailPoint.TopOfMemory)
			{
				return 0UL;
			}
			ulong num = 0UL;
			Win32Native.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = default(Win32Native.MEMORY_BASIC_INFORMATION);
			IntPtr intPtr = (IntPtr)Marshal.SizeOf(memory_BASIC_INFORMATION);
			while ((byte*)address + size < MemoryFailPoint.TopOfMemory)
			{
				IntPtr intPtr2 = Win32Native.VirtualQuery(address, ref memory_BASIC_INFORMATION, intPtr);
				if (intPtr2 == IntPtr.Zero)
				{
					__Error.WinIOError();
				}
				ulong num2 = memory_BASIC_INFORMATION.RegionSize.ToUInt64();
				if (memory_BASIC_INFORMATION.State == 65536U)
				{
					if (num2 >= size)
					{
						return num2;
					}
					num = Math.Max(num, num2);
				}
				address = (void*)((byte*)address + num2);
			}
			return num;
		}

		// Token: 0x060037C6 RID: 14278
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMemorySettings(out uint maxGCSegmentSize, out ulong topOfMemory);

		// Token: 0x060037C7 RID: 14279 RVA: 0x000BBE04 File Offset: 0x000BAE04
		~MemoryFailPoint()
		{
			this.Dispose(false);
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000BBE34 File Offset: 0x000BAE34
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000BBE44 File Offset: 0x000BAE44
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void Dispose(bool disposing)
		{
			if (this._mustSubtractReservation)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					SharedStatics.AddMemoryFailPointReservation((long)(-(long)this._reservedMemory));
					this._mustSubtractReservation = false;
				}
			}
		}

		// Token: 0x04001CBA RID: 7354
		private const int CheckThreshold = 10000;

		// Token: 0x04001CBB RID: 7355
		private const int LowMemoryFudgeFactor = 16777216;

		// Token: 0x04001CBC RID: 7356
		private static readonly ulong TopOfMemory;

		// Token: 0x04001CBD RID: 7357
		private static long LastKnownFreeAddressSpace;

		// Token: 0x04001CBE RID: 7358
		private static long LastTimeCheckingAddressSpace;

		// Token: 0x04001CBF RID: 7359
		private static readonly uint GCSegmentSize;

		// Token: 0x04001CC0 RID: 7360
		private ulong _reservedMemory;

		// Token: 0x04001CC1 RID: 7361
		private bool _mustSubtractReservation;
	}
}
