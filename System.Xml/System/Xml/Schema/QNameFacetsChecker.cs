using System;
using System.Collections;

namespace System.Xml.Schema
{
	internal class QNameFacetsChecker : FacetsChecker
	{
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)datatype.ValueConverter.ChangeType(value, typeof(XmlQualifiedName));
			return this.CheckValueFacets(xmlQualifiedName, datatype);
		}

		internal override Exception CheckValueFacets(XmlQualifiedName value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if (restrictionFlags != (RestrictionFlags)0)
			{
				string text = value.ToString();
				int length = text.Length;
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
				if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration))
				{
					return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
				}
			}
			return null;
		}

		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration((XmlQualifiedName)datatype.ValueConverter.ChangeType(value, typeof(XmlQualifiedName)), enumeration);
		}

		private bool MatchEnumeration(XmlQualifiedName value, ArrayList enumeration)
		{
			foreach (object obj in enumeration)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
				if (value.Equals(xmlQualifiedName))
				{
					return true;
				}
			}
			return false;
		}
	}
}
