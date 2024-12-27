using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000094 RID: 148
	internal class NamespaceFrame
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0000EA24 File Offset: 0x0000DA24
		internal NamespaceFrame()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000EA42 File Offset: 0x0000DA42
		internal void AddRendered(XmlAttribute attr)
		{
			this.m_rendered.Add(Utils.GetNamespacePrefix(attr), attr);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000EA56 File Offset: 0x0000DA56
		internal XmlAttribute GetRendered(string nsPrefix)
		{
			return (XmlAttribute)this.m_rendered[nsPrefix];
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000EA69 File Offset: 0x0000DA69
		internal void AddUnrendered(XmlAttribute attr)
		{
			this.m_unrendered.Add(Utils.GetNamespacePrefix(attr), attr);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000EA7D File Offset: 0x0000DA7D
		internal XmlAttribute GetUnrendered(string nsPrefix)
		{
			return (XmlAttribute)this.m_unrendered[nsPrefix];
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000EA90 File Offset: 0x0000DA90
		internal Hashtable GetUnrendered()
		{
			return this.m_unrendered;
		}

		// Token: 0x040004E7 RID: 1255
		private Hashtable m_rendered = new Hashtable();

		// Token: 0x040004E8 RID: 1256
		private Hashtable m_unrendered = new Hashtable();
	}
}
