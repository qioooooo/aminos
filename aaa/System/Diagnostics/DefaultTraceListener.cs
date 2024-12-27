using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020001BF RID: 447
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class DefaultTraceListener : TraceListener
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002BFDD File Offset: 0x0002AFDD
		public DefaultTraceListener()
			: base("Default")
		{
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002BFEA File Offset: 0x0002AFEA
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x0002C000 File Offset: 0x0002B000
		public bool AssertUiEnabled
		{
			get
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				return this.assertUIEnabled;
			}
			set
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				this.assertUIEnabled = value;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0002C017 File Offset: 0x0002B017
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0002C02D File Offset: 0x0002B02D
		public string LogFileName
		{
			get
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				return this.logFileName;
			}
			set
			{
				if (!this.settingsInitialized)
				{
					this.InitializeSettings();
				}
				this.logFileName = value;
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002C044 File Offset: 0x0002B044
		public override void Fail(string message)
		{
			this.Fail(message, null);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002C050 File Offset: 0x0002B050
		public override void Fail(string message, string detailMessage)
		{
			StackTrace stackTrace = new StackTrace(true);
			int num = 0;
			bool uiPermission = DefaultTraceListener.UiPermission;
			string text;
			try
			{
				text = this.StackTraceToString(stackTrace, num, stackTrace.FrameCount - 1);
			}
			catch
			{
				text = "";
			}
			this.WriteAssert(text, message, detailMessage);
			if (this.AssertUiEnabled && uiPermission)
			{
				AssertWrapper.ShowAssert(text, stackTrace.GetFrame(num), message, detailMessage);
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002C0BC File Offset: 0x0002B0BC
		private void InitializeSettings()
		{
			this.assertUIEnabled = DiagnosticsConfiguration.AssertUIEnabled;
			this.logFileName = DiagnosticsConfiguration.LogFileName;
			this.settingsInitialized = true;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002C0DC File Offset: 0x0002B0DC
		private void WriteAssert(string stackTrace, string message, string detailMessage)
		{
			string text = string.Concat(new string[]
			{
				SR.GetString("DebugAssertBanner"),
				"\r\n",
				SR.GetString("DebugAssertShortMessage"),
				"\r\n",
				message,
				"\r\n",
				SR.GetString("DebugAssertLongMessage"),
				"\r\n",
				detailMessage,
				"\r\n",
				stackTrace
			});
			this.WriteLine(text);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002C15C File Offset: 0x0002B15C
		private void WriteToLogFile(string message, bool useWriteLine)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(this.LogFileName);
				using (Stream stream = fileInfo.Open(FileMode.OpenOrCreate))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						stream.Position = stream.Length;
						if (useWriteLine)
						{
							streamWriter.WriteLine(message);
						}
						else
						{
							streamWriter.Write(message);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.WriteLine(SR.GetString("ExceptionOccurred", new object[]
				{
					this.LogFileName,
					ex.ToString()
				}), false);
			}
			catch
			{
				this.WriteLine(SR.GetString("ExceptionOccurred", new object[] { this.LogFileName, "" }), false);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002C254 File Offset: 0x0002B254
		private string StackTraceToString(StackTrace trace, int startFrameIndex, int endFrameIndex)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			for (int i = startFrameIndex; i <= endFrameIndex; i++)
			{
				StackFrame frame = trace.GetFrame(i);
				MethodBase method = frame.GetMethod();
				stringBuilder.Append("\r\n    at ");
				if (method.ReflectedType != null)
				{
					stringBuilder.Append(method.ReflectedType.Name);
				}
				else
				{
					stringBuilder.Append("<Module>");
				}
				stringBuilder.Append(".");
				stringBuilder.Append(method.Name);
				stringBuilder.Append("(");
				ParameterInfo[] parameters = method.GetParameters();
				for (int j = 0; j < parameters.Length; j++)
				{
					ParameterInfo parameterInfo = parameters[j];
					if (j > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(parameterInfo.ParameterType.Name);
					stringBuilder.Append(" ");
					stringBuilder.Append(parameterInfo.Name);
				}
				stringBuilder.Append(")  ");
				stringBuilder.Append(frame.GetFileName());
				int fileLineNumber = frame.GetFileLineNumber();
				if (fileLineNumber > 0)
				{
					stringBuilder.Append("(");
					stringBuilder.Append(fileLineNumber.ToString(CultureInfo.InvariantCulture));
					stringBuilder.Append(")");
				}
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002C3A9 File Offset: 0x0002B3A9
		public override void Write(string message)
		{
			this.Write(message, true);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002C3B4 File Offset: 0x0002B3B4
		private void Write(string message, bool useLogFile)
		{
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			if (message == null || message.Length <= 16384)
			{
				this.internalWrite(message);
			}
			else
			{
				int i;
				for (i = 0; i < message.Length - 16384; i += 16384)
				{
					this.internalWrite(message.Substring(i, 16384));
				}
				this.internalWrite(message.Substring(i));
			}
			if (useLogFile && this.LogFileName.Length != 0)
			{
				this.WriteToLogFile(message, false);
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002C43A File Offset: 0x0002B43A
		private void internalWrite(string message)
		{
			if (Debugger.IsLogging())
			{
				Debugger.Log(0, null, message);
				return;
			}
			if (message == null)
			{
				SafeNativeMethods.OutputDebugString(string.Empty);
				return;
			}
			SafeNativeMethods.OutputDebugString(message);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002C460 File Offset: 0x0002B460
		public override void WriteLine(string message)
		{
			this.WriteLine(message, true);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002C46A File Offset: 0x0002B46A
		private void WriteLine(string message, bool useLogFile)
		{
			if (base.NeedIndent)
			{
				this.WriteIndent();
			}
			this.Write(message + "\r\n", useLogFile);
			base.NeedIndent = true;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0002C494 File Offset: 0x0002B494
		private static bool UiPermission
		{
			get
			{
				bool flag = false;
				try
				{
					new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();
					flag = true;
				}
				catch
				{
				}
				return flag;
			}
		}

		// Token: 0x04000ED6 RID: 3798
		private const int internalWriteSize = 16384;

		// Token: 0x04000ED7 RID: 3799
		private bool assertUIEnabled;

		// Token: 0x04000ED8 RID: 3800
		private string logFileName;

		// Token: 0x04000ED9 RID: 3801
		private bool settingsInitialized;
	}
}
