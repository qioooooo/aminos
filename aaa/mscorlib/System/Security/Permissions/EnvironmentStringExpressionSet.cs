using System;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200060D RID: 1549
	[Serializable]
	internal class EnvironmentStringExpressionSet : StringExpressionSet
	{
		// Token: 0x0600384A RID: 14410 RVA: 0x000BEB6D File Offset: 0x000BDB6D
		public EnvironmentStringExpressionSet()
			: base(true, null, false)
		{
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000BEB78 File Offset: 0x000BDB78
		public EnvironmentStringExpressionSet(string str)
			: base(true, str, false)
		{
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000BEB83 File Offset: 0x000BDB83
		protected override StringExpressionSet CreateNewEmpty()
		{
			return new EnvironmentStringExpressionSet();
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000BEB8A File Offset: 0x000BDB8A
		protected override bool StringSubsetString(string left, string right, bool ignoreCase)
		{
			if (!ignoreCase)
			{
				return string.Compare(left, right, StringComparison.Ordinal) == 0;
			}
			return string.Compare(left, right, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000BEBA6 File Offset: 0x000BDBA6
		protected override string ProcessWholeString(string str)
		{
			return str;
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000BEBA9 File Offset: 0x000BDBA9
		protected override string ProcessSingleString(string str)
		{
			return str;
		}
	}
}
