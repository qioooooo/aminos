using System;

namespace System.Configuration
{
	// Token: 0x020000A0 RID: 160
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class StringValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0001CEB3 File Offset: 0x0001BEB3
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new StringValidator(this._minLength, this._maxLength, this._invalidChars);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001CECC File Offset: 0x0001BECC
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0001CED4 File Offset: 0x0001BED4
		public int MinLength
		{
			get
			{
				return this._minLength;
			}
			set
			{
				if (this._maxLength < value)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._minLength = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001CEFB File Offset: 0x0001BEFB
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0001CF03 File Offset: 0x0001BF03
		public int MaxLength
		{
			get
			{
				return this._maxLength;
			}
			set
			{
				if (this._minLength > value)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Validator_min_greater_than_max"));
				}
				this._maxLength = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001CF2A File Offset: 0x0001BF2A
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001CF32 File Offset: 0x0001BF32
		public string InvalidCharacters
		{
			get
			{
				return this._invalidChars;
			}
			set
			{
				this._invalidChars = value;
			}
		}

		// Token: 0x040003E9 RID: 1001
		private int _minLength;

		// Token: 0x040003EA RID: 1002
		private int _maxLength = int.MaxValue;

		// Token: 0x040003EB RID: 1003
		private string _invalidChars;
	}
}
