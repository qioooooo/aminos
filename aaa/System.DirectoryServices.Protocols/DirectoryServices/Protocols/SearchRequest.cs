using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003B RID: 59
	public class SearchRequest : DirectoryRequest
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00005B49 File Offset: 0x00004B49
		public SearchRequest()
		{
			this.directoryAttributes = new StringCollection();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005B7C File Offset: 0x00004B7C
		public SearchRequest(string distinguishedName, XmlDocument filter, SearchScope searchScope, params string[] attributeList)
			: this()
		{
			this.dn = distinguishedName;
			if (attributeList != null)
			{
				for (int i = 0; i < attributeList.Length; i++)
				{
					this.directoryAttributes.Add(attributeList[i]);
				}
			}
			this.Scope = searchScope;
			this.Filter = filter;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005BC8 File Offset: 0x00004BC8
		public SearchRequest(string distinguishedName, string ldapFilter, SearchScope searchScope, params string[] attributeList)
			: this()
		{
			this.dn = distinguishedName;
			if (attributeList != null)
			{
				for (int i = 0; i < attributeList.Length; i++)
				{
					this.directoryAttributes.Add(attributeList[i]);
				}
			}
			this.Scope = searchScope;
			this.Filter = ldapFilter;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005C13 File Offset: 0x00004C13
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005C1B File Offset: 0x00004C1B
		public string DistinguishedName
		{
			get
			{
				return this.dn;
			}
			set
			{
				this.dn = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005C24 File Offset: 0x00004C24
		public StringCollection Attributes
		{
			get
			{
				return this.directoryAttributes;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005C2C File Offset: 0x00004C2C
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00005C34 File Offset: 0x00004C34
		public object Filter
		{
			get
			{
				return this.directoryFilter;
			}
			set
			{
				if (value is string || value is XmlDocument || value == null)
				{
					this.directoryFilter = value;
					return;
				}
				throw new ArgumentException(Res.GetString("ValidFilterType"), "value");
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005C65 File Offset: 0x00004C65
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00005C6D File Offset: 0x00004C6D
		public SearchScope Scope
		{
			get
			{
				return this.directoryScope;
			}
			set
			{
				if (value < SearchScope.Base || value > SearchScope.Subtree)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SearchScope));
				}
				this.directoryScope = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005C94 File Offset: 0x00004C94
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00005C9C File Offset: 0x00004C9C
		public DereferenceAlias Aliases
		{
			get
			{
				return this.directoryRefAlias;
			}
			set
			{
				if (value < DereferenceAlias.Never || value > DereferenceAlias.Always)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DereferenceAlias));
				}
				this.directoryRefAlias = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005CC3 File Offset: 0x00004CC3
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00005CCB File Offset: 0x00004CCB
		public int SizeLimit
		{
			get
			{
				return this.directorySizeLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("NoNegativeSizeLimit"), "value");
				}
				this.directorySizeLimit = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005CED File Offset: 0x00004CED
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00005CF8 File Offset: 0x00004CF8
		public TimeSpan TimeLimit
		{
			get
			{
				return this.directoryTimeLimit;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.directoryTimeLimit = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005D55 File Offset: 0x00004D55
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00005D5D File Offset: 0x00004D5D
		public bool TypesOnly
		{
			get
			{
				return this.directoryTypesOnly;
			}
			set
			{
				this.directoryTypesOnly = value;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005D68 File Offset: 0x00004D68
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "searchRequest", true, this.dn);
			XmlAttribute xmlAttribute = doc.CreateAttribute("scope", null);
			switch (this.directoryScope)
			{
			case SearchScope.Base:
				xmlAttribute.InnerText = "baseObject";
				break;
			case SearchScope.OneLevel:
				xmlAttribute.InnerText = "singleLevel";
				break;
			case SearchScope.Subtree:
				xmlAttribute.InnerText = "wholeSubtree";
				break;
			}
			xmlElement.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("derefAliases", null);
			switch (this.directoryRefAlias)
			{
			case DereferenceAlias.Never:
				xmlAttribute2.InnerText = "neverDerefAliases";
				break;
			case DereferenceAlias.InSearching:
				xmlAttribute2.InnerText = "derefInSearching";
				break;
			case DereferenceAlias.FindingBaseObject:
				xmlAttribute2.InnerText = "derefFindingBaseObj";
				break;
			case DereferenceAlias.Always:
				xmlAttribute2.InnerText = "derefAlways";
				break;
			}
			xmlElement.Attributes.Append(xmlAttribute2);
			XmlAttribute xmlAttribute3 = doc.CreateAttribute("sizeLimit", null);
			xmlAttribute3.InnerText = this.directorySizeLimit.ToString(CultureInfo.InvariantCulture);
			xmlElement.Attributes.Append(xmlAttribute3);
			XmlAttribute xmlAttribute4 = doc.CreateAttribute("timeLimit", null);
			xmlAttribute4.InnerText = (this.directoryTimeLimit.Ticks / 10000000L).ToString(CultureInfo.InvariantCulture);
			xmlElement.Attributes.Append(xmlAttribute4);
			XmlAttribute xmlAttribute5 = doc.CreateAttribute("typesOnly", null);
			xmlAttribute5.InnerText = (this.directoryTypesOnly ? "true" : "false");
			xmlElement.Attributes.Append(xmlAttribute5);
			XmlElement xmlElement2 = doc.CreateElement("filter", "urn:oasis:names:tc:DSML:2:0:core");
			if (this.Filter != null)
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				try
				{
					if (this.Filter is XmlDocument)
					{
						if (((XmlDocument)this.Filter).NamespaceURI.Length == 0)
						{
							this.CopyFilter((XmlDocument)this.Filter, xmlTextWriter);
							xmlElement2.InnerXml = stringWriter.ToString();
						}
						else
						{
							xmlElement2.InnerXml = ((XmlDocument)this.Filter).OuterXml;
						}
					}
					else if (this.Filter is string)
					{
						string text = (string)this.Filter;
						if (!text.StartsWith("(", StringComparison.Ordinal) && !text.EndsWith(")", StringComparison.Ordinal))
						{
							text = text.Insert(0, "(");
							text += ")";
						}
						ADFilter adfilter = FilterParser.ParseFilterString(text);
						if (adfilter == null)
						{
							throw new ArgumentException(Res.GetString("BadSearchLDAPFilter"));
						}
						DSMLFilterWriter dsmlfilterWriter = new DSMLFilterWriter();
						dsmlfilterWriter.WriteFilter(adfilter, false, xmlTextWriter, "urn:oasis:names:tc:DSML:2:0:core");
						xmlElement2.InnerXml = stringWriter.ToString();
					}
					goto IL_02D0;
				}
				finally
				{
					xmlTextWriter.Close();
				}
			}
			xmlElement2.InnerXml = "<present name='objectClass' xmlns=\"urn:oasis:names:tc:DSML:2:0:core\"/>";
			IL_02D0:
			xmlElement.AppendChild(xmlElement2);
			if (this.directoryAttributes != null && this.directoryAttributes.Count != 0)
			{
				XmlElement xmlElement3 = doc.CreateElement("attributes", "urn:oasis:names:tc:DSML:2:0:core");
				xmlElement.AppendChild(xmlElement3);
				foreach (string text2 in this.directoryAttributes)
				{
					XmlElement xmlElement4 = new DirectoryAttribute
					{
						Name = text2
					}.ToXmlNode(doc, "attribute");
					xmlElement3.AppendChild(xmlElement4);
				}
			}
			return xmlElement;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006118 File Offset: 0x00005118
		private void CopyFilter(XmlNode node, XmlTextWriter writer)
		{
			for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode != null)
				{
					this.CopyXmlTree(xmlNode, writer);
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006144 File Offset: 0x00005144
		private void CopyXmlTree(XmlNode node, XmlTextWriter writer)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				writer.WriteStartElement(node.LocalName, "urn:oasis:names:tc:DSML:2:0:core");
				foreach (object obj in node.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					writer.WriteAttributeString(xmlAttribute.LocalName, xmlAttribute.Value);
				}
				for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					this.CopyXmlTree(xmlNode, writer);
				}
				writer.WriteEndElement();
				return;
			}
			writer.WriteRaw(node.OuterXml);
		}

		// Token: 0x04000111 RID: 273
		private string dn;

		// Token: 0x04000112 RID: 274
		private StringCollection directoryAttributes = new StringCollection();

		// Token: 0x04000113 RID: 275
		private object directoryFilter;

		// Token: 0x04000114 RID: 276
		private SearchScope directoryScope = SearchScope.Subtree;

		// Token: 0x04000115 RID: 277
		private DereferenceAlias directoryRefAlias;

		// Token: 0x04000116 RID: 278
		private int directorySizeLimit;

		// Token: 0x04000117 RID: 279
		private TimeSpan directoryTimeLimit = new TimeSpan(0L);

		// Token: 0x04000118 RID: 280
		private bool directoryTypesOnly;
	}
}
