﻿using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace System.Data.Design
{
	internal sealed class DataTableNameHandler
	{
		internal void GenerateMemberNames(DesignTable designTable, CodeDomProvider codeProvider, bool languageCaseInsensitive, ArrayList problemList)
		{
			this.languageCaseInsensitive = languageCaseInsensitive;
			this.validator = new MemberNameValidator(null, codeProvider, this.languageCaseInsensitive);
			this.AddReservedNames();
			this.ProcessMemberNames(designTable);
		}

		private void AddReservedNames()
		{
			this.validator.GetNewMemberName("OnRowChanging");
			this.validator.GetNewMemberName("OnRowChanged");
			this.validator.GetNewMemberName("OnRowDeleting");
			this.validator.GetNewMemberName("OnRowDeleted");
		}

		private void ProcessMemberNames(DesignTable designTable)
		{
			if (designTable.DesignColumns != null)
			{
				foreach (object obj in designTable.DesignColumns)
				{
					DesignColumn designColumn = (DesignColumn)obj;
					this.ProcessColumnRelatedNames(designColumn);
				}
			}
			DataRelationCollection childRelations = designTable.DataTable.ChildRelations;
			if (childRelations != null)
			{
				foreach (object obj2 in childRelations)
				{
					DataRelation dataRelation = (DataRelation)obj2;
					DesignRelation designRelation = this.FindCorrespondingDesignRelation(designTable, dataRelation);
					this.ProcessChildRelationName(designRelation);
				}
			}
			DataRelationCollection parentRelations = designTable.DataTable.ParentRelations;
			if (parentRelations != null)
			{
				foreach (object obj3 in parentRelations)
				{
					DataRelation dataRelation2 = (DataRelation)obj3;
					DesignRelation designRelation2 = this.FindCorrespondingDesignRelation(designTable, dataRelation2);
					this.ProcessParentRelationName(designRelation2);
				}
			}
			this.ProcessEventNames(designTable);
		}

		private DesignRelation FindCorrespondingDesignRelation(DesignTable designTable, DataRelation relation)
		{
			DesignDataSource owner = designTable.Owner;
			if (owner == null)
			{
				throw new InternalException("Unable to find DataSource for table.");
			}
			foreach (object obj in owner.DesignRelations)
			{
				DesignRelation designRelation = (DesignRelation)obj;
				if (designRelation.DataRelation != null && StringUtil.EqualValue(designRelation.DataRelation.ChildTable.TableName, relation.ChildTable.TableName) && StringUtil.EqualValue(designRelation.DataRelation.ParentTable.TableName, relation.ParentTable.TableName) && StringUtil.EqualValue(designRelation.Name, relation.RelationName))
				{
					return designRelation;
				}
			}
			return null;
		}

		private void ProcessColumnRelatedNames(DesignColumn column)
		{
			bool flag = !StringUtil.EqualValue(column.Name, column.UserColumnName, this.languageCaseInsensitive);
			bool flag2 = false;
			bool flag3 = false;
			string text = this.TableColumnPropertyName(column.DataColumn, out flag2);
			string text2 = this.PlainTableColumnPropertyName(column.DataColumn, out flag2);
			if (flag2)
			{
				column.GeneratorColumnPropNameInTable = text2;
			}
			else
			{
				if (flag || StringUtil.Empty(column.GeneratorColumnPropNameInTable))
				{
					column.GeneratorColumnPropNameInTable = this.validator.GenerateIdName(text);
				}
				else
				{
					column.GeneratorColumnPropNameInTable = this.validator.GenerateIdName(column.GeneratorColumnPropNameInTable);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text), column.GeneratorColumnPropNameInTable))
				{
					column.NamingPropertyNames.Add(DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINTABLE);
					flag3 = true;
				}
			}
			string text3 = this.TableColumnVariableName(column.DataColumn, out flag2);
			string text4 = this.PlainTableColumnVariableName(column.DataColumn, out flag2);
			if (flag2)
			{
				column.GeneratorColumnVarNameInTable = text4;
			}
			else
			{
				if (flag || StringUtil.Empty(column.GeneratorColumnVarNameInTable))
				{
					column.GeneratorColumnVarNameInTable = this.validator.GenerateIdName(text3);
				}
				else
				{
					column.GeneratorColumnVarNameInTable = this.validator.GenerateIdName(column.GeneratorColumnVarNameInTable);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text3), column.GeneratorColumnVarNameInTable))
				{
					column.NamingPropertyNames.Add(DesignColumn.EXTPROPNAME_GENERATOR_COLUMNVARNAMEINTABLE);
					flag3 = true;
				}
			}
			string text5 = this.RowColumnPropertyName(column.DataColumn, out flag2);
			string text6 = this.PlainRowColumnPropertyName(column.DataColumn, out flag2);
			if (flag2)
			{
				column.GeneratorColumnPropNameInRow = text6;
			}
			else
			{
				if (flag || StringUtil.Empty(column.GeneratorColumnPropNameInRow))
				{
					column.GeneratorColumnPropNameInRow = this.validator.GenerateIdName(text5);
				}
				else
				{
					column.GeneratorColumnPropNameInRow = this.validator.GenerateIdName(column.GeneratorColumnPropNameInRow);
				}
				if (!StringUtil.EqualValue(this.validator.GenerateIdName(text5), column.GeneratorColumnPropNameInRow))
				{
					column.NamingPropertyNames.Add(DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINROW);
					flag3 = true;
				}
			}
			column.UserColumnName = column.Name;
			if (flag3)
			{
				column.NamingPropertyNames.Add(DesignColumn.EXTPROPNAME_USER_COLUMNNAME);
			}
		}

		internal void ProcessChildRelationName(DesignRelation relation)
		{
			bool flag = !StringUtil.EqualValue(relation.Name, relation.UserRelationName, this.languageCaseInsensitive) || !StringUtil.EqualValue(relation.ChildDesignTable.Name, relation.UserChildTable, this.languageCaseInsensitive) || !StringUtil.EqualValue(relation.ParentDesignTable.Name, relation.UserParentTable, this.languageCaseInsensitive);
			bool flag2 = false;
			string text = this.ChildPropertyName(relation.DataRelation, out flag2);
			if (flag2)
			{
				relation.GeneratorChildPropName = text;
				return;
			}
			if (flag || StringUtil.Empty(relation.GeneratorChildPropName))
			{
				relation.GeneratorChildPropName = this.validator.GenerateIdName(text);
				return;
			}
			relation.GeneratorChildPropName = this.validator.GenerateIdName(relation.GeneratorChildPropName);
		}

		internal void ProcessParentRelationName(DesignRelation relation)
		{
			bool flag = !StringUtil.EqualValue(relation.Name, relation.UserRelationName, this.languageCaseInsensitive) || !StringUtil.EqualValue(relation.ChildDesignTable.Name, relation.UserChildTable, this.languageCaseInsensitive) || !StringUtil.EqualValue(relation.ParentDesignTable.Name, relation.UserParentTable, this.languageCaseInsensitive);
			bool flag2 = false;
			string text = this.ParentPropertyName(relation.DataRelation, out flag2);
			if (flag2)
			{
				relation.GeneratorParentPropName = text;
				return;
			}
			if (flag || StringUtil.Empty(relation.GeneratorParentPropName))
			{
				relation.GeneratorParentPropName = this.validator.GenerateIdName(text);
				return;
			}
			relation.GeneratorParentPropName = this.validator.GenerateIdName(relation.GeneratorParentPropName);
		}

		internal void ProcessEventNames(DesignTable designTable)
		{
			bool flag = false;
			bool flag2 = !StringUtil.EqualValue(designTable.Name, designTable.UserTableName, this.languageCaseInsensitive);
			string text = designTable.GeneratorRowClassName + "Changing";
			if (flag2 || StringUtil.Empty(designTable.GeneratorRowChangingName))
			{
				designTable.GeneratorRowChangingName = this.validator.GenerateIdName(text);
			}
			else
			{
				designTable.GeneratorRowChangingName = this.validator.GenerateIdName(designTable.GeneratorRowChangingName);
			}
			if (!StringUtil.EqualValue(this.validator.GenerateIdName(text), designTable.GeneratorRowChangingName))
			{
				designTable.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGINGNAME);
				flag = true;
			}
			string text2 = designTable.GeneratorRowClassName + "Changed";
			if (flag2 || StringUtil.Empty(designTable.GeneratorRowChangedName))
			{
				designTable.GeneratorRowChangedName = this.validator.GenerateIdName(text2);
			}
			else
			{
				designTable.GeneratorRowChangedName = this.validator.GenerateIdName(designTable.GeneratorRowChangedName);
			}
			if (!StringUtil.EqualValue(this.validator.GenerateIdName(text2), designTable.GeneratorRowChangedName))
			{
				designTable.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWCHANGEDNAME);
				flag = true;
			}
			string text3 = designTable.GeneratorRowClassName + "Deleting";
			if (flag2 || StringUtil.Empty(designTable.GeneratorRowDeletingName))
			{
				designTable.GeneratorRowDeletingName = this.validator.GenerateIdName(text3);
			}
			else
			{
				designTable.GeneratorRowDeletingName = this.validator.GenerateIdName(designTable.GeneratorRowDeletingName);
			}
			if (!StringUtil.EqualValue(this.validator.GenerateIdName(text3), designTable.GeneratorRowDeletingName))
			{
				designTable.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWDELETINGNAME);
				flag = true;
			}
			string text4 = designTable.GeneratorRowClassName + "Deleted";
			if (flag2 || StringUtil.Empty(designTable.GeneratorRowDeletedName))
			{
				designTable.GeneratorRowDeletedName = this.validator.GenerateIdName(text4);
			}
			else
			{
				designTable.GeneratorRowDeletedName = this.validator.GenerateIdName(designTable.GeneratorRowDeletedName);
			}
			if (!StringUtil.EqualValue(this.validator.GenerateIdName(text4), designTable.GeneratorRowDeletedName))
			{
				designTable.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_GENERATOR_ROWDELETEDNAME);
				flag = true;
			}
			if (flag && !designTable.NamingPropertyNames.Contains(DesignTable.EXTPROPNAME_USER_TABLENAME))
			{
				designTable.NamingPropertyNames.Add(DesignTable.EXTPROPNAME_USER_TABLENAME);
			}
		}

		private string RowColumnPropertyName(DataColumn column, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)column.ExtendedProperties["typedName"];
			if (StringUtil.Empty(text))
			{
				usesAnnotations = false;
				text = NameHandler.FixIdName(column.ColumnName);
			}
			return text;
		}

		private string PlainRowColumnPropertyName(DataColumn column, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)column.ExtendedProperties["typedName"];
			if (StringUtil.Empty(text))
			{
				usesAnnotations = false;
				text = column.ColumnName;
			}
			return text;
		}

		private string TableColumnVariableName(DataColumn column, out bool usesAnnotations)
		{
			string text = this.RowColumnPropertyName(column, out usesAnnotations);
			string text2;
			if (StringUtil.EqualValue("column", text, true))
			{
				text2 = "columnField" + text;
			}
			else
			{
				text2 = "column" + text;
			}
			if (!StringUtil.EqualValue(text2, "Columns", this.languageCaseInsensitive))
			{
				return text2;
			}
			return "_" + text2;
		}

		private string PlainTableColumnVariableName(DataColumn column, out bool usesAnnotations)
		{
			return "column" + this.PlainRowColumnPropertyName(column, out usesAnnotations);
		}

		private string TableColumnPropertyName(DataColumn column, out bool usesAnnotations)
		{
			return this.RowColumnPropertyName(column, out usesAnnotations) + "Column";
		}

		private string PlainTableColumnPropertyName(DataColumn column, out bool usesAnnotations)
		{
			return this.PlainRowColumnPropertyName(column, out usesAnnotations) + "Column";
		}

		private string ChildPropertyName(DataRelation relation, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)relation.ExtendedProperties["typedChildren"];
			if (StringUtil.Empty(text))
			{
				string text2 = (string)relation.ChildTable.ExtendedProperties["typedPlural"];
				if (StringUtil.Empty(text2))
				{
					text2 = (string)relation.ChildTable.ExtendedProperties["typedName"];
					if (StringUtil.Empty(text2))
					{
						usesAnnotations = false;
						text = "Get" + relation.ChildTable.TableName + "Rows";
						if (1 < DataTableNameHandler.TablesConnectedness(relation.ParentTable, relation.ChildTable))
						{
							text = text + "By" + relation.RelationName;
						}
						return NameHandler.FixIdName(text);
					}
					text2 += "Rows";
				}
				text = "Get" + text2;
			}
			return text;
		}

		private string ParentPropertyName(DataRelation relation, out bool usesAnnotations)
		{
			usesAnnotations = true;
			string text = (string)relation.ExtendedProperties["typedParent"];
			if (StringUtil.Empty(text))
			{
				text = this.RowClassName(relation.ParentTable, out usesAnnotations);
				if (relation.ChildTable == relation.ParentTable || relation.ChildColumns.Length != 1)
				{
					text += "Parent";
				}
				if (1 < DataTableNameHandler.TablesConnectedness(relation.ParentTable, relation.ChildTable))
				{
					text = text + "By" + NameHandler.FixIdName(relation.RelationName);
				}
			}
			return text;
		}

		private static int TablesConnectedness(DataTable parentTable, DataTable childTable)
		{
			int num = 0;
			DataRelationCollection parentRelations = childTable.ParentRelations;
			for (int i = 0; i < parentRelations.Count; i++)
			{
				if (parentRelations[i].ParentTable == parentTable)
				{
					num++;
				}
			}
			return num;
		}

		private string RowClassName(DataTable table, out bool usesAnnotations)
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

		private const string onRowChangingMethodName = "OnRowChanging";

		private const string onRowChangedMethodName = "OnRowChanged";

		private const string onRowDeletingMethodName = "OnRowDeleting";

		private const string onRowDeletedMethodName = "OnRowDeleted";

		private MemberNameValidator validator;

		private bool languageCaseInsensitive;
	}
}