using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace System.Xml.Schema
{
	// Token: 0x020001FB RID: 507
	internal class StringFacetsChecker : FacetsChecker
	{
		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x0006C674 File Offset: 0x0006B674
		private static Regex LanguagePattern
		{
			get
			{
				if (StringFacetsChecker.languagePattern == null)
				{
					Regex regex = new Regex("^([a-zA-Z]{1,8})(-[a-zA-Z0-9]{1,8})*$", RegexOptions.None);
					Interlocked.CompareExchange<Regex>(ref StringFacetsChecker.languagePattern, regex, null);
				}
				return StringFacetsChecker.languagePattern;
			}
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0006C6A8 File Offset: 0x0006B6A8
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			string text = datatype.ValueConverter.ToString(value);
			return this.CheckValueFacets(text, datatype, true);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0006C6CB File Offset: 0x0006B6CB
		internal override Exception CheckValueFacets(string value, XmlSchemaDatatype datatype)
		{
			return this.CheckValueFacets(value, datatype, true);
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0006C6D8 File Offset: 0x0006B6D8
		internal Exception CheckValueFacets(string value, XmlSchemaDatatype datatype, bool verifyUri)
		{
			int length = value.Length;
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			Exception ex = this.CheckBuiltInFacets(value, datatype.TypeCode, verifyUri);
			if (ex != null)
			{
				return ex;
			}
			if (restrictionFlags != (RestrictionFlags)0)
			{
				if ((restrictionFlags & RestrictionFlags.Length) != (RestrictionFlags)0 && restriction.Length != length)
				{
					return new XmlSchemaException("Sch_LengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.MinLength) != (RestrictionFlags)0 && length < restriction.MinLength)
				{
					return new XmlSchemaException("Sch_MinLengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.MaxLength) != (RestrictionFlags)0 && restriction.MaxLength < length)
				{
					return new XmlSchemaException("Sch_MaxLengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, datatype))
				{
					return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
				}
			}
			return null;
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0006C79B File Offset: 0x0006B79B
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration(datatype.ValueConverter.ToString(value), enumeration, datatype);
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0006C7B4 File Offset: 0x0006B7B4
		private bool MatchEnumeration(string value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			if (datatype.TypeCode == XmlTypeCode.AnyUri)
			{
				using (IEnumerator enumerator = enumeration.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Uri uri = (Uri)obj;
						if (value.Equals(uri.OriginalString))
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (object obj2 in enumeration)
			{
				string text = (string)obj2;
				if (value.Equals(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0006C874 File Offset: 0x0006B874
		private Exception CheckBuiltInFacets(string s, XmlTypeCode typeCode, bool verifyUri)
		{
			Exception ex = null;
			switch (typeCode)
			{
			case XmlTypeCode.AnyUri:
				if (verifyUri)
				{
					Uri uri;
					ex = XmlConvert.TryToUri(s, out uri);
				}
				break;
			case XmlTypeCode.NormalizedString:
				ex = XmlConvert.TryVerifyNormalizedString(s);
				break;
			case XmlTypeCode.Token:
				ex = XmlConvert.TryVerifyTOKEN(s);
				break;
			case XmlTypeCode.Language:
				if (s == null || s.Length == 0)
				{
					return new XmlSchemaException("Sch_EmptyAttributeValue", string.Empty);
				}
				if (!StringFacetsChecker.LanguagePattern.IsMatch(s))
				{
					return new XmlSchemaException("Sch_InvalidLanguageId", string.Empty);
				}
				break;
			case XmlTypeCode.NmToken:
				ex = XmlConvert.TryVerifyNMTOKEN(s);
				break;
			case XmlTypeCode.Name:
				ex = XmlConvert.TryVerifyName(s);
				break;
			case XmlTypeCode.NCName:
			case XmlTypeCode.Id:
			case XmlTypeCode.Idref:
			case XmlTypeCode.Entity:
				ex = XmlConvert.TryVerifyNCName(s);
				break;
			}
			return ex;
		}

		// Token: 0x04000E4F RID: 3663
		private static Regex languagePattern;
	}
}
