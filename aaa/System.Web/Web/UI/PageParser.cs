using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000442 RID: 1090
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PageParser : TemplateControlParser
	{
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x000E1DAF File Offset: 0x000E0DAF
		internal int TransactionMode
		{
			get
			{
				return this._transactionMode;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x000E1DB7 File Offset: 0x000E0DB7
		internal TraceMode TraceMode
		{
			get
			{
				return this._traceMode;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x000E1DBF File Offset: 0x000E0DBF
		internal TraceEnable TraceEnabled
		{
			get
			{
				return this._traceEnabled;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x000E1DC7 File Offset: 0x000E0DC7
		internal bool FRequiresSessionState
		{
			get
			{
				return this.flags[1048576];
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x000E1DD9 File Offset: 0x000E0DD9
		internal bool FReadOnlySessionState
		{
			get
			{
				return this.flags[2097152];
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x000E1DEB File Offset: 0x000E0DEB
		internal string StyleSheetTheme
		{
			get
			{
				return this._styleSheetTheme;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x000E1DF3 File Offset: 0x000E0DF3
		internal bool AspCompatMode
		{
			get
			{
				return this.flags[64];
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06003407 RID: 13319 RVA: 0x000E1E02 File Offset: 0x000E0E02
		internal bool AsyncMode
		{
			get
			{
				return this.flags[8388608];
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003408 RID: 13320 RVA: 0x000E1E14 File Offset: 0x000E0E14
		internal bool ValidateRequest
		{
			get
			{
				return this.flags[4194304];
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003409 RID: 13321 RVA: 0x000E1E26 File Offset: 0x000E0E26
		internal Type PreviousPageType
		{
			get
			{
				return this._previousPageType;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x000E1E2E File Offset: 0x000E0E2E
		internal Type MasterPageType
		{
			get
			{
				return this._masterPageType;
			}
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000E1E38 File Offset: 0x000E0E38
		public PageParser()
		{
			this.flags[524288] = true;
			this.flags[1048576] = true;
			this.flags[4194304] = true;
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000E1E8C File Offset: 0x000E0E8C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static IHttpHandler GetCompiledPageInstance(string virtualPath, string inputFile, HttpContext context)
		{
			if (!string.IsNullOrEmpty(inputFile))
			{
				inputFile = Path.GetFullPath(inputFile);
			}
			if (inputFile != null)
			{
				lock (PageParser.s_lock)
				{
					return PageParser.GetCompiledPageInstance(VirtualPath.Create(virtualPath), inputFile, context);
				}
			}
			return PageParser.GetCompiledPageInstance(VirtualPath.Create(virtualPath), inputFile, context);
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000E1EF0 File Offset: 0x000E0EF0
		private static IHttpHandler GetCompiledPageInstance(VirtualPath virtualPath, string inputFile, HttpContext context)
		{
			if (context != null)
			{
				virtualPath = context.Request.FilePathObject.Combine(virtualPath);
			}
			object obj = null;
			IHttpHandler httpHandler;
			try
			{
				try
				{
					if (inputFile != null)
					{
						obj = HostingEnvironment.AddVirtualPathToFileMapping(virtualPath, inputFile);
					}
					BuildResultCompiledType buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetVPathBuildResult(context, virtualPath, false, true, true);
					httpHandler = (IHttpHandler)HttpRuntime.CreatePublicInstance(buildResultCompiledType.ResultType);
				}
				finally
				{
					if (obj != null)
					{
						HostingEnvironment.ClearVirtualPathToFileMapping(obj);
					}
				}
			}
			catch
			{
				throw;
			}
			return httpHandler;
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x000E1F70 File Offset: 0x000E0F70
		internal override Type DefaultBaseType
		{
			get
			{
				return typeof(Page);
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x000E1F7C File Offset: 0x000E0F7C
		internal override Type DefaultFileLevelBuilderType
		{
			get
			{
				return typeof(FileLevelPageControlBuilder);
			}
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000E1F88 File Offset: 0x000E0F88
		internal override RootBuilder CreateDefaultFileLevelBuilder()
		{
			return new FileLevelPageControlBuilder();
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000E1F90 File Offset: 0x000E0F90
		private void EnsureMasterPageFileFromConfigApplied()
		{
			if (this._mainDirectiveMasterPageSet)
			{
				return;
			}
			if (this._configMasterPageFile != null)
			{
				int lineNumber = this._lineNumber;
				this._lineNumber = this._mainDirectiveLineNumber;
				try
				{
					if (this._configMasterPageFile.Length > 0)
					{
						Type referencedType = base.GetReferencedType(this._configMasterPageFile);
						if (!typeof(MasterPage).IsAssignableFrom(referencedType))
						{
							base.ProcessError(SR.GetString("Invalid_master_base", new object[] { this._configMasterPageFile }));
						}
					}
					if (((FileLevelPageControlBuilder)base.RootBuilder).ContentBuilderEntries != null)
					{
						base.RootBuilder.SetControlType(base.BaseType);
						base.RootBuilder.PreprocessAttribute(string.Empty, "MasterPageFile", this._configMasterPageFile, true);
					}
				}
				finally
				{
					this._lineNumber = lineNumber;
				}
			}
			this._mainDirectiveMasterPageSet = true;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000E2074 File Offset: 0x000E1074
		internal override void HandlePostParse()
		{
			base.HandlePostParse();
			this.EnsureMasterPageFileFromConfigApplied();
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000E2084 File Offset: 0x000E1084
		internal override void ProcessConfigSettings()
		{
			base.ProcessConfigSettings();
			if (base.PagesConfig != null)
			{
				if (!base.PagesConfig.Buffer)
				{
					this._mainDirectiveConfigSettings["buffer"] = Util.GetStringFromBool(base.PagesConfig.Buffer);
				}
				if (!base.PagesConfig.EnableViewStateMac)
				{
					this._mainDirectiveConfigSettings["enableviewstatemac"] = Util.GetStringFromBool(base.PagesConfig.EnableViewStateMac);
				}
				if (!base.PagesConfig.EnableEventValidation)
				{
					this._mainDirectiveConfigSettings["enableEventValidation"] = Util.GetStringFromBool(base.PagesConfig.EnableEventValidation);
				}
				if (base.PagesConfig.SmartNavigation)
				{
					this._mainDirectiveConfigSettings["smartnavigation"] = Util.GetStringFromBool(base.PagesConfig.SmartNavigation);
				}
				if (base.PagesConfig.ThemeInternal != null && base.PagesConfig.Theme.Length != 0)
				{
					this._mainDirectiveConfigSettings["theme"] = base.PagesConfig.Theme;
				}
				if (base.PagesConfig.StyleSheetThemeInternal != null && base.PagesConfig.StyleSheetThemeInternal.Length != 0)
				{
					this._mainDirectiveConfigSettings["stylesheettheme"] = base.PagesConfig.StyleSheetThemeInternal;
				}
				if (base.PagesConfig.MasterPageFileInternal != null && base.PagesConfig.MasterPageFileInternal.Length != 0)
				{
					this._configMasterPageFile = base.PagesConfig.MasterPageFileInternal;
				}
				if (base.PagesConfig.ViewStateEncryptionMode != ViewStateEncryptionMode.Auto)
				{
					this._mainDirectiveConfigSettings["viewStateEncryptionMode"] = Enum.Format(typeof(ViewStateEncryptionMode), base.PagesConfig.ViewStateEncryptionMode, "G");
				}
				if (base.PagesConfig.MaintainScrollPositionOnPostBack)
				{
					this._mainDirectiveConfigSettings["maintainScrollPositionOnPostBack"] = Util.GetStringFromBool(base.PagesConfig.MaintainScrollPositionOnPostBack);
				}
				if (base.PagesConfig.MaxPageStateFieldLength != Page.DefaultMaxPageStateFieldLength)
				{
					this._mainDirectiveConfigSettings["maxPageStateFieldLength"] = base.PagesConfig.MaxPageStateFieldLength;
				}
				this.flags[1048576] = base.PagesConfig.EnableSessionState == PagesEnableSessionState.True || base.PagesConfig.EnableSessionState == PagesEnableSessionState.ReadOnly;
				this.flags[2097152] = base.PagesConfig.EnableSessionState == PagesEnableSessionState.ReadOnly;
				this.flags[4194304] = base.PagesConfig.ValidateRequest;
				this.flags[64] = HttpRuntime.ApartmentThreading;
				if (base.PagesConfig.PageBaseTypeInternal != null)
				{
					base.BaseType = base.PagesConfig.PageBaseTypeInternal;
				}
			}
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000E2334 File Offset: 0x000E1334
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (StringUtil.EqualsIgnoreCase(directiveName, "previousPageType"))
			{
				if (this._previousPageType != null)
				{
					base.ProcessError(SR.GetString("Only_one_directive_allowed", new object[] { directiveName }));
					return;
				}
				this._previousPageType = base.GetDirectiveType(directive, directiveName);
				Util.CheckAssignableType(typeof(Page), this._previousPageType);
				return;
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(directiveName, "masterType"))
				{
					base.ProcessDirective(directiveName, directive);
					return;
				}
				if (this._masterPageType != null)
				{
					base.ProcessError(SR.GetString("Only_one_directive_allowed", new object[] { directiveName }));
					return;
				}
				this._masterPageType = base.GetDirectiveType(directive, directiveName);
				Util.CheckAssignableType(typeof(MasterPage), this._masterPageType);
				return;
			}
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000E23F5 File Offset: 0x000E13F5
		internal override void ProcessMainDirective(IDictionary mainDirective)
		{
			this._mainDirectiveLineNumber = this._lineNumber;
			base.ProcessMainDirective(mainDirective);
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000E240C File Offset: 0x000E140C
		internal override bool ProcessMainDirectiveAttribute(string deviceName, string name, string value, IDictionary parseData)
		{
			if (name != null)
			{
				if (<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x60033af-1 == null)
				{
					<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x60033af-1 = new Dictionary<string, int>(20)
					{
						{ "errorpage", 0 },
						{ "contenttype", 1 },
						{ "theme", 2 },
						{ "stylesheettheme", 3 },
						{ "enablesessionstate", 4 },
						{ "culture", 5 },
						{ "lcid", 6 },
						{ "uiculture", 7 },
						{ "responseencoding", 8 },
						{ "codepage", 9 },
						{ "transaction", 10 },
						{ "aspcompat", 11 },
						{ "async", 12 },
						{ "tracemode", 13 },
						{ "trace", 14 },
						{ "smartnavigation", 15 },
						{ "maintainscrollpositiononpostback", 16 },
						{ "validaterequest", 17 },
						{ "clienttarget", 18 },
						{ "masterpagefile", 19 }
					};
				}
				int num;
				if (<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x60033af-1.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
						this._errorPage = Util.GetNonEmptyAttribute(name, value);
						return false;
					case 1:
						Util.GetNonEmptyAttribute(name, value);
						return false;
					case 2:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						Util.CheckThemeAttribute(value);
						return false;
					case 3:
						base.ValidateBuiltInAttribute(deviceName, name, value);
						Util.CheckThemeAttribute(value);
						this._styleSheetTheme = value;
						return true;
					case 4:
						this.flags[1048576] = true;
						this.flags[2097152] = false;
						if (Util.IsFalseString(value))
						{
							this.flags[1048576] = false;
						}
						else if (StringUtil.EqualsIgnoreCase(value, "readonly"))
						{
							this.flags[2097152] = true;
						}
						else if (!Util.IsTrueString(value))
						{
							base.ProcessError(SR.GetString("Enablesessionstate_must_be_true_false_or_readonly"));
						}
						if (this.flags[1048576])
						{
							base.OnFoundAttributeRequiringCompilation(name);
						}
						break;
					case 5:
					{
						this._culture = Util.GetNonEmptyAttribute(name, value);
						if (!HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
						{
							throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "culture" }));
						}
						if (StringUtil.EqualsIgnoreCase(value, HttpApplication.AutoCulture))
						{
							return false;
						}
						CultureInfo cultureInfo;
						try
						{
							if (StringUtil.StringStartsWithIgnoreCase(value, HttpApplication.AutoCulture))
							{
								this._culture = this._culture.Substring(5);
							}
							cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(this._culture);
						}
						catch
						{
							base.ProcessError(SR.GetString("Invalid_attribute_value", new object[] { this._culture, "culture" }));
							return false;
						}
						if (cultureInfo.IsNeutralCulture)
						{
							base.ProcessError(SR.GetString("Invalid_culture_attribute", new object[] { Util.GetSpecificCulturesFormattedList(cultureInfo) }));
						}
						return false;
					}
					case 6:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						this._lcid = Util.GetNonNegativeIntegerAttribute(name, value);
						try
						{
							HttpServerUtility.CreateReadOnlyCultureInfo(this._lcid);
						}
						catch
						{
							base.ProcessError(SR.GetString("Invalid_attribute_value", new object[]
							{
								this._lcid.ToString(CultureInfo.InvariantCulture),
								"lcid"
							}));
						}
						return false;
					case 7:
						Util.GetNonEmptyAttribute(name, value);
						return false;
					case 8:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						this._responseEncoding = Util.GetNonEmptyAttribute(name, value);
						Encoding.GetEncoding(this._responseEncoding);
						return false;
					case 9:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						this._codePage = Util.GetNonNegativeIntegerAttribute(name, value);
						Encoding.GetEncoding(this._codePage);
						return false;
					case 10:
						base.OnFoundAttributeRequiringCompilation(name);
						this.ParseTransactionAttribute(name, value);
						break;
					case 11:
						base.OnFoundAttributeRequiringCompilation(name);
						this.flags[64] = Util.GetBooleanAttribute(name, value);
						if (this.flags[64] && !HttpRuntime.HasUnmanagedPermission())
						{
							throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "AspCompat" }));
						}
						break;
					case 12:
						base.OnFoundAttributeRequiringCompilation(name);
						this.flags[8388608] = Util.GetBooleanAttribute(name, value);
						if (!HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
						{
							throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "async" }));
						}
						break;
					case 13:
					{
						object enumAttribute = Util.GetEnumAttribute(name, value, typeof(PageParser.TraceModeInternal));
						this._traceMode = (TraceMode)enumAttribute;
						break;
					}
					case 14:
					{
						bool booleanAttribute = Util.GetBooleanAttribute(name, value);
						if (booleanAttribute)
						{
							this._traceEnabled = TraceEnable.Enable;
						}
						else
						{
							this._traceEnabled = TraceEnable.Disable;
						}
						break;
					}
					case 15:
					{
						base.ValidateBuiltInAttribute(deviceName, name, value);
						bool booleanAttribute2 = Util.GetBooleanAttribute(name, value);
						return !booleanAttribute2;
					}
					case 16:
					{
						bool booleanAttribute3 = Util.GetBooleanAttribute(name, value);
						return !booleanAttribute3;
					}
					case 17:
						this.flags[4194304] = Util.GetBooleanAttribute(name, value);
						break;
					case 18:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						HttpCapabilitiesEvaluator.GetUserAgentFromClientTarget(base.CurrentVirtualPath, value);
						return false;
					case 19:
						if (base.IsExpressionBuilderValue(value))
						{
							return false;
						}
						if (value.Length > 0)
						{
							Type referencedType = base.GetReferencedType(value);
							if (!typeof(MasterPage).IsAssignableFrom(referencedType))
							{
								base.ProcessError(SR.GetString("Invalid_master_base", new object[] { value }));
							}
							if (deviceName.Length > 0)
							{
								this.EnsureMasterPageFileFromConfigApplied();
							}
						}
						this._mainDirectiveMasterPageSet = true;
						return false;
					default:
						goto IL_05C3;
					}
					base.ValidateBuiltInAttribute(deviceName, name, value);
					return true;
				}
			}
			IL_05C3:
			return base.ProcessMainDirectiveAttribute(deviceName, name, value, parseData);
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000E2A14 File Offset: 0x000E1A14
		internal override void ProcessUnknownMainDirectiveAttribute(string filter, string attribName, string value)
		{
			if (attribName == "asynctimeout")
			{
				int nonNegativeIntegerAttribute = Util.GetNonNegativeIntegerAttribute(attribName, value);
				value = new TimeSpan(0, 0, nonNegativeIntegerAttribute).ToString();
			}
			base.ProcessUnknownMainDirectiveAttribute(filter, attribName, value);
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000E2A58 File Offset: 0x000E1A58
		internal override void PostProcessMainDirectiveAttributes(IDictionary parseData)
		{
			if (!this.flags[524288] && this._errorPage != null)
			{
				base.ProcessError(SR.GetString("Error_page_not_supported_when_buffering_off"));
				return;
			}
			if (this._culture != null && this._lcid > 0)
			{
				base.ProcessError(SR.GetString("Attributes_mutually_exclusive", new object[] { "Culture", "LCID" }));
				return;
			}
			if (this._responseEncoding != null && this._codePage > 0)
			{
				base.ProcessError(SR.GetString("Attributes_mutually_exclusive", new object[] { "ResponseEncoding", "CodePage" }));
				return;
			}
			if (this.AsyncMode && this.AspCompatMode)
			{
				base.ProcessError(SR.GetString("Async_and_aspcompat"));
				return;
			}
			if (this.AsyncMode && this._transactionMode != 0)
			{
				base.ProcessError(SR.GetString("Async_and_transaction"));
				return;
			}
			base.PostProcessMainDirectiveAttributes(parseData);
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000E2B50 File Offset: 0x000E1B50
		private void ParseTransactionAttribute(string name, string value)
		{
			object enumAttribute = Util.GetEnumAttribute(name, value, typeof(TransactionOption));
			if (enumAttribute != null)
			{
				this._transactionMode = (int)enumAttribute;
				if (this._transactionMode != 0)
				{
					if (!HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
					{
						throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "transaction" }));
					}
					base.AddAssemblyDependency(typeof(TransactionOption).Assembly);
				}
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x000E2BC7 File Offset: 0x000E1BC7
		internal override string DefaultDirectiveName
		{
			get
			{
				return "page";
			}
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000E2BD0 File Offset: 0x000E1BD0
		internal override void ProcessOutputCacheDirective(string directiveName, IDictionary directive)
		{
			bool flag = false;
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "varybycontentencoding");
			if (andRemoveNonEmptyAttribute != null)
			{
				base.OutputCacheParameters.VaryByContentEncoding = andRemoveNonEmptyAttribute;
			}
			string andRemoveNonEmptyAttribute2 = Util.GetAndRemoveNonEmptyAttribute(directive, "varybyheader");
			if (andRemoveNonEmptyAttribute2 != null)
			{
				base.OutputCacheParameters.VaryByHeader = andRemoveNonEmptyAttribute2;
			}
			object andRemoveEnumAttribute = Util.GetAndRemoveEnumAttribute(directive, typeof(OutputCacheLocation), "location");
			if (andRemoveEnumAttribute != null)
			{
				this._outputCacheLocation = (OutputCacheLocation)andRemoveEnumAttribute;
				base.OutputCacheParameters.Location = this._outputCacheLocation;
			}
			string andRemoveNonEmptyAttribute3 = Util.GetAndRemoveNonEmptyAttribute(directive, "sqldependency");
			if (andRemoveNonEmptyAttribute3 != null)
			{
				base.OutputCacheParameters.SqlDependency = andRemoveNonEmptyAttribute3;
				SqlCacheDependency.ValidateOutputCacheDependencyString(andRemoveNonEmptyAttribute3, true);
			}
			if (Util.GetAndRemoveBooleanAttribute(directive, "nostore", ref flag))
			{
				base.OutputCacheParameters.NoStore = flag;
			}
			base.ProcessOutputCacheDirective(directiveName, directive);
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000E2C93 File Offset: 0x000E1C93
		internal override bool FDurationRequiredOnOutputCache
		{
			get
			{
				return this._outputCacheLocation != OutputCacheLocation.None;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x000E2CA1 File Offset: 0x000E1CA1
		internal override bool FVaryByParamsRequiredOnOutputCache
		{
			get
			{
				return this._outputCacheLocation != OutputCacheLocation.None;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000E2CAF File Offset: 0x000E1CAF
		internal override string UnknownOutputCacheAttributeError
		{
			get
			{
				return "Attr_not_supported_in_pagedirective";
			}
		}

		// Token: 0x04002481 RID: 9345
		internal const string defaultDirectiveName = "page";

		// Token: 0x04002482 RID: 9346
		private int _transactionMode;

		// Token: 0x04002483 RID: 9347
		private TraceMode _traceMode = TraceMode.Default;

		// Token: 0x04002484 RID: 9348
		private TraceEnable _traceEnabled;

		// Token: 0x04002485 RID: 9349
		private int _codePage;

		// Token: 0x04002486 RID: 9350
		private string _responseEncoding;

		// Token: 0x04002487 RID: 9351
		private int _lcid;

		// Token: 0x04002488 RID: 9352
		private string _culture;

		// Token: 0x04002489 RID: 9353
		private int _mainDirectiveLineNumber = 1;

		// Token: 0x0400248A RID: 9354
		private bool _mainDirectiveMasterPageSet;

		// Token: 0x0400248B RID: 9355
		private OutputCacheLocation _outputCacheLocation;

		// Token: 0x0400248C RID: 9356
		private string _errorPage;

		// Token: 0x0400248D RID: 9357
		private string _styleSheetTheme;

		// Token: 0x0400248E RID: 9358
		private Type _previousPageType;

		// Token: 0x0400248F RID: 9359
		private Type _masterPageType;

		// Token: 0x04002490 RID: 9360
		private string _configMasterPageFile;

		// Token: 0x04002491 RID: 9361
		private static object s_lock = new object();

		// Token: 0x02000443 RID: 1091
		private enum TraceModeInternal
		{
			// Token: 0x04002493 RID: 9363
			SortByTime,
			// Token: 0x04002494 RID: 9364
			SortByCategory
		}
	}
}
