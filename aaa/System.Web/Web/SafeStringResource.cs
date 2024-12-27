using System;

namespace System.Web
{
	// Token: 0x020000D9 RID: 217
	internal class SafeStringResource
	{
		// Token: 0x060009E4 RID: 2532 RVA: 0x0002B628 File Offset: 0x0002A628
		internal SafeStringResource(IntPtr stringResourcePointer, int resourceSize)
		{
			this._stringResourcePointer = stringResourcePointer;
			this._resourceSize = resourceSize;
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0002B63E File Offset: 0x0002A63E
		internal IntPtr StringResourcePointer
		{
			get
			{
				return this._stringResourcePointer;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002B646 File Offset: 0x0002A646
		internal int ResourceSize
		{
			get
			{
				return this._resourceSize;
			}
		}

		// Token: 0x04001266 RID: 4710
		private IntPtr _stringResourcePointer;

		// Token: 0x04001267 RID: 4711
		private int _resourceSize;
	}
}
