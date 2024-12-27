using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Security
{
	// Token: 0x0200067B RID: 1659
	[ComVisible(true)]
	[Serializable]
	public class HostProtectionException : SystemException
	{
		// Token: 0x06003C88 RID: 15496 RVA: 0x000CFF29 File Offset: 0x000CEF29
		public HostProtectionException()
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x000CFF3F File Offset: 0x000CEF3F
		public HostProtectionException(string message)
			: base(message)
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x000CFF56 File Offset: 0x000CEF56
		public HostProtectionException(string message, Exception e)
			: base(message, e)
		{
			this.m_protected = HostProtectionResource.None;
			this.m_demanded = HostProtectionResource.None;
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x000CFF70 File Offset: 0x000CEF70
		protected HostProtectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_protected = (HostProtectionResource)info.GetValue("ProtectedResources", typeof(HostProtectionResource));
			this.m_demanded = (HostProtectionResource)info.GetValue("DemandedResources", typeof(HostProtectionResource));
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000CFFD3 File Offset: 0x000CEFD3
		public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources)
			: base(message)
		{
			base.SetErrorCode(-2146232768);
			this.m_protected = protectedResources;
			this.m_demanded = demandedResources;
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x000CFFF5 File Offset: 0x000CEFF5
		private HostProtectionException(HostProtectionResource protectedResources, HostProtectionResource demandedResources)
			: base(SecurityException.GetResString("HostProtection_HostProtection"))
		{
			base.SetErrorCode(-2146232768);
			this.m_protected = protectedResources;
			this.m_demanded = demandedResources;
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003C8E RID: 15502 RVA: 0x000D0020 File Offset: 0x000CF020
		public HostProtectionResource ProtectedResources
		{
			get
			{
				return this.m_protected;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003C8F RID: 15503 RVA: 0x000D0028 File Offset: 0x000CF028
		public HostProtectionResource DemandedResources
		{
			get
			{
				return this.m_demanded;
			}
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x000D0030 File Offset: 0x000CF030
		private string ToStringHelper(string resourceString, object attr)
		{
			if (attr == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Environment.GetResourceString(resourceString));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(attr);
			return stringBuilder.ToString();
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x000D008C File Offset: 0x000CF08C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(this.ToStringHelper("HostProtection_ProtectedResources", this.ProtectedResources));
			stringBuilder.Append(this.ToStringHelper("HostProtection_DemandedResources", this.DemandedResources));
			return stringBuilder.ToString();
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000D00EC File Offset: 0x000CF0EC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ProtectedResources", this.ProtectedResources, typeof(HostProtectionResource));
			info.AddValue("DemandedResources", this.DemandedResources, typeof(HostProtectionResource));
		}

		// Token: 0x04001EF3 RID: 7923
		private const string ProtectedResourcesName = "ProtectedResources";

		// Token: 0x04001EF4 RID: 7924
		private const string DemandedResourcesName = "DemandedResources";

		// Token: 0x04001EF5 RID: 7925
		private HostProtectionResource m_protected;

		// Token: 0x04001EF6 RID: 7926
		private HostProtectionResource m_demanded;
	}
}
