using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200024A RID: 586
	public class XmlSchemaElement : XmlSchemaParticle
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00082840 File Offset: 0x00081840
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00082848 File Offset: 0x00081848
		[DefaultValue(false)]
		[XmlAttribute("abstract")]
		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
			set
			{
				this.isAbstract = value;
				this.hasAbstractAttribute = true;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00082858 File Offset: 0x00081858
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x00082860 File Offset: 0x00081860
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("block")]
		public XmlSchemaDerivationMethod Block
		{
			get
			{
				return this.block;
			}
			set
			{
				this.block = value;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x00082869 File Offset: 0x00081869
		// (set) Token: 0x06001BEA RID: 7146 RVA: 0x00082871 File Offset: 0x00081871
		[XmlAttribute("default")]
		[DefaultValue(null)]
		public string DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0008287A File Offset: 0x0008187A
		// (set) Token: 0x06001BEC RID: 7148 RVA: 0x00082882 File Offset: 0x00081882
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("final")]
		public XmlSchemaDerivationMethod Final
		{
			get
			{
				return this.final;
			}
			set
			{
				this.final = value;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x0008288B File Offset: 0x0008188B
		// (set) Token: 0x06001BEE RID: 7150 RVA: 0x00082893 File Offset: 0x00081893
		[DefaultValue(null)]
		[XmlAttribute("fixed")]
		public string FixedValue
		{
			get
			{
				return this.fixedValue;
			}
			set
			{
				this.fixedValue = value;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0008289C File Offset: 0x0008189C
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x000828A4 File Offset: 0x000818A4
		[XmlAttribute("form")]
		[DefaultValue(XmlSchemaForm.None)]
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

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x000828AD File Offset: 0x000818AD
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x000828B5 File Offset: 0x000818B5
		[DefaultValue("")]
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x000828BE File Offset: 0x000818BE
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x000828C6 File Offset: 0x000818C6
		[DefaultValue(false)]
		[XmlAttribute("nillable")]
		public bool IsNillable
		{
			get
			{
				return this.isNillable;
			}
			set
			{
				this.isNillable = value;
				this.hasNillableAttribute = true;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x000828D6 File Offset: 0x000818D6
		[XmlIgnore]
		internal bool HasNillableAttribute
		{
			get
			{
				return this.hasNillableAttribute;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x000828DE File Offset: 0x000818DE
		[XmlIgnore]
		internal bool HasAbstractAttribute
		{
			get
			{
				return this.hasAbstractAttribute;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x000828E6 File Offset: 0x000818E6
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x000828EE File Offset: 0x000818EE
		[XmlAttribute("ref")]
		public XmlQualifiedName RefName
		{
			get
			{
				return this.refName;
			}
			set
			{
				this.refName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00082907 File Offset: 0x00081907
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x0008290F File Offset: 0x0008190F
		[XmlAttribute("substitutionGroup")]
		public XmlQualifiedName SubstitutionGroup
		{
			get
			{
				return this.substitutionGroup;
			}
			set
			{
				this.substitutionGroup = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00082928 File Offset: 0x00081928
		// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00082930 File Offset: 0x00081930
		[XmlAttribute("type")]
		public XmlQualifiedName SchemaTypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				this.typeName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x00082949 File Offset: 0x00081949
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00082951 File Offset: 0x00081951
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		public XmlSchemaType SchemaType
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

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0008295A File Offset: 0x0008195A
		[XmlElement("keyref", typeof(XmlSchemaKeyref))]
		[XmlElement("unique", typeof(XmlSchemaUnique))]
		[XmlElement("key", typeof(XmlSchemaKey))]
		public XmlSchemaObjectCollection Constraints
		{
			get
			{
				if (this.constraints == null)
				{
					this.constraints = new XmlSchemaObjectCollection();
				}
				return this.constraints;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x00082975 File Offset: 0x00081975
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0008297D File Offset: 0x0008197D
		[Obsolete("This property has been deprecated. Please use ElementSchemaType property that returns a strongly typed element type. http://go.microsoft.com/fwlink/?linkid=14202")]
		[XmlIgnore]
		public object ElementType
		{
			get
			{
				if (this.elementType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					return this.elementType.Datatype;
				}
				return this.elementType;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001C02 RID: 7170 RVA: 0x000829AD File Offset: 0x000819AD
		[XmlIgnore]
		public XmlSchemaType ElementSchemaType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x000829B5 File Offset: 0x000819B5
		[XmlIgnore]
		public XmlSchemaDerivationMethod BlockResolved
		{
			get
			{
				return this.blockResolved;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x000829BD File Offset: 0x000819BD
		[XmlIgnore]
		public XmlSchemaDerivationMethod FinalResolved
		{
			get
			{
				return this.finalResolved;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000829C8 File Offset: 0x000819C8
		internal XmlReader Validate(XmlReader reader, XmlResolver resolver, XmlSchemaSet schemaSet, ValidationEventHandler valEventHandler)
		{
			if (schemaSet != null)
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ValidationType = ValidationType.Schema;
				xmlReaderSettings.Schemas = schemaSet;
				xmlReaderSettings.ValidationEventHandler += valEventHandler;
				return new XsdValidatingReader(reader, resolver, xmlReaderSettings, this);
			}
			return null;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000829FF File Offset: 0x000819FF
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qualifiedName = value;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00082A08 File Offset: 0x00081A08
		internal void SetElementType(XmlSchemaType value)
		{
			this.elementType = value;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00082A11 File Offset: 0x00081A11
		internal void SetBlockResolved(XmlSchemaDerivationMethod value)
		{
			this.blockResolved = value;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x00082A1A File Offset: 0x00081A1A
		internal void SetFinalResolved(XmlSchemaDerivationMethod value)
		{
			this.finalResolved = value;
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00082A23 File Offset: 0x00081A23
		[XmlIgnore]
		internal bool HasDefault
		{
			get
			{
				return this.defaultValue != null && this.defaultValue.Length > 0;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00082A3D File Offset: 0x00081A3D
		internal bool HasConstraints
		{
			get
			{
				return this.constraints != null && this.constraints.Count > 0;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x00082A57 File Offset: 0x00081A57
		// (set) Token: 0x06001C0D RID: 7181 RVA: 0x00082A5F File Offset: 0x00081A5F
		internal bool IsLocalTypeDerivationChecked
		{
			get
			{
				return this.isLocalTypeDerivationChecked;
			}
			set
			{
				this.isLocalTypeDerivationChecked = value;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x00082A68 File Offset: 0x00081A68
		// (set) Token: 0x06001C0F RID: 7183 RVA: 0x00082A70 File Offset: 0x00081A70
		internal SchemaElementDecl ElementDecl
		{
			get
			{
				return this.elementDecl;
			}
			set
			{
				this.elementDecl = value;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x00082A79 File Offset: 0x00081A79
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x00082A81 File Offset: 0x00081A81
		[XmlIgnore]
		internal override string NameAttribute
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x00082A8A File Offset: 0x00081A8A
		[XmlIgnore]
		internal override string NameString
		{
			get
			{
				return this.qualifiedName.ToString();
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00082A98 File Offset: 0x00081A98
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)base.MemberwiseClone();
			xmlSchemaElement.refName = this.refName.Clone();
			xmlSchemaElement.substitutionGroup = this.substitutionGroup.Clone();
			xmlSchemaElement.typeName = this.typeName.Clone();
			xmlSchemaElement.constraints = null;
			return xmlSchemaElement;
		}

		// Token: 0x0400114B RID: 4427
		private bool isAbstract;

		// Token: 0x0400114C RID: 4428
		private bool hasAbstractAttribute;

		// Token: 0x0400114D RID: 4429
		private bool isNillable;

		// Token: 0x0400114E RID: 4430
		private bool hasNillableAttribute;

		// Token: 0x0400114F RID: 4431
		private bool isLocalTypeDerivationChecked;

		// Token: 0x04001150 RID: 4432
		private XmlSchemaDerivationMethod block = XmlSchemaDerivationMethod.None;

		// Token: 0x04001151 RID: 4433
		private XmlSchemaDerivationMethod final = XmlSchemaDerivationMethod.None;

		// Token: 0x04001152 RID: 4434
		private XmlSchemaForm form;

		// Token: 0x04001153 RID: 4435
		private string defaultValue;

		// Token: 0x04001154 RID: 4436
		private string fixedValue;

		// Token: 0x04001155 RID: 4437
		private string name;

		// Token: 0x04001156 RID: 4438
		private XmlQualifiedName refName = XmlQualifiedName.Empty;

		// Token: 0x04001157 RID: 4439
		private XmlQualifiedName substitutionGroup = XmlQualifiedName.Empty;

		// Token: 0x04001158 RID: 4440
		private XmlQualifiedName typeName = XmlQualifiedName.Empty;

		// Token: 0x04001159 RID: 4441
		private XmlSchemaType type;

		// Token: 0x0400115A RID: 4442
		private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

		// Token: 0x0400115B RID: 4443
		private XmlSchemaType elementType;

		// Token: 0x0400115C RID: 4444
		private XmlSchemaDerivationMethod blockResolved;

		// Token: 0x0400115D RID: 4445
		private XmlSchemaDerivationMethod finalResolved;

		// Token: 0x0400115E RID: 4446
		private XmlSchemaObjectCollection constraints;

		// Token: 0x0400115F RID: 4447
		private SchemaElementDecl elementDecl;
	}
}
