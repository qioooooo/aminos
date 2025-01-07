using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace System.Data.Design
{
	internal class QueryHandler
	{
		internal bool DeclarationsOnly
		{
			get
			{
				return this.declarationsOnly;
			}
			set
			{
				this.declarationsOnly = value;
			}
		}

		internal QueryHandler(TypedDataSourceCodeGenerator codeGenerator, DesignTable designTable)
		{
			this.codeGenerator = codeGenerator;
			this.designTable = designTable;
			this.languageSupportsNullables = this.codeGenerator.CodeProvider.Supports(GeneratorSupport.GenericTypeReference);
		}

		internal QueryHandler(CodeDomProvider codeProvider, DesignTable designTable)
		{
			this.codeGenerator = new TypedDataSourceCodeGenerator();
			this.codeGenerator.CodeProvider = codeProvider;
			this.designTable = designTable;
			this.languageSupportsNullables = this.codeGenerator.CodeProvider.Supports(GeneratorSupport.GenericTypeReference);
		}

		internal void AddQueriesToDataComponent(CodeTypeDeclaration classDeclaration)
		{
			this.AddMainQueriesToDataComponent(classDeclaration);
			this.AddSecondaryQueriesToDataComponent(classDeclaration);
			this.AddUpdateQueriesToDataComponent(classDeclaration);
			this.AddFunctionsToDataComponent(classDeclaration, false);
		}

		private void AddMainQueriesToDataComponent(CodeTypeDeclaration classDeclaration)
		{
			if (this.designTable == null)
			{
				throw new InternalException("Design Table should not be null.");
			}
			if (this.designTable.MainSource != null)
			{
				QueryGenerator queryGenerator = new QueryGenerator(this.codeGenerator);
				queryGenerator.DeclarationOnly = this.declarationsOnly;
				queryGenerator.MethodSource = this.designTable.MainSource as DbSource;
				queryGenerator.CommandIndex = 0;
				queryGenerator.DesignTable = this.designTable;
				if (queryGenerator.MethodSource.Connection != null)
				{
					queryGenerator.ProviderFactory = ProviderManager.GetFactory(queryGenerator.MethodSource.Connection.Provider);
				}
				else
				{
					if (this.designTable.Connection == null)
					{
						return;
					}
					queryGenerator.ProviderFactory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
				}
				if (queryGenerator.MethodSource.QueryType == QueryType.Rowset && queryGenerator.MethodSource.CommandOperation == CommandOperation.Select)
				{
					if (queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Fill || queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Both)
					{
						queryGenerator.MethodName = this.designTable.MainSource.GeneratorSourceName;
						this.GenerateQueries(classDeclaration, queryGenerator);
						if (queryGenerator.MethodSource.GeneratePagingMethods)
						{
							queryGenerator.MethodName = this.designTable.MainSource.GeneratorSourceNameForPaging;
							queryGenerator.GeneratePagingMethod = true;
							this.GenerateQueries(classDeclaration, queryGenerator);
							queryGenerator.GeneratePagingMethod = false;
						}
					}
					if (queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Get || queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Both)
					{
						queryGenerator.GenerateGetMethod = true;
						queryGenerator.MethodName = this.designTable.MainSource.GeneratorGetMethodName;
						this.GenerateQueries(classDeclaration, queryGenerator);
						if (queryGenerator.MethodSource.GeneratePagingMethods)
						{
							queryGenerator.MethodName = this.designTable.MainSource.GeneratorGetMethodNameForPaging;
							queryGenerator.GeneratePagingMethod = true;
							this.GenerateQueries(classDeclaration, queryGenerator);
							queryGenerator.GeneratePagingMethod = false;
						}
					}
				}
			}
		}

		private void AddSecondaryQueriesToDataComponent(CodeTypeDeclaration classDeclaration)
		{
			if (this.designTable == null)
			{
				throw new InternalException("Design Table should not be null.");
			}
			if (this.designTable.Sources == null)
			{
				return;
			}
			QueryGenerator queryGenerator = new QueryGenerator(this.codeGenerator);
			queryGenerator.DeclarationOnly = this.declarationsOnly;
			queryGenerator.DesignTable = this.designTable;
			if (this.designTable.Connection != null)
			{
				queryGenerator.ProviderFactory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
			}
			int num = 1;
			foreach (object obj in this.designTable.Sources)
			{
				Source source = (Source)obj;
				queryGenerator.MethodSource = source as DbSource;
				queryGenerator.CommandIndex = num++;
				if (queryGenerator.MethodSource.QueryType == QueryType.Rowset && queryGenerator.MethodSource.CommandOperation == CommandOperation.Select)
				{
					if (queryGenerator.MethodSource.Connection != null)
					{
						queryGenerator.ProviderFactory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
					}
					if (queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Fill || queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Both)
					{
						queryGenerator.GenerateGetMethod = false;
						queryGenerator.MethodName = source.GeneratorSourceName;
						this.GenerateQueries(classDeclaration, queryGenerator);
						if (queryGenerator.MethodSource.GeneratePagingMethods)
						{
							queryGenerator.MethodName = source.GeneratorSourceNameForPaging;
							queryGenerator.GeneratePagingMethod = true;
							this.GenerateQueries(classDeclaration, queryGenerator);
							queryGenerator.GeneratePagingMethod = false;
						}
					}
					if (queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Get || queryGenerator.MethodSource.GenerateMethods == GenerateMethodTypes.Both)
					{
						queryGenerator.GenerateGetMethod = true;
						queryGenerator.MethodName = source.GeneratorGetMethodName;
						this.GenerateQueries(classDeclaration, queryGenerator);
						if (queryGenerator.MethodSource.GeneratePagingMethods)
						{
							queryGenerator.MethodName = source.GeneratorGetMethodNameForPaging;
							queryGenerator.GeneratePagingMethod = true;
							this.GenerateQueries(classDeclaration, queryGenerator);
							queryGenerator.GeneratePagingMethod = false;
						}
					}
				}
			}
		}

		internal void AddFunctionsToDataComponent(CodeTypeDeclaration classDeclaration, bool isFunctionsDataComponent)
		{
			if (this.designTable == null)
			{
				throw new InternalException("Design Table should not be null.");
			}
			if (!isFunctionsDataComponent && this.designTable.MainSource != null && (((DbSource)this.designTable.MainSource).QueryType != QueryType.Rowset || ((DbSource)this.designTable.MainSource).CommandOperation != CommandOperation.Select))
			{
				this.AddFunctionToDataComponent(classDeclaration, (DbSource)this.designTable.MainSource, 0, isFunctionsDataComponent);
			}
			if (this.designTable.Sources != null)
			{
				int num = 1;
				if (isFunctionsDataComponent)
				{
					num = 0;
				}
				foreach (object obj in this.designTable.Sources)
				{
					Source source = (Source)obj;
					if (((DbSource)source).QueryType != QueryType.Rowset || ((DbSource)source).CommandOperation != CommandOperation.Select)
					{
						this.AddFunctionToDataComponent(classDeclaration, (DbSource)source, num, isFunctionsDataComponent);
					}
					num++;
				}
			}
		}

		private void AddFunctionToDataComponent(CodeTypeDeclaration classDeclaration, DbSource dbSource, int commandIndex, bool isFunctionsDataComponent)
		{
			if (this.DeclarationsOnly && dbSource.Modifier != MemberAttributes.Public)
			{
				return;
			}
			FunctionGenerator functionGenerator = new FunctionGenerator(this.codeGenerator);
			functionGenerator.DeclarationOnly = this.declarationsOnly;
			functionGenerator.MethodSource = dbSource;
			functionGenerator.CommandIndex = commandIndex;
			functionGenerator.DesignTable = this.designTable;
			functionGenerator.IsFunctionsDataComponent = isFunctionsDataComponent;
			if (functionGenerator.MethodSource.Connection != null)
			{
				functionGenerator.ProviderFactory = ProviderManager.GetFactory(functionGenerator.MethodSource.Connection.Provider);
			}
			else
			{
				if (this.designTable.Connection == null)
				{
					return;
				}
				functionGenerator.ProviderFactory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
			}
			functionGenerator.MethodName = dbSource.GeneratorSourceName;
			functionGenerator.ParameterOption = (this.languageSupportsNullables ? ParameterGenerationOption.ClrTypes : ParameterGenerationOption.Objects);
			CodeMemberMethod codeMemberMethod = functionGenerator.Generate();
			if (codeMemberMethod != null)
			{
				classDeclaration.Members.Add(codeMemberMethod);
			}
		}

		private void AddUpdateQueriesToDataComponent(CodeTypeDeclaration classDeclaration)
		{
			this.AddUpdateQueriesToDataComponent(classDeclaration, this.codeGenerator.DataSourceName, this.codeGenerator.CodeProvider);
		}

		internal void AddUpdateQueriesToDataComponent(CodeTypeDeclaration classDeclaration, string dataSourceClassName, CodeDomProvider codeProvider)
		{
			if (this.designTable == null)
			{
				throw new InternalException("Design Table should not be null.");
			}
			if (StringUtil.EmptyOrSpace(dataSourceClassName))
			{
				throw new InternalException("Data source class name should not be empty");
			}
			if (this.designTable.HasAnyUpdateCommand)
			{
				UpdateCommandGenerator updateCommandGenerator = new UpdateCommandGenerator(this.codeGenerator);
				updateCommandGenerator.CodeProvider = codeProvider;
				updateCommandGenerator.DeclarationOnly = this.declarationsOnly;
				updateCommandGenerator.MethodSource = this.designTable.MainSource as DbSource;
				updateCommandGenerator.DesignTable = this.designTable;
				if (this.designTable.Connection != null)
				{
					updateCommandGenerator.ProviderFactory = ProviderManager.GetFactory(this.designTable.Connection.Provider);
				}
				else if (!this.declarationsOnly)
				{
					throw new InternalException("DesignTable.Connection should not be null to generate update query statements.");
				}
				CodeMemberMethod codeMemberMethod = null;
				updateCommandGenerator.MethodName = DataComponentNameHandler.UpdateMethodName;
				updateCommandGenerator.ActiveCommand = updateCommandGenerator.MethodSource.UpdateCommand;
				updateCommandGenerator.MethodType = MethodTypeEnum.GenericUpdate;
				updateCommandGenerator.UpdateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataTable));
				updateCommandGenerator.UpdateParameterName = "dataTable";
				updateCommandGenerator.UpdateParameterTypeName = CodeGenHelper.GetTypeName(codeProvider, dataSourceClassName, this.designTable.GeneratorTableClassName);
				if (this.codeGenerator.DataSetNamespace != null)
				{
					updateCommandGenerator.UpdateParameterTypeName = CodeGenHelper.GetTypeName(this.codeGenerator.CodeProvider, this.codeGenerator.DataSetNamespace, updateCommandGenerator.UpdateParameterTypeName);
				}
				codeMemberMethod = updateCommandGenerator.Generate();
				if (codeMemberMethod != null)
				{
					classDeclaration.Members.Add(codeMemberMethod);
				}
				updateCommandGenerator.UpdateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataSet));
				updateCommandGenerator.UpdateParameterName = "dataSet";
				updateCommandGenerator.UpdateParameterTypeName = dataSourceClassName;
				if (this.codeGenerator.DataSetNamespace != null)
				{
					updateCommandGenerator.UpdateParameterTypeName = CodeGenHelper.GetTypeName(this.codeGenerator.CodeProvider, this.codeGenerator.DataSetNamespace, updateCommandGenerator.UpdateParameterTypeName);
				}
				codeMemberMethod = updateCommandGenerator.Generate();
				if (codeMemberMethod != null)
				{
					classDeclaration.Members.Add(codeMemberMethod);
				}
				updateCommandGenerator.UpdateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataRow));
				updateCommandGenerator.UpdateParameterName = "dataRow";
				updateCommandGenerator.UpdateParameterTypeName = null;
				codeMemberMethod = updateCommandGenerator.Generate();
				if (codeMemberMethod != null)
				{
					classDeclaration.Members.Add(codeMemberMethod);
				}
				updateCommandGenerator.UpdateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataRow), 1);
				updateCommandGenerator.UpdateParameterName = "dataRows";
				updateCommandGenerator.UpdateParameterTypeName = null;
				codeMemberMethod = updateCommandGenerator.Generate();
				if (codeMemberMethod != null)
				{
					classDeclaration.Members.Add(codeMemberMethod);
				}
				if (updateCommandGenerator.MethodSource.GenerateShortCommands)
				{
					updateCommandGenerator.MethodType = MethodTypeEnum.ColumnParameters;
					updateCommandGenerator.ActiveCommand = updateCommandGenerator.MethodSource.DeleteCommand;
					if (updateCommandGenerator.ActiveCommand != null)
					{
						updateCommandGenerator.MethodName = DataComponentNameHandler.DeleteMethodName;
						updateCommandGenerator.UpdateCommandName = "DeleteCommand";
						updateCommandGenerator.ParameterOption = (this.languageSupportsNullables ? ParameterGenerationOption.ClrTypes : ParameterGenerationOption.Objects);
						codeMemberMethod = updateCommandGenerator.Generate();
						if (codeMemberMethod != null)
						{
							classDeclaration.Members.Add(codeMemberMethod);
						}
					}
					updateCommandGenerator.ActiveCommand = updateCommandGenerator.MethodSource.InsertCommand;
					if (updateCommandGenerator.ActiveCommand != null)
					{
						updateCommandGenerator.MethodName = DataComponentNameHandler.InsertMethodName;
						updateCommandGenerator.UpdateCommandName = "InsertCommand";
						updateCommandGenerator.ParameterOption = (this.languageSupportsNullables ? ParameterGenerationOption.ClrTypes : ParameterGenerationOption.Objects);
						codeMemberMethod = updateCommandGenerator.Generate();
						if (codeMemberMethod != null)
						{
							classDeclaration.Members.Add(codeMemberMethod);
						}
					}
					updateCommandGenerator.ActiveCommand = updateCommandGenerator.MethodSource.UpdateCommand;
					if (updateCommandGenerator.ActiveCommand != null)
					{
						updateCommandGenerator.MethodName = DataComponentNameHandler.UpdateMethodName;
						updateCommandGenerator.UpdateCommandName = "UpdateCommand";
						updateCommandGenerator.ParameterOption = (this.languageSupportsNullables ? ParameterGenerationOption.ClrTypes : ParameterGenerationOption.Objects);
						codeMemberMethod = updateCommandGenerator.Generate();
						if (codeMemberMethod != null)
						{
							classDeclaration.Members.Add(codeMemberMethod);
							codeMemberMethod = null;
							updateCommandGenerator.GenerateOverloadWithoutCurrentPKParameters = true;
							try
							{
								codeMemberMethod = updateCommandGenerator.Generate();
							}
							finally
							{
								updateCommandGenerator.GenerateOverloadWithoutCurrentPKParameters = false;
							}
							if (codeMemberMethod != null)
							{
								classDeclaration.Members.Add(codeMemberMethod);
							}
						}
					}
				}
			}
		}

		private void GenerateQueries(CodeTypeDeclaration classDeclaration, QueryGenerator queryGenerator)
		{
			if (queryGenerator.DeclarationOnly)
			{
				if (!queryGenerator.GenerateGetMethod && queryGenerator.MethodSource.Modifier != MemberAttributes.Public)
				{
					return;
				}
				if (queryGenerator.GenerateGetMethod && queryGenerator.MethodSource.GetMethodModifier != MemberAttributes.Public)
				{
					return;
				}
			}
			queryGenerator.ContainerParameterType = typeof(DataTable);
			queryGenerator.ContainerParameterTypeName = CodeGenHelper.GetTypeName(this.codeGenerator.CodeProvider, this.codeGenerator.DataSourceName, this.designTable.GeneratorTableClassName);
			if (this.codeGenerator.DataSetNamespace != null)
			{
				queryGenerator.ContainerParameterTypeName = CodeGenHelper.GetTypeName(this.codeGenerator.CodeProvider, this.codeGenerator.DataSetNamespace, queryGenerator.ContainerParameterTypeName);
			}
			queryGenerator.ContainerParameterName = "dataTable";
			queryGenerator.ParameterOption = (this.languageSupportsNullables ? ParameterGenerationOption.ClrTypes : ParameterGenerationOption.Objects);
			CodeMemberMethod codeMemberMethod = queryGenerator.Generate();
			if (codeMemberMethod != null)
			{
				classDeclaration.Members.Add(codeMemberMethod);
			}
		}

		internal const string tableParameterName = "dataTable";

		internal const string dataSetParameterName = "dataSet";

		private TypedDataSourceCodeGenerator codeGenerator;

		private DesignTable designTable;

		private bool declarationsOnly;

		private bool languageSupportsNullables;
	}
}
