using System;
using System.Collections;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x020000CE RID: 206
	[Serializable]
	public class PropertyCollection : Hashtable
	{
		// Token: 0x06000CDC RID: 3292 RVA: 0x001FC30C File Offset: 0x001FB70C
		public PropertyCollection()
		{
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x001FC320 File Offset: 0x001FB720
		protected PropertyCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
