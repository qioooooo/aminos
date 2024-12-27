using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x020000A9 RID: 169
	internal sealed class NameHandler
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x0001284E File Offset: 0x0001184E
		internal NameHandler(CodeDomProvider codeProvider)
		{
			if (codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			NameHandler.codeProvider = codeProvider;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001286C File Offset: 0x0001186C
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

		// Token: 0x060007EC RID: 2028 RVA: 0x00012A08 File Offset: 0x00011A08
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

		// Token: 0x060007ED RID: 2029 RVA: 0x00012A6C File Offset: 0x00011A6C
		private static void InitLookupIdentifiers()
		{
			NameHandler.lookupIdentifiers = new Hashtable();
			PropertyInfo[] properties = typeof(DataRow).GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				NameHandler.lookupIdentifiers[propertyInfo.Name] = '_' + propertyInfo.Name;
			}
		}

		// Token: 0x04000BCF RID: 3023
		private const string FunctionsTableName = "Queries";

		// Token: 0x04000BD0 RID: 3024
		private DataSourceNameHandler dataSourceHandler;

		// Token: 0x04000BD1 RID: 3025
		private static CodeDomProvider codeProvider;

		// Token: 0x04000BD2 RID: 3026
		private bool languageCaseInsensitive;

		// Token: 0x04000BD3 RID: 3027
		private static Hashtable lookupIdentifiers;
	}
}
