using System;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000147 RID: 327
	public sealed class DesignerDataSchemaClass
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x000309CF File Offset: 0x0002F9CF
		private DesignerDataSchemaClass()
		{
		}

		// Token: 0x04000EAE RID: 3758
		public static readonly DesignerDataSchemaClass StoredProcedures = new DesignerDataSchemaClass();

		// Token: 0x04000EAF RID: 3759
		public static readonly DesignerDataSchemaClass Tables = new DesignerDataSchemaClass();

		// Token: 0x04000EB0 RID: 3760
		public static readonly DesignerDataSchemaClass Views = new DesignerDataSchemaClass();
	}
}
