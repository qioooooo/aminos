using System;
using System.Collections;

namespace System.Data.Design
{
	internal sealed class DesignUtil
	{
		private DesignUtil()
		{
		}

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
