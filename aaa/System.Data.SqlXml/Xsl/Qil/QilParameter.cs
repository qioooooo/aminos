using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000058 RID: 88
	internal class QilParameter : QilIterator
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x00021606 File Offset: 0x00020606
		public QilParameter(QilNodeType nodeType, QilNode defaultValue, QilNode name, XmlQueryType xmlType)
			: base(nodeType, defaultValue)
		{
			this.name = name;
			this.xmlType = xmlType;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0002161F File Offset: 0x0002061F
		public override int Count
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170000DD RID: 221
		public override QilNode this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return base.Binding;
				case 1:
					return this.name;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					base.Binding = value;
					return;
				case 1:
					this.name = value;
					return;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0002168C File Offset: 0x0002068C
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00021694 File Offset: 0x00020694
		public QilNode DefaultValue
		{
			get
			{
				return base.Binding;
			}
			set
			{
				base.Binding = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0002169D File Offset: 0x0002069D
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x000216AA File Offset: 0x000206AA
		public QilName Name
		{
			get
			{
				return (QilName)this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x04000408 RID: 1032
		private QilNode name;
	}
}
