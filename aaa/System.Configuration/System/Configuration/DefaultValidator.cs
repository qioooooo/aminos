using System;

namespace System.Configuration
{
	// Token: 0x0200005B RID: 91
	public sealed class DefaultValidator : ConfigurationValidatorBase
	{
		// Token: 0x06000393 RID: 915 RVA: 0x000128BD File Offset: 0x000118BD
		public override bool CanValidate(Type type)
		{
			return true;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000128C0 File Offset: 0x000118C0
		public override void Validate(object value)
		{
		}
	}
}
