using System;
using System.IO;

namespace Microsoft.JScript
{
	// Token: 0x02000113 RID: 275
	public class ScriptStream
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x00057514 File Offset: 0x00056514
		public static void PrintStackTrace()
		{
			try
			{
				throw new Exception();
			}
			catch (Exception ex)
			{
				ScriptStream.PrintStackTrace(ex);
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00057540 File Offset: 0x00056540
		public static void PrintStackTrace(Exception e)
		{
			ScriptStream.Out.WriteLine(e.StackTrace);
			ScriptStream.Out.Flush();
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0005755C File Offset: 0x0005655C
		public static void Write(string str)
		{
			ScriptStream.Out.Write(str);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00057569 File Offset: 0x00056569
		public static void WriteLine(string str)
		{
			ScriptStream.Out.WriteLine(str);
		}

		// Token: 0x040006E3 RID: 1763
		public static TextWriter Out = Console.Out;

		// Token: 0x040006E4 RID: 1764
		public static TextWriter Error = Console.Error;
	}
}
