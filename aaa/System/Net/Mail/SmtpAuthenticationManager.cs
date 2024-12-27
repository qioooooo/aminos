using System;
using System.Collections;

namespace System.Net.Mail
{
	// Token: 0x020006B8 RID: 1720
	internal static class SmtpAuthenticationManager
	{
		// Token: 0x06003519 RID: 13593 RVA: 0x000E1B76 File Offset: 0x000E0B76
		static SmtpAuthenticationManager()
		{
			if (ComNetOS.IsWin2K)
			{
				SmtpAuthenticationManager.Register(new SmtpNegotiateAuthenticationModule());
			}
			SmtpAuthenticationManager.Register(new SmtpNtlmAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpDigestAuthenticationModule());
			SmtpAuthenticationManager.Register(new SmtpLoginAuthenticationModule());
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000E1BB4 File Offset: 0x000E0BB4
		internal static void Register(ISmtpAuthenticationModule module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			lock (SmtpAuthenticationManager.modules)
			{
				SmtpAuthenticationManager.modules.Add(module);
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000E1C00 File Offset: 0x000E0C00
		internal static ISmtpAuthenticationModule[] GetModules()
		{
			ISmtpAuthenticationModule[] array2;
			lock (SmtpAuthenticationManager.modules)
			{
				ISmtpAuthenticationModule[] array = new ISmtpAuthenticationModule[SmtpAuthenticationManager.modules.Count];
				SmtpAuthenticationManager.modules.CopyTo(0, array, 0, SmtpAuthenticationManager.modules.Count);
				array2 = array;
			}
			return array2;
		}

		// Token: 0x040030AD RID: 12461
		private static ArrayList modules = new ArrayList();
	}
}
