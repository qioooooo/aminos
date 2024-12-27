using System;
using System.IO;
using System.Resources;
using System.Web.Hosting;

namespace System.Web.Compilation
{
	// Token: 0x02000190 RID: 400
	internal sealed class ResXBuildProvider : BaseResourcesBuildProvider
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x0004C9AC File Offset: 0x0004B9AC
		protected override IResourceReader GetResourceReader(Stream inputStream)
		{
			ResXResourceReader resXResourceReader = new ResXResourceReader(inputStream);
			string text = HostingEnvironment.MapPath(base.VirtualPath);
			resXResourceReader.BasePath = Path.GetDirectoryName(text);
			return resXResourceReader;
		}
	}
}
