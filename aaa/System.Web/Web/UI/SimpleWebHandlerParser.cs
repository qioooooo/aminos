using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.RegularExpressions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000460 RID: 1120
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class SimpleWebHandlerParser : IAssemblyDependencyParser
	{
		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x000E55DC File Offset: 0x000E45DC
		internal string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x000E55E4 File Offset: 0x000E45E4
		private static Regex DirectiveRegex
		{
			get
			{
				if (SimpleWebHandlerParser._directiveRegex == null)
				{
					if (AppSettings.UseStrictParserRegex)
					{
						SimpleWebHandlerParser._directiveRegex = (Regex)BaseParser.CreateInstanceNoException("System.Web.RegularExpressions.SimpleDirectiveRegex40");
					}
					if (SimpleWebHandlerParser._directiveRegex == null)
					{
						SimpleWebHandlerParser._directiveRegex = new SimpleDirectiveRegex();
					}
				}
				return SimpleWebHandlerParser._directiveRegex;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06003500 RID: 13568 RVA: 0x000E561E File Offset: 0x000E461E
		// (set) Token: 0x06003501 RID: 13569 RVA: 0x000E5626 File Offset: 0x000E4626
		internal bool IgnoreParseErrors
		{
			get
			{
				return this._ignoreParseErrors;
			}
			set
			{
				this._ignoreParseErrors = value;
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000E562F File Offset: 0x000E462F
		internal void SetBuildProvider(SimpleHandlerBuildProvider buildProvider)
		{
			this._buildProvider = buildProvider;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000E5638 File Offset: 0x000E4638
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected SimpleWebHandlerParser(HttpContext context, string virtualPath, string physicalPath)
		{
			this._virtualPath = VirtualPath.Create(virtualPath);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000E564C File Offset: 0x000E464C
		protected Type GetCompiledTypeFromCache()
		{
			BuildResultCompiledType buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetVPathBuildResult(this._virtualPath);
			return buildResultCompiledType.ResultType;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000E5670 File Offset: 0x000E4670
		internal void Parse(ICollection referencedAssemblies)
		{
			this._referencedAssemblies = referencedAssemblies;
			this.AddSourceDependency(this._virtualPath);
			using (this._reader = this._buildProvider.OpenReaderInternal())
			{
				this.ParseReader();
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x000E56C8 File Offset: 0x000E46C8
		internal CompilerType CompilerType
		{
			get
			{
				return this._compilerType;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06003507 RID: 13575 RVA: 0x000E56D0 File Offset: 0x000E46D0
		internal ICollection AssemblyDependencies
		{
			get
			{
				return this._linkedAssemblies;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06003508 RID: 13576 RVA: 0x000E56D8 File Offset: 0x000E46D8
		internal ICollection SourceDependencies
		{
			get
			{
				return this._sourceDependencies;
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000E56E0 File Offset: 0x000E46E0
		internal CodeCompileUnit GetCodeModel()
		{
			if (this._sourceString == null)
			{
				return null;
			}
			return new CodeSnippetCompileUnit(this._sourceString)
			{
				LinePragma = BaseCodeDomTreeGenerator.CreateCodeLinePragmaHelper(this._virtualPath.VirtualPathString, this._lineNumber)
			};
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000E5720 File Offset: 0x000E4720
		internal IDictionary GetLinePragmasTable()
		{
			LinePragmaCodeInfo linePragmaCodeInfo = new LinePragmaCodeInfo();
			linePragmaCodeInfo._startLine = this._lineNumber;
			linePragmaCodeInfo._startColumn = this._startColumn;
			linePragmaCodeInfo._startGeneratedColumn = 1;
			linePragmaCodeInfo._codeLength = -1;
			linePragmaCodeInfo._isCodeNugget = false;
			IDictionary dictionary = new Hashtable();
			dictionary[this._lineNumber] = linePragmaCodeInfo;
			return dictionary;
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x000E5779 File Offset: 0x000E4779
		internal bool HasInlineCode
		{
			get
			{
				return this._sourceString != null;
			}
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000E5788 File Offset: 0x000E4788
		internal Type GetTypeToCache(Assembly builtAssembly)
		{
			Type type = null;
			if (builtAssembly != null)
			{
				type = builtAssembly.GetType(this._typeName);
			}
			if (type == null)
			{
				type = this.GetType(this._typeName);
			}
			try
			{
				this.ValidateBaseType(type);
			}
			catch (Exception ex)
			{
				throw new HttpParseException(ex.Message, ex, this._virtualPath, this._sourceString, this._lineNumber);
			}
			return type;
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000E57F4 File Offset: 0x000E47F4
		internal virtual void ValidateBaseType(Type t)
		{
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000E57F8 File Offset: 0x000E47F8
		private void ParseReader()
		{
			string text = this._reader.ReadToEnd();
			try
			{
				this.ParseString(text);
			}
			catch (Exception ex)
			{
				throw new HttpParseException(ex.Message, ex, this._virtualPath, text, this._lineNumber);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000E5848 File Offset: 0x000E4848
		private void ParseString(string text)
		{
			int num = 0;
			this._lineNumber = 1;
			for (;;)
			{
				Match match = SimpleWebHandlerParser.DirectiveRegex.Match(text, num);
				if (!match.Success)
				{
					break;
				}
				this._lineNumber += Util.LineCount(text, num, match.Index);
				num = match.Index;
				IDictionary dictionary = CollectionsUtil.CreateCaseInsensitiveSortedList();
				string text2 = this.ProcessAttributes(match, dictionary);
				this.ProcessDirective(text2, dictionary);
				this._lineNumber += Util.LineCount(text, num, match.Index + match.Length);
				num = match.Index + match.Length;
				int num2 = text.LastIndexOfAny(SimpleWebHandlerParser.s_newlineChars, num - 1);
				this._startColumn = num - num2;
			}
			if (!this._fFoundMainDirective && !this.IgnoreParseErrors)
			{
				throw new HttpException(SR.GetString("Missing_directive", new object[] { this.DefaultDirectiveName }));
			}
			string text3 = text.Substring(num);
			if (!Util.IsWhiteSpaceString(text3))
			{
				this._sourceString = text3;
			}
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000E594C File Offset: 0x000E494C
		private string ProcessAttributes(Match match, IDictionary attribs)
		{
			string text = string.Empty;
			CaptureCollection captures = match.Groups["attrname"].Captures;
			CaptureCollection captures2 = match.Groups["attrval"].Captures;
			CaptureCollection captureCollection = null;
			captureCollection = match.Groups["equal"].Captures;
			for (int i = 0; i < captures.Count; i++)
			{
				string text2 = captures[i].ToString();
				string text3 = captures2[i].ToString();
				bool flag = captureCollection[i].ToString().Length > 0;
				if (text2 != null)
				{
					if (!flag && i == 0)
					{
						text = text2;
					}
					else
					{
						try
						{
							if (attribs != null)
							{
								attribs.Add(text2, text3);
							}
						}
						catch (ArgumentException)
						{
							if (!this.IgnoreParseErrors)
							{
								throw new HttpException(SR.GetString("Duplicate_attr_in_tag", new object[] { text2 }));
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003511 RID: 13585
		protected abstract string DefaultDirectiveName { get; }

		// Token: 0x06003512 RID: 13586 RVA: 0x000E5A50 File Offset: 0x000E4A50
		private static void ProcessCompilationParams(IDictionary directive, CompilerParameters compilParams)
		{
			bool flag = false;
			if (Util.GetAndRemoveBooleanAttribute(directive, "debug", ref flag))
			{
				compilParams.IncludeDebugInformation = flag;
			}
			if (compilParams.IncludeDebugInformation && !HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
			{
				throw new HttpException(SR.GetString("Insufficient_trust_for_attribute", new object[] { "debug" }));
			}
			int num = 0;
			if (Util.GetAndRemoveNonNegativeIntegerAttribute(directive, "warninglevel", ref num))
			{
				compilParams.WarningLevel = num;
				if (num > 0)
				{
					compilParams.TreatWarningsAsErrors = true;
				}
			}
			string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "compileroptions");
			if (andRemoveNonEmptyAttribute != null)
			{
				CompilationUtil.CheckCompilerOptionsAllowed(andRemoveNonEmptyAttribute, false, null, 0);
				compilParams.CompilerOptions = andRemoveNonEmptyAttribute;
			}
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000E5AEC File Offset: 0x000E4AEC
		internal virtual void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (directiveName.Length == 0)
			{
				directiveName = this.DefaultDirectiveName;
			}
			if (this.IsMainDirective(directiveName))
			{
				if (this._fFoundMainDirective && !this.IgnoreParseErrors)
				{
					throw new HttpException(SR.GetString("Only_one_directive_allowed", new object[] { this.DefaultDirectiveName }));
				}
				this._fFoundMainDirective = true;
				directive.Remove("description");
				directive.Remove("codebehind");
				string andRemoveNonEmptyAttribute = Util.GetAndRemoveNonEmptyAttribute(directive, "language");
				if (andRemoveNonEmptyAttribute != null)
				{
					this._compilerType = this._buildProvider.GetDefaultCompilerTypeForLanguageInternal(andRemoveNonEmptyAttribute);
				}
				else
				{
					this._compilerType = this._buildProvider.GetDefaultCompilerTypeInternal();
				}
				this._typeName = Util.GetAndRemoveRequiredAttribute(directive, "class");
				if (this._compilerType.CompilerParameters != null)
				{
					SimpleWebHandlerParser.ProcessCompilationParams(directive, this._compilerType.CompilerParameters);
				}
			}
			else if (StringUtil.EqualsIgnoreCase(directiveName, "assembly"))
			{
				string andRemoveNonEmptyAttribute2 = Util.GetAndRemoveNonEmptyAttribute(directive, "name");
				VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "src");
				if (andRemoveNonEmptyAttribute2 != null && andRemoveVirtualPathAttribute != null && !this.IgnoreParseErrors)
				{
					throw new HttpException(SR.GetString("Attributes_mutually_exclusive", new object[] { "Name", "Src" }));
				}
				if (andRemoveNonEmptyAttribute2 != null)
				{
					this.AddAssemblyDependency(andRemoveNonEmptyAttribute2);
				}
				else if (andRemoveVirtualPathAttribute != null)
				{
					this.ImportSourceFile(andRemoveVirtualPathAttribute);
				}
				else if (!this.IgnoreParseErrors)
				{
					throw new HttpException(SR.GetString("Missing_attr", new object[] { "name" }));
				}
			}
			else if (!this.IgnoreParseErrors)
			{
				throw new HttpException(SR.GetString("Unknown_directive", new object[] { directiveName }));
			}
			Util.CheckUnknownDirectiveAttributes(directiveName, directive);
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000E5CAD File Offset: 0x000E4CAD
		internal virtual bool IsMainDirective(string directiveName)
		{
			return string.Compare(directiveName, this.DefaultDirectiveName, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000E5CC0 File Offset: 0x000E4CC0
		private void ImportSourceFile(VirtualPath virtualPath)
		{
			VirtualPath parent = this._virtualPath.Parent;
			VirtualPath virtualPath2 = parent.Combine(virtualPath);
			this.AddSourceDependency(virtualPath2);
			CompilationUtil.GetCompilerInfoFromVirtualPath(virtualPath2);
			BuildResultCompiledAssembly buildResultCompiledAssembly = (BuildResultCompiledAssembly)BuildManager.GetVPathBuildResult(virtualPath2);
			Assembly resultAssembly = buildResultCompiledAssembly.ResultAssembly;
			this.AddAssemblyDependency(resultAssembly);
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000E5D09 File Offset: 0x000E4D09
		internal void AddSourceDependency(VirtualPath fileName)
		{
			if (this._sourceDependencies == null)
			{
				this._sourceDependencies = new CaseInsensitiveStringSet();
			}
			this._sourceDependencies.Add(fileName.VirtualPathString);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000E5D30 File Offset: 0x000E4D30
		private void AddAssemblyDependency(string assemblyName)
		{
			Assembly assembly = Assembly.Load(assemblyName);
			this.AddAssemblyDependency(assembly);
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000E5D4B File Offset: 0x000E4D4B
		private void AddAssemblyDependency(Assembly assembly)
		{
			if (this._linkedAssemblies == null)
			{
				this._linkedAssemblies = new AssemblySet();
			}
			this._linkedAssemblies.Add(assembly);
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000E5D6C File Offset: 0x000E4D6C
		private Type GetType(string typeName)
		{
			Type type;
			if (Util.TypeNameContainsAssembly(typeName))
			{
				try
				{
					type = Type.GetType(typeName, true);
				}
				catch (Exception ex)
				{
					throw new HttpParseException(null, ex, this._virtualPath, this._sourceString, this._lineNumber);
				}
				return type;
			}
			type = Util.GetTypeFromAssemblies(this._referencedAssemblies, typeName, false);
			if (type != null)
			{
				return type;
			}
			type = Util.GetTypeFromAssemblies(this._linkedAssemblies, typeName, false);
			if (type != null)
			{
				return type;
			}
			throw new HttpParseException(SR.GetString("Could_not_create_type", new object[] { typeName }), null, this._virtualPath, this._sourceString, this._lineNumber);
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x000E5E0C File Offset: 0x000E4E0C
		ICollection IAssemblyDependencyParser.AssemblyDependencies
		{
			get
			{
				return this.AssemblyDependencies;
			}
		}

		// Token: 0x0400250F RID: 9487
		private SimpleHandlerBuildProvider _buildProvider;

		// Token: 0x04002510 RID: 9488
		private TextReader _reader;

		// Token: 0x04002511 RID: 9489
		private VirtualPath _virtualPath;

		// Token: 0x04002512 RID: 9490
		private int _lineNumber;

		// Token: 0x04002513 RID: 9491
		private int _startColumn;

		// Token: 0x04002514 RID: 9492
		private bool _fFoundMainDirective;

		// Token: 0x04002515 RID: 9493
		private string _typeName;

		// Token: 0x04002516 RID: 9494
		private CompilerType _compilerType;

		// Token: 0x04002517 RID: 9495
		private string _sourceString;

		// Token: 0x04002518 RID: 9496
		private AssemblySet _linkedAssemblies;

		// Token: 0x04002519 RID: 9497
		private ICollection _referencedAssemblies;

		// Token: 0x0400251A RID: 9498
		private static char[] s_newlineChars = new char[] { '\r', '\n' };

		// Token: 0x0400251B RID: 9499
		private static Regex _directiveRegex;

		// Token: 0x0400251C RID: 9500
		private bool _ignoreParseErrors;

		// Token: 0x0400251D RID: 9501
		private StringSet _sourceDependencies;
	}
}
