using System;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	internal enum ComponentFlags
	{
		// Token: 0x040000C9 RID: 201
		TypeInfoFound = 1,
		// Token: 0x040000CA RID: 202
		COMPlusPropertiesFound,
		// Token: 0x040000CB RID: 203
		ProxyFound = 4,
		// Token: 0x040000CC RID: 204
		InterfacesFound = 8,
		// Token: 0x040000CD RID: 205
		AlreadyInstalled = 16,
		// Token: 0x040000CE RID: 206
		NotInApplication = 32
	}
}
