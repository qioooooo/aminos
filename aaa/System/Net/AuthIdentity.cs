using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004F9 RID: 1273
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct AuthIdentity
	{
		// Token: 0x060027DB RID: 10203 RVA: 0x000A488C File Offset: 0x000A388C
		internal AuthIdentity(string userName, string password, string domain)
		{
			this.UserName = userName;
			this.UserNameLength = ((userName == null) ? 0 : userName.Length);
			this.Password = password;
			this.PasswordLength = ((password == null) ? 0 : password.Length);
			this.Domain = domain;
			this.DomainLength = ((domain == null) ? 0 : domain.Length);
			this.Flags = (ComNetOS.IsWin9x ? 1 : 2);
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000A48F5 File Offset: 0x000A38F5
		public override string ToString()
		{
			return ValidationHelper.ToString(this.Domain) + "\\" + ValidationHelper.ToString(this.UserName);
		}

		// Token: 0x04002704 RID: 9988
		internal string UserName;

		// Token: 0x04002705 RID: 9989
		internal int UserNameLength;

		// Token: 0x04002706 RID: 9990
		internal string Domain;

		// Token: 0x04002707 RID: 9991
		internal int DomainLength;

		// Token: 0x04002708 RID: 9992
		internal string Password;

		// Token: 0x04002709 RID: 9993
		internal int PasswordLength;

		// Token: 0x0400270A RID: 9994
		internal int Flags;
	}
}
