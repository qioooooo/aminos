using System;
using System.ComponentModel;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OracleClient
{
	// Token: 0x02000075 RID: 117
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class OraclePermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x0006E7C4 File Offset: 0x0006DBC4
		public OraclePermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0006E7D8 File Offset: 0x0006DBD8
		public override IPermission CreatePermission()
		{
			return new OraclePermission(this);
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0006E7EC File Offset: 0x0006DBEC
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0006E800 File Offset: 0x0006DC00
		public bool AllowBlankPassword
		{
			get
			{
				return this._allowBlankPassword;
			}
			set
			{
				this._allowBlankPassword = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0006E814 File Offset: 0x0006DC14
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0006E834 File Offset: 0x0006DC34
		public string ConnectionString
		{
			get
			{
				string connectionString = this._connectionString;
				if (connectionString == null)
				{
					return string.Empty;
				}
				return connectionString;
			}
			set
			{
				this._connectionString = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0006E848 File Offset: 0x0006DC48
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x0006E85C File Offset: 0x0006DC5C
		public KeyRestrictionBehavior KeyRestrictionBehavior
		{
			get
			{
				return this._behavior;
			}
			set
			{
				switch (value)
				{
				case KeyRestrictionBehavior.AllowOnly:
				case KeyRestrictionBehavior.PreventUsage:
					this._behavior = value;
					return;
				default:
					throw ADP.InvalidKeyRestrictionBehavior(value);
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0006E88C File Offset: 0x0006DC8C
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x0006E8AC File Offset: 0x0006DCAC
		public string KeyRestrictions
		{
			get
			{
				string restrictions = this._restrictions;
				if (restrictions == null)
				{
					return ADP.StrEmpty;
				}
				return restrictions;
			}
			set
			{
				this._restrictions = value;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0006E8C0 File Offset: 0x0006DCC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeConnectionString()
		{
			return null != this._connectionString;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0006E8DC File Offset: 0x0006DCDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeKeyRestrictions()
		{
			return null != this._restrictions;
		}

		// Token: 0x040004AA RID: 1194
		private bool _allowBlankPassword;

		// Token: 0x040004AB RID: 1195
		private string _connectionString;

		// Token: 0x040004AC RID: 1196
		private string _restrictions;

		// Token: 0x040004AD RID: 1197
		private KeyRestrictionBehavior _behavior;
	}
}
