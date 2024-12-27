using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Xml.Xsl.IlGen;
using System.Xml.Xsl.Runtime;
using Microsoft.VisualBasic;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000103 RID: 259
	internal class Scripts
	{
		// Token: 0x06000B9F RID: 2975 RVA: 0x0003BE26 File Offset: 0x0003AE26
		public Scripts(Compiler compiler)
		{
			this.compiler = compiler;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0003BE56 File Offset: 0x0003AE56
		public Dictionary<string, Type> ScriptClasses
		{
			get
			{
				return this.nsToType;
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0003BE60 File Offset: 0x0003AE60
		public XmlExtensionFunction ResolveFunction(string name, string ns, int numArgs, IErrorHelper errorHelper)
		{
			Type type;
			if (this.nsToType.TryGetValue(ns, out type))
			{
				try
				{
					return this.extFuncs.Bind(name, ns, numArgs, type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				}
				catch (XslTransformException ex)
				{
					errorHelper.ReportError(ex.Message, new string[0]);
				}
			}
			return null;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0003BEBC File Offset: 0x0003AEBC
		public ScriptClass GetScriptClass(string ns, string language, IErrorHelper errorHelper)
		{
			CompilerInfo compilerInfo;
			try
			{
				compilerInfo = CodeDomProvider.GetCompilerInfo(language);
			}
			catch (ConfigurationException)
			{
				errorHelper.ReportError("Xslt_ScriptInvalidLanguage", new string[] { language });
				return null;
			}
			foreach (ScriptClass scriptClass in this.scriptClasses)
			{
				if (ns == scriptClass.ns)
				{
					if (compilerInfo != scriptClass.compilerInfo)
					{
						errorHelper.ReportError("Xslt_ScriptMixedLanguages", new string[] { ns });
						return null;
					}
					return scriptClass;
				}
			}
			ScriptClass scriptClass2 = new ScriptClass(ns, compilerInfo);
			scriptClass2.typeDecl.TypeAttributes = TypeAttributes.Public;
			this.scriptClasses.Add(scriptClass2);
			return scriptClass2;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0003BF9C File Offset: 0x0003AF9C
		public void CompileScripts()
		{
			List<ScriptClass> list = new List<ScriptClass>();
			for (int i = 0; i < this.scriptClasses.Count; i++)
			{
				if (this.scriptClasses[i] != null)
				{
					CompilerInfo compilerInfo = this.scriptClasses[i].compilerInfo;
					list.Clear();
					for (int j = i; j < this.scriptClasses.Count; j++)
					{
						if (this.scriptClasses[j] != null && this.scriptClasses[j].compilerInfo == compilerInfo)
						{
							list.Add(this.scriptClasses[j]);
							this.scriptClasses[j] = null;
						}
					}
					Assembly assembly = this.CompileAssembly(list);
					if (assembly != null)
					{
						foreach (ScriptClass scriptClass in list)
						{
							Type type = assembly.GetType("System.Xml.Xsl.CompiledQuery" + Type.Delimiter + scriptClass.typeDecl.Name);
							if (type != null)
							{
								this.nsToType.Add(scriptClass.ns, type);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0003C0D8 File Offset: 0x0003B0D8
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private Assembly CompileAssembly(List<ScriptClass> scriptsForLang)
		{
			TempFileCollection tempFiles = this.compiler.CompilerResults.TempFiles;
			CompilerErrorCollection errors = this.compiler.CompilerResults.Errors;
			ScriptClass scriptClass = scriptsForLang[scriptsForLang.Count - 1];
			bool flag = false;
			CodeDomProvider codeDomProvider;
			try
			{
				codeDomProvider = scriptClass.compilerInfo.CreateProvider();
			}
			catch (ConfigurationException ex)
			{
				errors.Add(scriptClass.CreateCompileExceptionError(ex));
				return null;
			}
			flag = codeDomProvider is VBCodeProvider;
			CodeCompileUnit[] array = new CodeCompileUnit[scriptsForLang.Count];
			CompilerParameters compilerParameters = scriptClass.compilerInfo.CreateDefaultCompilerParameters();
			compilerParameters.ReferencedAssemblies.Add(typeof(Res).Assembly.Location);
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			if (flag)
			{
				compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
			}
			bool flag2 = false;
			for (int i = 0; i < scriptsForLang.Count; i++)
			{
				ScriptClass scriptClass2 = scriptsForLang[i];
				CodeNamespace codeNamespace = new CodeNamespace("System.Xml.Xsl.CompiledQuery");
				foreach (string text in Scripts.defaultNamespaces)
				{
					codeNamespace.Imports.Add(new CodeNamespaceImport(text));
				}
				if (flag)
				{
					codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.VisualBasic"));
				}
				foreach (string text2 in scriptClass2.nsImports)
				{
					codeNamespace.Imports.Add(new CodeNamespaceImport(text2));
				}
				codeNamespace.Types.Add(scriptClass2.typeDecl);
				CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
				codeCompileUnit.Namespaces.Add(codeNamespace);
				if (flag)
				{
					codeCompileUnit.UserData["AllowLateBound"] = true;
					codeCompileUnit.UserData["RequireVariableDeclaration"] = false;
				}
				if (i == 0)
				{
					codeCompileUnit.AssemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Security.SecurityTransparentAttribute"));
				}
				array[i] = codeCompileUnit;
				foreach (string text3 in scriptClass2.refAssemblies)
				{
					compilerParameters.ReferencedAssemblies.Add(text3);
				}
				flag2 |= scriptClass2.refAssembliesByHref;
			}
			XsltSettings settings = this.compiler.Settings;
			compilerParameters.WarningLevel = ((settings.WarningLevel >= 0) ? settings.WarningLevel : compilerParameters.WarningLevel);
			compilerParameters.TreatWarningsAsErrors = settings.TreatWarningsAsErrors;
			compilerParameters.IncludeDebugInformation = this.compiler.IsDebug;
			string text4 = this.compiler.ScriptAssemblyPath;
			if (text4 != null && scriptsForLang.Count < this.scriptClasses.Count)
			{
				text4 = Path.ChangeExtension(text4, "." + this.GetLanguageName(scriptClass.compilerInfo) + Path.GetExtension(text4));
			}
			compilerParameters.OutputAssembly = text4;
			string text5 = ((settings.TempFiles != null) ? settings.TempFiles.TempDir : null);
			compilerParameters.TempFiles = new TempFileCollection(text5);
			bool flag3 = ((this.compiler.IsDebug && text4 == null) || XmlILTrace.IsEnabled) && !settings.CheckOnly;
			compilerParameters.TempFiles.KeepFiles = flag3;
			compilerParameters.GenerateInMemory = (text4 == null && !this.compiler.IsDebug && !flag2) || settings.CheckOnly;
			CompilerResults compilerResults;
			try
			{
				compilerResults = codeDomProvider.CompileAssemblyFromDom(compilerParameters, array);
			}
			catch (ExternalException ex2)
			{
				compilerResults = new CompilerResults(compilerParameters.TempFiles);
				compilerResults.Errors.Add(scriptClass.CreateCompileExceptionError(ex2));
			}
			if (!settings.CheckOnly)
			{
				foreach (object obj in compilerResults.TempFiles)
				{
					string text6 = (string)obj;
					tempFiles.AddFile(text6, tempFiles.KeepFiles);
				}
			}
			foreach (object obj2 in compilerResults.Errors)
			{
				CompilerError compilerError = (CompilerError)obj2;
				Scripts.FixErrorPosition(compilerError, scriptsForLang);
			}
			errors.AddRange(compilerResults.Errors);
			if (!compilerResults.Errors.HasErrors)
			{
				return compilerResults.CompiledAssembly;
			}
			return null;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0003C5B8 File Offset: 0x0003B5B8
		private string GetLanguageName(CompilerInfo compilerInfo)
		{
			Regex regex = new Regex("^[0-9a-zA-Z]+$");
			foreach (string text in compilerInfo.GetLanguages())
			{
				if (regex.IsMatch(text))
				{
					return text;
				}
			}
			string text2 = "script";
			int num = ++this.assemblyCounter;
			return text2 + num.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0003C628 File Offset: 0x0003B628
		private static void FixErrorPosition(CompilerError error, List<ScriptClass> scriptsForLang)
		{
			string text = error.FileName;
			foreach (ScriptClass scriptClass in scriptsForLang)
			{
				foreach (string text2 in scriptClass.scriptFiles)
				{
					if (text.Equals(text2, Scripts.fileNameComparison))
					{
						error.FileName = text2;
						return;
					}
				}
			}
			ScriptClass scriptClass2 = scriptsForLang[scriptsForLang.Count - 1];
			text = Path.GetFileNameWithoutExtension(text);
			int num;
			int num2;
			if ((num = text.LastIndexOf('.')) >= 0 && int.TryParse(text.Substring(num + 1), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num2) && (ulong)num2 < (ulong)((long)scriptsForLang.Count))
			{
				scriptClass2 = scriptsForLang[num2];
			}
			error.FileName = scriptClass2.endFileName;
			error.Line = scriptClass2.endLine;
			error.Column = scriptClass2.endPos;
		}

		// Token: 0x04000807 RID: 2055
		private const string ScriptClassesNamespace = "System.Xml.Xsl.CompiledQuery";

		// Token: 0x04000808 RID: 2056
		private Compiler compiler;

		// Token: 0x04000809 RID: 2057
		private List<ScriptClass> scriptClasses = new List<ScriptClass>();

		// Token: 0x0400080A RID: 2058
		private Dictionary<string, Type> nsToType = new Dictionary<string, Type>();

		// Token: 0x0400080B RID: 2059
		private XmlExtensionFunctionTable extFuncs = new XmlExtensionFunctionTable();

		// Token: 0x0400080C RID: 2060
		private static readonly string[] defaultNamespaces = new string[] { "System", "System.Collections", "System.Text", "System.Text.RegularExpressions", "System.Xml", "System.Xml.Xsl", "System.Xml.XPath" };

		// Token: 0x0400080D RID: 2061
		private int assemblyCounter;

		// Token: 0x0400080E RID: 2062
		private static readonly StringComparison fileNameComparison = StringComparison.OrdinalIgnoreCase;
	}
}
