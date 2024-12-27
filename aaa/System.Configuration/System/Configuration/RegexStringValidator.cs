using System;
using System.Text.RegularExpressions;

namespace System.Configuration
{
	// Token: 0x0200008F RID: 143
	public class RegexStringValidator : ConfigurationValidatorBase
	{
		// Token: 0x0600053B RID: 1339 RVA: 0x00019FD7 File Offset: 0x00018FD7
		public RegexStringValidator(string regex)
		{
			if (string.IsNullOrEmpty(regex))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("regex");
			}
			this._expression = regex;
			this._regex = new Regex(regex, RegexOptions.Compiled);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001A006 File Offset: 0x00019006
		public override bool CanValidate(Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001A018 File Offset: 0x00019018
		public override void Validate(object value)
		{
			ValidatorUtils.HelperParamValidation(value, typeof(string));
			if (value == null)
			{
				return;
			}
			Match match = this._regex.Match((string)value);
			if (!match.Success)
			{
				throw new ArgumentException(SR.GetString("Regex_validator_error", new object[] { this._expression }));
			}
		}

		// Token: 0x04000379 RID: 889
		private string _expression;

		// Token: 0x0400037A RID: 890
		private Regex _regex;
	}
}
