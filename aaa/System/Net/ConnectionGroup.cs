using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004CD RID: 1229
	internal class ConnectionGroup
	{
		// Token: 0x060025F9 RID: 9721 RVA: 0x00099264 File Offset: 0x00098264
		internal ConnectionGroup(ServicePoint servicePoint, string connName)
		{
			this.m_ServicePoint = servicePoint;
			this.m_ConnectionLimit = servicePoint.ConnectionLimit;
			this.m_ConnectionList = new ArrayList(3);
			this.m_Name = ConnectionGroup.MakeQueryStr(connName);
			this.m_AbortDelegate = new HttpAbortDelegate(this.Abort);
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x000992BB File Offset: 0x000982BB
		internal ServicePoint ServicePoint
		{
			get
			{
				return this.m_ServicePoint;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x000992C3 File Offset: 0x000982C3
		internal int CurrentConnections
		{
			get
			{
				return this.m_ConnectionList.Count;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000992D0 File Offset: 0x000982D0
		// (set) Token: 0x060025FD RID: 9725 RVA: 0x000992D8 File Offset: 0x000982D8
		internal int ConnectionLimit
		{
			get
			{
				return this.m_ConnectionLimit;
			}
			set
			{
				this.m_ConnectionLimit = value;
				this.PruneExcesiveConnections();
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x000992E8 File Offset: 0x000982E8
		private ManualResetEvent AsyncWaitHandle
		{
			get
			{
				if (this.m_Event == null)
				{
					Interlocked.CompareExchange(ref this.m_Event, new ManualResetEvent(false), null);
				}
				return (ManualResetEvent)this.m_Event;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x00099320 File Offset: 0x00098320
		// (set) Token: 0x06002600 RID: 9728 RVA: 0x00099374 File Offset: 0x00098374
		private Queue AuthenticationRequestQueue
		{
			get
			{
				if (this.m_AuthenticationRequestQueue == null)
				{
					lock (this.m_ConnectionList)
					{
						if (this.m_AuthenticationRequestQueue == null)
						{
							this.m_AuthenticationRequestQueue = new Queue();
						}
					}
				}
				return this.m_AuthenticationRequestQueue;
			}
			set
			{
				this.m_AuthenticationRequestQueue = value;
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0009937D File Offset: 0x0009837D
		internal static string MakeQueryStr(string connName)
		{
			if (connName != null)
			{
				return connName;
			}
			return "";
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0009938C File Offset: 0x0009838C
		internal void Associate(Connection connection)
		{
			lock (this.m_ConnectionList)
			{
				this.m_ConnectionList.Add(connection);
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000993CC File Offset: 0x000983CC
		internal void Disassociate(Connection connection)
		{
			lock (this.m_ConnectionList)
			{
				this.m_ConnectionList.Remove(connection);
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0009940C File Offset: 0x0009840C
		internal void ConnectionGoneIdle()
		{
			if (this.m_AuthenticationGroup)
			{
				lock (this.m_ConnectionList)
				{
					this.AsyncWaitHandle.Set();
				}
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00099454 File Offset: 0x00098454
		private bool Abort(HttpWebRequest request, WebException webException)
		{
			lock (this.m_ConnectionList)
			{
				this.AsyncWaitHandle.Set();
			}
			return true;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00099494 File Offset: 0x00098494
		private void PruneAbortedRequests()
		{
			lock (this.m_ConnectionList)
			{
				Queue queue = new Queue();
				foreach (object obj in this.AuthenticationRequestQueue)
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
					if (!httpWebRequest.Aborted)
					{
						queue.Enqueue(httpWebRequest);
					}
				}
				this.AuthenticationRequestQueue = queue;
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00099528 File Offset: 0x00098528
		private void PruneExcesiveConnections()
		{
			ArrayList arrayList = new ArrayList();
			lock (this.m_ConnectionList)
			{
				int connectionLimit = this.ConnectionLimit;
				if (this.CurrentConnections > connectionLimit)
				{
					int num = this.CurrentConnections - connectionLimit;
					for (int i = 0; i < num; i++)
					{
						arrayList.Add(this.m_ConnectionList[i]);
					}
					this.m_ConnectionList.RemoveRange(0, num);
				}
			}
			foreach (object obj in arrayList)
			{
				Connection connection = (Connection)obj;
				connection.CloseOnIdle();
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000995F4 File Offset: 0x000985F4
		internal void DisableKeepAliveOnConnections()
		{
			ArrayList arrayList = new ArrayList();
			lock (this.m_ConnectionList)
			{
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection = (Connection)obj;
					arrayList.Add(connection);
				}
				this.m_ConnectionList.Clear();
			}
			foreach (object obj2 in arrayList)
			{
				Connection connection2 = (Connection)obj2;
				connection2.CloseOnIdle();
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000996D4 File Offset: 0x000986D4
		private Connection FindMatchingConnection(HttpWebRequest request, string connName, out Connection leastbusyConnection)
		{
			bool flag = false;
			leastbusyConnection = null;
			lock (this.m_ConnectionList)
			{
				int num = int.MaxValue;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection = (Connection)obj;
					if (connection.LockedRequest == request)
					{
						leastbusyConnection = connection;
						return connection;
					}
					if (connection.BusyCount < num && connection.LockedRequest == null)
					{
						leastbusyConnection = connection;
						num = connection.BusyCount;
						if (num == 0)
						{
							flag = true;
						}
					}
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					leastbusyConnection = new Connection(this);
				}
			}
			return null;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000997B0 File Offset: 0x000987B0
		private Connection FindConnectionAuthenticationGroup(HttpWebRequest request, string connName)
		{
			Connection connection = null;
			lock (this.m_ConnectionList)
			{
				Connection connection2 = this.FindMatchingConnection(request, connName, out connection);
				if (connection2 != null)
				{
					connection2.MarkAsReserved();
					return connection2;
				}
				if (this.AuthenticationRequestQueue.Count == 0)
				{
					if (connection != null)
					{
						if (request.LockConnection)
						{
							this.m_NtlmNegGroup = true;
							this.m_IISVersion = connection.IISVersion;
						}
						if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
						{
							connection.LockedRequest = request;
						}
						connection.MarkAsReserved();
						return connection;
					}
				}
				else if (connection != null)
				{
					this.AsyncWaitHandle.Set();
				}
				this.AuthenticationRequestQueue.Enqueue(request);
			}
			Connection connection3;
			for (;;)
			{
				request.AbortDelegate = this.m_AbortDelegate;
				if (!request.Aborted)
				{
					this.AsyncWaitHandle.WaitOne();
				}
				lock (this.m_ConnectionList)
				{
					if (!request.Aborted)
					{
						this.FindMatchingConnection(request, connName, out connection);
						if (this.AuthenticationRequestQueue.Peek() == request)
						{
							this.AuthenticationRequestQueue.Dequeue();
							if (connection != null)
							{
								if (request.LockConnection)
								{
									this.m_NtlmNegGroup = true;
									this.m_IISVersion = connection.IISVersion;
								}
								if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
								{
									connection.LockedRequest = request;
								}
								connection.MarkAsReserved();
								connection3 = connection;
								break;
							}
							this.AuthenticationRequestQueue.Enqueue(request);
						}
						if (connection == null)
						{
							this.AsyncWaitHandle.Reset();
						}
						continue;
					}
					this.PruneAbortedRequests();
					connection3 = null;
				}
				break;
			}
			return connection3;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00099978 File Offset: 0x00098978
		internal Connection FindConnection(HttpWebRequest request, string connName)
		{
			Connection connection = null;
			Connection connection2 = null;
			bool flag = false;
			if (this.m_AuthenticationGroup || request.LockConnection)
			{
				this.m_AuthenticationGroup = true;
				return this.FindConnectionAuthenticationGroup(request, connName);
			}
			lock (this.m_ConnectionList)
			{
				int num = int.MaxValue;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection3 = (Connection)obj;
					if (connection3.BusyCount < num)
					{
						connection = connection3;
						num = connection3.BusyCount;
						if (num == 0)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					connection2 = new Connection(this);
				}
				else
				{
					connection2 = connection;
				}
				connection2.MarkAsReserved();
			}
			return connection2;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00099A64 File Offset: 0x00098A64
		[Conditional("DEBUG")]
		internal void Debug(int requestHash)
		{
			foreach (object obj in this.m_ConnectionList)
			{
				Connection connection = (Connection)obj;
			}
		}

		// Token: 0x040025B3 RID: 9651
		private const int DefaultConnectionListSize = 3;

		// Token: 0x040025B4 RID: 9652
		private ServicePoint m_ServicePoint;

		// Token: 0x040025B5 RID: 9653
		private string m_Name;

		// Token: 0x040025B6 RID: 9654
		private int m_ConnectionLimit;

		// Token: 0x040025B7 RID: 9655
		private ArrayList m_ConnectionList;

		// Token: 0x040025B8 RID: 9656
		private object m_Event;

		// Token: 0x040025B9 RID: 9657
		private Queue m_AuthenticationRequestQueue;

		// Token: 0x040025BA RID: 9658
		internal bool m_AuthenticationGroup;

		// Token: 0x040025BB RID: 9659
		private HttpAbortDelegate m_AbortDelegate;

		// Token: 0x040025BC RID: 9660
		private bool m_NtlmNegGroup;

		// Token: 0x040025BD RID: 9661
		private int m_IISVersion = -1;
	}
}
