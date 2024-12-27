using System;
using System.ComponentModel;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OleDb
{
	// Token: 0x02000235 RID: 565
	[Serializable]
	public sealed class OleDbPermission : DBDataPermission
	{
		// Token: 0x06002044 RID: 8260 RVA: 0x0026196C File Offset: 0x00260D6C
		[Obsolete("OleDbPermission() has been deprecated.  Use the OleDbPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public OleDbPermission()
			: this(PermissionState.None)
		{
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00261980 File Offset: 0x00260D80
		public OleDbPermission(PermissionState state)
			: base(state)
		{
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00261994 File Offset: 0x00260D94
		[Obsolete("OleDbPermission(PermissionState state, Boolean allowBlankPassword) has been deprecated.  Use the OleDbPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public OleDbPermission(PermissionState state, bool allowBlankPassword)
			: this(state)
		{
			base.AllowBlankPassword = allowBlankPassword;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x002619B0 File Offset: 0x00260DB0
		private OleDbPermission(OleDbPermission permission)
			: base(permission)
		{
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x002619C4 File Offset: 0x00260DC4
		internal OleDbPermission(OleDbPermissionAttribute permissionAttribute)
			: base(permissionAttribute)
		{
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x002619D8 File Offset: 0x00260DD8
		internal OleDbPermission(OleDbConnectionString constr)
			: base(constr)
		{
			if (constr == null || constr.IsEmpty)
			{
				base.Add(ADP.StrEmpty, ADP.StrEmpty, KeyRestrictionBehavior.AllowOnly);
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x00261A08 File Offset: 0x00260E08
		// (set) Token: 0x0600204B RID: 8267 RVA: 0x00261A5C File Offset: 0x00260E5C
		[Browsable(false)]
		[Obsolete("Provider property has been deprecated.  Use the Add method.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string Provider
		{
			get
			{
				string text = this._providers;
				if (text == null)
				{
					string[] providerRestriction = this._providerRestriction;
					if (providerRestriction != null && 0 < providerRestriction.Length)
					{
						text = providerRestriction[0];
						for (int i = 1; i < providerRestriction.Length; i++)
						{
							text = text + ";" + providerRestriction[i];
						}
					}
				}
				if (text == null)
				{
					return ADP.StrEmpty;
				}
				return text;
			}
			set
			{
				string[] array = null;
				if (!ADP.IsEmpty(value))
				{
					array = value.Split(new char[] { ';' });
					array = DBConnectionString.RemoveDuplicates(array);
				}
				this._providerRestriction = array;
				this._providers = value;
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00261A9C File Offset: 0x00260E9C
		public override IPermission Copy()
		{
			return new OleDbPermission(this);
		}

		// Token: 0x04001448 RID: 5192
		private string[] _providerRestriction;

		// Token: 0x04001449 RID: 5193
		private string _providers;
	}
}
