using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000260 RID: 608
	public class XmlSchemaXPath : XmlSchemaAnnotated
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x00083258 File Offset: 0x00082258
		// (set) Token: 0x06001C78 RID: 7288 RVA: 0x00083260 File Offset: 0x00082260
		[XmlAttribute("xpath")]
		[DefaultValue("")]
		public string XPath
		{
			get
			{
				return this.xpath;
			}
			set
			{
				this.xpath = value;
			}
		}

		// Token: 0x04001190 RID: 4496
		private string xpath;
	}
}
