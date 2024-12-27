using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000431 RID: 1073
	internal static class Fusion
	{
		// Token: 0x06002C20 RID: 11296 RVA: 0x00096908 File Offset: 0x00095908
		public static void ReadCache(ArrayList alAssems, string name, uint nFlag)
		{
			IAssemblyEnum assemblyEnum = null;
			IAssemblyName assemblyName = null;
			IAssemblyName assemblyName2 = null;
			IApplicationContext applicationContext = null;
			int num;
			if (name != null)
			{
				num = Win32Native.CreateAssemblyNameObject(out assemblyName2, name, 1U, IntPtr.Zero);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
			num = Win32Native.CreateAssemblyEnum(out assemblyEnum, applicationContext, assemblyName2, nFlag, IntPtr.Zero);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			for (;;)
			{
				num = assemblyEnum.GetNextAssembly(out applicationContext, out assemblyName, 0U);
				if (num != 0)
				{
					break;
				}
				string displayName = Fusion.GetDisplayName(assemblyName, 0U);
				if (displayName != null)
				{
					alAssems.Add(displayName);
				}
			}
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
				return;
			}
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x00096990 File Offset: 0x00095990
		private unsafe static string GetDisplayName(IAssemblyName aName, uint dwDisplayFlags)
		{
			uint num = 0U;
			string text = null;
			aName.GetDisplayName((IntPtr)0, ref num, dwDisplayFlags);
			if (num > 0U)
			{
				IntPtr intPtr = (IntPtr)0;
				byte[] array = new byte[(num + 1U) * 2U];
				fixed (byte* ptr = array)
				{
					intPtr = new IntPtr((void*)ptr);
					aName.GetDisplayName(intPtr, ref num, dwDisplayFlags);
					text = Marshal.PtrToStringUni(intPtr);
				}
			}
			return text;
		}
	}
}
