using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x0200030A RID: 778
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	public class XmlElementAttribute : Attribute
	{
		// Token: 0x060024E8 RID: 9448 RVA: 0x000ADD6E File Offset: 0x000ACD6E
		public XmlElementAttribute()
		{
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000ADD7D File Offset: 0x000ACD7D
		public XmlElementAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000ADD93 File Offset: 0x000ACD93
		public XmlElementAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000ADDA9 File Offset: 0x000ACDA9
		public XmlElementAttribute(string elementName, Type type)
		{
			this.elementName = elementName;
			this.type = type;
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000ADDC6 File Offset: 0x000ACDC6
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x000ADDCE File Offset: 0x000ACDCE
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

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x000ADDD7 File Offset: 0x000ACDD7
		// (set) Token: 0x060024EF RID: 9455 RVA: 0x000ADDED File Offset: 0x000ACDED
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

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000ADDF6 File Offset: 0x000ACDF6
		// (set) Token: 0x060024F1 RID: 9457 RVA: 0x000ADDFE File Offset: 0x000ACDFE
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

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x000ADE07 File Offset: 0x000ACE07
		// (set) Token: 0x060024F3 RID: 9459 RVA: 0x000ADE1D File Offset: 0x000ACE1D
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

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000ADE26 File Offset: 0x000ACE26
		// (set) Token: 0x060024F5 RID: 9461 RVA: 0x000ADE2E File Offset: 0x000ACE2E
		public bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
				this.nullableSpecified = true;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000ADE3E File Offset: 0x000ACE3E
		internal bool IsNullableSpecified
		{
			get
			{
				return this.nullableSpecified;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000ADE46 File Offset: 0x000ACE46
		// (set) Token: 0x060024F8 RID: 9464 RVA: 0x000ADE4E File Offset: 0x000ACE4E
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

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x000ADE57 File Offset: 0x000ACE57
		// (set) Token: 0x060024FA RID: 9466 RVA: 0x000ADE5F File Offset: 0x000ACE5F
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

		// Token: 0x04001576 RID: 5494
		private string elementName;

		// Token: 0x04001577 RID: 5495
		private Type type;

		// Token: 0x04001578 RID: 5496
		private string ns;

		// Token: 0x04001579 RID: 5497
		private string dataType;

		// Token: 0x0400157A RID: 5498
		private bool nullable;

		// Token: 0x0400157B RID: 5499
		private bool nullableSpecified;

		// Token: 0x0400157C RID: 5500
		private XmlSchemaForm form;

		// Token: 0x0400157D RID: 5501
		private int order = -1;
	}
}
