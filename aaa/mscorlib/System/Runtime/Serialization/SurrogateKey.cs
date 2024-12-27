using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000369 RID: 873
	[Serializable]
	internal class SurrogateKey
	{
		// Token: 0x060022B0 RID: 8880 RVA: 0x00058373 File Offset: 0x00057373
		internal SurrogateKey(Type type, StreamingContext context)
		{
			this.m_type = type;
			this.m_context = context;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00058389 File Offset: 0x00057389
		public override int GetHashCode()
		{
			return this.m_type.GetHashCode();
		}

		// Token: 0x04000E6C RID: 3692
		internal Type m_type;

		// Token: 0x04000E6D RID: 3693
		internal StreamingContext m_context;
	}
}
