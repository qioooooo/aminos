using System;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000B1 RID: 177
	internal static class ValidatorUtils
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x0001DCD2 File Offset: 0x0001CCD2
		public static void HelperParamValidation(object value, Type allowedType)
		{
			if (value == null)
			{
				return;
			}
			if (value.GetType() != allowedType)
			{
				throw new ArgumentException(SR.GetString("Validator_value_type_invalid"), string.Empty);
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001DCF6 File Offset: 0x0001CCF6
		public static void ValidateScalar<T>(T value, T min, T max, T resolution, bool exclusiveRange) where T : IComparable<T>
		{
			ValidatorUtils.ValidateRangeImpl<T>(value, min, max, exclusiveRange);
			ValidatorUtils.ValidateResolution(resolution.ToString(), Convert.ToInt64(value, CultureInfo.InvariantCulture), Convert.ToInt64(resolution, CultureInfo.InvariantCulture));
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001DD34 File Offset: 0x0001CD34
		private static void ValidateRangeImpl<T>(T value, T min, T max, bool exclusiveRange) where T : IComparable<T>
		{
			IComparable<T> comparable = value;
			bool flag = false;
			if (comparable.CompareTo(min) >= 0)
			{
				flag = true;
			}
			if (flag && comparable.CompareTo(max) > 0)
			{
				flag = false;
			}
			if (!(flag ^ exclusiveRange))
			{
				string text;
				if (min.Equals(max))
				{
					if (exclusiveRange)
					{
						text = SR.GetString("Validation_scalar_range_violation_not_different");
					}
					else
					{
						text = SR.GetString("Validation_scalar_range_violation_not_equal");
					}
				}
				else if (exclusiveRange)
				{
					text = SR.GetString("Validation_scalar_range_violation_not_outside_range");
				}
				else
				{
					text = SR.GetString("Validation_scalar_range_violation_not_in_range");
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, text, new object[]
				{
					min.ToString(),
					max.ToString()
				}));
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001DDF8 File Offset: 0x0001CDF8
		private static void ValidateResolution(string resolutionAsString, long value, long resolution)
		{
			if (value % resolution != 0L)
			{
				throw new ArgumentException(SR.GetString("Validator_scalar_resolution_violation", new object[] { resolutionAsString }));
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001DE28 File Offset: 0x0001CE28
		public static void ValidateScalar(TimeSpan value, TimeSpan min, TimeSpan max, long resolutionInSeconds, bool exclusiveRange)
		{
			ValidatorUtils.ValidateRangeImpl<TimeSpan>(value, min, max, exclusiveRange);
			if (resolutionInSeconds > 0L)
			{
				ValidatorUtils.ValidateResolution(TimeSpan.FromSeconds((double)resolutionInSeconds).ToString(), value.Ticks, resolutionInSeconds * 10000000L);
			}
		}
	}
}
