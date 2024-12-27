using System;

namespace System.Configuration
{
	// Token: 0x0200009F RID: 159
	public class StringValidator : ConfigurationValidatorBase
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0001CD53 File Offset: 0x0001BD53
		public StringValidator(int minLength)
			: this(minLength, int.MaxValue, null)
		{
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001CD62 File Offset: 0x0001BD62
		public StringValidator(int minLength, int maxLength)
			: this(minLength, maxLength, null)
		{
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001CD6D File Offset: 0x0001BD6D
		public StringValidator(int minLength, int maxLength, string invalidCharacters)
		{
			this._minLength = minLength;
			this._maxLength = maxLength;
			this._invalidChars = invalidCharacters;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001CD8A File Offset: 0x0001BD8A
		public override bool CanValidate(Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001CD9C File Offset: 0x0001BD9C
		public override void Validate(object value)
		{
			ValidatorUtils.HelperParamValidation(value, typeof(string));
			string text = value as string;
			int num = ((text == null) ? 0 : text.Length);
			if (num < this._minLength)
			{
				throw new ArgumentException(SR.GetString("Validator_string_min_length", new object[] { this._minLength }));
			}
			if (num > this._maxLength)
			{
				throw new ArgumentException(SR.GetString("Validator_string_max_length", new object[] { this._maxLength }));
			}
			if (num > 0 && this._invalidChars != null && this._invalidChars.Length > 0)
			{
				char[] array = new char[this._invalidChars.Length];
				this._invalidChars.CopyTo(0, array, 0, this._invalidChars.Length);
				if (text.IndexOfAny(array) != -1)
				{
					throw new ArgumentException(SR.GetString("Validator_string_invalid_chars", new object[] { this._invalidChars }));
				}
			}
		}

		// Token: 0x040003E6 RID: 998
		private int _minLength;

		// Token: 0x040003E7 RID: 999
		private int _maxLength;

		// Token: 0x040003E8 RID: 1000
		private string _invalidChars;
	}
}
