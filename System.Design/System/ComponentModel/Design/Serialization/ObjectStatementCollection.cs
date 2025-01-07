using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class ObjectStatementCollection : IEnumerable
	{
		internal ObjectStatementCollection()
		{
		}

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

		public bool ContainsKey(object statementOwner)
		{
			if (statementOwner == null)
			{
				throw new ArgumentNullException("statementOwner");
			}
			return this._table != null && this[statementOwner] != null;
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return new ObjectStatementCollection.TableEnumerator(this);
		}

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

		public void Populate(object owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.AddOwner(owner, null);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private List<ObjectStatementCollection.TableEntry> _table;

		private int _version;

		private struct TableEntry
		{
			public TableEntry(object owner, CodeStatementCollection statements)
			{
				this.Owner = owner;
				this.Statements = statements;
			}

			public object Owner;

			public CodeStatementCollection Statements;
		}

		private struct TableEnumerator : IDictionaryEnumerator, IEnumerator
		{
			public TableEnumerator(ObjectStatementCollection table)
			{
				this._table = table;
				this._version = this._table._version;
				this._position = -1;
			}

			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

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

			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			public bool MoveNext()
			{
				if (this._table._table != null && this._position + 1 < this._table._table.Count)
				{
					this._position++;
					return true;
				}
				return false;
			}

			public void Reset()
			{
				this._position = -1;
			}

			private ObjectStatementCollection _table;

			private int _version;

			private int _position;
		}
	}
}
