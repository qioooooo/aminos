using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x0200075B RID: 1883
	internal class ServerIdentity : Identity
	{
		// Token: 0x060043A0 RID: 17312 RVA: 0x000E8418 File Offset: 0x000E7418
		internal Type GetLastCalledType(string newTypeName)
		{
			ServerIdentity.LastCalledType lastCalledType = this._lastCalledType;
			if (lastCalledType == null)
			{
				return null;
			}
			string typeName = lastCalledType.typeName;
			Type type = lastCalledType.type;
			if (typeName == null || type == null)
			{
				return null;
			}
			if (typeName.Equals(newTypeName))
			{
				return type;
			}
			return null;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x000E8454 File Offset: 0x000E7454
		internal void SetLastCalledType(string newTypeName, Type newType)
		{
			this._lastCalledType = new ServerIdentity.LastCalledType
			{
				typeName = newTypeName,
				type = newType
			};
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x000E847C File Offset: 0x000E747C
		internal void SetHandle()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				if (!this._srvIdentityHandle.IsAllocated)
				{
					this._srvIdentityHandle = new GCHandle(this, GCHandleType.Normal);
				}
				else
				{
					this._srvIdentityHandle.Target = this;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x000E84DC File Offset: 0x000E74DC
		internal void ResetHandle()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this._srvIdentityHandle.Target = null;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x000E8520 File Offset: 0x000E7520
		internal GCHandle GetHandle()
		{
			return this._srvIdentityHandle;
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x000E8528 File Offset: 0x000E7528
		internal ServerIdentity(MarshalByRefObject obj, Context serverCtx)
			: base(obj is ContextBoundObject)
		{
			if (obj != null)
			{
				if (!RemotingServices.IsTransparentProxy(obj))
				{
					this._srvType = obj.GetType();
				}
				else
				{
					RealProxy realProxy = RemotingServices.GetRealProxy(obj);
					this._srvType = realProxy.GetProxiedType();
				}
			}
			this._srvCtx = serverCtx;
			this._serverObjectChain = null;
			this._stackBuilderSink = null;
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x000E8585 File Offset: 0x000E7585
		internal ServerIdentity(MarshalByRefObject obj, Context serverCtx, string uri)
			: this(obj, serverCtx)
		{
			base.SetOrCreateURI(uri, true);
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060043A7 RID: 17319 RVA: 0x000E8597 File Offset: 0x000E7597
		internal Context ServerContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._srvCtx;
			}
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x000E859F File Offset: 0x000E759F
		internal void SetSingleCallObjectMode()
		{
			this._flags |= 512;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x000E85B3 File Offset: 0x000E75B3
		internal void SetSingletonObjectMode()
		{
			this._flags |= 1024;
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x000E85C7 File Offset: 0x000E75C7
		internal bool IsSingleCall()
		{
			return (this._flags & 512) != 0;
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x000E85DB File Offset: 0x000E75DB
		internal bool IsSingleton()
		{
			return (this._flags & 1024) != 0;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x000E85F0 File Offset: 0x000E75F0
		internal IMessageSink GetServerObjectChain(out MarshalByRefObject obj)
		{
			obj = null;
			if (!this.IsSingleCall())
			{
				if (this._serverObjectChain == null)
				{
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(this, ref flag);
						if (this._serverObjectChain == null)
						{
							MarshalByRefObject tporObject = base.TPOrObject;
							this._serverObjectChain = this._srvCtx.CreateServerObjectChain(tporObject);
						}
					}
					finally
					{
						if (flag)
						{
							Monitor.Exit(this);
						}
					}
				}
				return this._serverObjectChain;
			}
			MarshalByRefObject marshalByRefObject;
			IMessageSink messageSink;
			if (this._tpOrObject != null && this._firstCallDispatched == 0 && Interlocked.CompareExchange(ref this._firstCallDispatched, 1, 0) == 0)
			{
				marshalByRefObject = (MarshalByRefObject)this._tpOrObject;
				messageSink = this._serverObjectChain;
				if (messageSink == null)
				{
					messageSink = this._srvCtx.CreateServerObjectChain(marshalByRefObject);
				}
			}
			else
			{
				marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._srvType, true);
				string objectUri = RemotingServices.GetObjectUri(marshalByRefObject);
				if (objectUri != null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_WellKnown_CtorCantMarshal"), new object[] { base.URI }));
				}
				if (!RemotingServices.IsTransparentProxy(marshalByRefObject))
				{
					marshalByRefObject.__RaceSetServerIdentity(this);
				}
				else
				{
					RealProxy realProxy = RemotingServices.GetRealProxy(marshalByRefObject);
					realProxy.IdentityObject = this;
				}
				messageSink = this._srvCtx.CreateServerObjectChain(marshalByRefObject);
			}
			obj = marshalByRefObject;
			return messageSink;
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060043AD RID: 17325 RVA: 0x000E8730 File Offset: 0x000E7730
		// (set) Token: 0x060043AE RID: 17326 RVA: 0x000E8738 File Offset: 0x000E7738
		internal Type ServerType
		{
			get
			{
				return this._srvType;
			}
			set
			{
				this._srvType = value;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060043AF RID: 17327 RVA: 0x000E8741 File Offset: 0x000E7741
		// (set) Token: 0x060043B0 RID: 17328 RVA: 0x000E8749 File Offset: 0x000E7749
		internal bool MarshaledAsSpecificType
		{
			get
			{
				return this._bMarshaledAsSpecificType;
			}
			set
			{
				this._bMarshaledAsSpecificType = value;
			}
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x000E8754 File Offset: 0x000E7754
		internal IMessageSink RaceSetServerObjectChain(IMessageSink serverObjectChain)
		{
			if (this._serverObjectChain == null)
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					if (this._serverObjectChain == null)
					{
						this._serverObjectChain = serverObjectChain;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
			return this._serverObjectChain;
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x000E87AC File Offset: 0x000E77AC
		internal bool AddServerSideDynamicProperty(IDynamicProperty prop)
		{
			if (this._dphSrv == null)
			{
				DynamicPropertyHolder dynamicPropertyHolder = new DynamicPropertyHolder();
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					if (this._dphSrv == null)
					{
						this._dphSrv = dynamicPropertyHolder;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
			return this._dphSrv.AddDynamicProperty(prop);
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x000E8810 File Offset: 0x000E7810
		internal bool RemoveServerSideDynamicProperty(string name)
		{
			if (this._dphSrv == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PropNotFound"));
			}
			return this._dphSrv.RemoveDynamicProperty(name);
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x000E8836 File Offset: 0x000E7836
		internal ArrayWithSize ServerSideDynamicSinks
		{
			get
			{
				if (this._dphSrv == null)
				{
					return null;
				}
				return this._dphSrv.DynamicSinks;
			}
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x000E884D File Offset: 0x000E784D
		internal override void AssertValid()
		{
			if (base.TPOrObject != null)
			{
				RemotingServices.IsTransparentProxy(base.TPOrObject);
			}
		}

		// Token: 0x040021B6 RID: 8630
		internal Context _srvCtx;

		// Token: 0x040021B7 RID: 8631
		internal IMessageSink _serverObjectChain;

		// Token: 0x040021B8 RID: 8632
		internal StackBuilderSink _stackBuilderSink;

		// Token: 0x040021B9 RID: 8633
		internal DynamicPropertyHolder _dphSrv;

		// Token: 0x040021BA RID: 8634
		internal Type _srvType;

		// Token: 0x040021BB RID: 8635
		private ServerIdentity.LastCalledType _lastCalledType;

		// Token: 0x040021BC RID: 8636
		internal bool _bMarshaledAsSpecificType;

		// Token: 0x040021BD RID: 8637
		internal int _firstCallDispatched;

		// Token: 0x040021BE RID: 8638
		internal GCHandle _srvIdentityHandle;

		// Token: 0x0200075C RID: 1884
		private class LastCalledType
		{
			// Token: 0x040021BF RID: 8639
			public string typeName;

			// Token: 0x040021C0 RID: 8640
			public Type type;
		}
	}
}
