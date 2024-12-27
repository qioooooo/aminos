using System;
using System.Runtime.CompilerServices;

namespace System.Diagnostics
{
	// Token: 0x02000299 RID: 665
	internal static class Assert
	{
		// Token: 0x06001AB2 RID: 6834 RVA: 0x000468CE File Offset: 0x000458CE
		static Assert()
		{
			Assert.AddFilter(new DefaultFilter());
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000468DC File Offset: 0x000458DC
		public static void AddFilter(AssertFilter filter)
		{
			if (Assert.iFilterArraySize <= Assert.iNumOfFilters)
			{
				AssertFilter[] array = new AssertFilter[Assert.iFilterArraySize + 2];
				if (Assert.iNumOfFilters > 0)
				{
					Array.Copy(Assert.ListOfFilters, array, Assert.iNumOfFilters);
				}
				Assert.iFilterArraySize += 2;
				Assert.ListOfFilters = array;
			}
			Assert.ListOfFilters[Assert.iNumOfFilters++] = filter;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00046940 File Offset: 0x00045940
		public static void Check(bool condition, string conditionString, string message)
		{
			if (!condition)
			{
				Assert.Fail(conditionString, message);
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0004694C File Offset: 0x0004594C
		public static void Fail(string conditionString, string message)
		{
			StackTrace stackTrace = new StackTrace();
			int i = Assert.iNumOfFilters;
			while (i > 0)
			{
				AssertFilters assertFilters = Assert.ListOfFilters[--i].AssertFailure(conditionString, message, stackTrace);
				if (assertFilters == AssertFilters.FailDebug)
				{
					if (Debugger.IsAttached)
					{
						Debugger.Break();
						return;
					}
					if (!Debugger.Launch())
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DebuggerLaunchFailed"));
					}
					break;
				}
				else if (assertFilters == AssertFilters.FailTerminate)
				{
					Environment.Exit(-1);
				}
				else if (assertFilters == AssertFilters.FailIgnore)
				{
					return;
				}
			}
		}

		// Token: 0x06001AB6 RID: 6838
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ShowDefaultAssertDialog(string conditionString, string message);

		// Token: 0x040009F8 RID: 2552
		private static AssertFilter[] ListOfFilters;

		// Token: 0x040009F9 RID: 2553
		private static int iNumOfFilters;

		// Token: 0x040009FA RID: 2554
		private static int iFilterArraySize;
	}
}
