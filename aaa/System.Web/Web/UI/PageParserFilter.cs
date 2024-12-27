using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Configuration;

namespace System.Web.UI
{
	// Token: 0x02000444 RID: 1092
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Medium)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Medium)]
	public abstract class PageParserFilter
	{
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000E2CC2 File Offset: 0x000E1CC2
		protected string VirtualPath
		{
			get
			{
				return this._virtualPath.VirtualPathString;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x000E2CCF File Offset: 0x000E1CCF
		protected int Line
		{
			get
			{
				return this._parser._lineNumber;
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000E2CDC File Offset: 0x000E1CDC
		internal static PageParserFilter Create(PagesSection pagesConfig, VirtualPath virtualPath, TemplateParser parser)
		{
			PageParserFilter pageParserFilter = pagesConfig.CreateControlTypeFilter();
			if (pageParserFilter != null)
			{
				pageParserFilter.InitializeInternal(virtualPath, parser);
			}
			return pageParserFilter;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000E2CFC File Offset: 0x000E1CFC
		internal void InitializeInternal(VirtualPath virtualPath, TemplateParser parser)
		{
			this._parser = parser;
			this._virtualPath = virtualPath;
			this.Initialize();
			this._numberOfControlsAllowed = this.NumberOfControlsAllowed;
			this._dependenciesAllowed = this.TotalNumberOfDependenciesAllowed + 1;
			this._directDependenciesAllowed = this.NumberOfDirectDependenciesAllowed + 1;
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000E2D3A File Offset: 0x000E1D3A
		protected virtual void Initialize()
		{
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000E2D3C File Offset: 0x000E1D3C
		public virtual void ParseComplete(ControlBuilder rootBuilder)
		{
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000E2D3E File Offset: 0x000E1D3E
		public virtual CompilationMode GetCompilationMode(CompilationMode current)
		{
			return current;
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000E2D41 File Offset: 0x000E1D41
		public virtual bool AllowCode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000E2D44 File Offset: 0x000E1D44
		internal bool AllowControlInternal(Type controlType, ControlBuilder builder)
		{
			this.OnControlAdded();
			return this.AllowControl(controlType, builder);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000E2D54 File Offset: 0x000E1D54
		public virtual bool AllowControl(Type controlType, ControlBuilder builder)
		{
			return false;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000E2D57 File Offset: 0x000E1D57
		public virtual bool AllowBaseType(Type baseType)
		{
			return false;
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000E2D5C File Offset: 0x000E1D5C
		internal bool AllowVirtualReference(CompilationSection compConfig, VirtualPath referenceVirtualPath)
		{
			string extension = referenceVirtualPath.Extension;
			Type buildProviderTypeFromExtension = CompilationUtil.GetBuildProviderTypeFromExtension(compConfig, extension, BuildProviderAppliesTo.Web, false);
			if (buildProviderTypeFromExtension == null)
			{
				return false;
			}
			VirtualReferenceType virtualReferenceType;
			if (buildProviderTypeFromExtension == typeof(PageBuildProvider))
			{
				virtualReferenceType = VirtualReferenceType.Page;
			}
			else if (buildProviderTypeFromExtension == typeof(UserControlBuildProvider))
			{
				virtualReferenceType = VirtualReferenceType.UserControl;
			}
			else if (buildProviderTypeFromExtension == typeof(MasterPageBuildProvider))
			{
				virtualReferenceType = VirtualReferenceType.Master;
			}
			else if (buildProviderTypeFromExtension == typeof(SourceFileBuildProvider))
			{
				virtualReferenceType = VirtualReferenceType.SourceFile;
			}
			else
			{
				virtualReferenceType = VirtualReferenceType.Other;
			}
			return this.AllowVirtualReference(referenceVirtualPath.VirtualPathString, virtualReferenceType);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000E2DD2 File Offset: 0x000E1DD2
		public virtual bool AllowVirtualReference(string referenceVirtualPath, VirtualReferenceType referenceType)
		{
			return false;
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000E2DD5 File Offset: 0x000E1DD5
		public virtual bool AllowServerSideInclude(string includeVirtualPath)
		{
			return false;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000E2DD8 File Offset: 0x000E1DD8
		public virtual void PreprocessDirective(string directiveName, IDictionary attributes)
		{
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600342F RID: 13359 RVA: 0x000E2DDA File Offset: 0x000E1DDA
		public virtual int NumberOfControlsAllowed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x000E2DDD File Offset: 0x000E1DDD
		public virtual int TotalNumberOfDependenciesAllowed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06003431 RID: 13361 RVA: 0x000E2DE0 File Offset: 0x000E1DE0
		public virtual int NumberOfDirectDependenciesAllowed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000E2DE4 File Offset: 0x000E1DE4
		private void OnControlAdded()
		{
			if (this._numberOfControlsAllowed < 0)
			{
				return;
			}
			this._currentControlCount++;
			if (this._currentControlCount > this._numberOfControlsAllowed)
			{
				throw new HttpException(SR.GetString("Too_many_controls", new object[] { this._numberOfControlsAllowed.ToString(CultureInfo.CurrentCulture) }));
			}
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000E2E44 File Offset: 0x000E1E44
		internal void OnDependencyAdded()
		{
			if (this._dependenciesAllowed <= 0)
			{
				return;
			}
			this._currentDependenciesCount++;
			if (this._currentDependenciesCount > this._dependenciesAllowed)
			{
				throw new HttpException(SR.GetString("Too_many_dependencies", new object[]
				{
					this.VirtualPath,
					this._dependenciesAllowed.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000E2EAC File Offset: 0x000E1EAC
		internal void OnDirectDependencyAdded()
		{
			if (this._directDependenciesAllowed <= 0)
			{
				return;
			}
			this._currentDirectDependenciesCount++;
			if (this._currentDirectDependenciesCount > this._directDependenciesAllowed)
			{
				throw new HttpException(SR.GetString("Too_many_direct_dependencies", new object[]
				{
					this.VirtualPath,
					this._directDependenciesAllowed.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000E2F13 File Offset: 0x000E1F13
		public virtual bool ProcessCodeConstruct(CodeConstructType codeType, string code)
		{
			return false;
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000E2F16 File Offset: 0x000E1F16
		public virtual bool ProcessDataBindingAttribute(string controlId, string name, string value)
		{
			return false;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000E2F19 File Offset: 0x000E1F19
		public virtual bool ProcessEventHookup(string controlId, string eventName, string handlerName)
		{
			return false;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000E2F1C File Offset: 0x000E1F1C
		public virtual Type GetNoCompileUserControlType()
		{
			return null;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000E2F1F File Offset: 0x000E1F1F
		protected void AddControl(Type type, IDictionary attributes)
		{
			this._parser.AddControl(type, attributes);
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000E2F2E File Offset: 0x000E1F2E
		protected void SetPageProperty(string filter, string name, string value)
		{
			if (filter == null)
			{
				filter = string.Empty;
			}
			this._parser.RootBuilder.PreprocessAttribute(filter, name, value, true);
		}

		// Token: 0x04002495 RID: 9365
		private VirtualPath _virtualPath;

		// Token: 0x04002496 RID: 9366
		private TemplateParser _parser;

		// Token: 0x04002497 RID: 9367
		private int _numberOfControlsAllowed;

		// Token: 0x04002498 RID: 9368
		private int _currentControlCount;

		// Token: 0x04002499 RID: 9369
		private int _dependenciesAllowed;

		// Token: 0x0400249A RID: 9370
		private int _currentDependenciesCount;

		// Token: 0x0400249B RID: 9371
		private int _directDependenciesAllowed;

		// Token: 0x0400249C RID: 9372
		private int _currentDirectDependenciesCount;
	}
}
