using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000344 RID: 836
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class MembershipCreateUserException : Exception
	{
		// Token: 0x060028AF RID: 10415 RVA: 0x000B2A00 File Offset: 0x000B1A00
		public MembershipCreateUserException(MembershipCreateStatus statusCode)
			: base(MembershipCreateUserException.GetMessageFromStatusCode(statusCode))
		{
			this._StatusCode = statusCode;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000B2A1D File Offset: 0x000B1A1D
		public MembershipCreateUserException(string message)
			: base(message)
		{
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000B2A2E File Offset: 0x000B1A2E
		protected MembershipCreateUserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._StatusCode = (MembershipCreateStatus)info.GetInt32("_StatusCode");
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000B2A51 File Offset: 0x000B1A51
		public MembershipCreateUserException()
		{
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000B2A61 File Offset: 0x000B1A61
		public MembershipCreateUserException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x000B2A73 File Offset: 0x000B1A73
		public MembershipCreateStatus StatusCode
		{
			get
			{
				return this._StatusCode;
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000B2A7B File Offset: 0x000B1A7B
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_StatusCode", this._StatusCode);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000B2A9C File Offset: 0x000B1A9C
		internal static string GetMessageFromStatusCode(MembershipCreateStatus statusCode)
		{
			string text = "Provider_Error";
			switch (statusCode)
			{
			case MembershipCreateStatus.Success:
				text = "Membership_No_error";
				break;
			case MembershipCreateStatus.InvalidUserName:
				text = "Membership_InvalidUserName";
				break;
			case MembershipCreateStatus.InvalidPassword:
				text = "Membership_InvalidPassword";
				break;
			case MembershipCreateStatus.InvalidQuestion:
				text = "Membership_InvalidQuestion";
				break;
			case MembershipCreateStatus.InvalidAnswer:
				text = "Membership_InvalidAnswer";
				break;
			case MembershipCreateStatus.InvalidEmail:
				text = "Membership_InvalidEmail";
				break;
			case MembershipCreateStatus.DuplicateUserName:
				text = "Membership_DuplicateUserName";
				break;
			case MembershipCreateStatus.DuplicateEmail:
				text = "Membership_DuplicateEmail";
				break;
			case MembershipCreateStatus.UserRejected:
				text = "Membership_UserRejected";
				break;
			case MembershipCreateStatus.InvalidProviderUserKey:
				text = "Membership_InvalidProviderUserKey";
				break;
			case MembershipCreateStatus.DuplicateProviderUserKey:
				text = "Membership_DuplicateProviderUserKey";
				break;
			}
			return SR.GetString(text);
		}

		// Token: 0x04001ECF RID: 7887
		private MembershipCreateStatus _StatusCode = MembershipCreateStatus.ProviderError;
	}
}
