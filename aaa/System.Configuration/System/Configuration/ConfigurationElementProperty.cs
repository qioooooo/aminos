using System;

namespace System.Configuration
{
	// Token: 0x0200002B RID: 43
	public sealed class ConfigurationElementProperty
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0000EEEA File Offset: 0x0000DEEA
		public ConfigurationElementProperty(ConfigurationValidatorBase validator)
		{
			if (validator == null)
			{
				throw new ArgumentNullException("validator");
			}
			this._validator = validator;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000EF07 File Offset: 0x0000DF07
		public ConfigurationValidatorBase Validator
		{
			get
			{
				return this._validator;
			}
		}

		// Token: 0x0400024F RID: 591
		private ConfigurationValidatorBase _validator;
	}
}
