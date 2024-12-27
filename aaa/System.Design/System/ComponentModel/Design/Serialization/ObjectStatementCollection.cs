using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200016F RID: 367
	public sealed class ObjectStatementCollection : IEnumerable
	{
		// Token: 0x06000D89 RID: 3465 RVA: 0x0003773A File Offset: 0x0003673A
		internal ObjectStatementCollection()
		{
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00037744 File Offset: 0x00036744
		private void AddOwner(object statementOwner, CodeStatementCollection statements)
		{
			if (this._table == null)
			{
				this._table = new List<ObjectStatementCollection.TableEntry>();
			}
			else
			{
				int i = 0;
				while (i < this._table.Count)
				{
					if (object.ReferenceEquals(this._table[i].Owner, statementOwner))
					{
						if (this._table[i].Statements != null)
						{
							throw new InvalidOperationException();
						}
						if (statements != null)
						{
							this._table[i] = new ObjectStatementCollection.TableEntry(statementOwner, statements);
						}
						return;
					}
					else
					{
						i++;
					}
				}
			}
			this._table.Add(new ObjectStatementCollection.TableEntry(statementOwner, statements));
			this._version++;
		}

		// Token: 0x1700021B RID: 539
		public CodeStatementCollection this[object statementOwner]
		{
			get
			{
				if (statementOwner == null)
				{
					throw new ArgumentNullException("statementOwner");
				}
				if (this._table != null)
				{
					for (int i = 0; i < this._table.Count; i++)
					{
						if (object.ReferenceEquals(this._table[i].Owner, statementOwner))
						{
							if (this._table[i].Statements == null)
							{
								this._table[i] = new ObjectStatementCollection.TableEntry(statementOwner, new CodeStatementCollection());
							}
							return this._table[i].Statements;
						}
					}
					foreach (ObjectStatementCollection.TableEntry tableEntry in this._table)
					{
						if (object.ReferenceEquals(tableEntry.Owner, statementOwner))
						{
							return tableEntry.Statements;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000378D4 File Offset: 0x000368D4
		public bool ContainsKey(object statementOwner)
		{
			if (statementOwner == null)
			{
				throw new ArgumentNullException("statementOwner");
			}
			return this._table != null && this[statementOwner] != null;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000378FB File Offset: 0x000368FB
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ObjectStatementCollection.TableEnumerator(this);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00037908 File Offset: 0x00036908
		public void Populate(ICollection statementOwners)
		{
			if (statementOwners == null)
			{
				throw new ArgumentNullException("statementOwners");
			}
			foreach (object obj in statementOwners)
			{
				this.Populate(obj);
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00037968 File Offset: 0x00036968
		public void Populate(object owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.AddOwner(owner, null);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00037980 File Offset: 0x00036980
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000F19 RID: 3865
		private List<ObjectStatementCollection.TableEntry> _table;

		// Token: 0x04000F1A RID: 3866
		private int _version;

		// Token: 0x02000170 RID: 368
		private struct TableEntry
		{
			// Token: 0x06000D91 RID: 3473 RVA: 0x00037988 File Offset: 0x00036988
			public TableEntry(object owner, CodeStatementCollection statements)
			{
				this.Owner = owner;
				this.Statements = statements;
			}

			// Token: 0x04000F1B RID: 3867
			public object Owner;

			// Token: 0x04000F1C RID: 3868
			public CodeStatementCollection Statements;
		}

		// Token: 0x02000171 RID: 369
		private struct TableEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000D92 RID: 3474 RVA: 0x00037998 File Offset: 0x00036998
			public TableEnumerator(ObjectStatementCollection table)
			{
				this._table = table;
				this._version = this._table._version;
				this._position = -1;
			}

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06000D93 RID: 3475 RVA: 0x000379B9 File Offset: 0x000369B9
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06000D94 RID: 3476 RVA: 0x000379C8 File Offset: 0x000369C8
			public DictionaryEntry Entry
			{
				get
				{
					if (this._version != this._table._version)
					{
						throw new InvalidOperationException();
					}
					if (this._position < 0 || this._table._table == null || this._position >= this._table._table.Count)
					{
						throw new InvalidOperationException();
					}
					if (this._table._table[this._position].Statements == null)
					{
						this._table._table[this._position] = new ObjectStatementCollection.TableEntry(this._table._table[this._position].Owner, new CodeStatementCollection());
					}
					ObjectStatementCollection.TableEntry tableEntry = this._table._table[this._position];
					return new DictionaryEntry(tableEntry.Owner, tableEntry.Statements);
				}
			}

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00037AA4 File Offset: 0x00036AA4
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00037AC0 File Offset: 0x00036AC0
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x06000D97 RID: 3479 RVA: 0x00037ADB File Offset: 0x00036ADB
			public bool MoveNext()
			{
				if (this._table._table != null && this._position + 1 < this._table._table.Count)
				{
					this._position++;
					return true;
				}
				return false;
			}

			// Token: 0x06000D98 RID: 3480 RVA: 0x00037B15 File Offset: 0x00036B15
			public void Reset()
			{
				this._position = -1;
			}

			// Token: 0x04000F1D RID: 3869
			private ObjectStatementCollection _table;

			// Token: 0x04000F1E RID: 3870
			private int _version;

			// Token: 0x04000F1F RID: 3871
			private int _position;
		}
	}
}
