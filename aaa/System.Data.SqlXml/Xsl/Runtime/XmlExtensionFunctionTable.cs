using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A5 RID: 165
	internal class XmlExtensionFunctionTable
	{
		// Token: 0x060007D3 RID: 2003 RVA: 0x00027E15 File Offset: 0x00026E15
		public XmlExtensionFunctionTable()
		{
			this.table = new Dictionary<XmlExtensionFunction, XmlExtensionFunction>();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00027E28 File Offset: 0x00026E28
		public XmlExtensionFunction Bind(string name, string namespaceUri, int numArgs, Type objectType, BindingFlags flags)
		{
			if (this.funcCached == null)
			{
				this.funcCached = new XmlExtensionFunction();
			}
			this.funcCached.Init(name, namespaceUri, numArgs, objectType, flags);
			XmlExtensionFunction xmlExtensionFunction;
			if (!this.table.TryGetValue(this.funcCached, out xmlExtensionFunction))
			{
				xmlExtensionFunction = this.funcCached;
				this.funcCached = null;
				xmlExtensionFunction.Bind();
				this.table.Add(xmlExtensionFunction, xmlExtensionFunction);
			}
			return xmlExtensionFunction;
		}

		// Token: 0x04000554 RID: 1364
		private Dictionary<XmlExtensionFunction, XmlExtensionFunction> table;

		// Token: 0x04000555 RID: 1365
		private XmlExtensionFunction funcCached;
	}
}
