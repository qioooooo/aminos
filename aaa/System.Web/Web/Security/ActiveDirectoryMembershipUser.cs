using System;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Security
{
	// Token: 0x02000323 RID: 803
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class ActiveDirectoryMembershipUser : MembershipUser
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000ACE38 File Offset: 0x000ABE38
		// (set) Token: 0x06002788 RID: 10120 RVA: 0x000ACE64 File Offset: 0x000ABE64
		public override DateTime LastLoginDate
		{
			get
			{
				throw new NotSupportedException(SR.GetString("ADMembership_UserProperty_not_supported", new object[] { "LastLoginDate" }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ADMembership_UserProperty_not_supported", new object[] { "LastLoginDate" }));
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x000ACE90 File Offset: 0x000ABE90
		// (set) Token: 0x0600278A RID: 10122 RVA: 0x000ACEBC File Offset: 0x000ABEBC
		public override DateTime LastActivityDate
		{
			get
			{
				throw new NotSupportedException(SR.GetString("ADMembership_UserProperty_not_supported", new object[] { "LastActivityDate" }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ADMembership_UserProperty_not_supported", new object[] { "LastActivityDate" }));
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000ACEE8 File Offset: 0x000ABEE8
		// (set) Token: 0x0600278C RID: 10124 RVA: 0x000ACEF0 File Offset: 0x000ABEF0
		public override string Email
		{
			get
			{
				return base.Email;
			}
			set
			{
				base.Email = value;
				this.emailModified = true;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000ACF00 File Offset: 0x000ABF00
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x000ACF08 File Offset: 0x000ABF08
		public override string Comment
		{
			get
			{
				return base.Comment;
			}
			set
			{
				base.Comment = value;
				this.commentModified = true;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000ACF18 File Offset: 0x000ABF18
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000ACF20 File Offset: 0x000ABF20
		public override bool IsApproved
		{
			get
			{
				return base.IsApproved;
			}
			set
			{
				base.IsApproved = value;
				this.isApprovedModified = true;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x000ACF30 File Offset: 0x000ABF30
		public override object ProviderUserKey
		{
			get
			{
				if (this.sid == null && this.sidBinaryForm != null)
				{
					this.sid = new SecurityIdentifier(this.sidBinaryForm, 0);
				}
				return this.sid;
			}
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000ACF60 File Offset: 0x000ABF60
		public ActiveDirectoryMembershipUser(string providerName, string name, object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
			: base(providerName, name, null, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
		{
			if (providerUserKey != null && !(providerUserKey is SecurityIdentifier))
			{
				throw new ArgumentException(SR.GetString("ADMembership_InvalidProviderUserKey"), "providerUserKey");
			}
			this.sid = (SecurityIdentifier)providerUserKey;
			if (this.sid != null)
			{
				this.sidBinaryForm = new byte[this.sid.BinaryLength];
				this.sid.GetBinaryForm(this.sidBinaryForm, 0);
			}
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000AD004 File Offset: 0x000AC004
		internal ActiveDirectoryMembershipUser(string providerName, string name, byte[] sidBinaryForm, object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate, bool valuesAreUpdated)
			: base(providerName, name, null, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
		{
			if (valuesAreUpdated)
			{
				this.emailModified = false;
				this.commentModified = false;
				this.isApprovedModified = false;
			}
			this.sidBinaryForm = sidBinaryForm;
			this.sid = (SecurityIdentifier)providerUserKey;
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000AD070 File Offset: 0x000AC070
		protected ActiveDirectoryMembershipUser()
		{
		}

		// Token: 0x04001E4B RID: 7755
		internal bool emailModified = true;

		// Token: 0x04001E4C RID: 7756
		internal bool commentModified = true;

		// Token: 0x04001E4D RID: 7757
		internal bool isApprovedModified = true;

		// Token: 0x04001E4E RID: 7758
		private byte[] sidBinaryForm;

		// Token: 0x04001E4F RID: 7759
		[NonSerialized]
		private SecurityIdentifier sid;
	}
}
