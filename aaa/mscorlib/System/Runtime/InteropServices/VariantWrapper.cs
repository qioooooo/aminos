using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000552 RID: 1362
	[Serializable]
	public sealed class VariantWrapper
	{
		// Token: 0x0600336A RID: 13162 RVA: 0x000AD8E7 File Offset: 0x000AC8E7
		public VariantWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000AD8F6 File Offset: 0x000AC8F6
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04001ABE RID: 6846
		private object m_WrappedObject;
	}
}
