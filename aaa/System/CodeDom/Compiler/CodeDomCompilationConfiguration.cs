using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001E5 RID: 485
	internal class CodeDomCompilationConfiguration
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x000347AC File Offset: 0x000337AC
		internal static CodeDomCompilationConfiguration Default
		{
			get
			{
				return CodeDomCompilationConfiguration.defaultInstance;
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x000347B4 File Offset: 0x000337B4
		internal CodeDomCompilationConfiguration()
		{
			this._compilerLanguages = new Hashtable(StringComparer.OrdinalIgnoreCase);
			this._compilerExtensions = new Hashtable(StringComparer.OrdinalIgnoreCase);
			this._allCompilerInfo = new ArrayList();
			CompilerParameters compilerParameters = new CompilerParameters();
			compilerParameters.WarningLevel = 4;
			string text = "Microsoft.CSharp.CSharpCodeProvider, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
			CompilerInfo compilerInfo = new CompilerInfo(compilerParameters, text);
			compilerInfo._compilerLanguages = new string[] { "c#", "cs", "csharp" };
			compilerInfo._compilerExtensions = new string[] { ".cs", "cs" };
			compilerInfo._providerOptions = new Dictionary<string, string>();
			compilerInfo._providerOptions["CompilerVersion"] = "v2.0";
			this.AddCompilerInfo(compilerInfo);
			compilerParameters = new CompilerParameters();
			compilerParameters.WarningLevel = 4;
			text = "Microsoft.VisualBasic.VBCodeProvider, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
			compilerInfo = new CompilerInfo(compilerParameters, text);
			compilerInfo._compilerLanguages = new string[] { "vb", "vbs", "visualbasic", "vbscript" };
			compilerInfo._compilerExtensions = new string[] { ".vb", "vb" };
			compilerInfo._providerOptions = new Dictionary<string, string>();
			compilerInfo._providerOptions["CompilerVersion"] = "v2.0";
			this.AddCompilerInfo(compilerInfo);
			compilerParameters = new CompilerParameters();
			compilerParameters.WarningLevel = 4;
			text = "Microsoft.JScript.JScriptCodeProvider, Microsoft.JScript, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			this.AddCompilerInfo(new CompilerInfo(compilerParameters, text)
			{
				_compilerLanguages = new string[] { "js", "jscript", "javascript" },
				_compilerExtensions = new string[] { ".js", "js" },
				_providerOptions = new Dictionary<string, string>()
			});
			compilerParameters = new CompilerParameters();
			compilerParameters.WarningLevel = 4;
			text = "Microsoft.VJSharp.VJSharpCodeProvider, VJSharpCodeProvider, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			this.AddCompilerInfo(new CompilerInfo(compilerParameters, text)
			{
				_compilerLanguages = new string[] { "vj#", "vjs", "vjsharp" },
				_compilerExtensions = new string[] { ".jsl", "jsl", ".java", "java" },
				_providerOptions = new Dictionary<string, string>()
			});
			compilerParameters = new CompilerParameters();
			compilerParameters.WarningLevel = 4;
			text = "Microsoft.VisualC.CppCodeProvider, CppCodeProvider, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			this.AddCompilerInfo(new CompilerInfo(compilerParameters, text)
			{
				_compilerLanguages = new string[] { "c++", "mc", "cpp" },
				_compilerExtensions = new string[] { ".h", "h" },
				_providerOptions = new Dictionary<string, string>()
			});
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00034A94 File Offset: 0x00033A94
		private CodeDomCompilationConfiguration(CodeDomCompilationConfiguration original)
		{
			if (original._compilerLanguages != null)
			{
				this._compilerLanguages = (Hashtable)original._compilerLanguages.Clone();
			}
			if (original._compilerExtensions != null)
			{
				this._compilerExtensions = (Hashtable)original._compilerExtensions.Clone();
			}
			if (original._allCompilerInfo != null)
			{
				this._allCompilerInfo = (ArrayList)original._allCompilerInfo.Clone();
			}
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00034B04 File Offset: 0x00033B04
		private void AddCompilerInfo(CompilerInfo compilerInfo)
		{
			foreach (string text in compilerInfo._compilerLanguages)
			{
				this._compilerLanguages[text] = compilerInfo;
			}
			foreach (string text2 in compilerInfo._compilerExtensions)
			{
				this._compilerExtensions[text2] = compilerInfo;
			}
			this._allCompilerInfo.Add(compilerInfo);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00034B74 File Offset: 0x00033B74
		private void RemoveUnmapped()
		{
			for (int i = 0; i < this._allCompilerInfo.Count; i++)
			{
				((CompilerInfo)this._allCompilerInfo[i])._mapped = false;
			}
			foreach (object obj in this._compilerLanguages.Values)
			{
				CompilerInfo compilerInfo = (CompilerInfo)obj;
				compilerInfo._mapped = true;
			}
			foreach (object obj2 in this._compilerExtensions.Values)
			{
				CompilerInfo compilerInfo2 = (CompilerInfo)obj2;
				compilerInfo2._mapped = true;
			}
			for (int j = this._allCompilerInfo.Count - 1; j >= 0; j--)
			{
				if (!((CompilerInfo)this._allCompilerInfo[j])._mapped)
				{
					this._allCompilerInfo.RemoveAt(j);
				}
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00034C98 File Offset: 0x00033C98
		private CompilerInfo FindExistingCompilerInfo(string[] languageList, string[] extensionList)
		{
			CompilerInfo compilerInfo = null;
			foreach (object obj in this._allCompilerInfo)
			{
				CompilerInfo compilerInfo2 = (CompilerInfo)obj;
				if (compilerInfo2._compilerExtensions.Length == extensionList.Length && compilerInfo2._compilerLanguages.Length == languageList.Length)
				{
					bool flag = false;
					for (int i = 0; i < compilerInfo2._compilerExtensions.Length; i++)
					{
						if (compilerInfo2._compilerExtensions[i] != extensionList[i])
						{
							flag = true;
							break;
						}
					}
					for (int j = 0; j < compilerInfo2._compilerLanguages.Length; j++)
					{
						if (compilerInfo2._compilerLanguages[j] != languageList[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						compilerInfo = compilerInfo2;
						break;
					}
				}
			}
			return compilerInfo;
		}

		// Token: 0x04000F54 RID: 3924
		internal const string sectionName = "system.codedom";

		// Token: 0x04000F55 RID: 3925
		private static readonly char[] s_fieldSeparators = new char[] { ';' };

		// Token: 0x04000F56 RID: 3926
		internal Hashtable _compilerLanguages;

		// Token: 0x04000F57 RID: 3927
		internal Hashtable _compilerExtensions;

		// Token: 0x04000F58 RID: 3928
		internal ArrayList _allCompilerInfo;

		// Token: 0x04000F59 RID: 3929
		private static CodeDomCompilationConfiguration defaultInstance = new CodeDomCompilationConfiguration();

		// Token: 0x020001E6 RID: 486
		internal class SectionHandler
		{
			// Token: 0x06001007 RID: 4103 RVA: 0x00034DA1 File Offset: 0x00033DA1
			private SectionHandler()
			{
			}

			// Token: 0x06001008 RID: 4104 RVA: 0x00034DAC File Offset: 0x00033DAC
			internal static object CreateStatic(object inheritedObject, XmlNode node)
			{
				CodeDomCompilationConfiguration codeDomCompilationConfiguration = (CodeDomCompilationConfiguration)inheritedObject;
				CodeDomCompilationConfiguration codeDomCompilationConfiguration2;
				if (codeDomCompilationConfiguration == null)
				{
					codeDomCompilationConfiguration2 = new CodeDomCompilationConfiguration();
				}
				else
				{
					codeDomCompilationConfiguration2 = new CodeDomCompilationConfiguration(codeDomCompilationConfiguration);
				}
				HandlerBase.CheckForUnrecognizedAttributes(node);
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
					{
						if (xmlNode.Name == "compilers")
						{
							CodeDomCompilationConfiguration.SectionHandler.ProcessCompilersElement(codeDomCompilationConfiguration2, xmlNode);
						}
						else
						{
							HandlerBase.ThrowUnrecognizedElement(xmlNode);
						}
					}
				}
				return codeDomCompilationConfiguration2;
			}

			// Token: 0x06001009 RID: 4105 RVA: 0x00034E4C File Offset: 0x00033E4C
			private static IDictionary<string, string> GetProviderOptions(XmlNode compilerNode)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (object obj in compilerNode)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.Name != "providerOption")
					{
						HandlerBase.ThrowUnrecognizedElement(xmlNode);
					}
					string text = null;
					string text2 = null;
					HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text);
					HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "value", ref text2);
					HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
					HandlerBase.CheckForChildNodes(xmlNode);
					dictionary[text] = text2;
				}
				return dictionary;
			}

			// Token: 0x0600100A RID: 4106 RVA: 0x00034EF4 File Offset: 0x00033EF4
			private static void ProcessCompilersElement(CodeDomCompilationConfiguration result, XmlNode node)
			{
				HandlerBase.CheckForUnrecognizedAttributes(node);
				string filename = ConfigurationErrorsException.GetFilename(node);
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					int lineNumber = ConfigurationErrorsException.GetLineNumber(xmlNode);
					if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
					{
						if (xmlNode.Name != "compiler")
						{
							HandlerBase.ThrowUnrecognizedElement(xmlNode);
						}
						string empty = string.Empty;
						HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "language", ref empty);
						string empty2 = string.Empty;
						HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "extension", ref empty2);
						string text = null;
						HandlerBase.GetAndRemoveStringAttribute(xmlNode, "type", ref text);
						CompilerParameters compilerParameters = new CompilerParameters();
						int num = 0;
						if (HandlerBase.GetAndRemoveNonNegativeIntegerAttribute(xmlNode, "warningLevel", ref num) != null)
						{
							compilerParameters.WarningLevel = num;
							compilerParameters.TreatWarningsAsErrors = num > 0;
						}
						string text2 = null;
						if (HandlerBase.GetAndRemoveStringAttribute(xmlNode, "compilerOptions", ref text2) != null)
						{
							compilerParameters.CompilerOptions = text2;
						}
						IDictionary<string, string> providerOptions = CodeDomCompilationConfiguration.SectionHandler.GetProviderOptions(xmlNode);
						HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
						string[] array = empty.Split(CodeDomCompilationConfiguration.s_fieldSeparators);
						string[] array2 = empty2.Split(CodeDomCompilationConfiguration.s_fieldSeparators);
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = array[i].Trim();
						}
						for (int j = 0; j < array2.Length; j++)
						{
							array2[j] = array2[j].Trim();
						}
						foreach (string text3 in array)
						{
							if (text3.Length == 0)
							{
								throw new ConfigurationErrorsException(SR.GetString("Language_Names_Cannot_Be_Empty"));
							}
						}
						foreach (string text4 in array2)
						{
							if (text4.Length == 0 || text4[0] != '.')
							{
								throw new ConfigurationErrorsException(SR.GetString("Extension_Names_Cannot_Be_Empty_Or_Non_Period_Based"));
							}
						}
						CompilerInfo compilerInfo;
						if (text != null)
						{
							compilerInfo = new CompilerInfo(compilerParameters, text);
						}
						else
						{
							compilerInfo = result.FindExistingCompilerInfo(array, array2);
							if (compilerInfo == null)
							{
								throw new ConfigurationErrorsException();
							}
						}
						compilerInfo.configFileName = filename;
						compilerInfo.configFileLineNumber = lineNumber;
						if (text != null)
						{
							compilerInfo._compilerLanguages = array;
							compilerInfo._compilerExtensions = array2;
							compilerInfo._providerOptions = providerOptions;
							result.AddCompilerInfo(compilerInfo);
						}
						else
						{
							foreach (KeyValuePair<string, string> keyValuePair in providerOptions)
							{
								compilerInfo._providerOptions[keyValuePair.Key] = keyValuePair.Value;
							}
						}
					}
				}
				result.RemoveUnmapped();
			}
		}
	}
}
