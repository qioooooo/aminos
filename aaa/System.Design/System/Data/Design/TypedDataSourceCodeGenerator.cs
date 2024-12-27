using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Xml.Serialization;

namespace System.Data.Design
{
	// Token: 0x020000D1 RID: 209
	internal sealed class TypedDataSourceCodeGenerator
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x0001C9BC File Offset: 0x0001B9BC
		internal TypedDataSourceCodeGenerator()
		{
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0001C9CF File Offset: 0x0001B9CF
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0001C9D7 File Offset: 0x0001B9D7
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0001C9E0 File Offset: 0x0001B9E0
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0001C9E8 File Offset: 0x0001B9E8
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0001C9F1 File Offset: 0x0001B9F1
		internal string DataSourceName
		{
			get
			{
				return this.designDataSource.GeneratorDataSetName;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001C9FE File Offset: 0x0001B9FE
		internal ArrayList ProblemList
		{
			get
			{
				return this.problemList;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001CA06 File Offset: 0x0001BA06
		internal TypedTableHandler TableHandler
		{
			get
			{
				return this.tableHandler;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001CA0E File Offset: 0x0001BA0E
		internal RelationHandler RelationHandler
		{
			get
			{
				return this.relationHandler;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001CA16 File Offset: 0x0001BA16
		internal TypedRowHandler RowHandler
		{
			get
			{
				return this.rowHandler;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0001CA1E File Offset: 0x0001BA1E
		internal bool GenerateExtendedProperties
		{
			get
			{
				return this.generateExtendedProperties;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001CA26 File Offset: 0x0001BA26
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0001CA2E File Offset: 0x0001BA2E
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001CA37 File Offset: 0x0001BA37
		internal TypedDataSetGenerator.GenerateOption GenerateOptions
		{
			get
			{
				return this.generateOption;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0001CA3F File Offset: 0x0001BA3F
		internal string DataSetNamespace
		{
			get
			{
				return this.dataSetNamespace;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001CA48 File Offset: 0x0001BA48
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

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001CD50 File Offset: 0x0001BD50
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

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001CDC4 File Offset: 0x0001BDC4
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

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001CFB4 File Offset: 0x0001BFB4
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

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001D03C File Offset: 0x0001C03C
		private string CreateAdaptersNamespace(string generatorDataSetName)
		{
			if (generatorDataSetName.StartsWith("[", StringComparison.Ordinal) && generatorDataSetName.EndsWith("]", StringComparison.Ordinal))
			{
				generatorDataSetName = generatorDataSetName.Substring(1, generatorDataSetName.Length - 2);
			}
			return MemberNameValidator.GenerateIdName(generatorDataSetName + "TableAdapters", this.CodeProvider, false);
		}

		// Token: 0x04000C9F RID: 3231
		private DesignDataSource designDataSource;

		// Token: 0x04000CA0 RID: 3232
		private CodeDomProvider codeProvider;

		// Token: 0x04000CA1 RID: 3233
		private ArrayList problemList = new ArrayList();

		// Token: 0x04000CA2 RID: 3234
		private TypedTableHandler tableHandler;

		// Token: 0x04000CA3 RID: 3235
		private RelationHandler relationHandler;

		// Token: 0x04000CA4 RID: 3236
		private TypedRowHandler rowHandler;

		// Token: 0x04000CA5 RID: 3237
		private bool generateExtendedProperties;

		// Token: 0x04000CA6 RID: 3238
		private IDictionary userData;

		// Token: 0x04000CA7 RID: 3239
		private bool generateSingleNamespace;

		// Token: 0x04000CA8 RID: 3240
		private TypedDataSetGenerator.GenerateOption generateOption;

		// Token: 0x04000CA9 RID: 3241
		private string dataSetNamespace;
	}
}
