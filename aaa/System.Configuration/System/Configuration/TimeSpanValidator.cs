using System;

namespace System.Configuration
{
	// Token: 0x020000A7 RID: 167
	public class TimeSpanValidator : ConfigurationValidatorBase
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x0001D1CA File Offset: 0x0001C1CA
		public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue)
			: this(minValue, maxValue, false, 0L)
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001D1D7 File Offset: 0x0001C1D7
		public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive)
			: this(minValue, maxValue, rangeIsExclusive, 0L)
		{
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001D1E4 File Offset: 0x0001C1E4
		public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive, long resolutionInSeconds)
		{
			if (resolutionInSeconds < 0L)
			{
				throw new ArgumentOutOfRangeException("resolutionInSeconds");
			}
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue", SR.GetString("Validator_min_greater_than_max"));
			}
			this._minValue = minValue;
			this._maxValue = maxValue;
			this._resolution = resolutionInSeconds;
			this._flags = (rangeIsExclusive ? TimeSpanValidator.ValidationFlags.ExclusiveRange : TimeSpanValidator.ValidationFlags.None);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001D25F File Offset: 0x0001C25F
		public override bool CanValidate(Type type)
		{
			return type == typeof(TimeSpan);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001D26E File Offset: 0x0001C26E
		public override void Validate(object value)
		{
			ValidatorUtils.HelperParamValidation(value, typeof(TimeSpan));
			ValidatorUtils.ValidateScalar((TimeSpan)value, this._minValue, this._maxValue, this._resolution, this._flags == TimeSpanValidator.ValidationFlags.ExclusiveRange);
		}

		// Token: 0x040003EE RID: 1006
		private TimeSpanValidator.ValidationFlags _flags;

		// Token: 0x040003EF RID: 1007
		private TimeSpan _minValue = TimeSpan.MinValue;

		// Token: 0x040003F0 RID: 1008
		private TimeSpan _maxValue = TimeSpan.MaxValue;

		// Token: 0x040003F1 RID: 1009
		private long _resolution;

		// Token: 0x020000A8 RID: 168
		private enum ValidationFlags
		{
			// Token: 0x040003F3 RID: 1011
			None,
			// Token: 0x040003F4 RID: 1012
			ExclusiveRange
		}
	}
}
