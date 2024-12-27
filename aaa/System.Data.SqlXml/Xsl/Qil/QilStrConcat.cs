using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005B RID: 91
	internal class QilStrConcat : QilBinary
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x0002212A File Offset: 0x0002112A
		public QilStrConcat(QilNodeType nodeType, QilNode delimiter, QilNode values)
			: base(nodeType, delimiter, values)
		{
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00022135 File Offset: 0x00021135
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x0002213D File Offset: 0x0002113D
		public QilNode Delimiter
		{
			get
			{
				return base.Left;
			}
			set
			{
				base.Left = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00022146 File Offset: 0x00021146
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x0002214E File Offset: 0x0002114E
		public QilNode Values
		{
			get
			{
				return base.Right;
			}
			set
			{
				base.Right = value;
			}
		}
	}
}
