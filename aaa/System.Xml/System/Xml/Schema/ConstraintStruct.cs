using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200018A RID: 394
	internal sealed class ConstraintStruct
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0005DFF4 File Offset: 0x0005CFF4
		internal int TableDim
		{
			get
			{
				return this.tableDim;
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0005DFFC File Offset: 0x0005CFFC
		internal ConstraintStruct(CompiledIdentityConstraint constraint)
		{
			this.constraint = constraint;
			this.tableDim = constraint.Fields.Length;
			this.axisFields = new ArrayList();
			this.axisSelector = new SelectorActiveAxis(constraint.Selector, this);
			if (this.constraint.Role != CompiledIdentityConstraint.ConstraintRole.Keyref)
			{
				this.qualifiedTable = new Hashtable();
			}
		}

		// Token: 0x04000C9D RID: 3229
		internal CompiledIdentityConstraint constraint;

		// Token: 0x04000C9E RID: 3230
		internal SelectorActiveAxis axisSelector;

		// Token: 0x04000C9F RID: 3231
		internal ArrayList axisFields;

		// Token: 0x04000CA0 RID: 3232
		internal Hashtable qualifiedTable;

		// Token: 0x04000CA1 RID: 3233
		internal Hashtable keyrefTable;

		// Token: 0x04000CA2 RID: 3234
		private int tableDim;
	}
}
