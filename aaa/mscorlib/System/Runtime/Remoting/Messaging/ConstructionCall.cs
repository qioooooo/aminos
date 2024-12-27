using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000707 RID: 1799
	[ComVisible(true)]
	[CLSCompliant(false)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class ConstructionCall : MethodCall, IConstructionCallMessage, IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x000DDC57 File Offset: 0x000DCC57
		public ConstructionCall(Header[] headers)
			: base(headers)
		{
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x000DDC60 File Offset: 0x000DCC60
		public ConstructionCall(IMessage m)
			: base(m)
		{
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x000DDC69 File Offset: 0x000DCC69
		internal ConstructionCall(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x000DDC74 File Offset: 0x000DCC74
		internal override bool FillSpecialHeader(string key, object value)
		{
			if (key != null)
			{
				if (key.Equals("__ActivationType"))
				{
					this._activationType = null;
				}
				else if (key.Equals("__ContextProperties"))
				{
					this._contextProperties = (IList)value;
				}
				else if (key.Equals("__CallSiteActivationAttributes"))
				{
					this._callSiteActivationAttributes = (object[])value;
				}
				else if (key.Equals("__Activator"))
				{
					this._activator = (IActivator)value;
				}
				else
				{
					if (!key.Equals("__ActivationTypeName"))
					{
						return base.FillSpecialHeader(key, value);
					}
					this._activationTypeName = (string)value;
				}
			}
			return true;
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x000DDD13 File Offset: 0x000DCD13
		public object[] CallSiteActivationAttributes
		{
			get
			{
				return this._callSiteActivationAttributes;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x000DDD1B File Offset: 0x000DCD1B
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

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x000DDD45 File Offset: 0x000DCD45
		public string ActivationTypeName
		{
			get
			{
				return this._activationTypeName;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x000DDD4D File Offset: 0x000DCD4D
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

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x060040E6 RID: 16614 RVA: 0x000DDD68 File Offset: 0x000DCD68
		public override IDictionary Properties
		{
			get
			{
				IDictionary externalProperties;
				lock (this)
				{
					if (this.InternalProperties == null)
					{
						this.InternalProperties = new Hashtable();
					}
					if (this.ExternalProperties == null)
					{
						this.ExternalProperties = new CCMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x000DDDCC File Offset: 0x000DCDCC
		// (set) Token: 0x060040E8 RID: 16616 RVA: 0x000DDDD4 File Offset: 0x000DCDD4
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

		// Token: 0x0400207D RID: 8317
		internal Type _activationType;

		// Token: 0x0400207E RID: 8318
		internal string _activationTypeName;

		// Token: 0x0400207F RID: 8319
		internal IList _contextProperties;

		// Token: 0x04002080 RID: 8320
		internal object[] _callSiteActivationAttributes;

		// Token: 0x04002081 RID: 8321
		internal IActivator _activator;
	}
}
