using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017A RID: 378
	public interface IComponentDiscoveryService
	{
		// Token: 0x06000C2D RID: 3117
		ICollection GetComponentTypes(IDesignerHost designerHost, Type baseType);
	}
}
