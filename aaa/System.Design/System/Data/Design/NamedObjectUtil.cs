using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x020000AB RID: 171
	internal class NamedObjectUtil
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x00012AC9 File Offset: 0x00011AC9
		private NamedObjectUtil()
		{
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00012AD1 File Offset: 0x00011AD1
		public static INamedObject Find(INamedObjectCollection coll, string name)
		{
			return NamedObjectUtil.Find(coll, name, false);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00012ADC File Offset: 0x00011ADC
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
