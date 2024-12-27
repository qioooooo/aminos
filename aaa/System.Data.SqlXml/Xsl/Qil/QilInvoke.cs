using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004E RID: 78
	internal class QilInvoke : QilBinary
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x00020FAE File Offset: 0x0001FFAE
		public QilInvoke(QilNodeType nodeType, QilNode function, QilNode arguments)
			: base(nodeType, function, arguments)
		{
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00020FB9 File Offset: 0x0001FFB9
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x00020FC6 File Offset: 0x0001FFC6
		public QilFunction Function
		{
			get
			{
				return (QilFunction)base.Left;
			}
			set
			{
				base.Left = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00020FCF File Offset: 0x0001FFCF
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00020FDC File Offset: 0x0001FFDC
		public QilList Arguments
		{
			get
			{
				return (QilList)base.Right;
			}
			set
			{
				base.Right = value;
			}
		}
	}
}
