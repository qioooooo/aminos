using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000378 RID: 888
	internal class PrefixLookup
	{
		// Token: 0x06001BD6 RID: 7126 RVA: 0x00069414 File Offset: 0x00068414
		internal void Add(string prefix, object value)
		{
			lock (this.m_Store)
			{
				this.m_Store[prefix] = value;
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00069454 File Offset: 0x00068454
		internal object Lookup(string lookupKey)
		{
			if (lookupKey == null)
			{
				return null;
			}
			object obj = null;
			int num = 0;
			lock (this.m_Store)
			{
				foreach (object obj2 in this.m_Store)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					string text = (string)dictionaryEntry.Key;
					if (lookupKey.StartsWith(text))
					{
						int length = text.Length;
						if (length > num)
						{
							num = length;
							obj = dictionaryEntry.Value;
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x04001C82 RID: 7298
		private Hashtable m_Store = new Hashtable();
	}
}
