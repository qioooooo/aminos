using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Design;

namespace System.Data.Design
{
	// Token: 0x02000079 RID: 121
	internal sealed class DataSourceNameHandler
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x000089F0 File Offset: 0x000079F0
		internal void GenerateMemberNames(DesignDataSource dataSource, CodeDomProvider codeProvider, bool languageCaseInsensitive, ArrayList problemList)
		{
			this.languageCaseInsensitive = languageCaseInsensitive;
			this.validator = new MemberNameValidator(new string[]
			{
				DataSourceNameHandler.tablesPropertyName,
				DataSourceNameHandler.relationsPropertyName
			}, codeProvider, this.languageCaseInsensitive);
			this.ProcessMemberNames(dataSource);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00008A38 File Offset: 0x00007A38
		internal void ProcessMemberNames(DesignDataSource dataSource)
		{
			this.ProcessDataSourceName(dataSource);
			if (dataSource.DesignTables != null)
			{
				foreach (object obj in dataSource.DesignTables)
				{
					DesignTable designTable = (DesignTable)obj;
					this.ProcessTableRelatedNames(designTable);
				}
			}
			if (dataSource.DesignRelations != null)
			{
				foreach (object obj2 in dataSource.DesignRelations)
				{
					DesignRelation designRelation = (DesignRelation)obj2;
					this.ProcessRelationRelatedNames(designRelation);
				}
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00008AF8 File Offset: 0x00007AF8
		internal void ProcessDataSourceName(DesignDataSource dataSource)
		{
			if (StringUtil.Empty(dataSource.Name))
			{
				throw new DataSourceGeneratorException(SR.GetString("CG_EmptyDSName"));
			}
			if (!StringUtil.EqualValue(dataSource.Name, dataSource.UserDataSetName, this.languageCaseInsensitive) || StringUtil.Empty(dataSource.GeneratorDataSetName))
			{
				dataSource.GeneratorDataSetName = NameHandler.FixIdName(dataSource.Name);
			}
			else
			{
				dataSource.GeneratorDataSetName = this.validator.GenerateIdName(dataSource.GeneratorDataSetName);
			}
			dataSource.UserDataSetName = dataSource.Name;
			if (!StringUtil.EqualValue(NameHandler.FixIdName(dataSource.Name), dataSource.GeneratorDataSetName))
			{
				dataSource.NamingPropertyNames.Add(DesignDataSource.EXTPROPNAME_USER_DATASETNAME);
				dataSource.NamingPropertyNames.Add(DesignDataSource.EXTPROPNAME_GENERATOR_DATASETNAME);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00008BC0 File Offset: 0x00007BC0
		internal void ProcessTableRelatedNames(DesignTable table)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = !StringUtil.EqualValue(table.Name, table.UserTableName, this.languageCaseInsensitive);
			string text = this.TablePropertyName(table.DataTable, out flag);
			string text2 = this.PlainTablePropertyName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorTablePropName = text2;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorTablePropName))
				{
					table.GeneratorTablePropName = this.validator.GenerateIdName(text);
				}
				else
				{
					table.GeneratorTablePropName = this.validator.GenerateIdName(table.GeneratorTablePropName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text), table.GeneratorTablePropName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_TABLEPROPNAME);
					flag2 = true;
				}
			}
			string text3 = this.TableVariableName(table.DataTable, out flag);
			string text4 = this.PlainTableVariableName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorTableVarName = text4;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorTableVarName))
				{
					table.GeneratorTableVarName = this.validator.GenerateIdName(text3);
				}
				else
				{
					table.GeneratorTableVarName = this.validator.GenerateIdName(table.GeneratorTableVarName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text3), table.GeneratorTableVarName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_TABLEVARNAME);
					flag2 = true;
				}
			}
			string text5 = this.TableClassName(table.DataTable, out flag);
			string text6 = this.PlainTableClassName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorTableClassName = text6;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorTableClassName))
				{
					table.GeneratorTableClassName = this.validator.GenerateIdName(text5);
				}
				else
				{
					table.GeneratorTableClassName = this.validator.GenerateIdName(table.GeneratorTableClassName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text5), table.GeneratorTableClassName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_TABLECLASSNAME);
					flag2 = true;
				}
			}
			string text7 = this.RowClassName(table.DataTable, out flag);
			string text8 = this.PlainRowClassName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorRowClassName = text8;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorRowClassName))
				{
					table.GeneratorRowClassName = this.validator.GenerateIdName(text7);
				}
				else
				{
					table.GeneratorRowClassName = this.validator.GenerateIdName(table.GeneratorRowClassName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text7), table.GeneratorRowClassName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWCLASSNAME);
					flag2 = true;
				}
			}
			string text9 = this.RowEventHandlerName(table.DataTable, out flag);
			string text10 = this.PlainRowEventHandlerName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorRowEvHandlerName = text10;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorRowEvHandlerName))
				{
					table.GeneratorRowEvHandlerName = this.validator.GenerateIdName(text9);
				}
				else
				{
					table.GeneratorRowEvHandlerName = this.validator.GenerateIdName(table.GeneratorRowEvHandlerName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text9), table.GeneratorRowEvHandlerName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWEVHANDLERNAME);
					flag2 = true;
				}
			}
			string text11 = this.RowEventArgClassName(table.DataTable, out flag);
			string text12 = this.PlainRowEventArgClassName(table.DataTable, out flag);
			if (flag)
			{
				table.GeneratorRowEvArgName = text12;
			}
			else
			{
				if (flag3 || StringUtil.Empty(table.GeneratorRowEvArgName))
				{
					table.GeneratorRowEvArgName = this.validator.GenerateIdName(text11);
				}
				else
				{
					table.GeneratorRowEvArgName = this.validator.GenerateIdName(table.GeneratorRowEvArgName);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text11), table.GeneratorRowEvArgName))
				{
					table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWEVARGNAME);
					flag2 = true;
				}
			}
			if (flag2)
			{
				table.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_USER_TABLENAME);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00008F84 File Offset: 0x00007F84
		internal void ProcessRelationRelatedNames(DesignRelation relation)
		{
			if (relation.DataRelation == null)
			{
				return;
			}
			if (!StringUtil.EqualValue(relation.Name, relation.UserRelationName, this.languageCaseInsensitive) || StringUtil.Empty(relation.GeneratorRelationVarName))
			{
				relation.GeneratorRelationVarName = this.validator.GenerateIdName(this.RelationVariableName(relation.DataRelation));
				return;
			}
			relation.GeneratorRelationVarName = this.validator.GenerateIdName(relation.GeneratorRelationVarName);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00008FFA File Offset: 0x00007FFA
		internal static string TablesPropertyName
		{
			get
			{
				return DataSourceNameHandler.tablesPropertyName;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00009001 File Offset: 0x00008001
		internal static string RelationsPropertyName
		{
			get
			{
				return DataSourceNameHandler.relationsPropertyName;
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00009008 File Offset: 0x00008008
		private string TableClassName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedPlural"];
			if (StringUtil.Empty(text))
			{
				text = (string)table.ExtendedProperties["typedName"];
				if (StringUtil.Empty(text))
				{
					usesAnnotations = false;
					text = NameHandler.FixIdName(table.TableName);
				}
			}
			return text + "DataTable";
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00009070 File Offset: 0x00008070
		private string PlainTableClassName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedPlural"];
			if (StringUtil.Empty(text))
			{
				text = (string)table.ExtendedProperties["typedName"];
				if (StringUtil.Empty(text))
				{
					usesAnnotations = false;
					text = table.TableName;
				}
			}
			return text + "DataTable";
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000090D4 File Offset: 0x000080D4
		private string TablePropertyName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedPlural"];
			if (StringUtil.Empty(text))
			{
				text = (string)table.ExtendedProperties["typedName"];
				if (StringUtil.Empty(text))
				{
					usesAnnotations = false;
					text = NameHandler.FixIdName(table.TableName);
				}
				else
				{
					text += "Table";
				}
			}
			return text;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00009140 File Offset: 0x00008140
		private string PlainTablePropertyName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedPlural"];
			if (StringUtil.Empty(text))
			{
				text = (string)table.ExtendedProperties["typedName"];
				if (StringUtil.Empty(text))
				{
					usesAnnotations = false;
					text = table.TableName;
				}
				else
				{
					text += "Table";
				}
			}
			return text;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000091A5 File Offset: 0x000081A5
		private string TableVariableName(DataTable table, out bool usesAnnotations)
		{
			return "table" + this.TablePropertyName(table, out usesAnnotations);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000091B9 File Offset: 0x000081B9
		private string PlainTableVariableName(DataTable table, out bool usesAnnotations)
		{
			return "table" + this.PlainTablePropertyName(table, out usesAnnotations);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000091D0 File Offset: 0x000081D0
		private string RowClassName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedName"];
			if (StringUtil.Empty(text))
			{
				usesAnnotations = false;
				text = NameHandler.FixIdName(table.TableName) + "Row";
			}
			return text;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00009218 File Offset: 0x00008218
		private string PlainRowClassName(DataTable table, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)table.ExtendedProperties["typedName"];
			if (StringUtil.Empty(text))
			{
				usesAnnotations = false;
				text = table.TableName + "Row";
			}
			return text;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000925B File Offset: 0x0000825B
		private string RowEventArgClassName(DataTable table, out bool usesAnnotations)
		{
			return this.RowClassName(table, out usesAnnotations) + "ChangeEvent";
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000926F File Offset: 0x0000826F
		private string PlainRowEventArgClassName(DataTable table, out bool usesAnnotations)
		{
			return this.PlainRowClassName(table, out usesAnnotations) + "ChangeEvent";
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00009283 File Offset: 0x00008283
		private string RowEventHandlerName(DataTable table, out bool usesAnnotations)
		{
			return this.RowClassName(table, out usesAnnotations) + "ChangeEventHandler";
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00009297 File Offset: 0x00008297
		private string PlainRowEventHandlerName(DataTable table, out bool usesAnnotations)
		{
			return this.PlainRowClassName(table, out usesAnnotations) + "ChangeEventHandler";
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000092AB File Offset: 0x000082AB
		private string RelationVariableName(DataRelation relation)
		{
			return NameHandler.FixIdName("relation" + relation.RelationName);
		}

		// Token: 0x04000AC3 RID: 2755
		private MemberNameValidator validator;

		// Token: 0x04000AC4 RID: 2756
		private bool languageCaseInsensitive;

		// Token: 0x04000AC5 RID: 2757
		private static string tablesPropertyName = "Tables";

		// Token: 0x04000AC6 RID: 2758
		private static string relationsPropertyName = "Relations";
	}
}
