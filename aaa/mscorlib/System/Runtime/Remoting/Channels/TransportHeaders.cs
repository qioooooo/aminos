using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D9 RID: 1753
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class TransportHeaders : ITransportHeaders
	{
		// Token: 0x06003F0D RID: 16141 RVA: 0x000D81B7 File Offset: 0x000D71B7
		public TransportHeaders()
		{
			this._headerList = new ArrayList(6);
		}

		// Token: 0x17000AAA RID: 2730
		public object this[object key]
		{
			get
			{
				string text = (string)key;
				foreach (object obj in this._headerList)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (string.Compare((string)dictionaryEntry.Key, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return dictionaryEntry.Value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					return;
				}
				string text = (string)key;
				for (int i = this._headerList.Count - 1; i >= 0; i--)
				{
					string text2 = (string)((DictionaryEntry)this._headerList[i]).Key;
					if (string.Compare(text2, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						this._headerList.RemoveAt(i);
						break;
					}
				}
				if (value != null)
				{
					this._headerList.Add(new DictionaryEntry(key, value));
				}
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x000D82CE File Offset: 0x000D72CE
		public IEnumerator GetEnumerator()
		{
			return this._headerList.GetEnumerator();
		}

		// Token: 0x04001FCA RID: 8138
		private ArrayList _headerList;
	}
}
