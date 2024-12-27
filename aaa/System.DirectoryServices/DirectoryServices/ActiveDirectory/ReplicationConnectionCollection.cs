using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DB RID: 219
	public class ReplicationConnectionCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060006D1 RID: 1745 RVA: 0x000247D8 File Offset: 0x000237D8
		internal ReplicationConnectionCollection()
		{
		}

		// Token: 0x1700019B RID: 411
		public ReplicationConnection this[int index]
		{
			get
			{
				return (ReplicationConnection)base.InnerList[index];
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000247F4 File Offset: 0x000237F4
		public bool Contains(ReplicationConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (!connection.existingConnection)
			{
				throw new InvalidOperationException(Res.GetString("ConnectionNotCommitted", new object[] { connection.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(connection.context, connection.cachedDirectoryEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ReplicationConnection replicationConnection = (ReplicationConnection)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(replicationConnection.context, replicationConnection.cachedDirectoryEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000248A8 File Offset: 0x000238A8
		public int IndexOf(ReplicationConnection connection)
		{
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			if (!connection.existingConnection)
			{
				throw new InvalidOperationException(Res.GetString("ConnectionNotCommitted", new object[] { connection.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(connection.context, connection.cachedDirectoryEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ReplicationConnection replicationConnection = (ReplicationConnection)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(replicationConnection.context, replicationConnection.cachedDirectoryEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002495B File Offset: 0x0002395B
		public void CopyTo(ReplicationConnection[] connections, int index)
		{
			base.InnerList.CopyTo(connections, index);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002496A File Offset: 0x0002396A
		internal int Add(ReplicationConnection value)
		{
			return base.InnerList.Add(value);
		}
	}
}
