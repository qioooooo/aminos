using System;
using System.Collections;
using System.Globalization;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000389 RID: 905
	internal class ConnectionPoolManager
	{
		// Token: 0x06001C2A RID: 7210 RVA: 0x0006A19C File Offset: 0x0006919C
		private ConnectionPoolManager()
		{
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x0006A1A4 File Offset: 0x000691A4
		private static object InternalSyncObject
		{
			get
			{
				if (ConnectionPoolManager.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ConnectionPoolManager.s_InternalSyncObject, obj, null);
				}
				return ConnectionPoolManager.s_InternalSyncObject;
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0006A1D0 File Offset: 0x000691D0
		private static string GenerateKey(string hostName, int port, string groupName)
		{
			return string.Concat(new string[]
			{
				hostName,
				"\r",
				port.ToString(NumberFormatInfo.InvariantInfo),
				"\r",
				groupName
			});
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0006A214 File Offset: 0x00069214
		internal static ConnectionPool GetConnectionPool(ServicePoint servicePoint, string groupName, CreateConnectionDelegate createConnectionCallback)
		{
			string text = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			ConnectionPool connectionPool2;
			lock (ConnectionPoolManager.InternalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[text];
				if (connectionPool == null)
				{
					connectionPool = new ConnectionPool(servicePoint, servicePoint.ConnectionLimit, 0, servicePoint.MaxIdleTime, createConnectionCallback);
					ConnectionPoolManager.m_ConnectionPools[text] = connectionPool;
				}
				connectionPool2 = connectionPool;
			}
			return connectionPool2;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0006A294 File Offset: 0x00069294
		internal static bool RemoveConnectionPool(ServicePoint servicePoint, string groupName)
		{
			string text = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			lock (ConnectionPoolManager.InternalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[text];
				if (connectionPool != null)
				{
					ConnectionPoolManager.m_ConnectionPools[text] = null;
					ConnectionPoolManager.m_ConnectionPools.Remove(text);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001CC6 RID: 7366
		private static Hashtable m_ConnectionPools = new Hashtable();

		// Token: 0x04001CC7 RID: 7367
		private static object s_InternalSyncObject;
	}
}
