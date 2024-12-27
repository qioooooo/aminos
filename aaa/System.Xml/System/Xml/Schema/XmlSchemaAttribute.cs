using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000238 RID: 568
	public class XmlSchemaAttribute : XmlSchemaAnnotated
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x00081061 File Offset: 0x00080061
		// (set) Token: 0x06001B0D RID: 6925 RVA: 0x00081069 File Offset: 0x00080069
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

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x00081072 File Offset: 0x00080072
		// (set) Token: 0x06001B0F RID: 6927 RVA: 0x0008107A File Offset: 0x0008007A
		[XmlAttribute("fixed")]
		[DefaultValue(null)]
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

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x00081083 File Offset: 0x00080083
		// (set) Token: 0x06001B11 RID: 6929 RVA: 0x0008108B File Offset: 0x0008008B
		[DefaultValue(XmlSchemaForm.None)]
		[XmlAttribute("form")]
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

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00081094 File Offset: 0x00080094
		// (set) Token: 0x06001B13 RID: 6931 RVA: 0x0008109C File Offset: 0x0008009C
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

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000810A5 File Offset: 0x000800A5
		// (set) Token: 0x06001B15 RID: 6933 RVA: 0x000810AD File Offset: 0x000800AD
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

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000810C6 File Offset: 0x000800C6
		// (set) Token: 0x06001B17 RID: 6935 RVA: 0x000810CE File Offset: 0x000800CE
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

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000810E7 File Offset: 0x000800E7
		// (set) Token: 0x06001B19 RID: 6937 RVA: 0x000810EF File Offset: 0x000800EF
		[XmlElement("simpleType")]
		public XmlSchemaSimpleType SchemaType
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

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x000810F8 File Offset: 0x000800F8
		// (set) Token: 0x06001B1B RID: 6939 RVA: 0x00081100 File Offset: 0x00080100
		[XmlAttribute("use")]
		[DefaultValue(XmlSchemaUse.None)]
		public XmlSchemaUse Use
		{
			get
			{
				return this.use;
			}
			set
			{
				this.use = value;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x00081109 File Offset: 0x00080109
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x00081111 File Offset: 0x00080111
		[XmlIgnore]
		[Obsolete("This property has been deprecated. Please use AttributeSchemaType property that returns a strongly typed attribute type. http://go.microsoft.com/fwlink/?linkid=14202")]
		public object AttributeType
		{
			get
			{
				if (this.attributeType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					return this.attributeType.Datatype;
				}
				return this.attributeType;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x00081141 File Offset: 0x00080141
		[XmlIgnore]
		public XmlSchemaSimpleType AttributeSchemaType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0008114C File Offset: 0x0008014C
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

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x00081183 File Offset: 0x00080183
		[XmlIgnore]
		internal XmlSchemaDatatype Datatype
		{
			get
			{
				if (this.attributeType != null)
				{
					return this.attributeType.Datatype;
				}
				return null;
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0008119A File Offset: 0x0008019A
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qualifiedName = value;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000811A3 File Offset: 0x000801A3
		internal void SetAttributeType(XmlSchemaSimpleType value)
		{
			this.attributeType = value;
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000811AC File Offset: 0x000801AC
		internal string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x000811B4 File Offset: 0x000801B4
		// (set) Token: 0x06001B25 RID: 6949 RVA: 0x000811BC File Offset: 0x000801BC
		internal SchemaAttDef AttDef
		{
			get
			{
				return this.attDef;
			}
			set
			{
				this.attDef = value;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x000811C5 File Offset: 0x000801C5
		internal bool HasDefault
		{
			get
			{
				return this.defaultValue != null;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x000811D3 File Offset: 0x000801D3
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x000811DB File Offset: 0x000801DB
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

		// Token: 0x06001B29 RID: 6953 RVA: 0x000811E4 File Offset: 0x000801E4
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)base.MemberwiseClone();
			xmlSchemaAttribute.refName = this.refName.Clone();
			xmlSchemaAttribute.typeName = this.typeName.Clone();
			return xmlSchemaAttribute;
		}

		// Token: 0x040010EB RID: 4331
		private string defaultValue;

		// Token: 0x040010EC RID: 4332
		private string fixedValue;

		// Token: 0x040010ED RID: 4333
		private string name;

		// Token: 0x040010EE RID: 4334
		private string prefix;

		// Token: 0x040010EF RID: 4335
		private XmlSchemaForm form;

		// Token: 0x040010F0 RID: 4336
		private XmlSchemaUse use;

		// Token: 0x040010F1 RID: 4337
		private XmlQualifiedName refName = XmlQualifiedName.Empty;

		// Token: 0x040010F2 RID: 4338
		private XmlQualifiedName typeName = XmlQualifiedName.Empty;

		// Token: 0x040010F3 RID: 4339
		private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

		// Token: 0x040010F4 RID: 4340
		private XmlSchemaSimpleType type;

		// Token: 0x040010F5 RID: 4341
		private XmlSchemaSimpleType attributeType;

		// Token: 0x040010F6 RID: 4342
		private SchemaAttDef attDef;
	}
}
