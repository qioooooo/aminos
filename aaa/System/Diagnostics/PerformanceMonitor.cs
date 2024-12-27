using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000769 RID: 1897
	internal class PerformanceMonitor
	{
		// Token: 0x06003A73 RID: 14963 RVA: 0x000F8559 File Offset: 0x000F7559
		internal PerformanceMonitor(string machineName)
		{
			this.machineName = machineName;
			this.Init();
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000F8570 File Offset: 0x000F7570
		private void Init()
		{
			try
			{
				if (this.machineName != "." && string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					this.perfDataKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.PerformanceData, this.machineName);
				}
				else
				{
					this.perfDataKey = Registry.PerformanceData;
				}
			}
			catch (UnauthorizedAccessException)
			{
				throw new Win32Exception(5);
			}
			catch (IOException ex)
			{
				throw new Win32Exception(Marshal.GetHRForException(ex));
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000F8604 File Offset: 0x000F7604
		internal void Close()
		{
			if (this.perfDataKey != null)
			{
				this.perfDataKey.Close();
			}
			this.perfDataKey = null;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000F8620 File Offset: 0x000F7620
		internal byte[] GetData(string item)
		{
			int i = 17;
			int num = 0;
			int num2 = 0;
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			while (i > 0)
			{
				try
				{
					return (byte[])this.perfDataKey.GetValue(item);
				}
				catch (IOException ex)
				{
					num2 = Marshal.GetHRForException(ex);
					int num3 = num2;
					if (num3 <= 167)
					{
						if (num3 != 6)
						{
							if (num3 != 21 && num3 != 167)
							{
								goto IL_00AC;
							}
							goto IL_0094;
						}
					}
					else if (num3 <= 258)
					{
						if (num3 != 170 && num3 != 258)
						{
							goto IL_00AC;
						}
						goto IL_0094;
					}
					else if (num3 != 1722 && num3 != 1726)
					{
						goto IL_00AC;
					}
					this.Init();
					IL_0094:
					i--;
					if (num == 0)
					{
						num = 10;
					}
					else
					{
						Thread.Sleep(num);
						num *= 2;
					}
					continue;
					IL_00AC:
					throw SharedUtils.CreateSafeWin32Exception(num2);
				}
			}
			throw SharedUtils.CreateSafeWin32Exception(num2);
		}

		// Token: 0x0400332C RID: 13100
		private RegistryKey perfDataKey;

		// Token: 0x0400332D RID: 13101
		private string machineName;
	}
}
