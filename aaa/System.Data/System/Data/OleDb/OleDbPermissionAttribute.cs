using System;
using System.ComponentModel;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OleDb
{
	// Token: 0x02000236 RID: 566
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class OleDbPermissionAttribute : DBDataPermissionAttribute
	{
		// Token: 0x0600204D RID: 8269 RVA: 0x00261AB0 File Offset: 0x00260EB0
		public OleDbPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600204E RID: 8270 RVA: 0x00261AC4 File Offset: 0x00260EC4
		// (set) Token: 0x0600204F RID: 8271 RVA: 0x00261AE4 File Offset: 0x00260EE4
		[Obsolete("Provider property has been deprecated.  Use the Add method.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public string Provider
		{
			get
			{
				string providers = this._providers;
				if (providers == null)
				{
					return ADP.StrEmpty;
				}
				return providers;
			}
			set
			{
				this._providers = value;
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00261AF8 File Offset: 0x00260EF8
		public override IPermission CreatePermission()
		{
			return new OleDbPermission(this);
		}

		// Token: 0x0400144A RID: 5194
		private string _providers;
	}
}
