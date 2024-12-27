using System;

namespace System.Configuration
{
	// Token: 0x02000090 RID: 144
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class RegexStringValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x0600053E RID: 1342 RVA: 0x0001A074 File Offset: 0x00019074
		public RegexStringValidatorAttribute(string regex)
		{
			this._regex = regex;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001A083 File Offset: 0x00019083
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new RegexStringValidator(this._regex);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001A090 File Offset: 0x00019090
		public string Regex
		{
			get
			{
				return this._regex;
			}
		}

		// Token: 0x0400037B RID: 891
		private string _regex;
	}
}
