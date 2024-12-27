using System;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000057 RID: 87
	internal static class Debug
	{
		// Token: 0x06000374 RID: 884 RVA: 0x000127A1 File Offset: 0x000117A1
		[Conditional("DBG")]
		internal static void Trace(string tagName, string message)
		{
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000127A3 File Offset: 0x000117A3
		[Conditional("DBG")]
		internal static void Trace(string tagName, string message, bool includePrefix)
		{
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000127A5 File Offset: 0x000117A5
		[Conditional("DBG")]
		internal static void Trace(string tagName, string message, Exception e)
		{
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000127A7 File Offset: 0x000117A7
		[Conditional("DBG")]
		internal static void Trace(string tagName, Exception e)
		{
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000127A9 File Offset: 0x000117A9
		[Conditional("DBG")]
		internal static void Trace(string tagName, string message, Exception e, bool includePrefix)
		{
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000127AB File Offset: 0x000117AB
		[Conditional("DBG")]
		internal static void Assert(bool assertion, string message)
		{
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000127AD File Offset: 0x000117AD
		[Conditional("DBG")]
		internal static void Assert(bool assertion)
		{
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000127AF File Offset: 0x000117AF
		[Conditional("DBG")]
		internal static void Fail(string message)
		{
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000127B1 File Offset: 0x000117B1
		internal static bool IsTagEnabled(string tagName)
		{
			return false;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000127B4 File Offset: 0x000117B4
		internal static bool IsTagPresent(string tagName)
		{
			return false;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000127B7 File Offset: 0x000117B7
		[Conditional("DBG")]
		internal static void Break()
		{
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000127B9 File Offset: 0x000117B9
		[Conditional("DBG")]
		internal static void AlwaysValidate(string tagName)
		{
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000127BB File Offset: 0x000117BB
		[Conditional("DBG")]
		internal static void CheckValid(bool assertion, string message)
		{
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000127BD File Offset: 0x000117BD
		[Conditional("DBG")]
		internal static void Validate(object obj)
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000127BF File Offset: 0x000117BF
		[Conditional("DBG")]
		internal static void Validate(string tagName, object obj)
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000127C1 File Offset: 0x000117C1
		[Conditional("DBG")]
		internal static void Dump(string tagName, object obj)
		{
		}

		// Token: 0x040002DA RID: 730
		internal const string TAG_INTERNAL = "Internal";

		// Token: 0x040002DB RID: 731
		internal const string TAG_EXTERNAL = "External";

		// Token: 0x040002DC RID: 732
		internal const string TAG_ALL = "*";

		// Token: 0x040002DD RID: 733
		internal const string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss.ffff";

		// Token: 0x040002DE RID: 734
		internal const string TIME_FORMAT = "HH:mm:ss:ffff";
	}
}
