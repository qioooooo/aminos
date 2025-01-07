using System;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class StatementContext
	{
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

		private ObjectStatementCollection _statements;
	}
}
