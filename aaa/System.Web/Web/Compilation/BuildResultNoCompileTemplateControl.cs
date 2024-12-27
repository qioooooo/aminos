using System;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200014E RID: 334
	internal abstract class BuildResultNoCompileTemplateControl : BuildResult, ITypedWebObjectFactory, IWebObjectFactory
	{
		// Token: 0x06000F6E RID: 3950 RVA: 0x00045036 File Offset: 0x00044036
		internal BuildResultNoCompileTemplateControl(Type baseType, TemplateParser parser)
		{
			this._baseType = baseType;
			this._rootBuilder = parser.RootBuilder;
			this._rootBuilder.PrepareNoCompilePageSupport();
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0004505C File Offset: 0x0004405C
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.Invalid;
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0004505F File Offset: 0x0004405F
		internal override bool CacheToDisk
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x00045062 File Offset: 0x00044062
		internal override TimeSpan MemoryCacheSlidingExpiration
		{
			get
			{
				return TimeSpan.FromMinutes(5.0);
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00045072 File Offset: 0x00044072
		internal Type BaseType
		{
			get
			{
				return this._baseType;
			}
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0004507C File Offset: 0x0004407C
		public virtual object CreateInstance()
		{
			TemplateControl templateControl = (TemplateControl)HttpRuntime.FastCreatePublicInstance(this._baseType);
			templateControl.TemplateControlVirtualPath = base.VirtualPath;
			templateControl.TemplateControlVirtualDirectory = base.VirtualPath.Parent;
			templateControl.SetNoCompileBuildResult(this);
			return templateControl;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x000450BF File Offset: 0x000440BF
		public virtual Type InstantiatedType
		{
			get
			{
				return this._baseType;
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x000450C8 File Offset: 0x000440C8
		internal virtual void FrameworkInitialize(TemplateControl templateControl)
		{
			HttpContext httpContext = HttpContext.Current;
			TemplateControl templateControl2 = httpContext.TemplateControl;
			httpContext.TemplateControl = templateControl;
			try
			{
				if (!this._initialized)
				{
					lock (this)
					{
						this._rootBuilder.InitObject(templateControl);
					}
					this._initialized = true;
				}
				else
				{
					this._rootBuilder.InitObject(templateControl);
				}
			}
			finally
			{
				if (templateControl2 != null)
				{
					httpContext.TemplateControl = templateControl2;
				}
			}
		}

		// Token: 0x040015E7 RID: 5607
		protected Type _baseType;

		// Token: 0x040015E8 RID: 5608
		protected RootBuilder _rootBuilder;

		// Token: 0x040015E9 RID: 5609
		protected bool _initialized;
	}
}
