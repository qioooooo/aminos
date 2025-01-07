using System;

namespace System.ComponentModel.Design.Data
{
	public sealed class DesignerDataSchemaClass
	{
		private DesignerDataSchemaClass()
		{
		}

		public static readonly DesignerDataSchemaClass StoredProcedures = new DesignerDataSchemaClass();

		public static readonly DesignerDataSchemaClass Tables = new DesignerDataSchemaClass();

		public static readonly DesignerDataSchemaClass Views = new DesignerDataSchemaClass();
	}
}
