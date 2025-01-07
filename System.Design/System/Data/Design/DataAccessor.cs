using System;

namespace System.Data.Design
{
	internal class DataAccessor : DataSourceComponent
	{
		public DataAccessor(DesignTable designTable)
		{
			if (designTable == null)
			{
				throw new ArgumentNullException("DesignTable");
			}
			this.designTable = designTable;
		}

		internal DesignTable DesignTable
		{
			get
			{
				return this.designTable;
			}
		}

		internal const string DEFAULT_BASE_CLASS = "System.ComponentModel.Component";

		internal const string DEFAULT_NAME_POSTFIX = "TableAdapter";

		private DesignTable designTable;
	}
}
