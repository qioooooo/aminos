using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000051 RID: 81
	internal class QilInvokeLateBound : QilBinary
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00021126 File Offset: 0x00020126
		public QilInvokeLateBound(QilNodeType nodeType, QilNode name, QilNode arguments)
			: base(nodeType, name, arguments)
		{
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00021131 File Offset: 0x00020131
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x0002113E File Offset: 0x0002013E
		public QilName Name
		{
			get
			{
				return (QilName)base.Left;
			}
			set
			{
				base.Left = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00021147 File Offset: 0x00020147
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00021154 File Offset: 0x00020154
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
