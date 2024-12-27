using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x02000332 RID: 818
	internal class TdsParserSessionPool
	{
		// Token: 0x06002A9C RID: 10908 RVA: 0x0029D48C File Offset: 0x0029C88C
		internal TdsParserSessionPool(TdsParser parser)
		{
			this._parser = parser;
			this._cache = new List<TdsParserStateObject>();
			this._freeStack = new TdsParserSessionPool.TdsParserStateObjectListStack();
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParserSessionPool.ctor|ADV> %d# created session pool for parser %d\n", this.ObjectID, parser.ObjectID);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x0029D4EC File Offset: 0x0029C8EC
		private bool IsDisposed
		{
			get
			{
				return null == this._freeStack;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x0029D504 File Offset: 0x0029C904
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x0029D518 File Offset: 0x0029C918
		internal TdsParserStateObject CreateSession()
		{
			TdsParserStateObject tdsParserStateObject = this._parser.CreateSession();
			lock (this._cache)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.TdsParserSessionPool.CreateSession|ADV> %d# adding session %d to pool\n", this.ObjectID, tdsParserStateObject.ObjectID);
				}
				this._cache.Add(tdsParserStateObject);
				this._cachedCount = this._cache.Count;
			}
			return tdsParserStateObject;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0029D5A0 File Offset: 0x0029C9A0
		internal void Deactivate()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.TdsParserSessionPool.Deactivate|ADV> %d# deactivating cachedCount=%d\n", this.ObjectID, this._cachedCount);
			try
			{
				lock (this._cache)
				{
					for (int i = this._cache.Count - 1; i >= 0; i--)
					{
						TdsParserStateObject tdsParserStateObject = this._cache[i];
						if (tdsParserStateObject != null && tdsParserStateObject.IsOrphaned)
						{
							if (Bid.AdvancedOn)
							{
								Bid.Trace("<sc.TdsParserSessionPool.Deactivate|ADV> %d# reclaiming session %d\n", this.ObjectID, tdsParserStateObject.ObjectID);
							}
							this.PutSession(tdsParserStateObject);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0029D670 File Offset: 0x0029CA70
		internal void Dispose()
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParserSessionPool.Dispose|ADV> %d# disposing cachedCount=%d\n", this.ObjectID, this._cachedCount);
			}
			this._freeStack = null;
			lock (this._cache)
			{
				for (int i = 0; i < this._cache.Count; i++)
				{
					TdsParserStateObject tdsParserStateObject = this._cache[i];
					if (tdsParserStateObject != null)
					{
						tdsParserStateObject.Dispose();
					}
				}
				this._cache.Clear();
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0029D70C File Offset: 0x0029CB0C
		internal TdsParserStateObject GetSession(object owner)
		{
			TdsParserStateObject tdsParserStateObject = this._freeStack.SynchronizedPop();
			if (tdsParserStateObject == null)
			{
				tdsParserStateObject = this.CreateSession();
			}
			tdsParserStateObject.Activate(owner);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParserSessionPool.GetSession|ADV> %d# using session %d\n", this.ObjectID, tdsParserStateObject.ObjectID);
			}
			return tdsParserStateObject;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0029D754 File Offset: 0x0029CB54
		internal void PutSession(TdsParserStateObject session)
		{
			bool flag = session.Deactivate();
			if (!this.IsDisposed)
			{
				if (flag && this._cachedCount < 10)
				{
					if (Bid.AdvancedOn)
					{
						Bid.Trace("<sc.TdsParserSessionPool.PutSession|ADV> %d# keeping session %d cachedCount=%d\n", this.ObjectID, session.ObjectID, this._cachedCount);
					}
					this._freeStack.SynchronizedPush(session);
					return;
				}
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.TdsParserSessionPool.PutSession|ADV> %d# disposing session %d cachedCount=%d\n", this.ObjectID, session.ObjectID, this._cachedCount);
				}
				lock (this._cache)
				{
					this._cache.Remove(session);
					this._cachedCount = this._cache.Count;
				}
				session.Dispose();
			}
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x0029D82C File Offset: 0x0029CC2C
		internal string TraceString()
		{
			return string.Format(null, "(ObjID={0}, free={1}, cached={2}, total={3})", new object[]
			{
				this._objectID,
				(this._freeStack == null) ? "(null)" : this._freeStack.CountDebugOnly.ToString(null),
				this._cachedCount,
				this._cache.Count
			});
		}

		// Token: 0x04001BF8 RID: 7160
		private const int MaxInactiveCount = 10;

		// Token: 0x04001BF9 RID: 7161
		private static int _objectTypeCount;

		// Token: 0x04001BFA RID: 7162
		private readonly int _objectID = Interlocked.Increment(ref TdsParserSessionPool._objectTypeCount);

		// Token: 0x04001BFB RID: 7163
		private readonly TdsParser _parser;

		// Token: 0x04001BFC RID: 7164
		private readonly List<TdsParserStateObject> _cache;

		// Token: 0x04001BFD RID: 7165
		private int _cachedCount;

		// Token: 0x04001BFE RID: 7166
		private TdsParserSessionPool.TdsParserStateObjectListStack _freeStack;

		// Token: 0x02000333 RID: 819
		private class TdsParserStateObjectListStack
		{
			// Token: 0x170006FB RID: 1787
			// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x0029D8A4 File Offset: 0x0029CCA4
			internal int CountDebugOnly
			{
				get
				{
					return -1;
				}
			}

			// Token: 0x06002AA6 RID: 10918 RVA: 0x0029D8B4 File Offset: 0x0029CCB4
			internal TdsParserStateObjectListStack()
			{
			}

			// Token: 0x06002AA7 RID: 10919 RVA: 0x0029D8C8 File Offset: 0x0029CCC8
			internal TdsParserStateObject SynchronizedPop()
			{
				TdsParserStateObject stack;
				lock (this)
				{
					stack = this._stack;
					if (stack != null)
					{
						this._stack = stack.NextPooledObject;
						stack.NextPooledObject = null;
					}
				}
				return stack;
			}

			// Token: 0x06002AA8 RID: 10920 RVA: 0x0029D920 File Offset: 0x0029CD20
			internal void SynchronizedPush(TdsParserStateObject value)
			{
				lock (this)
				{
					value.NextPooledObject = this._stack;
					this._stack = value;
				}
			}

			// Token: 0x04001BFF RID: 7167
			private TdsParserStateObject _stack;
		}
	}
}
