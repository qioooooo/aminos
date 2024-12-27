using System;
using System.Security.Permissions;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000067 RID: 103
	public abstract class DsmlSoapConnection : DirectoryConnection
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000206 RID: 518
		public abstract string SessionId { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008CC0 File Offset: 0x00007CC0
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00008CC8 File Offset: 0x00007CC8
		public XmlNode SoapRequestHeader
		{
			get
			{
				return this.soapHeaders;
			}
			set
			{
				this.soapHeaders = value;
			}
		}

		// Token: 0x06000209 RID: 521
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract void BeginSession();

		// Token: 0x0600020A RID: 522
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract void EndSession();

		// Token: 0x040001FB RID: 507
		internal XmlNode soapHeaders;
	}
}
