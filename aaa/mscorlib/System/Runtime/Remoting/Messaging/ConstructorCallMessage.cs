using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FD RID: 1789
	internal class ConstructorCallMessage : IConstructionCallMessage, IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x0600403F RID: 16447 RVA: 0x000DB9B6 File Offset: 0x000DA9B6
		private ConstructorCallMessage()
		{
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x000DB9BE File Offset: 0x000DA9BE
		internal ConstructorCallMessage(object[] callSiteActivationAttributes, object[] womAttr, object[] typeAttr, Type serverType)
		{
			this._activationType = serverType;
			this._activationTypeName = RemotingServices.GetDefaultQualifiedTypeName(this._activationType);
			this._callSiteActivationAttributes = callSiteActivationAttributes;
			this._womGlobalAttributes = womAttr;
			this._typeAttributes = typeAttr;
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x000DB9F4 File Offset: 0x000DA9F4
		public object GetThisPtr()
		{
			if (this._message != null)
			{
				return this._message.GetThisPtr();
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x000DBA19 File Offset: 0x000DAA19
		public object[] CallSiteActivationAttributes
		{
			get
			{
				return this._callSiteActivationAttributes;
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000DBA21 File Offset: 0x000DAA21
		internal object[] GetWOMAttributes()
		{
			return this._womGlobalAttributes;
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000DBA29 File Offset: 0x000DAA29
		internal object[] GetTypeAttributes()
		{
			return this._typeAttributes;
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x000DBA31 File Offset: 0x000DAA31
		public Type ActivationType
		{
			get
			{
				if (this._activationType == null && this._activationTypeName != null)
				{
					this._activationType = RemotingServices.InternalGetTypeFromQualifiedTypeName(this._activationTypeName, false);
				}
				return this._activationType;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004046 RID: 16454 RVA: 0x000DBA5B File Offset: 0x000DAA5B
		public string ActivationTypeName
		{
			get
			{
				return this._activationTypeName;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x000DBA63 File Offset: 0x000DAA63
		public IList ContextProperties
		{
			get
			{
				if (this._contextProperties == null)
				{
					this._contextProperties = new ArrayList();
				}
				return this._contextProperties;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x000DBA7E File Offset: 0x000DAA7E
		// (set) Token: 0x06004049 RID: 16457 RVA: 0x000DBAA3 File Offset: 0x000DAAA3
		public string Uri
		{
			get
			{
				if (this._message != null)
				{
					return this._message.Uri;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
			set
			{
				if (this._message != null)
				{
					this._message.Uri = value;
					return;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x000DBAC9 File Offset: 0x000DAAC9
		public string MethodName
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x000DBAEE File Offset: 0x000DAAEE
		public string TypeName
		{
			get
			{
				if (this._message != null)
				{
					return this._message.TypeName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x000DBB13 File Offset: 0x000DAB13
		public object MethodSignature
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodSignature;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x000DBB38 File Offset: 0x000DAB38
		public MethodBase MethodBase
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodBase;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x000DBB5D File Offset: 0x000DAB5D
		public int InArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x000DBB7F File Offset: 0x000DAB7F
		public object GetInArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x000DBBA2 File Offset: 0x000DABA2
		public string GetInArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x000DBBC5 File Offset: 0x000DABC5
		public object[] InArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x000DBBE7 File Offset: 0x000DABE7
		public int ArgCount
		{
			get
			{
				if (this._message != null)
				{
					return this._message.ArgCount;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000DBC0C File Offset: 0x000DAC0C
		public object GetArg(int argNum)
		{
			if (this._message != null)
			{
				return this._message.GetArg(argNum);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x000DBC32 File Offset: 0x000DAC32
		public string GetArgName(int index)
		{
			if (this._message != null)
			{
				return this._message.GetArgName(index);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x000DBC58 File Offset: 0x000DAC58
		public bool HasVarArgs
		{
			get
			{
				if (this._message != null)
				{
					return this._message.HasVarArgs;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x000DBC7D File Offset: 0x000DAC7D
		public object[] Args
		{
			get
			{
				if (this._message != null)
				{
					return this._message.Args;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x000DBCA4 File Offset: 0x000DACA4
		public IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					object obj = new CCMDictionary(this, new Hashtable());
					Interlocked.CompareExchange(ref this._properties, obj, null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06004058 RID: 16472 RVA: 0x000DBCDE File Offset: 0x000DACDE
		// (set) Token: 0x06004059 RID: 16473 RVA: 0x000DBCE6 File Offset: 0x000DACE6
		public IActivator Activator
		{
			get
			{
				return this._activator;
			}
			set
			{
				this._activator = value;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x000DBCEF File Offset: 0x000DACEF
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x000DBCF7 File Offset: 0x000DACF7
		// (set) Token: 0x0600405C RID: 16476 RVA: 0x000DBD07 File Offset: 0x000DAD07
		internal bool ActivateInContext
		{
			get
			{
				return (this._iFlags & 1) != 0;
			}
			set
			{
				this._iFlags = (value ? (this._iFlags | 1) : (this._iFlags & -2));
			}
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x000DBD25 File Offset: 0x000DAD25
		internal void SetFrame(MessageData msgData)
		{
			this._message = new Message();
			this._message.InitFields(msgData);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x000DBD3E File Offset: 0x000DAD3E
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._message != null)
			{
				return this._message.GetLogicalCallContext();
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x000DBD63 File Offset: 0x000DAD63
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			if (this._message != null)
			{
				return this._message.SetLogicalCallContext(ctx);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x000DBD89 File Offset: 0x000DAD89
		internal Message GetMessage()
		{
			return this._message;
		}

		// Token: 0x0400204B RID: 8267
		private const int CCM_ACTIVATEINCONTEXT = 1;

		// Token: 0x0400204C RID: 8268
		private object[] _callSiteActivationAttributes;

		// Token: 0x0400204D RID: 8269
		private object[] _womGlobalAttributes;

		// Token: 0x0400204E RID: 8270
		private object[] _typeAttributes;

		// Token: 0x0400204F RID: 8271
		[NonSerialized]
		private Type _activationType;

		// Token: 0x04002050 RID: 8272
		private string _activationTypeName;

		// Token: 0x04002051 RID: 8273
		private IList _contextProperties;

		// Token: 0x04002052 RID: 8274
		private int _iFlags;

		// Token: 0x04002053 RID: 8275
		private Message _message;

		// Token: 0x04002054 RID: 8276
		private object _properties;

		// Token: 0x04002055 RID: 8277
		private ArgMapper _argMapper;

		// Token: 0x04002056 RID: 8278
		private IActivator _activator;
	}
}
