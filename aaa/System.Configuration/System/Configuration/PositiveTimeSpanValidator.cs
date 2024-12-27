using System;

namespace System.Configuration
{
	// Token: 0x02000081 RID: 129
	public class PositiveTimeSpanValidator : ConfigurationValidatorBase
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00018F07 File Offset: 0x00017F07
		public override bool CanValidate(Type type)
		{
			return type == typeof(TimeSpan);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00018F16 File Offset: 0x00017F16
		public override void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if ((TimeSpan)value <= TimeSpan.Zero)
			{
				throw new ArgumentException(SR.GetString("Validator_timespan_value_must_be_positive"));
			}
		}
	}
}
