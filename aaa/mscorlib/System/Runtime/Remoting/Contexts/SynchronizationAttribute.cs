using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000784 RID: 1924
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class SynchronizationAttribute : ContextAttribute, IContributeServerContextSink, IContributeClientContextSink
	{
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06004500 RID: 17664 RVA: 0x000EBAA6 File Offset: 0x000EAAA6
		// (set) Token: 0x06004501 RID: 17665 RVA: 0x000EBAAE File Offset: 0x000EAAAE
		public virtual bool Locked
		{
			get
			{
				return this._locked;
			}
			set
			{
				this._locked = value;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004502 RID: 17666 RVA: 0x000EBAB7 File Offset: 0x000EAAB7
		public virtual bool IsReEntrant
		{
			get
			{
				return this._bReEntrant;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x000EBABF File Offset: 0x000EAABF
		// (set) Token: 0x06004504 RID: 17668 RVA: 0x000EBAC7 File Offset: 0x000EAAC7
		internal string SyncCallOutLCID
		{
			get
			{
				return this._syncLcid;
			}
			set
			{
				this._syncLcid = value;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004505 RID: 17669 RVA: 0x000EBAD0 File Offset: 0x000EAAD0
		internal ArrayList AsyncCallOutLCIDList
		{
			get
			{
				return this._asyncLcidList;
			}
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x000EBAD8 File Offset: 0x000EAAD8
		internal bool IsKnownLCID(IMessage reqMsg)
		{
			string logicalCallID = ((LogicalCallContext)reqMsg.Properties[Message.CallContextKey]).RemotingData.LogicalCallID;
			return logicalCallID.Equals(this._syncLcid) || this._asyncLcidList.Contains(logicalCallID);
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x000EBB21 File Offset: 0x000EAB21
		public SynchronizationAttribute()
			: this(4, false)
		{
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x000EBB2B File Offset: 0x000EAB2B
		public SynchronizationAttribute(bool reEntrant)
			: this(4, reEntrant)
		{
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x000EBB35 File Offset: 0x000EAB35
		public SynchronizationAttribute(int flag)
			: this(flag, false)
		{
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x000EBB40 File Offset: 0x000EAB40
		public SynchronizationAttribute(int flag, bool reEntrant)
			: base("Synchronization")
		{
			this._bReEntrant = reEntrant;
			switch (flag)
			{
			case 1:
			case 2:
			case 4:
				break;
			case 3:
				goto IL_0038;
			default:
				if (flag != 8)
				{
					goto IL_0038;
				}
				break;
			}
			this._flavor = flag;
			return;
			IL_0038:
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "flag");
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x000EBB99 File Offset: 0x000EAB99
		internal void Dispose()
		{
			if (this._waitHandle != null)
			{
				this._waitHandle.Unregister(null);
			}
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x000EBBB0 File Offset: 0x000EABB0
		[ComVisible(true)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			bool flag = true;
			if (this._flavor == 8)
			{
				flag = false;
			}
			else
			{
				SynchronizationAttribute synchronizationAttribute = (SynchronizationAttribute)ctx.GetProperty("Synchronization");
				if ((this._flavor == 1 && synchronizationAttribute != null) || (this._flavor == 4 && synchronizationAttribute == null))
				{
					flag = false;
				}
				if (this._flavor == 4)
				{
					this._cliCtxAttr = synchronizationAttribute;
				}
			}
			return flag;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x000EBC24 File Offset: 0x000EAC24
		[ComVisible(true)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (this._flavor == 1 || this._flavor == 2 || ctorMsg == null)
			{
				return;
			}
			if (this._cliCtxAttr != null)
			{
				ctorMsg.ContextProperties.Add(this._cliCtxAttr);
				this._cliCtxAttr = null;
				return;
			}
			ctorMsg.ContextProperties.Add(this);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x000EBC78 File Offset: 0x000EAC78
		internal virtual void InitIfNecessary()
		{
			lock (this)
			{
				if (this._asyncWorkEvent == null)
				{
					this._asyncWorkEvent = new AutoResetEvent(false);
					this._workItemQueue = new Queue();
					this._asyncLcidList = new ArrayList();
					WaitOrTimerCallback waitOrTimerCallback = new WaitOrTimerCallback(this.DispatcherCallBack);
					this._waitHandle = ThreadPool.RegisterWaitForSingleObject(this._asyncWorkEvent, waitOrTimerCallback, null, SynchronizationAttribute._timeOut, false);
				}
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x000EBCF8 File Offset: 0x000EACF8
		private void DispatcherCallBack(object stateIgnored, bool ignored)
		{
			WorkItem workItem;
			lock (this._workItemQueue)
			{
				workItem = (WorkItem)this._workItemQueue.Dequeue();
			}
			this.ExecuteWorkItem(workItem);
			this.HandleWorkCompletion();
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x000EBD4C File Offset: 0x000EAD4C
		internal virtual void HandleThreadExit()
		{
			this.HandleWorkCompletion();
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x000EBD54 File Offset: 0x000EAD54
		internal virtual void HandleThreadReEntry()
		{
			WorkItem workItem = new WorkItem(null, null, null);
			workItem.SetDummy();
			this.HandleWorkRequest(workItem);
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x000EBD78 File Offset: 0x000EAD78
		internal virtual void HandleWorkCompletion()
		{
			WorkItem workItem = null;
			bool flag = false;
			lock (this._workItemQueue)
			{
				if (this._workItemQueue.Count >= 1)
				{
					workItem = (WorkItem)this._workItemQueue.Peek();
					flag = true;
					workItem.SetSignaled();
				}
				else
				{
					this._locked = false;
				}
			}
			if (flag)
			{
				if (workItem.IsAsync())
				{
					this._asyncWorkEvent.Set();
					return;
				}
				lock (workItem)
				{
					Monitor.Pulse(workItem);
				}
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x000EBE1C File Offset: 0x000EAE1C
		internal virtual void HandleWorkRequest(WorkItem work)
		{
			if (!this.IsNestedCall(work._reqMsg))
			{
				if (work.IsAsync())
				{
					bool flag = true;
					lock (this._workItemQueue)
					{
						work.SetWaiting();
						this._workItemQueue.Enqueue(work);
						if (!this._locked && this._workItemQueue.Count == 1)
						{
							work.SetSignaled();
							this._locked = true;
							this._asyncWorkEvent.Set();
						}
						return;
					}
				}
				lock (work)
				{
					bool flag;
					lock (this._workItemQueue)
					{
						if (!this._locked && this._workItemQueue.Count == 0)
						{
							this._locked = true;
							flag = false;
						}
						else
						{
							flag = true;
							work.SetWaiting();
							this._workItemQueue.Enqueue(work);
						}
					}
					if (flag)
					{
						Monitor.Wait(work);
						if (!work.IsDummy())
						{
							this.DispatcherCallBack(null, true);
							goto IL_0122;
						}
						lock (this._workItemQueue)
						{
							this._workItemQueue.Dequeue();
							goto IL_0122;
						}
					}
					if (!work.IsDummy())
					{
						work.SetSignaled();
						this.ExecuteWorkItem(work);
						this.HandleWorkCompletion();
					}
					IL_0122:
					return;
				}
			}
			work.SetSignaled();
			work.Execute();
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x000EBF94 File Offset: 0x000EAF94
		internal void ExecuteWorkItem(WorkItem work)
		{
			work.Execute();
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x000EBF9C File Offset: 0x000EAF9C
		internal bool IsNestedCall(IMessage reqMsg)
		{
			bool flag = false;
			if (!this.IsReEntrant)
			{
				string syncCallOutLCID = this.SyncCallOutLCID;
				if (syncCallOutLCID != null)
				{
					LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
					if (logicalCallContext != null && syncCallOutLCID.Equals(logicalCallContext.RemotingData.LogicalCallID))
					{
						flag = true;
					}
				}
				if (!flag && this.AsyncCallOutLCIDList.Count > 0)
				{
					LogicalCallContext logicalCallContext2 = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
					if (this.AsyncCallOutLCIDList.Contains(logicalCallContext2.RemotingData.LogicalCallID))
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x000EC030 File Offset: 0x000EB030
		public virtual IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			this.InitIfNecessary();
			return new SynchronizedServerContextSink(this, nextSink);
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x000EC04C File Offset: 0x000EB04C
		public virtual IMessageSink GetClientContextSink(IMessageSink nextSink)
		{
			this.InitIfNecessary();
			return new SynchronizedClientContextSink(this, nextSink);
		}

		// Token: 0x0400221F RID: 8735
		public const int NOT_SUPPORTED = 1;

		// Token: 0x04002220 RID: 8736
		public const int SUPPORTED = 2;

		// Token: 0x04002221 RID: 8737
		public const int REQUIRED = 4;

		// Token: 0x04002222 RID: 8738
		public const int REQUIRES_NEW = 8;

		// Token: 0x04002223 RID: 8739
		private const string PROPERTY_NAME = "Synchronization";

		// Token: 0x04002224 RID: 8740
		private static readonly int _timeOut = -1;

		// Token: 0x04002225 RID: 8741
		[NonSerialized]
		internal AutoResetEvent _asyncWorkEvent;

		// Token: 0x04002226 RID: 8742
		[NonSerialized]
		private RegisteredWaitHandle _waitHandle;

		// Token: 0x04002227 RID: 8743
		[NonSerialized]
		internal Queue _workItemQueue;

		// Token: 0x04002228 RID: 8744
		[NonSerialized]
		internal bool _locked;

		// Token: 0x04002229 RID: 8745
		internal bool _bReEntrant;

		// Token: 0x0400222A RID: 8746
		internal int _flavor;

		// Token: 0x0400222B RID: 8747
		[NonSerialized]
		private SynchronizationAttribute _cliCtxAttr;

		// Token: 0x0400222C RID: 8748
		[NonSerialized]
		private string _syncLcid;

		// Token: 0x0400222D RID: 8749
		[NonSerialized]
		private ArrayList _asyncLcidList;
	}
}
