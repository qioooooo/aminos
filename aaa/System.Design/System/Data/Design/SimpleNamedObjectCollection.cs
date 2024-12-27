using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x020000B7 RID: 183
	internal class SimpleNamedObjectCollection : ArrayList, INamedObjectCollection, ICollection, IEnumerable
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x00014F74 File Offset: 0x00013F74
		protected virtual INameService NameService
		{
			get
			{
				if (SimpleNamedObjectCollection.myNameService == null)
				{
					SimpleNamedObjectCollection.myNameService = new SimpleNameService();
				}
				return SimpleNamedObjectCollection.myNameService;
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00014F8C File Offset: 0x00013F8C
		public INameService GetNameService()
		{
			return this.NameService;
		}

		// Token: 0x04000C07 RID: 3079
		private static SimpleNameService myNameService;
	}
}
