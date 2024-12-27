using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000F5 RID: 245
	internal sealed class XmlTreeGen
	{
		// Token: 0x06000E11 RID: 3601 RVA: 0x00205150 File Offset: 0x00204550
		internal XmlTreeGen(SchemaFormat format)
		{
			this.schFormat = format;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00205188 File Offset: 0x00204588
		internal static void AddXdoProperties(object instance, XmlElement root, XmlDocument xd)
		{
			if (instance == null)
			{
				return;
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
			if (!(instance is DataSet) && !(instance is DataTable) && !(instance is DataColumn) && !(instance is DataRelation))
			{
				return;
			}
			for (int i = 0; i < properties.Count; i++)
			{
				XmlTreeGen.AddXdoProperty(properties[i], instance, root, xd);
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x002051E4 File Offset: 0x002045E4
		internal static void AddExtendedProperties(PropertyCollection props, XmlElement node)
		{
			XmlTreeGen.AddExtendedProperties(props, node, null);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x002051FC File Offset: 0x002045FC
		internal static void AddExtendedProperties(PropertyCollection props, XmlElement node, Type type)
		{
			if (props != null)
			{
				foreach (object obj in props)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					if (dictionaryEntry.Key is INullable)
					{
						text = (string)SqlConvert.ChangeTypeForXML(dictionaryEntry.Key, typeof(string));
					}
					else
					{
						text = Convert.ToString(dictionaryEntry.Key, CultureInfo.InvariantCulture);
					}
					string text2;
					if (dictionaryEntry.Value is INullable)
					{
						text2 = (string)SqlConvert.ChangeTypeForXML(dictionaryEntry.Value, typeof(string));
					}
					else
					{
						text2 = Convert.ToString(dictionaryEntry.Value, CultureInfo.InvariantCulture);
					}
					if (type == typeof(DataRelation))
					{
						text = "rel_" + text;
					}
					else if (type == typeof(ForeignKeyConstraint))
					{
						text = "fk_" + text;
					}
					node.SetAttribute(XmlConvert.EncodeLocalName(text), "urn:schemas-microsoft-com:xml-msprop", text2);
				}
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00205328 File Offset: 0x00204728
		internal static void AddXdoProperty(PropertyDescriptor pd, object instance, XmlElement root, XmlDocument xd)
		{
			Type propertyType = pd.PropertyType;
			bool flag = false;
			DataColumn dataColumn = null;
			bool flag2 = false;
			bool flag3 = false;
			if (instance is DataColumn)
			{
				dataColumn = (DataColumn)instance;
				flag = true;
				flag2 = dataColumn.IsSqlType;
				flag3 = dataColumn.ImplementsINullable;
			}
			if (!flag3 && propertyType != typeof(string) && propertyType != typeof(bool) && propertyType != typeof(Type) && propertyType != typeof(object) && propertyType != typeof(CultureInfo) && propertyType != typeof(long) && propertyType != typeof(int))
			{
				return;
			}
			if ((!pd.ShouldSerializeValue(instance) || !pd.Attributes.Contains(DesignerSerializationVisibilityAttribute.Visible)) && !flag2)
			{
				return;
			}
			object value = pd.GetValue(instance);
			if (value is InternalDataCollectionBase)
			{
				return;
			}
			if (value is PropertyCollection)
			{
				return;
			}
			if (string.Compare(pd.Name, "Namespace", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "PrimaryKey", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "ColumnName", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "DefaultValue", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "TableName", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "DataSetName", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "AllowDBNull", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "Unique", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "NestedInDataSet", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "Locale", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "CaseSensitive", StringComparison.Ordinal) == 0 || string.Compare(pd.Name, "RemotingFormat", StringComparison.Ordinal) == 0)
			{
				return;
			}
			if (flag)
			{
				if (string.Compare(pd.Name, "DataType", StringComparison.Ordinal) == 0)
				{
					string text = XmlTreeGen.XmlDataTypeName(dataColumn.DataType);
					if (flag2)
					{
						root.SetAttribute("DataType", "urn:schemas-microsoft-com:xml-msdata", dataColumn.DataType.FullName);
						return;
					}
					if (text.Length == 0 || flag3 || (text == "anyType" && dataColumn.XmlDataType != "anyType") || dataColumn.DataType == typeof(DateTimeOffset))
					{
						root.SetAttribute("DataType", "urn:schemas-microsoft-com:xml-msdata", dataColumn.DataType.AssemblyQualifiedName);
					}
					return;
				}
				else if (string.Compare(pd.Name, "Attribute", StringComparison.Ordinal) == 0)
				{
					return;
				}
			}
			string text2 = pd.Converter.ConvertToString(value);
			root.SetAttribute(pd.Name, "urn:schemas-microsoft-com:xml-msdata", text2);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x002055D0 File Offset: 0x002049D0
		internal static string XmlDataTypeName(Type type)
		{
			if (type == typeof(char))
			{
				return "_";
			}
			if (type == typeof(byte[]) || type == typeof(SqlBytes))
			{
				return "base64Binary";
			}
			if (type == typeof(DateTime) || type == typeof(SqlDateTime))
			{
				return "dateTime";
			}
			if (type == typeof(TimeSpan))
			{
				return "duration";
			}
			if (type == typeof(decimal) || type == typeof(SqlDecimal) || type == typeof(SqlMoney))
			{
				return "decimal";
			}
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(bool) || type == typeof(SqlBoolean))
			{
				return "boolean";
			}
			if (type == typeof(float) || type == typeof(SqlSingle))
			{
				return "float";
			}
			if (type == typeof(double) || type == typeof(SqlDouble))
			{
				return "double";
			}
			if (type == typeof(sbyte) || type == typeof(SqlByte))
			{
				return "byte";
			}
			if (type == typeof(byte))
			{
				return "unsignedByte";
			}
			if (type == typeof(short) || type == typeof(SqlInt16))
			{
				return "short";
			}
			if (type == typeof(int) || type == typeof(SqlInt32))
			{
				return "int";
			}
			if (type == typeof(long) || type == typeof(SqlInt64))
			{
				return "long";
			}
			if (type == typeof(ushort))
			{
				return "unsignedShort";
			}
			if (type == typeof(uint))
			{
				return "unsignedInt";
			}
			if (type == typeof(ulong))
			{
				return "unsignedLong";
			}
			if (type == typeof(Uri))
			{
				return "anyURI";
			}
			if (type == typeof(SqlBinary))
			{
				return "hexBinary";
			}
			if (type == typeof(string) || type == typeof(SqlGuid) || type == typeof(SqlString) || type == typeof(SqlChars))
			{
				return "string";
			}
			if (type == typeof(object) || type == typeof(SqlXml) || type == typeof(DateTimeOffset))
			{
				return "anyType";
			}
			return string.Empty;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00205844 File Offset: 0x00204C44
		private void GenerateConstraintNames(DataTable table, bool fromTable)
		{
			StringBuilder stringBuilder = null;
			foreach (object obj in table.Constraints)
			{
				Constraint constraint = (Constraint)obj;
				if (!fromTable || !(constraint is ForeignKeyConstraint) || this._tables.Contains(((ForeignKeyConstraint)constraint).RelatedTable))
				{
					int num = 0;
					string text = constraint.ConstraintName;
					while (this.ConstraintNames.Contains(text))
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder();
						}
						stringBuilder.Append(table.TableName).Append('_').Append(constraint.ConstraintName);
						if (0 < num)
						{
							stringBuilder.Append('_').Append(num);
						}
						num++;
						text = stringBuilder.ToString();
						stringBuilder.Length = 0;
					}
					this.ConstraintNames.Add(text);
					constraint.SchemaName = text;
				}
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00205954 File Offset: 0x00204D54
		private void GenerateConstraintNames(ArrayList tables)
		{
			for (int i = 0; i < tables.Count; i++)
			{
				this.GenerateConstraintNames((DataTable)tables[i], true);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00205988 File Offset: 0x00204D88
		private void GenerateConstraintNames(DataSet ds)
		{
			foreach (object obj in ds.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				this.GenerateConstraintNames(dataTable, false);
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x002059F0 File Offset: 0x00204DF0
		private static bool _PropsNotEmpty(PropertyCollection props)
		{
			return props != null && props.Count != 0;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00205A10 File Offset: 0x00204E10
		private bool HaveExtendedProperties(DataSet ds)
		{
			if (XmlTreeGen._PropsNotEmpty(ds.extendedProperties))
			{
				return true;
			}
			for (int i = 0; i < ds.Tables.Count; i++)
			{
				DataTable dataTable = ds.Tables[i];
				if (XmlTreeGen._PropsNotEmpty(dataTable.extendedProperties))
				{
					return true;
				}
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					if (XmlTreeGen._PropsNotEmpty(dataTable.Columns[j].extendedProperties))
					{
						return true;
					}
				}
			}
			for (int k = 0; k < ds.Relations.Count; k++)
			{
				if (XmlTreeGen._PropsNotEmpty(ds.Relations[k].extendedProperties))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00205AC0 File Offset: 0x00204EC0
		internal void WriteSchemaRoot(XmlDocument xd, XmlElement rootSchema, string targetNamespace)
		{
			if (!ADP.IsEmpty(targetNamespace))
			{
				rootSchema.SetAttribute("targetNamespace", targetNamespace);
				rootSchema.SetAttribute("xmlns:mstns", targetNamespace);
			}
			rootSchema.SetAttribute("xmlns", targetNamespace);
			rootSchema.SetAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema");
			rootSchema.SetAttribute("xmlns:msdata", "urn:schemas-microsoft-com:xml-msdata");
			if (this._ds != null && this.HaveExtendedProperties(this._ds))
			{
				rootSchema.SetAttribute("xmlns:msprop", "urn:schemas-microsoft-com:xml-msprop");
			}
			if (!ADP.IsEmpty(targetNamespace))
			{
				rootSchema.SetAttribute("attributeFormDefault", "qualified");
				rootSchema.SetAttribute("elementFormDefault", "qualified");
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00205B68 File Offset: 0x00204F68
		internal static void ValidateColumnMapping(Type columnType)
		{
			if (DataStorage.IsTypeCustomType(columnType))
			{
				throw ExceptionBuilder.InvalidDataColumnMapping(columnType);
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00205B84 File Offset: 0x00204F84
		internal void SetupAutoGenerated(DataSet ds)
		{
			foreach (object obj in ds.Tables)
			{
				DataTable dataTable = (DataTable)obj;
				this.SetupAutoGenerated(dataTable);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00205BEC File Offset: 0x00204FEC
		internal void SetupAutoGenerated(ArrayList dt)
		{
			for (int i = 0; i < dt.Count; i++)
			{
				this.SetupAutoGenerated((DataTable)dt[i]);
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00205C1C File Offset: 0x0020501C
		internal void SetupAutoGenerated(DataTable dt)
		{
			foreach (object obj in dt.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (XmlTreeGen.AutoGenerated(dataColumn))
				{
					this.autogenerated[dataColumn] = dataColumn;
				}
			}
			foreach (object obj2 in dt.Constraints)
			{
				Constraint constraint = (Constraint)obj2;
				ForeignKeyConstraint foreignKeyConstraint = constraint as ForeignKeyConstraint;
				if (foreignKeyConstraint != null)
				{
					if (this.AutoGenerated(foreignKeyConstraint))
					{
						this.autogenerated[foreignKeyConstraint] = foreignKeyConstraint;
					}
					else
					{
						if (this.autogenerated[foreignKeyConstraint.Columns[0]] != null)
						{
							this.autogenerated[foreignKeyConstraint.Columns[0]] = null;
						}
						if (this.autogenerated[foreignKeyConstraint.RelatedColumnsReference[0]] != null)
						{
							this.autogenerated[foreignKeyConstraint.RelatedColumnsReference[0]] = null;
						}
						UniqueConstraint uniqueConstraint = (UniqueConstraint)foreignKeyConstraint.RelatedTable.Constraints.FindConstraint(new UniqueConstraint("TEMP", foreignKeyConstraint.RelatedColumnsReference));
						if (uniqueConstraint != null)
						{
							if (this.autogenerated[uniqueConstraint] != null)
							{
								this.autogenerated[uniqueConstraint] = null;
							}
							if (this.autogenerated[uniqueConstraint.Key.ColumnsReference[0]] != null)
							{
								this.autogenerated[uniqueConstraint.Key.ColumnsReference[0]] = null;
							}
						}
					}
				}
				else
				{
					UniqueConstraint uniqueConstraint2 = (UniqueConstraint)constraint;
					if (XmlTreeGen.AutoGenerated(uniqueConstraint2))
					{
						this.autogenerated[uniqueConstraint2] = uniqueConstraint2;
					}
					else if (this.autogenerated[uniqueConstraint2.Key.ColumnsReference[0]] != null)
					{
						this.autogenerated[uniqueConstraint2.Key.ColumnsReference[0]] = null;
					}
				}
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00205E50 File Offset: 0x00205250
		private void CreateTablesHierarchy(DataTable dt)
		{
			foreach (object obj in dt.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (!this._tables.Contains(dataRelation.ChildTable))
				{
					this._tables.Add(dataRelation.ChildTable);
					this.CreateTablesHierarchy(dataRelation.ChildTable);
				}
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00205EE0 File Offset: 0x002052E0
		private void CreateRelations(DataTable dt)
		{
			foreach (object obj in dt.ChildRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (!this._relations.Contains(dataRelation))
				{
					this._relations.Add(dataRelation);
					this.CreateRelations(dataRelation.ChildTable);
				}
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00205F68 File Offset: 0x00205368
		private DataTable[] CreateToplevelTables()
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this._tables.Count; i++)
			{
				DataTable dataTable = (DataTable)this._tables[i];
				if (dataTable.ParentRelations.Count == 0)
				{
					arrayList.Add(dataTable);
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < dataTable.ParentRelations.Count; j++)
					{
						if (dataTable.ParentRelations[j].Nested)
						{
							if (dataTable.ParentRelations[j].ParentTable == dataTable)
							{
								flag = false;
								break;
							}
							flag = true;
						}
					}
					if (!flag)
					{
						arrayList.Add(dataTable);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return new DataTable[0];
			}
			DataTable[] array = new DataTable[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0020603C File Offset: 0x0020543C
		internal void SchemaTree(XmlDocument xd, XmlWriter xmlWriter, DataSet ds, DataTable dt, bool writeHierarchy)
		{
			this.ConstraintNames = new ArrayList();
			this.autogenerated = new Hashtable();
			bool flag = this.filePath != null;
			this.dsElement = xd.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
			bool flag2 = false;
			if (ds != null)
			{
				this._ds = ds;
				using (IEnumerator enumerator = ds.Tables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DataTable dataTable = (DataTable)obj;
						this._tables.Add(dataTable);
					}
					goto IL_00CB;
				}
			}
			if (dt.DataSet != null)
			{
				this._ds = dt.DataSet;
			}
			this._tables.Add(dt);
			if (writeHierarchy)
			{
				this.CreateTablesHierarchy(dt);
			}
			IL_00CB:
			this._dc = xd;
			this.namespaces = new Hashtable();
			this.prefixes = new Hashtable();
			XmlElement xmlElement = xd.CreateElement("xs", "schema", "http://www.w3.org/2001/XMLSchema");
			this._sRoot = xmlElement;
			if (this._ds != null)
			{
				xmlElement.SetAttribute("id", XmlConvert.EncodeLocalName(this._ds.DataSetName));
			}
			else
			{
				xmlElement.SetAttribute("id", XmlConvert.EncodeLocalName("NewDataSet"));
			}
			if (this._ds != null)
			{
				this.WriteSchemaRoot(xd, xmlElement, this._ds.Namespace);
			}
			else
			{
				this.WriteSchemaRoot(xd, xmlElement, dt.Namespace);
			}
			if (this.schFormat == SchemaFormat.Remoting)
			{
				if (this._ds != null)
				{
					this.namespaces[this._ds.Namespace] = xmlElement;
				}
				else
				{
					this.namespaces[dt.Namespace] = xmlElement;
				}
			}
			if (this.schFormat != SchemaFormat.Remoting && this._ds != null)
			{
				this.namespaces[this._ds.Namespace] = xmlElement;
				if (this._ds.Namespace.Length == 0)
				{
					this.prefixes[this._ds.Namespace] = null;
				}
				else
				{
					xmlElement.SetAttribute("xmlns:mstns", this._ds.Namespace);
					this.prefixes[this._ds.Namespace] = "mstns";
				}
			}
			if (ds != null)
			{
				this.GenerateConstraintNames(ds);
			}
			else
			{
				this.GenerateConstraintNames(this._tables);
			}
			if (this.schFormat != SchemaFormat.Remoting)
			{
				if (ds != null)
				{
					this.SetupAutoGenerated(ds);
				}
				else
				{
					this.SetupAutoGenerated(this._tables);
				}
			}
			DataTable[] array = ((ds != null) ? ds.TopLevelTables(true) : this.CreateToplevelTables());
			if (array.Length == 0 || this.schFormat == SchemaFormat.WebServiceSkipSchema || this.schFormat == SchemaFormat.RemotingSkipSchema)
			{
				this.FillDataSetElement(xd, ds, dt);
				xmlElement.AppendChild(this.dsElement);
				XmlTreeGen.AddXdoProperties(this._ds, this.dsElement, xd);
				XmlTreeGen.AddExtendedProperties(ds.extendedProperties, this.dsElement);
				xd.AppendChild(xmlElement);
				xd.Save(xmlWriter);
				xmlWriter.Flush();
				return;
			}
			XmlElement xmlElement2 = this.FillDataSetElement(xd, ds, dt);
			this.constraintSeparator = xd.CreateElement("xs", "SHOULDNOTBEHERE", "http://www.w3.org/2001/XMLSchema");
			this.dsElement.AppendChild(this.constraintSeparator);
			if (this._ds != null)
			{
				XmlTreeGen.AddXdoProperties(this._ds, this.dsElement, xd);
				XmlTreeGen.AddExtendedProperties(this._ds.extendedProperties, this.dsElement);
			}
			for (int i = 0; i < array.Length; i++)
			{
				XmlElement xmlElement3 = this.HandleTable(array[i], xd, xmlElement);
				if ((this._ds != null && this._ds.Namespace == array[i].Namespace) || ADP.IsEmpty(array[i].Namespace) || this.schFormat == SchemaFormat.Remoting)
				{
					bool flag3 = array[i].fNestedInDataset;
					if (this._ds != null && this._ds.Namespace.Length != 0 && ADP.IsEmpty(array[i].Namespace))
					{
						flag3 = true;
					}
					if (array[i].SelfNested)
					{
						flag3 = false;
					}
					if (array[i].NestedParentsCount > 1)
					{
						flag3 = false;
					}
					if (flag3)
					{
						if (array[i].MinOccurs != 1m)
						{
							xmlElement3.SetAttribute("minOccurs", array[i].MinOccurs.ToString(CultureInfo.InvariantCulture));
						}
						if (array[i].MaxOccurs == -1m)
						{
							xmlElement3.SetAttribute("maxOccurs", "unbounded");
						}
						else if (array[i].MaxOccurs != 1m)
						{
							xmlElement3.SetAttribute("maxOccurs", array[i].MaxOccurs.ToString(CultureInfo.InvariantCulture));
						}
					}
					if (!flag3)
					{
						xmlElement.AppendChild(xmlElement3);
						XmlElement xmlElement4 = xd.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
						if ((this._ds != null && this._ds.Namespace == array[i].Namespace) || ADP.IsEmpty(array[i].Namespace) || this.schFormat == SchemaFormat.Remoting)
						{
							xmlElement4.SetAttribute("ref", array[i].EncodedTableName);
						}
						else
						{
							xmlElement4.SetAttribute("ref", (string)this.prefixes[array[i].Namespace] + ':' + array[i].EncodedTableName);
						}
						xmlElement2.AppendChild(xmlElement4);
					}
					else
					{
						xmlElement2.AppendChild(xmlElement3);
					}
				}
				else
				{
					this.AppendChildWithoutRef(xmlElement, array[i].Namespace, xmlElement3, "element");
					XmlElement xmlElement5 = xd.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
					xmlElement5.SetAttribute("ref", (string)this.prefixes[array[i].Namespace] + ':' + array[i].EncodedTableName);
					xmlElement2.AppendChild(xmlElement5);
				}
			}
			this.dsElement.RemoveChild(this.constraintSeparator);
			xmlElement.AppendChild(this.dsElement);
			DataRelation[] array2 = new DataRelation[0];
			if (ds != null && this._tables.Count > 0)
			{
				array2 = new DataRelation[ds.Relations.Count];
				for (int j = 0; j < ds.Relations.Count; j++)
				{
					array2[j] = ds.Relations[j];
				}
			}
			else if (writeHierarchy && this._tables.Count > 0)
			{
				this.CreateRelations((DataTable)this._tables[0]);
				array2 = new DataRelation[this._relations.Count];
				this._relations.CopyTo(array2, 0);
			}
			XmlElement xmlElement6 = null;
			XmlElement xmlElement7 = null;
			foreach (DataRelation dataRelation in array2)
			{
				if ((!dataRelation.Nested || flag2) && dataRelation.ChildKeyConstraint == null)
				{
					if (xmlElement6 == null)
					{
						xmlElement6 = xd.CreateElement("xs", "annotation", "http://www.w3.org/2001/XMLSchema");
						xmlElement.AppendChild(xmlElement6);
						xmlElement7 = xd.CreateElement("xs", "appinfo", "http://www.w3.org/2001/XMLSchema");
						xmlElement6.AppendChild(xmlElement7);
					}
					xmlElement7.AppendChild(this.HandleRelation(dataRelation, xd));
				}
			}
			XmlComment xmlComment = null;
			bool flag4 = this.namespaces.Count > 1 && !flag;
			if (this.schFormat != SchemaFormat.Remoting && this.schFormat != SchemaFormat.RemotingSkipSchema)
			{
				foreach (object obj2 in this.namespaces.Keys)
				{
					string text = (string)obj2;
					if (!(text == ((this._ds != null) ? this._ds.Namespace : dt.Namespace)) && !ADP.IsEmpty(text))
					{
						XmlElement xmlElement8 = xd.CreateElement("xs", "import", "http://www.w3.org/2001/XMLSchema");
						xmlElement8.SetAttribute("namespace", text);
						if (this.schFormat != SchemaFormat.WebService && !flag4)
						{
							xmlElement8.SetAttribute("schemaLocation", string.Concat(new object[]
							{
								this.fileName,
								"_",
								this.prefixes[text],
								".xsd"
							}));
						}
						xmlElement.PrependChild(xmlElement8);
					}
				}
				if (this.schFormat != SchemaFormat.WebService && flag4)
				{
					xmlElement.SetAttribute("schemafragmentcount", "urn:schemas-microsoft-com:xml-msdata", this.namespaces.Count.ToString(CultureInfo.InvariantCulture));
				}
				xd.AppendChild(xmlElement);
				if (this.schFormat != SchemaFormat.WebService && flag4)
				{
					xd.WriteTo(xmlWriter);
				}
				else
				{
					xd.Save(xmlWriter);
				}
				xd.RemoveChild(xmlElement);
				using (IEnumerator enumerator3 = this.namespaces.Keys.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						string text2 = (string)obj3;
						if (!(text2 == ((this._ds != null) ? this._ds.Namespace : dt.Namespace)) && !ADP.IsEmpty(text2))
						{
							XmlWriter xmlWriter2 = null;
							if (!flag)
							{
								xmlWriter2 = xmlWriter;
							}
							else
							{
								xmlWriter2 = new XmlTextWriter(string.Concat(new object[]
								{
									this.filePath,
									this.fileName,
									"_",
									this.prefixes[text2],
									".xsd"
								}), null);
							}
							try
							{
								if (flag)
								{
									if (xmlWriter2 is XmlTextWriter)
									{
										((XmlTextWriter)xmlWriter2).Formatting = Formatting.Indented;
									}
									xmlWriter2.WriteStartDocument(true);
								}
								XmlElement xmlElement9 = (XmlElement)this.namespaces[text2];
								this._dc.AppendChild(xmlElement9);
								foreach (object obj4 in this.namespaces.Keys)
								{
									string text3 = (string)obj4;
									if (!(text2 == text3))
									{
										string text4 = (string)this.prefixes[text3];
										if (text4 != null)
										{
											xmlElement9.SetAttribute("xmlns:" + text4, text3);
											XmlElement xmlElement10 = this._dc.CreateElement("xs", "import", "http://www.w3.org/2001/XMLSchema");
											xmlElement10.SetAttribute("namespace", text3);
											if (this.schFormat != SchemaFormat.WebService && !flag4)
											{
												if (text3 == ((this._ds != null) ? this._ds.Namespace : dt.Namespace))
												{
													xmlElement10.SetAttribute("schemaLocation", this.fileName + this.fileExt);
												}
												else
												{
													xmlElement10.SetAttribute("schemaLocation", this.fileName + "_" + text4 + ".xsd");
												}
											}
											xmlElement9.PrependChild(xmlElement10);
										}
									}
								}
								if (this.schFormat != SchemaFormat.WebService && flag4)
								{
									this._dc.WriteTo(xmlWriter2);
								}
								else
								{
									this._dc.Save(xmlWriter2);
								}
								this._dc.RemoveChild(xmlElement9);
								if (flag)
								{
									xmlWriter2.WriteEndDocument();
								}
							}
							finally
							{
								if (flag)
								{
									xmlWriter2.Close();
								}
							}
						}
					}
					goto IL_0B58;
				}
			}
			xd.AppendChild(xmlElement);
			xd.Save(xmlWriter);
			IL_0B58:
			if (xmlComment != null)
			{
				xmlElement.PrependChild(xmlComment);
			}
			if (!flag)
			{
				xmlWriter.Flush();
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00206C34 File Offset: 0x00206034
		internal XmlElement SchemaTree(XmlDocument xd, DataTable dt)
		{
			this.dsElement = xd.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
			this.ConstraintNames = new ArrayList();
			this._ds = dt.DataSet;
			this._dc = xd;
			this.namespaces = new Hashtable();
			this.prefixes = new Hashtable();
			if (this.schFormat != SchemaFormat.Remoting)
			{
				this.autogenerated = new Hashtable();
			}
			XmlElement xmlElement = xd.CreateElement("xs", "schema", "http://www.w3.org/2001/XMLSchema");
			this._sRoot = xmlElement;
			this.WriteSchemaRoot(xd, xmlElement, dt.Namespace);
			this.FillDataSetElement(xd, null, dt);
			this.constraintSeparator = xd.CreateElement("xs", "SHOULDNOTBEHERE", "http://www.w3.org/2001/XMLSchema");
			this.dsElement.AppendChild(this.constraintSeparator);
			if (this.schFormat != SchemaFormat.Remoting)
			{
				if (this._ds != null)
				{
					this.namespaces[this._ds.Namespace] = xmlElement;
					if (this._ds.Namespace.Length == 0)
					{
						this.prefixes[this._ds.Namespace] = null;
					}
					else
					{
						xmlElement.SetAttribute("xmlns:mstns", this._ds.Namespace);
						this.prefixes[this._ds.Namespace] = "mstns";
					}
				}
				else
				{
					this.namespaces[dt.Namespace] = xmlElement;
					if (dt.Namespace.Length == 0)
					{
						this.prefixes[dt.Namespace] = null;
					}
					else
					{
						xmlElement.SetAttribute("xmlns:mstns", dt.Namespace);
						this.prefixes[dt.Namespace] = "mstns";
					}
				}
			}
			this.GenerateConstraintNames(dt, true);
			XmlElement xmlElement2 = this.HandleTable(dt, xd, xmlElement, false);
			xmlElement.AppendChild(xmlElement2);
			this.dsElement.RemoveChild(this.constraintSeparator);
			xmlElement.AppendChild(this.dsElement);
			return xmlElement;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00206E2C File Offset: 0x0020622C
		internal XmlElement FillDataSetElement(XmlDocument xd, DataSet ds, DataTable dt)
		{
			DataSet dataSet = ((ds != null) ? ds : dt.DataSet);
			if (dataSet != null)
			{
				this.dsElement.SetAttribute("name", XmlConvert.EncodeLocalName(dataSet.DataSetName));
				this.dsElement.SetAttribute("IsDataSet", "urn:schemas-microsoft-com:xml-msdata", "true");
				if (ds == null)
				{
					this.dsElement.SetAttribute("MainDataTable", "urn:schemas-microsoft-com:xml-msdata", XmlConvert.EncodeLocalName((dt.Namespace.Length == 0) ? dt.TableName : (dt.Namespace + ":" + dt.TableName)));
				}
				if (dataSet.CaseSensitive)
				{
					this.dsElement.SetAttribute("CaseSensitive", "urn:schemas-microsoft-com:xml-msdata", "true");
				}
				if (dataSet.ShouldSerializeLocale() || !dataSet.Locale.Equals(CultureInfo.CurrentCulture))
				{
					this.dsElement.SetAttribute("Locale", "urn:schemas-microsoft-com:xml-msdata", dataSet.Locale.ToString());
				}
				else
				{
					this.dsElement.SetAttribute("UseCurrentLocale", "urn:schemas-microsoft-com:xml-msdata", "true");
				}
			}
			else if (dt != null)
			{
				this.dsElement.SetAttribute("name", XmlConvert.EncodeLocalName("NewDataSet"));
				this.dsElement.SetAttribute("IsDataSet", "urn:schemas-microsoft-com:xml-msdata", "true");
				this.dsElement.SetAttribute("MainDataTable", "urn:schemas-microsoft-com:xml-msdata", XmlConvert.EncodeLocalName((dt.Namespace.Length == 0) ? dt.TableName : (dt.Namespace + ":" + dt.TableName)));
				if (dt.CaseSensitive)
				{
					this.dsElement.SetAttribute("CaseSensitive", "urn:schemas-microsoft-com:xml-msdata", "true");
				}
				if (dt.ShouldSerializeLocale() || !dt.Locale.Equals(CultureInfo.CurrentCulture))
				{
					this.dsElement.SetAttribute("Locale", "urn:schemas-microsoft-com:xml-msdata", dt.Locale.ToString());
				}
				else
				{
					this.dsElement.SetAttribute("UseCurrentLocale", "urn:schemas-microsoft-com:xml-msdata", "true");
				}
			}
			XmlElement xmlElement = xd.CreateElement("xs", "complexType", "http://www.w3.org/2001/XMLSchema");
			this.dsElement.AppendChild(xmlElement);
			XmlElement xmlElement2 = xd.CreateElement("xs", "choice", "http://www.w3.org/2001/XMLSchema");
			xmlElement2.SetAttribute("minOccurs", "0");
			xmlElement2.SetAttribute("maxOccurs", "unbounded");
			xmlElement.AppendChild(xmlElement2);
			return xmlElement2;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x002070AC File Offset: 0x002064AC
		internal void SetPath(XmlWriter xw)
		{
			DataTextWriter dataTextWriter = xw as DataTextWriter;
			FileStream fileStream = ((dataTextWriter != null) ? (dataTextWriter.BaseStream as FileStream) : null);
			if (fileStream == null)
			{
				XmlTextWriter xmlTextWriter = xw as XmlTextWriter;
				if (xmlTextWriter == null)
				{
					return;
				}
				fileStream = xmlTextWriter.BaseStream as FileStream;
				if (fileStream == null)
				{
					return;
				}
			}
			this.filePath = Path.GetDirectoryName(fileStream.Name);
			this.fileName = Path.GetFileNameWithoutExtension(fileStream.Name);
			this.fileExt = Path.GetExtension(fileStream.Name);
			if (!ADP.IsEmpty(this.filePath))
			{
				this.filePath += "\\";
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00207148 File Offset: 0x00206548
		internal void Save(DataSet ds, XmlWriter xw)
		{
			this.Save(ds, null, xw);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00207160 File Offset: 0x00206560
		internal void Save(DataTable dt, XmlWriter xw)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (this.schFormat == SchemaFormat.Public)
			{
				this.SetPath(xw);
			}
			XmlElement xmlElement = this.SchemaTree(xmlDocument, dt);
			xmlDocument.AppendChild(xmlElement);
			xmlDocument.Save(xw);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0020719C File Offset: 0x0020659C
		internal void Save(DataSet ds, DataTable dt, XmlWriter xw)
		{
			this.Save(ds, dt, xw, false);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x002071B4 File Offset: 0x002065B4
		internal void Save(DataSet ds, DataTable dt, XmlWriter xw, bool writeHierarchy)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (this.schFormat == SchemaFormat.Public)
			{
				this.SetPath(xw);
			}
			if (this.schFormat == SchemaFormat.WebServiceSkipSchema && xw.WriteState == WriteState.Element)
			{
				xw.WriteAttributeString("msdata", "SchemaSerializationMode", "urn:schemas-microsoft-com:xml-msdata", "ExcludeSchema");
			}
			this.SchemaTree(xmlDocument, xw, ds, dt, writeHierarchy);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00207210 File Offset: 0x00206610
		internal XmlElement HandleRelation(DataRelation rel, XmlDocument dc)
		{
			XmlElement xmlElement = dc.CreateElement("msdata", "Relationship", "urn:schemas-microsoft-com:xml-msdata");
			xmlElement.SetAttribute("name", XmlConvert.EncodeLocalName(rel.RelationName));
			xmlElement.SetAttribute("parent", "urn:schemas-microsoft-com:xml-msdata", rel.ParentKey.Table.EncodedTableName);
			xmlElement.SetAttribute("child", "urn:schemas-microsoft-com:xml-msdata", rel.ChildKey.Table.EncodedTableName);
			if (this._ds == null || this._ds.Tables.InternalIndexOf(rel.ParentKey.Table.TableName) == -3)
			{
				xmlElement.SetAttribute("ParentTableNamespace", "urn:schemas-microsoft-com:xml-msdata", rel.ParentKey.Table.Namespace);
			}
			if (this._ds == null || this._ds.Tables.InternalIndexOf(rel.ChildKey.Table.TableName) == -3)
			{
				xmlElement.SetAttribute("ChildTableNamespace", "urn:schemas-microsoft-com:xml-msdata", rel.ChildKey.Table.Namespace);
			}
			DataColumn[] array = rel.ParentKey.ColumnsReference;
			string text = array[0].EncodedColumnName;
			StringBuilder stringBuilder = null;
			if (1 < array.Length)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append(text);
				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.Append(' ').Append(array[i].EncodedColumnName);
				}
				text = stringBuilder.ToString();
			}
			xmlElement.SetAttribute("parentkey", "urn:schemas-microsoft-com:xml-msdata", text);
			array = rel.ChildKey.ColumnsReference;
			text = array[0].EncodedColumnName;
			if (1 < array.Length)
			{
				if (stringBuilder != null)
				{
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder = new StringBuilder();
				}
				stringBuilder.Append(text);
				for (int j = 1; j < array.Length; j++)
				{
					stringBuilder.Append(' ').Append(array[j].EncodedColumnName);
				}
				text = stringBuilder.ToString();
			}
			xmlElement.SetAttribute("childkey", "urn:schemas-microsoft-com:xml-msdata", text);
			XmlTreeGen.AddExtendedProperties(rel.extendedProperties, xmlElement);
			return xmlElement;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00207438 File Offset: 0x00206838
		private static XmlElement FindSimpleType(XmlElement schema, string name)
		{
			for (XmlNode xmlNode = schema.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.GetAttribute("name") == name)
					{
						return xmlElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00207480 File Offset: 0x00206880
		internal XmlElement GetSchema(string NamespaceURI)
		{
			XmlElement xmlElement = (XmlElement)this.namespaces[NamespaceURI];
			if (xmlElement == null)
			{
				xmlElement = this._dc.CreateElement("xs", "schema", "http://www.w3.org/2001/XMLSchema");
				this.WriteSchemaRoot(this._dc, xmlElement, NamespaceURI);
				if (!ADP.IsEmpty(NamespaceURI))
				{
					string text = "app" + Convert.ToString(++this.prefixCount, CultureInfo.InvariantCulture);
					this._sRoot.SetAttribute("xmlns:" + text, NamespaceURI);
					xmlElement.SetAttribute("xmlns:" + text, NamespaceURI);
					this.prefixes[NamespaceURI] = text;
				}
				this.namespaces[NamespaceURI] = xmlElement;
			}
			return xmlElement;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00207540 File Offset: 0x00206940
		internal void HandleColumnType(DataColumn col, XmlDocument dc, XmlElement root, XmlElement schema)
		{
			string text = "type";
			if (col.ColumnMapping == MappingType.SimpleContent)
			{
				text = "base";
			}
			if (col.SimpleType != null)
			{
				for (SimpleType simpleType = col.SimpleType; simpleType != null; simpleType = simpleType.BaseSimpleType)
				{
					string name = simpleType.Name;
					if (name != null && name.Length != 0)
					{
						string text2 = ((this.schFormat != SchemaFormat.Remoting) ? simpleType.Namespace : ((col.Table.DataSet != null) ? col.Table.DataSet.Namespace : col.Table.Namespace));
						XmlElement schema2 = this.GetSchema(text2);
						if (simpleType.BaseSimpleType != null && simpleType.BaseSimpleType.Namespace != null && simpleType.BaseSimpleType.Namespace.Length > 0)
						{
							this.GetSchema(simpleType.BaseSimpleType.Namespace);
						}
						XmlNode xmlNode = simpleType.ToNode(dc, this.prefixes, this.schFormat == SchemaFormat.Remoting);
						if (simpleType == col.SimpleType)
						{
							string text3 = (string)this.prefixes[text2];
							if (text3 != null && text3.Length > 0)
							{
								if (this.schFormat != SchemaFormat.Remoting)
								{
									root.SetAttribute(text, text3 + ":" + name);
								}
								else
								{
									root.SetAttribute(text, name);
								}
							}
							else
							{
								root.SetAttribute(text, name);
							}
						}
						if (XmlTreeGen.FindSimpleType(schema2, name) == null)
						{
							schema2.AppendChild(xmlNode);
						}
					}
					else
					{
						if (simpleType.BaseSimpleType != null && simpleType.BaseSimpleType.Namespace != null && simpleType.BaseSimpleType.Namespace.Length > 0)
						{
							this.GetSchema(simpleType.BaseSimpleType.Namespace);
						}
						XmlNode xmlNode = simpleType.ToNode(dc, this.prefixes, this.schFormat == SchemaFormat.Remoting);
						root.AppendChild(xmlNode);
					}
				}
				return;
			}
			if (col.XmlDataType != null && col.XmlDataType.Length != 0 && XSDSchema.IsXsdType(col.XmlDataType))
			{
				root.SetAttribute(text, XSDSchema.QualifiedName(col.XmlDataType));
				return;
			}
			string text4 = XmlTreeGen.XmlDataTypeName(col.DataType);
			if (text4 == null || text4.Length == 0)
			{
				if (col.DataType == typeof(Guid) || col.DataType == typeof(Type))
				{
					text4 = "string";
				}
				else
				{
					if (col.ColumnMapping == MappingType.Attribute)
					{
						XmlTreeGen.ValidateColumnMapping(col.DataType);
					}
					text4 = "anyType";
				}
			}
			root.SetAttribute(text, XSDSchema.QualifiedName(text4));
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x002077A8 File Offset: 0x00206BA8
		internal void AddColumnProperties(DataColumn col, XmlElement root)
		{
			if (col.DataType != typeof(string))
			{
				string text = XmlTreeGen.XmlDataTypeName(col.DataType);
				if ((col.IsSqlType && (text.Length == 0 || col.ImplementsINullable)) || typeof(SqlXml) == col.DataType || col.DataType == typeof(DateTimeOffset))
				{
					root.SetAttribute("DataType", "urn:schemas-microsoft-com:xml-msdata", col.DataType.FullName);
				}
				else if (text.Length == 0 || col.ImplementsINullable || (text == "anyType" && col.XmlDataType != "anyType"))
				{
					root.SetAttribute("DataType", "urn:schemas-microsoft-com:xml-msdata", col.DataType.AssemblyQualifiedName);
				}
			}
			if (col.ReadOnly)
			{
				root.SetAttribute("ReadOnly", "urn:schemas-microsoft-com:xml-msdata", "true");
			}
			if (col.Expression.Length != 0)
			{
				root.SetAttribute("Expression", "urn:schemas-microsoft-com:xml-msdata", col.Expression);
			}
			if (col.AutoIncrement)
			{
				root.SetAttribute("AutoIncrement", "urn:schemas-microsoft-com:xml-msdata", "true");
			}
			if (col.AutoIncrementSeed != 0L)
			{
				root.SetAttribute("AutoIncrementSeed", "urn:schemas-microsoft-com:xml-msdata", col.AutoIncrementSeed.ToString(CultureInfo.InvariantCulture));
			}
			if (col.AutoIncrementStep != 1L)
			{
				root.SetAttribute("AutoIncrementStep", "urn:schemas-microsoft-com:xml-msdata", col.AutoIncrementStep.ToString(CultureInfo.InvariantCulture));
			}
			if (col.Caption != col.ColumnName)
			{
				root.SetAttribute("Caption", "urn:schemas-microsoft-com:xml-msdata", col.Caption);
			}
			if (col.Prefix.Length != 0)
			{
				root.SetAttribute("Prefix", "urn:schemas-microsoft-com:xml-msdata", col.Prefix);
			}
			if (col.DataType == typeof(DateTime) && col.DateTimeMode != DataSetDateTime.UnspecifiedLocal)
			{
				root.SetAttribute("DateTimeMode", "urn:schemas-microsoft-com:xml-msdata", col.DateTimeMode.ToString());
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x002079C4 File Offset: 0x00206DC4
		private string FindTargetNamespace(DataTable table)
		{
			string text = (table.TypeName.IsEmpty ? table.Namespace : table.TypeName.Namespace);
			if (ADP.IsEmpty(text))
			{
				DataRelation[] nestedParentRelations = table.NestedParentRelations;
				if (nestedParentRelations.Length != 0)
				{
					for (int i = 0; i < nestedParentRelations.Length; i++)
					{
						DataTable parentTable = nestedParentRelations[i].ParentTable;
						if (table != parentTable)
						{
							text = this.FindTargetNamespace(parentTable);
							if (!ADP.IsEmpty(text))
							{
								break;
							}
						}
					}
				}
				else
				{
					text = this._ds.Namespace;
				}
			}
			return text;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00207A40 File Offset: 0x00206E40
		internal XmlElement HandleColumn(DataColumn col, XmlDocument dc, XmlElement schema, bool fWriteOrdinal)
		{
			string text = ((col.ColumnMapping != MappingType.Element) ? "attribute" : "element");
			XmlElement xmlElement = dc.CreateElement("xs", text, "http://www.w3.org/2001/XMLSchema");
			xmlElement.SetAttribute("name", col.EncodedColumnName);
			if (col.Namespace.Length == 0)
			{
				DataTable table = col.Table;
				string text2 = this.FindTargetNamespace(table);
				if (col.Namespace != text2)
				{
					xmlElement.SetAttribute("form", "unqualified");
				}
			}
			if (col.GetType() != typeof(DataColumn))
			{
				XmlTreeGen.AddXdoProperties(col, xmlElement, dc);
			}
			else
			{
				this.AddColumnProperties(col, xmlElement);
			}
			XmlTreeGen.AddExtendedProperties(col.extendedProperties, xmlElement);
			this.HandleColumnType(col, dc, xmlElement, schema);
			if (col.ColumnMapping == MappingType.Hidden)
			{
				if (!col.AllowDBNull)
				{
					xmlElement.SetAttribute("AllowDBNull", "urn:schemas-microsoft-com:xml-msdata", "false");
				}
				if (!col.DefaultValueIsNull)
				{
					if (col.DataType == typeof(bool))
					{
						xmlElement.SetAttribute("DefaultValue", "urn:schemas-microsoft-com:xml-msdata", ((bool)col.DefaultValue) ? "true" : "false");
					}
					else
					{
						XmlTreeGen.ValidateColumnMapping(col.DataType);
						xmlElement.SetAttribute("DefaultValue", "urn:schemas-microsoft-com:xml-msdata", col.ConvertObjectToXml(col.DefaultValue));
					}
				}
			}
			if (!col.DefaultValueIsNull && col.ColumnMapping != MappingType.Hidden)
			{
				XmlTreeGen.ValidateColumnMapping(col.DataType);
				if (col.ColumnMapping == MappingType.Attribute && !col.AllowDBNull)
				{
					if (col.DataType == typeof(bool))
					{
						xmlElement.SetAttribute("DefaultValue", "urn:schemas-microsoft-com:xml-msdata", ((bool)col.DefaultValue) ? "true" : "false");
					}
					else
					{
						xmlElement.SetAttribute("DefaultValue", "urn:schemas-microsoft-com:xml-msdata", col.ConvertObjectToXml(col.DefaultValue));
					}
				}
				else if (col.DataType == typeof(bool))
				{
					xmlElement.SetAttribute("default", ((bool)col.DefaultValue) ? "true" : "false");
				}
				else if (!col.IsCustomType)
				{
					xmlElement.SetAttribute("default", col.ConvertObjectToXml(col.DefaultValue));
				}
			}
			if (this.schFormat == SchemaFormat.Remoting)
			{
				xmlElement.SetAttribute("targetNamespace", "urn:schemas-microsoft-com:xml-msdata", col.Namespace);
			}
			else if (col.Namespace != (col.Table.TypeName.IsEmpty ? col.Table.Namespace : col.Table.TypeName.Namespace) && col.Namespace.Length != 0)
			{
				XmlElement schema2 = this.GetSchema(col.Namespace);
				if (this.FindTypeNode(schema2, col.EncodedColumnName) == null)
				{
					schema2.AppendChild(xmlElement);
				}
				xmlElement = this._dc.CreateElement("xs", text, "http://www.w3.org/2001/XMLSchema");
				xmlElement.SetAttribute("ref", this.prefixes[col.Namespace] + ":" + col.EncodedColumnName);
				if (col.Table.Namespace != this._ds.Namespace)
				{
					string text3 = (string)this.prefixes[col.Namespace];
					this.GetSchema(col.Table.Namespace);
				}
			}
			int num = (col.AllowDBNull ? 0 : 1);
			if (col.ColumnMapping == MappingType.Attribute && num != 0)
			{
				xmlElement.SetAttribute("use", "required");
			}
			if (col.ColumnMapping == MappingType.Hidden)
			{
				xmlElement.SetAttribute("use", "prohibited");
			}
			else if (col.ColumnMapping != MappingType.Attribute && num != 1)
			{
				xmlElement.SetAttribute("minOccurs", num.ToString(CultureInfo.InvariantCulture));
			}
			if (col.ColumnMapping == MappingType.Element && fWriteOrdinal)
			{
				xmlElement.SetAttribute("Ordinal", "urn:schemas-microsoft-com:xml-msdata", col.Ordinal.ToString(CultureInfo.InvariantCulture));
			}
			return xmlElement;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00207E3C File Offset: 0x0020723C
		internal static string TranslateAcceptRejectRule(AcceptRejectRule rule)
		{
			switch (rule)
			{
			case AcceptRejectRule.None:
				return "None";
			case AcceptRejectRule.Cascade:
				return "Cascade";
			default:
				return null;
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00207E68 File Offset: 0x00207268
		internal static string TranslateRule(Rule rule)
		{
			switch (rule)
			{
			case Rule.None:
				return "None";
			case Rule.Cascade:
				return "Cascade";
			case Rule.SetNull:
				return "SetNull";
			case Rule.SetDefault:
				return "SetDefault";
			default:
				return null;
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00207EA8 File Offset: 0x002072A8
		internal void AppendChildWithoutRef(XmlElement node, string Namespace, XmlElement el, string refString)
		{
			XmlElement schema = this.GetSchema(Namespace);
			if (this.FindTypeNode(schema, el.GetAttribute("name")) == null)
			{
				schema.AppendChild(el);
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00207EDC File Offset: 0x002072DC
		internal XmlElement FindTypeNode(XmlElement node, string strType)
		{
			if (node == null)
			{
				return null;
			}
			for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if ((XMLSchema.FEqualIdentity(xmlElement, "element", "http://www.w3.org/2001/XMLSchema") || XMLSchema.FEqualIdentity(xmlElement, "attribute", "http://www.w3.org/2001/XMLSchema") || XMLSchema.FEqualIdentity(xmlElement, "complexType", "http://www.w3.org/2001/XMLSchema") || XMLSchema.FEqualIdentity(xmlElement, "simpleType", "http://www.w3.org/2001/XMLSchema")) && xmlElement.GetAttribute("name") == strType)
					{
						return xmlElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00207F70 File Offset: 0x00207370
		internal XmlElement HandleTable(DataTable table, XmlDocument dc, XmlElement schema)
		{
			return this.HandleTable(table, dc, schema, true);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00207F88 File Offset: 0x00207388
		private bool HasMixedColumns(DataTable table)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (object obj in table.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (!flag2 && dataColumn.ColumnMapping == MappingType.Element)
				{
					flag2 = true;
				}
				if (!flag && (dataColumn.ColumnMapping == MappingType.Attribute || dataColumn.ColumnMapping == MappingType.Hidden))
				{
					flag = !XmlTreeGen.AutoGenerated(dataColumn);
				}
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0020802C File Offset: 0x0020742C
		internal static bool AutoGenerated(DataColumn col)
		{
			if (col.ColumnMapping != MappingType.Hidden)
			{
				return false;
			}
			if (col.DataType != typeof(int))
			{
				return false;
			}
			string text = col.Table.TableName + "_Id";
			if (col.ColumnName == text || col.ColumnName == text + "_0")
			{
				return true;
			}
			text = "";
			foreach (object obj in col.Table.ParentRelations)
			{
				DataRelation dataRelation = (DataRelation)obj;
				if (dataRelation.Nested && dataRelation.ChildColumnsReference.Length == 1 && dataRelation.ChildColumnsReference[0] == col && dataRelation.ParentColumnsReference.Length == 1)
				{
					text = dataRelation.ParentColumnsReference[0].Table.TableName + "_Id";
				}
			}
			return col.ColumnName == text || col.ColumnName == text + "_0";
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00208164 File Offset: 0x00207564
		internal static bool AutoGenerated(DataRelation rel)
		{
			string text = rel.ParentTable.TableName + "_" + rel.ChildTable.TableName;
			return rel.RelationName.StartsWith(text, StringComparison.Ordinal) && rel.ExtendedProperties.Count <= 0;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x002081B4 File Offset: 0x002075B4
		internal static bool AutoGenerated(UniqueConstraint unique)
		{
			return unique.ConstraintName.StartsWith("Constraint", StringComparison.Ordinal) && unique.Key.ColumnsReference.Length == 1 && unique.ExtendedProperties.Count <= 0 && XmlTreeGen.AutoGenerated(unique.Key.ColumnsReference[0]);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00208210 File Offset: 0x00207610
		private bool AutoGenerated(ForeignKeyConstraint fk)
		{
			return XmlTreeGen.AutoGenerated(fk, true);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00208224 File Offset: 0x00207624
		internal static bool AutoGenerated(ForeignKeyConstraint fk, bool checkRelation)
		{
			DataRelation dataRelation = fk.FindParentRelation();
			if (checkRelation)
			{
				if (dataRelation == null)
				{
					return false;
				}
				if (!XmlTreeGen.AutoGenerated(dataRelation))
				{
					return false;
				}
				if (dataRelation.RelationName != fk.ConstraintName)
				{
					return false;
				}
			}
			return fk.ExtendedProperties.Count <= 0 && fk.AcceptRejectRule == AcceptRejectRule.None && fk.DeleteRule == Rule.Cascade && fk.DeleteRule == Rule.Cascade && fk.RelatedColumnsReference.Length == 1 && XmlTreeGen.AutoGenerated(fk.RelatedColumnsReference[0]);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x002082AC File Offset: 0x002076AC
		private bool IsAutoGenerated(object o)
		{
			return this.schFormat != SchemaFormat.Remoting && this.autogenerated[o] != null;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x002082D8 File Offset: 0x002076D8
		internal XmlElement HandleTable(DataTable table, XmlDocument dc, XmlElement schema, bool genNested)
		{
			XmlElement xmlElement = dc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
			bool flag = false;
			if ((table.DataSet == null || (this._ds != null && table.Namespace != this._ds.Namespace)) && this.schFormat == SchemaFormat.Remoting)
			{
				xmlElement.SetAttribute("targetNamespace", "urn:schemas-microsoft-com:xml-msdata", table.Namespace);
			}
			xmlElement.SetAttribute("name", table.EncodedTableName);
			if (table.Namespace.Length == 0)
			{
				DataTable dataTable = table;
				string text = dataTable.Namespace;
				while (ADP.IsEmpty(text))
				{
					DataRelation[] nestedParentRelations = dataTable.NestedParentRelations;
					if (nestedParentRelations.Length == 0)
					{
						text = ((this._ds != null) ? this._ds.Namespace : "");
						break;
					}
					int num = -1;
					for (int i = 0; i < nestedParentRelations.Length; i++)
					{
						if (nestedParentRelations[i].ParentTable != dataTable)
						{
							num = i;
							break;
						}
					}
					if (num == -1)
					{
						break;
					}
					dataTable = nestedParentRelations[num].ParentTable;
					text = dataTable.Namespace;
				}
				if (table.Namespace != text)
				{
					xmlElement.SetAttribute("form", "unqualified");
					flag = true;
				}
			}
			if (table.ShouldSerializeCaseSensitive())
			{
				xmlElement.SetAttribute("CaseSensitive", "urn:schemas-microsoft-com:xml-msdata", table.CaseSensitive.ToString(CultureInfo.InvariantCulture));
			}
			if (table.ShouldSerializeLocale())
			{
				xmlElement.SetAttribute("Locale", "urn:schemas-microsoft-com:xml-msdata", table.Locale.ToString());
			}
			XmlTreeGen.AddXdoProperties(table, xmlElement, dc);
			DataColumnCollection columns = table.Columns;
			int count = columns.Count;
			int num2 = 0;
			if (count == 1 || count == 2)
			{
				for (int j = 0; j < count; j++)
				{
					DataColumn dataColumn = columns[j];
					if (dataColumn.ColumnMapping == MappingType.Hidden)
					{
						DataRelationCollection childRelations = table.ChildRelations;
						for (int k = 0; k < childRelations.Count; k++)
						{
							if (childRelations[k].Nested && childRelations[k].ParentKey.ColumnsReference.Length == 1 && childRelations[k].ParentKey.ColumnsReference[0] == dataColumn)
							{
								num2++;
							}
						}
					}
					if (dataColumn.ColumnMapping == MappingType.Element)
					{
						num2++;
					}
				}
			}
			if (table.repeatableElement && num2 == 1)
			{
				DataColumn dataColumn2 = table.Columns[0];
				string text2 = XmlTreeGen.XmlDataTypeName(dataColumn2.DataType);
				if (text2 == null || text2.Length == 0)
				{
					text2 = "anyType";
				}
				xmlElement.SetAttribute("type", XSDSchema.QualifiedName(text2));
				return xmlElement;
			}
			XmlElement xmlElement2 = dc.CreateElement("xs", "complexType", "http://www.w3.org/2001/XMLSchema");
			if (!table.TypeName.IsEmpty && this.schFormat != SchemaFormat.Remoting)
			{
				XmlElement xmlElement3 = this.GetSchema(table.TypeName.Namespace);
				if (ADP.IsEmpty(table.TypeName.Namespace))
				{
					if (this._ds == null)
					{
						xmlElement3 = this.GetSchema(table.Namespace);
					}
					else
					{
						xmlElement3 = (flag ? this.GetSchema(this._ds.Namespace) : this.GetSchema(table.Namespace));
					}
				}
				if (this.FindTypeNode(xmlElement3, table.TypeName.Name) == null)
				{
					xmlElement3.AppendChild(xmlElement2);
				}
				xmlElement2.SetAttribute("name", table.TypeName.Name);
			}
			else
			{
				xmlElement.AppendChild(xmlElement2);
			}
			if (!table.TypeName.IsEmpty && this.schFormat != SchemaFormat.Remoting)
			{
				xmlElement.SetAttribute("type", NewDiffgramGen.QualifiedName((string)this.prefixes[table.TypeName.Namespace], table.TypeName.Name));
			}
			DataColumn xmlText = table.XmlText;
			if (xmlText != null)
			{
				XmlElement xmlElement4 = dc.CreateElement("xs", "simpleContent", "http://www.w3.org/2001/XMLSchema");
				if (xmlText.GetType() != typeof(DataColumn))
				{
					XmlTreeGen.AddXdoProperties(xmlText, xmlElement4, dc);
				}
				else
				{
					this.AddColumnProperties(xmlText, xmlElement4);
				}
				XmlTreeGen.AddExtendedProperties(xmlText.extendedProperties, xmlElement4);
				if (xmlText.AllowDBNull)
				{
					xmlElement.SetAttribute("nillable", string.Empty, "true");
				}
				if (!xmlText.DefaultValueIsNull)
				{
					XmlTreeGen.ValidateColumnMapping(xmlText.DataType);
					xmlElement4.SetAttribute("DefaultValue", "urn:schemas-microsoft-com:xml-msdata", xmlText.ConvertObjectToXml(xmlText.DefaultValue));
				}
				xmlElement4.SetAttribute("ColumnName", "urn:schemas-microsoft-com:xml-msdata", xmlText.ColumnName);
				xmlElement4.SetAttribute("Ordinal", "urn:schemas-microsoft-com:xml-msdata", xmlText.Ordinal.ToString(CultureInfo.InvariantCulture));
				xmlElement2.AppendChild(xmlElement4);
				XmlElement xmlElement5 = dc.CreateElement("xs", "extension", "http://www.w3.org/2001/XMLSchema");
				xmlElement4.AppendChild(xmlElement5);
				this.HandleColumnType(xmlText, dc, xmlElement5, schema);
				xmlElement2 = xmlElement5;
			}
			XmlElement xmlElement6 = dc.CreateElement("xs", "sequence", "http://www.w3.org/2001/XMLSchema");
			xmlElement2.AppendChild(xmlElement6);
			bool flag2 = this.HasMixedColumns(table);
			for (int l = 0; l < count; l++)
			{
				DataColumn dataColumn3 = columns[l];
				if (dataColumn3.ColumnMapping != MappingType.SimpleContent && (dataColumn3.ColumnMapping == MappingType.Attribute || dataColumn3.ColumnMapping == MappingType.Element || dataColumn3.ColumnMapping == MappingType.Hidden) && !this.IsAutoGenerated(dataColumn3))
				{
					bool flag3 = dataColumn3.ColumnMapping != MappingType.Element;
					XmlElement xmlElement7 = this.HandleColumn(dataColumn3, dc, schema, flag2);
					XmlElement xmlElement8 = (flag3 ? xmlElement2 : xmlElement6);
					xmlElement8.AppendChild(xmlElement7);
				}
			}
			if (table.XmlText == null && genNested)
			{
				DataRelationCollection childRelations2 = table.ChildRelations;
				for (int m = 0; m < childRelations2.Count; m++)
				{
					if (childRelations2[m].Nested)
					{
						DataTable childTable = childRelations2[m].ChildTable;
						XmlElement xmlElement9;
						if (childTable == table)
						{
							xmlElement9 = dc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
							xmlElement9.SetAttribute("ref", table.EncodedTableName);
						}
						else if (childTable.NestedParentsCount > 1)
						{
							xmlElement9 = dc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
							xmlElement9.SetAttribute("ref", childTable.EncodedTableName);
						}
						else
						{
							xmlElement9 = this.HandleTable(childTable, dc, schema);
						}
						if (childTable.Namespace == table.Namespace)
						{
							xmlElement9.SetAttribute("minOccurs", "0");
							xmlElement9.SetAttribute("maxOccurs", "unbounded");
						}
						if (childTable.Namespace == table.Namespace || childTable.Namespace.Length == 0 || this.schFormat == SchemaFormat.Remoting)
						{
							xmlElement6.AppendChild(xmlElement9);
						}
						else
						{
							if (childTable.NestedParentsCount <= 1)
							{
								this.GetSchema(childTable.Namespace).AppendChild(xmlElement9);
							}
							xmlElement9 = dc.CreateElement("xs", "element", "http://www.w3.org/2001/XMLSchema");
							xmlElement9.SetAttribute("ref", (string)this.prefixes[childTable.Namespace] + ':' + childTable.EncodedTableName);
							xmlElement6.AppendChild(xmlElement9);
						}
						if (childRelations2[m].ChildKeyConstraint == null)
						{
							XmlElement xmlElement10 = this._dc.CreateElement("xs", "annotation", "http://www.w3.org/2001/XMLSchema");
							xmlElement9.PrependChild(xmlElement10);
							XmlElement xmlElement11 = this._dc.CreateElement("xs", "appinfo", "http://www.w3.org/2001/XMLSchema");
							xmlElement10.AppendChild(xmlElement11);
							xmlElement11.AppendChild(this.HandleRelation(childRelations2[m], dc));
						}
					}
				}
			}
			if (xmlElement6 != null && !xmlElement6.HasChildNodes)
			{
				xmlElement2.RemoveChild(xmlElement6);
			}
			ConstraintCollection constraints = table.Constraints;
			string text3 = ((this._ds != null) ? ((this._ds.Namespace.Length != 0) ? "mstns:" : string.Empty) : string.Empty);
			if (this.schFormat != SchemaFormat.Remoting)
			{
				this.GetSchema(table.Namespace);
				text3 = ((table.Namespace.Length != 0) ? ((string)this.prefixes[table.Namespace] + ':') : string.Empty);
			}
			for (int n = 0; n < constraints.Count; n++)
			{
				if (constraints[n] is UniqueConstraint)
				{
					UniqueConstraint uniqueConstraint = (UniqueConstraint)constraints[n];
					if (!this.IsAutoGenerated(uniqueConstraint))
					{
						DataColumn[] array = uniqueConstraint.Key.ColumnsReference;
						XmlElement xmlElement12 = dc.CreateElement("xs", "unique", "http://www.w3.org/2001/XMLSchema");
						if (this._ds == null || this._ds.Tables.InternalIndexOf(table.TableName) == -3)
						{
							xmlElement12.SetAttribute("TableNamespace", "urn:schemas-microsoft-com:xml-msdata", table.Namespace);
						}
						xmlElement12.SetAttribute("name", XmlConvert.EncodeLocalName(uniqueConstraint.SchemaName));
						if (uniqueConstraint.ConstraintName != uniqueConstraint.SchemaName)
						{
							xmlElement12.SetAttribute("ConstraintName", "urn:schemas-microsoft-com:xml-msdata", uniqueConstraint.ConstraintName);
						}
						XmlTreeGen.AddExtendedProperties(uniqueConstraint.extendedProperties, xmlElement12);
						XmlElement xmlElement13 = dc.CreateElement("xs", "selector", "http://www.w3.org/2001/XMLSchema");
						xmlElement13.SetAttribute("xpath", ".//" + text3 + table.EncodedTableName);
						xmlElement12.AppendChild(xmlElement13);
						if (uniqueConstraint.IsPrimaryKey)
						{
							xmlElement12.SetAttribute("PrimaryKey", "urn:schemas-microsoft-com:xml-msdata", "true");
						}
						if (0 < array.Length)
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int num3 = 0; num3 < array.Length; num3++)
							{
								stringBuilder.Length = 0;
								if (this.schFormat != SchemaFormat.Remoting)
								{
									this.GetSchema(array[num3].Namespace);
									if (!ADP.IsEmpty(array[num3].Namespace))
									{
										stringBuilder.Append(this.prefixes[array[num3].Namespace]).Append(':');
									}
									stringBuilder.Append(array[num3].EncodedColumnName);
								}
								else
								{
									stringBuilder.Append(text3).Append(array[num3].EncodedColumnName);
								}
								if (array[num3].ColumnMapping == MappingType.Attribute || array[num3].ColumnMapping == MappingType.Hidden)
								{
									stringBuilder.Insert(0, '@');
								}
								XmlElement xmlElement14 = dc.CreateElement("xs", "field", "http://www.w3.org/2001/XMLSchema");
								xmlElement14.SetAttribute("xpath", stringBuilder.ToString());
								xmlElement12.AppendChild(xmlElement14);
							}
						}
						this.dsElement.InsertBefore(xmlElement12, this.constraintSeparator);
					}
				}
				else if (constraints[n] is ForeignKeyConstraint && genNested)
				{
					ForeignKeyConstraint foreignKeyConstraint = (ForeignKeyConstraint)constraints[n];
					if ((this._tables.Count <= 0 || (this._tables.Contains(foreignKeyConstraint.RelatedTable) && this._tables.Contains(foreignKeyConstraint.Table))) && !this.IsAutoGenerated(foreignKeyConstraint))
					{
						DataRelation dataRelation = foreignKeyConstraint.FindParentRelation();
						DataColumn[] array = foreignKeyConstraint.RelatedColumnsReference;
						UniqueConstraint uniqueConstraint2 = (UniqueConstraint)foreignKeyConstraint.RelatedTable.Constraints.FindConstraint(new UniqueConstraint("TEMP", array));
						XmlElement xmlElement12;
						XmlElement xmlElement13;
						if (uniqueConstraint2 == null)
						{
							xmlElement12 = dc.CreateElement("xs", "key", "http://www.w3.org/2001/XMLSchema");
							xmlElement12.SetAttribute("name", XmlConvert.EncodeLocalName(foreignKeyConstraint.SchemaName));
							if (this._ds == null || this._ds.Tables.InternalIndexOf(table.TableName) == -3)
							{
								xmlElement12.SetAttribute("TableNamespace", "urn:schemas-microsoft-com:xml-msdata", table.Namespace);
							}
							xmlElement13 = dc.CreateElement("xs", "selector", "http://www.w3.org/2001/XMLSchema");
							xmlElement13.SetAttribute("xpath", ".//" + text3 + foreignKeyConstraint.RelatedTable.EncodedTableName);
							xmlElement12.AppendChild(xmlElement13);
							if (0 < array.Length)
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (int num4 = 0; num4 < array.Length; num4++)
								{
									stringBuilder2.Length = 0;
									if (this.schFormat != SchemaFormat.Remoting)
									{
										this.GetSchema(array[num4].Namespace);
										if (!ADP.IsEmpty(array[num4].Namespace))
										{
											stringBuilder2.Append(this.prefixes[array[num4].Namespace]).Append(':');
										}
										stringBuilder2.Append(array[num4].EncodedColumnName);
									}
									else
									{
										stringBuilder2.Append(text3).Append(array[num4].EncodedColumnName);
									}
									if (array[num4].ColumnMapping == MappingType.Attribute || array[num4].ColumnMapping == MappingType.Hidden)
									{
										stringBuilder2.Insert(0, '@');
									}
									XmlElement xmlElement14 = dc.CreateElement("xs", "field", "http://www.w3.org/2001/XMLSchema");
									xmlElement14.SetAttribute("xpath", stringBuilder2.ToString());
									xmlElement12.AppendChild(xmlElement14);
								}
							}
							this.dsElement.InsertBefore(xmlElement12, this.constraintSeparator);
						}
						xmlElement12 = dc.CreateElement("xs", "keyref", "http://www.w3.org/2001/XMLSchema");
						xmlElement12.SetAttribute("name", XmlConvert.EncodeLocalName(foreignKeyConstraint.SchemaName));
						if (this._ds == null || this._ds.Tables.InternalIndexOf(foreignKeyConstraint.RelatedTable.TableName) == -3)
						{
							xmlElement12.SetAttribute("TableNamespace", "urn:schemas-microsoft-com:xml-msdata", foreignKeyConstraint.Table.Namespace);
						}
						if (uniqueConstraint2 == null)
						{
							xmlElement12.SetAttribute("refer", XmlConvert.EncodeLocalName(foreignKeyConstraint.SchemaName));
						}
						else
						{
							xmlElement12.SetAttribute("refer", XmlConvert.EncodeLocalName(uniqueConstraint2.SchemaName));
						}
						XmlTreeGen.AddExtendedProperties(foreignKeyConstraint.extendedProperties, xmlElement12, typeof(ForeignKeyConstraint));
						if (foreignKeyConstraint.ConstraintName != foreignKeyConstraint.SchemaName)
						{
							xmlElement12.SetAttribute("ConstraintName", "urn:schemas-microsoft-com:xml-msdata", foreignKeyConstraint.ConstraintName);
						}
						if (dataRelation == null)
						{
							xmlElement12.SetAttribute("ConstraintOnly", "urn:schemas-microsoft-com:xml-msdata", "true");
						}
						else
						{
							if (dataRelation.Nested)
							{
								xmlElement12.SetAttribute("IsNested", "urn:schemas-microsoft-com:xml-msdata", "true");
							}
							XmlTreeGen.AddExtendedProperties(dataRelation.extendedProperties, xmlElement12, typeof(DataRelation));
							if (foreignKeyConstraint.ConstraintName != dataRelation.RelationName)
							{
								xmlElement12.SetAttribute("RelationName", "urn:schemas-microsoft-com:xml-msdata", XmlConvert.EncodeLocalName(dataRelation.RelationName));
							}
						}
						xmlElement13 = dc.CreateElement("xs", "selector", "http://www.w3.org/2001/XMLSchema");
						xmlElement13.SetAttribute("xpath", ".//" + text3 + table.EncodedTableName);
						xmlElement12.AppendChild(xmlElement13);
						if (foreignKeyConstraint.AcceptRejectRule != AcceptRejectRule.None)
						{
							xmlElement12.SetAttribute("AcceptRejectRule", "urn:schemas-microsoft-com:xml-msdata", XmlTreeGen.TranslateAcceptRejectRule(foreignKeyConstraint.AcceptRejectRule));
						}
						if (foreignKeyConstraint.UpdateRule != Rule.Cascade)
						{
							xmlElement12.SetAttribute("UpdateRule", "urn:schemas-microsoft-com:xml-msdata", XmlTreeGen.TranslateRule(foreignKeyConstraint.UpdateRule));
						}
						if (foreignKeyConstraint.DeleteRule != Rule.Cascade)
						{
							xmlElement12.SetAttribute("DeleteRule", "urn:schemas-microsoft-com:xml-msdata", XmlTreeGen.TranslateRule(foreignKeyConstraint.DeleteRule));
						}
						array = foreignKeyConstraint.Columns;
						if (0 < array.Length)
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							for (int num5 = 0; num5 < array.Length; num5++)
							{
								stringBuilder3.Length = 0;
								if (this.schFormat != SchemaFormat.Remoting)
								{
									this.GetSchema(array[num5].Namespace);
									if (!ADP.IsEmpty(array[num5].Namespace))
									{
										stringBuilder3.Append(this.prefixes[array[num5].Namespace]).Append(':');
									}
									stringBuilder3.Append(array[num5].EncodedColumnName);
								}
								else
								{
									stringBuilder3.Append(text3).Append(array[num5].EncodedColumnName);
								}
								if (array[num5].ColumnMapping == MappingType.Attribute || array[num5].ColumnMapping == MappingType.Hidden)
								{
									stringBuilder3.Insert(0, '@');
								}
								XmlElement xmlElement14 = dc.CreateElement("xs", "field", "http://www.w3.org/2001/XMLSchema");
								xmlElement14.SetAttribute("xpath", stringBuilder3.ToString());
								xmlElement12.AppendChild(xmlElement14);
							}
						}
						this.dsElement.InsertAfter(xmlElement12, this.constraintSeparator);
					}
				}
			}
			XmlTreeGen.AddExtendedProperties(table.extendedProperties, xmlElement);
			return xmlElement;
		}

		// Token: 0x04000A5D RID: 2653
		private ArrayList ConstraintNames;

		// Token: 0x04000A5E RID: 2654
		private Hashtable namespaces;

		// Token: 0x04000A5F RID: 2655
		private Hashtable autogenerated;

		// Token: 0x04000A60 RID: 2656
		private Hashtable prefixes;

		// Token: 0x04000A61 RID: 2657
		private DataSet _ds;

		// Token: 0x04000A62 RID: 2658
		private ArrayList _tables = new ArrayList();

		// Token: 0x04000A63 RID: 2659
		private ArrayList _relations = new ArrayList();

		// Token: 0x04000A64 RID: 2660
		private XmlDocument _dc;

		// Token: 0x04000A65 RID: 2661
		private XmlElement _sRoot;

		// Token: 0x04000A66 RID: 2662
		private int prefixCount;

		// Token: 0x04000A67 RID: 2663
		private SchemaFormat schFormat = SchemaFormat.Public;

		// Token: 0x04000A68 RID: 2664
		private string filePath;

		// Token: 0x04000A69 RID: 2665
		private string fileName;

		// Token: 0x04000A6A RID: 2666
		private string fileExt;

		// Token: 0x04000A6B RID: 2667
		private XmlElement dsElement;

		// Token: 0x04000A6C RID: 2668
		private XmlElement constraintSeparator;
	}
}
