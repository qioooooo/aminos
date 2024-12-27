using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000087 RID: 135
	internal class Wow64Helper
	{
		// Token: 0x0600032D RID: 813
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern uint GetSystemWow64Directory(char[] buffer, int length);

		// Token: 0x0600032E RID: 814
		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		private static extern bool IsWow64Process(IntPtr hProcess, ref bool bIsWow);

		// Token: 0x0600032F RID: 815
		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x06000330 RID: 816 RVA: 0x0000A33D File Offset: 0x0000933D
		private Wow64Helper()
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000A348 File Offset: 0x00009348
		public static bool IsWow64Supported()
		{
			bool flag = false;
			char[] array = new char[260];
			uint num = 0U;
			try
			{
				num = Wow64Helper.GetSystemWow64Directory(array, 260);
			}
			catch (EntryPointNotFoundException)
			{
				return flag;
			}
			if (num == 0U)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if ((long)lastWin32Error != 120L)
				{
					throw new RegistrationException(Resource.FormatString("Reg_CannotDetermineWow64", lastWin32Error));
				}
			}
			else
			{
				if (num <= 0U)
				{
					throw new RegistrationException(Resource.FormatString("Reg_CannotDetermineWow64Ex", num));
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000A3D0 File Offset: 0x000093D0
		public static bool IsWow64Process()
		{
			bool flag = false;
			if (!Wow64Helper.IsWow64Supported())
			{
				return flag;
			}
			try
			{
				if (!Wow64Helper.IsWow64Process(Wow64Helper.GetCurrentProcess(), ref flag))
				{
					throw new RegistrationException(Resource.FormatString("Reg_CannotDetermineBitness", Marshal.GetLastWin32Error()));
				}
			}
			catch (EntryPointNotFoundException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0400013E RID: 318
		private const uint ERROR_CALL_NOT_IMPLEMENTED = 120U;

		// Token: 0x0400013F RID: 319
		private const int MAX_PATH = 260;
	}
}
