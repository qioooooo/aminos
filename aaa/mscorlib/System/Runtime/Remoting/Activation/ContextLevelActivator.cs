using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200068B RID: 1675
	[Serializable]
	internal class ContextLevelActivator : IActivator
	{
		// Token: 0x06003D0D RID: 15629 RVA: 0x000D1DB9 File Offset: 0x000D0DB9
		internal ContextLevelActivator()
		{
			this.m_NextActivator = null;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x000D1DC8 File Offset: 0x000D0DC8
		internal ContextLevelActivator(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_NextActivator = (IActivator)info.GetValue("m_NextActivator", typeof(IActivator));
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003D0F RID: 15631 RVA: 0x000D1DFE File Offset: 0x000D0DFE
		// (set) Token: 0x06003D10 RID: 15632 RVA: 0x000D1E06 File Offset: 0x000D0E06
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

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003D11 RID: 15633 RVA: 0x000D1E0F File Offset: 0x000D0E0F
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Context;
			}
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x000D1E12 File Offset: 0x000D0E12
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = ctorMsg.Activator.NextActivator;
			return ActivationServices.DoCrossContextActivation(ctorMsg);
		}

		// Token: 0x04001F1A RID: 7962
		private IActivator m_NextActivator;
	}
}
