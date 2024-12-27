using System;
using System.Diagnostics;

namespace System.Data.SqlTypes
{
	// Token: 0x02000377 RID: 887
	internal sealed class SQLDebug
	{
		// Token: 0x06002F50 RID: 12112 RVA: 0x002B0064 File Offset: 0x002AF464
		private SQLDebug()
		{
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x002B0078 File Offset: 0x002AF478
		[Conditional("DEBUG")]
		internal static void Check(bool condition)
		{
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x002B0088 File Offset: 0x002AF488
		[Conditional("DEBUG")]
		internal static void Check(bool condition, string conditionString, string message)
		{
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x002B0098 File Offset: 0x002AF498
		[Conditional("DEBUG")]
		internal static void Check(bool condition, string conditionString)
		{
		}
	}
}
