using System;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x0200000A RID: 10
	internal interface IAdminApiDsp
	{
		// Token: 0x06000027 RID: 39
		AdminStatus sntl_admin_context_new_scope(string scope);

		// Token: 0x06000028 RID: 40
		AdminStatus sntl_admin_context_new(string hostname, ushort port, string password);

		// Token: 0x06000029 RID: 41
		AdminStatus sntl_admin_context_delete();

		// Token: 0x0600002A RID: 42
		AdminStatus sntl_admin_get(string scope, string format, ref string info);

		// Token: 0x0600002B RID: 43
		AdminStatus sntl_admin_set(string action, ref string return_status);

		// Token: 0x0600002C RID: 44
		void setLibPath(string path);
	}
}
