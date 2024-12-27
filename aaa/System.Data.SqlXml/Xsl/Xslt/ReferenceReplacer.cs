using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000FC RID: 252
	internal class ReferenceReplacer : QilReplaceVisitor
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x000358A9 File Offset: 0x000348A9
		public ReferenceReplacer(QilFactory f)
			: base(f)
		{
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000358B2 File Offset: 0x000348B2
		public QilNode Replace(QilNode expr, QilReference lookFor, QilReference replaceBy)
		{
			QilDepthChecker.Check(expr);
			this.lookFor = lookFor;
			this.replaceBy = replaceBy;
			return this.VisitAssumeReference(expr);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000358CF File Offset: 0x000348CF
		protected override QilNode VisitReference(QilNode n)
		{
			if (n != this.lookFor)
			{
				return n;
			}
			return this.replaceBy;
		}

		// Token: 0x040007BF RID: 1983
		private QilReference lookFor;

		// Token: 0x040007C0 RID: 1984
		private QilReference replaceBy;
	}
}
