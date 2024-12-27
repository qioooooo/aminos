using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000783 RID: 1923
	internal static class NtProcessInfoHelper
	{
		// Token: 0x06003B5A RID: 15194 RVA: 0x000FD418 File Offset: 0x000FC418
		private static int GetNewBufferSize(int existingBufferSize, int requiredSize)
		{
			if (requiredSize != 0)
			{
				return requiredSize + 10240;
			}
			int num = existingBufferSize * 2;
			if (num < existingBufferSize)
			{
				throw new OutOfMemoryException();
			}
			return num;
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x000FD440 File Offset: 0x000FC440
		public static ProcessInfo[] GetProcessInfos()
		{
			int num = 131072;
			int num2 = 0;
			GCHandle gchandle = default(GCHandle);
			ProcessInfo[] processInfos;
			try
			{
				int num3;
				do
				{
					byte[] array = new byte[num];
					gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
					num3 = NativeMethods.NtQuerySystemInformation(5, gchandle.AddrOfPinnedObject(), num, out num2);
					if (num3 == -1073741820)
					{
						if (gchandle.IsAllocated)
						{
							gchandle.Free();
						}
						num = NtProcessInfoHelper.GetNewBufferSize(num, num2);
					}
				}
				while (num3 == -1073741820);
				if (num3 < 0)
				{
					throw new InvalidOperationException(SR.GetString("CouldntGetProcessInfos"), new Win32Exception(num3));
				}
				processInfos = NtProcessInfoHelper.GetProcessInfos(gchandle.AddrOfPinnedObject());
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return processInfos;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000FD4F4 File Offset: 0x000FC4F4
		private unsafe static ProcessInfo[] GetProcessInfos(IntPtr dataPtr)
		{
			Hashtable hashtable = new Hashtable(60);
			long num = 0L;
			for (;;)
			{
				IntPtr intPtr = (IntPtr)((long)dataPtr + num);
				NtProcessInfoHelper.SystemProcessInformation systemProcessInformation = new NtProcessInfoHelper.SystemProcessInformation();
				Marshal.PtrToStructure(intPtr, systemProcessInformation);
				ProcessInfo processInfo = new ProcessInfo();
				processInfo.processId = systemProcessInformation.UniqueProcessId.ToInt32();
				processInfo.handleCount = (int)systemProcessInformation.HandleCount;
				processInfo.sessionId = (int)systemProcessInformation.SessionId;
				processInfo.poolPagedBytes = (long)systemProcessInformation.QuotaPagedPoolUsage;
				processInfo.poolNonpagedBytes = (long)systemProcessInformation.QuotaNonPagedPoolUsage;
				processInfo.virtualBytes = (long)systemProcessInformation.VirtualSize;
				processInfo.virtualBytesPeak = (long)systemProcessInformation.PeakVirtualSize;
				processInfo.workingSetPeak = (long)systemProcessInformation.PeakWorkingSetSize;
				processInfo.workingSet = (long)systemProcessInformation.WorkingSetSize;
				processInfo.pageFileBytesPeak = (long)systemProcessInformation.PeakPagefileUsage;
				processInfo.pageFileBytes = (long)systemProcessInformation.PagefileUsage;
				processInfo.privateBytes = (long)systemProcessInformation.PrivatePageCount;
				processInfo.basePriority = systemProcessInformation.BasePriority;
				if (systemProcessInformation.NamePtr == IntPtr.Zero)
				{
					if (processInfo.processId == NtProcessManager.SystemProcessID)
					{
						processInfo.processName = "System";
					}
					else if (processInfo.processId == 0)
					{
						processInfo.processName = "Idle";
					}
					else
					{
						processInfo.processName = processInfo.processId.ToString(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					string text = NtProcessInfoHelper.GetProcessShortName((char*)systemProcessInformation.NamePtr.ToPointer(), (int)(systemProcessInformation.NameLength / 2));
					if (ProcessManager.IsOSOlderThanXP && text.Length == 15)
					{
						if (text.EndsWith(".", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 14);
						}
						else if (text.EndsWith(".e", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 13);
						}
						else if (text.EndsWith(".ex", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 12);
						}
					}
					processInfo.processName = text;
				}
				hashtable[processInfo.processId] = processInfo;
				intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemProcessInformation));
				int num2 = 0;
				while ((long)num2 < (long)((ulong)systemProcessInformation.NumberOfThreads))
				{
					NtProcessInfoHelper.SystemThreadInformation systemThreadInformation = new NtProcessInfoHelper.SystemThreadInformation();
					Marshal.PtrToStructure(intPtr, systemThreadInformation);
					ThreadInfo threadInfo = new ThreadInfo();
					threadInfo.processId = (int)systemThreadInformation.UniqueProcess;
					threadInfo.threadId = (int)systemThreadInformation.UniqueThread;
					threadInfo.basePriority = systemThreadInformation.BasePriority;
					threadInfo.currentPriority = systemThreadInformation.Priority;
					threadInfo.startAddress = systemThreadInformation.StartAddress;
					threadInfo.threadState = (ThreadState)systemThreadInformation.ThreadState;
					threadInfo.threadWaitReason = NtProcessManager.GetThreadWaitReason((int)systemThreadInformation.WaitReason);
					processInfo.threadInfoList.Add(threadInfo);
					intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemThreadInformation));
					num2++;
				}
				if (systemProcessInformation.NextEntryOffset == 0)
				{
					break;
				}
				num += (long)systemProcessInformation.NextEntryOffset;
			}
			ProcessInfo[] array = new ProcessInfo[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x000FD824 File Offset: 0x000FC824
		internal unsafe static string GetProcessShortName(char* name, int length)
		{
			char* ptr = name;
			char* ptr2 = name;
			char* ptr3 = name;
			int num = 0;
			while (*ptr3 != '\0')
			{
				if (*ptr3 == '\\')
				{
					ptr = ptr3;
				}
				else if (*ptr3 == '.')
				{
					ptr2 = ptr3;
				}
				ptr3++;
				num++;
				if (num >= length)
				{
					break;
				}
			}
			if (ptr2 == name)
			{
				ptr2 = ptr3;
			}
			else
			{
				string text = new string(ptr2);
				if (!string.Equals(".exe", text, StringComparison.OrdinalIgnoreCase))
				{
					ptr2 = ptr3;
				}
			}
			if (*ptr == '\\')
			{
				ptr++;
			}
			int num2 = (int)((long)(ptr2 - ptr));
			return new string(ptr, 0, num2);
		}

		// Token: 0x02000784 RID: 1924
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemProcessInformation
		{
			// Token: 0x040033F1 RID: 13297
			internal int NextEntryOffset;

			// Token: 0x040033F2 RID: 13298
			internal uint NumberOfThreads;

			// Token: 0x040033F3 RID: 13299
			private long SpareLi1;

			// Token: 0x040033F4 RID: 13300
			private long SpareLi2;

			// Token: 0x040033F5 RID: 13301
			private long SpareLi3;

			// Token: 0x040033F6 RID: 13302
			private long CreateTime;

			// Token: 0x040033F7 RID: 13303
			private long UserTime;

			// Token: 0x040033F8 RID: 13304
			private long KernelTime;

			// Token: 0x040033F9 RID: 13305
			internal ushort NameLength;

			// Token: 0x040033FA RID: 13306
			internal ushort MaximumNameLength;

			// Token: 0x040033FB RID: 13307
			internal IntPtr NamePtr;

			// Token: 0x040033FC RID: 13308
			internal int BasePriority;

			// Token: 0x040033FD RID: 13309
			internal IntPtr UniqueProcessId;

			// Token: 0x040033FE RID: 13310
			internal IntPtr InheritedFromUniqueProcessId;

			// Token: 0x040033FF RID: 13311
			internal uint HandleCount;

			// Token: 0x04003400 RID: 13312
			internal uint SessionId;

			// Token: 0x04003401 RID: 13313
			internal IntPtr PageDirectoryBase;

			// Token: 0x04003402 RID: 13314
			internal IntPtr PeakVirtualSize;

			// Token: 0x04003403 RID: 13315
			internal IntPtr VirtualSize;

			// Token: 0x04003404 RID: 13316
			internal uint PageFaultCount;

			// Token: 0x04003405 RID: 13317
			internal IntPtr PeakWorkingSetSize;

			// Token: 0x04003406 RID: 13318
			internal IntPtr WorkingSetSize;

			// Token: 0x04003407 RID: 13319
			internal IntPtr QuotaPeakPagedPoolUsage;

			// Token: 0x04003408 RID: 13320
			internal IntPtr QuotaPagedPoolUsage;

			// Token: 0x04003409 RID: 13321
			internal IntPtr QuotaPeakNonPagedPoolUsage;

			// Token: 0x0400340A RID: 13322
			internal IntPtr QuotaNonPagedPoolUsage;

			// Token: 0x0400340B RID: 13323
			internal IntPtr PagefileUsage;

			// Token: 0x0400340C RID: 13324
			internal IntPtr PeakPagefileUsage;

			// Token: 0x0400340D RID: 13325
			internal IntPtr PrivatePageCount;

			// Token: 0x0400340E RID: 13326
			private long ReadOperationCount;

			// Token: 0x0400340F RID: 13327
			private long WriteOperationCount;

			// Token: 0x04003410 RID: 13328
			private long OtherOperationCount;

			// Token: 0x04003411 RID: 13329
			private long ReadTransferCount;

			// Token: 0x04003412 RID: 13330
			private long WriteTransferCount;

			// Token: 0x04003413 RID: 13331
			private long OtherTransferCount;
		}

		// Token: 0x02000785 RID: 1925
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemThreadInformation
		{
			// Token: 0x04003414 RID: 13332
			private long KernelTime;

			// Token: 0x04003415 RID: 13333
			private long UserTime;

			// Token: 0x04003416 RID: 13334
			private long CreateTime;

			// Token: 0x04003417 RID: 13335
			private uint WaitTime;

			// Token: 0x04003418 RID: 13336
			internal IntPtr StartAddress;

			// Token: 0x04003419 RID: 13337
			internal IntPtr UniqueProcess;

			// Token: 0x0400341A RID: 13338
			internal IntPtr UniqueThread;

			// Token: 0x0400341B RID: 13339
			internal int Priority;

			// Token: 0x0400341C RID: 13340
			internal int BasePriority;

			// Token: 0x0400341D RID: 13341
			internal uint ContextSwitches;

			// Token: 0x0400341E RID: 13342
			internal uint ThreadState;

			// Token: 0x0400341F RID: 13343
			internal uint WaitReason;
		}
	}
}
