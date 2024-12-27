using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000179 RID: 377
	internal class EndEvent : Event
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x0004D5D2 File Offset: 0x0004C5D2
		internal EndEvent(XPathNodeType nodeType)
		{
			this.nodeType = nodeType;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0004D5E1 File Offset: 0x0004C5E1
		public override bool Output(Processor processor, ActionFrame frame)
		{
			return processor.EndEvent(this.nodeType);
		}

		// Token: 0x040009F5 RID: 2549
		private XPathNodeType nodeType;
	}
}
