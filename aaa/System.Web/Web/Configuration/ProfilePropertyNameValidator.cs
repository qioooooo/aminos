using System;
using System.Configuration;

namespace System.Web.Configuration
{
	// Token: 0x02000229 RID: 553
	internal sealed class ProfilePropertyNameValidator : ConfigurationValidatorBase
	{
		// Token: 0x06001DB8 RID: 7608 RVA: 0x0008663F File Offset: 0x0008563F
		public override bool CanValidate(Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x00086650 File Offset: 0x00085650
		public override void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			string text = value as string;
			if (text != null)
			{
				text = text.Trim();
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException(SR.GetString("Profile_name_can_not_be_empty"));
			}
			if (text.Contains("."))
			{
				throw new ArgumentException(SR.GetString("Profile_name_can_not_contain_period"));
			}
		}

		// Token: 0x04001986 RID: 6534
		internal static ProfilePropertyNameValidator SingletonInstance = new ProfilePropertyNameValidator();
	}
}
