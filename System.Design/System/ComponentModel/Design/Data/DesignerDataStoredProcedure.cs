using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	public abstract class DesignerDataStoredProcedure
	{
		protected DesignerDataStoredProcedure(string name)
		{
			this._name = name;
		}

		protected DesignerDataStoredProcedure(string name, string owner)
		{
			this._name = name;
			this._owner = owner;
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

		public ICollection Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = this.CreateParameters();
				}
				return this._parameters;
			}
		}

		protected abstract ICollection CreateParameters();

		private string _name;

		private string _owner;

		private ICollection _parameters;
	}
}
