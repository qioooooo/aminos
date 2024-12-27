using System;
using System.Data;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000143 RID: 323
	public sealed class DesignerDataColumn
	{
		// Token: 0x06000C7B RID: 3195 RVA: 0x0003082C File Offset: 0x0002F82C
		public DesignerDataColumn(string name, DbType dataType)
			: this(name, dataType, null, false, false, false, -1, -1, -1)
		{
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00030848 File Offset: 0x0002F848
		public DesignerDataColumn(string name, DbType dataType, object defaultValue)
			: this(name, dataType, defaultValue, false, false, false, -1, -1, -1)
		{
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00030864 File Offset: 0x0002F864
		public DesignerDataColumn(string name, DbType dataType, object defaultValue, bool identity, bool nullable, bool primaryKey, int precision, int scale, int length)
		{
			this._dataType = dataType;
			this._defaultValue = defaultValue;
			this._identity = identity;
			this._length = length;
			this._name = name;
			this._nullable = nullable;
			this._precision = precision;
			this._primaryKey = primaryKey;
			this._scale = scale;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000308BC File Offset: 0x0002F8BC
		public DbType DataType
		{
			get
			{
				return this._dataType;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x000308C4 File Offset: 0x0002F8C4
		public object DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000308CC File Offset: 0x0002F8CC
		public bool Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000308D4 File Offset: 0x0002F8D4
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000308DC File Offset: 0x0002F8DC
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x000308E4 File Offset: 0x0002F8E4
		public bool Nullable
		{
			get
			{
				return this._nullable;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x000308EC File Offset: 0x0002F8EC
		public int Precision
		{
			get
			{
				return this._precision;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x000308F4 File Offset: 0x0002F8F4
		public bool PrimaryKey
		{
			get
			{
				return this._primaryKey;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x000308FC File Offset: 0x0002F8FC
		public int Scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x04000E9A RID: 3738
		private DbType _dataType;

		// Token: 0x04000E9B RID: 3739
		private object _defaultValue;

		// Token: 0x04000E9C RID: 3740
		private bool _identity;

		// Token: 0x04000E9D RID: 3741
		private int _length;

		// Token: 0x04000E9E RID: 3742
		private string _name;

		// Token: 0x04000E9F RID: 3743
		private bool _nullable;

		// Token: 0x04000EA0 RID: 3744
		private int _precision;

		// Token: 0x04000EA1 RID: 3745
		private bool _primaryKey;

		// Token: 0x04000EA2 RID: 3746
		private int _scale;
	}
}
