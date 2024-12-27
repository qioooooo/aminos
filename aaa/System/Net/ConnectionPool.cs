using System;
using System.Collections;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000385 RID: 901
	internal class ConnectionPool
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00069691 File Offset: 0x00068691
		private Mutex CreationMutex
		{
			get
			{
				return (Mutex)this.m_WaitHandles[2];
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x000696A0 File Offset: 0x000686A0
		private ManualResetEvent ErrorEvent
		{
			get
			{
				return (ManualResetEvent)this.m_WaitHandles[1];
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000696AF File Offset: 0x000686AF
		private Semaphore Semaphore
		{
			get
			{
				return (Semaphore)this.m_WaitHandles[0];
			}
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000696C0 File Offset: 0x000686C0
		internal ConnectionPool(ServicePoint servicePoint, int maxPoolSize, int minPoolSize, int idleTimeout, CreateConnectionDelegate createConnectionCallback)
		{
			this.m_State = ConnectionPool.State.Initializing;
			this.m_CreateConnectionCallback = createConnectionCallback;
			this.m_MaxPoolSize = maxPoolSize;
			this.m_MinPoolSize = minPoolSize;
			this.m_ServicePoint = servicePoint;
			this.Initialize();
			if (idleTimeout > 0)
			{
				this.m_CleanupQueue = TimerThread.GetOrCreateQueue(idleTimeout / 2);
				this.m_CleanupQueue.CreateTimer(ConnectionPool.s_CleanupCallback, this);
			}
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00069724 File Offset: 0x00068724
		private void Initialize()
		{
			this.m_StackOld = new InterlockedStack();
			this.m_StackNew = new InterlockedStack();
			this.m_QueuedRequests = new Queue();
			this.m_WaitHandles = new WaitHandle[3];
			this.m_WaitHandles[0] = new Semaphore(0, 1048576);
			this.m_WaitHandles[1] = new ManualResetEvent(false);
			this.m_WaitHandles[2] = new Mutex();
			this.m_ErrorTimer = null;
			this.m_ObjectList = new ArrayList();
			this.m_State = ConnectionPool.State.Running;
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000697A8 File Offset: 0x000687A8
		private void QueueRequest(ConnectionPool.AsyncConnectionPoolRequest asyncRequest)
		{
			lock (this.m_QueuedRequests)
			{
				this.m_QueuedRequests.Enqueue(asyncRequest);
				if (this.m_AsyncThread == null)
				{
					this.m_AsyncThread = new Thread(new ThreadStart(this.AsyncThread));
					this.m_AsyncThread.IsBackground = true;
					this.m_AsyncThread.Start();
				}
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00069820 File Offset: 0x00068820
		private void AsyncThread()
		{
			for (;;)
			{
				if (this.m_QueuedRequests.Count <= 0)
				{
					Thread.Sleep(500);
					lock (this.m_QueuedRequests)
					{
						if (this.m_QueuedRequests.Count != 0)
						{
							continue;
						}
						this.m_AsyncThread = null;
					}
					break;
				}
				bool flag = true;
				ConnectionPool.AsyncConnectionPoolRequest asyncConnectionPoolRequest = null;
				lock (this.m_QueuedRequests)
				{
					asyncConnectionPoolRequest = (ConnectionPool.AsyncConnectionPoolRequest)this.m_QueuedRequests.Dequeue();
				}
				WaitHandle[] waitHandles = this.m_WaitHandles;
				PooledStream pooledStream = null;
				try
				{
					while (pooledStream == null && flag)
					{
						int num = WaitHandle.WaitAny(waitHandles, asyncConnectionPoolRequest.CreationTimeout, false);
						pooledStream = this.Get(asyncConnectionPoolRequest.OwningObject, num, ref flag, ref waitHandles);
					}
					pooledStream.Activate(asyncConnectionPoolRequest.OwningObject, asyncConnectionPoolRequest.AsyncCallback);
				}
				catch (Exception ex)
				{
					if (pooledStream != null)
					{
						pooledStream.Close();
						this.PutConnection(pooledStream, asyncConnectionPoolRequest.OwningObject, asyncConnectionPoolRequest.CreationTimeout);
					}
					asyncConnectionPoolRequest.AsyncCallback(asyncConnectionPoolRequest.OwningObject, ex);
				}
				catch
				{
					if (pooledStream != null)
					{
						pooledStream.Close();
						this.PutConnection(pooledStream, asyncConnectionPoolRequest.OwningObject, asyncConnectionPoolRequest.CreationTimeout);
					}
					asyncConnectionPoolRequest.AsyncCallback(asyncConnectionPoolRequest.OwningObject, new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x00069998 File Offset: 0x00068998
		internal int Count
		{
			get
			{
				return this.m_TotalObjects;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x000699A0 File Offset: 0x000689A0
		internal ServicePoint ServicePoint
		{
			get
			{
				return this.m_ServicePoint;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000699A8 File Offset: 0x000689A8
		internal int MaxPoolSize
		{
			get
			{
				return this.m_MaxPoolSize;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000699B0 File Offset: 0x000689B0
		internal int MinPoolSize
		{
			get
			{
				return this.m_MinPoolSize;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000699B8 File Offset: 0x000689B8
		private bool ErrorOccurred
		{
			get
			{
				return this.m_ErrorOccured;
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000699C4 File Offset: 0x000689C4
		private static void CleanupCallbackWrapper(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ConnectionPool connectionPool = (ConnectionPool)context;
			try
			{
				connectionPool.CleanupCallback();
			}
			finally
			{
				connectionPool.m_CleanupQueue.CreateTimer(ConnectionPool.s_CleanupCallback, context);
			}
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00069A04 File Offset: 0x00068A04
		private void CleanupCallback()
		{
			while (this.Count > this.MinPoolSize && this.Semaphore.WaitOne(0, false))
			{
				PooledStream pooledStream = (PooledStream)this.m_StackOld.Pop();
				if (pooledStream == null)
				{
					this.Semaphore.ReleaseSemaphore();
					break;
				}
				this.Destroy(pooledStream);
			}
			if (this.Semaphore.WaitOne(0, false))
			{
				for (;;)
				{
					PooledStream pooledStream2 = (PooledStream)this.m_StackNew.Pop();
					if (pooledStream2 == null)
					{
						break;
					}
					this.m_StackOld.Push(pooledStream2);
				}
				this.Semaphore.ReleaseSemaphore();
			}
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00069A98 File Offset: 0x00068A98
		private PooledStream Create(CreateConnectionDelegate createConnectionCallback)
		{
			PooledStream pooledStream = null;
			try
			{
				pooledStream = createConnectionCallback(this);
				if (pooledStream == null)
				{
					throw new InternalException();
				}
				if (!pooledStream.CanBePooled)
				{
					throw new InternalException();
				}
				pooledStream.PrePush(null);
				lock (this.m_ObjectList.SyncRoot)
				{
					this.m_ObjectList.Add(pooledStream);
					this.m_TotalObjects = this.m_ObjectList.Count;
				}
			}
			catch (Exception ex)
			{
				pooledStream = null;
				this.m_ResError = ex;
				this.Abort();
			}
			catch
			{
				pooledStream = null;
				this.m_ResError = new Exception(SR.GetString("net_nonClsCompliantException"));
				this.Abort();
			}
			return pooledStream;
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00069B64 File Offset: 0x00068B64
		private void Destroy(PooledStream pooledStream)
		{
			try
			{
				lock (this.m_ObjectList.SyncRoot)
				{
					this.m_ObjectList.Remove(pooledStream);
					this.m_TotalObjects = this.m_ObjectList.Count;
				}
			}
			finally
			{
				if (pooledStream != null)
				{
					pooledStream.Destroy();
				}
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00069BD4 File Offset: 0x00068BD4
		private static void CancelErrorCallbackWrapper(TimerThread.Timer timer, int timeNoticed, object context)
		{
			((ConnectionPool)context).CancelErrorCallback();
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x00069BE4 File Offset: 0x00068BE4
		private void CancelErrorCallback()
		{
			TimerThread.Timer errorTimer = this.m_ErrorTimer;
			if (errorTimer != null && errorTimer.Cancel())
			{
				this.m_ErrorOccured = false;
				this.ErrorEvent.Reset();
				this.m_ErrorTimer = null;
				this.m_ResError = null;
			}
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x00069C28 File Offset: 0x00068C28
		private PooledStream GetFromPool(object owningObject)
		{
			PooledStream pooledStream = (PooledStream)this.m_StackNew.Pop();
			if (pooledStream == null)
			{
				pooledStream = (PooledStream)this.m_StackOld.Pop();
			}
			if (pooledStream != null)
			{
				pooledStream.PostPop(owningObject);
			}
			return pooledStream;
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00069C68 File Offset: 0x00068C68
		private PooledStream Get(object owningObject, int result, ref bool continueLoop, ref WaitHandle[] waitHandles)
		{
			PooledStream pooledStream = null;
			switch (result)
			{
			case 1:
			{
				int num = Interlocked.Decrement(ref this.m_WaitCount);
				continueLoop = false;
				Exception resError = this.m_ResError;
				if (num == 0)
				{
					this.CancelErrorCallback();
				}
				throw resError;
			}
			case 2:
				try
				{
					continueLoop = true;
					pooledStream = this.UserCreateRequest();
					if (pooledStream != null)
					{
						pooledStream.PostPop(owningObject);
						Interlocked.Decrement(ref this.m_WaitCount);
						continueLoop = false;
					}
					else if (this.Count >= this.MaxPoolSize && this.MaxPoolSize != 0 && !this.ReclaimEmancipatedObjects())
					{
						waitHandles = new WaitHandle[2];
						waitHandles[0] = this.m_WaitHandles[0];
						waitHandles[1] = this.m_WaitHandles[1];
					}
					return pooledStream;
				}
				finally
				{
					this.CreationMutex.ReleaseMutex();
				}
				break;
			default:
				if (result == 258)
				{
					Interlocked.Decrement(ref this.m_WaitCount);
					continueLoop = false;
					throw new WebException(NetRes.GetWebStatusString("net_timeout", WebExceptionStatus.ConnectFailure), WebExceptionStatus.Timeout);
				}
				break;
			}
			Interlocked.Decrement(ref this.m_WaitCount);
			pooledStream = this.GetFromPool(owningObject);
			continueLoop = false;
			return pooledStream;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00069D74 File Offset: 0x00068D74
		internal void Abort()
		{
			if (this.m_ResError == null)
			{
				this.m_ResError = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
			this.ErrorEvent.Set();
			this.m_ErrorOccured = true;
			this.m_ErrorTimer = ConnectionPool.s_CancelErrorQueue.CreateTimer(ConnectionPool.s_CancelErrorCallback, this);
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00069DCC File Offset: 0x00068DCC
		internal PooledStream GetConnection(object owningObject, GeneralAsyncDelegate asyncCallback, int creationTimeout)
		{
			PooledStream pooledStream = null;
			bool flag = true;
			bool flag2 = asyncCallback != null;
			if (this.m_State != ConnectionPool.State.Running)
			{
				throw new InternalException();
			}
			Interlocked.Increment(ref this.m_WaitCount);
			WaitHandle[] waitHandles = this.m_WaitHandles;
			if (flag2)
			{
				int num = WaitHandle.WaitAny(waitHandles, 0, false);
				if (num != 258)
				{
					pooledStream = this.Get(owningObject, num, ref flag, ref waitHandles);
				}
				if (pooledStream == null)
				{
					ConnectionPool.AsyncConnectionPoolRequest asyncConnectionPoolRequest = new ConnectionPool.AsyncConnectionPoolRequest(this, owningObject, asyncCallback, creationTimeout);
					this.QueueRequest(asyncConnectionPoolRequest);
				}
			}
			else
			{
				while (pooledStream == null && flag)
				{
					int num = WaitHandle.WaitAny(waitHandles, creationTimeout, false);
					pooledStream = this.Get(owningObject, num, ref flag, ref waitHandles);
				}
			}
			if (pooledStream != null)
			{
				if (!pooledStream.IsInitalizing)
				{
					asyncCallback = null;
				}
				try
				{
					if (!pooledStream.Activate(owningObject, asyncCallback))
					{
						pooledStream = null;
					}
					return pooledStream;
				}
				catch
				{
					pooledStream.Close();
					this.PutConnection(pooledStream, owningObject, creationTimeout);
					throw;
				}
			}
			if (!flag2)
			{
				throw new InternalException();
			}
			return pooledStream;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x00069EA8 File Offset: 0x00068EA8
		internal void PutConnection(PooledStream pooledStream, object owningObject, int creationTimeout)
		{
			if (pooledStream == null)
			{
				throw new ArgumentNullException("pooledStream");
			}
			pooledStream.PrePush(owningObject);
			if (this.m_State != ConnectionPool.State.ShuttingDown)
			{
				pooledStream.Deactivate();
				if (this.m_WaitCount == 0)
				{
					this.CancelErrorCallback();
				}
				if (pooledStream.CanBePooled)
				{
					this.PutNew(pooledStream);
					return;
				}
				this.Destroy(pooledStream);
				if (this.m_WaitCount <= 0)
				{
					return;
				}
				if (!this.CreationMutex.WaitOne(creationTimeout, false))
				{
					this.Abort();
					return;
				}
				try
				{
					pooledStream = this.UserCreateRequest();
					if (pooledStream != null)
					{
						this.PutNew(pooledStream);
					}
					return;
				}
				finally
				{
					this.CreationMutex.ReleaseMutex();
				}
			}
			this.Destroy(pooledStream);
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x00069F54 File Offset: 0x00068F54
		private void PutNew(PooledStream pooledStream)
		{
			this.m_StackNew.Push(pooledStream);
			this.Semaphore.ReleaseSemaphore();
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00069F70 File Offset: 0x00068F70
		private bool ReclaimEmancipatedObjects()
		{
			bool flag = false;
			lock (this.m_ObjectList.SyncRoot)
			{
				object[] array = this.m_ObjectList.ToArray();
				if (array != null)
				{
					foreach (PooledStream pooledStream in array)
					{
						if (pooledStream != null)
						{
							bool flag2 = false;
							try
							{
								flag2 = Monitor.TryEnter(pooledStream);
								if (flag2 && pooledStream.IsEmancipated)
								{
									this.PutConnection(pooledStream, null, -1);
									flag = true;
								}
							}
							finally
							{
								if (flag2)
								{
									Monitor.Exit(pooledStream);
								}
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0006A014 File Offset: 0x00069014
		private PooledStream UserCreateRequest()
		{
			PooledStream pooledStream = null;
			if (!this.ErrorOccurred && (this.Count < this.MaxPoolSize || this.MaxPoolSize == 0) && ((this.Count & 1) == 1 || !this.ReclaimEmancipatedObjects()))
			{
				pooledStream = this.Create(this.m_CreateConnectionCallback);
			}
			return pooledStream;
		}

		// Token: 0x04001CA0 RID: 7328
		private const int MaxQueueSize = 1048576;

		// Token: 0x04001CA1 RID: 7329
		private const int SemaphoreHandleIndex = 0;

		// Token: 0x04001CA2 RID: 7330
		private const int ErrorHandleIndex = 1;

		// Token: 0x04001CA3 RID: 7331
		private const int CreationHandleIndex = 2;

		// Token: 0x04001CA4 RID: 7332
		private const int WaitTimeout = 258;

		// Token: 0x04001CA5 RID: 7333
		private const int WaitAbandoned = 128;

		// Token: 0x04001CA6 RID: 7334
		private const int ErrorWait = 5000;

		// Token: 0x04001CA7 RID: 7335
		private static TimerThread.Callback s_CleanupCallback = new TimerThread.Callback(ConnectionPool.CleanupCallbackWrapper);

		// Token: 0x04001CA8 RID: 7336
		private static TimerThread.Callback s_CancelErrorCallback = new TimerThread.Callback(ConnectionPool.CancelErrorCallbackWrapper);

		// Token: 0x04001CA9 RID: 7337
		private static TimerThread.Queue s_CancelErrorQueue = TimerThread.GetOrCreateQueue(5000);

		// Token: 0x04001CAA RID: 7338
		private readonly TimerThread.Queue m_CleanupQueue;

		// Token: 0x04001CAB RID: 7339
		private ConnectionPool.State m_State;

		// Token: 0x04001CAC RID: 7340
		private InterlockedStack m_StackOld;

		// Token: 0x04001CAD RID: 7341
		private InterlockedStack m_StackNew;

		// Token: 0x04001CAE RID: 7342
		private int m_WaitCount;

		// Token: 0x04001CAF RID: 7343
		private WaitHandle[] m_WaitHandles;

		// Token: 0x04001CB0 RID: 7344
		private Exception m_ResError;

		// Token: 0x04001CB1 RID: 7345
		private volatile bool m_ErrorOccured;

		// Token: 0x04001CB2 RID: 7346
		private TimerThread.Timer m_ErrorTimer;

		// Token: 0x04001CB3 RID: 7347
		private ArrayList m_ObjectList;

		// Token: 0x04001CB4 RID: 7348
		private int m_TotalObjects;

		// Token: 0x04001CB5 RID: 7349
		private Queue m_QueuedRequests;

		// Token: 0x04001CB6 RID: 7350
		private Thread m_AsyncThread;

		// Token: 0x04001CB7 RID: 7351
		private int m_MaxPoolSize;

		// Token: 0x04001CB8 RID: 7352
		private int m_MinPoolSize;

		// Token: 0x04001CB9 RID: 7353
		private ServicePoint m_ServicePoint;

		// Token: 0x04001CBA RID: 7354
		private CreateConnectionDelegate m_CreateConnectionCallback;

		// Token: 0x02000386 RID: 902
		private enum State
		{
			// Token: 0x04001CBC RID: 7356
			Initializing,
			// Token: 0x04001CBD RID: 7357
			Running,
			// Token: 0x04001CBE RID: 7358
			ShuttingDown
		}

		// Token: 0x02000387 RID: 903
		private class AsyncConnectionPoolRequest
		{
			// Token: 0x06001C26 RID: 7206 RVA: 0x0006A095 File Offset: 0x00069095
			public AsyncConnectionPoolRequest(ConnectionPool pool, object owningObject, GeneralAsyncDelegate asyncCallback, int creationTimeout)
			{
				this.Pool = pool;
				this.OwningObject = owningObject;
				this.AsyncCallback = asyncCallback;
				this.CreationTimeout = creationTimeout;
			}

			// Token: 0x04001CBF RID: 7359
			public object OwningObject;

			// Token: 0x04001CC0 RID: 7360
			public GeneralAsyncDelegate AsyncCallback;

			// Token: 0x04001CC1 RID: 7361
			public bool Completed;

			// Token: 0x04001CC2 RID: 7362
			public ConnectionPool Pool;

			// Token: 0x04001CC3 RID: 7363
			public int CreationTimeout;
		}
	}
}
