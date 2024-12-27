using System;

namespace System.Xml.Schema
{
	// Token: 0x02000266 RID: 614
	public class XmlSchemaInfo : IXmlSchemaInfo
	{
		// Token: 0x06001C89 RID: 7305 RVA: 0x00083318 File Offset: 0x00082318
		public XmlSchemaInfo()
		{
			this.Clear();
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00083326 File Offset: 0x00082326
		internal XmlSchemaInfo(XmlSchemaValidity validity)
			: this()
		{
			this.validity = validity;
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00083335 File Offset: 0x00082335
		// (set) Token: 0x06001C8C RID: 7308 RVA: 0x0008333D File Offset: 0x0008233D
		public XmlSchemaValidity Validity
		{
			get
			{
				return this.validity;
			}
			set
			{
				this.validity = value;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x00083346 File Offset: 0x00082346
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x0008334E File Offset: 0x0008234E
		public bool IsDefault
		{
			get
			{
				return this.isDefault;
			}
			set
			{
				this.isDefault = value;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x00083357 File Offset: 0x00082357
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x0008335F File Offset: 0x0008235F
		public bool IsNil
		{
			get
			{
				return this.isNil;
			}
			set
			{
				this.isNil = value;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x00083368 File Offset: 0x00082368
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x00083370 File Offset: 0x00082370
		public XmlSchemaSimpleType MemberType
		{
			get
			{
				return this.memberType;
			}
			set
			{
				this.memberType = value;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00083379 File Offset: 0x00082379
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x00083381 File Offset: 0x00082381
		public XmlSchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
			set
			{
				this.schemaType = value;
				if (this.schemaType != null)
				{
					this.contentType = this.schemaType.SchemaContentType;
					return;
				}
				this.contentType = XmlSchemaContentType.Empty;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x000833AB File Offset: 0x000823AB
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x000833B3 File Offset: 0x000823B3
		public XmlSchemaElement SchemaElement
		{
			get
			{
				return this.schemaElement;
			}
			set
			{
				this.schemaElement = value;
				if (value != null)
				{
					this.schemaAttribute = null;
				}
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000833C6 File Offset: 0x000823C6
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x000833CE File Offset: 0x000823CE
		public XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				return this.schemaAttribute;
			}
			set
			{
				this.schemaAttribute = value;
				if (value != null)
				{
					this.schemaElement = null;
				}
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x000833E1 File Offset: 0x000823E1
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x000833E9 File Offset: 0x000823E9
		public XmlSchemaContentType ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				this.contentType = value;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000833F2 File Offset: 0x000823F2
		internal XmlSchemaType XmlType
		{
			get
			{
				if (this.memberType != null)
				{
					return this.memberType;
				}
				return this.schemaType;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00083409 File Offset: 0x00082409
		internal bool HasDefaultValue
		{
			get
			{
				return this.schemaElement != null && this.schemaElement.ElementDecl.DefaultValueTyped != null;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x0008342B File Offset: 0x0008242B
		internal bool IsUnionType
		{
			get
			{
				return this.schemaType != null && this.schemaType.Datatype != null && this.schemaType.Datatype.Variety == XmlSchemaDatatypeVariety.Union;
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x00083457 File Offset: 0x00082457
		internal void Clear()
		{
			this.isNil = false;
			this.isDefault = false;
			this.schemaType = null;
			this.schemaElement = null;
			this.schemaAttribute = null;
			this.memberType = null;
			this.validity = XmlSchemaValidity.NotKnown;
			this.contentType = XmlSchemaContentType.Empty;
		}

		// Token: 0x04001195 RID: 4501
		private bool isDefault;

		// Token: 0x04001196 RID: 4502
		private bool isNil;

		// Token: 0x04001197 RID: 4503
		private XmlSchemaElement schemaElement;

		// Token: 0x04001198 RID: 4504
		private XmlSchemaAttribute schemaAttribute;

		// Token: 0x04001199 RID: 4505
		private XmlSchemaType schemaType;

		// Token: 0x0400119A RID: 4506
		private XmlSchemaSimpleType memberType;

		// Token: 0x0400119B RID: 4507
		private XmlSchemaValidity validity;

		// Token: 0x0400119C RID: 4508
		private XmlSchemaContentType contentType;
	}
}
