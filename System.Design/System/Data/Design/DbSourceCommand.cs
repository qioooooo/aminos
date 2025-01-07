using System;
using System.ComponentModel;
using System.Design;

namespace System.Data.Design
{
	[DefaultProperty("CommandText")]
	[DataSourceXmlClass("DbCommand")]
	internal class DbSourceCommand : DataSourceComponent, ICloneable, INamedObject
	{
		public DbSourceCommand()
		{
			this.commandText = string.Empty;
			this.commandType = CommandType.Text;
			this.parameterCollection = new DbSourceParameterCollection(this);
		}

		public DbSourceCommand(DbSource parent, CommandOperation operation)
			: this()
		{
			this.SetParent(parent);
			this.CommandOperation = operation;
		}

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

		[DataSourceXmlSubItem(ItemType = typeof(DesignParameter))]
		public DbSourceParameterCollection Parameters
		{
			get
			{
				return this.parameterCollection;
			}
		}

		private bool ShouldSerializeParameters()
		{
			return this.parameterCollection != null && 0 < this.parameterCollection.Count;
		}

		[Browsable(false)]
		public override object Parent
		{
			get
			{
				return this._parent;
			}
		}

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

		internal void SetParent(DbSource parent)
		{
			this._parent = parent;
		}

		public override string ToString()
		{
			if (StringUtil.NotEmptyAfterTrim(((INamedObject)this).Name))
			{
				return ((INamedObject)this).Name;
			}
			return base.ToString();
		}

		private DbSource _parent;

		private CommandOperation commandOperation;

		private string commandText;

		private CommandType commandType;

		private DbSourceParameterCollection parameterCollection;

		private bool modifiedByUser;

		private string name;
	}
}
