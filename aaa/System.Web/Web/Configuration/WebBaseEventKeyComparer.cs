using System;
using System.Collections;

namespace System.Web.Configuration
{
	// Token: 0x02000265 RID: 613
	internal class WebBaseEventKeyComparer : IEqualityComparer
	{
		// Token: 0x06002043 RID: 8259 RVA: 0x0008CE18 File Offset: 0x0008BE18
		public bool Equals(object x, object y)
		{
			CustomWebEventKey customWebEventKey = (CustomWebEventKey)x;
			CustomWebEventKey customWebEventKey2 = (CustomWebEventKey)y;
			return customWebEventKey._eventCode == customWebEventKey2._eventCode && customWebEventKey._type.Equals(customWebEventKey2._type);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0008CE57 File Offset: 0x0008BE57
		public int GetHashCode(object obj)
		{
			return ((CustomWebEventKey)obj)._eventCode ^ ((CustomWebEventKey)obj)._type.GetHashCode();
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0008CE78 File Offset: 0x0008BE78
		public int Compare(object x, object y)
		{
			CustomWebEventKey customWebEventKey = (CustomWebEventKey)x;
			CustomWebEventKey customWebEventKey2 = (CustomWebEventKey)y;
			int eventCode = customWebEventKey._eventCode;
			int eventCode2 = customWebEventKey2._eventCode;
			if (eventCode == eventCode2)
			{
				Type type = customWebEventKey._type;
				Type type2 = customWebEventKey2._type;
				if (type.Equals(type2))
				{
					return 0;
				}
				return Comparer.Default.Compare(type.ToString(), type2.ToString());
			}
			else
			{
				if (eventCode > eventCode2)
				{
					return 1;
				}
				return -1;
			}
		}
	}
}
