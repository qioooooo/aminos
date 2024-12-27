using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000703 RID: 1795
	internal class MessageDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x0600408B RID: 16523 RVA: 0x000DC95F File Offset: 0x000DB95F
		public MessageDictionaryEnumerator(MessageDictionary md, IDictionary hashtable)
		{
			this._md = md;
			if (hashtable != null)
			{
				this._enumHash = hashtable.GetEnumerator();
				return;
			}
			this._enumHash = null;
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x000DC98C File Offset: 0x000DB98C
		public object Key
		{
			get
			{
				if (this.i < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
				}
				if (this.i < this._md._keys.Length)
				{
					return this._md._keys[this.i];
				}
				return this._enumHash.Key;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600408D RID: 16525 RVA: 0x000DC9E8 File Offset: 0x000DB9E8
		public object Value
		{
			get
			{
				if (this.i < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
				}
				if (this.i < this._md._keys.Length)
				{
					return this._md.GetMessageValue(this.i);
				}
				return this._enumHash.Value;
			}
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x000DCA40 File Offset: 0x000DBA40
		public bool MoveNext()
		{
			if (this.i == -2)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
			this.i++;
			if (this.i < this._md._keys.Length)
			{
				return true;
			}
			if (this._enumHash != null && this._enumHash.MoveNext())
			{
				return true;
			}
			this.i = -2;
			return false;
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x000DCAAC File Offset: 0x000DBAAC
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x000DCAB9 File Offset: 0x000DBAB9
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x000DCACC File Offset: 0x000DBACC
		public void Reset()
		{
			this.i = -1;
			if (this._enumHash != null)
			{
				this._enumHash.Reset();
			}
		}

		// Token: 0x04002065 RID: 8293
		private int i = -1;

		// Token: 0x04002066 RID: 8294
		private IDictionaryEnumerator _enumHash;

		// Token: 0x04002067 RID: 8295
		private MessageDictionary _md;
	}
}
