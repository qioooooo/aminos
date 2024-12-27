using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200042B RID: 1067
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class TemplateControlParser : BaseTemplateParser
	{
		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x000DE5B8 File Offset: 0x000DD5B8
		internal OutputCacheParameters OutputCacheParameters
		{
			get
			{
				return this._outputCacheSettings;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x000DE5C0 File Offset: 0x000DD5C0
		internal bool FAutoEventWireup
		{
			get
			{
				return !this.flags[131072];
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x000DE5D5 File Offset: 0x000DD5D5
		internal override bool RequiresCompilation
		{
			get
			{
				return this.flags[16] || base.CompilationMode == CompilationMode.Always;
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000DE5F4 File Offset: 0x000DD5F4
		internal override void ProcessConfigSettings()
		{
			base.ProcessConfigSettings();
			if (base.PagesConfig != null)
			{
				this.flags[131072] = !base.PagesConfig.AutoEventWireup;
				if (!base.PagesConfig.EnableViewState)
				{
					this._mainDirectiveConfigSettings["enableviewstate"] = Util.GetStringFromBool(base.PagesConfig.EnableViewState);
				}
				base.CompilationMode = base.PagesConfig.CompilationMode;
			}
			if (this._pageParserFilter != null)
			{
				base.CompilationMode = this._pageParserFilter.GetCompilationMode(base.CompilationMode);
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000DE68C File Offset: 0x000DD68C
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (StringUtil.EqualsIgnoreCase(directiveName, "outputcache"))
			{
				if (this.FInDesigner)
				{
					return;
				}
				if (this._outputCacheSettings == null)
				{
					this._outputCacheSettings = new OutputCacheParameters();
				}
				if (this._outputCacheDirective != null)
				{
					throw new HttpException(SR.GetString("Only_one_directive_allowed", new object[] { directiveName }));
				}
				this.ProcessOutputCacheDirective(directiveName, directive);
				this._outputCacheDirective = directive;
				return;
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(directiveName, "reference"))
				{
					base.ProcessDirective(directiveName, directive);
					return;
				}
				if (this.FInDesigner)
				{
					return;
				}
				VirtualPath virtualPath = Util.GetAndRemoveVirtualPathAttribute(directive, "virtualpath");
				bool flag = false;
				bool flag2 = false;
				VirtualPath virtualPath2 = Util.GetAndRemoveVirtualPathAttribute(directive, "page");
				if (virtualPath2 != null)
				{
					if (virtualPath != null)
					{
						base.ProcessError(SR.GetString("Invalid_reference_directive"));
						return;
					}
					virtualPath = virtualPath2;
					flag = true;
				}
				virtualPath2 = Util.GetAndRemoveVirtualPathAttribute(directive, "control");
				if (virtualPath2 != null)
				{
					if (virtualPath != null)
					{
						base.ProcessError(SR.GetString("Invalid_reference_directive"));
						return;
					}
					virtualPath = virtualPath2;
					flag2 = true;
				}
				if (virtualPath == null)
				{
					base.ProcessError(SR.GetString("Invalid_reference_directive"));
					return;
				}
				Type referencedType = base.GetReferencedType(virtualPath);
				if (referencedType == null)
				{
					base.ProcessError(SR.GetString("Invalid_reference_directive_attrib", new object[] { virtualPath }));
				}
				if (flag && !typeof(Page).IsAssignableFrom(referencedType))
				{
					base.ProcessError(SR.GetString("Invalid_reference_directive_attrib", new object[] { virtualPath }));
				}
				if (flag2 && !typeof(UserControl).IsAssignableFrom(referencedType))
				{
					base.ProcessError(SR.GetString("Invalid_reference_directive_attrib", new object[] { virtualPath }));
				}
				Util.CheckUnknownDirectiveAttributes(directiveName, directive);
				return;
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000DE848 File Offset: 0x000DD848
		internal override void ProcessMainDirective(IDictionary mainDirective)
		{
			object obj = null;
			try
			{
				obj = Util.GetAndRemoveEnumAttribute(mainDirective, typeof(CompilationMode), "compilationmode");
			}
			catch (Exception ex)
			{
				base.ProcessError(ex.Message);
			}
			if (obj != null)
			{
				base.CompilationMode = (CompilationMode)obj;
				if (this._pageParserFilter != null)
				{
					base.CompilationMode = this._pageParserFilter.GetCompilationMode(base.CompilationMode);
				}
			}
			base.ProcessMainDirective(mainDirective);
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000DE8C4 File Offset: 0x000DD8C4
		internal override bool ProcessMainDirectiveAttribute(string deviceName, string name, string value, IDictionary parseData)
		{
			if (name != null)
			{
				if (!(name == "targetschema"))
				{
					if (!(name == "autoeventwireup"))
					{
						if (name == "enabletheming")
						{
							return false;
						}
						if (!(name == "codefilebaseclass"))
						{
							goto IL_0071;
						}
						parseData[name] = Util.GetNonEmptyAttribute(name, value);
					}
					else
					{
						base.OnFoundAttributeRequiringCompilation(name);
						this.flags[131072] = !Util.GetBooleanAttribute(name, value);
					}
				}
				base.ValidateBuiltInAttribute(deviceName, name, value);
				return true;
			}
			IL_0071:
			return base.ProcessMainDirectiveAttribute(deviceName, name, value, parseData);
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000DE958 File Offset: 0x000DD958
		internal override void ProcessUnknownMainDirectiveAttribute(string filter, string attribName, string value)
		{
			if (attribName == "id")
			{
				base.ProcessUnknownMainDirectiveAttribute(filter, attribName, value);
				return;
			}
			try
			{
				base.RootBuilder.PreprocessAttribute(filter, attribName, value, true);
			}
			catch (Exception ex)
			{
				base.ProcessError(SR.GetString("Attrib_parse_error", new object[] { attribName, ex.Message }));
			}
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000DE9C8 File Offset: 0x000DD9C8
		private void AddStaticObjectAssemblyDependencies(HttpStaticObjectsCollection staticObjects)
		{
			if (staticObjects == null || staticObjects.Objects == null)
			{
				return;
			}
			IDictionaryEnumerator enumerator = staticObjects.Objects.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)enumerator.Value;
				base.AddTypeDependency(httpStaticObjectsEntry.ObjectType);
			}
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000DEA10 File Offset: 0x000DDA10
		internal Type GetDirectiveType(IDictionary directive, string directiveName)
		{
			string andRemoveNonEmptyNoSpaceAttribute = Util.GetAndRemoveNonEmptyNoSpaceAttribute(directive, "typeName");
			VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "virtualPath");
			if (andRemoveNonEmptyNoSpaceAttribute == null == (andRemoveVirtualPathAttribute == null))
			{
				throw new HttpException(SR.GetString("Invalid_typeNameOrVirtualPath_directive", new object[] { directiveName }));
			}
			Type type;
			if (andRemoveNonEmptyNoSpaceAttribute != null)
			{
				type = base.GetType(andRemoveNonEmptyNoSpaceAttribute);
				base.AddTypeDependency(type);
			}
			else
			{
				type = base.GetReferencedType(andRemoveVirtualPathAttribute);
			}
			Util.CheckUnknownDirectiveAttributes(directiveName, directive);
			return type;
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000DEA84 File Offset: 0x000DDA84
		internal override void HandlePostParse()
		{
			base.HandlePostParse();
			if (!this.FInDesigner)
			{
				if (base.ScriptList.Count == 0 && base.BaseType == this.DefaultBaseType && base.CodeFileVirtualPath == null)
				{
					this.flags[131072] = true;
				}
				this._applicationObjects = HttpApplicationFactory.ApplicationState.StaticObjects;
				this.AddStaticObjectAssemblyDependencies(this._applicationObjects);
				this._sessionObjects = HttpApplicationFactory.ApplicationState.SessionStaticObjects;
				this.AddStaticObjectAssemblyDependencies(this._sessionObjects);
			}
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000DEB14 File Offset: 0x000DDB14
		internal virtual void ProcessOutputCacheDirective(string directiveName, IDictionary directive)
		{
			int num = 0;
			string text = null;
			bool andRemovePositiveIntegerAttribute = Util.GetAndRemovePositiveIntegerAttribute(directive, "duration", ref num);
			if (andRemovePositiveIntegerAttribute)
			{
				this.OutputCacheParameters.Duration = num;
			}
			if (this is PageParser)
			{
				text = Util.GetAndRemoveNonEmptyAttribute(directive, "cacheProfile");
				if (text != null)
				{
					this.OutputCacheParameters.CacheProfile = text;
				}
			}
			if (!andRemovePositiveIntegerAttribute && (text == null || text.Length == 0) && this.FDurationRequiredOnOutputCache)
			{
				throw new HttpException(SR.GetString("Missing_attr", new object[] { "duration" }));
			}
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "varybycustom");
			if (andRemoveNonEmptyAttribute != null)
			{
				this.OutputCacheParameters.VaryByCustom = andRemoveNonEmptyAttribute;
			}
			string andRemoveNonEmptyAttribute2 = Util.GetAndRemoveNonEmptyAttribute(directive, "varybycontrol");
			if (andRemoveNonEmptyAttribute2 != null)
			{
				this.OutputCacheParameters.VaryByControl = andRemoveNonEmptyAttribute2;
			}
			string andRemoveNonEmptyAttribute3 = Util.GetAndRemoveNonEmptyAttribute(directive, "varybyparam");
			if (andRemoveNonEmptyAttribute3 != null)
			{
				this.OutputCacheParameters.VaryByParam = andRemoveNonEmptyAttribute3;
			}
			if (andRemoveNonEmptyAttribute3 == null && andRemoveNonEmptyAttribute2 == null && (text == null || text.Length == 0) && this.FVaryByParamsRequiredOnOutputCache)
			{
				throw new HttpException(SR.GetString("Missing_varybyparam_attr"));
			}
			if (StringUtil.EqualsIgnoreCase(andRemoveNonEmptyAttribute3, "none"))
			{
				this.OutputCacheParameters.VaryByParam = null;
			}
			if (StringUtil.EqualsIgnoreCase(andRemoveNonEmptyAttribute2, "none"))
			{
				this.OutputCacheParameters.VaryByControl = null;
			}
			Util.CheckUnknownDirectiveAttributes(directiveName, directive, this.UnknownOutputCacheAttributeError);
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000DEC5F File Offset: 0x000DDC5F
		internal virtual bool FDurationRequiredOnOutputCache
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x000DEC62 File Offset: 0x000DDC62
		internal virtual bool FVaryByParamsRequiredOnOutputCache
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003343 RID: 13123
		internal abstract string UnknownOutputCacheAttributeError { get; }

		// Token: 0x040023F1 RID: 9201
		private IDictionary _outputCacheDirective;

		// Token: 0x040023F2 RID: 9202
		private OutputCacheParameters _outputCacheSettings;
	}
}
