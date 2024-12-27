using System;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000648 RID: 1608
	internal static class CommonConfigurationStrings
	{
		// Token: 0x060031CA RID: 12746 RVA: 0x000D4BA8 File Offset: 0x000D3BA8
		private static string GetSectionPath(string sectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { sectionName });
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000D4BD0 File Offset: 0x000D3BD0
		private static string GetSectionPath(string sectionName, string subSectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { sectionName, subSectionName });
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x000D4BFC File Offset: 0x000D3BFC
		internal static string UriSectionPath
		{
			get
			{
				return CommonConfigurationStrings.GetSectionPath("uri");
			}
		}

		// Token: 0x04002EE0 RID: 12000
		internal const string UriSectionName = "uri";

		// Token: 0x04002EE1 RID: 12001
		internal const string IriParsing = "iriParsing";

		// Token: 0x04002EE2 RID: 12002
		internal const string Idn = "idn";

		// Token: 0x04002EE3 RID: 12003
		internal const string Enabled = "enabled";
	}
}
