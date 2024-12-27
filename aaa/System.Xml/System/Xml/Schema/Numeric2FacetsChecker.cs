using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001F8 RID: 504
	internal class Numeric2FacetsChecker : FacetsChecker
	{
		// Token: 0x06001831 RID: 6193 RVA: 0x0006C158 File Offset: 0x0006B158
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			double num = datatype.ValueConverter.ToDouble(value);
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0006C17C File Offset: 0x0006B17C
		internal override Exception CheckValueFacets(double value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			XmlValueConverter valueConverter = datatype.ValueConverter;
			if ((restrictionFlags & RestrictionFlags.MaxInclusive) != (RestrictionFlags)0 && value > valueConverter.ToDouble(restriction.MaxInclusive))
			{
				return new XmlSchemaException("Sch_MaxInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxExclusive) != (RestrictionFlags)0 && value >= valueConverter.ToDouble(restriction.MaxExclusive))
			{
				return new XmlSchemaException("Sch_MaxExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinInclusive) != (RestrictionFlags)0 && value < valueConverter.ToDouble(restriction.MinInclusive))
			{
				return new XmlSchemaException("Sch_MinInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinExclusive) != (RestrictionFlags)0 && value <= valueConverter.ToDouble(restriction.MinExclusive))
			{
				return new XmlSchemaException("Sch_MinExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, valueConverter))
			{
				return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
			}
			return null;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0006C268 File Offset: 0x0006B268
		internal override Exception CheckValueFacets(float value, XmlSchemaDatatype datatype)
		{
			double num = (double)value;
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0006C280 File Offset: 0x0006B280
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration(datatype.ValueConverter.ToDouble(value), enumeration, datatype.ValueConverter);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0006C29C File Offset: 0x0006B29C
		private bool MatchEnumeration(double value, ArrayList enumeration, XmlValueConverter valueConverter)
		{
			foreach (object obj in enumeration)
			{
				if (value == valueConverter.ToDouble(obj))
				{
					return true;
				}
			}
			return false;
		}
	}
}
