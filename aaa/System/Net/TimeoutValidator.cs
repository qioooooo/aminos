using System;
using System.Configuration;

namespace System.Net
{
	// Token: 0x02000669 RID: 1641
	internal sealed class TimeoutValidator : ConfigurationValidatorBase
	{
		// Token: 0x060032C0 RID: 12992 RVA: 0x000D73A2 File Offset: 0x000D63A2
		internal TimeoutValidator(bool zeroValid)
		{
			this._zeroValid = zeroValid;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000D73B1 File Offset: 0x000D63B1
		public override bool CanValidate(Type type)
		{
			return type == typeof(int) || type == typeof(long);
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000D73D0 File Offset: 0x000D63D0
		public override void Validate(object value)
		{
			if (value == null)
			{
				return;
			}
			int num = (int)value;
			if (this._zeroValid && num == 0)
			{
				return;
			}
			if (num <= 0 && num != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_io_timeout_use_gt_zero"));
			}
		}

		// Token: 0x04002F67 RID: 12135
		private bool _zeroValid;
	}
}
