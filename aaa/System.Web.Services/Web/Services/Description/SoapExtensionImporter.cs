using System;
using System.CodeDom;
using System.Security.Permissions;

namespace System.Web.Services.Description
{
	// Token: 0x0200011A RID: 282
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public abstract class SoapExtensionImporter
	{
		// Token: 0x060008AF RID: 2223
		public abstract void ImportMethod(CodeAttributeDeclarationCollection metadata);

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00040F12 File Offset: 0x0003FF12
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00040F1A File Offset: 0x0003FF1A
		public SoapProtocolImporter ImportContext
		{
			get
			{
				return this.protocolImporter;
			}
			set
			{
				this.protocolImporter = value;
			}
		}

		// Token: 0x040005B7 RID: 1463
		private SoapProtocolImporter protocolImporter;
	}
}
