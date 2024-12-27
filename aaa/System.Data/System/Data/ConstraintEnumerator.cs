using System;
using System.Collections;

namespace System.Data
{
	// Token: 0x02000064 RID: 100
	internal class ConstraintEnumerator
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x001D61F0 File Offset: 0x001D55F0
		public ConstraintEnumerator(DataSet dataSet)
		{
			this.tables = ((dataSet != null) ? dataSet.Tables.GetEnumerator() : null);
			this.currentObject = null;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x001D6224 File Offset: 0x001D5624
		public bool GetNext()
		{
			this.currentObject = null;
			while (this.tables != null)
			{
				if (this.constraints == null)
				{
					if (!this.tables.MoveNext())
					{
						this.tables = null;
						return false;
					}
					this.constraints = ((DataTable)this.tables.Current).Constraints.GetEnumerator();
				}
				if (!this.constraints.MoveNext())
				{
					this.constraints = null;
				}
				else
				{
					Constraint constraint = (Constraint)this.constraints.Current;
					if (this.IsValidCandidate(constraint))
					{
						this.currentObject = constraint;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x001D62C0 File Offset: 0x001D56C0
		public Constraint GetConstraint()
		{
			return this.currentObject;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x001D62D4 File Offset: 0x001D56D4
		protected virtual bool IsValidCandidate(Constraint constraint)
		{
			return true;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x001D62E4 File Offset: 0x001D56E4
		protected Constraint CurrentObject
		{
			get
			{
				return this.currentObject;
			}
		}

		// Token: 0x040006CE RID: 1742
		private IEnumerator tables;

		// Token: 0x040006CF RID: 1743
		private IEnumerator constraints;

		// Token: 0x040006D0 RID: 1744
		private Constraint currentObject;
	}
}
