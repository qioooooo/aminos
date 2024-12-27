using System;
using System.Diagnostics;

namespace Microsoft.JScript
{
	// Token: 0x0200005C RID: 92
	internal static class Debug
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x00024592 File Offset: 0x00023592
		[Conditional("ASSERTION")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				throw new AssertException("Assertion fired");
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000245A2 File Offset: 0x000235A2
		[Conditional("ASSERTION")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				throw new AssertException(message);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000245AE File Offset: 0x000235AE
		[Conditional("ASSERTION")]
		public static void NotImplemented(string message)
		{
			throw new AssertException("Method Not Yet Implemented");
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000245BA File Offset: 0x000235BA
		[Conditional("ASSERTION")]
		public static void PostCondition(bool condition)
		{
			if (!condition)
			{
				throw new PostConditionException("PostCondition missed");
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000245CA File Offset: 0x000235CA
		[Conditional("ASSERTION")]
		public static void PostCondition(bool condition, string message)
		{
			if (!condition)
			{
				throw new PostConditionException(message);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000245D6 File Offset: 0x000235D6
		[Conditional("ASSERTION")]
		public static void PreCondition(bool condition)
		{
			if (!condition)
			{
				throw new PreConditionException("PreCondition missed");
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000245E6 File Offset: 0x000235E6
		[Conditional("ASSERTION")]
		public static void PreCondition(bool condition, string message)
		{
			if (!condition)
			{
				throw new PreConditionException(message);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000245F2 File Offset: 0x000235F2
		[Conditional("LOGGING")]
		public static void Print(string str)
		{
			ScriptStream.Out.WriteLine(str);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000245FF File Offset: 0x000235FF
		[Conditional("LOGGING")]
		internal static void PrintLine(string message)
		{
			ScriptStream.Out.WriteLine(message);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002460C File Offset: 0x0002360C
		[Conditional("LOGGING")]
		public static void PrintStack()
		{
			ScriptStream.PrintStackTrace();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00024613 File Offset: 0x00023613
		[Conditional("LOGGING")]
		public static void PrintStack(Exception e)
		{
			ScriptStream.PrintStackTrace(e);
		}
	}
}
