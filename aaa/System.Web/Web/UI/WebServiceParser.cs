using System;
using System.Security.Permissions;
using System.Web.Compilation;

namespace System.Web.UI
{
	// Token: 0x02000462 RID: 1122
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebServiceParser : SimpleWebHandlerParser
	{
		// Token: 0x0600351F RID: 13599 RVA: 0x000E5E5C File Offset: 0x000E4E5C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static Type GetCompiledType(string inputFile, HttpContext context)
		{
			BuildResultCompiledType buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetVPathBuildResult(context, VirtualPath.Create(inputFile));
			return buildResultCompiledType.ResultType;
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x000E5E81 File Offset: 0x000E4E81
		internal WebServiceParser(string virtualPath)
			: base(null, virtualPath, null)
		{
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x000E5E8C File Offset: 0x000E4E8C
		protected override string DefaultDirectiveName
		{
			get
			{
				return "webservice";
			}
		}
	}
}
