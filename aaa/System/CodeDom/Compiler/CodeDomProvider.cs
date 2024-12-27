using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001EA RID: 490
	[ComVisible(true)]
	[ToolboxItem(false)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeDomProvider : Component
	{
		// Token: 0x0600101B RID: 4123 RVA: 0x00035444 File Offset: 0x00034444
		[ComVisible(false)]
		public static CodeDomProvider CreateProvider(string language)
		{
			CompilerInfo compilerInfo = CodeDomProvider.GetCompilerInfo(language);
			return compilerInfo.CreateProvider();
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00035460 File Offset: 0x00034460
		[ComVisible(false)]
		public static string GetLanguageFromExtension(string extension)
		{
			CompilerInfo compilerInfoForExtensionNoThrow = CodeDomProvider.GetCompilerInfoForExtensionNoThrow(extension);
			if (compilerInfoForExtensionNoThrow == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("CodeDomProvider_NotDefined"));
			}
			return compilerInfoForExtensionNoThrow._compilerLanguages[0];
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003548F File Offset: 0x0003448F
		[ComVisible(false)]
		public static bool IsDefinedLanguage(string language)
		{
			return CodeDomProvider.GetCompilerInfoForLanguageNoThrow(language) != null;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0003549D File Offset: 0x0003449D
		[ComVisible(false)]
		public static bool IsDefinedExtension(string extension)
		{
			return CodeDomProvider.GetCompilerInfoForExtensionNoThrow(extension) != null;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000354AC File Offset: 0x000344AC
		[ComVisible(false)]
		public static CompilerInfo GetCompilerInfo(string language)
		{
			CompilerInfo compilerInfoForLanguageNoThrow = CodeDomProvider.GetCompilerInfoForLanguageNoThrow(language);
			if (compilerInfoForLanguageNoThrow == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("CodeDomProvider_NotDefined"));
			}
			return compilerInfoForLanguageNoThrow;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000354D4 File Offset: 0x000344D4
		private static CompilerInfo GetCompilerInfoForLanguageNoThrow(string language)
		{
			if (language == null)
			{
				throw new ArgumentNullException("language");
			}
			return (CompilerInfo)CodeDomProvider.Config._compilerLanguages[language.Trim()];
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003550C File Offset: 0x0003450C
		private static CompilerInfo GetCompilerInfoForExtensionNoThrow(string extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			return (CompilerInfo)CodeDomProvider.Config._compilerExtensions[extension.Trim()];
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00035544 File Offset: 0x00034544
		[ComVisible(false)]
		public static CompilerInfo[] GetAllCompilerInfo()
		{
			ArrayList allCompilerInfo = CodeDomProvider.Config._allCompilerInfo;
			return (CompilerInfo[])allCompilerInfo.ToArray(typeof(CompilerInfo));
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x00035574 File Offset: 0x00034574
		private static CodeDomCompilationConfiguration Config
		{
			get
			{
				CodeDomCompilationConfiguration codeDomCompilationConfiguration = (CodeDomCompilationConfiguration)PrivilegedConfigurationManager.GetSection("system.codedom");
				if (codeDomCompilationConfiguration == null)
				{
					return CodeDomCompilationConfiguration.Default;
				}
				return codeDomCompilationConfiguration;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0003559B File Offset: 0x0003459B
		public virtual string FileExtension
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x000355A2 File Offset: 0x000345A2
		public virtual LanguageOptions LanguageOptions
		{
			get
			{
				return LanguageOptions.None;
			}
		}

		// Token: 0x06001026 RID: 4134
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public abstract ICodeGenerator CreateGenerator();

		// Token: 0x06001027 RID: 4135 RVA: 0x000355A5 File Offset: 0x000345A5
		public virtual ICodeGenerator CreateGenerator(TextWriter output)
		{
			return this.CreateGenerator();
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000355AD File Offset: 0x000345AD
		public virtual ICodeGenerator CreateGenerator(string fileName)
		{
			return this.CreateGenerator();
		}

		// Token: 0x06001029 RID: 4137
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public abstract ICodeCompiler CreateCompiler();

		// Token: 0x0600102A RID: 4138 RVA: 0x000355B5 File Offset: 0x000345B5
		[Obsolete("Callers should not use the ICodeParser interface and should instead use the methods directly on the CodeDomProvider class. Those inheriting from CodeDomProvider must still implement this interface, and should exclude this warning or also obsolete this method.")]
		public virtual ICodeParser CreateParser()
		{
			return null;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000355B8 File Offset: 0x000345B8
		public virtual TypeConverter GetConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000355C0 File Offset: 0x000345C0
		public virtual CompilerResults CompileAssemblyFromDom(CompilerParameters options, params CodeCompileUnit[] compilationUnits)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromDomBatch(options, compilationUnits);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000355CF File Offset: 0x000345CF
		public virtual CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromFileBatch(options, fileNames);
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000355DE File Offset: 0x000345DE
		public virtual CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources)
		{
			return this.CreateCompilerHelper().CompileAssemblyFromSourceBatch(options, sources);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000355ED File Offset: 0x000345ED
		public virtual bool IsValidIdentifier(string value)
		{
			return this.CreateGeneratorHelper().IsValidIdentifier(value);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x000355FB File Offset: 0x000345FB
		public virtual string CreateEscapedIdentifier(string value)
		{
			return this.CreateGeneratorHelper().CreateEscapedIdentifier(value);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00035609 File Offset: 0x00034609
		public virtual string CreateValidIdentifier(string value)
		{
			return this.CreateGeneratorHelper().CreateValidIdentifier(value);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00035617 File Offset: 0x00034617
		public virtual string GetTypeOutput(CodeTypeReference type)
		{
			return this.CreateGeneratorHelper().GetTypeOutput(type);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00035625 File Offset: 0x00034625
		public virtual bool Supports(GeneratorSupport generatorSupport)
		{
			return this.CreateGeneratorHelper().Supports(generatorSupport);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00035633 File Offset: 0x00034633
		public virtual void GenerateCodeFromExpression(CodeExpression expression, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromExpression(expression, writer, options);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00035643 File Offset: 0x00034643
		public virtual void GenerateCodeFromStatement(CodeStatement statement, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromStatement(statement, writer, options);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00035653 File Offset: 0x00034653
		public virtual void GenerateCodeFromNamespace(CodeNamespace codeNamespace, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromNamespace(codeNamespace, writer, options);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00035663 File Offset: 0x00034663
		public virtual void GenerateCodeFromCompileUnit(CodeCompileUnit compileUnit, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromCompileUnit(compileUnit, writer, options);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00035673 File Offset: 0x00034673
		public virtual void GenerateCodeFromType(CodeTypeDeclaration codeType, TextWriter writer, CodeGeneratorOptions options)
		{
			this.CreateGeneratorHelper().GenerateCodeFromType(codeType, writer, options);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00035683 File Offset: 0x00034683
		public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00035694 File Offset: 0x00034694
		public virtual CodeCompileUnit Parse(TextReader codeStream)
		{
			return this.CreateParserHelper().Parse(codeStream);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000356A4 File Offset: 0x000346A4
		private ICodeCompiler CreateCompilerHelper()
		{
			ICodeCompiler codeCompiler = this.CreateCompiler();
			if (codeCompiler == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeCompiler;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x000356CC File Offset: 0x000346CC
		private ICodeGenerator CreateGeneratorHelper()
		{
			ICodeGenerator codeGenerator = this.CreateGenerator();
			if (codeGenerator == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeGenerator;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000356F4 File Offset: 0x000346F4
		private ICodeParser CreateParserHelper()
		{
			ICodeParser codeParser = this.CreateParser();
			if (codeParser == null)
			{
				throw new NotImplementedException(SR.GetString("NotSupported_CodeDomAPI"));
			}
			return codeParser;
		}
	}
}
