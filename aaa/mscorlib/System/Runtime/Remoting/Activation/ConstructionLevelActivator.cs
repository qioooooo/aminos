using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200068C RID: 1676
	[Serializable]
	internal class ConstructionLevelActivator : IActivator
	{
		// Token: 0x06003D13 RID: 15635 RVA: 0x000D1E2B File Offset: 0x000D0E2B
		internal ConstructionLevelActivator()
		{
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x000D1E33 File Offset: 0x000D0E33
		// (set) Token: 0x06003D15 RID: 15637 RVA: 0x000D1E36 File Offset: 0x000D0E36
		public virtual IActivator NextActivator
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003D16 RID: 15638 RVA: 0x000D1E3D File Offset: 0x000D0E3D
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Construction;
			}
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x000D1E40 File Offset: 0x000D0E40
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = ctorMsg.Activator.NextActivator;
			return ActivationServices.DoServerContextActivation(ctorMsg);
		}
	}
}
