using System;
using System.Collections;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036A RID: 874
	internal class SurrogateHashtable : Hashtable
	{
		// Token: 0x060022B2 RID: 8882 RVA: 0x00058396 File Offset: 0x00057396
		internal SurrogateHashtable(int size)
			: base(size)
		{
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000583A0 File Offset: 0x000573A0
		protected override bool KeyEquals(object key, object item)
		{
			SurrogateKey surrogateKey = (SurrogateKey)item;
			SurrogateKey surrogateKey2 = (SurrogateKey)key;
			return surrogateKey2.m_type == surrogateKey.m_type && (surrogateKey2.m_context.m_state & surrogateKey.m_context.m_state) == surrogateKey.m_context.m_state && surrogateKey2.m_context.Context == surrogateKey.m_context.Context;
		}
	}
}
