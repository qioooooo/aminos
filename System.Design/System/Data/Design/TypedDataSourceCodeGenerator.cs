using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Xml.Serialization;

namespace System.Data.Design
{
	internal sealed class TypedDataSourceCodeGenerator
	{
		internal TypedDataSourceCodeGenerator()
		{
		}

		internal CodeDomProvider CodeProvider
		{
			get
			{
				return this.codeProvider;
			}
			set
			{
				this.codeProvider = value;
			}
		}

		internal IDictionary UserData
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		internal string DataSourceName
		{
			get
			{
				return this.designDataSource.GeneratorDataSetName;
			}
		}

		internal ArrayList ProblemList
		{
			get
			{
				return this.problemList;
			}
		}

		internal TypedTableHandler TableHandler
		{
			get
			{
				return this.tableHandler;
			}
		}

		internal RelationHandler RelationHandler
		{
			get
			{
				return this.relationHandler;
			}
		}

		internal TypedRowHandler RowHandler
		{
			get
			{
				return this.rowHandler;
			}
		}

		internal bool GenerateExtendedProperties
		{
			get
			{
				return this.generateExtendedProperties;
			}
		}

		internal bool GenerateSingleNamespace
		{
			get
			{
				return this.generateSingleNamespace;
			}
			set
			{
				this.generateSingleNamespace = value;
			}
		}

		internal TypedDataSetGenerator.GenerateOption GenerateOptions
		{
			get
			{
				return this.generateOption;
			}
		}

		internal string DataSetNamespace
		{
			get
			{
				return this.dataSetNamespace;
			}
		}

		internal void GenerateDataSource(DesignDataSource dtDataSource, CodeCompileUnit codeCompileUnit, CodeNamespace mainNamespace, string dataSetNamespace, TypedDataSetGenerator.GenerateOption generateOption)
		{
			this.designDataSource = dtDataSource;
			this.generateOption = generateOption;
			this.dataSetNamespace = dataSetNamespace;
			bool flag = (generateOption & TypedDataSetGenerator.GenerateOption.HierarchicalUpdate) == TypedDataSetGenerator.GenerateOption.HierarchicalUpdate;
			flag = flag && dtDataSource.EnableTableAdapterManager;
			this.AddUserData(codeCompileUnit);
			CodeTypeDeclaration codeTypeDeclaration = this.CreateDataSourceDeclaration(dtDataSource);
			mainNamespace.Types.Add(codeTypeDeclaration);
			bool flag2 = CodeGenHelper.SupportsMultipleNamespaces(this.codeProvider);
			CodeNamespace codeNamespace = null;
			if (!this.GenerateSingleNamespace && flag2)
			{
				string text = this.CreateAdaptersNamespace(dtDataSource.GeneratorDataSetName);
				if (!StringUtil.Empty(mainNamespace.Name))
				{
					text = mainNamespace.Name + "." + text;
				}
				codeNamespace = new CodeNamespace(text);
			}
			DataComponentGenerator dataComponentGenerator = new DataComponentGenerator(this);
			bool flag3 = false;
			foreach (object obj in dtDataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if (designTable.TableType == TableType.RadTable)
				{
					flag3 = true;
					designTable.PropertyCache = new DesignTable.CodeGenPropertyCache(designTable);
					CodeTypeDeclaration codeTypeDeclaration2 = dataComponentGenerator.GenerateDataComponent(designTable, false, flag);
					if (this.GenerateSingleNamespace)
					{
						mainNamespace.Types.Add(codeTypeDeclaration2);
					}
					else if (flag2)
					{
						codeNamespace.Types.Add(codeTypeDeclaration2);
					}
					else
					{
						codeTypeDeclaration2.Name = codeTypeDeclaration.Name + codeTypeDeclaration2.Name;
						mainNamespace.Types.Add(codeTypeDeclaration2);
					}
				}
			}
			flag = flag && flag3;
			if (dtDataSource.Sources != null && dtDataSource.Sources.Count > 0)
			{
				DesignTable designTable2 = new DesignTable();
				designTable2.TableType = TableType.RadTable;
				designTable2.MainSource = null;
				designTable2.GeneratorDataComponentClassName = dtDataSource.GeneratorFunctionsComponentClassName;
				foreach (object obj2 in dtDataSource.Sources)
				{
					Source source = (Source)obj2;
					designTable2.Sources.Add(source);
				}
				CodeTypeDeclaration codeTypeDeclaration3 = dataComponentGenerator.GenerateDataComponent(designTable2, true, flag);
				if (this.GenerateSingleNamespace)
				{
					mainNamespace.Types.Add(codeTypeDeclaration3);
				}
				else if (flag2)
				{
					codeNamespace.Types.Add(codeTypeDeclaration3);
				}
				else
				{
					codeTypeDeclaration3.Name = codeTypeDeclaration.Name + codeTypeDeclaration3.Name;
					mainNamespace.Types.Add(codeTypeDeclaration3);
				}
			}
			if (codeNamespace != null && codeNamespace.Types.Count > 0)
			{
				codeCompileUnit.Namespaces.Add(codeNamespace);
			}
			if (flag)
			{
				TableAdapterManagerGenerator tableAdapterManagerGenerator = new TableAdapterManagerGenerator(this);
				CodeTypeDeclaration codeTypeDeclaration4 = tableAdapterManagerGenerator.GenerateAdapterManager(this.designDataSource, codeTypeDeclaration);
				if (this.GenerateSingleNamespace)
				{
					mainNamespace.Types.Add(codeTypeDeclaration4);
					return;
				}
				if (flag2)
				{
					codeNamespace.Types.Add(codeTypeDeclaration4);
					return;
				}
				codeTypeDeclaration4.Name = codeTypeDeclaration.Name + codeTypeDeclaration4.Name;
				mainNamespace.Types.Add(codeTypeDeclaration4);
			}
		}

		private void AddUserData(CodeCompileUnit compileUnit)
		{
			if (this.UserData != null)
			{
				foreach (object obj in this.UserData.Keys)
				{
					compileUnit.UserData.Add(obj, this.userData[obj]);
				}
			}
		}

		private CodeTypeDeclaration CreateDataSourceDeclaration(DesignDataSource dtDataSource)
		{
			if (dtDataSource.Name == null)
			{
				throw new DataSourceGeneratorException("DataSource name cannot be null.");
			}
			NameHandler nameHandler = new NameHandler(this.codeProvider);
			nameHandler.GenerateMemberNames(dtDataSource, this.problemList);
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class(dtDataSource.GeneratorDataSetName, true, dtDataSource.Modifier);
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(DataSet)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.Serializable"));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerCategoryAttribute", CodeGenHelper.Str("code")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.ToolboxItem", CodeGenHelper.Primitive(true)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(XmlSchemaProviderAttribute).FullName, CodeGenHelper.Primitive("GetTypedDataSetSchema")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(XmlRootAttribute).FullName, CodeGenHelper.Primitive(dtDataSource.GeneratorDataSetName)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(HelpKeywordAttribute).FullName, CodeGenHelper.Str("vs.data.DataSet")));
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Represents a strongly typed in-memory cache of data.", true));
			this.tableHandler = new TypedTableHandler(this, dtDataSource.DesignTables);
			this.relationHandler = new RelationHandler(this, dtDataSource.DesignRelations);
			this.rowHandler = new TypedRowHandler(this, dtDataSource.DesignTables);
			DatasetMethodGenerator datasetMethodGenerator = new DatasetMethodGenerator(this, dtDataSource);
			this.tableHandler.AddPrivateVars(codeTypeDeclaration);
			this.tableHandler.AddTableProperties(codeTypeDeclaration);
			this.relationHandler.AddPrivateVars(codeTypeDeclaration);
			datasetMethodGenerator.AddMethods(codeTypeDeclaration);
			this.rowHandler.AddTypedRowEventHandlers(codeTypeDeclaration);
			this.tableHandler.AddTableClasses(codeTypeDeclaration);
			this.rowHandler.AddTypedRows(codeTypeDeclaration);
			this.rowHandler.AddTypedRowEventArgs(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		internal static ArrayList GetProviderAssemblies(DesignDataSource designDS)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in designDS.DesignConnections)
			{
				IDesignConnection designConnection = (IDesignConnection)obj;
				IDbConnection dbConnection = designConnection.CreateEmptyDbConnection();
				if (dbConnection != null)
				{
					Assembly assembly = dbConnection.GetType().Assembly;
					if (!arrayList.Contains(assembly))
					{
						arrayList.Add(assembly);
					}
				}
			}
			return arrayList;
		}

		private string CreateAdaptersNamespace(string generatorDataSetName)
		{
			if (generatorDataSetName.StartsWith("[", StringComparison.Ordinal) && generatorDataSetName.EndsWith("]", StringComparison.Ordinal))
			{
				generatorDataSetName = generatorDataSetName.Substring(1, generatorDataSetName.Length - 2);
			}
			return MemberNameValidator.GenerateIdName(generatorDataSetName + "TableAdapters", this.CodeProvider, false);
		}

		private DesignDataSource designDataSource;

		private CodeDomProvider codeProvider;

		private ArrayList problemList = new ArrayList();

		private TypedTableHandler tableHandler;

		private RelationHandler relationHandler;

		private TypedRowHandler rowHandler;

		private bool generateExtendedProperties;

		private IDictionary userData;

		private bool generateSingleNamespace;

		private TypedDataSetGenerator.GenerateOption generateOption;

		private string dataSetNamespace;
	}
}
