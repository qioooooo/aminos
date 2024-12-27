using System;
using System.ComponentModel;
using System.Design;

namespace System.Data.Design
{
	// Token: 0x0200008C RID: 140
	[DefaultProperty("CommandText")]
	[DataSourceXmlClass("DbCommand")]
	internal class DbSourceCommand : DataSourceComponent, ICloneable, INamedObject
	{
		// Token: 0x060005B4 RID: 1460 RVA: 0x0000B0A7 File Offset: 0x0000A0A7
		public DbSourceCommand()
		{
			this.commandText = string.Empty;
			this.commandType = CommandType.Text;
			this.parameterCollection = new DbSourceParameterCollection(this);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000B0CD File Offset: 0x0000A0CD
		public DbSourceCommand(DbSource parent, CommandOperation operation)
			: this()
		{
			this.SetParent(parent);
			this.CommandOperation = operation;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0000B0E3 File Offset: 0x0000A0E3
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0000B0EB File Offset: 0x0000A0EB
		internal CommandOperation CommandOperation
		{
			get
			{
				return this.commandOperation;
			}
			set
			{
				this.commandOperation = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0000B0F4 File Offset: 0x0000A0F4
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x0000B0FC File Offset: 0x0000A0FC
		[DataSourceXmlElement]
		[Browsable(false)]
		public string CommandText
		{
			get
			{
				return this.commandText;
			}
			set
			{
				this.commandText = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0000B105 File Offset: 0x0000A105
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0000B110 File Offset: 0x0000A110
		[DefaultValue(CommandType.Text)]
		[DataSourceXmlAttribute(ItemType = typeof(CommandType))]
		public CommandType CommandType
		{
			get
			{
				return this.commandType;
			}
			set
			{
				if (value == CommandType.TableDirect && this._parent != null && this._parent.Connection != null)
				{
					string provider = this._parent.Connection.Provider;
					if (!StringUtil.EqualValue(provider, "System.Data.OleDb"))
					{
						throw new Exception(SR.GetString("DD_E_TableDirectValidForOleDbOnly"));
					}
				}
				this.commandType = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0000B16F File Offset: 0x0000A16F
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0000B177 File Offset: 0x0000A177
		[DataSourceXmlAttribute(ItemType = typeof(bool))]
		[Browsable(false)]
		public bool ModifiedByUser
		{
			get
			{
				return this.modifiedByUser;
			}
			set
			{
				this.modifiedByUser = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0000B180 File Offset: 0x0000A180
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0000B188 File Offset: 0x0000A188
		[Browsable(false)]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0000B191 File Offset: 0x0000A191
		[DataSourceXmlSubItem(ItemType = typeof(DesignParameter))]
		public DbSourceParameterCollection Parameters
		{
			get
			{
				return this.parameterCollection;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000B199 File Offset: 0x0000A199
		private bool ShouldSerializeParameters()
		{
			return this.parameterCollection != null && 0 < this.parameterCollection.Count;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0000B1B3 File Offset: 0x0000A1B3
		[Browsable(false)]
		public override object Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000B1BC File Offset: 0x0000A1BC
		public object Clone()
		{
			DbSourceCommand dbSourceCommand = new DbSourceCommand();
			dbSourceCommand.commandText = this.commandText;
			dbSourceCommand.commandType = this.commandType;
			dbSourceCommand.commandOperation = this.commandOperation;
			dbSourceCommand.parameterCollection = (DbSourceParameterCollection)this.parameterCollection.Clone();
			dbSourceCommand.parameterCollection.CollectionHost = dbSourceCommand;
			return dbSourceCommand;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000B216 File Offset: 0x0000A216
		internal void SetParent(DbSource parent)
		{
			this._parent = parent;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000B21F File Offset: 0x0000A21F
		public override string ToString()
		{
			if (StringUtil.NotEmptyAfterTrim(((INamedObject)this).Name))
			{
				return ((INamedObject)this).Name;
			}
			return base.ToString();
		}

		// Token: 0x04000B08 RID: 2824
		private DbSource _parent;

		// Token: 0x04000B09 RID: 2825
		private CommandOperation commandOperation;

		// Token: 0x04000B0A RID: 2826
		private string commandText;

		// Token: 0x04000B0B RID: 2827
		private CommandType commandType;

		// Token: 0x04000B0C RID: 2828
		private DbSourceParameterCollection parameterCollection;

		// Token: 0x04000B0D RID: 2829
		private bool modifiedByUser;

		// Token: 0x04000B0E RID: 2830
		private string name;
	}
}
