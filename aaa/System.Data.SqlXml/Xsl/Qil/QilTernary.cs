using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004F RID: 79
	internal class QilTernary : QilNode
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x00020FE5 File Offset: 0x0001FFE5
		public QilTernary(QilNodeType nodeType, QilNode left, QilNode center, QilNode right)
			: base(nodeType)
		{
			this.left = left;
			this.center = center;
			this.right = right;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00021004 File Offset: 0x00020004
		public override int Count
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170000C6 RID: 198
		public override QilNode this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.left;
				case 1:
					return this.center;
				case 2:
					return this.right;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.left = value;
					return;
				case 1:
					this.center = value;
					return;
				case 2:
					this.right = value;
					return;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00021088 File Offset: 0x00020088
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x00021090 File Offset: 0x00020090
		public QilNode Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00021099 File Offset: 0x00020099
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x000210A1 File Offset: 0x000200A1
		public QilNode Center
		{
			get
			{
				return this.center;
			}
			set
			{
				this.center = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x000210AA File Offset: 0x000200AA
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x000210B2 File Offset: 0x000200B2
		public QilNode Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		// Token: 0x04000392 RID: 914
		private QilNode left;

		// Token: 0x04000393 RID: 915
		private QilNode center;

		// Token: 0x04000394 RID: 916
		private QilNode right;
	}
}
