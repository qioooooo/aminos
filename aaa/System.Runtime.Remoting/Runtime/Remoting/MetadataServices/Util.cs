using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200007A RID: 122
	internal static class Util
	{
		// Token: 0x06000397 RID: 919 RVA: 0x00011A8D File Offset: 0x00010A8D
		[Conditional("_LOGGING")]
		internal static void Log(string message)
		{
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00011A90 File Offset: 0x00010A90
		[Conditional("_LOGGING")]
		internal static void LogInput(ref TextReader input)
		{
			if (InternalRM.SoapCheckEnabled())
			{
				string text = input.ReadToEnd();
				input = new StringReader(text);
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00011AB4 File Offset: 0x00010AB4
		[Conditional("_LOGGING")]
		internal static void LogString(string strbuffer)
		{
		}

		// Token: 0x040002D4 RID: 724
		internal static StreamWriter writer;
	}
}
