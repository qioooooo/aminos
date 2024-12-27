using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Web.UI
{
	// Token: 0x02000438 RID: 1080
	internal static class OrderedDictionaryStateHelper
	{
		// Token: 0x060033A8 RID: 13224 RVA: 0x000E0D90 File Offset: 0x000DFD90
		public static void LoadViewState(IOrderedDictionary dictionary, ArrayList state)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (state != null)
			{
				for (int i = 0; i < state.Count; i++)
				{
					Pair pair = (Pair)state[i];
					dictionary.Add(pair.First, pair.Second);
				}
			}
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000E0DEC File Offset: 0x000DFDEC
		public static ArrayList SaveViewState(IOrderedDictionary dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			ArrayList arrayList = new ArrayList(dictionary.Count);
			foreach (object obj in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				arrayList.Add(new Pair(dictionaryEntry.Key, dictionaryEntry.Value));
			}
			return arrayList;
		}
	}
}
