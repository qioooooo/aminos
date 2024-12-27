using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000010 RID: 16
	public abstract class DirectoryConnection
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003A85 File Offset: 0x00002A85
		protected DirectoryConnection()
		{
			Utility.CheckOSVersion();
			this.certificatesCollection = new X509CertificateCollection();
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003AAC File Offset: 0x00002AAC
		public virtual DirectoryIdentifier Directory
		{
			get
			{
				return this.directoryIdentifier;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003AB4 File Offset: 0x00002AB4
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.certificatesCollection;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003ABC File Offset: 0x00002ABC
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003AC4 File Offset: 0x00002AC4
		public virtual TimeSpan Timeout
		{
			get
			{
				return this.connectionTimeOut;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				this.connectionTimeOut = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003AEF File Offset: 0x00002AEF
		public virtual NetworkCredential Credential
		{
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			set
			{
				this.directoryCredential = ((value != null) ? new NetworkCredential(value.UserName, value.Password, value.Domain) : null);
			}
		}

		// Token: 0x0600005B RID: 91
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract DirectoryResponse SendRequest(DirectoryRequest request);

		// Token: 0x0600005C RID: 92 RVA: 0x00003B14 File Offset: 0x00002B14
		internal NetworkCredential GetCredential()
		{
			return this.directoryCredential;
		}

		// Token: 0x040000C3 RID: 195
		internal NetworkCredential directoryCredential;

		// Token: 0x040000C4 RID: 196
		internal X509CertificateCollection certificatesCollection;

		// Token: 0x040000C5 RID: 197
		internal TimeSpan connectionTimeOut = new TimeSpan(0, 0, 30);

		// Token: 0x040000C6 RID: 198
		internal DirectoryIdentifier directoryIdentifier;
	}
}
