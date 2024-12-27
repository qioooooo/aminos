using System;
using System.Xml.Schema;

namespace System.Data
{
	// Token: 0x020000FA RID: 250
	internal sealed class ConstraintTable
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0020B710 File Offset: 0x0020AB10
		public ConstraintTable(DataTable t, XmlSchemaIdentityConstraint c)
		{
			this.table = t;
			this.constraint = c;
		}

		// Token: 0x04000A81 RID: 2689
		public DataTable table;

		// Token: 0x04000A82 RID: 2690
		public XmlSchemaIdentityConstraint constraint;
	}
}
