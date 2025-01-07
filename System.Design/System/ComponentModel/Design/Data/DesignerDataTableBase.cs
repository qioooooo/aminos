using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	public abstract class DesignerDataTableBase
	{
		protected DesignerDataTableBase(string name)
		{
			this._name = name;
		}

		protected DesignerDataTableBase(string name, string owner)
		{
			this._name = name;
			this._owner = owner;
		}

		public ICollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = this.CreateColumns();
				}
				return this._columns;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public string Owner
		{
			get
			{
				return this._owner;
			}
		}

		protected abstract ICollection CreateColumns();

		private ICollection _columns;

		private string _name;

		private string _owner;
	}
}
