using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000EA RID: 234
	[DefaultProperty("ConstraintName")]
	[Editor("Microsoft.VSDesigner.Data.Design.UniqueConstraintEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class UniqueConstraint : Constraint
	{
		// Token: 0x06000DAF RID: 3503 RVA: 0x00200FFC File Offset: 0x002003FC
		public UniqueConstraint(string name, DataColumn column)
		{
			this.Create(name, new DataColumn[] { column });
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00201024 File Offset: 0x00200424
		public UniqueConstraint(DataColumn column)
		{
			this.Create(null, new DataColumn[] { column });
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0020104C File Offset: 0x0020044C
		public UniqueConstraint(string name, DataColumn[] columns)
		{
			this.Create(name, columns);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00201068 File Offset: 0x00200468
		public UniqueConstraint(DataColumn[] columns)
		{
			this.Create(null, columns);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00201084 File Offset: 0x00200484
		[Browsable(false)]
		public UniqueConstraint(string name, string[] columnNames, bool isPrimaryKey)
		{
			this.constraintName = name;
			this.columnNames = columnNames;
			this.bPrimaryKey = isPrimaryKey;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x002010AC File Offset: 0x002004AC
		public UniqueConstraint(string name, DataColumn column, bool isPrimaryKey)
		{
			DataColumn[] array = new DataColumn[] { column };
			this.bPrimaryKey = isPrimaryKey;
			this.Create(name, array);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x002010DC File Offset: 0x002004DC
		public UniqueConstraint(DataColumn column, bool isPrimaryKey)
		{
			DataColumn[] array = new DataColumn[] { column };
			this.bPrimaryKey = isPrimaryKey;
			this.Create(null, array);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0020110C File Offset: 0x0020050C
		public UniqueConstraint(string name, DataColumn[] columns, bool isPrimaryKey)
		{
			this.bPrimaryKey = isPrimaryKey;
			this.Create(name, columns);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00201130 File Offset: 0x00200530
		public UniqueConstraint(DataColumn[] columns, bool isPrimaryKey)
		{
			this.bPrimaryKey = isPrimaryKey;
			this.Create(null, columns);
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00201154 File Offset: 0x00200554
		internal string[] ColumnNames
		{
			get
			{
				return this.key.GetColumnNames();
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0020116C File Offset: 0x0020056C
		internal Index ConstraintIndex
		{
			get
			{
				return this._constraintIndex;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00201180 File Offset: 0x00200580
		internal void ConstraintIndexClear()
		{
			if (this._constraintIndex != null)
			{
				this._constraintIndex.RemoveRef();
				this._constraintIndex = null;
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x002011A8 File Offset: 0x002005A8
		internal void ConstraintIndexInitialize()
		{
			if (this._constraintIndex == null)
			{
				this._constraintIndex = this.key.GetSortIndex();
				this._constraintIndex.AddRef();
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x002011DC File Offset: 0x002005DC
		internal override void CheckState()
		{
			this.NonVirtualCheckState();
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x002011F0 File Offset: 0x002005F0
		private void NonVirtualCheckState()
		{
			this.key.CheckState();
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00201208 File Offset: 0x00200608
		internal override void CheckCanAddToCollection(ConstraintCollection constraints)
		{
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00201218 File Offset: 0x00200618
		internal override bool CanBeRemovedFromCollection(ConstraintCollection constraints, bool fThrowException)
		{
			if (!this.Equals(constraints.Table.primaryKey))
			{
				ParentForeignKeyConstraintEnumerator parentForeignKeyConstraintEnumerator = new ParentForeignKeyConstraintEnumerator(this.Table.DataSet, this.Table);
				while (parentForeignKeyConstraintEnumerator.GetNext())
				{
					ForeignKeyConstraint foreignKeyConstraint = parentForeignKeyConstraintEnumerator.GetForeignKeyConstraint();
					if (this.key.ColumnsEqual(foreignKeyConstraint.ParentKey))
					{
						if (!fThrowException)
						{
							return false;
						}
						throw ExceptionBuilder.NeededForForeignKeyConstraint(this, foreignKeyConstraint);
					}
				}
				return true;
			}
			if (!fThrowException)
			{
				return false;
			}
			throw ExceptionBuilder.RemovePrimaryKey(constraints.Table);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00201294 File Offset: 0x00200694
		internal override bool CanEnableConstraint()
		{
			return !this.Table.EnforceConstraints || this.ConstraintIndex.CheckUnique();
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x002012BC File Offset: 0x002006BC
		internal override bool IsConstraintViolated()
		{
			bool flag = false;
			Index constraintIndex = this.ConstraintIndex;
			if (constraintIndex.HasDuplicates)
			{
				object[] uniqueKeyValues = constraintIndex.GetUniqueKeyValues();
				for (int i = 0; i < uniqueKeyValues.Length; i++)
				{
					Range range = constraintIndex.FindRecords((object[])uniqueKeyValues[i]);
					if (1 < range.Count)
					{
						DataRow[] rows = constraintIndex.GetRows(range);
						string text = ExceptionBuilder.UniqueConstraintViolationText(this.key.ColumnsReference, (object[])uniqueKeyValues[i]);
						for (int j = 0; j < rows.Length; j++)
						{
							rows[j].RowError = text;
							foreach (DataColumn dataColumn in this.key.ColumnsReference)
							{
								rows[j].SetColumnError(dataColumn, text);
							}
						}
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0020138C File Offset: 0x0020078C
		internal override void CheckConstraint(DataRow row, DataRowAction action)
		{
			if (this.Table.EnforceConstraints && (action == DataRowAction.Add || action == DataRowAction.Change || (action == DataRowAction.Rollback && row.tempRecord != -1)) && row.HaveValuesChanged(this.ColumnsReference) && this.ConstraintIndex.IsKeyRecordInIndex(row.GetDefaultRecord()))
			{
				object[] columnValues = row.GetColumnValues(this.ColumnsReference);
				throw ExceptionBuilder.ConstraintViolation(this.ColumnsReference, columnValues);
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x002013F8 File Offset: 0x002007F8
		internal override bool ContainsColumn(DataColumn column)
		{
			return this.key.ContainsColumn(column);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00201414 File Offset: 0x00200814
		internal override Constraint Clone(DataSet destination)
		{
			return this.Clone(destination, false);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0020142C File Offset: 0x0020082C
		internal override Constraint Clone(DataSet destination, bool ignorNSforTableLookup)
		{
			int num;
			if (ignorNSforTableLookup)
			{
				num = destination.Tables.IndexOf(this.Table.TableName);
			}
			else
			{
				num = destination.Tables.IndexOf(this.Table.TableName, this.Table.Namespace, false);
			}
			if (num < 0)
			{
				return null;
			}
			DataTable dataTable = destination.Tables[num];
			int num2 = this.ColumnsReference.Length;
			DataColumn[] array = new DataColumn[num2];
			for (int i = 0; i < num2; i++)
			{
				DataColumn dataColumn = this.ColumnsReference[i];
				num = dataTable.Columns.IndexOf(dataColumn.ColumnName);
				if (num < 0)
				{
					return null;
				}
				array[i] = dataTable.Columns[num];
			}
			UniqueConstraint uniqueConstraint = new UniqueConstraint(this.ConstraintName, array);
			foreach (object obj in base.ExtendedProperties.Keys)
			{
				uniqueConstraint.ExtendedProperties[obj] = base.ExtendedProperties[obj];
			}
			return uniqueConstraint;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00201560 File Offset: 0x00200960
		internal UniqueConstraint Clone(DataTable table)
		{
			int num = this.ColumnsReference.Length;
			DataColumn[] array = new DataColumn[num];
			for (int i = 0; i < num; i++)
			{
				DataColumn dataColumn = this.ColumnsReference[i];
				int num2 = table.Columns.IndexOf(dataColumn.ColumnName);
				if (num2 < 0)
				{
					return null;
				}
				array[i] = table.Columns[num2];
			}
			UniqueConstraint uniqueConstraint = new UniqueConstraint(this.ConstraintName, array);
			foreach (object obj in base.ExtendedProperties.Keys)
			{
				uniqueConstraint.ExtendedProperties[obj] = base.ExtendedProperties[obj];
			}
			return uniqueConstraint;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00201640 File Offset: 0x00200A40
		[ReadOnly(true)]
		[ResDescription("KeyConstraintColumnsDescr")]
		[ResCategory("DataCategory_Data")]
		public virtual DataColumn[] Columns
		{
			get
			{
				return this.key.ToArray();
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00201658 File Offset: 0x00200A58
		internal DataColumn[] ColumnsReference
		{
			get
			{
				return this.key.ColumnsReference;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00201670 File Offset: 0x00200A70
		[ResCategory("DataCategory_Data")]
		[ResDescription("KeyConstraintIsPrimaryKeyDescr")]
		public bool IsPrimaryKey
		{
			get
			{
				return this.Table != null && this == this.Table.primaryKey;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00201698 File Offset: 0x00200A98
		private void Create(string constraintName, DataColumn[] columns)
		{
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i].Computed)
				{
					throw ExceptionBuilder.ExpressionInConstraint(columns[i]);
				}
			}
			this.key = new DataKey(columns, true);
			this.ConstraintName = constraintName;
			this.NonVirtualCheckState();
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x002016E0 File Offset: 0x00200AE0
		public override bool Equals(object key2)
		{
			return key2 is UniqueConstraint && this.Key.ColumnsEqual(((UniqueConstraint)key2).Key);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00201710 File Offset: 0x00200B10
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000210 RID: 528
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00201724 File Offset: 0x00200B24
		internal override bool InCollection
		{
			set
			{
				base.InCollection = value;
				if (this.key.ColumnsReference.Length == 1)
				{
					this.key.ColumnsReference[0].InternalUnique(value);
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0020175C File Offset: 0x00200B5C
		internal DataKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00201770 File Offset: 0x00200B70
		[ReadOnly(true)]
		[ResDescription("ConstraintTableDescr")]
		[ResCategory("DataCategory_Data")]
		public override DataTable Table
		{
			get
			{
				if (this.key.HasValue)
				{
					return this.key.Table;
				}
				return null;
			}
		}

		// Token: 0x0400095D RID: 2397
		private DataKey key;

		// Token: 0x0400095E RID: 2398
		private Index _constraintIndex;

		// Token: 0x0400095F RID: 2399
		internal bool bPrimaryKey;

		// Token: 0x04000960 RID: 2400
		internal string constraintName;

		// Token: 0x04000961 RID: 2401
		internal string[] columnNames;
	}
}
