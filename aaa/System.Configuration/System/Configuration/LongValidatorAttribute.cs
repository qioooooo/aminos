using System;

namespace System.Configuration
{
	// Token: 0x0200007A RID: 122
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class LongValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00014571 File Offset: 0x00013571
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new LongValidator(this._min, this._max, this._excludeRange);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x000145B0 File Offset: 0x000135B0
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x000145B8 File Offset: 0x000135B8
		public long MinValue
		{
			get
			{
				return this._min;
			}
			set
			{
				if (this._max < value)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._min = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000145DF File Offset: 0x000135DF
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x000145E7 File Offset: 0x000135E7
		public long MaxValue
		{
			get
			{
				return this._max;
			}
			set
			{
				if (this._min > value)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._max = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0001460E File Offset: 0x0001360E
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00014616 File Offset: 0x00013616
		public bool ExcludeRange
		{
			get
			{
				return this._excludeRange;
			}
			set
			{
				this._excludeRange = value;
			}
		}

		// Token: 0x0400033B RID: 827
		private long _min = long.MinValue;

		// Token: 0x0400033C RID: 828
		private long _max = long.MaxValue;

		// Token: 0x0400033D RID: 829
		private bool _excludeRange;
	}
}
