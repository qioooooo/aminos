using System;
using System.Collections;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002E5 RID: 741
	internal class SchemaObjectWriter
	{
		// Token: 0x06002291 RID: 8849 RVA: 0x000A17F0 File Offset: 0x000A07F0
		private void WriteIndent()
		{
			for (int i = 0; i < this.indentLevel; i++)
			{
				this.w.Append(" ");
			}
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000A1820 File Offset: 0x000A0820
		protected void WriteAttribute(string localName, string ns, string value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			this.w.Append(",");
			this.w.Append(ns);
			if (ns != null && ns.Length != 0)
			{
				this.w.Append(":");
			}
			this.w.Append(localName);
			this.w.Append("=");
			this.w.Append(value);
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000A189E File Offset: 0x000A089E
		protected void WriteAttribute(string localName, string ns, XmlQualifiedName value)
		{
			if (value.IsEmpty)
			{
				return;
			}
			this.WriteAttribute(localName, ns, value.ToString());
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000A18B7 File Offset: 0x000A08B7
		protected void WriteStartElement(string name)
		{
			this.NewLine();
			this.indentLevel++;
			this.w.Append("[");
			this.w.Append(name);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000A18EB File Offset: 0x000A08EB
		protected void WriteEndElement()
		{
			this.w.Append("]");
			this.indentLevel--;
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000A190C File Offset: 0x000A090C
		protected void NewLine()
		{
			this.w.Append(Environment.NewLine);
			this.WriteIndent();
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000A1925 File Offset: 0x000A0925
		protected string GetString()
		{
			return this.w.ToString();
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000A1932 File Offset: 0x000A0932
		private void WriteAttribute(XmlAttribute a)
		{
			if (a.Value != null)
			{
				this.WriteAttribute(a.Name, a.NamespaceURI, a.Value);
			}
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000A1954 File Offset: 0x000A0954
		private void WriteAttributes(XmlAttribute[] a, XmlSchemaObject o)
		{
			if (a == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < a.Length; i++)
			{
				arrayList.Add(a[i]);
			}
			arrayList.Sort(new XmlAttributeComparer());
			for (int j = 0; j < arrayList.Count; j++)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)arrayList[j];
				this.WriteAttribute(xmlAttribute);
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000A19B4 File Offset: 0x000A09B4
		internal static string ToString(NamespaceList list)
		{
			if (list == null)
			{
				return null;
			}
			switch (list.Type)
			{
			case NamespaceList.ListType.Any:
				return "##any";
			case NamespaceList.ListType.Other:
				return "##other";
			case NamespaceList.ListType.Set:
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in list.Enumerate)
				{
					string text = (string)obj;
					arrayList.Add(text);
				}
				arrayList.Sort();
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				foreach (object obj2 in arrayList)
				{
					string text2 = (string)obj2;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(" ");
					}
					if (text2.Length == 0)
					{
						stringBuilder.Append("##local");
					}
					else
					{
						stringBuilder.Append(text2);
					}
				}
				return stringBuilder.ToString();
			}
			default:
				return list.ToString();
			}
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000A1AE0 File Offset: 0x000A0AE0
		internal string WriteXmlSchemaObject(XmlSchemaObject o)
		{
			if (o == null)
			{
				return string.Empty;
			}
			this.Write3_XmlSchemaObject(o);
			return this.GetString();
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x000A1AF8 File Offset: 0x000A0AF8
		private void WriteSortedItems(XmlSchemaObjectCollection items)
		{
			if (items == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < items.Count; i++)
			{
				arrayList.Add(items[i]);
			}
			arrayList.Sort(new XmlSchemaObjectComparer());
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.Write3_XmlSchemaObject((XmlSchemaObject)arrayList[j]);
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000A1B5C File Offset: 0x000A0B5C
		private void Write1_XmlSchemaAttribute(XmlSchemaAttribute o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("attribute");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.WriteAttribute("default", "", o.DefaultValue);
			this.WriteAttribute("fixed", "", o.FixedValue);
			if (o.Parent != null && !(o.Parent is XmlSchema))
			{
				if (o.QualifiedName != null && !o.QualifiedName.IsEmpty && o.QualifiedName.Namespace != null && o.QualifiedName.Namespace.Length != 0)
				{
					this.WriteAttribute("form", "", "qualified");
				}
				else
				{
					this.WriteAttribute("form", "", "unqualified");
				}
			}
			this.WriteAttribute("name", "", o.Name);
			if (!o.RefName.IsEmpty)
			{
				this.WriteAttribute("ref", "", o.RefName);
			}
			else if (!o.SchemaTypeName.IsEmpty)
			{
				this.WriteAttribute("type", "", o.SchemaTypeName);
			}
			XmlSchemaUse xmlSchemaUse = ((o.Use == XmlSchemaUse.None) ? XmlSchemaUse.Optional : o.Use);
			this.WriteAttribute("use", "", this.Write30_XmlSchemaUse(xmlSchemaUse));
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write9_XmlSchemaSimpleType(o.SchemaType);
			this.WriteEndElement();
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000A1CE8 File Offset: 0x000A0CE8
		private void Write3_XmlSchemaObject(XmlSchemaObject o)
		{
			if (o == null)
			{
				return;
			}
			Type type = o.GetType();
			if (type == typeof(XmlSchemaComplexType))
			{
				this.Write35_XmlSchemaComplexType((XmlSchemaComplexType)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleType))
			{
				this.Write9_XmlSchemaSimpleType((XmlSchemaSimpleType)o);
				return;
			}
			if (type == typeof(XmlSchemaElement))
			{
				this.Write46_XmlSchemaElement((XmlSchemaElement)o);
				return;
			}
			if (type == typeof(XmlSchemaAppInfo))
			{
				this.Write7_XmlSchemaAppInfo((XmlSchemaAppInfo)o);
				return;
			}
			if (type == typeof(XmlSchemaDocumentation))
			{
				this.Write6_XmlSchemaDocumentation((XmlSchemaDocumentation)o);
				return;
			}
			if (type == typeof(XmlSchemaAnnotation))
			{
				this.Write5_XmlSchemaAnnotation((XmlSchemaAnnotation)o);
				return;
			}
			if (type == typeof(XmlSchemaGroup))
			{
				this.Write57_XmlSchemaGroup((XmlSchemaGroup)o);
				return;
			}
			if (type == typeof(XmlSchemaXPath))
			{
				this.Write49_XmlSchemaXPath("xpath", "", (XmlSchemaXPath)o);
				return;
			}
			if (type == typeof(XmlSchemaIdentityConstraint))
			{
				this.Write48_XmlSchemaIdentityConstraint((XmlSchemaIdentityConstraint)o);
				return;
			}
			if (type == typeof(XmlSchemaUnique))
			{
				this.Write51_XmlSchemaUnique((XmlSchemaUnique)o);
				return;
			}
			if (type == typeof(XmlSchemaKeyref))
			{
				this.Write50_XmlSchemaKeyref((XmlSchemaKeyref)o);
				return;
			}
			if (type == typeof(XmlSchemaKey))
			{
				this.Write47_XmlSchemaKey((XmlSchemaKey)o);
				return;
			}
			if (type == typeof(XmlSchemaGroupRef))
			{
				this.Write55_XmlSchemaGroupRef((XmlSchemaGroupRef)o);
				return;
			}
			if (type == typeof(XmlSchemaAny))
			{
				this.Write53_XmlSchemaAny((XmlSchemaAny)o);
				return;
			}
			if (type == typeof(XmlSchemaSequence))
			{
				this.Write54_XmlSchemaSequence((XmlSchemaSequence)o);
				return;
			}
			if (type == typeof(XmlSchemaChoice))
			{
				this.Write52_XmlSchemaChoice((XmlSchemaChoice)o);
				return;
			}
			if (type == typeof(XmlSchemaAll))
			{
				this.Write43_XmlSchemaAll((XmlSchemaAll)o);
				return;
			}
			if (type == typeof(XmlSchemaComplexContentRestriction))
			{
				this.Write56_XmlSchemaComplexContentRestriction((XmlSchemaComplexContentRestriction)o);
				return;
			}
			if (type == typeof(XmlSchemaComplexContentExtension))
			{
				this.Write42_XmlSchemaComplexContentExtension((XmlSchemaComplexContentExtension)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleContentRestriction))
			{
				this.Write40_XmlSchemaSimpleContentRestriction((XmlSchemaSimpleContentRestriction)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleContentExtension))
			{
				this.Write38_XmlSchemaSimpleContentExtension((XmlSchemaSimpleContentExtension)o);
				return;
			}
			if (type == typeof(XmlSchemaComplexContent))
			{
				this.Write41_XmlSchemaComplexContent((XmlSchemaComplexContent)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleContent))
			{
				this.Write36_XmlSchemaSimpleContent((XmlSchemaSimpleContent)o);
				return;
			}
			if (type == typeof(XmlSchemaAnyAttribute))
			{
				this.Write33_XmlSchemaAnyAttribute((XmlSchemaAnyAttribute)o);
				return;
			}
			if (type == typeof(XmlSchemaAttributeGroupRef))
			{
				this.Write32_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef)o);
				return;
			}
			if (type == typeof(XmlSchemaAttributeGroup))
			{
				this.Write31_XmlSchemaAttributeGroup((XmlSchemaAttributeGroup)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleTypeRestriction))
			{
				this.Write15_XmlSchemaSimpleTypeRestriction((XmlSchemaSimpleTypeRestriction)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleTypeList))
			{
				this.Write14_XmlSchemaSimpleTypeList((XmlSchemaSimpleTypeList)o);
				return;
			}
			if (type == typeof(XmlSchemaSimpleTypeUnion))
			{
				this.Write12_XmlSchemaSimpleTypeUnion((XmlSchemaSimpleTypeUnion)o);
				return;
			}
			if (type == typeof(XmlSchemaAttribute))
			{
				this.Write1_XmlSchemaAttribute((XmlSchemaAttribute)o);
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000A2018 File Offset: 0x000A1018
		private void Write5_XmlSchemaAnnotation(XmlSchemaAnnotation o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("annotation");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					XmlSchemaObject xmlSchemaObject = items[i];
					if (xmlSchemaObject is XmlSchemaAppInfo)
					{
						this.Write7_XmlSchemaAppInfo((XmlSchemaAppInfo)xmlSchemaObject);
					}
					else if (xmlSchemaObject is XmlSchemaDocumentation)
					{
						this.Write6_XmlSchemaDocumentation((XmlSchemaDocumentation)xmlSchemaObject);
					}
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x000A20AC File Offset: 0x000A10AC
		private void Write6_XmlSchemaDocumentation(XmlSchemaDocumentation o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("documentation");
			this.WriteAttribute("source", "", o.Source);
			this.WriteAttribute("lang", "http://www.w3.org/XML/1998/namespace", o.Language);
			XmlNode[] markup = o.Markup;
			if (markup != null)
			{
				foreach (XmlNode xmlNode in markup)
				{
					this.WriteStartElement("node");
					this.WriteAttribute("xml", "", xmlNode.OuterXml);
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000A2138 File Offset: 0x000A1138
		private void Write7_XmlSchemaAppInfo(XmlSchemaAppInfo o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("appinfo");
			this.WriteAttribute("source", "", o.Source);
			XmlNode[] markup = o.Markup;
			if (markup != null)
			{
				foreach (XmlNode xmlNode in markup)
				{
					this.WriteStartElement("node");
					this.WriteAttribute("xml", "", xmlNode.OuterXml);
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000A21B0 File Offset: 0x000A11B0
		private void Write9_XmlSchemaSimpleType(XmlSchemaSimpleType o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("simpleType");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttribute("final", "", this.Write11_XmlSchemaDerivationMethod(o.FinalResolved));
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Content is XmlSchemaSimpleTypeUnion)
			{
				this.Write12_XmlSchemaSimpleTypeUnion((XmlSchemaSimpleTypeUnion)o.Content);
			}
			else if (o.Content is XmlSchemaSimpleTypeRestriction)
			{
				this.Write15_XmlSchemaSimpleTypeRestriction((XmlSchemaSimpleTypeRestriction)o.Content);
			}
			else if (o.Content is XmlSchemaSimpleTypeList)
			{
				this.Write14_XmlSchemaSimpleTypeList((XmlSchemaSimpleTypeList)o.Content);
			}
			this.WriteEndElement();
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000A2291 File Offset: 0x000A1291
		private string Write11_XmlSchemaDerivationMethod(XmlSchemaDerivationMethod v)
		{
			return v.ToString();
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000A22A0 File Offset: 0x000A12A0
		private void Write12_XmlSchemaSimpleTypeUnion(XmlSchemaSimpleTypeUnion o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("union");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (o.MemberTypes != null)
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < o.MemberTypes.Length; i++)
				{
					arrayList.Add(o.MemberTypes[i]);
				}
				arrayList.Sort(new QNameComparer());
				this.w.Append(",");
				this.w.Append("memberTypes=");
				for (int j = 0; j < arrayList.Count; j++)
				{
					XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)arrayList[j];
					this.w.Append(xmlQualifiedName.ToString());
					this.w.Append(",");
				}
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteSortedItems(o.BaseTypes);
			this.WriteEndElement();
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000A23A0 File Offset: 0x000A13A0
		private void Write14_XmlSchemaSimpleTypeList(XmlSchemaSimpleTypeList o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("list");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.ItemTypeName.IsEmpty)
			{
				this.WriteAttribute("itemType", "", o.ItemTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write9_XmlSchemaSimpleType(o.ItemType);
			this.WriteEndElement();
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000A2420 File Offset: 0x000A1420
		private void Write15_XmlSchemaSimpleTypeRestriction(XmlSchemaSimpleTypeRestriction o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("restriction");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.BaseTypeName.IsEmpty)
			{
				this.WriteAttribute("base", "", o.BaseTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write9_XmlSchemaSimpleType(o.BaseType);
			this.WriteFacets(o.Facets);
			this.WriteEndElement();
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000A24AC File Offset: 0x000A14AC
		private void WriteFacets(XmlSchemaObjectCollection facets)
		{
			if (facets == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < facets.Count; i++)
			{
				arrayList.Add(facets[i]);
			}
			arrayList.Sort(new XmlFacetComparer());
			for (int j = 0; j < arrayList.Count; j++)
			{
				XmlSchemaObject xmlSchemaObject = (XmlSchemaObject)arrayList[j];
				if (xmlSchemaObject is XmlSchemaMinExclusiveFacet)
				{
					this.Write_XmlSchemaFacet("minExclusive", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaMaxInclusiveFacet)
				{
					this.Write_XmlSchemaFacet("maxInclusive", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaMaxExclusiveFacet)
				{
					this.Write_XmlSchemaFacet("maxExclusive", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaMinInclusiveFacet)
				{
					this.Write_XmlSchemaFacet("minInclusive", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaLengthFacet)
				{
					this.Write_XmlSchemaFacet("length", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaEnumerationFacet)
				{
					this.Write_XmlSchemaFacet("enumeration", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaMinLengthFacet)
				{
					this.Write_XmlSchemaFacet("minLength", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaPatternFacet)
				{
					this.Write_XmlSchemaFacet("pattern", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaTotalDigitsFacet)
				{
					this.Write_XmlSchemaFacet("totalDigits", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaMaxLengthFacet)
				{
					this.Write_XmlSchemaFacet("maxLength", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaWhiteSpaceFacet)
				{
					this.Write_XmlSchemaFacet("whiteSpace", (XmlSchemaFacet)xmlSchemaObject);
				}
				else if (xmlSchemaObject is XmlSchemaFractionDigitsFacet)
				{
					this.Write_XmlSchemaFacet("fractionDigit", (XmlSchemaFacet)xmlSchemaObject);
				}
			}
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000A2668 File Offset: 0x000A1668
		private void Write_XmlSchemaFacet(string name, XmlSchemaFacet o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement(name);
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("value", "", o.Value);
			if (o.IsFixed)
			{
				this.WriteAttribute("fixed", "", XmlConvert.ToString(o.IsFixed));
			}
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000A26F0 File Offset: 0x000A16F0
		private string Write30_XmlSchemaUse(XmlSchemaUse v)
		{
			string text = null;
			switch (v)
			{
			case XmlSchemaUse.Optional:
				text = "optional";
				break;
			case XmlSchemaUse.Prohibited:
				text = "prohibited";
				break;
			case XmlSchemaUse.Required:
				text = "required";
				break;
			}
			return text;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000A2730 File Offset: 0x000A1730
		private void Write31_XmlSchemaAttributeGroup(XmlSchemaAttributeGroup o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("attributeGroup");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000A27B0 File Offset: 0x000A17B0
		private void Write32_XmlSchemaAttributeGroupRef(XmlSchemaAttributeGroupRef o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("attributeGroup");
			this.WriteAttribute("id", "", o.Id);
			if (!o.RefName.IsEmpty)
			{
				this.WriteAttribute("ref", "", o.RefName);
			}
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000A2824 File Offset: 0x000A1824
		private void Write33_XmlSchemaAnyAttribute(XmlSchemaAnyAttribute o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("anyAttribute");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("namespace", "", SchemaObjectWriter.ToString(o.NamespaceList));
			XmlSchemaContentProcessing xmlSchemaContentProcessing = ((o.ProcessContents == XmlSchemaContentProcessing.None) ? XmlSchemaContentProcessing.Strict : o.ProcessContents);
			this.WriteAttribute("processContents", "", this.Write34_XmlSchemaContentProcessing(xmlSchemaContentProcessing));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000A28BC File Offset: 0x000A18BC
		private string Write34_XmlSchemaContentProcessing(XmlSchemaContentProcessing v)
		{
			string text = null;
			switch (v)
			{
			case XmlSchemaContentProcessing.Skip:
				text = "skip";
				break;
			case XmlSchemaContentProcessing.Lax:
				text = "lax";
				break;
			case XmlSchemaContentProcessing.Strict:
				text = "strict";
				break;
			}
			return text;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000A28FC File Offset: 0x000A18FC
		private void Write35_XmlSchemaComplexType(XmlSchemaComplexType o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("complexType");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttribute("final", "", this.Write11_XmlSchemaDerivationMethod(o.FinalResolved));
			if (o.IsAbstract)
			{
				this.WriteAttribute("abstract", "", XmlConvert.ToString(o.IsAbstract));
			}
			this.WriteAttribute("block", "", this.Write11_XmlSchemaDerivationMethod(o.BlockResolved));
			if (o.IsMixed)
			{
				this.WriteAttribute("mixed", "", XmlConvert.ToString(o.IsMixed));
			}
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.ContentModel is XmlSchemaComplexContent)
			{
				this.Write41_XmlSchemaComplexContent((XmlSchemaComplexContent)o.ContentModel);
			}
			else if (o.ContentModel is XmlSchemaSimpleContent)
			{
				this.Write36_XmlSchemaSimpleContent((XmlSchemaSimpleContent)o.ContentModel);
			}
			if (o.Particle is XmlSchemaSequence)
			{
				this.Write54_XmlSchemaSequence((XmlSchemaSequence)o.Particle);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write55_XmlSchemaGroupRef((XmlSchemaGroupRef)o.Particle);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write52_XmlSchemaChoice((XmlSchemaChoice)o.Particle);
			}
			else if (o.Particle is XmlSchemaAll)
			{
				this.Write43_XmlSchemaAll((XmlSchemaAll)o.Particle);
			}
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000A2AB8 File Offset: 0x000A1AB8
		private void Write36_XmlSchemaSimpleContent(XmlSchemaSimpleContent o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("simpleContent");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Content is XmlSchemaSimpleContentRestriction)
			{
				this.Write40_XmlSchemaSimpleContentRestriction((XmlSchemaSimpleContentRestriction)o.Content);
			}
			else if (o.Content is XmlSchemaSimpleContentExtension)
			{
				this.Write38_XmlSchemaSimpleContentExtension((XmlSchemaSimpleContentExtension)o.Content);
			}
			this.WriteEndElement();
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000A2B48 File Offset: 0x000A1B48
		private void Write38_XmlSchemaSimpleContentExtension(XmlSchemaSimpleContentExtension o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("extension");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.BaseTypeName.IsEmpty)
			{
				this.WriteAttribute("base", "", o.BaseTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000A2BD4 File Offset: 0x000A1BD4
		private void Write40_XmlSchemaSimpleContentRestriction(XmlSchemaSimpleContentRestriction o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("restriction");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.BaseTypeName.IsEmpty)
			{
				this.WriteAttribute("base", "", o.BaseTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write9_XmlSchemaSimpleType(o.BaseType);
			this.WriteFacets(o.Facets);
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000A2C78 File Offset: 0x000A1C78
		private void Write41_XmlSchemaComplexContent(XmlSchemaComplexContent o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("complexContent");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("mixed", "", XmlConvert.ToString(o.IsMixed));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Content is XmlSchemaComplexContentRestriction)
			{
				this.Write56_XmlSchemaComplexContentRestriction((XmlSchemaComplexContentRestriction)o.Content);
			}
			else if (o.Content is XmlSchemaComplexContentExtension)
			{
				this.Write42_XmlSchemaComplexContentExtension((XmlSchemaComplexContentExtension)o.Content);
			}
			this.WriteEndElement();
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000A2D24 File Offset: 0x000A1D24
		private void Write42_XmlSchemaComplexContentExtension(XmlSchemaComplexContentExtension o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("extension");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.BaseTypeName.IsEmpty)
			{
				this.WriteAttribute("base", "", o.BaseTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Particle is XmlSchemaSequence)
			{
				this.Write54_XmlSchemaSequence((XmlSchemaSequence)o.Particle);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write55_XmlSchemaGroupRef((XmlSchemaGroupRef)o.Particle);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write52_XmlSchemaChoice((XmlSchemaChoice)o.Particle);
			}
			else if (o.Particle is XmlSchemaAll)
			{
				this.Write43_XmlSchemaAll((XmlSchemaAll)o.Particle);
			}
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000A2E30 File Offset: 0x000A1E30
		private void Write43_XmlSchemaAll(XmlSchemaAll o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("all");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteSortedItems(o.Items);
			this.WriteEndElement();
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000A2EE4 File Offset: 0x000A1EE4
		private void Write46_XmlSchemaElement(XmlSchemaElement o)
		{
			if (o == null)
			{
				return;
			}
			o.GetType();
			this.WriteStartElement("element");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			if (o.IsAbstract)
			{
				this.WriteAttribute("abstract", "", XmlConvert.ToString(o.IsAbstract));
			}
			this.WriteAttribute("block", "", this.Write11_XmlSchemaDerivationMethod(o.BlockResolved));
			this.WriteAttribute("default", "", o.DefaultValue);
			this.WriteAttribute("final", "", this.Write11_XmlSchemaDerivationMethod(o.FinalResolved));
			this.WriteAttribute("fixed", "", o.FixedValue);
			if (o.Parent != null && !(o.Parent is XmlSchema))
			{
				if (o.QualifiedName != null && !o.QualifiedName.IsEmpty && o.QualifiedName.Namespace != null && o.QualifiedName.Namespace.Length != 0)
				{
					this.WriteAttribute("form", "", "qualified");
				}
				else
				{
					this.WriteAttribute("form", "", "unqualified");
				}
			}
			if (o.Name != null && o.Name.Length != 0)
			{
				this.WriteAttribute("name", "", o.Name);
			}
			if (o.IsNillable)
			{
				this.WriteAttribute("nillable", "", XmlConvert.ToString(o.IsNillable));
			}
			if (!o.SubstitutionGroup.IsEmpty)
			{
				this.WriteAttribute("substitutionGroup", "", o.SubstitutionGroup);
			}
			if (!o.RefName.IsEmpty)
			{
				this.WriteAttribute("ref", "", o.RefName);
			}
			else if (!o.SchemaTypeName.IsEmpty)
			{
				this.WriteAttribute("type", "", o.SchemaTypeName);
			}
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.SchemaType is XmlSchemaComplexType)
			{
				this.Write35_XmlSchemaComplexType((XmlSchemaComplexType)o.SchemaType);
			}
			else if (o.SchemaType is XmlSchemaSimpleType)
			{
				this.Write9_XmlSchemaSimpleType((XmlSchemaSimpleType)o.SchemaType);
			}
			this.WriteSortedItems(o.Constraints);
			this.WriteEndElement();
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000A3198 File Offset: 0x000A2198
		private void Write47_XmlSchemaKey(XmlSchemaKey o)
		{
			if (o == null)
			{
				return;
			}
			o.GetType();
			this.WriteStartElement("key");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write49_XmlSchemaXPath("selector", "", o.Selector);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int i = 0; i < fields.Count; i++)
				{
					this.Write49_XmlSchemaXPath("field", "", (XmlSchemaXPath)fields[i]);
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000A3254 File Offset: 0x000A2254
		private void Write48_XmlSchemaIdentityConstraint(XmlSchemaIdentityConstraint o)
		{
			if (o == null)
			{
				return;
			}
			Type type = o.GetType();
			if (type == typeof(XmlSchemaUnique))
			{
				this.Write51_XmlSchemaUnique((XmlSchemaUnique)o);
				return;
			}
			if (type == typeof(XmlSchemaKeyref))
			{
				this.Write50_XmlSchemaKeyref((XmlSchemaKeyref)o);
				return;
			}
			if (type == typeof(XmlSchemaKey))
			{
				this.Write47_XmlSchemaKey((XmlSchemaKey)o);
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000A32BC File Offset: 0x000A22BC
		private void Write49_XmlSchemaXPath(string name, string ns, XmlSchemaXPath o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement(name);
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("xpath", "", o.XPath);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000A3320 File Offset: 0x000A2320
		private void Write50_XmlSchemaKeyref(XmlSchemaKeyref o)
		{
			if (o == null)
			{
				return;
			}
			o.GetType();
			this.WriteStartElement("keyref");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.WriteAttribute("refer", "", o.Refer);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write49_XmlSchemaXPath("selector", "", o.Selector);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int i = 0; i < fields.Count; i++)
				{
					this.Write49_XmlSchemaXPath("field", "", (XmlSchemaXPath)fields[i]);
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000A33F4 File Offset: 0x000A23F4
		private void Write51_XmlSchemaUnique(XmlSchemaUnique o)
		{
			if (o == null)
			{
				return;
			}
			o.GetType();
			this.WriteStartElement("unique");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.Write49_XmlSchemaXPath("selector", "", o.Selector);
			XmlSchemaObjectCollection fields = o.Fields;
			if (fields != null)
			{
				for (int i = 0; i < fields.Count; i++)
				{
					this.Write49_XmlSchemaXPath("field", "", (XmlSchemaXPath)fields[i]);
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000A34B0 File Offset: 0x000A24B0
		private void Write52_XmlSchemaChoice(XmlSchemaChoice o)
		{
			if (o == null)
			{
				return;
			}
			o.GetType();
			this.WriteStartElement("choice");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteSortedItems(o.Items);
			this.WriteEndElement();
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000A3568 File Offset: 0x000A2568
		private void Write53_XmlSchemaAny(XmlSchemaAny o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("any");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			this.WriteAttribute("namespace", "", SchemaObjectWriter.ToString(o.NamespaceList));
			XmlSchemaContentProcessing xmlSchemaContentProcessing = ((o.ProcessContents == XmlSchemaContentProcessing.None) ? XmlSchemaContentProcessing.Strict : o.ProcessContents);
			this.WriteAttribute("processContents", "", this.Write34_XmlSchemaContentProcessing(xmlSchemaContentProcessing));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000A3654 File Offset: 0x000A2654
		private void Write54_XmlSchemaSequence(XmlSchemaSequence o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("sequence");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			XmlSchemaObjectCollection items = o.Items;
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					XmlSchemaObject xmlSchemaObject = items[i];
					if (xmlSchemaObject is XmlSchemaAny)
					{
						this.Write53_XmlSchemaAny((XmlSchemaAny)xmlSchemaObject);
					}
					else if (xmlSchemaObject is XmlSchemaSequence)
					{
						this.Write54_XmlSchemaSequence((XmlSchemaSequence)xmlSchemaObject);
					}
					else if (xmlSchemaObject is XmlSchemaChoice)
					{
						this.Write52_XmlSchemaChoice((XmlSchemaChoice)xmlSchemaObject);
					}
					else if (xmlSchemaObject is XmlSchemaElement)
					{
						this.Write46_XmlSchemaElement((XmlSchemaElement)xmlSchemaObject);
					}
					else if (xmlSchemaObject is XmlSchemaGroupRef)
					{
						this.Write55_XmlSchemaGroupRef((XmlSchemaGroupRef)xmlSchemaObject);
					}
				}
			}
			this.WriteEndElement();
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000A3790 File Offset: 0x000A2790
		private void Write55_XmlSchemaGroupRef(XmlSchemaGroupRef o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("group");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("minOccurs", "", XmlConvert.ToString(o.MinOccurs));
			this.WriteAttribute("maxOccurs", "", (o.MaxOccurs == decimal.MaxValue) ? "unbounded" : XmlConvert.ToString(o.MaxOccurs));
			if (!o.RefName.IsEmpty)
			{
				this.WriteAttribute("ref", "", o.RefName);
			}
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			this.WriteEndElement();
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000A3858 File Offset: 0x000A2858
		private void Write56_XmlSchemaComplexContentRestriction(XmlSchemaComplexContentRestriction o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("restriction");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttributes(o.UnhandledAttributes, o);
			if (!o.BaseTypeName.IsEmpty)
			{
				this.WriteAttribute("base", "", o.BaseTypeName);
			}
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Particle is XmlSchemaSequence)
			{
				this.Write54_XmlSchemaSequence((XmlSchemaSequence)o.Particle);
			}
			else if (o.Particle is XmlSchemaGroupRef)
			{
				this.Write55_XmlSchemaGroupRef((XmlSchemaGroupRef)o.Particle);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write52_XmlSchemaChoice((XmlSchemaChoice)o.Particle);
			}
			else if (o.Particle is XmlSchemaAll)
			{
				this.Write43_XmlSchemaAll((XmlSchemaAll)o.Particle);
			}
			this.WriteSortedItems(o.Attributes);
			this.Write33_XmlSchemaAnyAttribute(o.AnyAttribute);
			this.WriteEndElement();
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000A3964 File Offset: 0x000A2964
		private void Write57_XmlSchemaGroup(XmlSchemaGroup o)
		{
			if (o == null)
			{
				return;
			}
			this.WriteStartElement("group");
			this.WriteAttribute("id", "", o.Id);
			this.WriteAttribute("name", "", o.Name);
			this.WriteAttributes(o.UnhandledAttributes, o);
			this.Write5_XmlSchemaAnnotation(o.Annotation);
			if (o.Particle is XmlSchemaSequence)
			{
				this.Write54_XmlSchemaSequence((XmlSchemaSequence)o.Particle);
			}
			else if (o.Particle is XmlSchemaChoice)
			{
				this.Write52_XmlSchemaChoice((XmlSchemaChoice)o.Particle);
			}
			else if (o.Particle is XmlSchemaAll)
			{
				this.Write43_XmlSchemaAll((XmlSchemaAll)o.Particle);
			}
			this.WriteEndElement();
		}

		// Token: 0x040014CE RID: 5326
		private StringBuilder w = new StringBuilder();

		// Token: 0x040014CF RID: 5327
		private int indentLevel = -1;
	}
}
