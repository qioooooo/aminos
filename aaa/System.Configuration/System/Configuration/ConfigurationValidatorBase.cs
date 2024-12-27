using System;

namespace System.Configuration
{
	// Token: 0x02000013 RID: 19
	public abstract class ConfigurationValidatorBase
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000A8FD File Offset: 0x000098FD
		public virtual bool CanValidate(Type type)
		{
			return false;
		}

		// Token: 0x060000F5 RID: 245
		public abstract void Validate(object value);
	}
}
