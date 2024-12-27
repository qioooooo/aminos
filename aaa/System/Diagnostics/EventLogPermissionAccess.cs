using System;

namespace System.Diagnostics
{
	// Token: 0x02000756 RID: 1878
	[Flags]
	public enum EventLogPermissionAccess
	{
		// Token: 0x040032B9 RID: 12985
		None = 0,
		// Token: 0x040032BA RID: 12986
		Write = 16,
		// Token: 0x040032BB RID: 12987
		Administer = 48,
		// Token: 0x040032BC RID: 12988
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 2,
		// Token: 0x040032BD RID: 12989
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 6,
		// Token: 0x040032BE RID: 12990
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Audit = 10
	}
}
