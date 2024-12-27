using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200016C RID: 364
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DesignTimeResourceProviderFactoryAttribute : Attribute
	{
		// Token: 0x06001054 RID: 4180 RVA: 0x00048E22 File Offset: 0x00047E22
		public DesignTimeResourceProviderFactoryAttribute(Type factoryType)
		{
			this._factoryTypeName = factoryType.AssemblyQualifiedName;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00048E36 File Offset: 0x00047E36
		public DesignTimeResourceProviderFactoryAttribute(string factoryTypeName)
		{
			this._factoryTypeName = factoryTypeName;
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x00048E45 File Offset: 0x00047E45
		public string FactoryTypeName
		{
			get
			{
				return this._factoryTypeName;
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00048E4D File Offset: 0x00047E4D
		public override bool IsDefaultAttribute()
		{
			return this._factoryTypeName == null;
		}

		// Token: 0x0400164D RID: 5709
		private string _factoryTypeName;
	}
}
