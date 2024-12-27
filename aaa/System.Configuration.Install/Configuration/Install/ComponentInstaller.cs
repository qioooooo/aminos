using System;
using System.ComponentModel;

namespace System.Configuration.Install
{
	// Token: 0x0200000D RID: 13
	public abstract class ComponentInstaller : Installer
	{
		// Token: 0x0600004B RID: 75
		public abstract void CopyFromComponent(IComponent component);

		// Token: 0x0600004C RID: 76 RVA: 0x00003930 File Offset: 0x00002930
		public virtual bool IsEquivalentInstaller(ComponentInstaller otherInstaller)
		{
			return false;
		}
	}
}
