using System;
using System.Net;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x0200003D RID: 61
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class SearchResult
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x000072A3 File Offset: 0x000062A3
		internal SearchResult(NetworkCredential parentCredentials, AuthenticationTypes parentAuthenticationType)
		{
			this.parentCredentials = parentCredentials;
			this.parentAuthenticationType = parentAuthenticationType;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000072C4 File Offset: 0x000062C4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public DirectoryEntry GetDirectoryEntry()
		{
			if (this.parentCredentials != null)
			{
				return new DirectoryEntry(this.Path, true, this.parentCredentials.UserName, this.parentCredentials.Password, this.parentAuthenticationType);
			}
			return new DirectoryEntry(this.Path, true, null, null, this.parentAuthenticationType);
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007318 File Offset: 0x00006318
		public string Path
		{
			get
			{
				return (string)this.Properties["ADsPath"][0];
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007335 File Offset: 0x00006335
		public ResultPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x040001DD RID: 477
		private NetworkCredential parentCredentials;

		// Token: 0x040001DE RID: 478
		private AuthenticationTypes parentAuthenticationType;

		// Token: 0x040001DF RID: 479
		private ResultPropertyCollection properties = new ResultPropertyCollection();
	}
}
