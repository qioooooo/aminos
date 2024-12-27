using System;

namespace System.Data
{
	// Token: 0x02000066 RID: 102
	internal sealed class ChildForeignKeyConstraintEnumerator : ForeignKeyConstraintEnumerator
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x001D633C File Offset: 0x001D573C
		public ChildForeignKeyConstraintEnumerator(DataSet dataSet, DataTable inTable)
			: base(dataSet)
		{
			this.table = inTable;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x001D6358 File Offset: 0x001D5758
		protected override bool IsValidCandidate(Constraint constraint)
		{
			return constraint is ForeignKeyConstraint && ((ForeignKeyConstraint)constraint).Table == this.table;
		}

		// Token: 0x040006D1 RID: 1745
		private DataTable table;
	}
}
