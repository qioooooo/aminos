using System;
using System.Collections;

namespace System.Data.Design
{
	internal class NamedObjectUtil
	{
		private NamedObjectUtil()
		{
		}

		public static INamedObject Find(INamedObjectCollection coll, string name)
		{
			return NamedObjectUtil.Find(coll, name, false);
		}

		private static INamedObject Find(ICollection coll, string name, bool ignoreCase)
		{
			foreach (object obj in coll)
			{
				INamedObject namedObject = obj as INamedObject;
				if (namedObject == null)
				{
					throw new InternalException("Named object collection holds something that is not a named object", 2);
				}
				if (StringUtil.EqualValue(namedObject.Name, name, ignoreCase))
				{
					return namedObject;
				}
			}
			return null;
		}
	}
}
