using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	public abstract class DesignerDataTable : DesignerDataTableBase
	{
		protected DesignerDataTable(string name)
			: base(name)
		{
		}

		protected DesignerDataTable(string name, string owner)
			: base(name, owner)
		{
		}

		public ICollection Relationships
		{
			get
			{
				if (this._relationships == null)
				{
					this._relationships = this.CreateRelationships();
				}
				return this._relationships;
			}
		}

		protected abstract ICollection CreateRelationships();

		private ICollection _relationships;
	}
}
