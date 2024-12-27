using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data
{
	// Token: 0x02000061 RID: 97
	[TypeConverter(typeof(ConstraintConverter))]
	[DefaultProperty("ConstraintName")]
	public abstract class Constraint
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x001D4E30 File Offset: 0x001D4230
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x001D4E44 File Offset: 0x001D4244
		[ResDescription("ConstraintNameDescr")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Data")]
		public virtual string ConstraintName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (ADP.IsEmpty(value) && this.Table != null && this.InCollection)
				{
					throw ExceptionBuilder.NoConstraintName();
				}
				CultureInfo cultureInfo = ((this.Table != null) ? this.Table.Locale : CultureInfo.CurrentCulture);
				if (string.Compare(this.name, value, true, cultureInfo) != 0)
				{
					if (this.Table != null && this.InCollection)
					{
						this.Table.Constraints.RegisterName(value);
						if (this.name.Length != 0)
						{
							this.Table.Constraints.UnregisterName(this.name);
						}
					}
					this.name = value;
					return;
				}
				if (string.Compare(this.name, value, false, cultureInfo) != 0)
				{
					this.name = value;
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x001D4F08 File Offset: 0x001D4308
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x001D4F30 File Offset: 0x001D4330
		internal string SchemaName
		{
			get
			{
				if (ADP.IsEmpty(this._schemaName))
				{
					return this.ConstraintName;
				}
				return this._schemaName;
			}
			set
			{
				if (!ADP.IsEmpty(value))
				{
					this._schemaName = value;
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x001D4F4C File Offset: 0x001D434C
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x001D4F60 File Offset: 0x001D4360
		internal virtual bool InCollection
		{
			get
			{
				return this.inCollection;
			}
			set
			{
				this.inCollection = value;
				if (value)
				{
					this.dataSet = this.Table.DataSet;
					return;
				}
				this.dataSet = null;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000479 RID: 1145
		[ResDescription("ConstraintTableDescr")]
		public abstract DataTable Table { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x001D4F90 File Offset: 0x001D4390
		[ResDescription("ExtendedPropertiesDescr")]
		[Browsable(false)]
		[ResCategory("DataCategory_Data")]
		public PropertyCollection ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new PropertyCollection();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x0600047B RID: 1147
		internal abstract bool ContainsColumn(DataColumn column);

		// Token: 0x0600047C RID: 1148
		internal abstract bool CanEnableConstraint();

		// Token: 0x0600047D RID: 1149
		internal abstract Constraint Clone(DataSet destination);

		// Token: 0x0600047E RID: 1150
		internal abstract Constraint Clone(DataSet destination, bool ignoreNSforTableLookup);

		// Token: 0x0600047F RID: 1151 RVA: 0x001D4FB8 File Offset: 0x001D43B8
		internal void CheckConstraint()
		{
			if (!this.CanEnableConstraint())
			{
				throw ExceptionBuilder.ConstraintViolation(this.ConstraintName);
			}
		}

		// Token: 0x06000480 RID: 1152
		internal abstract void CheckCanAddToCollection(ConstraintCollection constraint);

		// Token: 0x06000481 RID: 1153
		internal abstract bool CanBeRemovedFromCollection(ConstraintCollection constraint, bool fThrowException);

		// Token: 0x06000482 RID: 1154
		internal abstract void CheckConstraint(DataRow row, DataRowAction action);

		// Token: 0x06000483 RID: 1155
		internal abstract void CheckState();

		// Token: 0x06000484 RID: 1156 RVA: 0x001D4FDC File Offset: 0x001D43DC
		protected void CheckStateForProperty()
		{
			try
			{
				this.CheckState();
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ExceptionBuilder.BadObjectPropertyAccess(ex.Message);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x001D5028 File Offset: 0x001D4428
		[CLSCompliant(false)]
		protected virtual DataSet _DataSet
		{
			get
			{
				return this.dataSet;
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x001D503C File Offset: 0x001D443C
		protected internal void SetDataSet(DataSet dataSet)
		{
			this.dataSet = dataSet;
		}

		// Token: 0x06000487 RID: 1159
		internal abstract bool IsConstraintViolated();

		// Token: 0x06000488 RID: 1160 RVA: 0x001D5050 File Offset: 0x001D4450
		public override string ToString()
		{
			return this.ConstraintName;
		}

		// Token: 0x040006C3 RID: 1731
		internal string name = "";

		// Token: 0x040006C4 RID: 1732
		private string _schemaName = "";

		// Token: 0x040006C5 RID: 1733
		private bool inCollection;

		// Token: 0x040006C6 RID: 1734
		private DataSet dataSet;

		// Token: 0x040006C7 RID: 1735
		internal PropertyCollection extendedProperties;
	}
}
