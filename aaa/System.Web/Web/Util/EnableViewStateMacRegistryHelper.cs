using System;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Web.Util
{
	// Token: 0x02000760 RID: 1888
	internal static class EnableViewStateMacRegistryHelper
	{
		// Token: 0x06005BE0 RID: 23520 RVA: 0x00170790 File Offset: 0x0016F790
		static EnableViewStateMacRegistryHelper()
		{
			bool flag = EnableViewStateMacRegistryHelper.IsMacEnforcementEnabledViaRegistry();
			if (flag)
			{
				EnableViewStateMacRegistryHelper.EnforceViewStateMac = true;
				EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsFromCrossPagePostbacks = true;
			}
			if (AppSettings.AllowInsecureDeserialization != null)
			{
				EnableViewStateMacRegistryHelper.EnforceViewStateMac = !AppSettings.AllowInsecureDeserialization.Value;
				EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsFromCrossPagePostbacks |= !AppSettings.AllowInsecureDeserialization.Value;
			}
			EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsAlways = AppSettings.AlwaysIgnoreViewStateValidationErrors;
			if (EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsAlways)
			{
				EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsFromCrossPagePostbacks = true;
				return;
			}
			if (EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsFromCrossPagePostbacks)
			{
				EnableViewStateMacRegistryHelper.WriteViewStateGeneratorField = true;
			}
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x00170818 File Offset: 0x0016F818
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool IsMacEnforcementEnabledViaRegistry()
		{
			bool flag;
			try
			{
				string text = string.Format(CultureInfo.InvariantCulture, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework\\v{0}", new object[] { Environment.Version.ToString(3) });
				int num = (int)Registry.GetValue(text, "AspNetEnforceViewStateMac", 0);
				flag = num != 0;
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x04003123 RID: 12579
		public static readonly bool EnforceViewStateMac;

		// Token: 0x04003124 RID: 12580
		public static readonly bool SuppressMacValidationErrorsAlways;

		// Token: 0x04003125 RID: 12581
		public static readonly bool SuppressMacValidationErrorsFromCrossPagePostbacks;

		// Token: 0x04003126 RID: 12582
		public static readonly bool WriteViewStateGeneratorField;
	}
}
