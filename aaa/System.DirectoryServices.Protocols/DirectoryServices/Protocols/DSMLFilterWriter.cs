using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000066 RID: 102
	internal class DSMLFilterWriter
	{
		// Token: 0x06000201 RID: 513 RVA: 0x00008788 File Offset: 0x00007788
		protected void WriteValue(string valueElt, ADValue value, XmlWriter mXmlWriter, string strNamespace)
		{
			if (strNamespace != null)
			{
				mXmlWriter.WriteStartElement(valueElt, strNamespace);
			}
			else
			{
				mXmlWriter.WriteStartElement(valueElt);
			}
			if (value.IsBinary && value.BinaryVal != null)
			{
				mXmlWriter.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:base64Binary");
				mXmlWriter.WriteBase64(value.BinaryVal, 0, value.BinaryVal.Length);
			}
			else
			{
				mXmlWriter.WriteString(value.StringVal);
			}
			mXmlWriter.WriteEndElement();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008800 File Offset: 0x00007800
		protected void WriteAttrib(string attrName, ADAttribute attrib, XmlWriter mXmlWriter, string strNamespace)
		{
			if (strNamespace != null)
			{
				mXmlWriter.WriteStartElement(attrName, strNamespace);
			}
			else
			{
				mXmlWriter.WriteStartElement(attrName);
			}
			mXmlWriter.WriteAttributeString("name", attrib.Name);
			foreach (object obj in attrib.Values)
			{
				ADValue advalue = (ADValue)obj;
				this.WriteValue("value", advalue, mXmlWriter, strNamespace);
			}
			mXmlWriter.WriteEndElement();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008890 File Offset: 0x00007890
		public void WriteFilter(ADFilter filter, bool filterTags, XmlWriter mXmlWriter, string strNamespace)
		{
			if (filterTags)
			{
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("filter", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("filter");
				}
			}
			switch (filter.Type)
			{
			case ADFilter.FilterType.And:
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("and", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("and");
				}
				foreach (object obj in filter.Filter.And)
				{
					this.WriteFilter((ADFilter)obj, false, mXmlWriter, strNamespace);
				}
				mXmlWriter.WriteEndElement();
				break;
			case ADFilter.FilterType.Or:
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("or", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("or");
				}
				foreach (object obj2 in filter.Filter.Or)
				{
					this.WriteFilter((ADFilter)obj2, false, mXmlWriter, strNamespace);
				}
				mXmlWriter.WriteEndElement();
				break;
			case ADFilter.FilterType.Not:
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("not", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("not");
				}
				this.WriteFilter(filter.Filter.Not, false, mXmlWriter, strNamespace);
				mXmlWriter.WriteEndElement();
				break;
			case ADFilter.FilterType.EqualityMatch:
				this.WriteAttrib("equalityMatch", filter.Filter.EqualityMatch, mXmlWriter, strNamespace);
				break;
			case ADFilter.FilterType.Substrings:
			{
				ADSubstringFilter substrings = filter.Filter.Substrings;
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("substrings", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("substrings");
				}
				mXmlWriter.WriteAttributeString("name", substrings.Name);
				if (substrings.Initial != null)
				{
					this.WriteValue("initial", substrings.Initial, mXmlWriter, strNamespace);
				}
				if (substrings.Any != null)
				{
					foreach (object obj3 in substrings.Any)
					{
						this.WriteValue("any", (ADValue)obj3, mXmlWriter, strNamespace);
					}
				}
				if (substrings.Final != null)
				{
					this.WriteValue("final", substrings.Final, mXmlWriter, strNamespace);
				}
				mXmlWriter.WriteEndElement();
				break;
			}
			case ADFilter.FilterType.GreaterOrEqual:
				this.WriteAttrib("greaterOrEqual", filter.Filter.GreaterOrEqual, mXmlWriter, strNamespace);
				break;
			case ADFilter.FilterType.LessOrEqual:
				this.WriteAttrib("lessOrEqual", filter.Filter.LessOrEqual, mXmlWriter, strNamespace);
				break;
			case ADFilter.FilterType.Present:
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("present", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("present");
				}
				mXmlWriter.WriteAttributeString("name", filter.Filter.Present);
				mXmlWriter.WriteEndElement();
				break;
			case ADFilter.FilterType.ApproxMatch:
				this.WriteAttrib("approxMatch", filter.Filter.ApproxMatch, mXmlWriter, strNamespace);
				break;
			case ADFilter.FilterType.ExtensibleMatch:
			{
				ADExtenMatchFilter extensibleMatch = filter.Filter.ExtensibleMatch;
				if (strNamespace != null)
				{
					mXmlWriter.WriteStartElement("extensibleMatch", strNamespace);
				}
				else
				{
					mXmlWriter.WriteStartElement("extensibleMatch");
				}
				if (extensibleMatch.Name != null && extensibleMatch.Name.Length != 0)
				{
					mXmlWriter.WriteAttributeString("name", extensibleMatch.Name);
				}
				if (extensibleMatch.MatchingRule != null && extensibleMatch.MatchingRule.Length != 0)
				{
					mXmlWriter.WriteAttributeString("matchingRule", extensibleMatch.MatchingRule);
				}
				mXmlWriter.WriteAttributeString("dnAttributes", XmlConvert.ToString(extensibleMatch.DNAttributes));
				this.WriteValue("value", extensibleMatch.Value, mXmlWriter, strNamespace);
				mXmlWriter.WriteEndElement();
				break;
			}
			default:
				throw new ArgumentException(Res.GetString("InvalidFilterType", new object[] { filter.Type }));
			}
			if (filterTags)
			{
				mXmlWriter.WriteEndElement();
			}
		}
	}
}
