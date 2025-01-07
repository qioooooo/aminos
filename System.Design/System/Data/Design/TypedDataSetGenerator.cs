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
	public sealed class TypedDataSetGenerator
	{
		public static ICollection<Assembly> ReferencedAssemblies
		{
			get
			{
				return TypedDataSetGenerator.referencedAssemblies;
			}
		}

		private TypedDataSetGenerator()
		{
		}

		public static string GetProviderName(string inputFileContent)
		{
			return TypedDataSetGenerator.GetProviderName(inputFileContent, null);
		}

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

		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, Hashtable customDBProviders)
		{
			TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, customDBProviders, TypedDataSetGenerator.GenerateOption.None);
		}

		public static void Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, Hashtable customDBProviders, TypedDataSetGenerator.GenerateOption option)
		{
			TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, customDBProviders, option, null);
		}

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

		public static string Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider)
		{
			return TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, TypedDataSetGenerator.GenerateOption.None);
		}

		public static string Generate(string inputFileContent, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider, TypedDataSetGenerator.GenerateOption option)
		{
			return TypedDataSetGenerator.Generate(inputFileContent, compileUnit, mainNamespace, codeProvider, option, null);
		}

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

		private static Assembly systemAssembly = Assembly.GetAssembly(typeof(Uri));

		private static Assembly dataAssembly = Assembly.GetAssembly(typeof(SqlDataAdapter));

		private static Assembly xmlAssembly = Assembly.GetAssembly(typeof(XmlSchemaType));

		private static Assembly[] fixedReferences = new Assembly[]
		{
			TypedDataSetGenerator.systemAssembly,
			TypedDataSetGenerator.dataAssembly,
			TypedDataSetGenerator.xmlAssembly
		};

		private static Assembly[] referencedAssemblies = null;

		private static Assembly entityAssembly;

		private static string LINQOverTDSAssemblyName = "System.Data.DataSetExtensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private static string[] imports = new string[0];

		[Flags]
		public enum GenerateOption
		{
			None = 0,
			HierarchicalUpdate = 1,
			LinqOverTypedDatasets = 2
		}
	}
}
