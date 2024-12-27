using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200068A RID: 1674
	internal class AppDomainLevelActivator : IActivator
	{
		// Token: 0x06003D07 RID: 15623 RVA: 0x000D1D46 File Offset: 0x000D0D46
		internal AppDomainLevelActivator(string remActivatorURL)
		{
			this.m_RemActivatorURL = remActivatorURL;
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x000D1D55 File Offset: 0x000D0D55
		internal AppDomainLevelActivator(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_NextActivator = (IActivator)info.GetValue("m_NextActivator", typeof(IActivator));
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x000D1D8B File Offset: 0x000D0D8B
		// (set) Token: 0x06003D0A RID: 15626 RVA: 0x000D1D93 File Offset: 0x000D0D93
		public virtual IActivator NextActivator
		{
			get
			{
				return this.m_NextActivator;
			}
			set
			{
				this.m_NextActivator = value;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x000D1D9C File Offset: 0x000D0D9C
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.AppDomain;
			}
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x000D1DA0 File Offset: 0x000D0DA0
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = this.m_NextActivator;
			return ActivationServices.GetActivator().Activate(ctorMsg);
		}

		// Token: 0x04001F18 RID: 7960
		private IActivator m_NextActivator;

		// Token: 0x04001F19 RID: 7961
		private string m_RemActivatorURL;
	}
}
