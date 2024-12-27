using System;

namespace System.Data.ProviderBase
{
	// Token: 0x020000A0 RID: 160
	internal abstract class DbReferenceCollection
	{
		// Token: 0x06000871 RID: 2161
		public abstract void Add(object value, int tag);

		// Token: 0x06000872 RID: 2162
		public abstract void Notify(int message);

		// Token: 0x06000873 RID: 2163
		public abstract void Remove(object value);
	}
}
