using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005C RID: 92
	internal class QilTargetType : QilBinary
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x00022157 File Offset: 0x00021157
		public QilTargetType(QilNodeType nodeType, QilNode expr, QilNode targetType)
			: base(nodeType, expr, targetType)
		{
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00022162 File Offset: 0x00021162
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x0002216A File Offset: 0x0002116A
		public QilNode Source
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00022173 File Offset: 0x00021173
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x0002218A File Offset: 0x0002118A
		public XmlQueryType TargetType
		{
			get
			{
				return (XmlQueryType)((QilLiteral)base.Right).Value;
			}
			set
			{
				((QilLiteral)base.Right).Value = value;
			}
		}
	}
}
