using System;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000B9 RID: 185
	internal class PartitionInfo : IDisposable, IPartitionInfo
	{
		// Token: 0x060008AC RID: 2220 RVA: 0x00027AA0 File Offset: 0x00026AA0
		internal PartitionInfo(ResourcePool rpool)
		{
			this._rpool = rpool;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00027AAF File Offset: 0x00026AAF
		internal object RetrieveResource()
		{
			return this._rpool.RetrieveResource();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00027ABC File Offset: 0x00026ABC
		internal void StoreResource(IDisposable o)
		{
			this._rpool.StoreResource(o);
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00027ACA File Offset: 0x00026ACA
		protected virtual string TracingPartitionString
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00027AD1 File Offset: 0x00026AD1
		string IPartitionInfo.GetTracingPartitionString()
		{
			return this.TracingPartitionString;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00027ADC File Offset: 0x00026ADC
		public void Dispose()
		{
			if (this._rpool == null)
			{
				return;
			}
			lock (this)
			{
				if (this._rpool != null)
				{
					this._rpool.Dispose();
					this._rpool = null;
				}
			}
		}

		// Token: 0x040011E4 RID: 4580
		private ResourcePool _rpool;
	}
}
