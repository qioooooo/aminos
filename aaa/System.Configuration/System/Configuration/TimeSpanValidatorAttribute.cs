using System;

namespace System.Configuration
{
	// Token: 0x020000A9 RID: 169
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class TimeSpanValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001D2C4 File Offset: 0x0001C2C4
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new TimeSpanValidator(this._min, this._max, this._excludeRange);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001D2DD File Offset: 0x0001C2DD
		public TimeSpan MinValue
		{
			get
			{
				return this._min;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001D2E5 File Offset: 0x0001C2E5
		public TimeSpan MaxValue
		{
			get
			{
				return this._max;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001D2ED File Offset: 0x0001C2ED
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x0001D300 File Offset: 0x0001C300
		public string MinValueString
		{
			get
			{
				return this._min.ToString();
			}
			set
			{
				TimeSpan timeSpan = TimeSpan.Parse(value);
				if (this._max < timeSpan)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._min = timeSpan;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0001D33E File Offset: 0x0001C33E
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x0001D354 File Offset: 0x0001C354
		public string MaxValueString
		{
			get
			{
				return this._max.ToString();
			}
			set
			{
				TimeSpan timeSpan = TimeSpan.Parse(value);
				if (this._min > timeSpan)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._max = timeSpan;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0001D392 File Offset: 0x0001C392
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0001D39A File Offset: 0x0001C39A
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

		// Token: 0x040003F5 RID: 1013
		public const string TimeSpanMinValue = "-10675199.02:48:05.4775808";

		// Token: 0x040003F6 RID: 1014
		public const string TimeSpanMaxValue = "10675199.02:48:05.4775807";

		// Token: 0x040003F7 RID: 1015
		private TimeSpan _min = TimeSpan.MinValue;

		// Token: 0x040003F8 RID: 1016
		private TimeSpan _max = TimeSpan.MaxValue;

		// Token: 0x040003F9 RID: 1017
		private bool _excludeRange;
	}
}
