using System;

namespace System.Xml.Schema
{
	// Token: 0x02000183 RID: 387
	internal class AutoValidator : BaseValidator
	{
		// Token: 0x06001489 RID: 5257 RVA: 0x00057C86 File Offset: 0x00056C86
		public AutoValidator(XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, ValidationEventHandler eventHandler)
			: base(reader, schemaCollection, eventHandler)
		{
			this.schemaInfo = new SchemaInfo();
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x00057C9C File Offset: 0x00056C9C
		public override bool PreserveWhitespace
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00057CA0 File Offset: 0x00056CA0
		public override void Validate()
		{
			switch (this.DetectValidationType())
			{
			case ValidationType.Auto:
			case ValidationType.DTD:
				break;
			case ValidationType.XDR:
				this.reader.Validator = new XdrValidator(this);
				this.reader.Validator.Validate();
				return;
			case ValidationType.Schema:
				this.reader.Validator = new XsdValidator(this);
				this.reader.Validator.Validate();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00057D12 File Offset: 0x00056D12
		public override void CompleteValidation()
		{
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00057D14 File Offset: 0x00056D14
		public override object FindId(string name)
		{
			return null;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00057D18 File Offset: 0x00056D18
		private ValidationType DetectValidationType()
		{
			if (this.reader.Schemas != null && this.reader.Schemas.Count > 0)
			{
				XmlSchemaCollectionEnumerator enumerator = this.reader.Schemas.GetEnumerator();
				while (enumerator.MoveNext())
				{
					XmlSchemaCollectionNode currentNode = enumerator.CurrentNode;
					SchemaInfo schemaInfo = currentNode.SchemaInfo;
					if (schemaInfo.SchemaType == SchemaType.XSD)
					{
						return ValidationType.Schema;
					}
					if (schemaInfo.SchemaType == SchemaType.XDR)
					{
						return ValidationType.XDR;
					}
				}
			}
			if (this.reader.NodeType == XmlNodeType.Element)
			{
				SchemaType schemaType = base.SchemaNames.SchemaTypeFromRoot(this.reader.LocalName, this.reader.NamespaceURI);
				if (schemaType == SchemaType.XSD)
				{
					return ValidationType.Schema;
				}
				if (schemaType == SchemaType.XDR)
				{
					return ValidationType.XDR;
				}
				int attributeCount = this.reader.AttributeCount;
				for (int i = 0; i < attributeCount; i++)
				{
					this.reader.MoveToAttribute(i);
					string namespaceURI = this.reader.NamespaceURI;
					string localName = this.reader.LocalName;
					if (Ref.Equal(namespaceURI, base.SchemaNames.NsXmlNs))
					{
						if (XdrBuilder.IsXdrSchema(this.reader.Value))
						{
							this.reader.MoveToElement();
							return ValidationType.XDR;
						}
					}
					else
					{
						if (Ref.Equal(namespaceURI, base.SchemaNames.NsXsi))
						{
							this.reader.MoveToElement();
							return ValidationType.Schema;
						}
						if (Ref.Equal(namespaceURI, base.SchemaNames.QnDtDt.Namespace) && Ref.Equal(localName, base.SchemaNames.QnDtDt.Name))
						{
							this.reader.SchemaTypeObject = XmlSchemaDatatype.FromXdrName(this.reader.Value);
							this.reader.MoveToElement();
							return ValidationType.XDR;
						}
					}
				}
				if (attributeCount > 0)
				{
					this.reader.MoveToElement();
				}
			}
			return ValidationType.Auto;
		}

		// Token: 0x04000C78 RID: 3192
		private const string x_schema = "x-schema";
	}
}
