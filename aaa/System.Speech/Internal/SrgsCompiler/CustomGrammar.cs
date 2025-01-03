﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Speech.Internal.SrgsParser;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x0200017E RID: 382
	internal class CustomGrammar
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x00028D2C File Offset: 0x00027D2C
		internal CustomGrammar()
		{
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00028D98 File Offset: 0x00027D98
		internal string CreateAssembly(int iCfg, string outputFile, CultureInfo culture)
		{
			string text = null;
			File.Delete(outputFile);
			try
			{
				this.CreateAssembly(outputFile, false, null);
				this.CheckValidAssembly(iCfg, CustomGrammar.ExtractCodeGenerated(outputFile));
				text = this.GenerateCode(true, culture);
			}
			finally
			{
				CustomGrammar.DeleteFile(outputFile);
			}
			return text;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00028DE8 File Offset: 0x00027DE8
		internal void CreateAssembly(out byte[] il, out byte[] pdb)
		{
			string text = Path.GetTempFileName() + ".dll";
			File.Delete(text);
			try
			{
				this.CreateAssembly(text, this._fDebugScript, null);
				il = CustomGrammar.ExtractCodeGenerated(text);
				pdb = null;
				if (this._fDebugScript)
				{
					string text2 = text.Substring(0, text.LastIndexOf('.')) + ".pdb";
					pdb = CustomGrammar.ExtractCodeGenerated(text2);
					CustomGrammar.DeleteFile(text2);
				}
				this.CheckValidAssembly(0, il);
			}
			finally
			{
				CustomGrammar.DeleteFile(text);
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00028E78 File Offset: 0x00027E78
		internal void CreateAssembly(string path, List<CustomGrammar.CfgResource> cfgResources)
		{
			this.CreateAssembly(path, this._fDebugScript, cfgResources);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00028E88 File Offset: 0x00027E88
		internal void Combine(CustomGrammar cg, string innerCode)
		{
			if (this._rules.Count == 0)
			{
				this._language = cg._language;
			}
			else if (this._language != cg._language)
			{
				XmlParser.ThrowSrgsException(SRID.IncompatibleLanguageProperties, new object[0]);
			}
			if (this._namespace == null)
			{
				this._namespace = cg._namespace;
			}
			else if (this._namespace != cg._namespace)
			{
				XmlParser.ThrowSrgsException(SRID.IncompatibleNamespaceProperties, new object[0]);
			}
			this._fDebugScript |= cg._fDebugScript;
			foreach (string text in cg._codebehind)
			{
				if (!this._codebehind.Contains(text))
				{
					this._codebehind.Add(text);
				}
			}
			foreach (string text2 in cg._assemblyReferences)
			{
				if (!this._assemblyReferences.Contains(text2))
				{
					this._assemblyReferences.Add(text2);
				}
			}
			foreach (string text3 in cg._importNamespaces)
			{
				if (!this._importNamespaces.Contains(text3))
				{
					this._importNamespaces.Add(text3);
				}
			}
			this._keyFile = cg._keyFile;
			this._types.AddRange(cg._types);
			foreach (Rule rule in cg._rules)
			{
				if (this._types.Contains(rule.Name))
				{
					XmlParser.ThrowSrgsException(SRID.RuleDefinedMultipleTimes2, new object[] { rule.Name });
				}
			}
			this._script.Append(innerCode);
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x000290B0 File Offset: 0x000280B0
		internal bool HasScript
		{
			get
			{
				bool flag = this._script.Length > 0 || this._codebehind.Count > 0;
				if (!flag)
				{
					foreach (Rule rule in this._rules)
					{
						if (rule.Script.Length > 0)
						{
							flag = true;
							break;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00029134 File Offset: 0x00028134
		private void CreateAssembly(string outputFile, bool debug, List<CustomGrammar.CfgResource> cfgResources)
		{
			if (this._language == null)
			{
				XmlParser.ThrowSrgsException(SRID.NoLanguageSet, new object[0]);
			}
			string text = this.GenerateCode(false, null);
			string text2 = null;
			string[] array = null;
			try
			{
				if (this._codebehind.Count > 0)
				{
					int num = this._codebehind.Count + ((text != null) ? 1 : 0);
					array = new string[num];
					for (int i = 0; i < this._codebehind.Count; i++)
					{
						array[i] = this._codebehind[i];
					}
					if (text != null)
					{
						text2 = (array[array.Length - 1] = Path.GetTempFileName());
						using (StreamWriter streamWriter = new StreamWriter(text2))
						{
							streamWriter.Write(text);
						}
					}
				}
				this.CompileScript(outputFile, debug, text, array, cfgResources);
			}
			finally
			{
				CustomGrammar.DeleteFile(text2);
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002921C File Offset: 0x0002821C
		private void CompileScript(string outputFile, bool debug, string code, string[] codeFiles, List<CustomGrammar.CfgResource> cfgResouces)
		{
			using (CodeDomProvider codeDomProvider = this.CodeProvider())
			{
				CompilerParameters compilerParameters = CustomGrammar.GetCompilerParameters(outputFile, cfgResouces, debug, this._assemblyReferences, this._keyFile);
				CompilerResults compilerResults;
				if (codeFiles != null)
				{
					compilerResults = codeDomProvider.CompileAssemblyFromFile(compilerParameters, codeFiles);
				}
				else
				{
					compilerResults = codeDomProvider.CompileAssemblyFromSource(compilerParameters, new string[] { code });
				}
				if (compilerResults.Errors.Count > 0)
				{
					CustomGrammar.ThrowCompilationErrors(compilerResults);
				}
				if (compilerResults.NativeCompilerReturnValue != 0)
				{
					XmlParser.ThrowSrgsException(SRID.UnexpectedError, new object[] { compilerResults.NativeCompilerReturnValue });
				}
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000292C4 File Offset: 0x000282C4
		private CodeDomProvider CodeProvider()
		{
			CodeDomProvider codeDomProvider = null;
			string language;
			if ((language = this._language) != null)
			{
				if (language == "C#")
				{
					return CustomGrammar.CreateCSharpCompiler();
				}
				if (language == "VB.Net")
				{
					return CustomGrammar.CreateVBCompiler();
				}
			}
			XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { this._language });
			return codeDomProvider;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00029324 File Offset: 0x00028324
		private string GenerateCode(bool classDefinitionOnly, CultureInfo culture)
		{
			string empty = string.Empty;
			string language;
			if ((language = this._language) != null)
			{
				if (language == "C#")
				{
					return this.WrapScriptCSharp(classDefinitionOnly, culture);
				}
				if (language == "VB.Net")
				{
					return this.WrapScriptVB(classDefinitionOnly, culture);
				}
			}
			XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { this._language });
			return empty;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002938C File Offset: 0x0002838C
		private string WrapScriptCSharp(bool classDefinitionOnly, CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Rule rule in this._rules)
			{
				if (rule.Script != null)
				{
					CustomGrammar.WrapClassCSharp(stringBuilder, rule.Name, rule.BaseClass, culture, rule.Script.ToString(), rule.Constructors.ToString());
				}
			}
			if (this._script.Length > 0)
			{
				stringBuilder.Append(this._script);
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			if (classDefinitionOnly)
			{
				return stringBuilder.ToString();
			}
			return this.WrapScriptOuterCSharp(stringBuilder.ToString());
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002944C File Offset: 0x0002844C
		private string WrapScriptVB(bool classDefinitionOnly, CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Rule rule in this._rules)
			{
				if (rule.Script != null)
				{
					CustomGrammar.WrapClassVB(stringBuilder, rule.Name, rule.BaseClass, culture, rule.Script.ToString(), rule.Constructors.ToString());
				}
			}
			if (this._script.Length > 0)
			{
				stringBuilder.Append(this._script);
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			if (classDefinitionOnly)
			{
				return stringBuilder.ToString();
			}
			return this.WrapScriptOuterVB(stringBuilder.ToString());
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002950C File Offset: 0x0002850C
		private static CodeDomProvider CreateCSharpCompiler()
		{
			return new CSharpCodeProvider();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00029514 File Offset: 0x00028514
		private string WrapScriptOuterCSharp(string innerCode)
		{
			if (!string.IsNullOrEmpty(innerCode))
			{
				int num = 0;
				foreach (string text in this._importNamespaces)
				{
					num += text.Length;
				}
				SRID srid = SRID.ArrayOfNullIllegal;
				string @namespace = srid.GetType().Namespace;
				string text2 = string.Format(CultureInfo.InvariantCulture, "#line 1 \"{0}\"\nusing System;\nusing System.Collections.Generic;\nusing System.Diagnostics;\nusing {1};\nusing {1}.Recognition;\nusing {1}.Recognition.SrgsGrammar;\n", new object[] { "<Does Not Exist>", @namespace });
				StringBuilder stringBuilder = new StringBuilder(this._script.Length + text2.Length + 200);
				stringBuilder.Append(text2);
				foreach (string text3 in this._importNamespaces)
				{
					stringBuilder.Append("using ");
					stringBuilder.Append(text3);
					stringBuilder.Append(";\n");
				}
				if (this._namespace != null)
				{
					stringBuilder.Append("namespace ");
					stringBuilder.Append(this._namespace);
					stringBuilder.Append("\n{\n");
				}
				stringBuilder.Append(innerCode);
				if (this._namespace != null)
				{
					stringBuilder.Append("}\n");
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00029694 File Offset: 0x00028694
		private static void WrapClassCSharp(StringBuilder sb, string classname, string baseclass, CultureInfo culture, string script, string constructor)
		{
			sb.Append("public partial class ");
			sb.Append(classname);
			sb.Append(" : ");
			sb.Append((!string.IsNullOrEmpty(baseclass)) ? baseclass : "Grammar");
			sb.Append(" \n {\n");
			if (culture != null)
			{
				sb.Append("[DebuggerBrowsable (DebuggerBrowsableState.Never)]public static string __cultureId = \"");
				sb.Append(culture.LCID.ToString(CultureInfo.InvariantCulture));
				sb.Append("\";\n");
			}
			sb.Append(constructor);
			sb.Append(script);
			sb.Append("override protected bool IsStg { get { return true; }}\n\n");
			sb.Append("\n}\n");
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00029743 File Offset: 0x00028743
		private static CodeDomProvider CreateVBCompiler()
		{
			return new VBCodeProvider();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002974C File Offset: 0x0002874C
		private string WrapScriptOuterVB(string innerCode)
		{
			if (!string.IsNullOrEmpty(innerCode))
			{
				int num = 0;
				foreach (string text in this._importNamespaces)
				{
					num += text.Length;
				}
				SRID srid = SRID.ArrayOfNullIllegal;
				string @namespace = srid.GetType().Namespace;
				string text2 = string.Format(CultureInfo.InvariantCulture, "#ExternalSource (\"{0}\", 1)\nImports System\nImports System.Collections.Generic\nImports System.Diagnostics\nImports {1}\nImports {1}.Recognition\nImports {1}.Recognition.SrgsGrammar\n", new object[] { "<Does Not Exist>", @namespace });
				StringBuilder stringBuilder = new StringBuilder(this._script.Length + text2.Length + 200);
				stringBuilder.Append(text2);
				foreach (string text3 in this._importNamespaces)
				{
					stringBuilder.Append("Imports ");
					stringBuilder.Append(text3);
					stringBuilder.Append("\n");
				}
				if (this._namespace != null)
				{
					stringBuilder.Append("Namespace ");
					stringBuilder.Append(this._namespace);
					stringBuilder.Append("\n");
				}
				stringBuilder.Append("#End ExternalSource\n");
				stringBuilder.Append(innerCode);
				if (this._namespace != null)
				{
					stringBuilder.Append("End Namespace\n");
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000298D8 File Offset: 0x000288D8
		private static void WrapClassVB(StringBuilder sb, string classname, string baseclass, CultureInfo culture, string script, string constructor)
		{
			sb.Append("Public Partial class ");
			sb.Append(classname);
			sb.Append("\n Inherits ");
			sb.Append((!string.IsNullOrEmpty(baseclass)) ? baseclass : "Grammar");
			sb.Append(" \n");
			if (culture != null)
			{
				sb.Append("<DebuggerBrowsable (DebuggerBrowsableState.Never)>Public Shared __cultureId as String = \"");
				sb.Append(culture.LCID.ToString(CultureInfo.InvariantCulture));
				sb.Append("\"\n");
			}
			sb.Append(constructor);
			sb.Append(script);
			sb.Append("Protected Overrides ReadOnly Property IsStg() As Boolean\nGet\nReturn True\nEnd Get\nEnd Property\n");
			sb.Append("\nEnd Class\n");
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00029988 File Offset: 0x00028988
		private static void ThrowCompilationErrors(CompilerResults results)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in results.Errors)
			{
				CompilerError compilerError = (CompilerError)obj;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("\n");
				}
				if (compilerError.FileName.IndexOf("<Does Not Exist>", StringComparison.Ordinal) == -1)
				{
					stringBuilder.Append(compilerError.FileName);
					stringBuilder.Append("(");
					stringBuilder.Append(compilerError.Line);
					stringBuilder.Append(",");
					stringBuilder.Append(compilerError.Column);
					stringBuilder.Append("): ");
				}
				stringBuilder.Append("error ");
				stringBuilder.Append(compilerError.ErrorNumber);
				stringBuilder.Append(": ");
				stringBuilder.Append(compilerError.ErrorText);
			}
			XmlParser.ThrowSrgsException(SRID.GrammarCompilerError, new object[] { stringBuilder.ToString() });
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00029AA8 File Offset: 0x00028AA8
		private static CompilerParameters GetCompilerParameters(string outputFile, List<CustomGrammar.CfgResource> cfgResources, bool debug, Collection<string> assemblyReferences, string keyfile)
		{
			CompilerParameters compilerParameters = new CompilerParameters();
			StringBuilder stringBuilder = new StringBuilder();
			compilerParameters.GenerateInMemory = false;
			compilerParameters.OutputAssembly = outputFile;
			compilerParameters.IncludeDebugInformation = debug;
			if (debug)
			{
				stringBuilder.Append("/define:DEBUG ");
			}
			if (keyfile != null)
			{
				stringBuilder.Append("/keyfile:");
				stringBuilder.Append(keyfile);
			}
			compilerParameters.CompilerOptions = stringBuilder.ToString();
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			compilerParameters.ReferencedAssemblies.Add(executingAssembly.Location);
			foreach (string text in assemblyReferences)
			{
				compilerParameters.ReferencedAssemblies.Add(text);
			}
			if (cfgResources != null)
			{
				foreach (CustomGrammar.CfgResource cfgResource in cfgResources)
				{
					using (FileStream fileStream = new FileStream(cfgResource.name, FileMode.Create, FileAccess.Write))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write(cfgResource.data, 0, cfgResource.data.Length);
							compilerParameters.EmbeddedResources.Add(cfgResource.name);
						}
					}
				}
			}
			return compilerParameters;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00029C34 File Offset: 0x00028C34
		private static void DeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				try
				{
					File.Delete(path);
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00029C64 File Offset: 0x00028C64
		private void CheckValidAssembly(int iCfg, byte[] il)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AppDomain appDomain = null;
			try
			{
				appDomain = AppDomain.CreateDomain("Loading Domain");
				AppDomainCompilerProxy appDomainCompilerProxy = (AppDomainCompilerProxy)appDomain.CreateInstanceFromAndUnwrap(executingAssembly.GetName().CodeBase, "System.Speech.Internal.SrgsCompiler.AppDomainCompilerProxy");
				int count = this._scriptRefs.Count;
				string[] array = new string[count];
				string[] array2 = new string[count];
				int[] array3 = new int[count];
				for (int i = 0; i < count; i++)
				{
					ScriptRef scriptRef = this._scriptRefs[i];
					array[i] = scriptRef._rule;
					array2[i] = scriptRef._sMethod;
					array3[i] = (int)scriptRef._method;
				}
				Exception ex = appDomainCompilerProxy.CheckAssembly(il, iCfg, this._language, this._namespace, array, array2, array3);
				if (ex != null)
				{
					throw ex;
				}
				CustomGrammar.AssociateConstructorsWithRules(appDomainCompilerProxy, array, this._rules, iCfg, this._language);
			}
			finally
			{
				if (appDomain != null)
				{
					AppDomain.Unload(appDomain);
					appDomain = null;
				}
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00029D60 File Offset: 0x00028D60
		private static void AssociateConstructorsWithRules(AppDomainCompilerProxy proxy, string[] names, List<Rule> rules, int iCfg, string language)
		{
			string[] array = proxy.Constructors();
			foreach (Rule rule in rules)
			{
				int num = 0;
				while (num < names.Length && (num = Array.IndexOf<string>(names, rule.Name, num)) >= 0)
				{
					if (array[num] != null)
					{
						rule.Constructors.Append(array[num]);
					}
					num++;
				}
				if (rule.Constructors.Length == 0)
				{
					rule.Constructors.Append(proxy.GenerateConstructor(iCfg, new ParameterInfo[0], language, rule.Name));
				}
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00029E10 File Offset: 0x00028E10
		private static byte[] ExtractCodeGenerated(string path)
		{
			byte[] array = null;
			if (!string.IsNullOrEmpty(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					array = Helpers.ReadStreamToByteArray(fileStream, (int)fileStream.Length);
				}
			}
			return array;
		}

		// Token: 0x040008CE RID: 2254
		private const string _preambuleMarker = "<Does Not Exist>";

		// Token: 0x040008CF RID: 2255
		internal string _language = "C#";

		// Token: 0x040008D0 RID: 2256
		internal string _namespace;

		// Token: 0x040008D1 RID: 2257
		internal List<Rule> _rules = new List<Rule>();

		// Token: 0x040008D2 RID: 2258
		internal Collection<string> _codebehind = new Collection<string>();

		// Token: 0x040008D3 RID: 2259
		internal bool _fDebugScript;

		// Token: 0x040008D4 RID: 2260
		internal Collection<string> _assemblyReferences = new Collection<string>();

		// Token: 0x040008D5 RID: 2261
		internal Collection<string> _importNamespaces = new Collection<string>();

		// Token: 0x040008D6 RID: 2262
		internal string _keyFile;

		// Token: 0x040008D7 RID: 2263
		internal Collection<ScriptRef> _scriptRefs = new Collection<ScriptRef>();

		// Token: 0x040008D8 RID: 2264
		internal List<string> _types = new List<string>();

		// Token: 0x040008D9 RID: 2265
		internal StringBuilder _script = new StringBuilder();

		// Token: 0x0200017F RID: 383
		internal class CfgResource
		{
			// Token: 0x040008DA RID: 2266
			internal string name;

			// Token: 0x040008DB RID: 2267
			internal byte[] data;
		}
	}
}
