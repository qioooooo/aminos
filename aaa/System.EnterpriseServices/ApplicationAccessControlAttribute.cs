using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000092 RID: 146
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	public sealed class ApplicationAccessControlAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600036A RID: 874 RVA: 0x0000B544 File Offset: 0x0000A544
		public ApplicationAccessControlAttribute()
			: this(true)
		{
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000B54D File Offset: 0x0000A54D
		public ApplicationAccessControlAttribute(bool val)
		{
			this._val = val;
			this._authLevel = (AuthenticationOption)(-1);
			this._impLevel = (ImpersonationLevelOption)(-1);
			if (this._val)
			{
				this._checkLevel = AccessChecksLevelOption.ApplicationComponent;
				return;
			}
			this._checkLevel = AccessChecksLevelOption.Application;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000B581 File Offset: 0x0000A581
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000B589 File Offset: 0x0000A589
		public bool Value
		{
			get
			{
				return this._val;
			}
			set
			{
				this._val = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000B592 File Offset: 0x0000A592
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000B59A File Offset: 0x0000A59A
		public AccessChecksLevelOption AccessChecksLevel
		{
			get
			{
				return this._checkLevel;
			}
			set
			{
				Platform.Assert(Platform.W2K, "ApplicationAccessControlAttribute.AccessChecksLevel");
				this._checkLevel = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000B5B2 File Offset: 0x0000A5B2
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000B5BA File Offset: 0x0000A5BA
		public AuthenticationOption Authentication
		{
			get
			{
				return this._authLevel;
			}
			set
			{
				this._authLevel = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000B5C3 File Offset: 0x0000A5C3
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000B5CB File Offset: 0x0000A5CB
		public ImpersonationLevelOption ImpersonationLevel
		{
			get
			{
				return this._impLevel;
			}
			set
			{
				Platform.Assert(Platform.W2K, "ApplicationAccessControlAttribute.ImpersonationLevel");
				this._impLevel = value;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000B5E3 File Offset: 0x0000A5E3
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000B5F0 File Offset: 0x0000A5F0
		bool IConfigurationAttribute.Apply(Hashtable cache)
		{
			Platform.Assert(Platform.MTS, "ApplicationAccessControlAttribute");
			ICatalogObject catalogObject = (ICatalogObject)cache["Application"];
			if (Platform.IsLessThan(Platform.W2K))
			{
				bool val = this._val;
				catalogObject.SetValue("SecurityEnabled", val ? "Y" : "N");
			}
			else
			{
				catalogObject.SetValue("ApplicationAccessChecksEnabled", this._val);
				catalogObject.SetValue("AccessChecksLevel", this._checkLevel);
			}
			if (this._authLevel != (AuthenticationOption)(-1))
			{
				catalogObject.SetValue("Authentication", this._authLevel);
			}
			if (this._impLevel != (ImpersonationLevelOption)(-1))
			{
				catalogObject.SetValue("ImpersonationLevel", this._impLevel);
			}
			return true;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000B6B7 File Offset: 0x0000A6B7
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000164 RID: 356
		private bool _val;

		// Token: 0x04000165 RID: 357
		private AccessChecksLevelOption _checkLevel;

		// Token: 0x04000166 RID: 358
		private AuthenticationOption _authLevel;

		// Token: 0x04000167 RID: 359
		private ImpersonationLevelOption _impLevel;
	}
}
