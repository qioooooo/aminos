using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Schema
{
	// Token: 0x020001F7 RID: 503
	internal class Numeric10FacetsChecker : FacetsChecker
	{
		// Token: 0x06001826 RID: 6182 RVA: 0x0006BDFE File Offset: 0x0006ADFE
		internal Numeric10FacetsChecker(decimal minVal, decimal maxVal)
		{
			this.minValue = minVal;
			this.maxValue = maxVal;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0006BE14 File Offset: 0x0006AE14
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			decimal num = datatype.ValueConverter.ToDecimal(value);
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0006BE38 File Offset: 0x0006AE38
		internal override Exception CheckValueFacets(decimal value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			XmlValueConverter valueConverter = datatype.ValueConverter;
			if (value > this.maxValue || value < this.minValue)
			{
				return new OverflowException(Res.GetString("XmlConvert_Overflow", new object[]
				{
					value.ToString(CultureInfo.InvariantCulture),
					datatype.TypeCodeString
				}));
			}
			if (restrictionFlags == (RestrictionFlags)0)
			{
				return null;
			}
			if ((restrictionFlags & RestrictionFlags.MaxInclusive) != (RestrictionFlags)0 && value > valueConverter.ToDecimal(restriction.MaxInclusive))
			{
				return new XmlSchemaException("Sch_MaxInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxExclusive) != (RestrictionFlags)0 && value >= valueConverter.ToDecimal(restriction.MaxExclusive))
			{
				return new XmlSchemaException("Sch_MaxExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinInclusive) != (RestrictionFlags)0 && value < valueConverter.ToDecimal(restriction.MinInclusive))
			{
				return new XmlSchemaException("Sch_MinInclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinExclusive) != (RestrictionFlags)0 && value <= valueConverter.ToDecimal(restriction.MinExclusive))
			{
				return new XmlSchemaException("Sch_MinExclusiveConstraintFailed", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, valueConverter))
			{
				return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
			}
			return this.CheckTotalAndFractionDigits(value, restriction.TotalDigits, restriction.FractionDigits, (restrictionFlags & RestrictionFlags.TotalDigits) != (RestrictionFlags)0, (restrictionFlags & RestrictionFlags.FractionDigits) != (RestrictionFlags)0);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0006BFB8 File Offset: 0x0006AFB8
		internal override Exception CheckValueFacets(long value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0006BFD4 File Offset: 0x0006AFD4
		internal override Exception CheckValueFacets(int value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0006BFF0 File Offset: 0x0006AFF0
		internal override Exception CheckValueFacets(short value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0006C00C File Offset: 0x0006B00C
		internal override Exception CheckValueFacets(byte value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0006C028 File Offset: 0x0006B028
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration(datatype.ValueConverter.ToDecimal(value), enumeration, datatype.ValueConverter);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0006C044 File Offset: 0x0006B044
		internal bool MatchEnumeration(decimal value, ArrayList enumeration, XmlValueConverter valueConverter)
		{
			foreach (object obj in enumeration)
			{
				if (value == valueConverter.ToDecimal(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0006C0A4 File Offset: 0x0006B0A4
		internal Exception CheckTotalAndFractionDigits(decimal value, int totalDigits, int fractionDigits, bool checkTotal, bool checkFraction)
		{
			decimal num = FacetsChecker.Power(10, totalDigits) - 1m;
			int num2 = 0;
			if (value < 0m)
			{
				value = decimal.Negate(value);
			}
			while (decimal.Truncate(value) != value)
			{
				value *= 10m;
				num2++;
			}
			if (checkTotal && (value > num || num2 > totalDigits))
			{
				return new XmlSchemaException("Sch_TotalDigitsConstraintFailed", string.Empty);
			}
			if (checkFraction && num2 > fractionDigits)
			{
				return new XmlSchemaException("Sch_FractionDigitsConstraintFailed", string.Empty);
			}
			return null;
		}

		// Token: 0x04000E4C RID: 3660
		private static readonly char[] signs = new char[] { '+', '-' };

		// Token: 0x04000E4D RID: 3661
		private decimal maxValue;

		// Token: 0x04000E4E RID: 3662
		private decimal minValue;
	}
}
