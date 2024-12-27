using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200020D RID: 525
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class MachineKeyValidationConverter : ConfigurationConverterBase
	{
		// Token: 0x06001C3A RID: 7226 RVA: 0x000810FC File Offset: 0x000800FC
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			if (value != null && value.GetType() != typeof(MachineKeyValidation))
			{
				throw new ArgumentException(SR.GetString("Invalid_enum_value", new object[] { "SHA1, MD5, 3DES, AES" }));
			}
			switch ((MachineKeyValidation)value)
			{
			case MachineKeyValidation.MD5:
				return "MD5";
			case MachineKeyValidation.SHA1:
				return "SHA1";
			case MachineKeyValidation.TripleDES:
				return "3DES";
			case MachineKeyValidation.AES:
				return "AES";
			default:
				throw new ArgumentOutOfRangeException("value");
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00081180 File Offset: 0x00080180
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			string text = (string)data;
			string text2;
			if ((text2 = text) != null)
			{
				if (text2 == "SHA1")
				{
					return MachineKeyValidation.SHA1;
				}
				if (text2 == "MD5")
				{
					return MachineKeyValidation.MD5;
				}
				if (text2 == "3DES")
				{
					return MachineKeyValidation.TripleDES;
				}
				if (text2 == "AES")
				{
					return MachineKeyValidation.AES;
				}
			}
			throw new ArgumentException(SR.GetString("Config_Invalid_enum_value", new object[] { "SHA1, MD5, 3DES, AES" }));
		}
	}
}
