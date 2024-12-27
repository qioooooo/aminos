using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x020004A8 RID: 1192
	[ComVisible(true)]
	[Serializable]
	public sealed class GacInstalled : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06003035 RID: 12341 RVA: 0x000A638E File Offset: 0x000A538E
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000A6395 File Offset: 0x000A5395
		public override bool Equals(object o)
		{
			return o is GacInstalled;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000A63A2 File Offset: 0x000A53A2
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000A63A5 File Offset: 0x000A53A5
		public object Copy()
		{
			return new GacInstalled();
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x000A63AC File Offset: 0x000A53AC
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000A63DB File Offset: 0x000A53DB
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position] = '\t';
			return position + 1;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000A63E5 File Offset: 0x000A53E5
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return 1;
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000A63E8 File Offset: 0x000A53E8
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return position;
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000A63EB File Offset: 0x000A53EB
		public override string ToString()
		{
			return this.ToXml().ToString();
		}
	}
}
