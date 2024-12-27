using System;
using System.Diagnostics;
using System.Xml.Utils;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005F RID: 95
	internal class QilValidationVisitor : QilScopedVisitor
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x00023158 File Offset: 0x00022158
		[Conditional("DEBUG")]
		public static void Validate(QilNode node)
		{
			new QilValidationVisitor().VisitAssumeReference(node);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00023166 File Offset: 0x00022166
		protected QilValidationVisitor()
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00023184 File Offset: 0x00022184
		[Conditional("DEBUG")]
		internal static void SetError(QilNode n, string message)
		{
			message = Res.GetString("Qil_Validation", new object[] { message });
			string text = n.Annotation as string;
			if (text != null)
			{
				message = text + "\n" + message;
			}
			n.Annotation = message;
		}

		// Token: 0x0400040C RID: 1036
		private SubstitutionList subs = new SubstitutionList();

		// Token: 0x0400040D RID: 1037
		private QilTypeChecker typeCheck = new QilTypeChecker();
	}
}
