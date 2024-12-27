using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace System.Data
{
	// Token: 0x020000E2 RID: 226
	[Serializable]
	internal sealed class SimpleType : ISerializable
	{
		// Token: 0x06000D81 RID: 3457 RVA: 0x001FFC48 File Offset: 0x001FF048
		internal SimpleType(string baseType)
		{
			this.baseType = baseType;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x001FFCD0 File Offset: 0x001FF0D0
		internal SimpleType(XmlSchemaSimpleType node)
		{
			this.name = node.Name;
			this.ns = ((node.QualifiedName != null) ? node.QualifiedName.Namespace : "");
			this.LoadTypeValues(node);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x001FFD8C File Offset: 0x001FF18C
		private SimpleType(SerializationInfo info, StreamingContext context)
		{
			this.baseType = info.GetString("SimpleType.BaseType");
			this.baseSimpleType = (SimpleType)info.GetValue("SimpleType.BaseSimpleType", typeof(SimpleType));
			if (info.GetBoolean("SimpleType.XmlBaseType.XmlQualifiedNameExists"))
			{
				string @string = info.GetString("SimpleType.XmlBaseType.Name");
				string string2 = info.GetString("SimpleType.XmlBaseType.Namespace");
				this.xmlBaseType = new XmlQualifiedName(@string, string2);
			}
			else
			{
				this.xmlBaseType = null;
			}
			this.name = info.GetString("SimpleType.Name");
			this.ns = info.GetString("SimpleType.NS");
			this.maxLength = info.GetInt32("SimpleType.MaxLength");
			this.length = info.GetInt32("SimpleType.Length");
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x001FFEBC File Offset: 0x001FF2BC
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("SimpleType.BaseType", this.baseType);
			info.AddValue("SimpleType.BaseSimpleType", this.baseSimpleType);
			XmlQualifiedName xmlQualifiedName = this.xmlBaseType;
			info.AddValue("SimpleType.XmlBaseType.XmlQualifiedNameExists", xmlQualifiedName != null);
			info.AddValue("SimpleType.XmlBaseType.Name", (xmlQualifiedName != null) ? xmlQualifiedName.Name : null);
			info.AddValue("SimpleType.XmlBaseType.Namespace", (xmlQualifiedName != null) ? xmlQualifiedName.Namespace : null);
			info.AddValue("SimpleType.Name", this.name);
			info.AddValue("SimpleType.NS", this.ns);
			info.AddValue("SimpleType.MaxLength", this.maxLength);
			info.AddValue("SimpleType.Length", this.length);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x001FFF88 File Offset: 0x001FF388
		internal void LoadTypeValues(XmlSchemaSimpleType node)
		{
			if (node.Content is XmlSchemaSimpleTypeList || node.Content is XmlSchemaSimpleTypeUnion)
			{
				throw ExceptionBuilder.SimpleTypeNotSupported();
			}
			if (node.Content is XmlSchemaSimpleTypeRestriction)
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)node.Content;
				XmlSchemaSimpleType xmlSchemaSimpleType = node.BaseXmlSchemaType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType != null && xmlSchemaSimpleType.QualifiedName.Namespace != "http://www.w3.org/2001/XMLSchema")
				{
					this.baseSimpleType = new SimpleType(node.BaseXmlSchemaType as XmlSchemaSimpleType);
				}
				if (xmlSchemaSimpleTypeRestriction.BaseTypeName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					this.baseType = xmlSchemaSimpleTypeRestriction.BaseTypeName.Name;
				}
				else
				{
					this.baseType = xmlSchemaSimpleTypeRestriction.BaseTypeName.ToString();
				}
				if (this.baseSimpleType != null && this.baseSimpleType.Name != null && this.baseSimpleType.Name.Length > 0)
				{
					this.xmlBaseType = this.baseSimpleType.XmlBaseType;
				}
				else
				{
					this.xmlBaseType = xmlSchemaSimpleTypeRestriction.BaseTypeName;
				}
				if (this.baseType == null || this.baseType.Length == 0)
				{
					this.baseType = xmlSchemaSimpleTypeRestriction.BaseType.Name;
					this.xmlBaseType = null;
				}
				if (this.baseType == "NOTATION")
				{
					this.baseType = "string";
				}
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaSimpleTypeRestriction.Facets)
				{
					XmlSchemaFacet xmlSchemaFacet = (XmlSchemaFacet)xmlSchemaObject;
					if (xmlSchemaFacet is XmlSchemaLengthFacet)
					{
						this.length = Convert.ToInt32(xmlSchemaFacet.Value, null);
					}
					if (xmlSchemaFacet is XmlSchemaMinLengthFacet)
					{
						this.minLength = Convert.ToInt32(xmlSchemaFacet.Value, null);
					}
					if (xmlSchemaFacet is XmlSchemaMaxLengthFacet)
					{
						this.maxLength = Convert.ToInt32(xmlSchemaFacet.Value, null);
					}
					if (xmlSchemaFacet is XmlSchemaPatternFacet)
					{
						this.pattern = xmlSchemaFacet.Value;
					}
					if (xmlSchemaFacet is XmlSchemaEnumerationFacet)
					{
						this.enumeration = ((!ADP.IsEmpty(this.enumeration)) ? (this.enumeration + " " + xmlSchemaFacet.Value) : xmlSchemaFacet.Value);
					}
					if (xmlSchemaFacet is XmlSchemaMinExclusiveFacet)
					{
						this.minExclusive = xmlSchemaFacet.Value;
					}
					if (xmlSchemaFacet is XmlSchemaMinInclusiveFacet)
					{
						this.minInclusive = xmlSchemaFacet.Value;
					}
					if (xmlSchemaFacet is XmlSchemaMaxExclusiveFacet)
					{
						this.maxExclusive = xmlSchemaFacet.Value;
					}
					if (xmlSchemaFacet is XmlSchemaMaxInclusiveFacet)
					{
						this.maxInclusive = xmlSchemaFacet.Value;
					}
				}
			}
			string msdataAttribute = XSDSchema.GetMsdataAttribute(node, "targetNamespace");
			if (msdataAttribute != null)
			{
				this.ns = msdataAttribute;
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00200238 File Offset: 0x001FF638
		internal bool IsPlainString()
		{
			return XSDSchema.QualifiedName(this.baseType) == XSDSchema.QualifiedName("string") && ADP.IsEmpty(this.name) && this.length == -1 && this.minLength == -1 && this.maxLength == -1 && ADP.IsEmpty(this.pattern) && ADP.IsEmpty(this.maxExclusive) && ADP.IsEmpty(this.maxInclusive) && ADP.IsEmpty(this.minExclusive) && ADP.IsEmpty(this.minInclusive) && ADP.IsEmpty(this.enumeration);
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x002002DC File Offset: 0x001FF6DC
		internal string BaseType
		{
			get
			{
				return this.baseType;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x002002F0 File Offset: 0x001FF6F0
		internal XmlQualifiedName XmlBaseType
		{
			get
			{
				return this.xmlBaseType;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00200304 File Offset: 0x001FF704
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00200318 File Offset: 0x001FF718
		internal string Namespace
		{
			get
			{
				return this.ns;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x0020032C File Offset: 0x001FF72C
		internal int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00200340 File Offset: 0x001FF740
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00200354 File Offset: 0x001FF754
		internal int MaxLength
		{
			get
			{
				return this.maxLength;
			}
			set
			{
				this.maxLength = value;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00200368 File Offset: 0x001FF768
		internal SimpleType BaseSimpleType
		{
			get
			{
				return this.baseSimpleType;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0020037C File Offset: 0x001FF77C
		public string SimpleTypeQualifiedName
		{
			get
			{
				if (this.ns.Length == 0)
				{
					return this.name;
				}
				return this.ns + ":" + this.name;
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x002003B4 File Offset: 0x001FF7B4
		internal string QualifiedName(string name)
		{
			int num = name.IndexOf(':');
			if (num == -1)
			{
				return "xs:" + name;
			}
			return name;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x002003DC File Offset: 0x001FF7DC
		internal XmlNode ToNode(XmlDocument dc, Hashtable prefixes, bool inRemoting)
		{
			XmlElement xmlElement = dc.CreateElement("xs", "simpleType", "http://www.w3.org/2001/XMLSchema");
			if (this.name != null && this.name.Length != 0)
			{
				xmlElement.SetAttribute("name", this.name);
				if (inRemoting)
				{
					xmlElement.SetAttribute("targetNamespace", "urn:schemas-microsoft-com:xml-msdata", this.Namespace);
				}
			}
			XmlElement xmlElement2 = dc.CreateElement("xs", "restriction", "http://www.w3.org/2001/XMLSchema");
			if (!inRemoting)
			{
				if (this.baseSimpleType != null)
				{
					if (this.baseSimpleType.Namespace != null && this.baseSimpleType.Namespace.Length > 0)
					{
						string text = ((prefixes != null) ? ((string)prefixes[this.baseSimpleType.Namespace]) : null);
						if (text != null)
						{
							xmlElement2.SetAttribute("base", text + ":" + this.baseSimpleType.Name);
						}
						else
						{
							xmlElement2.SetAttribute("base", this.baseSimpleType.Name);
						}
					}
					else
					{
						xmlElement2.SetAttribute("base", this.baseSimpleType.Name);
					}
				}
				else
				{
					xmlElement2.SetAttribute("base", this.QualifiedName(this.baseType));
				}
			}
			else
			{
				xmlElement2.SetAttribute("base", (this.baseSimpleType != null) ? this.baseSimpleType.Name : this.QualifiedName(this.baseType));
			}
			if (this.length >= 0)
			{
				XmlElement xmlElement3 = dc.CreateElement("xs", "length", "http://www.w3.org/2001/XMLSchema");
				xmlElement3.SetAttribute("value", this.length.ToString(CultureInfo.InvariantCulture));
				xmlElement2.AppendChild(xmlElement3);
			}
			if (this.maxLength >= 0)
			{
				XmlElement xmlElement3 = dc.CreateElement("xs", "maxLength", "http://www.w3.org/2001/XMLSchema");
				xmlElement3.SetAttribute("value", this.maxLength.ToString(CultureInfo.InvariantCulture));
				xmlElement2.AppendChild(xmlElement3);
			}
			xmlElement.AppendChild(xmlElement2);
			return xmlElement;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x002005D0 File Offset: 0x001FF9D0
		internal static SimpleType CreateEnumeratedType(string values)
		{
			return new SimpleType("string")
			{
				enumeration = values
			};
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x002005F0 File Offset: 0x001FF9F0
		internal static SimpleType CreateByteArrayType(string encoding)
		{
			return new SimpleType("base64Binary");
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0020060C File Offset: 0x001FFA0C
		internal static SimpleType CreateLimitedStringType(int length)
		{
			return new SimpleType("string")
			{
				maxLength = length
			};
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0020062C File Offset: 0x001FFA2C
		internal static SimpleType CreateSimpleType(Type type)
		{
			SimpleType simpleType = null;
			if (type == typeof(char))
			{
				simpleType = new SimpleType("string");
				simpleType.length = 1;
			}
			return simpleType;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0020065C File Offset: 0x001FFA5C
		internal string HasConflictingDefinition(SimpleType otherSimpleType)
		{
			if (otherSimpleType == null)
			{
				return "otherSimpleType";
			}
			if (this.MaxLength != otherSimpleType.MaxLength)
			{
				return "MaxLength";
			}
			if (string.Compare(this.BaseType, otherSimpleType.BaseType, StringComparison.Ordinal) != 0)
			{
				return "BaseType";
			}
			if (this.BaseSimpleType == null && otherSimpleType.BaseSimpleType != null && this.BaseSimpleType.HasConflictingDefinition(otherSimpleType.BaseSimpleType).Length != 0)
			{
				return "BaseSimpleType";
			}
			return string.Empty;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x002006D4 File Offset: 0x001FFAD4
		internal bool CanHaveMaxLength()
		{
			SimpleType simpleType = this;
			while (simpleType.BaseSimpleType != null)
			{
				simpleType = simpleType.BaseSimpleType;
			}
			return string.Compare(simpleType.BaseType, "string", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0020070C File Offset: 0x001FFB0C
		internal void ConvertToAnnonymousSimpleType()
		{
			this.name = null;
			this.ns = string.Empty;
			SimpleType simpleType = this;
			while (simpleType.baseSimpleType != null)
			{
				simpleType = simpleType.baseSimpleType;
			}
			this.baseType = simpleType.baseType;
			this.baseSimpleType = simpleType.baseSimpleType;
			this.xmlBaseType = simpleType.xmlBaseType;
		}

		// Token: 0x04000919 RID: 2329
		private string baseType;

		// Token: 0x0400091A RID: 2330
		private SimpleType baseSimpleType;

		// Token: 0x0400091B RID: 2331
		private XmlQualifiedName xmlBaseType;

		// Token: 0x0400091C RID: 2332
		private string name = "";

		// Token: 0x0400091D RID: 2333
		private int length = -1;

		// Token: 0x0400091E RID: 2334
		private int minLength = -1;

		// Token: 0x0400091F RID: 2335
		private int maxLength = -1;

		// Token: 0x04000920 RID: 2336
		private string pattern = "";

		// Token: 0x04000921 RID: 2337
		private string ns = "";

		// Token: 0x04000922 RID: 2338
		private string maxExclusive = "";

		// Token: 0x04000923 RID: 2339
		private string maxInclusive = "";

		// Token: 0x04000924 RID: 2340
		private string minExclusive = "";

		// Token: 0x04000925 RID: 2341
		private string minInclusive = "";

		// Token: 0x04000926 RID: 2342
		internal string enumeration = "";
	}
}
