using System;

namespace System.Data
{
	// Token: 0x02000065 RID: 101
	internal class ForeignKeyConstraintEnumerator : ConstraintEnumerator
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x001D62F8 File Offset: 0x001D56F8
		public ForeignKeyConstraintEnumerator(DataSet dataSet)
			: base(dataSet)
		{
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x001D630C File Offset: 0x001D570C
		protected override bool IsValidCandidate(Constraint constraint)
		{
			return constraint is ForeignKeyConstraint;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x001D6324 File Offset: 0x001D5724
		public ForeignKeyConstraint GetForeignKeyConstraint()
		{
			return (ForeignKeyConstraint)base.CurrentObject;
		}
	}
}
