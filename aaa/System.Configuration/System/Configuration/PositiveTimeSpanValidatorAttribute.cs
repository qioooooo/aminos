using System;

namespace System.Configuration
{
	// Token: 0x02000082 RID: 130
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class PositiveTimeSpanValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00018F58 File Offset: 0x00017F58
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new PositiveTimeSpanValidator();
			}
		}
	}
}
