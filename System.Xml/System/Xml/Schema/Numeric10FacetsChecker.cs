using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Schema
{
	internal class Numeric10FacetsChecker : FacetsChecker
	{
		internal Numeric10FacetsChecker(decimal minVal, decimal maxVal)
		{
			this.minValue = minVal;
			this.maxValue = maxVal;
		}

		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			decimal num = datatype.ValueConverter.ToDecimal(value);
			return this.CheckValueFacets(num, datatype);
		}

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

		internal override Exception CheckValueFacets(long value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		internal override Exception CheckValueFacets(int value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		internal override Exception CheckValueFacets(short value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		internal override Exception CheckValueFacets(byte value, XmlSchemaDatatype datatype)
		{
			decimal num = value;
			return this.CheckValueFacets(num, datatype);
		}

		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration(datatype.ValueConverter.ToDecimal(value), enumeration, datatype.ValueConverter);
		}

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

		private static readonly char[] signs = new char[] { '+', '-' };

		private decimal maxValue;

		private decimal minValue;
	}
}
