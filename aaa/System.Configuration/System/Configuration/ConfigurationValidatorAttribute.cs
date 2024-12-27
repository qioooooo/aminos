using System;

namespace System.Configuration
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Property)]
	public class ConfigurationValidatorAttribute : Attribute
	{
		// Token: 0x060000FB RID: 251 RVA: 0x0000A96E File Offset: 0x0000996E
		protected ConfigurationValidatorAttribute()
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000A978 File Offset: 0x00009978
		public ConfigurationValidatorAttribute(Type validator)
		{
			if (validator == null)
			{
				throw new ArgumentNullException("validator");
			}
			if (!typeof(ConfigurationValidatorBase).IsAssignableFrom(validator))
			{
				throw new ArgumentException(SR.GetString("Validator_Attribute_param_not_validator", new object[] { "ConfigurationValidatorBase" }));
			}
			this._validator = validator;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000A9D2 File Offset: 0x000099D2
		public virtual ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return (ConfigurationValidatorBase)TypeUtil.CreateInstanceRestricted(this._declaringType, this._validator);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000A9EA File Offset: 0x000099EA
		public Type ValidatorType
		{
			get
			{
				return this._validator;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000A9F2 File Offset: 0x000099F2
		internal void SetDeclaringType(Type declaringType)
		{
			if (declaringType == null)
			{
				return;
			}
			if (this._declaringType == null)
			{
				this._declaringType = declaringType;
				return;
			}
			if (this._declaringType != declaringType)
			{
			}
		}

		// Token: 0x040001D0 RID: 464
		internal Type _declaringType;

		// Token: 0x040001D1 RID: 465
		private readonly Type _validator;
	}
}
