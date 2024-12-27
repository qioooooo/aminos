using System;
using System.Collections.Generic;
using System.Xml.XmlConfiguration;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000060 RID: 96
	internal class QilDepthChecker
	{
		// Token: 0x06000696 RID: 1686 RVA: 0x000231CD File Offset: 0x000221CD
		public static void Check(QilNode input)
		{
			if (XsltConfigSection.LimitXPathComplexity)
			{
				new QilDepthChecker().Check(input, 0);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000231E4 File Offset: 0x000221E4
		private void Check(QilNode input, int depth)
		{
			if (depth > 800)
			{
				throw XsltException.Create("Xslt_CompileError2", new string[0]);
			}
			if (input is QilReference)
			{
				if (this.visitedRef.ContainsKey(input))
				{
					return;
				}
				this.visitedRef[input] = true;
			}
			int num = depth + 1;
			for (int i = 0; i < input.Count; i++)
			{
				QilNode qilNode = input[i];
				if (qilNode != null)
				{
					this.Check(qilNode, num);
				}
			}
		}

		// Token: 0x0400040E RID: 1038
		private const int MAX_QIL_DEPTH = 800;

		// Token: 0x0400040F RID: 1039
		private Dictionary<QilNode, bool> visitedRef = new Dictionary<QilNode, bool>();
	}
}
