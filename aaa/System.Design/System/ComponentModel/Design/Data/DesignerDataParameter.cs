using System;
using System.Data;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000145 RID: 325
	public sealed class DesignerDataParameter
	{
		// Token: 0x06000C8D RID: 3213 RVA: 0x00030955 File Offset: 0x0002F955
		public DesignerDataParameter(string name, DbType dataType, ParameterDirection direction)
		{
			this._dataType = dataType;
			this._direction = direction;
			this._name = name;
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00030972 File Offset: 0x0002F972
		public DbType DataType
		{
			get
			{
				return this._dataType;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0003097A File Offset: 0x0002F97A
		public ParameterDirection Direction
		{
			get
			{
				return this._direction;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00030982 File Offset: 0x0002F982
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x04000EA7 RID: 3751
		private DbType _dataType;

		// Token: 0x04000EA8 RID: 3752
		private ParameterDirection _direction;

		// Token: 0x04000EA9 RID: 3753
		private string _name;
	}
}
