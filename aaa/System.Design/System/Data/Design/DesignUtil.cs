using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x0200009F RID: 159
	internal sealed class DesignUtil
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x0000F85A File Offset: 0x0000E85A
		private DesignUtil()
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000F864 File Offset: 0x0000E864
		internal static IDictionary CloneDictionary(IDictionary source)
		{
			if (source == null)
			{
				return null;
			}
			if (source is ICloneable)
			{
				return (IDictionary)((ICloneable)source).Clone();
			}
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(source.GetType());
			IDictionaryEnumerator enumerator = source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ICloneable cloneable = enumerator.Key as ICloneable;
				ICloneable cloneable2 = enumerator.Value as ICloneable;
				if (cloneable != null && cloneable2 != null)
				{
					dictionary.Add(cloneable.Clone(), cloneable2.Clone());
				}
			}
			return dictionary;
		}
	}
}
