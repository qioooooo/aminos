using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;

namespace System.Data.Design
{
	internal sealed class NameHandler
	{
		internal NameHandler(CodeDomProvider codeProvider)
		{
			if (codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			NameHandler.codeProvider = codeProvider;
		}

		internal void GenerateMemberNames(DesignDataSource dataSource, ArrayList problemList)
		{
			if (dataSource == null || NameHandler.codeProvider == null)
			{
				throw new InternalException("DesignDataSource or/and CodeDomProvider parameters are null.");
			}
			NameHandler.InitLookupIdentifiers();
			this.dataSourceHandler = new DataSourceNameHandler();
			this.dataSourceHandler.GenerateMemberNames(dataSource, NameHandler.codeProvider, this.languageCaseInsensitive, problemList);
			foreach (object obj in dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				DataTableNameHandler dataTableNameHandler = new DataTableNameHandler();
				dataTableNameHandler.GenerateMemberNames(designTable, NameHandler.codeProvider, this.languageCaseInsensitive, problemList);
				DataComponentNameHandler dataComponentNameHandler = new DataComponentNameHandler();
				dataComponentNameHandler.GenerateMemberNames(designTable, NameHandler.codeProvider, this.languageCaseInsensitive, problemList);
			}
			if (dataSource.Sources != null && dataSource.Sources.Count > 0)
			{
				DesignTable designTable2 = new DesignTable();
				designTable2.TableType = TableType.RadTable;
				designTable2.DataAccessorName = dataSource.FunctionsComponentName;
				designTable2.UserDataComponentName = dataSource.UserFunctionsComponentName;
				designTable2.GeneratorDataComponentClassName = dataSource.GeneratorFunctionsComponentClassName;
				foreach (object obj2 in dataSource.Sources)
				{
					Source source = (Source)obj2;
					designTable2.Sources.Add(source);
				}
				new DataComponentNameHandler
				{
					GlobalSources = true
				}.GenerateMemberNames(designTable2, NameHandler.codeProvider, this.languageCaseInsensitive, problemList);
				dataSource.GeneratorFunctionsComponentClassName = designTable2.GeneratorDataComponentClassName;
			}
		}

		internal static string FixIdName(string inVarName)
		{
			if (NameHandler.lookupIdentifiers == null)
			{
				NameHandler.InitLookupIdentifiers();
			}
			string text = (string)NameHandler.lookupIdentifiers[inVarName];
			if (text == null)
			{
				text = MemberNameValidator.GenerateIdName(inVarName, NameHandler.codeProvider, false);
				while (NameHandler.lookupIdentifiers.ContainsValue(text))
				{
					text = '_' + text;
				}
				NameHandler.lookupIdentifiers[inVarName] = text;
			}
			return text;
		}

		private static void InitLookupIdentifiers()
		{
			NameHandler.lookupIdentifiers = new Hashtable();
			PropertyInfo[] properties = typeof(DataRow).GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				NameHandler.lookupIdentifiers[propertyInfo.Name] = '_' + propertyInfo.Name;
			}
		}

		private const string FunctionsTableName = "Queries";

		private DataSourceNameHandler dataSourceHandler;

		private static CodeDomProvider codeProvider;

		private bool languageCaseInsensitive;

		private static Hashtable lookupIdentifiers;
	}
}
