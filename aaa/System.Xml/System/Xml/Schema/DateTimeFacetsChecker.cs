using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001FA RID: 506
	internal class DateTimeFacetsChecker : FacetsChecker
	{
		// Token: 0x0600183C RID: 6204 RVA: 0x0006C4A4 File Offset: 0x0006B4A4
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			DateTime dateTime = datatype.ValueConverter.ToDateTime(value);
			return this.CheckValueFacets(dateTime, datatype);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0006C4C8 File Offset: 0x0006B4C8
		internal override Exception CheckValueFacets(DateTime value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if ((restrictionFlags & RestrictionFlags.MaxInclusive) != (RestrictionFlags)0 && datatype.Compare(value, (DateTime)restriction.MaxInclusive) > 0)
			{
				return new XmlSchemaException("Sch_MaxInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxExclusive) != (RestrictionFlags)0 && datatype.Compare(value, (DateTime)restriction.MaxExclusive) >= 0)
			{
				return new XmlSchemaException("Sch_MaxExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinInclusive) != (RestrictionFlags)0 && datatype.Compare(value, (DateTime)restriction.MinInclusive) < 0)
			{
				return new XmlSchemaException("Sch_MinInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinExclusive) != (RestrictionFlags)0 && datatype.Compare(value, (DateTime)restriction.MinExclusive) <= 0)
			{
				return new XmlSchemaException("Sch_MinExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, datatype))
			{
				return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
			}
			return null;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0006C5ED File Offset: 0x0006B5ED
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration(datatype.ValueConverter.ToDateTime(value), enumeration, datatype);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0006C604 File Offset: 0x0006B604
		private bool MatchEnumeration(DateTime value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			foreach (object obj in enumeration)
			{
				DateTime dateTime = (DateTime)obj;
				if (datatype.Compare(value, dateTime) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
