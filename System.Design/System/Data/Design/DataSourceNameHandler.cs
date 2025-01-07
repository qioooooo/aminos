using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Design;

namespace System.Data.Design
{
	internal sealed class DataSourceNameHandler
	{
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

		internal static string TablesPropertyName
		{
			get
			{
				return DataSourceNameHandler.tablesPropertyName;
			}
		}

		internal static string RelationsPropertyName
		{
			get
			{
				return DataSourceNameHandler.relationsPropertyName;
			}
		}

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

		private string TableVariableName(DataTable table, out bool usesAnnotations)
		{
			return "table" + this.TablePropertyName(table, out usesAnnotations);
		}

		private string PlainTableVariableName(DataTable table, out bool usesAnnotations)
		{
			return "table" + this.PlainTablePropertyName(table, out usesAnnotations);
		}

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

		private string RowEventArgClassName(DataTable table, out bool usesAnnotations)
		{
			return this.RowClassName(table, out usesAnnotations) + "ChangeEvent";
		}

		private string PlainRowEventArgClassName(DataTable table, out bool usesAnnotations)
		{
			return this.PlainRowClassName(table, out usesAnnotations) + "ChangeEvent";
		}

		private string RowEventHandlerName(DataTable table, out bool usesAnnotations)
		{
			return this.RowClassName(table, out usesAnnotations) + "ChangeEventHandler";
		}

		private string PlainRowEventHandlerName(DataTable table, out bool usesAnnotations)
		{
			return this.PlainRowClassName(table, out usesAnnotations) + "ChangeEventHandler";
		}

		private string RelationVariableName(DataRelation relation)
		{
			return NameHandler.FixIdName("relation" + relation.RelationName);
		}

		private MemberNameValidator validator;

		private bool languageCaseInsensitive;

		private static string tablesPropertyName = "Tables";

		private static string relationsPropertyName = "Relations";
	}
}
