using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Configuration.Install
{
	// Token: 0x02000010 RID: 16
	public class InstallContext
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00003E0D File Offset: 0x00002E0D
		public InstallContext()
			: this(null, null)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003E18 File Offset: 0x00002E18
		public InstallContext(string logFilePath, string[] commandLine)
		{
			this.parameters = InstallContext.ParseCommandLine(commandLine);
			if (this.Parameters["logfile"] != null)
			{
				this.logFilePath = this.Parameters["logfile"];
				return;
			}
			if (logFilePath != null)
			{
				this.logFilePath = logFilePath;
				this.Parameters["logfile"] = logFilePath;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003E7B File Offset: 0x00002E7B
		public StringDictionary Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003E84 File Offset: 0x00002E84
		public bool IsParameterTrue(string paramName)
		{
			string text = this.Parameters[paramName.ToLower(CultureInfo.InvariantCulture)];
			return text != null && (string.Compare(text, "true", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "yes", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "1", StringComparison.OrdinalIgnoreCase) == 0 || "".Equals(text));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003EE4 File Offset: 0x00002EE4
		public void LogMessage(string message)
		{
			this.logFilePath = this.Parameters["logfile"];
			if (this.logFilePath != null && !"".Equals(this.logFilePath))
			{
				StreamWriter streamWriter = null;
				try
				{
					streamWriter = new StreamWriter(this.logFilePath, true, Encoding.UTF8);
					streamWriter.WriteLine(message);
				}
				finally
				{
					if (streamWriter != null)
					{
						streamWriter.Close();
					}
				}
			}
			if (this.IsParameterTrue("LogToConsole") || this.Parameters["logtoconsole"] == null)
			{
				Console.WriteLine(message);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003F80 File Offset: 0x00002F80
		protected static StringDictionary ParseCommandLine(string[] args)
		{
			StringDictionary stringDictionary = new StringDictionary();
			if (args == null)
			{
				return stringDictionary;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("/", StringComparison.Ordinal) || args[i].StartsWith("-", StringComparison.Ordinal))
				{
					args[i] = args[i].Substring(1);
				}
				int num = args[i].IndexOf('=');
				if (num < 0)
				{
					stringDictionary[args[i].ToLower(CultureInfo.InvariantCulture)] = "";
				}
				else
				{
					stringDictionary[args[i].Substring(0, num).ToLower(CultureInfo.InvariantCulture)] = args[i].Substring(num + 1);
				}
			}
			return stringDictionary;
		}

		// Token: 0x040000F0 RID: 240
		private string logFilePath;

		// Token: 0x040000F1 RID: 241
		private StringDictionary parameters;
	}
}
