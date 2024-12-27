using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004D RID: 77
	internal class QilFunction : QilReference
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x00020EB5 File Offset: 0x0001FEB5
		public QilFunction(QilNodeType nodeType, QilNode arguments, QilNode definition, QilNode sideEffects, XmlQueryType resultType)
			: base(nodeType)
		{
			this.arguments = arguments;
			this.definition = definition;
			this.sideEffects = sideEffects;
			this.xmlType = resultType;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00020EDC File Offset: 0x0001FEDC
		public override int Count
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170000BF RID: 191
		public override QilNode this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.arguments;
				case 1:
					return this.definition;
				case 2:
					return this.sideEffects;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.arguments = value;
					return;
				case 1:
					this.definition = value;
					return;
				case 2:
					this.sideEffects = value;
					return;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00020F60 File Offset: 0x0001FF60
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00020F6D File Offset: 0x0001FF6D
		public QilList Arguments
		{
			get
			{
				return (QilList)this.arguments;
			}
			set
			{
				this.arguments = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00020F76 File Offset: 0x0001FF76
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00020F7E File Offset: 0x0001FF7E
		public QilNode Definition
		{
			get
			{
				return this.definition;
			}
			set
			{
				this.definition = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00020F87 File Offset: 0x0001FF87
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x00020F98 File Offset: 0x0001FF98
		public bool MaybeSideEffects
		{
			get
			{
				return this.sideEffects.NodeType == QilNodeType.True;
			}
			set
			{
				this.sideEffects.NodeType = (value ? QilNodeType.True : QilNodeType.False);
			}
		}

		// Token: 0x0400038F RID: 911
		private QilNode arguments;

		// Token: 0x04000390 RID: 912
		private QilNode definition;

		// Token: 0x04000391 RID: 913
		private QilNode sideEffects;
	}
}
