using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F4 RID: 500
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class Executor
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x000378DC File Offset: 0x000368DC
		private Executor()
		{
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000378E4 File Offset: 0x000368E4
		internal static string GetRuntimeInstallDirectory()
		{
			return RuntimeEnvironment.GetRuntimeDirectory();
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000378EB File Offset: 0x000368EB
		private static FileStream CreateInheritedFile(string file)
		{
			return new FileStream(file, FileMode.CreateNew, FileAccess.Write, FileShare.Read | FileShare.Inheritable);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000378F8 File Offset: 0x000368F8
		public static void ExecWait(string cmd, TempFileCollection tempFiles)
		{
			string text = null;
			string text2 = null;
			Executor.ExecWaitWithCapture(null, cmd, tempFiles, ref text, ref text2);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00037916 File Offset: 0x00036916
		public static int ExecWaitWithCapture(string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(null, cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00037928 File Offset: 0x00036928
		public static int ExecWaitWithCapture(string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(null, cmd, currentDir, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00037937 File Offset: 0x00036937
		public static int ExecWaitWithCapture(IntPtr userToken, string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(new SafeUserTokenHandle(userToken, false), cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00037950 File Offset: 0x00036950
		public static int ExecWaitWithCapture(IntPtr userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName)
		{
			return Executor.ExecWaitWithCapture(new SafeUserTokenHandle(userToken, false), cmd, Environment.CurrentDirectory, tempFiles, ref outputName, ref errorName, null);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0003796C File Offset: 0x0003696C
		internal static int ExecWaitWithCapture(SafeUserTokenHandle userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName, string trueCmdLine)
		{
			int num = 0;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					num = Executor.ExecWaitWithCaptureUnimpersonated(userToken, cmd, currentDir, tempFiles, ref outputName, ref errorName, trueCmdLine);
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return num;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x000379BC File Offset: 0x000369BC
		private unsafe static int ExecWaitWithCaptureUnimpersonated(SafeUserTokenHandle userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName, string trueCmdLine)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (outputName == null || outputName.Length == 0)
			{
				outputName = tempFiles.AddExtension("out");
			}
			if (errorName == null || errorName.Length == 0)
			{
				errorName = tempFiles.AddExtension("err");
			}
			FileStream fileStream = Executor.CreateInheritedFile(outputName);
			FileStream fileStream2 = Executor.CreateInheritedFile(errorName);
			bool flag = false;
			SafeNativeMethods.PROCESS_INFORMATION process_INFORMATION = new SafeNativeMethods.PROCESS_INFORMATION();
			SafeProcessHandle safeProcessHandle = new SafeProcessHandle();
			SafeThreadHandle safeThreadHandle = new SafeThreadHandle();
			SafeUserTokenHandle safeUserTokenHandle = null;
			try
			{
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
				streamWriter.Write(currentDir);
				streamWriter.Write("> ");
				streamWriter.WriteLine((trueCmdLine != null) ? trueCmdLine : cmd);
				streamWriter.WriteLine();
				streamWriter.WriteLine();
				streamWriter.Flush();
				NativeMethods.STARTUPINFO startupinfo = new NativeMethods.STARTUPINFO();
				startupinfo.cb = Marshal.SizeOf(startupinfo);
				startupinfo.dwFlags = 257;
				startupinfo.wShowWindow = 0;
				startupinfo.hStdOutput = fileStream.SafeFileHandle;
				startupinfo.hStdError = fileStream2.SafeFileHandle;
				startupinfo.hStdInput = new SafeFileHandle(UnsafeNativeMethods.GetStdHandle(-10), false);
				StringDictionary stringDictionary = new StringDictionary();
				foreach (object obj in Environment.GetEnvironmentVariables())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					stringDictionary.Add((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
				stringDictionary["_ClrRestrictSecAttributes"] = "1";
				byte[] array = EnvironmentBlock.ToByteArray(stringDictionary, false);
				try
				{
					fixed (byte* ptr = array)
					{
						IntPtr intPtr = new IntPtr((void*)ptr);
						if (userToken == null || userToken.IsInvalid)
						{
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
								goto IL_02F5;
							}
							finally
							{
								flag = NativeMethods.CreateProcess(null, new StringBuilder(cmd), null, null, true, 0, intPtr, currentDir, startupinfo, process_INFORMATION);
								if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
								{
									safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
								}
								if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
								{
									safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
								}
							}
						}
						flag = SafeUserTokenHandle.DuplicateTokenEx(userToken, 983551, null, 2, 1, out safeUserTokenHandle);
						if (flag)
						{
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
							}
							finally
							{
								flag = NativeMethods.CreateProcessAsUser(safeUserTokenHandle, null, cmd, null, null, true, 0, new HandleRef(null, intPtr), currentDir, startupinfo, process_INFORMATION);
								if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
								{
									safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
								}
								if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
								{
									safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
								}
							}
						}
						IL_02F5:;
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			finally
			{
				if (!flag && safeUserTokenHandle != null && !safeUserTokenHandle.IsInvalid)
				{
					safeUserTokenHandle.Close();
					safeUserTokenHandle = null;
				}
				fileStream.Close();
				fileStream2.Close();
			}
			if (flag)
			{
				try
				{
					int num = NativeMethods.WaitForSingleObject(safeProcessHandle, 600000);
					if (num == 258)
					{
						throw new ExternalException(SR.GetString("ExecTimeout", new object[] { cmd }), 258);
					}
					if (num != 0)
					{
						throw new ExternalException(SR.GetString("ExecBadreturn", new object[] { cmd }), Marshal.GetLastWin32Error());
					}
					int num2 = 259;
					if (!NativeMethods.GetExitCodeProcess(safeProcessHandle, out num2))
					{
						throw new ExternalException(SR.GetString("ExecCantGetRetCode", new object[] { cmd }), Marshal.GetLastWin32Error());
					}
					return num2;
				}
				finally
				{
					safeProcessHandle.Close();
					safeThreadHandle.Close();
					if (safeUserTokenHandle != null && !safeUserTokenHandle.IsInvalid)
					{
						safeUserTokenHandle.Close();
					}
				}
			}
			throw new ExternalException(SR.GetString("ExecCantExec", new object[] { cmd }), Marshal.GetLastWin32Error());
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00037E70 File Offset: 0x00036E70
		internal static WindowsImpersonationContext RevertImpersonation()
		{
			new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
			return WindowsIdentity.Impersonate(new IntPtr(0));
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00037E8C File Offset: 0x00036E8C
		internal static void ReImpersonate(WindowsImpersonationContext impersonation)
		{
			impersonation.Undo();
		}

		// Token: 0x04000F82 RID: 3970
		private const int ProcessTimeOut = 600000;
	}
}
