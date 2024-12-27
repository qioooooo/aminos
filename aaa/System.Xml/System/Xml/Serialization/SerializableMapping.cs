using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002D3 RID: 723
	internal class SerializableMapping : SpecialMapping
	{
		// Token: 0x0600222A RID: 8746 RVA: 0x0009FF09 File Offset: 0x0009EF09
		internal SerializableMapping()
		{
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x0009FF18 File Offset: 0x0009EF18
		internal SerializableMapping(MethodInfo getSchemaMethod, bool any, string ns)
		{
			this.getSchemaMethod = getSchemaMethod;
			this.any = any;
			base.Namespace = ns;
			this.needSchema = getSchemaMethod != null;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x0009FF49 File Offset: 0x0009EF49
		internal SerializableMapping(XmlQualifiedName xsiType, XmlSchemaSet schemas)
		{
			this.xsiType = xsiType;
			this.schemas = schemas;
			base.TypeName = xsiType.Name;
			base.Namespace = xsiType.Namespace;
			this.needSchema = false;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x0009FF88 File Offset: 0x0009EF88
		internal void SetBaseMapping(SerializableMapping mapping)
		{
			this.baseMapping = mapping;
			if (this.baseMapping != null)
			{
				this.nextDerivedMapping = this.baseMapping.derivedMappings;
				this.baseMapping.derivedMappings = this;
				if (this == this.nextDerivedMapping)
				{
					throw new InvalidOperationException(Res.GetString("XmlCircularDerivation", new object[] { base.TypeDesc.FullName }));
				}
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0009FFF0 File Offset: 0x0009EFF0
		internal bool IsAny
		{
			get
			{
				if (this.any)
				{
					return true;
				}
				if (this.getSchemaMethod == null)
				{
					return false;
				}
				if (this.needSchema && typeof(XmlSchemaType).IsAssignableFrom(this.getSchemaMethod.ReturnType))
				{
					return false;
				}
				this.RetrieveSerializableSchema();
				return this.any;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x000A0044 File Offset: 0x0009F044
		internal string NamespaceList
		{
			get
			{
				this.RetrieveSerializableSchema();
				if (this.namespaces == null)
				{
					if (this.schemas != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						foreach (object obj in this.schemas.Schemas())
						{
							XmlSchema xmlSchema = (XmlSchema)obj;
							if (xmlSchema.TargetNamespace != null && xmlSchema.TargetNamespace.Length > 0)
							{
								if (stringBuilder.Length > 0)
								{
									stringBuilder.Append(" ");
								}
								stringBuilder.Append(xmlSchema.TargetNamespace);
							}
						}
						this.namespaces = stringBuilder.ToString();
					}
					else
					{
						this.namespaces = string.Empty;
					}
				}
				return this.namespaces;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000A0114 File Offset: 0x0009F114
		internal SerializableMapping DerivedMappings
		{
			get
			{
				return this.derivedMappings;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x000A011C File Offset: 0x0009F11C
		internal SerializableMapping NextDerivedMapping
		{
			get
			{
				return this.nextDerivedMapping;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x000A0124 File Offset: 0x0009F124
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x000A012C File Offset: 0x0009F12C
		internal SerializableMapping Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x000A0135 File Offset: 0x0009F135
		// (set) Token: 0x06002235 RID: 8757 RVA: 0x000A013D File Offset: 0x0009F13D
		internal Type Type
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

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000A0146 File Offset: 0x0009F146
		internal XmlSchemaSet Schemas
		{
			get
			{
				this.RetrieveSerializableSchema();
				return this.schemas;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002237 RID: 8759 RVA: 0x000A0154 File Offset: 0x0009F154
		internal XmlSchema Schema
		{
			get
			{
				this.RetrieveSerializableSchema();
				return this.schema;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x000A0164 File Offset: 0x0009F164
		internal XmlQualifiedName XsiType
		{
			get
			{
				if (!this.needSchema)
				{
					return this.xsiType;
				}
				if (this.getSchemaMethod == null)
				{
					return null;
				}
				if (typeof(XmlSchemaType).IsAssignableFrom(this.getSchemaMethod.ReturnType))
				{
					return null;
				}
				this.RetrieveSerializableSchema();
				return this.xsiType;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x000A01B4 File Offset: 0x0009F1B4
		internal XmlSchemaType XsdType
		{
			get
			{
				this.RetrieveSerializableSchema();
				return this.xsdType;
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000A01C4 File Offset: 0x0009F1C4
		internal static void ValidationCallbackWithErrorCode(object sender, ValidationEventArgs args)
		{
			if (args.Severity == XmlSeverityType.Error)
			{
				throw new InvalidOperationException(Res.GetString("XmlSerializableSchemaError", new object[]
				{
					typeof(IXmlSerializable).Name,
					args.Message
				}));
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000A020C File Offset: 0x0009F20C
		internal void CheckDuplicateElement(XmlSchemaElement element, string elementNs)
		{
			if (element == null)
			{
				return;
			}
			if (element.Parent == null || !(element.Parent is XmlSchema))
			{
				return;
			}
			XmlSchemaObjectTable xmlSchemaObjectTable;
			if (this.Schema != null && this.Schema.TargetNamespace == elementNs)
			{
				XmlSchemas.Preprocess(this.Schema);
				xmlSchemaObjectTable = this.Schema.Elements;
			}
			else
			{
				if (this.Schemas == null)
				{
					return;
				}
				xmlSchemaObjectTable = this.Schemas.GlobalElements;
			}
			foreach (object obj in xmlSchemaObjectTable.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				if (xmlSchemaElement.Name == element.Name && xmlSchemaElement.QualifiedName.Namespace == elementNs)
				{
					if (this.Match(xmlSchemaElement, element))
					{
						break;
					}
					throw new InvalidOperationException(Res.GetString("XmlSerializableRootDupName", new object[]
					{
						this.getSchemaMethod.DeclaringType.FullName,
						xmlSchemaElement.Name,
						elementNs
					}));
				}
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000A0334 File Offset: 0x0009F334
		private bool Match(XmlSchemaElement e1, XmlSchemaElement e2)
		{
			return e1.IsNillable == e2.IsNillable && !(e1.RefName != e2.RefName) && e1.SchemaType == e2.SchemaType && !(e1.SchemaTypeName != e2.SchemaTypeName) && !(e1.MinOccurs != e2.MinOccurs) && !(e1.MaxOccurs != e2.MaxOccurs) && e1.IsAbstract == e2.IsAbstract && !(e1.DefaultValue != e2.DefaultValue) && !(e1.SubstitutionGroup != e2.SubstitutionGroup);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000A03F0 File Offset: 0x0009F3F0
		private void RetrieveSerializableSchema()
		{
			if (this.needSchema)
			{
				this.needSchema = false;
				if (this.getSchemaMethod != null)
				{
					if (this.schemas == null)
					{
						this.schemas = new XmlSchemaSet();
					}
					object obj = this.getSchemaMethod.Invoke(null, new object[] { this.schemas });
					this.xsiType = XmlQualifiedName.Empty;
					if (obj != null)
					{
						if (typeof(XmlSchemaType).IsAssignableFrom(this.getSchemaMethod.ReturnType))
						{
							this.xsdType = (XmlSchemaType)obj;
							this.xsiType = this.xsdType.QualifiedName;
						}
						else
						{
							if (!typeof(XmlQualifiedName).IsAssignableFrom(this.getSchemaMethod.ReturnType))
							{
								throw new InvalidOperationException(Res.GetString("XmlGetSchemaMethodReturnType", new object[]
								{
									this.type.Name,
									this.getSchemaMethod.Name,
									typeof(XmlSchemaProviderAttribute).Name,
									typeof(XmlQualifiedName).FullName
								}));
							}
							this.xsiType = (XmlQualifiedName)obj;
							if (this.xsiType.IsEmpty)
							{
								throw new InvalidOperationException(Res.GetString("XmlGetSchemaEmptyTypeName", new object[]
								{
									this.type.FullName,
									this.getSchemaMethod.Name
								}));
							}
						}
					}
					else
					{
						this.any = true;
					}
					this.schemas.ValidationEventHandler += SerializableMapping.ValidationCallbackWithErrorCode;
					this.schemas.Compile();
					if (!this.xsiType.IsEmpty && this.xsiType.Namespace != "http://www.w3.org/2001/XMLSchema")
					{
						ArrayList arrayList = (ArrayList)this.schemas.Schemas(this.xsiType.Namespace);
						if (arrayList.Count == 0)
						{
							throw new InvalidOperationException(Res.GetString("XmlMissingSchema", new object[] { this.xsiType.Namespace }));
						}
						if (arrayList.Count > 1)
						{
							throw new InvalidOperationException(Res.GetString("XmlGetSchemaInclude", new object[]
							{
								this.xsiType.Namespace,
								this.getSchemaMethod.DeclaringType.FullName,
								this.getSchemaMethod.Name
							}));
						}
						XmlSchema xmlSchema = (XmlSchema)arrayList[0];
						if (xmlSchema == null)
						{
							throw new InvalidOperationException(Res.GetString("XmlMissingSchema", new object[] { this.xsiType.Namespace }));
						}
						this.xsdType = (XmlSchemaType)xmlSchema.SchemaTypes[this.xsiType];
						if (this.xsdType == null)
						{
							throw new InvalidOperationException(Res.GetString("XmlGetSchemaTypeMissing", new object[]
							{
								this.getSchemaMethod.DeclaringType.FullName,
								this.getSchemaMethod.Name,
								this.xsiType.Name,
								this.xsiType.Namespace
							}));
						}
						this.xsdType = ((this.xsdType.Redefined != null) ? this.xsdType.Redefined : this.xsdType);
						return;
					}
				}
				else
				{
					IXmlSerializable xmlSerializable = (IXmlSerializable)Activator.CreateInstance(this.type);
					this.schema = xmlSerializable.GetSchema();
					if (this.schema != null && (this.schema.Id == null || this.schema.Id.Length == 0))
					{
						throw new InvalidOperationException(Res.GetString("XmlSerializableNameMissing1", new object[] { this.type.FullName }));
					}
				}
			}
		}

		// Token: 0x0400149F RID: 5279
		private XmlSchema schema;

		// Token: 0x040014A0 RID: 5280
		private Type type;

		// Token: 0x040014A1 RID: 5281
		private bool needSchema = true;

		// Token: 0x040014A2 RID: 5282
		private MethodInfo getSchemaMethod;

		// Token: 0x040014A3 RID: 5283
		private XmlQualifiedName xsiType;

		// Token: 0x040014A4 RID: 5284
		private XmlSchemaType xsdType;

		// Token: 0x040014A5 RID: 5285
		private XmlSchemaSet schemas;

		// Token: 0x040014A6 RID: 5286
		private bool any;

		// Token: 0x040014A7 RID: 5287
		private string namespaces;

		// Token: 0x040014A8 RID: 5288
		private SerializableMapping baseMapping;

		// Token: 0x040014A9 RID: 5289
		private SerializableMapping derivedMappings;

		// Token: 0x040014AA RID: 5290
		private SerializableMapping nextDerivedMapping;

		// Token: 0x040014AB RID: 5291
		private SerializableMapping next;
	}
}
