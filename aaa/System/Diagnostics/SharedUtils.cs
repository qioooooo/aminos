using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000798 RID: 1944
	internal static class SharedUtils
	{
		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003BEE RID: 15342 RVA: 0x0010050C File Offset: 0x000FF50C
		private static object InternalSyncObject
		{
			get
			{
				if (SharedUtils.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SharedUtils.s_InternalSyncObject, obj, null);
				}
				return SharedUtils.s_InternalSyncObject;
			}
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x00100538 File Offset: 0x000FF538
		internal static Win32Exception CreateSafeWin32Exception()
		{
			return SharedUtils.CreateSafeWin32Exception(0);
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00100540 File Offset: 0x000FF540
		internal static Win32Exception CreateSafeWin32Exception(int error)
		{
			Win32Exception ex = null;
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				if (error == 0)
				{
					ex = new Win32Exception();
				}
				else
				{
					ex = new Win32Exception(error);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return ex;
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00100588 File Offset: 0x000FF588
		internal static int CurrentEnvironment
		{
			get
			{
				if (SharedUtils.environment == 0)
				{
					lock (SharedUtils.InternalSyncObject)
					{
						if (SharedUtils.environment == 0)
						{
							if (Environment.OSVersion.Platform == PlatformID.Win32NT)
							{
								if (Environment.OSVersion.Version.Major >= 5)
								{
									SharedUtils.environment = 1;
								}
								else
								{
									SharedUtils.environment = 2;
								}
							}
							else
							{
								SharedUtils.environment = 3;
							}
						}
					}
				}
				return SharedUtils.environment;
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00100604 File Offset: 0x000FF604
		internal static void CheckEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 3)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0010061E File Offset: 0x000FF61E
		internal static void CheckNtEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 2)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00100638 File Offset: 0x000FF638
		internal static void EnterMutex(string name, ref Mutex mutex)
		{
			string text;
			if (SharedUtils.CurrentEnvironment == 1)
			{
				text = "Global\\" + name;
			}
			else
			{
				text = name;
			}
			SharedUtils.EnterMutexWithoutGlobal(text, ref mutex);
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00100668 File Offset: 0x000FF668
		internal static void EnterMutexWithoutGlobal(string mutexName, ref Mutex mutex)
		{
			MutexSecurity mutexSecurity = new MutexSecurity();
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
			mutexSecurity.AddAccessRule(new MutexAccessRule(securityIdentifier, MutexRights.Modify | MutexRights.Synchronize, AccessControlType.Allow));
			bool flag;
			Mutex mutex2 = new Mutex(false, mutexName, out flag, mutexSecurity);
			SharedUtils.SafeWaitForMutex(mutex2, ref mutex);
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001006A9 File Offset: 0x000FF6A9
		private static bool SafeWaitForMutex(Mutex mutexIn, ref Mutex mutexOut)
		{
			while (SharedUtils.SafeWaitForMutexOnce(mutexIn, ref mutexOut))
			{
				if (mutexOut != null)
				{
					return true;
				}
				Thread.Sleep(0);
			}
			return false;
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x001006C4 File Offset: 0x000FF6C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool SafeWaitForMutexOnce(Mutex mutexIn, ref Mutex mutexOut)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag;
			try
			{
			}
			finally
			{
				Thread.BeginCriticalRegion();
				Thread.BeginThreadAffinity();
				int num = SharedUtils.WaitForSingleObjectDontCallThis(mutexIn.SafeWaitHandle, 500);
				int num2 = num;
				if (num2 != 0 && num2 != 128)
				{
					flag = num2 == 258;
				}
				else
				{
					mutexOut = mutexIn;
					flag = true;
				}
				if (mutexOut == null)
				{
					Thread.EndThreadAffinity();
					Thread.EndCriticalRegion();
				}
			}
			return flag;
		}

		// Token: 0x06003BF8 RID: 15352
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", ExactSpelling = true, SetLastError = true)]
		private static extern int WaitForSingleObjectDontCallThis(SafeWaitHandle handle, int timeout);

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00100738 File Offset: 0x000FF738
		internal static string GetLatestBuildDllDirectory(string machineName)
		{
			string text = "";
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				if (machineName.Equals("."))
				{
					return SharedUtils.GetLocalBuildDirectory();
				}
				registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[] { "HKEY_LOCAL_MACHINE", machineName }));
				}
				registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
				if (registryKey2 != null)
				{
					string text2 = (string)registryKey2.GetValue("InstallRoot");
					if (text2 != null && text2 != string.Empty)
					{
						Version version = Environment.Version;
						string text3 = "v" + version.ToString(2);
						string text4 = null;
						RegistryKey registryKey3 = registryKey2.OpenSubKey("policy\\" + text3);
						if (registryKey3 != null)
						{
							try
							{
								text4 = (string)registryKey3.GetValue("Version");
								if (text4 == null)
								{
									string[] valueNames = registryKey3.GetValueNames();
									for (int i = 0; i < valueNames.Length; i++)
									{
										string text5 = text3 + "." + valueNames[i].Replace('-', '.');
										if (string.Compare(text5, text4, StringComparison.Ordinal) > 0)
										{
											text4 = text5;
										}
									}
								}
							}
							finally
							{
								registryKey3.Close();
							}
							if (text4 != null && text4 != string.Empty)
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.Append(text2);
								if (!text2.EndsWith("\\", StringComparison.Ordinal))
								{
									stringBuilder.Append("\\");
								}
								stringBuilder.Append(text4);
								stringBuilder.Append("\\");
								text = stringBuilder.ToString();
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return text;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0010095C File Offset: 0x000FF95C
		private static string GetLocalBuildDirectory()
		{
			int num = 264;
			int num2 = 25;
			StringBuilder stringBuilder = new StringBuilder(num);
			StringBuilder stringBuilder2 = new StringBuilder(num2);
			uint num4;
			uint num5;
			uint num3;
			for (num3 = NativeMethods.GetRequestedRuntimeInfo(null, null, null, 0U, 65U, stringBuilder, num, out num4, stringBuilder2, num2, out num5); num3 == 122U; num3 = NativeMethods.GetRequestedRuntimeInfo(null, null, null, 0U, 0U, stringBuilder, num, out num4, stringBuilder2, num2, out num5))
			{
				num *= 2;
				num2 *= 2;
				stringBuilder = new StringBuilder(num);
				stringBuilder2 = new StringBuilder(num2);
			}
			if (num3 != 0U)
			{
				throw SharedUtils.CreateSafeWin32Exception();
			}
			stringBuilder.Append(stringBuilder2);
			return stringBuilder.ToString();
		}

		// Token: 0x0400348C RID: 13452
		internal const int UnknownEnvironment = 0;

		// Token: 0x0400348D RID: 13453
		internal const int W2kEnvironment = 1;

		// Token: 0x0400348E RID: 13454
		internal const int NtEnvironment = 2;

		// Token: 0x0400348F RID: 13455
		internal const int NonNtEnvironment = 3;

		// Token: 0x04003490 RID: 13456
		private static int environment;

		// Token: 0x04003491 RID: 13457
		private static object s_InternalSyncObject;
	}
}
