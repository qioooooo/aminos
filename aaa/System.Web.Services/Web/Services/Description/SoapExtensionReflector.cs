using System;
using System.Security.Permissions;

namespace System.Web.Services.Description
{
	// Token: 0x0200011B RID: 283
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class SoapExtensionReflector
	{
		// Token: 0x060008B3 RID: 2227
		public abstract void ReflectMethod();

		// Token: 0x060008B4 RID: 2228 RVA: 0x00040F2B File Offset: 0x0003FF2B
		public virtual void ReflectDescription()
		{
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00040F2D File Offset: 0x0003FF2D
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x00040F35 File Offset: 0x0003FF35
		public ProtocolReflector ReflectionContext
		{
			get
			{
				return this.protocolReflector;
			}
			set
			{
				this.protocolReflector = value;
			}
		}

		// Token: 0x040005B8 RID: 1464
		private ProtocolReflector protocolReflector;
	}
}
