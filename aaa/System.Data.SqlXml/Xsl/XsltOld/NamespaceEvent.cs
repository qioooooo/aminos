using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000180 RID: 384
	internal class NamespaceEvent : Event
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0004F146 File Offset: 0x0004E146
		public NamespaceEvent(NavigatorInput input)
		{
			this.namespaceUri = input.Value;
			this.name = input.LocalName;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0004F168 File Offset: 0x0004E168
		public override void ReplaceNamespaceAlias(Compiler compiler)
		{
			if (this.namespaceUri.Length != 0)
			{
				NamespaceInfo namespaceInfo = compiler.FindNamespaceAlias(this.namespaceUri);
				if (namespaceInfo != null)
				{
					this.namespaceUri = namespaceInfo.nameSpace;
					if (namespaceInfo.prefix != null)
					{
						this.name = namespaceInfo.prefix;
					}
				}
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0004F1B2 File Offset: 0x0004E1B2
		public override bool Output(Processor processor, ActionFrame frame)
		{
			processor.BeginEvent(XPathNodeType.Namespace, null, this.name, this.namespaceUri, false);
			processor.EndEvent(XPathNodeType.Namespace);
			return true;
		}

		// Token: 0x04000ADC RID: 2780
		private string namespaceUri;

		// Token: 0x04000ADD RID: 2781
		private string name;
	}
}
