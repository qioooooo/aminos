using System;

namespace System.Web.Util
{
	// Token: 0x0200014A RID: 330
	internal interface ITypedWebObjectFactory : IWebObjectFactory
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000F58 RID: 3928
		Type InstantiatedType { get; }
	}
}
