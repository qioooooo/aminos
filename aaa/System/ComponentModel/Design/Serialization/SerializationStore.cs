using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001B5 RID: 437
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class SerializationStore : IDisposable
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000D5A RID: 3418
		public abstract ICollection Errors { get; }

		// Token: 0x06000D5B RID: 3419
		public abstract void Close();

		// Token: 0x06000D5C RID: 3420
		public abstract void Save(Stream stream);

		// Token: 0x06000D5D RID: 3421 RVA: 0x0002AD63 File Offset: 0x00029D63
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0002AD6C File Offset: 0x00029D6C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}
	}
}
