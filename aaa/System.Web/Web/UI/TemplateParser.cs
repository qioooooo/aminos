using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000384 RID: 900
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class TemplateParser : BaseParser, IAssemblyDependencyParser
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06002B9B RID: 11163 RVA: 0x000C2193 File Offset: 0x000C1193
		internal CompilationSection CompConfig
		{
			get
			{
				return this._compConfig;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000C219B File Offset: 0x000C119B
		internal PagesSection PagesConfig
		{
			get
			{
				return this._pagesConfig;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06002B9D RID: 11165 RVA: 0x000C21A3 File Offset: 0x000C11A3
		internal MainTagNameToTypeMapper TypeMapper
		{
			get
			{
				return this._typeMapper;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000C21AB File Offset: 0x000C11AB
		internal ICollection UserControlRegisterEntries
		{
			get
			{
				return this.TypeMapper.UserControlRegisterEntries;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000C21B8 File Offset: 0x000C11B8
		internal List<TagNamespaceRegisterEntry> TagRegisterEntries
		{
			get
			{
				return this.TypeMapper.TagRegisterEntries;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000C21C5 File Offset: 0x000C11C5
		internal Stack BuilderStack
		{
			get
			{
				this.EnsureRootBuilderCreated();
				return this._builderStack;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000C21D3 File Offset: 0x000C11D3
		// (set) Token: 0x06002BA2 RID: 11170 RVA: 0x000C21DB File Offset: 0x000C11DB
		internal string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x000C21E4 File Offset: 0x000C11E4
		// (set) Token: 0x06002BA4 RID: 11172 RVA: 0x000C21EC File Offset: 0x000C11EC
		internal Type BaseType
		{
			get
			{
				return this._baseType;
			}
			set
			{
				this._baseType = value;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x000C21F5 File Offset: 0x000C11F5
		internal string BaseTypeNamespace
		{
			get
			{
				return this._baseTypeNamespace;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x000C21FD File Offset: 0x000C11FD
		internal string BaseTypeName
		{
			get
			{
				return this._baseTypeName;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000C2205 File Offset: 0x000C1205
		// (set) Token: 0x06002BA8 RID: 11176 RVA: 0x000C2214 File Offset: 0x000C1214
		internal bool IgnoreControlProperties
		{
			get
			{
				return this.flags[32];
			}
			set
			{
				this.flags[32] = value;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000C2224 File Offset: 0x000C1224
		// (set) Token: 0x06002BAA RID: 11178 RVA: 0x000C2236 File Offset: 0x000C1236
		internal bool ThrowOnFirstParseError
		{
			get
			{
				return this.flags[16777216];
			}
			set
			{
				this.flags[16777216] = value;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000C2249 File Offset: 0x000C1249
		internal ArrayList ImplementedInterfaces
		{
			get
			{
				return this._implementedInterfaces;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000C2251 File Offset: 0x000C1251
		internal bool HasCodeBehind
		{
			get
			{
				return this.flags[128];
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06002BAD RID: 11181
		internal abstract Type DefaultBaseType { get; }

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000C2263 File Offset: 0x000C1263
		// (set) Token: 0x06002BAF RID: 11183 RVA: 0x000C2275 File Offset: 0x000C1275
		internal virtual bool FInDesigner
		{
			get
			{
				return this.flags[256];
			}
			set
			{
				this.flags[256] = value;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000C2288 File Offset: 0x000C1288
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x000C229A File Offset: 0x000C129A
		internal virtual bool IgnoreParseErrors
		{
			get
			{
				return this.flags[512];
			}
			set
			{
				this.flags[512] = value;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000C22AD File Offset: 0x000C12AD
		// (set) Token: 0x06002BB3 RID: 11187 RVA: 0x000C22BE File Offset: 0x000C12BE
		internal CompilationMode CompilationMode
		{
			get
			{
				if (BuildManager.PrecompilingForDeployment)
				{
					return CompilationMode.Always;
				}
				return this._compilationMode;
			}
			set
			{
				if (value == CompilationMode.Never && this.flags[16])
				{
					this.ProcessError(SR.GetString("Compilmode_not_allowed"));
				}
				this._compilationMode = value;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x000C22EA File Offset: 0x000C12EA
		private ParserErrorCollection ParserErrors
		{
			get
			{
				if (this._parserErrors == null)
				{
					this._parserErrors = new ParserErrorCollection();
				}
				return this._parserErrors;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x000C2305 File Offset: 0x000C1305
		private bool HasParserErrors
		{
			get
			{
				return this._parserErrors != null && this._parserErrors.Count > 0;
			}
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000C2320 File Offset: 0x000C1320
		protected void ProcessError(string message)
		{
			if (this.IgnoreParseErrors)
			{
				return;
			}
			if (this.ThrowOnFirstParseError)
			{
				throw new HttpException(message);
			}
			ParserError parserError = new ParserError(message, base.CurrentVirtualPath, this._lineNumber);
			this.ParserErrors.Add(parserError);
			BuildManager.ReportParseError(parserError);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000C236C File Offset: 0x000C136C
		protected void ProcessException(Exception ex)
		{
			if (this.IgnoreParseErrors)
			{
				return;
			}
			if (!this.ThrowOnFirstParseError && !(ex is HttpCompileException))
			{
				HttpParseException ex2 = ex as HttpParseException;
				ParserError parserError;
				if (ex2 != null)
				{
					parserError = new ParserError(ex2.Message, ex2.VirtualPath, ex2.Line);
				}
				else
				{
					parserError = new ParserError(ex.Message, base.CurrentVirtualPath, this._lineNumber);
				}
				parserError.Exception = ex;
				this.ParserErrors.Add(parserError);
				if (ex2 == null || base.CurrentVirtualPath.Equals(ex2.VirtualPathObject))
				{
					BuildManager.ReportParseError(parserError);
				}
				return;
			}
			if (ex is HttpParseException)
			{
				throw ex;
			}
			throw new HttpParseException(ex.Message, ex);
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000C2415 File Offset: 0x000C1415
		internal virtual bool RequiresCompilation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06002BB9 RID: 11193 RVA: 0x000C2418 File Offset: 0x000C1418
		internal virtual bool IsCodeAllowed
		{
			get
			{
				return this.CompilationMode != CompilationMode.Never && (this._pageParserFilter == null || this._pageParserFilter.AllowCode);
			}
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000C243D File Offset: 0x000C143D
		internal void EnsureCodeAllowed()
		{
			if (!this.IsCodeAllowed)
			{
				this.ProcessError(SR.GetString("Code_not_allowed"));
			}
			this.flags[16] = true;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000C2468 File Offset: 0x000C1468
		internal void OnFoundAttributeRequiringCompilation(string attribName)
		{
			if (!this.IsCodeAllowed)
			{
				this.ProcessError(SR.GetString("Attrib_not_allowed", new object[] { attribName }));
			}
			this.flags[16] = true;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000C24A8 File Offset: 0x000C14A8
		internal void OnFoundDirectiveRequiringCompilation(string directiveName)
		{
			if (!this.IsCodeAllowed)
			{
				this.ProcessError(SR.GetString("Directive_not_allowed", new object[] { directiveName }));
			}
			this.flags[16] = true;
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000C24E8 File Offset: 0x000C14E8
		internal void OnFoundEventHandler(string directiveName)
		{
			if (!this.IsCodeAllowed)
			{
				this.ProcessError(SR.GetString("Event_not_allowed", new object[] { directiveName }));
			}
			this.flags[16] = true;
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000C2527 File Offset: 0x000C1527
		// (set) Token: 0x06002BBF RID: 11199 RVA: 0x000C2530 File Offset: 0x000C1530
		internal IDesignerHost DesignerHost
		{
			get
			{
				return this._designerHost;
			}
			set
			{
				this._designerHost = value;
				this._typeResolutionService = null;
				if (this._designerHost != null)
				{
					this._typeResolutionService = (ITypeResolutionService)this._designerHost.GetService(typeof(ITypeResolutionService));
					if (this._typeResolutionService == null)
					{
						throw new ArgumentException(SR.GetString("TypeResService_Needed"));
					}
				}
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x000C258B File Offset: 0x000C158B
		internal virtual bool FApplicationFile
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x000C258E File Offset: 0x000C158E
		// (set) Token: 0x06002BC2 RID: 11202 RVA: 0x000C2596 File Offset: 0x000C1596
		internal EventHandler DesignTimeDataBindHandler
		{
			get
			{
				return this._designTimeDataBindHandler;
			}
			set
			{
				this._designTimeDataBindHandler = value;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x000C259F File Offset: 0x000C159F
		internal AssemblySet AssemblyDependencies
		{
			get
			{
				return this._assemblyDependencies;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x000C25A7 File Offset: 0x000C15A7
		internal StringSet SourceDependencies
		{
			get
			{
				return this._sourceDependencies;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x000C25AF File Offset: 0x000C15AF
		internal HttpStaticObjectsCollection SessionObjects
		{
			get
			{
				return this._sessionObjects;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x000C25B7 File Offset: 0x000C15B7
		internal HttpStaticObjectsCollection ApplicationObjects
		{
			get
			{
				return this._applicationObjects;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000C25BF File Offset: 0x000C15BF
		internal RootBuilder RootBuilder
		{
			get
			{
				this.EnsureRootBuilderCreated();
				return this._rootBuilder;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000C25CD File Offset: 0x000C15CD
		internal Hashtable NamespaceEntries
		{
			get
			{
				return this._namespaceEntries;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000C25D5 File Offset: 0x000C15D5
		internal CompilerType CompilerType
		{
			get
			{
				return this._compilerType;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002BCA RID: 11210 RVA: 0x000C25DD File Offset: 0x000C15DD
		internal ArrayList ScriptList
		{
			get
			{
				return this._scriptList;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000C25E5 File Offset: 0x000C15E5
		internal int TypeHashCode
		{
			get
			{
				return this._typeHashCode.CombinedHash32;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x000C25F2 File Offset: 0x000C15F2
		internal ArrayList PageObjectList
		{
			get
			{
				return this._pageObjectList;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000C25FA File Offset: 0x000C15FA
		internal CompilerParameters CompilParams
		{
			get
			{
				return this._compilerType.CompilerParameters;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000C2607 File Offset: 0x000C1607
		internal bool FExplicit
		{
			get
			{
				return this.flags[4096];
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000C2619 File Offset: 0x000C1619
		internal bool FLinePragmas
		{
			get
			{
				return !this.flags[32768];
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000C262E File Offset: 0x000C162E
		internal bool FStrict
		{
			get
			{
				return this.flags[65536];
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x000C2640 File Offset: 0x000C1640
		internal VirtualPath CodeFileVirtualPath
		{
			get
			{
				return this._codeFileVirtualPath;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000C2648 File Offset: 0x000C1648
		internal string GeneratedClassName
		{
			get
			{
				return this._generatedClassName;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000C2650 File Offset: 0x000C1650
		internal string GeneratedNamespace
		{
			get
			{
				if (this._generatedNamespace == null)
				{
					return "ASP";
				}
				return this._generatedNamespace;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000C2668 File Offset: 0x000C1668
		internal ControlBuilderInterceptor ControlBuilderInterceptor
		{
			get
			{
				if (this._controlBuilderInterceptor == null && !string.IsNullOrEmpty(AppSettings.ControlBuilderInterceptor))
				{
					if (TemplateParser._controlBuilderInterceptorType == null)
					{
						TemplateParser._controlBuilderInterceptorType = Type.GetType(AppSettings.ControlBuilderInterceptor, true, false);
					}
					object obj = Activator.CreateInstance(TemplateParser._controlBuilderInterceptorType);
					this._controlBuilderInterceptor = ControlBuilderInterceptor.WrapImplementingObject(obj);
				}
				return this._controlBuilderInterceptor;
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000C26C4 File Offset: 0x000C16C4
		internal static Control ParseControl(string content, VirtualPath virtualPath, bool ignoreFilter)
		{
			if (content == null)
			{
				return null;
			}
			ITemplate template = TemplateParser.ParseTemplate(content, virtualPath, ignoreFilter);
			Control control = new Control();
			template.InstantiateIn(control);
			return control;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000C26F0 File Offset: 0x000C16F0
		private static ITemplate ParseTemplate(string content, VirtualPath virtualPath, bool ignoreFilter)
		{
			TemplateParser templateParser = new UserControlParser();
			return templateParser.ParseTemplateInternal(content, virtualPath, ignoreFilter);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000C270C File Offset: 0x000C170C
		private ITemplate ParseTemplateInternal(string content, VirtualPath virtualPath, bool ignoreFilter)
		{
			base.CurrentVirtualPath = virtualPath;
			this.CompilationMode = CompilationMode.Never;
			this._text = content;
			this.flags[33554432] = ignoreFilter;
			this.Parse();
			return this.RootBuilder;
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000C2740 File Offset: 0x000C1740
		internal virtual void PrepareParse()
		{
			if (this._circularReferenceChecker == null)
			{
				this._circularReferenceChecker = new CaseInsensitiveStringSet();
			}
			this._baseType = this.DefaultBaseType;
			this._mainDirectiveConfigSettings = TemplateParser.CreateEmptyAttributeBag();
			if (!this.FInDesigner)
			{
				this._compConfig = RuntimeConfig.GetConfig(base.CurrentVirtualPath).Compilation;
				this._pagesConfig = RuntimeConfig.GetConfig(base.CurrentVirtualPath).Pages;
			}
			this.ProcessConfigSettings();
			this._typeMapper = new MainTagNameToTypeMapper(this as BaseTemplateParser);
			this._typeMapper.RegisterTag("object", typeof(ObjectTag));
			this._sourceDependencies = new CaseInsensitiveStringSet();
			this._idListStack = new Stack();
			this._idList = new CaseInsensitiveStringSet();
			this._scriptList = new ArrayList();
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000C2808 File Offset: 0x000C1808
		private void EnsureRootBuilderCreated()
		{
			if (this._rootBuilder != null)
			{
				return;
			}
			if (this.BaseType == this.DefaultBaseType)
			{
				this._rootBuilder = this.CreateDefaultFileLevelBuilder();
			}
			else
			{
				Type fileLevelControlBuilderType = this.GetFileLevelControlBuilderType();
				if (fileLevelControlBuilderType == null)
				{
					this._rootBuilder = this.CreateDefaultFileLevelBuilder();
				}
				else
				{
					this._rootBuilder = (RootBuilder)HttpRuntime.CreateNonPublicInstance(fileLevelControlBuilderType);
				}
			}
			this._rootBuilder.Line = 1;
			this._rootBuilder.Init(this, null, null, null, null, null);
			this._rootBuilder.SetTypeMapper(this.TypeMapper);
			this._rootBuilder.VirtualPath = base.CurrentVirtualPath;
			this._builderStack = new Stack();
			this._builderStack.Push(new BuilderStackEntry(this.RootBuilder, null, null, 0, null, 0));
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x000C28C8 File Offset: 0x000C18C8
		internal virtual Type DefaultFileLevelBuilderType
		{
			get
			{
				return typeof(RootBuilder);
			}
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000C28D4 File Offset: 0x000C18D4
		internal virtual RootBuilder CreateDefaultFileLevelBuilder()
		{
			return new RootBuilder();
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000C28DC File Offset: 0x000C18DC
		private Type GetFileLevelControlBuilderType()
		{
			FileLevelControlBuilderAttribute fileLevelControlBuilderAttribute = null;
			object[] customAttributes = this.BaseType.GetCustomAttributes(typeof(FileLevelControlBuilderAttribute), true);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				fileLevelControlBuilderAttribute = (FileLevelControlBuilderAttribute)customAttributes[0];
			}
			if (fileLevelControlBuilderAttribute == null)
			{
				return null;
			}
			Util.CheckAssignableType(this.DefaultFileLevelBuilderType, fileLevelControlBuilderAttribute.BuilderType);
			return fileLevelControlBuilderAttribute.BuilderType;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000C2930 File Offset: 0x000C1930
		internal virtual void ProcessConfigSettings()
		{
			if (this._compConfig != null)
			{
				this.flags[4096] = this._compConfig.Explicit;
				this.flags[65536] = this._compConfig.Strict;
			}
			if (this.PagesConfig != null)
			{
				this._namespaceEntries = this.PagesConfig.Namespaces.NamespaceEntries;
				if (this._namespaceEntries != null)
				{
					this._namespaceEntries = (Hashtable)this._namespaceEntries.Clone();
				}
				if (!this.flags[33554432])
				{
					this._pageParserFilter = PageParserFilter.Create(this.PagesConfig, base.CurrentVirtualPath, this);
				}
			}
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000C29E1 File Offset: 0x000C19E1
		internal void Parse(ICollection referencedAssemblies, VirtualPath virtualPath)
		{
			this._referencedAssemblies = referencedAssemblies;
			base.CurrentVirtualPath = virtualPath;
			this.Parse();
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000C29F8 File Offset: 0x000C19F8
		internal void Parse()
		{
			Thread currentThread = Thread.CurrentThread;
			CultureInfo currentCulture = currentThread.CurrentCulture;
			currentThread.CurrentCulture = CultureInfo.InvariantCulture;
			try
			{
				try
				{
					this.PrepareParse();
					this.ParseInternal();
					this.HandlePostParse();
				}
				finally
				{
					currentThread.CurrentCulture = currentCulture;
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000C2A5C File Offset: 0x000C1A5C
		internal virtual void ParseInternal()
		{
			if (this._text != null)
			{
				this.ParseString(this._text, base.CurrentVirtualPath, Encoding.UTF8);
				return;
			}
			this.AddSourceDependency(base.CurrentVirtualPath);
			this.ParseFile(null, base.CurrentVirtualPath.VirtualPathString);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000C2A9C File Offset: 0x000C1A9C
		internal TemplateParser()
		{
			this.ThrowOnFirstParseError = true;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000C2ABD File Offset: 0x000C1ABD
		protected void ParseFile(string physicalPath, string virtualPath)
		{
			this.ParseFile(physicalPath, VirtualPath.Create(virtualPath));
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000C2ACC File Offset: 0x000C1ACC
		internal void ParseFile(string physicalPath, VirtualPath virtualPath)
		{
			string text = ((physicalPath != null) ? physicalPath : virtualPath.VirtualPathString);
			if (this._circularReferenceChecker.Contains(text))
			{
				this.ProcessError(SR.GetString("Circular_include"));
				return;
			}
			this._circularReferenceChecker.Add(text);
			try
			{
				if (physicalPath != null)
				{
					StreamReader streamReader2;
					StreamReader streamReader = (streamReader2 = Util.ReaderFromFile(physicalPath, base.CurrentVirtualPath));
					try
					{
						this.ParseReader(streamReader, virtualPath);
						goto IL_0086;
					}
					finally
					{
						if (streamReader2 != null)
						{
							((IDisposable)streamReader2).Dispose();
						}
					}
				}
				using (Stream stream = virtualPath.OpenFile())
				{
					StreamReader streamReader = Util.ReaderFromStream(stream, base.CurrentVirtualPath);
					this.ParseReader(streamReader, virtualPath);
				}
				IL_0086:;
			}
			finally
			{
				this._circularReferenceChecker.Remove(text);
			}
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000C2B98 File Offset: 0x000C1B98
		private void ParseReader(StreamReader reader, VirtualPath virtualPath)
		{
			string text = reader.ReadToEnd();
			this._text = text;
			this.ParseString(text, virtualPath, reader.CurrentEncoding);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000C2BC1 File Offset: 0x000C1BC1
		private void AddLiteral(string literal)
		{
			if (this._literalBuilder == null)
			{
				this._literalBuilder = new StringBuilder();
			}
			this._literalBuilder.Append(literal);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000C2BE3 File Offset: 0x000C1BE3
		private string GetLiteral()
		{
			if (this._literalBuilder == null)
			{
				return null;
			}
			return this._literalBuilder.ToString();
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000C2BFA File Offset: 0x000C1BFA
		internal void UpdateTypeHashCode(string text)
		{
			this._typeHashCode.AddObject(text);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000C2C08 File Offset: 0x000C1C08
		internal void ParseString(string text, VirtualPath virtualPath, Encoding fileEncoding)
		{
			VirtualPath currentVirtualPath = base.CurrentVirtualPath;
			int lineNumber = this._lineNumber;
			base.CurrentVirtualPath = virtualPath;
			this._lineNumber = 1;
			this.flags[8] = true;
			try
			{
				this.ParseStringInternal(text, fileEncoding);
				if (this.HasParserErrors)
				{
					ParserError parserError = this.ParserErrors[0];
					Exception ex = parserError.Exception;
					if (ex == null)
					{
						ex = new HttpException(parserError.ErrorText);
					}
					HttpParseException ex2 = new HttpParseException(parserError.ErrorText, ex, parserError.VirtualPath, this.Text, parserError.Line);
					for (int i = 1; i < this.ParserErrors.Count; i++)
					{
						ex2.ParserErrors.Add(this.ParserErrors[i]);
					}
					throw ex2;
				}
				this.ThrowOnFirstParseError = true;
			}
			catch (Exception ex3)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_PRE_PROCESSING);
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
				if (HttpException.GetErrorFormatter(ex3) == null)
				{
					throw new HttpParseException(ex3.Message, ex3, base.CurrentVirtualPath, text, this._lineNumber);
				}
				throw;
			}
			finally
			{
				base.CurrentVirtualPath = currentVirtualPath;
				this._lineNumber = lineNumber;
			}
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000C2D3C File Offset: 0x000C1D3C
		private void ParseStringInternal(string text, Encoding fileEncoding)
		{
			int num = 0;
			int num2 = text.LastIndexOf('>');
			do
			{
				Match match;
				if ((match = BaseParser.textRegex.Match(text, num)).Success)
				{
					this.AddLiteral(match.ToString());
					this._lineNumber += Util.LineCount(text, num, match.Index + match.Length);
					num = match.Index + match.Length;
				}
				if (num == text.Length)
				{
					break;
				}
				bool flag = false;
				if (!this.flags[2] && (match = BaseParser.DirectiveRegex.Match(text, num)).Success)
				{
					this.ProcessLiteral();
					ParsedAttributeCollection parsedAttributeCollection;
					string text3;
					string text2 = this.ProcessAttributes(match, out parsedAttributeCollection, true, out text3);
					try
					{
						this.PreprocessDirective(text2, parsedAttributeCollection);
						this.ProcessDirective(text2, parsedAttributeCollection);
					}
					catch (Exception ex)
					{
						this.ProcessException(ex);
					}
					if (text2.Length == 0 && this._codeFileVirtualPath != null)
					{
						this.CreateModifiedMainDirectiveFileIfNeeded(text, match, parsedAttributeCollection, fileEncoding);
					}
					this.flags[8] = true;
				}
				else
				{
					if ((match = BaseParser.includeRegex.Match(text, num)).Success)
					{
						try
						{
							this.ProcessServerInclude(match);
							goto IL_0286;
						}
						catch (Exception ex2)
						{
							this.ProcessException(ex2);
							goto IL_0286;
						}
					}
					if (!(match = BaseParser.commentRegex.Match(text, num)).Success)
					{
						if (!this.flags[2] && (match = BaseParser.aspExprRegex.Match(text, num)).Success)
						{
							this.ProcessCodeBlock(match, CodeBlockType.Expression, text);
						}
						else if (!this.flags[2] && (match = BaseParser.databindExprRegex.Match(text, num)).Success)
						{
							this.ProcessCodeBlock(match, CodeBlockType.DataBinding, text);
						}
						else if (!this.flags[2] && (match = BaseParser.aspCodeRegex.Match(text, num)).Success)
						{
							string text4 = match.Groups["code"].Value.Trim();
							if (text4.StartsWith("$", StringComparison.Ordinal))
							{
								this.ProcessError(SR.GetString("ExpressionBuilder_LiteralExpressionsNotAllowed", new object[]
								{
									match.ToString(),
									text4
								}));
							}
							else
							{
								this.ProcessCodeBlock(match, CodeBlockType.Code, text);
							}
						}
						else
						{
							if (!this.flags[2] && num2 > num && (match = BaseParser.TagRegex.Match(text, num)).Success)
							{
								try
								{
									if (!this.ProcessBeginTag(match, text))
									{
										flag = true;
									}
									goto IL_0286;
								}
								catch (Exception ex3)
								{
									this.ProcessException(ex3);
									goto IL_0286;
								}
							}
							if ((match = BaseParser.endtagRegex.Match(text, num)).Success && !this.ProcessEndTag(match))
							{
								flag = true;
							}
						}
					}
				}
				IL_0286:
				if (match == null || !match.Success || flag)
				{
					if (!flag && !this.flags[2])
					{
						this.DetectSpecialServerTagError(text, num);
					}
					num++;
					this.AddLiteral("<");
				}
				else
				{
					this._lineNumber += Util.LineCount(text, num, match.Index + match.Length);
					num = match.Index + match.Length;
				}
			}
			while (num != text.Length);
			if (this.flags[2] && !this.IgnoreParseErrors)
			{
				this._lineNumber = this._scriptStartLineNumber;
				this.ProcessError(SR.GetString("Unexpected_eof_looking_for_tag", new object[] { "script" }));
				return;
			}
			this.ProcessLiteral();
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000C30B8 File Offset: 0x000C20B8
		private void CreateModifiedMainDirectiveFileIfNeeded(string text, Match match, ParsedAttributeCollection mainDirective, Encoding fileEncoding)
		{
			TextWriter updatableDeploymentTargetWriter = BuildManager.GetUpdatableDeploymentTargetWriter(base.CurrentVirtualPath, fileEncoding);
			if (updatableDeploymentTargetWriter == null)
			{
				return;
			}
			using (updatableDeploymentTargetWriter)
			{
				updatableDeploymentTargetWriter.Write(text.Substring(0, match.Index));
				updatableDeploymentTargetWriter.Write("<%@ " + this.DefaultDirectiveName);
				foreach (object obj in ((IEnumerable)mainDirective))
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text2 = (string)dictionaryEntry.Key;
					string text3 = (string)dictionaryEntry.Value;
					if (!StringUtil.EqualsIgnoreCase(text2, "codefile") && !StringUtil.EqualsIgnoreCase(text2, "codefilebaseclass"))
					{
						if (StringUtil.EqualsIgnoreCase(text2, "inherits"))
						{
							text3 = "__ASPNET_INHERITS";
						}
						updatableDeploymentTargetWriter.Write(" ");
						updatableDeploymentTargetWriter.Write(text2);
						updatableDeploymentTargetWriter.Write("=\"");
						updatableDeploymentTargetWriter.Write(text3);
						updatableDeploymentTargetWriter.Write("\"");
					}
				}
				updatableDeploymentTargetWriter.Write(" %>");
				updatableDeploymentTargetWriter.Write(text.Substring(match.Index + match.Length));
			}
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000C321C File Offset: 0x000C221C
		internal virtual void HandlePostParse()
		{
			if (!this.flags[2048])
			{
				this.ProcessMainDirective(this._mainDirectiveConfigSettings);
				this.flags[2048] = true;
			}
			if (this._pageParserFilter != null && !this._pageParserFilter.AllowBaseType(this.BaseType))
			{
				throw new HttpException(SR.GetString("Base_type_not_allowed", new object[] { this.BaseType.FullName }));
			}
			if (this.BuilderStack.Count > 1)
			{
				BuilderStackEntry builderStackEntry = (BuilderStackEntry)this._builderStack.Peek();
				string @string = SR.GetString("Unexpected_eof_looking_for_tag", new object[] { builderStackEntry._tagName });
				this.ProcessException(new HttpParseException(@string, null, builderStackEntry.VirtualPath, builderStackEntry._inputText, builderStackEntry.Line));
				return;
			}
			if (this._compilerType == null)
			{
				if (!this.FInDesigner)
				{
					this._compilerType = CompilationUtil.GetDefaultLanguageCompilerInfo(this._compConfig, base.CurrentVirtualPath);
				}
				else
				{
					this._compilerType = CompilationUtil.GetCodeDefaultLanguageCompilerInfo();
				}
			}
			CompilerParameters compilerParameters = this._compilerType.CompilerParameters;
			if (this.flags[8192])
			{
				compilerParameters.IncludeDebugInformation = this.flags[16384];
			}
			if (compilerParameters.IncludeDebugInformation)
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Medium, "Debugging_not_supported_in_low_trust");
			}
			if (this._warningLevel >= 0)
			{
				compilerParameters.WarningLevel = this._warningLevel;
				compilerParameters.TreatWarningsAsErrors = this._warningLevel > 0;
			}
			if (this._compilerOptions != null)
			{
				compilerParameters.CompilerOptions = this._compilerOptions;
			}
			if (this._pageParserFilter != null)
			{
				this._pageParserFilter.ParseComplete(this.RootBuilder);
			}
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000C33C8 File Offset: 0x000C23C8
		private void ProcessLiteral()
		{
			string literal = this.GetLiteral();
			if (string.IsNullOrEmpty(literal))
			{
				this.flags[8] = false;
				return;
			}
			if (this.FApplicationFile)
			{
				int num = Util.FirstNonWhiteSpaceIndex(literal);
				if (num >= 0 && !this.IgnoreParseErrors)
				{
					this._lineNumber -= Util.LineCount(literal, num, literal.Length);
					this.ProcessError(SR.GetString("Invalid_app_file_content"));
				}
			}
			else
			{
				bool flag = false;
				if (this.flags[8])
				{
					this.flags[8] = false;
					if (Util.IsWhiteSpaceString(literal))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (!this.flags[2048])
					{
						this.ProcessMainDirective(this._mainDirectiveConfigSettings);
						this.flags[2048] = true;
					}
					ControlBuilder builder = ((BuilderStackEntry)this.BuilderStack.Peek())._builder;
					try
					{
						builder.AppendLiteralString(literal);
					}
					catch (Exception ex)
					{
						if (!this.IgnoreParseErrors)
						{
							int num2 = Util.FirstNonWhiteSpaceIndex(literal);
							if (num2 < 0)
							{
								num2 = 0;
							}
							this._lineNumber -= Util.LineCount(literal, num2, literal.Length);
							this.ProcessException(ex);
						}
					}
					this.UpdateTypeHashCode("string");
				}
			}
			this._literalBuilder = null;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000C3520 File Offset: 0x000C2520
		private void ProcessServerScript()
		{
			string text = this.GetLiteral();
			if (string.IsNullOrEmpty(text))
			{
				if (!this.IgnoreParseErrors)
				{
					return;
				}
				text = string.Empty;
			}
			if (!this.flags[4] && !this.PageParserFilterProcessedCodeBlock(CodeConstructType.ScriptTag, text, this._currentScript.Line))
			{
				this.EnsureCodeAllowed();
				this._currentScript.Script = text;
				this._scriptList.Add(this._currentScript);
				this._currentScript = null;
			}
			this._literalBuilder = null;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000C35A0 File Offset: 0x000C25A0
		internal virtual void CheckObjectTagScope(ref ObjectTagScope scope)
		{
			if (scope == ObjectTagScope.Default)
			{
				scope = ObjectTagScope.Page;
			}
			if (scope != ObjectTagScope.Page)
			{
				throw new HttpException(SR.GetString("App_session_only_valid_in_global_asax"));
			}
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000C35C0 File Offset: 0x000C25C0
		private void ProcessObjectTag(ObjectTagBuilder objectBuilder)
		{
			ObjectTagScope scope = objectBuilder.Scope;
			this.CheckObjectTagScope(ref scope);
			if (scope == ObjectTagScope.Page || scope == ObjectTagScope.AppInstance)
			{
				if (this._pageObjectList == null)
				{
					this._pageObjectList = new ArrayList();
				}
				this._pageObjectList.Add(objectBuilder);
				return;
			}
			if (scope == ObjectTagScope.Session)
			{
				if (this._sessionObjects == null)
				{
					this._sessionObjects = new HttpStaticObjectsCollection();
				}
				this._sessionObjects.Add(objectBuilder.ID, objectBuilder.ObjectType, objectBuilder.LateBound);
				return;
			}
			if (scope == ObjectTagScope.Application)
			{
				if (this._applicationObjects == null)
				{
					this._applicationObjects = new HttpStaticObjectsCollection();
				}
				this._applicationObjects.Add(objectBuilder.ID, objectBuilder.ObjectType, objectBuilder.LateBound);
			}
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000C366E File Offset: 0x000C266E
		private void AppendSubBuilder(ControlBuilder builder, ControlBuilder subBuilder)
		{
			if (subBuilder is ObjectTagBuilder)
			{
				this.ProcessObjectTag((ObjectTagBuilder)subBuilder);
				return;
			}
			builder.AppendSubBuilder(subBuilder);
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000C368C File Offset: 0x000C268C
		private bool ProcessBeginTag(Match match, string inputText)
		{
			string value = match.Groups["tagname"].Value;
			ParsedAttributeCollection parsedAttributeCollection;
			string text;
			this.ProcessAttributes(match, out parsedAttributeCollection, false, out text);
			bool success = match.Groups["empty"].Success;
			if (StringUtil.EqualsIgnoreCase(value, "script") && this.flags[1])
			{
				this.ProcessScriptTag(match, inputText, parsedAttributeCollection, success);
				return true;
			}
			if (!this.flags[2048])
			{
				this.ProcessMainDirective(this._mainDirectiveConfigSettings);
				this.flags[2048] = true;
			}
			ControlBuilder controlBuilder = null;
			ControlBuilder controlBuilder2 = null;
			Type type = null;
			string text3;
			string text2 = Util.ParsePropertyDeviceFilter(value, out text3);
			if (this.BuilderStack.Count > 1)
			{
				controlBuilder = ((BuilderStackEntry)this._builderStack.Peek())._builder;
				if (controlBuilder is StringPropertyBuilder)
				{
					return false;
				}
				controlBuilder2 = controlBuilder.CreateChildBuilder(text2, text3, parsedAttributeCollection, this, controlBuilder, this._id, this._lineNumber, base.CurrentVirtualPath, ref type, false);
			}
			if (controlBuilder2 == null && this.flags[1])
			{
				controlBuilder2 = this.RootBuilder.CreateChildBuilder(text2, text3, parsedAttributeCollection, this, controlBuilder, this._id, this._lineNumber, base.CurrentVirtualPath, ref type, false);
			}
			if (controlBuilder2 == null && this._builderStack.Count > 1 && !success)
			{
				BuilderStackEntry builderStackEntry = (BuilderStackEntry)this._builderStack.Peek();
				if (StringUtil.EqualsIgnoreCase(value, builderStackEntry._tagName))
				{
					builderStackEntry._repeatCount++;
				}
			}
			if (controlBuilder2 == null)
			{
				if (!this.flags[1] || this.IgnoreParseErrors)
				{
					return false;
				}
				this.ProcessError(SR.GetString("Unknown_server_tag", new object[] { value }));
				return true;
			}
			else
			{
				if (this._pageParserFilter != null && !this._pageParserFilter.AllowControlInternal(type, controlBuilder2))
				{
					this.ProcessError(SR.GetString("Control_type_not_allowed", new object[] { type.FullName }));
					return true;
				}
				if (text != null)
				{
					this.ProcessError(SR.GetString("Duplicate_attr_in_tag", new object[] { text }));
				}
				this._id = controlBuilder2.ID;
				if (this._id != null)
				{
					if (!CodeGenerator.IsValidLanguageIndependentIdentifier(this._id))
					{
						this.ProcessError(SR.GetString("Invalid_identifier", new object[] { this._id }));
						return true;
					}
					if (this._idList.Contains(this._id))
					{
						this.ProcessError(SR.GetString("Id_already_used", new object[] { this._id }));
						return true;
					}
					this._idList.Add(this._id);
				}
				else if (this.flags[1])
				{
					PartialCachingAttribute partialCachingAttribute = (PartialCachingAttribute)TypeDescriptor.GetAttributes(type)[typeof(PartialCachingAttribute)];
					if (partialCachingAttribute != null)
					{
						this._id = "_ctrl_" + this._controlCount.ToString(NumberFormatInfo.InvariantInfo);
						controlBuilder2.ID = this._id;
						this._controlCount++;
						controlBuilder2.PreprocessAttribute(string.Empty, "id", this._id, false);
					}
				}
				this.ProcessLiteral();
				if (type != null)
				{
					this.UpdateTypeHashCode(type.FullName);
				}
				if (!success && controlBuilder2.HasBody())
				{
					if (controlBuilder2 is TemplateBuilder && ((TemplateBuilder)controlBuilder2).AllowMultipleInstances)
					{
						this._idListStack.Push(this._idList);
						this._idList = new CaseInsensitiveStringSet();
					}
					this._builderStack.Push(new BuilderStackEntry(controlBuilder2, value, base.CurrentVirtualPathString, this._lineNumber, inputText, match.Index + match.Length));
				}
				else
				{
					controlBuilder = ((BuilderStackEntry)this._builderStack.Peek())._builder;
					this.AppendSubBuilder(controlBuilder, controlBuilder2);
					controlBuilder2.CloseControl();
				}
				return true;
			}
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000C3A7C File Offset: 0x000C2A7C
		private void ProcessScriptTag(Match match, string text, IDictionary attribs, bool fSelfClosed)
		{
			this.ProcessLiteral();
			this.flags[8] = true;
			VirtualPath virtualPath = Util.GetAndRemoveVirtualPathAttribute(attribs, "src");
			if (virtualPath != null)
			{
				this.EnsureCodeAllowed();
				virtualPath = base.ResolveVirtualPath(virtualPath);
				HttpRuntime.CheckVirtualFilePermission(virtualPath.VirtualPathString);
				this.AddSourceDependency(virtualPath);
				this.ProcessLanguageAttribute((string)attribs["language"]);
				this._currentScript = new ScriptBlockData(1, 1, virtualPath.VirtualPathString);
				this._currentScript.Script = Util.StringFromVirtualPath(virtualPath);
				this._scriptList.Add(this._currentScript);
				this._currentScript = null;
				if (!fSelfClosed)
				{
					this.flags[2] = true;
					this._scriptStartLineNumber = this._lineNumber;
					this.flags[4] = true;
				}
				return;
			}
			this.ProcessLanguageAttribute((string)attribs["language"]);
			int num = match.Index + match.Length;
			int num2 = text.LastIndexOfAny(TemplateParser.s_newlineChars, num - 1);
			int num3 = num - num2;
			this._currentScript = new ScriptBlockData(this._lineNumber, num3, base.CurrentVirtualPathString);
			if (fSelfClosed)
			{
				this.ProcessError(SR.GetString("Script_tag_without_src_must_have_content"));
			}
			this.flags[2] = true;
			this._scriptStartLineNumber = this._lineNumber;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000C3BD0 File Offset: 0x000C2BD0
		private bool ProcessEndTag(Match match)
		{
			string value = match.Groups["tagname"].Value;
			if (!this.flags[2])
			{
				return this.MaybeTerminateControl(value, match.Index);
			}
			if (!StringUtil.EqualsIgnoreCase(value, "script"))
			{
				return false;
			}
			this.ProcessServerScript();
			this.flags[2] = false;
			this.flags[4] = false;
			return true;
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000C3C3F File Offset: 0x000C2C3F
		internal bool IsExpressionBuilderValue(string val)
		{
			return ControlBuilder.expressionBuilderRegex.Match(val, 0).Success;
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002BF5 RID: 11253
		internal abstract string DefaultDirectiveName { get; }

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000C3C52 File Offset: 0x000C2C52
		internal void PreprocessDirective(string directiveName, IDictionary directive)
		{
			if (this._pageParserFilter == null)
			{
				return;
			}
			if (directiveName.Length == 0)
			{
				directiveName = this.DefaultDirectiveName;
			}
			this._pageParserFilter.PreprocessDirective(directiveName, directive);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000C3C7C File Offset: 0x000C2C7C
		internal virtual void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (directiveName.Length == 0)
			{
				if (this.FInDesigner)
				{
					return;
				}
				if (this.flags[1024])
				{
					this.ProcessError(SR.GetString("Only_one_directive_allowed", new object[] { this.DefaultDirectiveName }));
					return;
				}
				if (this._mainDirectiveConfigSettings != null)
				{
					foreach (object obj in this._mainDirectiveConfigSettings)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (!directive.Contains(dictionaryEntry.Key))
						{
							directive[dictionaryEntry.Key] = dictionaryEntry.Value;
						}
					}
				}
				this.ProcessMainDirective(directive);
				this.flags[1024] = true;
				this.flags[2048] = true;
				return;
			}
			else if (StringUtil.EqualsIgnoreCase(directiveName, "assembly"))
			{
				string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "name");
				VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "src");
				Util.CheckUnknownDirectiveAttributes(directiveName, directive);
				if (andRemoveNonEmptyAttribute != null && andRemoveVirtualPathAttribute != null)
				{
					this.ProcessError(SR.GetString("Attributes_mutually_exclusive", new object[] { "Name", "Src" }));
				}
				if (andRemoveNonEmptyAttribute != null)
				{
					this.AddAssemblyDependency(andRemoveNonEmptyAttribute);
					return;
				}
				if (andRemoveVirtualPathAttribute != null)
				{
					this.ImportSourceFile(andRemoveVirtualPathAttribute);
					return;
				}
				this.ProcessError(SR.GetString("Missing_attr", new object[] { "name" }));
				return;
			}
			else
			{
				if (StringUtil.EqualsIgnoreCase(directiveName, "import"))
				{
					this.ProcessImportDirective(directiveName, directive);
					return;
				}
				if (!StringUtil.EqualsIgnoreCase(directiveName, "implements"))
				{
					if (!this.FInDesigner)
					{
						this.ProcessError(SR.GetString("Unknown_directive", new object[] { directiveName }));
					}
					return;
				}
				this.OnFoundDirectiveRequiringCompilation(directiveName);
				string andRemoveRequiredAttribute = Util.GetAndRemoveRequiredAttribute(directive, "interface");
				Util.CheckUnknownDirectiveAttributes(directiveName, directive);
				Type type = this.GetType(andRemoveRequiredAttribute);
				if (!type.IsInterface)
				{
					this.ProcessError(SR.GetString("Invalid_type_to_implement", new object[] { andRemoveRequiredAttribute }));
					return;
				}
				if (this._implementedInterfaces == null)
				{
					this._implementedInterfaces = new ArrayList();
				}
				this._implementedInterfaces.Add(type);
				return;
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000C3ED8 File Offset: 0x000C2ED8
		internal virtual void ProcessMainDirective(IDictionary mainDirective)
		{
			IDictionary dictionary = new HybridDictionary();
			ParsedAttributeCollection parsedAttributeCollection = null;
			foreach (object obj in mainDirective)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				string text2 = Util.ParsePropertyDeviceFilter(text, out text);
				try
				{
					if (!this.ProcessMainDirectiveAttribute(text2, text, (string)dictionaryEntry.Value, dictionary))
					{
						if (parsedAttributeCollection == null)
						{
							parsedAttributeCollection = TemplateParser.CreateEmptyAttributeBag();
						}
						parsedAttributeCollection.AddFilteredAttribute(text2, text, (string)dictionaryEntry.Value);
					}
				}
				catch (Exception ex)
				{
					this.ProcessException(ex);
				}
			}
			this.PostProcessMainDirectiveAttributes(dictionary);
			this.RootBuilder.SetControlType(this.BaseType);
			if (parsedAttributeCollection == null)
			{
				return;
			}
			this.RootBuilder.ProcessImplicitResources(parsedAttributeCollection);
			foreach (object obj2 in parsedAttributeCollection.GetFilteredAttributeDictionaries())
			{
				FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)obj2;
				string filter = filteredAttributeDictionary.Filter;
				foreach (object obj3 in ((IEnumerable)filteredAttributeDictionary))
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj3;
					string text3 = (string)dictionaryEntry2.Key;
					this.ProcessUnknownMainDirectiveAttribute(filter, text3, (string)dictionaryEntry2.Value);
				}
			}
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000C4080 File Offset: 0x000C3080
		internal virtual bool ProcessMainDirectiveAttribute(string deviceName, string name, string value, IDictionary parseData)
		{
			if (name != null)
			{
				if (<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x6002ba4-1 == null)
				{
					<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x6002ba4-1 = new Dictionary<string, int>(13)
					{
						{ "description", 0 },
						{ "codebehind", 1 },
						{ "debug", 2 },
						{ "linepragmas", 3 },
						{ "warninglevel", 4 },
						{ "compileroptions", 5 },
						{ "explicit", 6 },
						{ "strict", 7 },
						{ "language", 8 },
						{ "src", 9 },
						{ "inherits", 10 },
						{ "classname", 11 },
						{ "codefile", 12 }
					};
				}
				int num;
				if (<PrivateImplementationDetails>{D1C745C9-496D-4107-B23F-453EA6C426FC}.$$method0x6002ba4-1.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
					case 1:
						break;
					case 2:
						this.flags[16384] = Util.GetBooleanAttribute(name, value);
						if (this.flags[16384] && !HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
						{
							throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "debug" }));
						}
						this.flags[8192] = true;
						break;
					case 3:
						this.flags[32768] = !Util.GetBooleanAttribute(name, value);
						break;
					case 4:
						this._warningLevel = Util.GetNonNegativeIntegerAttribute(name, value);
						break;
					case 5:
					{
						this.OnFoundAttributeRequiringCompilation(name);
						string text = value.Trim();
						CompilationUtil.CheckCompilerOptionsAllowed(text, false, null, 0);
						this._compilerOptions = text;
						break;
					}
					case 6:
						this.flags[4096] = Util.GetBooleanAttribute(name, value);
						break;
					case 7:
						this.flags[65536] = Util.GetBooleanAttribute(name, value);
						break;
					case 8:
					{
						this.ValidateBuiltInAttribute(deviceName, name, value);
						string nonEmptyAttribute = Util.GetNonEmptyAttribute(name, value);
						this.ProcessLanguageAttribute(nonEmptyAttribute);
						break;
					}
					case 9:
						this.OnFoundAttributeRequiringCompilation(name);
						parseData[name] = Util.GetNonEmptyAttribute(name, value);
						break;
					case 10:
						parseData[name] = Util.GetNonEmptyAttribute(name, value);
						break;
					case 11:
						this._generatedClassName = Util.GetNonEmptyFullClassNameAttribute(name, value, ref this._generatedNamespace);
						break;
					case 12:
						this.OnFoundAttributeRequiringCompilation(name);
						try
						{
							this.ProcessCodeFile(VirtualPath.Create(Util.GetNonEmptyAttribute(name, value)));
							break;
						}
						catch (Exception ex)
						{
							this.ProcessException(ex);
							break;
						}
						return false;
					default:
						return false;
					}
					this.ValidateBuiltInAttribute(deviceName, name, value);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000C4334 File Offset: 0x000C3334
		internal void ValidateBuiltInAttribute(string deviceName, string name, string value)
		{
			if (this.IsExpressionBuilderValue(value))
			{
				this.ProcessError(SR.GetString("Illegal_Resource_Builder", new object[] { name }));
			}
			if (deviceName.Length > 0)
			{
				this.ProcessError(SR.GetString("Illegal_Device", new object[] { name }));
			}
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000C438C File Offset: 0x000C338C
		internal virtual void ProcessUnknownMainDirectiveAttribute(string filter, string attribName, string value)
		{
			this.ProcessError(SR.GetString("Attr_not_supported_in_directive", new object[] { attribName, this.DefaultDirectiveName }));
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000C43C0 File Offset: 0x000C33C0
		internal virtual void PostProcessMainDirectiveAttributes(IDictionary parseData)
		{
			string text = (string)parseData["src"];
			Assembly assembly = null;
			if (text != null)
			{
				try
				{
					assembly = this.ImportSourceFile(VirtualPath.Create(text));
				}
				catch (Exception ex)
				{
					this.ProcessException(ex);
				}
			}
			string text2 = (string)parseData["codefilebaseclass"];
			if (text2 != null && this._codeFileVirtualPath == null)
			{
				throw new HttpException(SR.GetString("CodeFileBaseClass_Without_Codefile"));
			}
			string text3 = (string)parseData["inherits"];
			if (text3 != null)
			{
				try
				{
					this.ProcessInheritsAttribute(text3, text2, text, assembly);
					return;
				}
				catch (Exception ex2)
				{
					this.ProcessException(ex2);
					return;
				}
			}
			if (this._codeFileVirtualPath != null)
			{
				throw new HttpException(SR.GetString("Codefile_without_inherits"));
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000C4498 File Offset: 0x000C3498
		private void ProcessInheritsAttribute(string baseTypeName, string codeFileBaseTypeName, string src, Assembly assembly)
		{
			if (this._codeFileVirtualPath != null)
			{
				this._baseTypeName = Util.GetNonEmptyFullClassNameAttribute("inherits", baseTypeName, ref this._baseTypeNamespace);
				baseTypeName = codeFileBaseTypeName;
				if (baseTypeName == null)
				{
					return;
				}
			}
			Type type = null;
			if (assembly != null)
			{
				type = assembly.GetType(baseTypeName, false, true);
			}
			else
			{
				try
				{
					type = this.GetType(baseTypeName);
				}
				catch
				{
					if (this._generatedNamespace == null)
					{
						throw;
					}
					if (baseTypeName.IndexOf('.') >= 0)
					{
						throw;
					}
					try
					{
						string text = this._generatedNamespace + "." + baseTypeName;
						type = this.GetType(text);
					}
					catch
					{
					}
					if (type == null)
					{
						throw;
					}
				}
			}
			if (type == null)
			{
				this.ProcessError(SR.GetString("Non_existent_base_type", new object[] { baseTypeName, src }));
				return;
			}
			if (!this.DefaultBaseType.IsAssignableFrom(type))
			{
				this.ProcessError(SR.GetString("Invalid_type_to_inherit_from", new object[]
				{
					baseTypeName,
					this._baseType.FullName
				}));
				return;
			}
			if (this._pageParserFilter != null && !this._pageParserFilter.AllowBaseType(type))
			{
				throw new HttpException(SR.GetString("Base_type_not_allowed", new object[] { type.FullName }));
			}
			this._baseType = type;
			this.EnsureRootBuilderCreated();
			this.AddTypeDependency(this._baseType);
			this.flags[128] = true;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000C460C File Offset: 0x000C360C
		private void ProcessImportDirective(string directiveName, IDictionary directive)
		{
			string andRemoveNonEmptyNoSpaceAttribute = Util.GetAndRemoveNonEmptyNoSpaceAttribute(directive, "namespace");
			if (andRemoveNonEmptyNoSpaceAttribute == null)
			{
				this.ProcessError(SR.GetString("Missing_attr", new object[] { "namespace" }));
			}
			else
			{
				this.AddImportEntry(andRemoveNonEmptyNoSpaceAttribute);
			}
			Util.CheckUnknownDirectiveAttributes(directiveName, directive);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000C4658 File Offset: 0x000C3658
		private void ProcessLanguageAttribute(string language)
		{
			if (language == null)
			{
				return;
			}
			if (this.FInDesigner)
			{
				return;
			}
			CompilerType compilerInfoFromLanguage = CompilationUtil.GetCompilerInfoFromLanguage(base.CurrentVirtualPath, language);
			if (this._compilerType != null && this._compilerType.CodeDomProviderType != compilerInfoFromLanguage.CodeDomProviderType)
			{
				this.ProcessError(SR.GetString("Mixed_lang_not_supported", new object[] { language }));
				return;
			}
			this._compilerType = compilerInfoFromLanguage;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000C46C0 File Offset: 0x000C36C0
		private void ProcessCodeFile(VirtualPath codeFileVirtualPath)
		{
			this._codeFileVirtualPath = base.ResolveVirtualPath(codeFileVirtualPath);
			CompilerType compilerInfoFromVirtualPath = CompilationUtil.GetCompilerInfoFromVirtualPath(this._codeFileVirtualPath);
			if (this._compilerType != null && this._compilerType.CodeDomProviderType != compilerInfoFromVirtualPath.CodeDomProviderType)
			{
				this.ProcessError(SR.GetString("Inconsistent_CodeFile_Language"));
				return;
			}
			BuildManager.ValidateCodeFileVirtualPath(this._codeFileVirtualPath);
			Util.CheckVirtualFileExists(this._codeFileVirtualPath);
			this._compilerType = compilerInfoFromVirtualPath;
			this.AddSourceDependency(this._codeFileVirtualPath);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000C473C File Offset: 0x000C373C
		private Assembly ImportSourceFile(VirtualPath virtualPath)
		{
			if (this.CompilationMode == CompilationMode.Never)
			{
				return null;
			}
			virtualPath = base.ResolveVirtualPath(virtualPath);
			if (this._pageParserFilter != null && !this._pageParserFilter.AllowVirtualReference(this.CompConfig, virtualPath))
			{
				this.ProcessError(SR.GetString("Reference_not_allowed", new object[] { virtualPath }));
			}
			this.AddSourceDependency(virtualPath);
			BuildResultCompiledAssembly buildResultCompiledAssembly = BuildManager.GetVPathBuildResult(virtualPath) as BuildResultCompiledAssembly;
			if (buildResultCompiledAssembly == null)
			{
				this.ProcessError(SR.GetString("Not_a_src_file", new object[] { virtualPath }));
			}
			Assembly resultAssembly = buildResultCompiledAssembly.ResultAssembly;
			this.AddAssemblyDependency(resultAssembly, true);
			return resultAssembly;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000C47D8 File Offset: 0x000C37D8
		private void DetectSpecialServerTagError(string text, int textPos)
		{
			if (this.IgnoreParseErrors)
			{
				return;
			}
			if (text.Length > textPos + 1 && text[textPos + 1] == '%')
			{
				this.ProcessError(SR.GetString("Malformed_server_block"));
				return;
			}
			Match match = BaseParser.gtRegex.Match(text, textPos);
			if (!match.Success)
			{
				return;
			}
			string text2 = text.Substring(textPos, match.Index - textPos + 2);
			match = BaseParser.runatServerRegex.Match(text2);
			if (!match.Success)
			{
				return;
			}
			Match match2 = BaseParser.ltRegex.Match(text2, 1);
			if (match2.Success && match2.Index < match.Index)
			{
				return;
			}
			string text3 = BaseParser.serverTagsRegex.Replace(text2, string.Empty);
			if (text3 != text2 && BaseParser.TagRegex.Match(text3).Success)
			{
				this.ProcessError(SR.GetString("Server_tags_cant_contain_percent_constructs"));
				return;
			}
			this.ProcessError(SR.GetString("Malformed_server_tag"));
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000C48C0 File Offset: 0x000C38C0
		internal void AddImportEntry(string ns)
		{
			if (this._namespaceEntries != null)
			{
				this._namespaceEntries = (Hashtable)this._namespaceEntries.Clone();
			}
			else
			{
				this._namespaceEntries = new Hashtable();
			}
			NamespaceEntry namespaceEntry = new NamespaceEntry();
			namespaceEntry.Namespace = ns;
			namespaceEntry.Line = this._lineNumber;
			namespaceEntry.VirtualPath = base.CurrentVirtualPathString;
			this._namespaceEntries[ns] = namespaceEntry;
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000C492C File Offset: 0x000C392C
		internal Assembly LoadAssembly(string assemblyName, bool throwOnFail)
		{
			if (this._typeResolutionService != null)
			{
				AssemblyName assemblyName2 = new AssemblyName(assemblyName);
				return this._typeResolutionService.GetAssembly(assemblyName2, throwOnFail);
			}
			return this._compConfig.LoadAssembly(assemblyName, throwOnFail);
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000C4963 File Offset: 0x000C3963
		internal Type GetType(string typeName, bool ignoreCase)
		{
			return this.GetType(typeName, ignoreCase, true);
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000C4970 File Offset: 0x000C3970
		internal Type GetType(string typeName, bool ignoreCase, bool throwOnError)
		{
			Assembly assembly = null;
			int num = Util.CommaIndexInTypeName(typeName);
			if (num > 0)
			{
				string text = typeName.Substring(num + 1).Trim();
				typeName = typeName.Substring(0, num).Trim();
				try
				{
					assembly = this.LoadAssembly(text, !this.FInDesigner);
				}
				catch
				{
					throw new HttpException(SR.GetString("Assembly_not_compiled", new object[] { text }));
				}
			}
			if (assembly != null)
			{
				return assembly.GetType(typeName, throwOnError, ignoreCase);
			}
			Type type = Util.GetTypeFromAssemblies(this._referencedAssemblies, typeName, ignoreCase);
			if (type != null)
			{
				return type;
			}
			type = Util.GetTypeFromAssemblies(this.AssemblyDependencies, typeName, ignoreCase);
			if (type != null)
			{
				return type;
			}
			if (throwOnError)
			{
				throw new HttpException(SR.GetString("Invalid_type", new object[] { typeName }));
			}
			return null;
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000C4A44 File Offset: 0x000C3A44
		internal Type GetType(string typeName)
		{
			return this.GetType(typeName, false);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000C4A50 File Offset: 0x000C3A50
		private void ProcessServerInclude(Match match)
		{
			if (this.flags[2])
			{
				throw new HttpException(SR.GetString("Include_not_allowed_in_server_script_tag"));
			}
			this.ProcessLiteral();
			string value = match.Groups["pathtype"].Value;
			string value2 = match.Groups["filename"].Value;
			if (value2.Length == 0)
			{
				this.ProcessError(SR.GetString("Empty_file_name"));
				return;
			}
			VirtualPath virtualPath = base.CurrentVirtualPath;
			string text = null;
			if (StringUtil.EqualsIgnoreCase(value, "file"))
			{
				if (UrlPath.IsAbsolutePhysicalPath(value2))
				{
					text = value2;
				}
				else
				{
					bool flag = true;
					try
					{
						virtualPath = base.ResolveVirtualPath(VirtualPath.Create(value2));
					}
					catch
					{
						flag = false;
					}
					if (flag)
					{
						HttpRuntime.CheckVirtualFilePermission(virtualPath.VirtualPathString);
						this.AddSourceDependency(virtualPath);
					}
					else
					{
						string directoryName = Path.GetDirectoryName(base.CurrentVirtualPath.MapPath());
						text = Path.GetFullPath(Path.Combine(directoryName, value2.Replace('/', '\\')));
					}
				}
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(value, "virtual"))
				{
					this.ProcessError(SR.GetString("Only_file_virtual_supported_on_server_include"));
					return;
				}
				virtualPath = base.ResolveVirtualPath(VirtualPath.Create(value2));
				HttpRuntime.CheckVirtualFilePermission(virtualPath.VirtualPathString);
				this.AddSourceDependency(virtualPath);
			}
			if (text != null)
			{
				HttpRuntime.CheckFilePermission(text);
			}
			if (this._pageParserFilter != null && !this._pageParserFilter.AllowServerSideInclude(virtualPath.VirtualPathString))
			{
				this.ProcessError(SR.GetString("Include_not_allowed", new object[] { virtualPath }));
			}
			this.ParseFile(text, virtualPath);
			this.flags[8] = true;
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000C4BF0 File Offset: 0x000C3BF0
		private void ProcessCodeBlock(Match match, CodeBlockType blockType, string text)
		{
			this.ProcessLiteral();
			Group group = match.Groups["code"];
			string text2 = group.Value;
			text2 = text2.Replace("%\\>", "%>");
			int num = this._lineNumber;
			int num2 = -1;
			if (blockType != CodeBlockType.Code)
			{
				int num3 = -1;
				int num4 = 0;
				while (num4 < text2.Length && char.IsWhiteSpace(text2[num4]))
				{
					if (text2[num4] == '\r' || (text2[num4] == '\n' && (num4 == 0 || text2[num4 - 1] != '\r')))
					{
						num++;
						num3 = num4;
					}
					else if (text2[num4] == '\n')
					{
						num3 = num4;
					}
					num4++;
				}
				if (num3 >= 0)
				{
					text2 = text2.Substring(num3 + 1);
					num2 = 1;
				}
				num3 = -1;
				int num5 = text2.Length - 1;
				while (num5 >= 0 && char.IsWhiteSpace(text2[num5]))
				{
					if (text2[num5] == '\r' || text2[num5] == '\n')
					{
						num3 = num5;
					}
					num5--;
				}
				if (num3 >= 0)
				{
					text2 = text2.Substring(0, num3);
				}
				if (!this.IgnoreParseErrors && Util.IsWhiteSpaceString(text2))
				{
					this.ProcessError(SR.GetString("Empty_expression"));
					return;
				}
			}
			if (num2 < 0)
			{
				int num6 = text.LastIndexOfAny(TemplateParser.s_newlineChars, group.Index - 1);
				num2 = group.Index - num6;
			}
			ControlBuilder builder = ((BuilderStackEntry)this.BuilderStack.Peek())._builder;
			if (!this.PageParserFilterProcessedCodeBlock(TemplateParser.CodeConstructTypeFromCodeBlockType(blockType), text2, num))
			{
				this.EnsureCodeAllowed();
				ControlBuilder controlBuilder = new CodeBlockBuilder(blockType, text2, num, num2, base.CurrentVirtualPath);
				this.AppendSubBuilder(builder, controlBuilder);
			}
			if (blockType == CodeBlockType.Code)
			{
				this.flags[8] = true;
			}
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000C4DA8 File Offset: 0x000C3DA8
		private static CodeConstructType CodeConstructTypeFromCodeBlockType(CodeBlockType blockType)
		{
			switch (blockType)
			{
			case CodeBlockType.Code:
				return CodeConstructType.CodeSnippet;
			case CodeBlockType.Expression:
				return CodeConstructType.ExpressionSnippet;
			case CodeBlockType.DataBinding:
				return CodeConstructType.DataBindingSnippet;
			default:
				return CodeConstructType.CodeSnippet;
			}
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000C4DD4 File Offset: 0x000C3DD4
		private bool PageParserFilterProcessedCodeBlock(CodeConstructType codeConstructType, string code, int lineNumber)
		{
			if (this._pageParserFilter == null || this.CompilationMode == CompilationMode.Never)
			{
				return false;
			}
			int lineNumber2 = this._lineNumber;
			this._lineNumber = lineNumber;
			bool flag;
			try
			{
				flag = this._pageParserFilter.ProcessCodeConstruct(codeConstructType, code);
			}
			finally
			{
				this._lineNumber = lineNumber2;
			}
			return flag;
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000C4E2C File Offset: 0x000C3E2C
		internal bool PageParserFilterProcessedDataBindingAttribute(string controlId, string attributeName, string code)
		{
			return this._pageParserFilter != null && this.CompilationMode != CompilationMode.Never && this._pageParserFilter.ProcessDataBindingAttribute(controlId, attributeName, code);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000C4E4F File Offset: 0x000C3E4F
		internal bool PageParserFilterProcessedEventHookupAttribute(string controlId, string eventName, string handlerName)
		{
			return this._pageParserFilter != null && this.CompilationMode != CompilationMode.Never && this._pageParserFilter.ProcessEventHookup(controlId, eventName, handlerName);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000C4E74 File Offset: 0x000C3E74
		internal void AddControl(Type type, IDictionary attributes)
		{
			ControlBuilder builder = ((BuilderStackEntry)this.BuilderStack.Peek())._builder;
			ControlBuilder controlBuilder = ControlBuilder.CreateBuilderFromType(this, builder, type, null, null, attributes, this._lineNumber, base.CurrentVirtualPath.VirtualPathString);
			this.AppendSubBuilder(builder, controlBuilder);
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000C4EBC File Offset: 0x000C3EBC
		private string ProcessAttributes(Match match, out ParsedAttributeCollection attribs, bool fDirective, out string duplicateAttribute)
		{
			string text = string.Empty;
			attribs = TemplateParser.CreateEmptyAttributeBag();
			CaptureCollection captures = match.Groups["attrname"].Captures;
			CaptureCollection captures2 = match.Groups["attrval"].Captures;
			CaptureCollection captureCollection = null;
			if (fDirective)
			{
				captureCollection = match.Groups["equal"].Captures;
			}
			this.flags[1] = false;
			this._id = null;
			duplicateAttribute = null;
			for (int i = 0; i < captures.Count; i++)
			{
				string text2 = captures[i].ToString();
				if (fDirective)
				{
					text2 = text2.ToLower(CultureInfo.InvariantCulture);
				}
				string text3 = captures2[i].ToString();
				string empty = string.Empty;
				string text4 = Util.ParsePropertyDeviceFilter(text2, out empty);
				text3 = HttpUtility.HtmlDecode(text3);
				bool flag = false;
				if (fDirective)
				{
					flag = captureCollection[i].ToString().Length > 0;
				}
				if (StringUtil.EqualsIgnoreCase(empty, "id"))
				{
					this._id = text3;
				}
				else if (StringUtil.EqualsIgnoreCase(empty, "runat"))
				{
					this.ValidateBuiltInAttribute(text4, empty, text3);
					if (!StringUtil.EqualsIgnoreCase(text3, "server"))
					{
						this.ProcessError(SR.GetString("Runat_can_only_be_server"));
					}
					this.flags[1] = true;
					text2 = null;
				}
				else if (this.FInDesigner && StringUtil.EqualsIgnoreCase(empty, "ignoreParentFrozen"))
				{
					text2 = null;
				}
				if (text2 != null)
				{
					if (fDirective && !flag && i == 0)
					{
						text = text2;
						if (string.Compare(text, this.DefaultDirectiveName, StringComparison.OrdinalIgnoreCase) == 0)
						{
							text = string.Empty;
						}
					}
					else
					{
						try
						{
							if (fDirective && text.Length > 0 && text4.Length > 0)
							{
								this.ProcessError(SR.GetString("Device_unsupported_in_directive", new object[] { text }));
							}
							else
							{
								attribs.AddFilteredAttribute(text4, empty, text3);
							}
						}
						catch (ArgumentException)
						{
							duplicateAttribute = text2;
						}
						catch (Exception ex)
						{
							this.ProcessException(ex);
						}
					}
				}
			}
			if (duplicateAttribute != null && fDirective)
			{
				this.ProcessError(SR.GetString("Duplicate_attr_in_directive", new object[] { duplicateAttribute }));
			}
			return text;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000C50FC File Offset: 0x000C40FC
		private static ParsedAttributeCollection CreateEmptyAttributeBag()
		{
			return new ParsedAttributeCollection();
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000C5104 File Offset: 0x000C4104
		private bool MaybeTerminateControl(string tagName, int textPos)
		{
			BuilderStackEntry builderStackEntry = (BuilderStackEntry)this.BuilderStack.Peek();
			ControlBuilder builder = builderStackEntry._builder;
			if (builderStackEntry._tagName == null || !StringUtil.EqualsIgnoreCase(builderStackEntry._tagName, tagName))
			{
				return false;
			}
			if (builderStackEntry._repeatCount > 0)
			{
				builderStackEntry._repeatCount--;
				return false;
			}
			this.ProcessLiteral();
			if (builder.NeedsTagInnerText())
			{
				try
				{
					builder.SetTagInnerText(builderStackEntry._inputText.Substring(builderStackEntry._textPos, textPos - builderStackEntry._textPos));
				}
				catch (Exception ex)
				{
					if (!this.IgnoreParseErrors)
					{
						this._lineNumber = builder.Line;
						this.ProcessException(ex);
						return true;
					}
				}
			}
			if (builder is TemplateBuilder && ((TemplateBuilder)builder).AllowMultipleInstances)
			{
				this._idList = (StringSet)this._idListStack.Pop();
			}
			this._builderStack.Pop();
			this.AppendSubBuilder(((BuilderStackEntry)this._builderStack.Peek())._builder, builder);
			builder.CloseControl();
			return true;
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000C5218 File Offset: 0x000C4218
		internal Type MapStringToType(string typeName, IDictionary attribs)
		{
			return this.RootBuilder.GetChildControlType(typeName, attribs);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000C5227 File Offset: 0x000C4227
		internal void AddSourceDependency(VirtualPath fileName)
		{
			if (this._pageParserFilter != null)
			{
				this._pageParserFilter.OnDependencyAdded();
				this._pageParserFilter.OnDirectDependencyAdded();
			}
			this.AddSourceDependency2(fileName);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000C524E File Offset: 0x000C424E
		private void AddSourceDependency2(VirtualPath fileName)
		{
			if (this._sourceDependencies == null)
			{
				this._sourceDependencies = new CaseInsensitiveStringSet();
			}
			this._sourceDependencies.Add(fileName.VirtualPathString);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000C5274 File Offset: 0x000C4274
		internal void AddBuildResultDependency(BuildResult result)
		{
			if (this._pageParserFilter != null)
			{
				this._pageParserFilter.OnDirectDependencyAdded();
			}
			if (result.VirtualPathDependencies == null)
			{
				return;
			}
			foreach (object obj in result.VirtualPathDependencies)
			{
				string text = (string)obj;
				if (this._pageParserFilter != null)
				{
					this._pageParserFilter.OnDependencyAdded();
				}
				this.AddSourceDependency2(VirtualPath.Create(text));
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000C5304 File Offset: 0x000C4304
		internal void AddTypeDependency(Type type)
		{
			this.AddBaseTypeDependencies(type);
			if (type.Namespace != null && BaseCodeDomTreeGenerator.IsAspNetNamespace(type.Namespace))
			{
				this.AddImportEntry(type.Namespace);
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000C5330 File Offset: 0x000C4330
		private void AddBaseTypeDependencies(Type type)
		{
			Assembly assembly = type.Module.Assembly;
			if (assembly == typeof(string).Assembly || assembly == typeof(Page).Assembly || assembly == typeof(Uri).Assembly)
			{
				return;
			}
			this.AddAssemblyDependency(assembly);
			if (type.BaseType != null)
			{
				this.AddBaseTypeDependencies(type.BaseType);
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				this.AddBaseTypeDependencies(type2);
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000C53C4 File Offset: 0x000C43C4
		internal Assembly AddAssemblyDependency(string assemblyName, bool addDependentAssemblies)
		{
			Assembly assembly = this.LoadAssembly(assemblyName, !this.FInDesigner);
			if (assembly != null)
			{
				this.AddAssemblyDependency(assembly, addDependentAssemblies);
			}
			return assembly;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000C53EE File Offset: 0x000C43EE
		internal Assembly AddAssemblyDependency(string assemblyName)
		{
			return this.AddAssemblyDependency(assemblyName, false);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000C53F8 File Offset: 0x000C43F8
		internal void AddAssemblyDependency(Assembly assembly, bool addDependentAssemblies)
		{
			if (this._assemblyDependencies == null)
			{
				this._assemblyDependencies = new AssemblySet();
			}
			if (this._typeResolutionService != null)
			{
				this._typeResolutionService.ReferenceAssembly(assembly.GetName());
			}
			this._assemblyDependencies.Add(assembly);
			if (addDependentAssemblies)
			{
				AssemblySet referencedAssemblies = Util.GetReferencedAssemblies(assembly);
				this.AddAssemblyDependencies(referencedAssemblies);
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000C544E File Offset: 0x000C444E
		internal void AddAssemblyDependency(Assembly assembly)
		{
			this.AddAssemblyDependency(assembly, false);
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000C5458 File Offset: 0x000C4458
		private void AddAssemblyDependencies(AssemblySet assemblyDependencies)
		{
			if (assemblyDependencies == null)
			{
				return;
			}
			foreach (object obj in ((IEnumerable)assemblyDependencies))
			{
				Assembly assembly = (Assembly)obj;
				this.AddAssemblyDependency(assembly);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002C1D RID: 11293 RVA: 0x000C54B0 File Offset: 0x000C44B0
		ICollection IAssemblyDependencyParser.AssemblyDependencies
		{
			get
			{
				return this.AssemblyDependencies;
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000C54B8 File Offset: 0x000C44B8
		internal IImplicitResourceProvider GetImplicitResourceProvider()
		{
			if (this.FInDesigner)
			{
				return null;
			}
			if (this.flags[262144])
			{
				return this._implicitResourceProvider;
			}
			this.flags[262144] = true;
			IResourceProvider localResourceProvider = ResourceExpressionBuilder.GetLocalResourceProvider(this._rootBuilder.VirtualPath);
			if (localResourceProvider == null)
			{
				return null;
			}
			this._implicitResourceProvider = localResourceProvider as IImplicitResourceProvider;
			if (this._implicitResourceProvider == null)
			{
				this._implicitResourceProvider = new DefaultImplicitResourceProvider(localResourceProvider);
			}
			return this._implicitResourceProvider;
		}

		// Token: 0x04002034 RID: 8244
		internal const string CodeFileBaseClassAttributeName = "codefilebaseclass";

		// Token: 0x04002035 RID: 8245
		private const int isServerTag = 1;

		// Token: 0x04002036 RID: 8246
		private const int inScriptTag = 2;

		// Token: 0x04002037 RID: 8247
		private const int ignoreScriptTag = 4;

		// Token: 0x04002038 RID: 8248
		private const int ignoreNextSpaceString = 8;

		// Token: 0x04002039 RID: 8249
		internal const int requiresCompilation = 16;

		// Token: 0x0400203A RID: 8250
		private const int ignoreControlProperties = 32;

		// Token: 0x0400203B RID: 8251
		internal const int aspCompatMode = 64;

		// Token: 0x0400203C RID: 8252
		private const int hasCodeBehind = 128;

		// Token: 0x0400203D RID: 8253
		private const int inDesigner = 256;

		// Token: 0x0400203E RID: 8254
		private const int ignoreParseErrors = 512;

		// Token: 0x0400203F RID: 8255
		private const int mainDirectiveSpecified = 1024;

		// Token: 0x04002040 RID: 8256
		private const int mainDirectiveHandled = 2048;

		// Token: 0x04002041 RID: 8257
		private const int useExplicit = 4096;

		// Token: 0x04002042 RID: 8258
		private const int hasDebugAttribute = 8192;

		// Token: 0x04002043 RID: 8259
		private const int debug = 16384;

		// Token: 0x04002044 RID: 8260
		private const int noLinePragmas = 32768;

		// Token: 0x04002045 RID: 8261
		private const int strict = 65536;

		// Token: 0x04002046 RID: 8262
		internal const int noAutoEventWireup = 131072;

		// Token: 0x04002047 RID: 8263
		private const int attemptedImplicitResources = 262144;

		// Token: 0x04002048 RID: 8264
		internal const int buffer = 524288;

		// Token: 0x04002049 RID: 8265
		internal const int requiresSessionState = 1048576;

		// Token: 0x0400204A RID: 8266
		internal const int readOnlySessionState = 2097152;

		// Token: 0x0400204B RID: 8267
		internal const int validateRequest = 4194304;

		// Token: 0x0400204C RID: 8268
		internal const int asyncMode = 8388608;

		// Token: 0x0400204D RID: 8269
		private const int throwOnFirstParseError = 16777216;

		// Token: 0x0400204E RID: 8270
		private const int ignoreParserFilter = 33554432;

		// Token: 0x0400204F RID: 8271
		private CompilationSection _compConfig;

		// Token: 0x04002050 RID: 8272
		private PagesSection _pagesConfig;

		// Token: 0x04002051 RID: 8273
		internal SimpleBitVector32 flags;

		// Token: 0x04002052 RID: 8274
		private MainTagNameToTypeMapper _typeMapper;

		// Token: 0x04002053 RID: 8275
		private Stack _builderStack;

		// Token: 0x04002054 RID: 8276
		private string _id;

		// Token: 0x04002055 RID: 8277
		private StringSet _idList;

		// Token: 0x04002056 RID: 8278
		private Stack _idListStack;

		// Token: 0x04002057 RID: 8279
		private ScriptBlockData _currentScript;

		// Token: 0x04002058 RID: 8280
		private StringBuilder _literalBuilder;

		// Token: 0x04002059 RID: 8281
		internal int _lineNumber;

		// Token: 0x0400205A RID: 8282
		private int _scriptStartLineNumber;

		// Token: 0x0400205B RID: 8283
		private string _text;

		// Token: 0x0400205C RID: 8284
		private Type _baseType;

		// Token: 0x0400205D RID: 8285
		private string _baseTypeNamespace;

		// Token: 0x0400205E RID: 8286
		private string _baseTypeName;

		// Token: 0x0400205F RID: 8287
		private ArrayList _implementedInterfaces;

		// Token: 0x04002060 RID: 8288
		internal PageParserFilter _pageParserFilter;

		// Token: 0x04002061 RID: 8289
		private IImplicitResourceProvider _implicitResourceProvider;

		// Token: 0x04002062 RID: 8290
		private CompilationMode _compilationMode;

		// Token: 0x04002063 RID: 8291
		private ParserErrorCollection _parserErrors;

		// Token: 0x04002064 RID: 8292
		private IDesignerHost _designerHost;

		// Token: 0x04002065 RID: 8293
		private ITypeResolutionService _typeResolutionService;

		// Token: 0x04002066 RID: 8294
		private EventHandler _designTimeDataBindHandler;

		// Token: 0x04002067 RID: 8295
		private StringSet _circularReferenceChecker;

		// Token: 0x04002068 RID: 8296
		private ICollection _referencedAssemblies;

		// Token: 0x04002069 RID: 8297
		private AssemblySet _assemblyDependencies;

		// Token: 0x0400206A RID: 8298
		private StringSet _sourceDependencies;

		// Token: 0x0400206B RID: 8299
		internal HttpStaticObjectsCollection _sessionObjects;

		// Token: 0x0400206C RID: 8300
		internal HttpStaticObjectsCollection _applicationObjects;

		// Token: 0x0400206D RID: 8301
		private RootBuilder _rootBuilder;

		// Token: 0x0400206E RID: 8302
		internal IDictionary _mainDirectiveConfigSettings;

		// Token: 0x0400206F RID: 8303
		private Hashtable _namespaceEntries;

		// Token: 0x04002070 RID: 8304
		private CompilerType _compilerType;

		// Token: 0x04002071 RID: 8305
		private ArrayList _scriptList;

		// Token: 0x04002072 RID: 8306
		private HashCodeCombiner _typeHashCode = new HashCodeCombiner();

		// Token: 0x04002073 RID: 8307
		private ArrayList _pageObjectList;

		// Token: 0x04002074 RID: 8308
		private int _warningLevel = -1;

		// Token: 0x04002075 RID: 8309
		private string _compilerOptions;

		// Token: 0x04002076 RID: 8310
		private VirtualPath _codeFileVirtualPath;

		// Token: 0x04002077 RID: 8311
		private string _generatedClassName;

		// Token: 0x04002078 RID: 8312
		private string _generatedNamespace;

		// Token: 0x04002079 RID: 8313
		private static volatile Type _controlBuilderInterceptorType;

		// Token: 0x0400207A RID: 8314
		private ControlBuilderInterceptor _controlBuilderInterceptor;

		// Token: 0x0400207B RID: 8315
		private int _controlCount;

		// Token: 0x0400207C RID: 8316
		private static char[] s_newlineChars = new char[] { '\r', '\n' };
	}
}
