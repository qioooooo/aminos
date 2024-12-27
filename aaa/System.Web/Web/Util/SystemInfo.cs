using System;

namespace System.Web.Util
{
	// Token: 0x02000791 RID: 1937
	internal static class SystemInfo
	{
		// Token: 0x06005D26 RID: 23846 RVA: 0x00175318 File Offset: 0x00174318
		internal static int GetNumProcessCPUs()
		{
			if (SystemInfo._trueNumberOfProcessors == 0)
			{
				UnsafeNativeMethods.SYSTEM_INFO system_INFO;
				UnsafeNativeMethods.GetSystemInfo(out system_INFO);
				if (system_INFO.dwNumberOfProcessors == 1U)
				{
					SystemInfo._trueNumberOfProcessors = 1;
				}
				else
				{
					IntPtr invalid_HANDLE_VALUE = UnsafeNativeMethods.INVALID_HANDLE_VALUE;
					IntPtr intPtr;
					IntPtr intPtr2;
					if (UnsafeNativeMethods.GetProcessAffinityMask(invalid_HANDLE_VALUE, out intPtr, out intPtr2) == 0)
					{
						SystemInfo._trueNumberOfProcessors = 1;
					}
					else
					{
						int num = 0;
						if (IntPtr.Size == 4)
						{
							for (uint num2 = (uint)(int)intPtr; num2 != 0U; num2 >>= 1)
							{
								if ((num2 & 1U) == 1U)
								{
									num++;
								}
							}
						}
						else
						{
							for (ulong num3 = (ulong)(long)intPtr; num3 != 0UL; num3 >>= 1)
							{
								if ((num3 & 1UL) == 1UL)
								{
									num++;
								}
							}
						}
						SystemInfo._trueNumberOfProcessors = num;
					}
				}
			}
			return SystemInfo._trueNumberOfProcessors;
		}

		// Token: 0x040031BB RID: 12731
		private static int _trueNumberOfProcessors;
	}
}
