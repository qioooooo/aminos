using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D3 RID: 211
	public class ReadOnlyDirectoryServerCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x00022D18 File Offset: 0x00021D18
		internal ReadOnlyDirectoryServerCollection()
		{
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00022D20 File Offset: 0x00021D20
		internal ReadOnlyDirectoryServerCollection(ArrayList values)
		{
			if (values != null)
			{
				for (int i = 0; i < values.Count; i++)
				{
					this.Add((DirectoryServer)values[i]);
				}
			}
		}

		// Token: 0x17000189 RID: 393
		public DirectoryServer this[int index]
		{
			get
			{
				return (DirectoryServer)base.InnerList[index];
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00022D70 File Offset: 0x00021D70
		public bool Contains(DirectoryServer directoryServer)
		{
			if (directoryServer == null)
			{
				throw new ArgumentNullException("directoryServer");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer2 = (DirectoryServer)base.InnerList[i];
				if (Utils.Compare(directoryServer2.Name, directoryServer.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00022DCC File Offset: 0x00021DCC
		public int IndexOf(DirectoryServer directoryServer)
		{
			if (directoryServer == null)
			{
				throw new ArgumentNullException("directoryServer");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer2 = (DirectoryServer)base.InnerList[i];
				if (Utils.Compare(directoryServer2.Name, directoryServer.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00022E25 File Offset: 0x00021E25
		public void CopyTo(DirectoryServer[] directoryServers, int index)
		{
			base.InnerList.CopyTo(directoryServers, index);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00022E34 File Offset: 0x00021E34
		internal int Add(DirectoryServer server)
		{
			return base.InnerList.Add(server);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00022E42 File Offset: 0x00021E42
		internal void AddRange(ICollection servers)
		{
			base.InnerList.AddRange(servers);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00022E50 File Offset: 0x00021E50
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
