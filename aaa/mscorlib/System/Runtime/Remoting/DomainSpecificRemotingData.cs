using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x020006AA RID: 1706
	internal class DomainSpecificRemotingData
	{
		// Token: 0x06003DCC RID: 15820 RVA: 0x000D4154 File Offset: 0x000D3154
		internal DomainSpecificRemotingData()
		{
			this._flags = 0;
			this._ConfigLock = new object();
			this._ChannelServicesData = new ChannelServicesData();
			this._IDTableLock = new ReaderWriterLock();
			this._appDomainProperties = new IContextProperty[1];
			this._appDomainProperties[0] = new LeaseLifeTimeServiceProperty();
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x000D41A8 File Offset: 0x000D31A8
		// (set) Token: 0x06003DCE RID: 15822 RVA: 0x000D41B0 File Offset: 0x000D31B0
		internal LeaseManager LeaseManager
		{
			get
			{
				return this._LeaseManager;
			}
			set
			{
				this._LeaseManager = value;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x000D41B9 File Offset: 0x000D31B9
		internal object ConfigLock
		{
			get
			{
				return this._ConfigLock;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x000D41C1 File Offset: 0x000D31C1
		internal ReaderWriterLock IDTableLock
		{
			get
			{
				return this._IDTableLock;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x000D41C9 File Offset: 0x000D31C9
		// (set) Token: 0x06003DD2 RID: 15826 RVA: 0x000D41D1 File Offset: 0x000D31D1
		internal LocalActivator LocalActivator
		{
			get
			{
				return this._LocalActivator;
			}
			set
			{
				this._LocalActivator = value;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x000D41DA File Offset: 0x000D31DA
		// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x000D41E2 File Offset: 0x000D31E2
		internal ActivationListener ActivationListener
		{
			get
			{
				return this._ActivationListener;
			}
			set
			{
				this._ActivationListener = value;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x000D41EB File Offset: 0x000D31EB
		// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x000D41F8 File Offset: 0x000D31F8
		internal bool InitializingActivation
		{
			get
			{
				return (this._flags & 1) == 1;
			}
			set
			{
				if (value)
				{
					this._flags |= 1;
					return;
				}
				this._flags &= -2;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x000D421B File Offset: 0x000D321B
		// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x000D4228 File Offset: 0x000D3228
		internal bool ActivationInitialized
		{
			get
			{
				return (this._flags & 2) == 2;
			}
			set
			{
				if (value)
				{
					this._flags |= 2;
					return;
				}
				this._flags &= -3;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x000D424B File Offset: 0x000D324B
		// (set) Token: 0x06003DDA RID: 15834 RVA: 0x000D4258 File Offset: 0x000D3258
		internal bool ActivatorListening
		{
			get
			{
				return (this._flags & 4) == 4;
			}
			set
			{
				if (value)
				{
					this._flags |= 4;
					return;
				}
				this._flags &= -5;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x000D427B File Offset: 0x000D327B
		internal IContextProperty[] AppDomainContextProperties
		{
			get
			{
				return this._appDomainProperties;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x000D4283 File Offset: 0x000D3283
		internal ChannelServicesData ChannelServicesData
		{
			get
			{
				return this._ChannelServicesData;
			}
		}

		// Token: 0x04001F5C RID: 8028
		private const int ACTIVATION_INITIALIZING = 1;

		// Token: 0x04001F5D RID: 8029
		private const int ACTIVATION_INITIALIZED = 2;

		// Token: 0x04001F5E RID: 8030
		private const int ACTIVATOR_LISTENING = 4;

		// Token: 0x04001F5F RID: 8031
		private LocalActivator _LocalActivator;

		// Token: 0x04001F60 RID: 8032
		private ActivationListener _ActivationListener;

		// Token: 0x04001F61 RID: 8033
		private IContextProperty[] _appDomainProperties;

		// Token: 0x04001F62 RID: 8034
		private int _flags;

		// Token: 0x04001F63 RID: 8035
		private object _ConfigLock;

		// Token: 0x04001F64 RID: 8036
		private ChannelServicesData _ChannelServicesData;

		// Token: 0x04001F65 RID: 8037
		private LeaseManager _LeaseManager;

		// Token: 0x04001F66 RID: 8038
		private ReaderWriterLock _IDTableLock;
	}
}
