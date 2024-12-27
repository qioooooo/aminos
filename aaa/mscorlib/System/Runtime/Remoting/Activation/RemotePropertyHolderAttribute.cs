using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200068D RID: 1677
	internal class RemotePropertyHolderAttribute : IContextAttribute
	{
		// Token: 0x06003D18 RID: 15640 RVA: 0x000D1E59 File Offset: 0x000D0E59
		internal RemotePropertyHolderAttribute(IList cp)
		{
			this._cp = cp;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x000D1E68 File Offset: 0x000D0E68
		[ComVisible(true)]
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return false;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x000D1E6C File Offset: 0x000D0E6C
		[ComVisible(true)]
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			for (int i = 0; i < this._cp.Count; i++)
			{
				ctorMsg.ContextProperties.Add(this._cp[i]);
			}
		}

		// Token: 0x04001F1B RID: 7963
		private IList _cp;
	}
}
