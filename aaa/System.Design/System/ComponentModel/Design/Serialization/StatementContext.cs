using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200016E RID: 366
	public sealed class StatementContext
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00037717 File Offset: 0x00036717
		public ObjectStatementCollection StatementCollection
		{
			get
			{
				if (this._statements == null)
				{
					this._statements = new ObjectStatementCollection();
				}
				return this._statements;
			}
		}

		// Token: 0x04000F18 RID: 3864
		private ObjectStatementCollection _statements;
	}
}
