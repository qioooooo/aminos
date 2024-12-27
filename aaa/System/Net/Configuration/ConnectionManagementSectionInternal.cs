using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200064C RID: 1612
	internal sealed class ConnectionManagementSectionInternal
	{
		// Token: 0x060031E5 RID: 12773 RVA: 0x000D4E0C File Offset: 0x000D3E0C
		internal ConnectionManagementSectionInternal(ConnectionManagementSection section)
		{
			if (section.ConnectionManagement.Count > 0)
			{
				this.connectionManagement = new Hashtable(section.ConnectionManagement.Count);
				foreach (object obj in section.ConnectionManagement)
				{
					ConnectionManagementElement connectionManagementElement = (ConnectionManagementElement)obj;
					this.connectionManagement[connectionManagementElement.Address] = connectionManagementElement.MaxConnection;
				}
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x000D4EA4 File Offset: 0x000D3EA4
		internal Hashtable ConnectionManagement
		{
			get
			{
				Hashtable hashtable = this.connectionManagement;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
				}
				return hashtable;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x000D4EC4 File Offset: 0x000D3EC4
		internal static object ClassSyncObject
		{
			get
			{
				if (ConnectionManagementSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ConnectionManagementSectionInternal.classSyncObject, obj, null);
				}
				return ConnectionManagementSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000D4EF0 File Offset: 0x000D3EF0
		internal static ConnectionManagementSectionInternal GetSection()
		{
			ConnectionManagementSectionInternal connectionManagementSectionInternal;
			lock (ConnectionManagementSectionInternal.ClassSyncObject)
			{
				ConnectionManagementSection connectionManagementSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.ConnectionManagementSectionPath) as ConnectionManagementSection;
				if (connectionManagementSection == null)
				{
					connectionManagementSectionInternal = null;
				}
				else
				{
					connectionManagementSectionInternal = new ConnectionManagementSectionInternal(connectionManagementSection);
				}
			}
			return connectionManagementSectionInternal;
		}

		// Token: 0x04002EE9 RID: 12009
		private Hashtable connectionManagement;

		// Token: 0x04002EEA RID: 12010
		private static object classSyncObject;
	}
}
