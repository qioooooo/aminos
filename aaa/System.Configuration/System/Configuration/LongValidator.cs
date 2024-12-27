using System;

namespace System.Configuration
{
	// Token: 0x02000078 RID: 120
	public class LongValidator : ConfigurationValidatorBase
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x00014487 File Offset: 0x00013487
		public LongValidator(long minValue, long maxValue)
			: this(minValue, maxValue, false, 1L)
		{
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00014494 File Offset: 0x00013494
		public LongValidator(long minValue, long maxValue, bool rangeIsExclusive)
			: this(minValue, maxValue, rangeIsExclusive, 1L)
		{
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000144A4 File Offset: 0x000134A4
		public LongValidator(long minValue, long maxValue, bool rangeIsExclusive, long resolution)
		{
			if (resolution <= 0L)
			{
				throw new ArgumentOutOfRangeException("resolution");
			}
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue", SR.GetString("Validator_min_greater_than_max"));
			}
			this._minValue = minValue;
			this._maxValue = maxValue;
			this._resolution = resolution;
			this._flags = (rangeIsExclusive ? LongValidator.ValidationFlags.ExclusiveRange : LongValidator.ValidationFlags.None);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001452A File Offset: 0x0001352A
		public override bool CanValidate(Type type)
		{
			return type == typeof(long);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00014539 File Offset: 0x00013539
		public override void Validate(object value)
		{
			ValidatorUtils.HelperParamValidation(value, typeof(long));
			ValidatorUtils.ValidateScalar<long>((long)value, this._minValue, this._maxValue, this._resolution, this._flags == LongValidator.ValidationFlags.ExclusiveRange);
		}

		// Token: 0x04000334 RID: 820
		private LongValidator.ValidationFlags _flags;

		// Token: 0x04000335 RID: 821
		private long _minValue = long.MinValue;

		// Token: 0x04000336 RID: 822
		private long _maxValue = long.MaxValue;

		// Token: 0x04000337 RID: 823
		private long _resolution = 1L;

		// Token: 0x02000079 RID: 121
		private enum ValidationFlags
		{
			// Token: 0x04000339 RID: 825
			None,
			// Token: 0x0400033A RID: 826
			ExclusiveRange
		}
	}
}
