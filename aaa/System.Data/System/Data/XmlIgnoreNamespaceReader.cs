using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000FD RID: 253
	internal sealed class XmlIgnoreNamespaceReader : XmlNodeReader
	{
		// Token: 0x06000EE0 RID: 3808 RVA: 0x0021017C File Offset: 0x0020F57C
		internal XmlIgnoreNamespaceReader(XmlDocument xdoc, string[] namespacesToIgnore)
			: base(xdoc)
		{
			this.namespacesToIgnore = new List<string>(namespacesToIgnore);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0021019C File Offset: 0x0020F59C
		public override bool MoveToFirstAttribute()
		{
			return base.MoveToFirstAttribute() && ((!this.namespacesToIgnore.Contains(this.NamespaceURI) && (!(this.NamespaceURI == "http://www.w3.org/XML/1998/namespace") || !(this.LocalName != "lang"))) || this.MoveToNextAttribute());
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x002101F4 File Offset: 0x0020F5F4
		public override bool MoveToNextAttribute()
		{
			bool flag;
			bool flag2;
			do
			{
				flag = false;
				flag2 = false;
				if (base.MoveToNextAttribute())
				{
					flag = true;
					if (this.namespacesToIgnore.Contains(this.NamespaceURI) || (this.NamespaceURI == "http://www.w3.org/XML/1998/namespace" && this.LocalName != "lang"))
					{
						flag2 = true;
					}
				}
			}
			while (flag2);
			return flag;
		}

		// Token: 0x04000A99 RID: 2713
		private List<string> namespacesToIgnore;
	}
}
