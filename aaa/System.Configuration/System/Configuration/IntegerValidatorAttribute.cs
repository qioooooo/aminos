using System;

namespace System.Configuration
{
	// Token: 0x02000071 RID: 113
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class IntegerValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00014061 File Offset: 0x00013061
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new IntegerValidator(this._min, this._max, this._excludeRange);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001407A File Offset: 0x0001307A
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00014082 File Offset: 0x00013082
		public int MinValue
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000140A9 File Offset: 0x000130A9
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x000140B1 File Offset: 0x000130B1
		public int MaxValue
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x000140D8 File Offset: 0x000130D8
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000140E0 File Offset: 0x000130E0
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

		// Token: 0x04000322 RID: 802
		private int _min = int.MinValue;

		// Token: 0x04000323 RID: 803
		private int _max = int.MaxValue;

		// Token: 0x04000324 RID: 804
		private bool _excludeRange;
	}
}
