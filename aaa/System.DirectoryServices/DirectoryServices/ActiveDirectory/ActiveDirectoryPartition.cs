using System;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200006C RID: 108
	public abstract class ActiveDirectoryPartition : IDisposable
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00008880 File Offset: 0x00007880
		protected ActiveDirectoryPartition()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008888 File Offset: 0x00007888
		internal ActiveDirectoryPartition(DirectoryContext context, string name)
		{
			this.context = context;
			this.partitionName = name;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000889E File Offset: 0x0000789E
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000088A8 File Offset: 0x000078A8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (object obj in this.directoryEntryMgr.GetCachedDirectoryEntries())
					{
						DirectoryEntry directoryEntry = (DirectoryEntry)obj;
						directoryEntry.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008918 File Offset: 0x00007918
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000275 RID: 629
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract DirectoryEntry GetDirectoryEntry();

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00008920 File Offset: 0x00007920
		public string Name
		{
			get
			{
				this.CheckIfDisposed();
				return this.partitionName;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000892E File Offset: 0x0000792E
		internal void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x0400029D RID: 669
		private bool disposed;

		// Token: 0x0400029E RID: 670
		internal string partitionName;

		// Token: 0x0400029F RID: 671
		internal DirectoryContext context;

		// Token: 0x040002A0 RID: 672
		internal DirectoryEntryManager directoryEntryMgr;
	}
}
