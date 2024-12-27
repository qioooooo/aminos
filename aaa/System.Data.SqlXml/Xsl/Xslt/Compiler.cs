using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl.IlGen;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E2 RID: 226
	internal class Compiler
	{
		// Token: 0x06000A7E RID: 2686 RVA: 0x00032C24 File Offset: 0x00031C24
		public Compiler(XsltSettings settings, bool debug, string scriptAssemblyPath)
		{
			TempFileCollection tempFileCollection = settings.TempFiles ?? new TempFileCollection();
			if (XmlILTrace.IsEnabled)
			{
				tempFileCollection.KeepFiles = true;
			}
			this.Settings = settings;
			this.IsDebug = settings.IncludeDebugInformation || debug;
			this.ScriptAssemblyPath = scriptAssemblyPath;
			this.CompilerResults = new CompilerResults(tempFileCollection);
			this.Scripts = new Scripts(this);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00032D2A File Offset: 0x00031D2A
		public CompilerResults Compile(object stylesheet, XmlResolver xmlResolver, out QilExpression qil)
		{
			new XsltLoader().Load(this, stylesheet, xmlResolver);
			qil = QilGenerator.CompileStylesheet(this);
			return this.CompilerResults;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00032D48 File Offset: 0x00031D48
		public Stylesheet CreateStylesheet()
		{
			Stylesheet stylesheet = new Stylesheet(this, this.CurrentPrecedence);
			if (this.CurrentPrecedence-- == 0)
			{
				this.PrincipalStylesheet = stylesheet;
			}
			return stylesheet;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00032D80 File Offset: 0x00031D80
		public void ApplyNsAliases(ref string prefix, ref string nsUri)
		{
			NsAlias nsAlias;
			if (this.NsAliases.TryGetValue(nsUri, out nsAlias))
			{
				nsUri = nsAlias.ResultNsUri;
				prefix = nsAlias.ResultPrefix;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00032DB0 File Offset: 0x00031DB0
		public bool SetNsAlias(string ssheetNsUri, string resultNsUri, string resultPrefix, int importPrecedence)
		{
			NsAlias nsAlias;
			if (this.NsAliases.TryGetValue(ssheetNsUri, out nsAlias) && (importPrecedence < nsAlias.ImportPrecedence || resultNsUri == nsAlias.ResultNsUri))
			{
				return false;
			}
			this.NsAliases[ssheetNsUri] = new NsAlias(resultNsUri, resultPrefix, importPrecedence);
			return nsAlias != null;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00032E04 File Offset: 0x00031E04
		private void MergeWhitespaceRules(Stylesheet sheet)
		{
			for (int i = 0; i <= 2; i++)
			{
				sheet.WhitespaceRules[i].Reverse();
				this.WhitespaceRules.AddRange(sheet.WhitespaceRules[i]);
			}
			sheet.WhitespaceRules = null;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00032E44 File Offset: 0x00031E44
		private void MergeAttributeSets(Stylesheet sheet)
		{
			foreach (QilName qilName in sheet.AttributeSets.Keys)
			{
				AttributeSet attributeSet;
				if (!this.AttributeSets.TryGetValue(qilName, out attributeSet))
				{
					this.AttributeSets[qilName] = sheet.AttributeSets[qilName];
				}
				else
				{
					attributeSet.MergeContent(sheet.AttributeSets[qilName]);
				}
			}
			sheet.AttributeSets = null;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00032ED8 File Offset: 0x00031ED8
		private void MergeGlobalVarPars(Stylesheet sheet)
		{
			foreach (XslNode xslNode in sheet.GlobalVarPars)
			{
				VarPar varPar = (VarPar)xslNode;
				if (!this.AllGlobalVarPars.ContainsKey(varPar.Name))
				{
					if (varPar.NodeType == XslNodeType.Variable)
					{
						this.GlobalVars.Add(varPar);
					}
					else
					{
						this.ExternalPars.Add(varPar);
					}
					this.AllGlobalVarPars[varPar.Name] = varPar;
				}
			}
			sheet.GlobalVarPars = null;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00032F7C File Offset: 0x00031F7C
		public void MergeWithStylesheet(Stylesheet sheet)
		{
			this.MergeWhitespaceRules(sheet);
			this.MergeAttributeSets(sheet);
			this.MergeGlobalVarPars(sheet);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00032F93 File Offset: 0x00031F93
		public static string ConstructQName(string prefix, string localName)
		{
			if (prefix.Length == 0)
			{
				return localName;
			}
			return prefix + ':' + localName;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00032FB0 File Offset: 0x00031FB0
		public bool ParseQName(string qname, out string prefix, out string localName, IErrorHelper errorHelper)
		{
			bool flag;
			try
			{
				ValidateNames.ParseQNameThrow(qname, out prefix, out localName);
				flag = true;
			}
			catch (XmlException ex)
			{
				errorHelper.ReportError(ex.Message, null);
				prefix = this.PhantomNCName;
				localName = this.PhantomNCName;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00033000 File Offset: 0x00032000
		public bool ParseNameTest(string nameTest, out string prefix, out string localName, IErrorHelper errorHelper)
		{
			bool flag;
			try
			{
				ValidateNames.ParseNameTestThrow(nameTest, out prefix, out localName);
				flag = true;
			}
			catch (XmlException ex)
			{
				errorHelper.ReportError(ex.Message, null);
				prefix = this.PhantomNCName;
				localName = this.PhantomNCName;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00033050 File Offset: 0x00032050
		public void ValidatePiName(string name, IErrorHelper errorHelper)
		{
			try
			{
				ValidateNames.ValidateNameThrow(string.Empty, name, string.Empty, XPathNodeType.ProcessingInstruction, ValidateNames.Flags.AllExceptPrefixMapping);
			}
			catch (XmlException ex)
			{
				errorHelper.ReportError(ex.Message, null);
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00033094 File Offset: 0x00032094
		public string CreatePhantomNamespace()
		{
			return "\0namespace" + this.phantomNsCounter++;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000330C1 File Offset: 0x000320C1
		public bool IsPhantomNamespace(string namespaceName)
		{
			return namespaceName.Length > 0 && namespaceName[0] == '\0';
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000330D8 File Offset: 0x000320D8
		public bool IsPhantomName(QilName qname)
		{
			string namespaceUri = qname.NamespaceUri;
			return namespaceUri.Length > 0 && namespaceUri[0] == '\0';
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00033101 File Offset: 0x00032101
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00033114 File Offset: 0x00032114
		private int ErrorCount
		{
			get
			{
				return this.CompilerResults.Errors.Count;
			}
			set
			{
				for (int i = this.ErrorCount - 1; i >= value; i--)
				{
					this.CompilerResults.Errors.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00033145 File Offset: 0x00032145
		public void EnterForwardsCompatible()
		{
			this.savedErrorCount = this.ErrorCount;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00033153 File Offset: 0x00032153
		public bool ExitForwardsCompatible(bool fwdCompat)
		{
			if (fwdCompat && this.ErrorCount > this.savedErrorCount)
			{
				this.ErrorCount = this.savedErrorCount;
				return false;
			}
			return true;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00033175 File Offset: 0x00032175
		public CompilerError CreateError(ISourceLineInfo lineInfo, string res, params string[] args)
		{
			return new CompilerError(lineInfo.Uri, lineInfo.StartLine, lineInfo.StartPos, string.Empty, XslTransformException.CreateMessage(res, args));
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0003319C File Offset: 0x0003219C
		public void ReportError(ISourceLineInfo lineInfo, string res, params string[] args)
		{
			CompilerError compilerError = this.CreateError(lineInfo, res, args);
			this.CompilerResults.Errors.Add(compilerError);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x000331C8 File Offset: 0x000321C8
		public void ReportWarning(ISourceLineInfo lineInfo, string res, params string[] args)
		{
			int num = 1;
			if (0 <= this.Settings.WarningLevel && this.Settings.WarningLevel < num)
			{
				return;
			}
			CompilerError compilerError = this.CreateError(lineInfo, res, args);
			if (this.Settings.TreatWarningsAsErrors)
			{
				compilerError.ErrorText = XslTransformException.CreateMessage("Xslt_WarningAsError", new string[] { compilerError.ErrorText });
				this.CompilerResults.Errors.Add(compilerError);
				return;
			}
			compilerError.IsWarning = true;
			this.CompilerResults.Errors.Add(compilerError);
		}

		// Token: 0x040006E9 RID: 1769
		public XsltSettings Settings;

		// Token: 0x040006EA RID: 1770
		public bool IsDebug;

		// Token: 0x040006EB RID: 1771
		public string ScriptAssemblyPath;

		// Token: 0x040006EC RID: 1772
		public CompilerResults CompilerResults;

		// Token: 0x040006ED RID: 1773
		public Stylesheet PrincipalStylesheet;

		// Token: 0x040006EE RID: 1774
		public int CurrentPrecedence;

		// Token: 0x040006EF RID: 1775
		public XslNode StartApplyTemplates;

		// Token: 0x040006F0 RID: 1776
		public Scripts Scripts;

		// Token: 0x040006F1 RID: 1777
		public Output Output = new Output();

		// Token: 0x040006F2 RID: 1778
		public List<VarPar> ExternalPars = new List<VarPar>();

		// Token: 0x040006F3 RID: 1779
		public List<VarPar> GlobalVars = new List<VarPar>();

		// Token: 0x040006F4 RID: 1780
		public List<WhitespaceRule> WhitespaceRules = new List<WhitespaceRule>();

		// Token: 0x040006F5 RID: 1781
		public DecimalFormats DecimalFormats = new DecimalFormats();

		// Token: 0x040006F6 RID: 1782
		public Keys Keys = new Keys();

		// Token: 0x040006F7 RID: 1783
		public List<ProtoTemplate> AllTemplates = new List<ProtoTemplate>();

		// Token: 0x040006F8 RID: 1784
		public Dictionary<QilName, VarPar> AllGlobalVarPars = new Dictionary<QilName, VarPar>();

		// Token: 0x040006F9 RID: 1785
		public Dictionary<QilName, Template> NamedTemplates = new Dictionary<QilName, Template>();

		// Token: 0x040006FA RID: 1786
		public Dictionary<QilName, AttributeSet> AttributeSets = new Dictionary<QilName, AttributeSet>();

		// Token: 0x040006FB RID: 1787
		public Dictionary<string, NsAlias> NsAliases = new Dictionary<string, NsAlias>();

		// Token: 0x040006FC RID: 1788
		public Dictionary<QilName, XslFlags> ModeFlags = new Dictionary<QilName, XslFlags>();

		// Token: 0x040006FD RID: 1789
		public Dictionary<QilName, List<QilFunction>> ApplyTemplatesFunctions = new Dictionary<QilName, List<QilFunction>>();

		// Token: 0x040006FE RID: 1790
		public readonly string PhantomNCName = "error";

		// Token: 0x040006FF RID: 1791
		private int phantomNsCounter;

		// Token: 0x04000700 RID: 1792
		private int savedErrorCount = -1;
	}
}
