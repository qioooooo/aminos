using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005E RID: 94
	internal class QilUnary : QilNode
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x00023111 File Offset: 0x00022111
		public QilUnary(QilNodeType nodeType, QilNode child)
			: base(nodeType)
		{
			this.child = child;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00023121 File Offset: 0x00022121
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000E9 RID: 233
		public override QilNode this[int index]
		{
			get
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				return this.child;
			}
			set
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				this.child = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00023147 File Offset: 0x00022147
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0002314F File Offset: 0x0002214F
		public QilNode Child
		{
			get
			{
				return this.child;
			}
			set
			{
				this.child = value;
			}
		}

		// Token: 0x0400040B RID: 1035
		private QilNode child;
	}
}
