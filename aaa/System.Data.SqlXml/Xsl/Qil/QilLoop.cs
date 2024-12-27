using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000055 RID: 85
	internal class QilLoop : QilBinary
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x00021474 File Offset: 0x00020474
		public QilLoop(QilNodeType nodeType, QilNode variable, QilNode body)
			: base(nodeType, variable, body)
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0002147F File Offset: 0x0002047F
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0002148C File Offset: 0x0002048C
		public QilIterator Variable
		{
			get
			{
				return (QilIterator)base.Left;
			}
			set
			{
				base.Left = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00021495 File Offset: 0x00020495
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0002149D File Offset: 0x0002049D
		public QilNode Body
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
