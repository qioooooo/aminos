using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x02000302 RID: 770
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class XmlAttributeAttribute : Attribute
	{
		// Token: 0x0600240B RID: 9227 RVA: 0x000AA580 File Offset: 0x000A9580
		public XmlAttributeAttribute()
		{
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000AA588 File Offset: 0x000A9588
		public XmlAttributeAttribute(string attributeName)
		{
			this.attributeName = attributeName;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000AA597 File Offset: 0x000A9597
		public XmlAttributeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000AA5A6 File Offset: 0x000A95A6
		public XmlAttributeAttribute(string attributeName, Type type)
		{
			this.attributeName = attributeName;
			this.type = type;
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x000AA5BC File Offset: 0x000A95BC
		// (set) Token: 0x06002410 RID: 9232 RVA: 0x000AA5C4 File Offset: 0x000A95C4
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000AA5CD File Offset: 0x000A95CD
		// (set) Token: 0x06002412 RID: 9234 RVA: 0x000AA5E3 File Offset: 0x000A95E3
		public string AttributeName
		{
			get
			{
				if (this.attributeName != null)
				{
					return this.attributeName;
				}
				return string.Empty;
			}
			set
			{
				this.attributeName = value;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000AA5EC File Offset: 0x000A95EC
		// (set) Token: 0x06002414 RID: 9236 RVA: 0x000AA5F4 File Offset: 0x000A95F4
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x000AA5FD File Offset: 0x000A95FD
		// (set) Token: 0x06002416 RID: 9238 RVA: 0x000AA613 File Offset: 0x000A9613
		public string DataType
		{
			get
			{
				if (this.dataType != null)
				{
					return this.dataType;
				}
				return string.Empty;
			}
			set
			{
				this.dataType = value;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x000AA61C File Offset: 0x000A961C
		// (set) Token: 0x06002418 RID: 9240 RVA: 0x000AA624 File Offset: 0x000A9624
		public XmlSchemaForm Form
		{
			get
			{
				return this.form;
			}
			set
			{
				this.form = value;
			}
		}

		// Token: 0x0400154D RID: 5453
		private string attributeName;

		// Token: 0x0400154E RID: 5454
		private Type type;

		// Token: 0x0400154F RID: 5455
		private string ns;

		// Token: 0x04001550 RID: 5456
		private string dataType;

		// Token: 0x04001551 RID: 5457
		private XmlSchemaForm form;
	}
}
