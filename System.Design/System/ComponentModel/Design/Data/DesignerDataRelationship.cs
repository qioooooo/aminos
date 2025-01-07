using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	public sealed class DesignerDataRelationship
	{
		public DesignerDataRelationship(string name, ICollection parentColumns, DesignerDataTable childTable, ICollection childColumns)
		{
			this._childColumns = childColumns;
			this._childTable = childTable;
			this._name = name;
			this._parentColumns = parentColumns;
		}

		public ICollection ChildColumns
		{
			get
			{
				return this._childColumns;
			}
		}

		public DesignerDataTable ChildTable
		{
			get
			{
				return this._childTable;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public ICollection ParentColumns
		{
			get
			{
				return this._parentColumns;
			}
		}

		private ICollection _childColumns;

		private DesignerDataTable _childTable;

		private string _name;

		private ICollection _parentColumns;
	}
}
