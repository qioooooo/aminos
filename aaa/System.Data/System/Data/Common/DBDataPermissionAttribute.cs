using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Data.Common
{
	// Token: 0x02000135 RID: 309
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class DBDataPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06001483 RID: 5251 RVA: 0x00227EB4 File Offset: 0x002272B4
		protected DBDataPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x00227EC8 File Offset: 0x002272C8
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x00227EDC File Offset: 0x002272DC
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

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x00227EF0 File Offset: 0x002272F0
		// (set) Token: 0x06001487 RID: 5255 RVA: 0x00227F10 File Offset: 0x00227310
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x00227F24 File Offset: 0x00227324
		// (set) Token: 0x06001489 RID: 5257 RVA: 0x00227F38 File Offset: 0x00227338
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

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x00227F68 File Offset: 0x00227368
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x00227F88 File Offset: 0x00227388
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

		// Token: 0x0600148C RID: 5260 RVA: 0x00227F9C File Offset: 0x0022739C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeConnectionString()
		{
			return null != this._connectionString;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00227FB8 File Offset: 0x002273B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeKeyRestrictions()
		{
			return null != this._restrictions;
		}

		// Token: 0x04000C40 RID: 3136
		private bool _allowBlankPassword;

		// Token: 0x04000C41 RID: 3137
		private string _connectionString;

		// Token: 0x04000C42 RID: 3138
		private string _restrictions;

		// Token: 0x04000C43 RID: 3139
		private KeyRestrictionBehavior _behavior;
	}
}
