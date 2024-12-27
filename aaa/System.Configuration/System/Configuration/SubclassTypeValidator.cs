using System;

namespace System.Configuration
{
	// Token: 0x020000A1 RID: 161
	public sealed class SubclassTypeValidator : ConfigurationValidatorBase
	{
		// Token: 0x06000648 RID: 1608 RVA: 0x0001CF3B File Offset: 0x0001BF3B
		public SubclassTypeValidator(Type baseClass)
		{
			if (baseClass == null)
			{
				throw new ArgumentNullException("baseClass");
			}
			this._base = baseClass;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001CF58 File Offset: 0x0001BF58
		public override bool CanValidate(Type type)
		{
			return type == typeof(Type);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001CF68 File Offset: 0x0001BF68
		public override void Validate(object value)
		{
			if (value == null)
			{
				return;
			}
			if (!(value is Type))
			{
				ValidatorUtils.HelperParamValidation(value, typeof(Type));
			}
			if (!this._base.IsAssignableFrom((Type)value))
			{
				throw new ArgumentException(SR.GetString("Subclass_validator_error", new object[]
				{
					((Type)value).FullName,
					this._base.FullName
				}));
			}
		}

		// Token: 0x040003EC RID: 1004
		private Type _base;
	}
}
