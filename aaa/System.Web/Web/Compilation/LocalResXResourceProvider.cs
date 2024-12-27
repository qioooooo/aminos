using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000179 RID: 377
	internal class LocalResXResourceProvider : BaseResXResourceProvider
	{
		// Token: 0x06001082 RID: 4226 RVA: 0x00049292 File Offset: 0x00048292
		internal LocalResXResourceProvider(VirtualPath virtualPath)
		{
			this._virtualPath = virtualPath;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000492A4 File Offset: 0x000482A4
		protected override ResourceManager CreateResourceManager()
		{
			Assembly localResourceAssembly = this.GetLocalResourceAssembly();
			if (localResourceAssembly != null)
			{
				string fileName = this._virtualPath.FileName;
				return new ResourceManager(fileName, localResourceAssembly)
				{
					IgnoreCase = true
				};
			}
			throw new InvalidOperationException(SR.GetString("ResourceExpresionBuilder_PageResourceNotFound"));
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x000492EC File Offset: 0x000482EC
		public override IResourceReader ResourceReader
		{
			get
			{
				Assembly localResourceAssembly = this.GetLocalResourceAssembly();
				if (localResourceAssembly == null)
				{
					return null;
				}
				string text = this._virtualPath.FileName + ".resources";
				text = text.ToLower(CultureInfo.InvariantCulture);
				Stream manifestResourceStream = localResourceAssembly.GetManifestResourceStream(text);
				if (manifestResourceStream == null)
				{
					return null;
				}
				return new ResourceReader(manifestResourceStream);
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0004933C File Offset: 0x0004833C
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private Assembly GetLocalResourceAssembly()
		{
			VirtualPath parent = this._virtualPath.Parent;
			string localResourcesAssemblyName = BuildManager.GetLocalResourcesAssemblyName(parent);
			BuildResult buildResultFromCache = BuildManager.GetBuildResultFromCache(localResourcesAssemblyName);
			if (buildResultFromCache != null)
			{
				return ((BuildResultCompiledAssembly)buildResultFromCache).ResultAssembly;
			}
			return null;
		}

		// Token: 0x0400165A RID: 5722
		private VirtualPath _virtualPath;
	}
}
