using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000170 RID: 368
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DesigntimeLicenseContext : LicenseContext
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00028E0A File Offset: 0x00027E0A
		public override LicenseUsageMode UsageMode
		{
			get
			{
				return LicenseUsageMode.Designtime;
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00028E0D File Offset: 0x00027E0D
		public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
		{
			return null;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00028E10 File Offset: 0x00027E10
		public override void SetSavedLicenseKey(Type type, string key)
		{
			this.savedLicenseKeys[type.AssemblyQualifiedName] = key;
		}

		// Token: 0x04000AC7 RID: 2759
		internal Hashtable savedLicenseKeys = new Hashtable();
	}
}
