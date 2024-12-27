using System;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000347 RID: 839
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ValidatePasswordEventArgs : EventArgs
	{
		// Token: 0x060028BB RID: 10427 RVA: 0x000B2B66 File Offset: 0x000B1B66
		public ValidatePasswordEventArgs(string userName, string password, bool isNewUser)
		{
			this._userName = userName;
			this._password = password;
			this._isNewUser = isNewUser;
			this._cancel = false;
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x000B2B8A File Offset: 0x000B1B8A
		public string UserName
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x000B2B92 File Offset: 0x000B1B92
		public string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x000B2B9A File Offset: 0x000B1B9A
		public bool IsNewUser
		{
			get
			{
				return this._isNewUser;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000B2BA2 File Offset: 0x000B1BA2
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000B2BAA File Offset: 0x000B1BAA
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000B2BB3 File Offset: 0x000B1BB3
		// (set) Token: 0x060028C2 RID: 10434 RVA: 0x000B2BBB File Offset: 0x000B1BBB
		public Exception FailureInformation
		{
			get
			{
				return this._failureInformation;
			}
			set
			{
				this._failureInformation = value;
			}
		}

		// Token: 0x04001ED4 RID: 7892
		private string _userName;

		// Token: 0x04001ED5 RID: 7893
		private string _password;

		// Token: 0x04001ED6 RID: 7894
		private bool _isNewUser;

		// Token: 0x04001ED7 RID: 7895
		private bool _cancel;

		// Token: 0x04001ED8 RID: 7896
		private Exception _failureInformation;
	}
}
