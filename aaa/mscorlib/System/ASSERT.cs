using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x02000102 RID: 258
	internal sealed class ASSERT : Exception
	{
		// Token: 0x06000EBD RID: 3773 RVA: 0x0002C2DC File Offset: 0x0002B2DC
		private static bool AssertIsFriend(Type[] friends, StackTrace st)
		{
			Type declaringType = st.GetFrame(1).GetMethod().DeclaringType;
			Type declaringType2 = st.GetFrame(2).GetMethod().DeclaringType;
			bool flag = true;
			foreach (Type type in friends)
			{
				if (declaringType2 != type && declaringType2 != declaringType)
				{
					flag = false;
				}
			}
			if (flag)
			{
				ASSERT.Assert(false, Environment.GetResourceString("RtType.InvalidCaller"), st.ToString());
			}
			return true;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0002C350 File Offset: 0x0002B350
		[Conditional("_DEBUG")]
		internal static void FRIEND(Type[] friends)
		{
			StackTrace stackTrace = new StackTrace();
			ASSERT.AssertIsFriend(friends, stackTrace);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0002C36C File Offset: 0x0002B36C
		[Conditional("_DEBUG")]
		internal static void FRIEND(Type friend)
		{
			StackTrace stackTrace = new StackTrace();
			ASSERT.AssertIsFriend(new Type[] { friend }, stackTrace);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0002C394 File Offset: 0x0002B394
		[Conditional("_DEBUG")]
		internal static void FRIEND(string ns)
		{
			StackTrace stackTrace = new StackTrace();
			string @namespace = stackTrace.GetFrame(1).GetMethod().DeclaringType.Namespace;
			string namespace2 = stackTrace.GetFrame(2).GetMethod().DeclaringType.Namespace;
			ASSERT.Assert(namespace2.Equals(namespace2) || namespace2.Equals(ns), Environment.GetResourceString("RtType.InvalidCaller"), stackTrace.ToString());
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0002C3FD File Offset: 0x0002B3FD
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002C405 File Offset: 0x0002B405
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0002C40E File Offset: 0x0002B40E
		[Conditional("_DEBUG")]
		internal static void PRECONDITION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0002C418 File Offset: 0x0002B418
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0002C420 File Offset: 0x0002B420
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0002C429 File Offset: 0x0002B429
		[Conditional("_DEBUG")]
		internal static void POSTCONDITION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0002C433 File Offset: 0x0002B433
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0002C43B File Offset: 0x0002B43B
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0002C444 File Offset: 0x0002B444
		[Conditional("_DEBUG")]
		internal static void CONSISTENCY_CHECK(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0002C44E File Offset: 0x0002B44E
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition)
		{
			ASSERT.Assert(condition);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0002C456 File Offset: 0x0002B456
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition, string message)
		{
			ASSERT.Assert(condition, message);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0002C45F File Offset: 0x0002B45F
		[Conditional("_DEBUG")]
		internal static void SIMPLIFYING_ASSUMPTION(bool condition, string message, string detailedMessage)
		{
			ASSERT.Assert(condition, message, detailedMessage);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0002C469 File Offset: 0x0002B469
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE()
		{
			ASSERT.Assert();
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0002C470 File Offset: 0x0002B470
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE(string message)
		{
			ASSERT.Assert(message);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0002C478 File Offset: 0x0002B478
		[Conditional("_DEBUG")]
		internal static void UNREACHABLE(string message, string detailedMessage)
		{
			ASSERT.Assert(message, detailedMessage);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0002C481 File Offset: 0x0002B481
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED()
		{
			ASSERT.Assert();
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0002C488 File Offset: 0x0002B488
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED(string message)
		{
			ASSERT.Assert(message);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0002C490 File Offset: 0x0002B490
		[Conditional("_DEBUG")]
		internal static void NOT_IMPLEMENTED(string message, string detailedMessage)
		{
			ASSERT.Assert(message, detailedMessage);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0002C499 File Offset: 0x0002B499
		private static void Assert()
		{
			ASSERT.Assert(false, null, null);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0002C4A3 File Offset: 0x0002B4A3
		private static void Assert(string message)
		{
			ASSERT.Assert(false, message, null);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0002C4AD File Offset: 0x0002B4AD
		private static void Assert(bool condition)
		{
			ASSERT.Assert(condition, null, null);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002C4B7 File Offset: 0x0002B4B7
		private static void Assert(bool condition, string message)
		{
			ASSERT.Assert(condition, message, null);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0002C4C1 File Offset: 0x0002B4C1
		private static void Assert(string message, string detailedMessage)
		{
			ASSERT.Assert(false, message, detailedMessage);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0002C4CB File Offset: 0x0002B4CB
		private static void Assert(bool condition, string message, string detailedMessage)
		{
		}
	}
}
