using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001FC RID: 508
	internal class QNameFacetsChecker : FacetsChecker
	{
		// Token: 0x06001849 RID: 6217 RVA: 0x0006C93C File Offset: 0x0006B93C
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)datatype.ValueConverter.ChangeType(value, typeof(XmlQualifiedName));
			return this.CheckValueFacets(xmlQualifiedName, datatype);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0006C970 File Offset: 0x0006B970
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

		// Token: 0x0600184B RID: 6219 RVA: 0x0006CA25 File Offset: 0x0006BA25
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration((XmlQualifiedName)datatype.ValueConverter.ChangeType(value, typeof(XmlQualifiedName)), enumeration);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0006CA4C File Offset: 0x0006BA4C
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
