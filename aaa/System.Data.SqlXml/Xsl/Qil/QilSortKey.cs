using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005A RID: 90
	internal class QilSortKey : QilBinary
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x000220FD File Offset: 0x000210FD
		public QilSortKey(QilNodeType nodeType, QilNode key, QilNode collation)
			: base(nodeType, key, collation)
		{
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00022108 File Offset: 0x00021108
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x00022110 File Offset: 0x00021110
		public QilNode Key
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00022119 File Offset: 0x00021119
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00022121 File Offset: 0x00021121
		public QilNode Collation
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
