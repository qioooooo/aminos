using System;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000322 RID: 802
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class MembershipUser
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000AC7EB File Offset: 0x000AB7EB
		public virtual string UserName
		{
			get
			{
				return this._UserName;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x000AC7F3 File Offset: 0x000AB7F3
		public virtual object ProviderUserKey
		{
			get
			{
				return this._ProviderUserKey;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x000AC7FB File Offset: 0x000AB7FB
		// (set) Token: 0x06002764 RID: 10084 RVA: 0x000AC803 File Offset: 0x000AB803
		public virtual string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				this._Email = value;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x000AC80C File Offset: 0x000AB80C
		public virtual string PasswordQuestion
		{
			get
			{
				return this._PasswordQuestion;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x000AC814 File Offset: 0x000AB814
		// (set) Token: 0x06002767 RID: 10087 RVA: 0x000AC81C File Offset: 0x000AB81C
		public virtual string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x000AC825 File Offset: 0x000AB825
		// (set) Token: 0x06002769 RID: 10089 RVA: 0x000AC82D File Offset: 0x000AB82D
		public virtual bool IsApproved
		{
			get
			{
				return this._IsApproved;
			}
			set
			{
				this._IsApproved = value;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x000AC836 File Offset: 0x000AB836
		public virtual bool IsLockedOut
		{
			get
			{
				return this._IsLockedOut;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x000AC83E File Offset: 0x000AB83E
		public virtual DateTime LastLockoutDate
		{
			get
			{
				return this._LastLockoutDate.ToLocalTime();
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x000AC84B File Offset: 0x000AB84B
		public virtual DateTime CreationDate
		{
			get
			{
				return this._CreationDate.ToLocalTime();
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000AC858 File Offset: 0x000AB858
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x000AC865 File Offset: 0x000AB865
		public virtual DateTime LastLoginDate
		{
			get
			{
				return this._LastLoginDate.ToLocalTime();
			}
			set
			{
				this._LastLoginDate = value.ToUniversalTime();
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x000AC874 File Offset: 0x000AB874
		// (set) Token: 0x06002770 RID: 10096 RVA: 0x000AC881 File Offset: 0x000AB881
		public virtual DateTime LastActivityDate
		{
			get
			{
				return this._LastActivityDate.ToLocalTime();
			}
			set
			{
				this._LastActivityDate = value.ToUniversalTime();
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x000AC890 File Offset: 0x000AB890
		public virtual DateTime LastPasswordChangedDate
		{
			get
			{
				return this._LastPasswordChangedDate.ToLocalTime();
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x000AC8A0 File Offset: 0x000AB8A0
		public bool IsOnline
		{
			get
			{
				TimeSpan timeSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
				DateTime dateTime = DateTime.UtcNow.Subtract(timeSpan);
				return this.LastActivityDate.ToUniversalTime() > dateTime;
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000AC8DE File Offset: 0x000AB8DE
		public override string ToString()
		{
			return this.UserName;
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x000AC8E6 File Offset: 0x000AB8E6
		public virtual string ProviderName
		{
			get
			{
				return this._ProviderName;
			}
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000AC8F0 File Offset: 0x000AB8F0
		public MembershipUser(string providerName, string name, object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
		{
			if (providerName == null || Membership.Providers[providerName] == null)
			{
				throw new ArgumentException(SR.GetString("Membership_provider_name_invalid"), "providerName");
			}
			if (name != null)
			{
				name = name.Trim();
			}
			if (email != null)
			{
				email = email.Trim();
			}
			if (passwordQuestion != null)
			{
				passwordQuestion = passwordQuestion.Trim();
			}
			this._ProviderName = providerName;
			this._UserName = name;
			this._ProviderUserKey = providerUserKey;
			this._Email = email;
			this._PasswordQuestion = passwordQuestion;
			this._Comment = comment;
			this._IsApproved = isApproved;
			this._IsLockedOut = isLockedOut;
			this._CreationDate = creationDate.ToUniversalTime();
			this._LastLoginDate = lastLoginDate.ToUniversalTime();
			this._LastActivityDate = lastActivityDate.ToUniversalTime();
			this._LastPasswordChangedDate = lastPasswordChangedDate.ToUniversalTime();
			this._LastLockoutDate = lastLockoutDate.ToUniversalTime();
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000AC9CB File Offset: 0x000AB9CB
		protected MembershipUser()
		{
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000AC9D3 File Offset: 0x000AB9D3
		internal virtual void Update()
		{
			Membership.Providers[this.ProviderName].UpdateUser(this);
			this.UpdateSelf();
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000AC9F1 File Offset: 0x000AB9F1
		public virtual string GetPassword()
		{
			return Membership.Providers[this.ProviderName].GetPassword(this.UserName, null);
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000ACA0F File Offset: 0x000ABA0F
		public virtual string GetPassword(string passwordAnswer)
		{
			return Membership.Providers[this.ProviderName].GetPassword(this.UserName, passwordAnswer);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000ACA2D File Offset: 0x000ABA2D
		internal string GetPassword(bool throwOnError)
		{
			return this.GetPassword(null, false, throwOnError);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000ACA38 File Offset: 0x000ABA38
		internal string GetPassword(string answer, bool throwOnError)
		{
			return this.GetPassword(answer, true, throwOnError);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000ACA44 File Offset: 0x000ABA44
		private string GetPassword(string answer, bool useAnswer, bool throwOnError)
		{
			string text = null;
			try
			{
				if (useAnswer)
				{
					text = this.GetPassword(answer);
				}
				else
				{
					text = this.GetPassword();
				}
			}
			catch (ArgumentException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (MembershipPasswordException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (ProviderException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			return text;
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000ACAAC File Offset: 0x000ABAAC
		public virtual bool ChangePassword(string oldPassword, string newPassword)
		{
			SecUtility.CheckPasswordParameter(ref oldPassword, 0, "oldPassword");
			SecUtility.CheckPasswordParameter(ref newPassword, 0, "newPassword");
			if (!Membership.Providers[this.ProviderName].ChangePassword(this.UserName, oldPassword, newPassword))
			{
				return false;
			}
			this.UpdateSelf();
			return true;
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000ACAFC File Offset: 0x000ABAFC
		internal bool ChangePassword(string oldPassword, string newPassword, bool throwOnError)
		{
			bool flag = false;
			try
			{
				flag = this.ChangePassword(oldPassword, newPassword);
			}
			catch (ArgumentException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (MembershipPasswordException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (ProviderException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			return flag;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000ACB58 File Offset: 0x000ABB58
		public virtual bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			SecUtility.CheckPasswordParameter(ref password, 0, "password");
			SecUtility.CheckParameter(ref newPasswordQuestion, false, true, false, 0, "newPasswordQuestion");
			SecUtility.CheckParameter(ref newPasswordAnswer, false, true, false, 0, "newPasswordAnswer");
			if (!Membership.Providers[this.ProviderName].ChangePasswordQuestionAndAnswer(this.UserName, password, newPasswordQuestion, newPasswordAnswer))
			{
				return false;
			}
			this.UpdateSelf();
			return true;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000ACBBC File Offset: 0x000ABBBC
		public virtual string ResetPassword(string passwordAnswer)
		{
			string text = Membership.Providers[this.ProviderName].ResetPassword(this.UserName, passwordAnswer);
			if (!string.IsNullOrEmpty(text))
			{
				this.UpdateSelf();
			}
			return text;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000ACBF5 File Offset: 0x000ABBF5
		public virtual string ResetPassword()
		{
			return this.ResetPassword(null);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000ACBFE File Offset: 0x000ABBFE
		internal string ResetPassword(bool throwOnError)
		{
			return this.ResetPassword(null, false, throwOnError);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000ACC09 File Offset: 0x000ABC09
		internal string ResetPassword(string passwordAnswer, bool throwOnError)
		{
			return this.ResetPassword(passwordAnswer, true, throwOnError);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000ACC14 File Offset: 0x000ABC14
		private string ResetPassword(string passwordAnswer, bool useAnswer, bool throwOnError)
		{
			string text = null;
			try
			{
				if (useAnswer)
				{
					text = this.ResetPassword(passwordAnswer);
				}
				else
				{
					text = this.ResetPassword();
				}
			}
			catch (ArgumentException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (MembershipPasswordException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			catch (ProviderException)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			return text;
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000ACC7C File Offset: 0x000ABC7C
		public virtual bool UnlockUser()
		{
			if (Membership.Providers[this.ProviderName].UnlockUser(this.UserName))
			{
				this.UpdateSelf();
				return !this.IsLockedOut;
			}
			return false;
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000ACCAC File Offset: 0x000ABCAC
		private void UpdateSelf()
		{
			MembershipUser user = Membership.Providers[this.ProviderName].GetUser(this.UserName, false);
			if (user != null)
			{
				try
				{
					this._LastPasswordChangedDate = user.LastPasswordChangedDate.ToUniversalTime();
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this.LastActivityDate = user.LastActivityDate;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this.LastLoginDate = user.LastLoginDate;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this._CreationDate = user.CreationDate.ToUniversalTime();
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this._LastLockoutDate = user.LastLockoutDate.ToUniversalTime();
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this._IsLockedOut = user.IsLockedOut;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this.IsApproved = user.IsApproved;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this.Comment = user.Comment;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this._PasswordQuestion = user.PasswordQuestion;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this.Email = user.Email;
				}
				catch (NotSupportedException)
				{
				}
				try
				{
					this._ProviderUserKey = user.ProviderUserKey;
				}
				catch (NotSupportedException)
				{
				}
			}
		}

		// Token: 0x04001E3E RID: 7742
		private string _UserName;

		// Token: 0x04001E3F RID: 7743
		private object _ProviderUserKey;

		// Token: 0x04001E40 RID: 7744
		private string _Email;

		// Token: 0x04001E41 RID: 7745
		private string _PasswordQuestion;

		// Token: 0x04001E42 RID: 7746
		private string _Comment;

		// Token: 0x04001E43 RID: 7747
		private bool _IsApproved;

		// Token: 0x04001E44 RID: 7748
		private bool _IsLockedOut;

		// Token: 0x04001E45 RID: 7749
		private DateTime _LastLockoutDate;

		// Token: 0x04001E46 RID: 7750
		private DateTime _CreationDate;

		// Token: 0x04001E47 RID: 7751
		private DateTime _LastLoginDate;

		// Token: 0x04001E48 RID: 7752
		private DateTime _LastActivityDate;

		// Token: 0x04001E49 RID: 7753
		private DateTime _LastPasswordChangedDate;

		// Token: 0x04001E4A RID: 7754
		private string _ProviderName;
	}
}
