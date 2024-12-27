using System;
using System.Reflection;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000050 RID: 80
	internal class QilInvokeEarlyBound : QilTernary
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x000210BB File Offset: 0x000200BB
		public QilInvokeEarlyBound(QilNodeType nodeType, QilNode name, QilNode method, QilNode arguments, XmlQueryType resultType)
			: base(nodeType, name, method, arguments)
		{
			this.xmlType = resultType;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000210D0 File Offset: 0x000200D0
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x000210DD File Offset: 0x000200DD
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000210E6 File Offset: 0x000200E6
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x000210FD File Offset: 0x000200FD
		public MethodInfo ClrMethod
		{
			get
			{
				return (MethodInfo)((QilLiteral)base.Center).Value;
			}
			set
			{
				((QilLiteral)base.Center).Value = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00021110 File Offset: 0x00020110
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0002111D File Offset: 0x0002011D
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
