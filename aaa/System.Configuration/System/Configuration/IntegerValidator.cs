using System;

namespace System.Configuration
{
	// Token: 0x0200006F RID: 111
	public class IntegerValidator : ConfigurationValidatorBase
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00013F68 File Offset: 0x00012F68
		public IntegerValidator(int minValue, int maxValue)
			: this(minValue, maxValue, false, 1)
		{
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00013F74 File Offset: 0x00012F74
		public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive)
			: this(minValue, maxValue, rangeIsExclusive, 1)
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00013F80 File Offset: 0x00012F80
		public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive, int resolution)
		{
			if (resolution <= 0)
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
			this._flags = (rangeIsExclusive ? IntegerValidator.ValidationFlags.ExclusiveRange : IntegerValidator.ValidationFlags.None);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00013FFC File Offset: 0x00012FFC
		public override bool CanValidate(Type type)
		{
			return type == typeof(int);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001400B File Offset: 0x0001300B
		public override void Validate(object value)
		{
			ValidatorUtils.HelperParamValidation(value, typeof(int));
			ValidatorUtils.ValidateScalar<int>((int)value, this._minValue, this._maxValue, this._resolution, this._flags == IntegerValidator.ValidationFlags.ExclusiveRange);
		}

		// Token: 0x0400031B RID: 795
		private IntegerValidator.ValidationFlags _flags;

		// Token: 0x0400031C RID: 796
		private int _minValue = int.MinValue;

		// Token: 0x0400031D RID: 797
		private int _maxValue = int.MaxValue;

		// Token: 0x0400031E RID: 798
		private int _resolution = 1;

		// Token: 0x02000070 RID: 112
		private enum ValidationFlags
		{
			// Token: 0x04000320 RID: 800
			None,
			// Token: 0x04000321 RID: 801
			ExclusiveRange
		}
	}
}
