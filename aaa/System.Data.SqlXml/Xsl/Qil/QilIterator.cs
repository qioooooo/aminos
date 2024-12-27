using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000052 RID: 82
	internal class QilIterator : QilReference
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0002115D File Offset: 0x0002015D
		public QilIterator(QilNodeType nodeType, QilNode binding)
			: base(nodeType)
		{
			this.Binding = binding;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0002116D File Offset: 0x0002016D
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000D0 RID: 208
		public override QilNode this[int index]
		{
			get
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				return this.binding;
			}
			set
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				this.binding = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00021193 File Offset: 0x00020193
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0002119B File Offset: 0x0002019B
		public QilNode Binding
		{
			get
			{
				return this.binding;
			}
			set
			{
				this.binding = value;
			}
		}

		// Token: 0x04000395 RID: 917
		private QilNode binding;
	}
}
