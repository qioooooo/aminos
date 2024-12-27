using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002FF RID: 767
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
	public class XmlArrayAttribute : Attribute
	{
		// Token: 0x060023E3 RID: 9187 RVA: 0x000AA381 File Offset: 0x000A9381
		public XmlArrayAttribute()
		{
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000AA390 File Offset: 0x000A9390
		public XmlArrayAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x000AA3A6 File Offset: 0x000A93A6
		// (set) Token: 0x060023E6 RID: 9190 RVA: 0x000AA3BC File Offset: 0x000A93BC
		public string ElementName
		{
			get
			{
				if (this.elementName != null)
				{
					return this.elementName;
				}
				return string.Empty;
			}
			set
			{
				this.elementName = value;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000AA3C5 File Offset: 0x000A93C5
		// (set) Token: 0x060023E8 RID: 9192 RVA: 0x000AA3CD File Offset: 0x000A93CD
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

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x000AA3D6 File Offset: 0x000A93D6
		// (set) Token: 0x060023EA RID: 9194 RVA: 0x000AA3DE File Offset: 0x000A93DE
		public bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000AA3E7 File Offset: 0x000A93E7
		// (set) Token: 0x060023EC RID: 9196 RVA: 0x000AA3EF File Offset: 0x000A93EF
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

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000AA3F8 File Offset: 0x000A93F8
		// (set) Token: 0x060023EE RID: 9198 RVA: 0x000AA400 File Offset: 0x000A9400
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("XmlDisallowNegativeValues"), "Order");
				}
				this.order = value;
			}
		}

		// Token: 0x04001540 RID: 5440
		private string elementName;

		// Token: 0x04001541 RID: 5441
		private string ns;

		// Token: 0x04001542 RID: 5442
		private bool nullable;

		// Token: 0x04001543 RID: 5443
		private XmlSchemaForm form;

		// Token: 0x04001544 RID: 5444
		private int order = -1;
	}
}
