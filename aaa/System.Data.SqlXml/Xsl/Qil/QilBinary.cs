using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000045 RID: 69
	internal class QilBinary : QilNode
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0001F2C8 File Offset: 0x0001E2C8
		public QilBinary(QilNodeType nodeType, QilNode left, QilNode right)
			: base(nodeType)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0001F2DF File Offset: 0x0001E2DF
		public override int Count
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000AA RID: 170
		public override QilNode this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.left;
				case 1:
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
					this.right = value;
					return;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001F34C File Offset: 0x0001E34C
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0001F354 File Offset: 0x0001E354
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0001F35D File Offset: 0x0001E35D
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0001F365 File Offset: 0x0001E365
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

		// Token: 0x0400037F RID: 895
		private QilNode left;

		// Token: 0x04000380 RID: 896
		private QilNode right;
	}
}
