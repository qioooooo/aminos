using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061A RID: 1562
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class NetworkInformationPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003013 RID: 12307 RVA: 0x000CFA65 File Offset: 0x000CEA65
		public NetworkInformationPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000CFA6E File Offset: 0x000CEA6E
		// (set) Token: 0x06003015 RID: 12309 RVA: 0x000CFA76 File Offset: 0x000CEA76
		public string Access
		{
			get
			{
				return this.access;
			}
			set
			{
				this.access = value;
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000CFA80 File Offset: 0x000CEA80
		public override IPermission CreatePermission()
		{
			NetworkInformationPermission networkInformationPermission;
			if (base.Unrestricted)
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.Unrestricted);
			}
			else
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Read", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Read);
					}
					else if (string.Compare(this.access, "Ping", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Ping);
					}
					else
					{
						if (string.Compare(this.access, "None", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.access }));
						}
						networkInformationPermission.AddPermission(NetworkInformationAccess.None);
					}
				}
			}
			return networkInformationPermission;
		}

		// Token: 0x04002DD7 RID: 11735
		private const string strAccess = "Access";

		// Token: 0x04002DD8 RID: 11736
		private string access;
	}
}
