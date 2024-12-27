using System;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008E0 RID: 2272
	internal static class NameSpaceExtractor
	{
		// Token: 0x060052D7 RID: 21207 RVA: 0x0012C80C File Offset: 0x0012B80C
		public static string ExtractNameSpace(string FullyQualifiedTypeName)
		{
			int num = FullyQualifiedTypeName.LastIndexOf(NameSpaceExtractor.NameSpaceSeperator);
			if (num == -1)
			{
				return "";
			}
			return FullyQualifiedTypeName.Substring(0, num);
		}

		// Token: 0x04002ABD RID: 10941
		private static char NameSpaceSeperator = '.';
	}
}
