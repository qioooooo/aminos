using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200070C RID: 1804
	[Serializable]
	internal class TransitionCall : IMessage, IInternalMessage, IMessageSink, ISerializable
	{
		// Token: 0x0600411E RID: 16670 RVA: 0x000DEB0C File Offset: 0x000DDB0C
		internal TransitionCall(IntPtr targetCtxID, CrossContextDelegate deleg)
		{
			this._sourceCtxID = Thread.CurrentContext.InternalContextID;
			this._targetCtxID = targetCtxID;
			this._delegate = deleg;
			this._targetDomainID = 0;
			this._eeData = IntPtr.Zero;
			this._srvID = new ServerIdentity(null, Thread.GetContextInternal(this._targetCtxID));
			this._ID = this._srvID;
			this._ID.RaceSetChannelSink(CrossContextChannel.MessageSink);
			this._srvID.RaceSetServerObjectChain(this);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000DEB90 File Offset: 0x000DDB90
		internal TransitionCall(IntPtr targetCtxID, IntPtr eeData, int targetDomainID)
		{
			this._sourceCtxID = Thread.CurrentContext.InternalContextID;
			this._targetCtxID = targetCtxID;
			this._delegate = null;
			this._targetDomainID = targetDomainID;
			this._eeData = eeData;
			this._srvID = null;
			this._ID = new Identity("TransitionCallURI", null);
			CrossAppDomainData crossAppDomainData = new CrossAppDomainData(this._targetCtxID, this._targetDomainID, Identity.ProcessGuid);
			string text;
			IMessageSink messageSink = CrossAppDomainChannel.AppDomainChannel.CreateMessageSink(null, crossAppDomainData, out text);
			this._ID.RaceSetChannelSink(messageSink);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000DEC1C File Offset: 0x000DDC1C
		internal TransitionCall(SerializationInfo info, StreamingContext context)
		{
			if (info == null || context.State != StreamingContextStates.CrossAppDomain)
			{
				throw new ArgumentNullException("info");
			}
			this._props = (IDictionary)info.GetValue("props", typeof(IDictionary));
			this._delegate = (CrossContextDelegate)info.GetValue("delegate", typeof(CrossContextDelegate));
			this._sourceCtxID = (IntPtr)info.GetValue("sourceCtxID", typeof(IntPtr));
			this._targetCtxID = (IntPtr)info.GetValue("targetCtxID", typeof(IntPtr));
			this._eeData = (IntPtr)info.GetValue("eeData", typeof(IntPtr));
			this._targetDomainID = info.GetInt32("targetDomainID");
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x000DECFC File Offset: 0x000DDCFC
		public IDictionary Properties
		{
			get
			{
				if (this._props == null)
				{
					lock (this)
					{
						if (this._props == null)
						{
							this._props = new Hashtable();
						}
					}
				}
				return this._props;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004122 RID: 16674 RVA: 0x000DED4C File Offset: 0x000DDD4C
		// (set) Token: 0x06004123 RID: 16675 RVA: 0x000DEDCC File Offset: 0x000DDDCC
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				if (this._targetDomainID != 0 && this._srvID == null)
				{
					lock (this)
					{
						if (Thread.GetContextInternal(this._targetCtxID) == null)
						{
							Context defaultContext = Context.DefaultContext;
						}
						this._srvID = new ServerIdentity(null, Thread.GetContextInternal(this._targetCtxID));
						this._srvID.RaceSetServerObjectChain(this);
					}
				}
				return this._srvID;
			}
			set
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004124 RID: 16676 RVA: 0x000DEDDD File Offset: 0x000DDDDD
		// (set) Token: 0x06004125 RID: 16677 RVA: 0x000DEDE5 File Offset: 0x000DDDE5
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return this._ID;
			}
			set
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x000DEDF6 File Offset: 0x000DDDF6
		void IInternalMessage.SetURI(string uri)
		{
			throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x000DEE07 File Offset: 0x000DDE07
		void IInternalMessage.SetCallContext(LogicalCallContext callContext)
		{
			throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x000DEE18 File Offset: 0x000DDE18
		bool IInternalMessage.HasProperties()
		{
			throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x000DEE2C File Offset: 0x000DDE2C
		public IMessage SyncProcessMessage(IMessage msg)
		{
			try
			{
				LogicalCallContext logicalCallContext = Message.PropagateCallContextFromMessageToThread(msg);
				if (this._delegate != null)
				{
					this._delegate();
				}
				else
				{
					CallBackHelper callBackHelper = new CallBackHelper(this._eeData, true, this._targetDomainID);
					CrossContextDelegate crossContextDelegate = new CrossContextDelegate(callBackHelper.Func);
					crossContextDelegate();
				}
				Message.PropagateCallContextFromThreadToMessage(msg, logicalCallContext);
			}
			catch (Exception ex)
			{
				ReturnMessage returnMessage = new ReturnMessage(ex, new ErrorMessage());
				returnMessage.SetLogicalCallContext((LogicalCallContext)msg.Properties[Message.CallContextKey]);
				return returnMessage;
			}
			return this;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x000DEECC File Offset: 0x000DDECC
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMessage message = this.SyncProcessMessage(msg);
			replySink.SyncProcessMessage(message);
			return null;
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x000DEEEA File Offset: 0x000DDEEA
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x000DEEF0 File Offset: 0x000DDEF0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null || context.State != StreamingContextStates.CrossAppDomain)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("props", this._props, typeof(IDictionary));
			info.AddValue("delegate", this._delegate, typeof(CrossContextDelegate));
			info.AddValue("sourceCtxID", this._sourceCtxID);
			info.AddValue("targetCtxID", this._targetCtxID);
			info.AddValue("targetDomainID", this._targetDomainID);
			info.AddValue("eeData", this._eeData);
		}

		// Token: 0x04002094 RID: 8340
		private IDictionary _props;

		// Token: 0x04002095 RID: 8341
		private IntPtr _sourceCtxID;

		// Token: 0x04002096 RID: 8342
		private IntPtr _targetCtxID;

		// Token: 0x04002097 RID: 8343
		private int _targetDomainID;

		// Token: 0x04002098 RID: 8344
		private ServerIdentity _srvID;

		// Token: 0x04002099 RID: 8345
		private Identity _ID;

		// Token: 0x0400209A RID: 8346
		private CrossContextDelegate _delegate;

		// Token: 0x0400209B RID: 8347
		private IntPtr _eeData;
	}
}
