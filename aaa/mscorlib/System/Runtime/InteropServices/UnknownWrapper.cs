using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000551 RID: 1361
	[ComVisible(true)]
	[Serializable]
	public sealed class UnknownWrapper
	{
		// Token: 0x06003368 RID: 13160 RVA: 0x000AD8D0 File Offset: 0x000AC8D0
		public UnknownWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x000AD8DF File Offset: 0x000AC8DF
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04001ABD RID: 6845
		private object m_WrappedObject;
	}
}
