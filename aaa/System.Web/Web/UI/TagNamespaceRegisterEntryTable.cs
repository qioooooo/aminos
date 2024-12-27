using System;
using System.Collections;

namespace System.Web.UI
{
	// Token: 0x0200038C RID: 908
	internal class TagNamespaceRegisterEntryTable : Hashtable
	{
		// Token: 0x06002C4B RID: 11339 RVA: 0x000C5D3A File Offset: 0x000C4D3A
		public TagNamespaceRegisterEntryTable()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000C5D48 File Offset: 0x000C4D48
		public override object Clone()
		{
			TagNamespaceRegisterEntryTable tagNamespaceRegisterEntryTable = new TagNamespaceRegisterEntryTable();
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				tagNamespaceRegisterEntryTable[dictionaryEntry.Key] = ((ArrayList)dictionaryEntry.Value).Clone();
			}
			return tagNamespaceRegisterEntryTable;
		}
	}
}
