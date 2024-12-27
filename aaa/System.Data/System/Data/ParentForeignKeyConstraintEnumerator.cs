using System;

namespace System.Data
{
	// Token: 0x02000067 RID: 103
	internal sealed class ParentForeignKeyConstraintEnumerator : ForeignKeyConstraintEnumerator
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x001D6384 File Offset: 0x001D5784
		public ParentForeignKeyConstraintEnumerator(DataSet dataSet, DataTable inTable)
			: base(dataSet)
		{
			this.table = inTable;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x001D63A0 File Offset: 0x001D57A0
		protected override bool IsValidCandidate(Constraint constraint)
		{
			return constraint is ForeignKeyConstraint && ((ForeignKeyConstraint)constraint).RelatedTable == this.table;
		}

		// Token: 0x040006D2 RID: 1746
		private DataTable table;
	}
}
