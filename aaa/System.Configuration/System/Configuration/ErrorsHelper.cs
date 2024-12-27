using System;
using System.Collections.Generic;

namespace System.Configuration
{
	// Token: 0x02000063 RID: 99
	internal static class ErrorsHelper
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00013461 File Offset: 0x00012461
		internal static int GetErrorCount(List<ConfigurationException> errors)
		{
			if (errors == null)
			{
				return 0;
			}
			return errors.Count;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001346E File Offset: 0x0001246E
		internal static bool GetHasErrors(List<ConfigurationException> errors)
		{
			return ErrorsHelper.GetErrorCount(errors) > 0;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001347C File Offset: 0x0001247C
		internal static void AddError(ref List<ConfigurationException> errors, ConfigurationException e)
		{
			if (errors == null)
			{
				errors = new List<ConfigurationException>();
			}
			ConfigurationErrorsException ex = e as ConfigurationErrorsException;
			if (ex == null)
			{
				errors.Add(e);
				return;
			}
			ICollection<ConfigurationException> errorsGeneric = ex.ErrorsGeneric;
			if (errorsGeneric.Count == 1)
			{
				errors.Add(e);
				return;
			}
			errors.AddRange(errorsGeneric);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000134C8 File Offset: 0x000124C8
		internal static void AddErrors(ref List<ConfigurationException> errors, ICollection<ConfigurationException> coll)
		{
			if (coll == null || coll.Count == 0)
			{
				return;
			}
			foreach (ConfigurationException ex in coll)
			{
				ErrorsHelper.AddError(ref errors, ex);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001351C File Offset: 0x0001251C
		internal static ConfigurationErrorsException GetErrorsException(List<ConfigurationException> errors)
		{
			if (errors == null)
			{
				return null;
			}
			return new ConfigurationErrorsException(errors);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001352C File Offset: 0x0001252C
		internal static void ThrowOnErrors(List<ConfigurationException> errors)
		{
			ConfigurationErrorsException errorsException = ErrorsHelper.GetErrorsException(errors);
			if (errorsException != null)
			{
				throw errorsException;
			}
		}
	}
}
