using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Data.Design
{
	// Token: 0x020000C8 RID: 200
	public sealed class TypedDataSetGenerator
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001BF1C File Offset: 0x0001AF1C
		public static ICollection<Assembly> ReferencedAssemblies
		{
			get
			{
				return TypedDataSetGenerator.referencedAssemblies;
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001BF23 File Offset: 0x0001AF23
		private TypedDataSetGenerator()
		{
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001BF2B File Offset: 0x0001AF2B
		public static string GetProviderName(string inputFileContent)
		{
			return TypedDataSetGenerator.GetProviderName(inputFileContent, null);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001BF34 File Offset: 0x0001AF34
		public static string GetProviderName(string inputFileContent, string tableName)
		{
			if (inputFileContent == null || inputFileContent.Length == 0)
			{
				throw new ArgumentException(SR.GetString("CG_DataSetGeneratorFail_InputFileEmpty"));
			}
			StringReader stringReader = new StringReader(inputFileContent);
			DesignDataSource designDataSource = new DesignDataSource();
			try
			{
				designDataSource.ReadXmlSchema(stringReader);
			}
			catch (Exception ex)
			{
				string @string = SR.GetString("CG_DataSetGeneratorFail_UnableToConvertToDataSet", new object[] { TypedDataSetGenerator.CreateExceptionMessage(ex) });
				throw new Exception(@string, ex);
			}
			if (tableName == null || tableName.Length == 0)
			{
				if (designDataSource.DefaultConnection != null)
				{
					return designDataSource.DefaultConnection.Provider;
				}
			}
			else
			{
				DesignTable designTable = designDataSource.DesignTables[tableName];
				if (designTable != null)
				{
					return designTable.Connection.Provider;
				}
			}
			return null;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001BFEC File Offset: 0x0001AFEC
		public static string Generate(DataSet dataSet, CodeNamespace codeNamespace, CodeDomProvider codeProvider)
		{
			if (codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			if (dataSet == null)
			{
				throw new ArgumentException(SR.GetString("CG_DataSetGeneratorFail_DatasetNull"));
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			dataSet.WriteXmlSchema(stringWriter);
			StringBuilder stringBuilder = stringWriter.GetStringBuilder();
			return TypedDataSetGenerator.Generate(stringBuilder.ToString(), null, codeNamespace, codeProvider);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001C044 File Offset: 0x0001B044
		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, DbProviderFactory specifiedFactory)
		{
			if (specifiedFactory != null)
			{
				ProviderManager.ActiveFactoryContext = specifiedFactory;
			}
			try
			{
				TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider);
			}
			finally
			{
				ProviderManager.ActiveFactoryContext = null;
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001C080 File Offset: 0x0001B080
		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, Hashtable customDBProviders)
		{
			TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, customDBProviders, TypedDataSetGenerator.GenerateOption.None);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001C08E File Offset: 0x0001B08E
		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, Hashtable customDBProviders, TypedDataSetGenerator.GenerateOption option)
		{
			TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, customDBProviders, option, null);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001C0A0 File Offset: 0x0001B0A0
		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, Hashtable customDBProviders, TypedDataSetGenerator.GenerateOption option, string dataSetNamespace)
		{
			if (customDBProviders != null)
			{
				ProviderManager.CustomDBProviders = customDBProviders;
			}
			try
			{
				TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, option, dataSetNamespace);
			}
			finally
			{
				ProviderManager.CustomDBProviders = null;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001C0E0 File Offset: 0x0001B0E0
		public static string Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider)
		{
			return TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, TypedDataSetGenerator.GenerateOption.None);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001C0EC File Offset: 0x0001B0EC
		public static string Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, TypedDataSetGenerator.GenerateOption option)
		{
			return TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, option, null);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001C0FC File Offset: 0x0001B0FC
		public static string Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, TypedDataSetGenerator.GenerateOption option, string dataSetNamespace)
		{
			if (inputFileContent == null || inputFileContent.Length == 0)
			{
				throw new ArgumentException(SR.GetString("CG_DataSetGeneratorFail_InputFileEmpty"));
			}
			if (mainNamespace == null)
			{
				throw new ArgumentException(SR.GetString("CG_DataSetGeneratorFail_CodeNamespaceNull"));
			}
			if (codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			StringReader stringReader = new StringReader(inputFileContent);
			DesignDataSource designDataSource = new DesignDataSource();
			try
			{
				designDataSource.ReadXmlSchema(stringReader);
			}
			catch (Exception ex)
			{
				string @string = SR.GetString("CG_DataSetGeneratorFail_UnableToConvertToDataSet", new object[] { TypedDataSetGenerator.CreateExceptionMessage(ex) });
				throw new Exception(@string, ex);
			}
			return TypedDataSetGenerator.GenerateInternal(designDataSource, compileUnit, mainNamespace, codeProvider, option, dataSetNamespace);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001C1A0 File Offset: 0x0001B1A0
		internal static string GenerateInternal(DesignDataSource designDS, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, TypedDataSetGenerator.GenerateOption generateOption, string dataSetNamespace)
		{
			if (StringUtil.Empty(designDS.Name))
			{
				designDS.Name = "DataSet1";
			}
			try
			{
				TypedDataSourceCodeGenerator typedDataSourceCodeGenerator = new TypedDataSourceCodeGenerator();
				typedDataSourceCodeGenerator.CodeProvider = codeProvider;
				typedDataSourceCodeGenerator.GenerateSingleNamespace = false;
				if (mainNamespace == null)
				{
					mainNamespace = new CodeNamespace();
				}
				if (compileUnit == null)
				{
					compileUnit = new CodeCompileUnit();
					compileUnit.Namespaces.Add(mainNamespace);
				}
				typedDataSourceCodeGenerator.GenerateDataSource(designDS, compileUnit, mainNamespace, dataSetNamespace, generateOption);
				foreach (string text in TypedDataSetGenerator.imports)
				{
					mainNamespace.Imports.Add(new CodeNamespaceImport(text));
				}
			}
			catch (Exception ex)
			{
				string @string = SR.GetString("CG_DataSetGeneratorFail_FailToGenerateCode", new object[] { TypedDataSetGenerator.CreateExceptionMessage(ex) });
				throw new Exception(@string, ex);
			}
			ArrayList arrayList = new ArrayList(TypedDataSetGenerator.fixedReferences);
			arrayList.AddRange(TypedDataSourceCodeGenerator.GetProviderAssemblies(designDS));
			if ((generateOption & TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets) == TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets)
			{
				Assembly assembly = TypedDataSetGenerator.EntityAssembly;
				if (assembly != null)
				{
					arrayList.Add(assembly);
				}
			}
			TypedDataSetGenerator.referencedAssemblies = (Assembly[])arrayList.ToArray(typeof(Assembly));
			foreach (Assembly assembly2 in TypedDataSetGenerator.referencedAssemblies)
			{
				compileUnit.ReferencedAssemblies.Add(assembly2.GetName().Name + ".dll");
			}
			return designDS.GeneratorDataSetName;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0001C30C File Offset: 0x0001B30C
		private static Assembly EntityAssembly
		{
			get
			{
				if (TypedDataSetGenerator.entityAssembly == null)
				{
					try
					{
						TypedDataSetGenerator.entityAssembly = Assembly.Load(TypedDataSetGenerator.LINQOverTDSAssemblyName);
					}
					catch
					{
					}
				}
				return TypedDataSetGenerator.entityAssembly;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001C34C File Offset: 0x0001B34C
		private static string CreateExceptionMessage(Exception e)
		{
			string text = ((e.Message != null) ? e.Message : string.Empty);
			for (Exception ex = e.InnerException; ex != null; ex = ex.InnerException)
			{
				string message = ex.Message;
				if (message != null && message.Length > 0)
				{
					text = text + " " + message;
				}
			}
			return text;
		}

		// Token: 0x04000C7C RID: 3196
		private static Assembly systemAssembly = Assembly.GetAssembly(typeof(Uri));

		// Token: 0x04000C7D RID: 3197
		private static Assembly dataAssembly = Assembly.GetAssembly(typeof(SqlDataAdapter));

		// Token: 0x04000C7E RID: 3198
		private static Assembly xmlAssembly = Assembly.GetAssembly(typeof(XmlSchemaType));

		// Token: 0x04000C7F RID: 3199
		private static Assembly[] fixedReferences = new Assembly[]
		{
			TypedDataSetGenerator.systemAssembly,
			TypedDataSetGenerator.dataAssembly,
			TypedDataSetGenerator.xmlAssembly
		};

		// Token: 0x04000C80 RID: 3200
		private static Assembly[] referencedAssemblies = null;

		// Token: 0x04000C81 RID: 3201
		private static Assembly entityAssembly;

		// Token: 0x04000C82 RID: 3202
		private static string LINQOverTDSAssemblyName = "System.Data.DataSetExtensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000C83 RID: 3203
		private static string[] imports = new string[0];

		// Token: 0x020000C9 RID: 201
		[Flags]
		public enum GenerateOption
		{
			// Token: 0x04000C85 RID: 3205
			None = 0,
			// Token: 0x04000C86 RID: 3206
			HierarchicalUpdate = 1,
			// Token: 0x04000C87 RID: 3207
			LinqOverTypedDatasets = 2
		}
	}
}
