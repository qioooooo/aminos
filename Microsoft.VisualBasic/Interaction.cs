using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Interaction
	{
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int Shell(string PathName, AppWinStyle Style = AppWinStyle.MinimizedFocus, bool Wait = false, int Timeout = -1)
		{
			NativeTypes.STARTUPINFO startupinfo = new NativeTypes.STARTUPINFO();
			NativeTypes.PROCESS_INFORMATION process_INFORMATION = new NativeTypes.PROCESS_INFORMATION();
			try
			{
				new UIPermission(UIPermissionWindow.AllWindows).Demand();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			if (PathName == null)
			{
				throw new NullReferenceException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Pathname" }));
			}
			if (Style < AppWinStyle.Hide || Style > (AppWinStyle)9)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Style" }));
			}
			NativeMethods.GetStartupInfo(startupinfo);
			int num2;
			try
			{
				startupinfo.dwFlags = 1;
				startupinfo.wShowWindow = (short)Style;
				IntPtr intPtr;
				int num = NativeMethods.CreateProcess(null, PathName, null, null, false, 32, intPtr, null, startupinfo, process_INFORMATION);
				try
				{
					if (num != 0)
					{
						if (Wait)
						{
							num = NativeMethods.WaitForSingleObject(process_INFORMATION.hProcess, Timeout);
							if (num == 0)
							{
								num2 = 0;
							}
							else
							{
								num2 = process_INFORMATION.dwProcessId;
							}
						}
						else
						{
							NativeMethods.WaitForInputIdle(process_INFORMATION.hProcess, 10000);
							num2 = process_INFORMATION.dwProcessId;
						}
					}
					else
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error == 5)
						{
							throw ExceptionUtils.VbMakeException(70);
						}
						throw ExceptionUtils.VbMakeException(53);
					}
				}
				finally
				{
					process_INFORMATION.Dispose();
				}
			}
			finally
			{
				startupinfo.Dispose();
			}
			return num2;
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void AppActivate(int ProcessId)
		{
			IntPtr intPtr = NativeMethods.GetWindow(NativeMethods.GetDesktopWindow(), 5);
			int num;
			while (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.GetWindowThreadProcessId(intPtr, ref num);
				if (num == ProcessId && SafeNativeMethods.IsWindowEnabled(intPtr) && SafeNativeMethods.IsWindowVisible(intPtr))
				{
					break;
				}
				intPtr = NativeMethods.GetWindow(intPtr, 2);
			}
			if (intPtr == IntPtr.Zero)
			{
				intPtr = NativeMethods.GetWindow(NativeMethods.GetDesktopWindow(), 5);
				while (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.GetWindowThreadProcessId(intPtr, ref num);
					if (num == ProcessId)
					{
						break;
					}
					intPtr = NativeMethods.GetWindow(intPtr, 2);
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new ArgumentException(Utils.GetResourceString("ProcessNotFound", new string[] { Conversions.ToString(ProcessId) }));
			}
			Interaction.AppActivateHelper(intPtr);
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void AppActivate(string Title)
		{
			string text = null;
			IntPtr intPtr = NativeMethods.FindWindow(ref text, ref Title);
			if (intPtr == IntPtr.Zero)
			{
				string text2 = string.Empty;
				StringBuilder stringBuilder = new StringBuilder(511);
				int num = Strings.Len(Title);
				intPtr = NativeMethods.GetWindow(NativeMethods.GetDesktopWindow(), 5);
				while (intPtr != IntPtr.Zero)
				{
					int num2 = NativeMethods.GetWindowText(intPtr, stringBuilder, stringBuilder.Capacity);
					text2 = stringBuilder.ToString();
					if (num2 >= num && string.Compare(text2, 0, Title, 0, num, StringComparison.OrdinalIgnoreCase) == 0)
					{
						break;
					}
					intPtr = NativeMethods.GetWindow(intPtr, 2);
				}
				if (intPtr == IntPtr.Zero)
				{
					intPtr = NativeMethods.GetWindow(NativeMethods.GetDesktopWindow(), 5);
					while (intPtr != IntPtr.Zero)
					{
						int num2 = NativeMethods.GetWindowText(intPtr, stringBuilder, stringBuilder.Capacity);
						text2 = stringBuilder.ToString();
						if (num2 >= num && string.Compare(Strings.Right(text2, num), 0, Title, 0, num, StringComparison.OrdinalIgnoreCase) == 0)
						{
							break;
						}
						intPtr = NativeMethods.GetWindow(intPtr, 2);
					}
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new ArgumentException(Utils.GetResourceString("ProcessNotFound", new string[] { Title }));
			}
			Interaction.AppActivateHelper(intPtr);
		}

		private static void AppActivateHelper(IntPtr hwndApp)
		{
			try
			{
				new UIPermission(UIPermissionWindow.AllWindows).Demand();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			if (!SafeNativeMethods.IsWindowEnabled(hwndApp) || !SafeNativeMethods.IsWindowVisible(hwndApp))
			{
				IntPtr intPtr = NativeMethods.GetWindow(hwndApp, 0);
				while (intPtr != IntPtr.Zero)
				{
					if (NativeMethods.GetWindow(intPtr, 4) == hwndApp)
					{
						if (SafeNativeMethods.IsWindowEnabled(intPtr) && SafeNativeMethods.IsWindowVisible(intPtr))
						{
							break;
						}
						hwndApp = intPtr;
						intPtr = NativeMethods.GetWindow(hwndApp, 0);
					}
					intPtr = NativeMethods.GetWindow(intPtr, 2);
				}
				if (intPtr == IntPtr.Zero)
				{
					throw new ArgumentException(Utils.GetResourceString("ProcessNotFound"));
				}
				hwndApp = intPtr;
			}
			int num;
			NativeMethods.AttachThreadInput(0, SafeNativeMethods.GetWindowThreadProcessId(hwndApp, ref num), 1);
			NativeMethods.SetForegroundWindow(hwndApp);
			NativeMethods.SetFocus(hwndApp);
			NativeMethods.AttachThreadInput(0, SafeNativeMethods.GetWindowThreadProcessId(hwndApp, ref num), 0);
		}

		public static string Command()
		{
			new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Demand();
			if (Interaction.m_CommandLine == null)
			{
				string text = Environment.CommandLine;
				if (text == null || text.Length == 0)
				{
					return "";
				}
				int length = Environment.GetCommandLineArgs()[0].Length;
				int num;
				do
				{
					num = text.IndexOf('"', num);
					if (num >= 0 && num <= length)
					{
						text = text.Remove(num, 1);
					}
				}
				while (num >= 0 && num <= length);
				if (num == 0 || num > text.Length)
				{
					Interaction.m_CommandLine = "";
				}
				else
				{
					Interaction.m_CommandLine = Strings.LTrim(text.Substring(length));
				}
			}
			return Interaction.m_CommandLine;
		}

		public static string Environ(int Expression)
		{
			if (Expression <= 0 || Expression > 255)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_Range1toFF1", new string[] { "Expression" }));
			}
			if (Interaction.m_SortedEnvList == null)
			{
				object environSyncObject = Interaction.m_EnvironSyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(environSyncObject);
				lock (environSyncObject)
				{
					if (Interaction.m_SortedEnvList == null)
					{
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						Interaction.m_SortedEnvList = new SortedList(Environment.GetEnvironmentVariables());
						PermissionSet.RevertAssert();
					}
				}
			}
			if (Expression > Interaction.m_SortedEnvList.Count)
			{
				return "";
			}
			checked
			{
				string text = Interaction.m_SortedEnvList.GetKey(Expression - 1).ToString();
				string text2 = Interaction.m_SortedEnvList.GetByIndex(Expression - 1).ToString();
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, text).Demand();
				return text + "=" + text2;
			}
		}

		public static string Environ(string Expression)
		{
			Expression = Strings.Trim(Expression);
			if (Expression.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Expression" }));
			}
			return Environment.GetEnvironmentVariable(Expression);
		}

		public static void Beep()
		{
			try
			{
				new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();
			}
			catch (SecurityException ex)
			{
				try
				{
					new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
				}
				catch (SecurityException ex2)
				{
					return;
				}
			}
			UnsafeNativeMethods.MessageBeep(0);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.UI)]
		public static string InputBox(string Prompt, string Title = "", string DefaultResponse = "", int XPos = -1, int YPos = -1)
		{
			IWin32Window win32Window = null;
			IVbHost vbhost = HostServices.VBHost;
			if (vbhost != null)
			{
				win32Window = vbhost.GetParentWindow();
			}
			if (Title.Length == 0)
			{
				if (vbhost == null)
				{
					Title = Interaction.GetTitleFromAssembly(Assembly.GetCallingAssembly());
				}
				else
				{
					Title = vbhost.GetWindowTitle();
				}
			}
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
			{
				Interaction.InputBoxHandler inputBoxHandler = new Interaction.InputBoxHandler(Prompt, Title, DefaultResponse, XPos, YPos, win32Window);
				Thread thread = new Thread(new ThreadStart(inputBoxHandler.StartHere));
				thread.Start();
				thread.Join();
				return inputBoxHandler.Result;
			}
			return Interaction.InternalInputBox(Prompt, Title, DefaultResponse, XPos, YPos, win32Window);
		}

		private static string GetTitleFromAssembly(Assembly CallingAssembly)
		{
			string text;
			try
			{
				text = CallingAssembly.GetName().Name;
			}
			catch (SecurityException ex)
			{
				string fullName = CallingAssembly.FullName;
				int num = fullName.IndexOf(',');
				if (num >= 0)
				{
					text = fullName.Substring(0, num);
				}
				else
				{
					text = "";
				}
			}
			return text;
		}

		private static string InternalInputBox(string Prompt, string Title, string DefaultResponse, int XPos, int YPos, IWin32Window ParentWindow)
		{
			VBInputBox vbinputBox = new VBInputBox(Prompt, Title, DefaultResponse, XPos, YPos);
			vbinputBox.ShowDialog(ParentWindow);
			string output = vbinputBox.Output;
			vbinputBox.Dispose();
			return output;
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.UI)]
		public static MsgBoxResult MsgBox(object Prompt, MsgBoxStyle Buttons = MsgBoxStyle.OkOnly, object Title = null)
		{
			string text = null;
			IWin32Window win32Window = null;
			IVbHost vbhost = HostServices.VBHost;
			if (vbhost != null)
			{
				win32Window = vbhost.GetParentWindow();
			}
			if ((Buttons & (MsgBoxStyle)15) > MsgBoxStyle.RetryCancel || (Buttons & (MsgBoxStyle)240) > MsgBoxStyle.Information || (Buttons & (MsgBoxStyle)3840) > MsgBoxStyle.DefaultButton3)
			{
				Buttons = MsgBoxStyle.OkOnly;
			}
			try
			{
				if (Prompt != null)
				{
					text = (string)Conversions.ChangeType(Prompt, typeof(string));
				}
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "Prompt", "String" }));
			}
			string text2;
			try
			{
				if (Title == null)
				{
					if (vbhost == null)
					{
						text2 = Interaction.GetTitleFromAssembly(Assembly.GetCallingAssembly());
					}
					else
					{
						text2 = vbhost.GetWindowTitle();
					}
				}
				else
				{
					text2 = Conversions.ToString(Title);
				}
			}
			catch (StackOverflowException ex4)
			{
				throw ex4;
			}
			catch (OutOfMemoryException ex5)
			{
				throw ex5;
			}
			catch (ThreadAbortException ex6)
			{
				throw ex6;
			}
			catch (Exception)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "Title", "String" }));
			}
			return (MsgBoxResult)MessageBox.Show(win32Window, text, text2, (MessageBoxButtons)(Buttons & (MsgBoxStyle)15), (MessageBoxIcon)(Buttons & (MsgBoxStyle)240), (MessageBoxDefaultButton)(Buttons & (MsgBoxStyle)3840), (MessageBoxOptions)(Buttons & (MsgBoxStyle)(-4096)));
		}

		public static object Choose(double Index, params object[] Choice)
		{
			checked
			{
				int num = (int)Math.Round(unchecked(Conversion.Fix(Index) - 1.0));
				if (Choice.Rank != 1)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1", new string[] { "Choice" }));
				}
				if (num < 0 || num > Choice.GetUpperBound(0))
				{
					return null;
				}
				return Choice[num];
			}
		}

		public static object IIf(bool Expression, object TruePart, object FalsePart)
		{
			if (Expression)
			{
				return TruePart;
			}
			return FalsePart;
		}

		internal static T IIf<T>(bool Condition, T TruePart, T FalsePart)
		{
			if (Condition)
			{
				return TruePart;
			}
			return FalsePart;
		}

		public static string Partition(long Number, long Start, long Stop, long Interval)
		{
			string text = null;
			if (Start < 0L)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
			}
			if (Stop <= Start)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Stop" }));
			}
			if (Interval < 1L)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
			}
			long num;
			bool flag;
			long num2;
			bool flag2;
			string text2;
			string text3;
			checked
			{
				if (Number < Start)
				{
					num = Start - 1L;
					flag = true;
				}
				else if (Number > Stop)
				{
					num2 = Stop + 1L;
					flag2 = true;
				}
				else if (Interval == 1L)
				{
					num2 = Number;
					num = Number;
				}
				else
				{
					num2 = (Number - Start) / Interval * Interval + Start;
					num = num2 + Interval - 1L;
					if (num > Stop)
					{
						num = Stop;
					}
					if (num2 < Start)
					{
						num2 = Start;
					}
				}
				text2 = Conversions.ToString(Stop + 1L);
				text3 = Conversions.ToString(Start - 1L);
			}
			long num3;
			if (Strings.Len(text2) > Strings.Len(text3))
			{
				num3 = (long)Strings.Len(text2);
			}
			else
			{
				num3 = (long)Strings.Len(text3);
			}
			if (flag)
			{
				text2 = Conversions.ToString(num);
				if (num3 < (long)Strings.Len(text2))
				{
					num3 = (long)Strings.Len(text2);
				}
			}
			if (flag)
			{
				Interaction.InsertSpaces(ref text, num3);
			}
			else
			{
				Interaction.InsertNumber(ref text, num2, num3);
			}
			text += ":";
			if (flag2)
			{
				Interaction.InsertSpaces(ref text, num3);
			}
			else
			{
				Interaction.InsertNumber(ref text, num, num3);
			}
			return text;
		}

		private static void InsertSpaces(ref string Buffer, long Spaces)
		{
			checked
			{
				while (Spaces > 0L)
				{
					Buffer += " ";
					Spaces -= 1L;
				}
			}
		}

		private static void InsertNumber(ref string Buffer, long Num, long Spaces)
		{
			string text = Conversions.ToString(Num);
			checked
			{
				Interaction.InsertSpaces(ref Buffer, Spaces - unchecked((long)Strings.Len(text)));
				Buffer += text;
			}
		}

		public static object Switch(params object[] VarExpr)
		{
			if (VarExpr == null)
			{
				return null;
			}
			int i = VarExpr.Length;
			int num = 0;
			if (i % 2 != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "VarExpr" }));
			}
			checked
			{
				while (i > 0)
				{
					if (Conversions.ToBoolean(VarExpr[num]))
					{
						return VarExpr[num + 1];
					}
					num += 2;
					i -= 2;
				}
				return null;
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public static void DeleteSetting(string AppName, string Section = null, string Key = null)
		{
			RegistryKey registryKey = null;
			Interaction.CheckPathComponent(AppName);
			string text = Interaction.FormRegKey(AppName, Section);
			try
			{
				RegistryKey currentUser = Registry.CurrentUser;
				if (Information.IsNothing(Key) || Key.Length == 0)
				{
					currentUser.DeleteSubKeyTree(text);
				}
				else
				{
					registryKey = currentUser.OpenSubKey(text, true);
					if (registryKey == null)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Section" }));
					}
					registryKey.DeleteValue(Key);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
		}

		public static string[,] GetAllSettings(string AppName, string Section)
		{
			Interaction.CheckPathComponent(AppName);
			Interaction.CheckPathComponent(Section);
			string text = Interaction.FormRegKey(AppName, Section);
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text);
			if (registryKey == null)
			{
				return null;
			}
			string[,] array = null;
			checked
			{
				try
				{
					if (registryKey.ValueCount != 0)
					{
						string[] valueNames = registryKey.GetValueNames();
						int upperBound = valueNames.GetUpperBound(0);
						string[,] array2 = new string[upperBound + 1, 2];
						int num = 0;
						int num2 = upperBound;
						for (int i = num; i <= num2; i++)
						{
							string text2 = valueNames[i];
							array2[i, 0] = text2;
							object value = registryKey.GetValue(text2);
							if (value != null && value is string)
							{
								array2[i, 1] = value.ToString();
							}
						}
						array = array2;
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception ex4)
				{
				}
				finally
				{
					registryKey.Close();
				}
				return array;
			}
		}

		public static string GetSetting(string AppName, string Section, string Key, string Default = "")
		{
			RegistryKey registryKey = null;
			Interaction.CheckPathComponent(AppName);
			Interaction.CheckPathComponent(Section);
			Interaction.CheckPathComponent(Key);
			if (Default == null)
			{
				Default = "";
			}
			string text = Interaction.FormRegKey(AppName, Section);
			object value;
			try
			{
				registryKey = Registry.CurrentUser.OpenSubKey(text);
				if (registryKey == null)
				{
					return Default;
				}
				value = registryKey.GetValue(Key, Default);
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				return (string)value;
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
		}

		public static void SaveSetting(string AppName, string Section, string Key, string Setting)
		{
			Interaction.CheckPathComponent(AppName);
			Interaction.CheckPathComponent(Section);
			Interaction.CheckPathComponent(Key);
			string text = Interaction.FormRegKey(AppName, Section);
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
			if (registryKey == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Interaction_ResKeyNotCreated1", new string[] { text }));
			}
			try
			{
				registryKey.SetValue(Key, Setting);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				registryKey.Close();
			}
		}

		private static string FormRegKey(string sApp, string sSect)
		{
			string text;
			if (Information.IsNothing(sApp) || sApp.Length == 0)
			{
				text = "Software\\VB and VBA Program Settings";
			}
			else if (Information.IsNothing(sSect) || sSect.Length == 0)
			{
				text = "Software\\VB and VBA Program Settings\\" + sApp;
			}
			else
			{
				text = "Software\\VB and VBA Program Settings\\" + sApp + "\\" + sSect;
			}
			return text;
		}

		private static void CheckPathComponent(string s)
		{
			if (s == null || s.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_PathNullOrEmpty"));
			}
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public static object CreateObject(string ProgId, string ServerName = "")
		{
			if (ProgId.Length == 0)
			{
				throw ExceptionUtils.VbMakeException(429);
			}
			if (ServerName == null || ServerName.Length == 0)
			{
				ServerName = null;
			}
			else if (string.Compare(Environment.MachineName, ServerName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				ServerName = null;
			}
			object obj;
			try
			{
				Type type;
				if (ServerName == null)
				{
					type = Type.GetTypeFromProgID(ProgId);
				}
				else
				{
					type = Type.GetTypeFromProgID(ProgId, ServerName, true);
				}
				obj = Activator.CreateInstance(type);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147023174)
				{
					throw ExceptionUtils.VbMakeException(462);
				}
				throw ExceptionUtils.VbMakeException(429);
			}
			catch (StackOverflowException ex2)
			{
				throw ex2;
			}
			catch (OutOfMemoryException ex3)
			{
				throw ex3;
			}
			catch (ThreadAbortException ex4)
			{
				throw ex4;
			}
			catch (Exception ex5)
			{
				throw ExceptionUtils.VbMakeException(429);
			}
			return obj;
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public static object GetObject(string PathName = null, string Class = null)
		{
			if (Strings.Len(Class) == 0)
			{
				try
				{
					return Marshal.BindToMoniker(PathName);
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(429);
				}
			}
			if (PathName == null)
			{
				try
				{
					return Marshal.GetActiveObject(Class);
				}
				catch (StackOverflowException ex4)
				{
					throw ex4;
				}
				catch (OutOfMemoryException ex5)
				{
					throw ex5;
				}
				catch (ThreadAbortException ex6)
				{
					throw ex6;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(429);
				}
			}
			if (Strings.Len(PathName) == 0)
			{
				try
				{
					Type typeFromProgID = Type.GetTypeFromProgID(Class);
					return Activator.CreateInstance(typeFromProgID);
				}
				catch (StackOverflowException ex7)
				{
					throw ex7;
				}
				catch (OutOfMemoryException ex8)
				{
					throw ex8;
				}
				catch (ThreadAbortException ex9)
				{
					throw ex9;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(429);
				}
			}
			Interaction.IPersistFile persistFile;
			try
			{
				object activeObject = Marshal.GetActiveObject(Class);
				persistFile = (Interaction.IPersistFile)activeObject;
			}
			catch (StackOverflowException ex10)
			{
				throw ex10;
			}
			catch (OutOfMemoryException ex11)
			{
				throw ex11;
			}
			catch (ThreadAbortException ex12)
			{
				throw ex12;
			}
			catch (Exception)
			{
				throw ExceptionUtils.VbMakeException(432);
			}
			try
			{
				persistFile.Load(PathName, 0);
			}
			catch (StackOverflowException ex13)
			{
				throw ex13;
			}
			catch (OutOfMemoryException ex14)
			{
				throw ex14;
			}
			catch (ThreadAbortException ex15)
			{
				throw ex15;
			}
			catch (Exception)
			{
				throw ExceptionUtils.VbMakeException(429);
			}
			return persistFile;
		}

		public static object CallByName(object ObjectRef, string ProcName, CallType UseCallType, params object[] Args)
		{
			switch (UseCallType)
			{
			case CallType.Method:
				return LateBinding.InternalLateCall(ObjectRef, null, ProcName, Args, null, null, false);
			case CallType.Get:
				return LateBinding.LateGet(ObjectRef, null, ProcName, Args, null, null);
			case CallType.Let:
			case CallType.Set:
			{
				Type type = null;
				LateBinding.InternalLateSet(ObjectRef, ref type, ProcName, Args, null, false, UseCallType);
				return null;
			}
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "CallType" }));
		}

		private static SortedList m_SortedEnvList;

		private static string m_CommandLine;

		private static object m_EnvironSyncObject = new object();

		private sealed class InputBoxHandler
		{
			public InputBoxHandler(string Prompt, string Title, string DefaultResponse, int XPos, int YPos, IWin32Window ParentWindow)
			{
				this.m_Prompt = Prompt;
				this.m_Title = Title;
				this.m_DefaultResponse = DefaultResponse;
				this.m_XPos = XPos;
				this.m_YPos = YPos;
				this.m_ParentWindow = ParentWindow;
			}

			[STAThread]
			public void StartHere()
			{
				this.m_Result = Interaction.InternalInputBox(this.m_Prompt, this.m_Title, this.m_DefaultResponse, this.m_XPos, this.m_YPos, this.m_ParentWindow);
			}

			public string Result
			{
				get
				{
					return this.m_Result;
				}
			}

			private string m_Prompt;

			private string m_Title;

			private string m_DefaultResponse;

			private int m_XPos;

			private int m_YPos;

			private string m_Result;

			private IWin32Window m_ParentWindow;
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000010B-0000-0000-C000-000000000046")]
		[ComVisible(true)]
		private interface IPersistFile
		{
			void GetClassID(ref Guid pClassID);

			void IsDirty();

			void Load(string pszFileName, int dwMode);

			void Save(string pszFileName, int fRemember);

			void SaveCompleted(string pszFileName);

			string GetCurFile();
		}
	}
}
