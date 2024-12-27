using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000049 RID: 73
	internal class QilDataSource : QilBinary
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x0001F7BF File Offset: 0x0001E7BF
		public QilDataSource(QilNodeType nodeType, QilNode name, QilNode baseUri)
			: base(nodeType, name, baseUri)
		{
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001F7CA File Offset: 0x0001E7CA
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x0001F7D2 File Offset: 0x0001E7D2
		public QilNode Name
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001F7DB File Offset: 0x0001E7DB
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x0001F7E3 File Offset: 0x0001E7E3
		public QilNode BaseUri
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
