using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OracleClient
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public sealed class OraclePermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x0006DEB0 File Offset: 0x0006D2B0
		public OraclePermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this._isUnrestricted = true;
				return;
			}
			if (state == PermissionState.None)
			{
				this._isUnrestricted = false;
				return;
			}
			throw ADP.InvalidPermissionState(state);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0006DEEC File Offset: 0x0006D2EC
		private OraclePermission(OraclePermission permission)
		{
			if (permission == null)
			{
				throw ADP.ArgumentNull("permissionAttribute");
			}
			this.CopyFrom(permission);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0006DF20 File Offset: 0x0006D320
		internal OraclePermission(OraclePermissionAttribute permissionAttribute)
		{
			if (permissionAttribute == null)
			{
				throw ADP.ArgumentNull("permissionAttribute");
			}
			this._isUnrestricted = permissionAttribute.Unrestricted;
			if (!this._isUnrestricted)
			{
				this._allowBlankPassword = permissionAttribute.AllowBlankPassword;
				if (permissionAttribute.ShouldSerializeConnectionString() || permissionAttribute.ShouldSerializeKeyRestrictions())
				{
					this.Add(permissionAttribute.ConnectionString, permissionAttribute.KeyRestrictions, permissionAttribute.KeyRestrictionBehavior);
				}
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0006DF94 File Offset: 0x0006D394
		internal OraclePermission(OracleConnectionString connectionOptions)
		{
			if (connectionOptions != null)
			{
				this._allowBlankPassword = connectionOptions.HasBlankPassword;
				this.AddPermissionEntry(new DBConnectionString(connectionOptions));
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0006DFD0 File Offset: 0x0006D3D0
		public void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
		{
			DBConnectionString dbconnectionString = new DBConnectionString(connectionString, restrictions, behavior, OracleConnectionString.GetParseSynonyms(), false);
			this.AddPermissionEntry(dbconnectionString);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0006DFF4 File Offset: 0x0006D3F4
		public override IPermission Copy()
		{
			return new OraclePermission(this);
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0006E008 File Offset: 0x0006D408
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0006E01C File Offset: 0x0006D41C
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

		// Token: 0x06000666 RID: 1638 RVA: 0x0006E030 File Offset: 0x0006D430
		internal void AddPermissionEntry(DBConnectionString entry)
		{
			if (this._keyvaluetree == null)
			{
				this._keyvaluetree = new NameValuePermission();
			}
			if (this._keyvalues == null)
			{
				this._keyvalues = new ArrayList();
			}
			NameValuePermission.AddEntry(this._keyvaluetree, this._keyvalues, entry);
			this._isUnrestricted = false;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0006E07C File Offset: 0x0006D47C
		private void Clear()
		{
			this._keyvaluetree = null;
			this._keyvalues = null;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0006E098 File Offset: 0x0006D498
		private void CopyFrom(OraclePermission permission)
		{
			this._isUnrestricted = permission.IsUnrestricted();
			if (!this._isUnrestricted)
			{
				this._allowBlankPassword = permission.AllowBlankPassword;
				if (permission._keyvalues != null)
				{
					this._keyvalues = (ArrayList)permission._keyvalues.Clone();
					if (permission._keyvaluetree != null)
					{
						this._keyvaluetree = permission._keyvaluetree.CopyNameValue();
					}
				}
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0006E0FC File Offset: 0x0006D4FC
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != base.GetType())
			{
				throw ADP.PermissionTypeMismatch();
			}
			if (this.IsUnrestricted())
			{
				return target.Copy();
			}
			OraclePermission oraclePermission = (OraclePermission)target;
			if (oraclePermission.IsUnrestricted())
			{
				return this.Copy();
			}
			OraclePermission oraclePermission2 = (OraclePermission)oraclePermission.Copy();
			oraclePermission2._allowBlankPassword &= this.AllowBlankPassword;
			if (this._keyvalues != null && oraclePermission2._keyvalues != null)
			{
				oraclePermission2._keyvalues.Clear();
				oraclePermission2._keyvaluetree.Intersect(oraclePermission2._keyvalues, this._keyvaluetree);
			}
			else
			{
				oraclePermission2._keyvalues = null;
				oraclePermission2._keyvaluetree = null;
			}
			if (oraclePermission2.IsEmpty())
			{
				oraclePermission2 = null;
			}
			return oraclePermission2;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0006E1B4 File Offset: 0x0006D5B4
		private bool IsEmpty()
		{
			ArrayList keyvalues = this._keyvalues;
			return !this.IsUnrestricted() && !this.AllowBlankPassword && (keyvalues == null || 0 == keyvalues.Count);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0006E1EC File Offset: 0x0006D5EC
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.IsEmpty();
			}
			if (target.GetType() != base.GetType())
			{
				throw ADP.PermissionTypeMismatch();
			}
			OraclePermission oraclePermission = target as OraclePermission;
			bool flag = oraclePermission.IsUnrestricted();
			if (!flag && !this.IsUnrestricted() && (!this.AllowBlankPassword || oraclePermission.AllowBlankPassword) && (this._keyvalues == null || oraclePermission._keyvaluetree != null))
			{
				flag = true;
				if (this._keyvalues != null)
				{
					foreach (object obj in this._keyvalues)
					{
						DBConnectionString dbconnectionString = (DBConnectionString)obj;
						if (!oraclePermission._keyvaluetree.CheckValueForKeyPermit(dbconnectionString))
						{
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0006E2C4 File Offset: 0x0006D6C4
		public bool IsUnrestricted()
		{
			return this._isUnrestricted;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0006E2D8 File Offset: 0x0006D6D8
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != base.GetType())
			{
				throw ADP.PermissionTypeMismatch();
			}
			if (this.IsUnrestricted())
			{
				return this.Copy();
			}
			OraclePermission oraclePermission = (OraclePermission)target.Copy();
			if (!oraclePermission.IsUnrestricted())
			{
				oraclePermission._allowBlankPassword |= this.AllowBlankPassword;
				if (this._keyvalues != null)
				{
					foreach (object obj in this._keyvalues)
					{
						DBConnectionString dbconnectionString = (DBConnectionString)obj;
						oraclePermission.AddPermissionEntry(dbconnectionString);
					}
				}
			}
			if (!oraclePermission.IsEmpty())
			{
				return oraclePermission;
			}
			return null;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0006E3A4 File Offset: 0x0006D7A4
		private string DecodeXmlValue(string value)
		{
			if (value != null && 0 < value.Length)
			{
				value = value.Replace("&quot;", "\"");
				value = value.Replace("&apos;", "'");
				value = value.Replace("&lt;", "<");
				value = value.Replace("&gt;", ">");
				value = value.Replace("&amp;", "&");
			}
			return value;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0006E418 File Offset: 0x0006D818
		private string EncodeXmlValue(string value)
		{
			if (value != null && 0 < value.Length)
			{
				value = value.Replace('\0', ' ');
				value = value.Trim();
				value = value.Replace("&", "&amp;");
				value = value.Replace(">", "&gt;");
				value = value.Replace("<", "&lt;");
				value = value.Replace("'", "&apos;");
				value = value.Replace("\"", "&quot;");
			}
			return value;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0006E4A4 File Offset: 0x0006D8A4
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw ADP.ArgumentNull("securityElement");
			}
			string text = securityElement.Tag;
			if (!text.Equals("Permission") && !text.Equals("IPermission"))
			{
				throw ADP.NotAPermissionElement();
			}
			string text2 = securityElement.Attribute("version");
			if (text2 != null && !text2.Equals("1"))
			{
				throw ADP.InvalidXMLBadVersion();
			}
			string text3 = securityElement.Attribute("Unrestricted");
			this._isUnrestricted = text3 != null && bool.Parse(text3);
			this.Clear();
			if (!this._isUnrestricted)
			{
				string text4 = securityElement.Attribute("AllowBlankPassword");
				this._allowBlankPassword = text4 != null && bool.Parse(text4);
				ArrayList children = securityElement.Children;
				if (children == null)
				{
					return;
				}
				using (IEnumerator enumerator = children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						SecurityElement securityElement2 = (SecurityElement)obj;
						text = securityElement2.Tag;
						if ("add" == text || (text != null && "add" == text.ToLower(CultureInfo.InvariantCulture)))
						{
							string text5 = securityElement2.Attribute("ConnectionString");
							string text6 = securityElement2.Attribute("KeyRestrictions");
							string text7 = securityElement2.Attribute("KeyRestrictionBehavior");
							KeyRestrictionBehavior keyRestrictionBehavior = KeyRestrictionBehavior.AllowOnly;
							if (text7 != null)
							{
								keyRestrictionBehavior = (KeyRestrictionBehavior)Enum.Parse(typeof(KeyRestrictionBehavior), text7, true);
							}
							text5 = this.DecodeXmlValue(text5);
							text6 = this.DecodeXmlValue(text6);
							this.Add(text5, text6, keyRestrictionBehavior);
						}
					}
					return;
				}
			}
			this._allowBlankPassword = false;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0006E660 File Offset: 0x0006DA60
		public override SecurityElement ToXml()
		{
			Type type = base.GetType();
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", type.AssemblyQualifiedName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				securityElement.AddAttribute("AllowBlankPassword", this._allowBlankPassword.ToString(CultureInfo.InvariantCulture));
				if (this._keyvalues != null)
				{
					foreach (object obj in this._keyvalues)
					{
						DBConnectionString dbconnectionString = (DBConnectionString)obj;
						SecurityElement securityElement2 = new SecurityElement("add");
						string text = dbconnectionString.ConnectionString;
						text = this.EncodeXmlValue(text);
						if (!ADP.IsEmpty(text))
						{
							securityElement2.AddAttribute("ConnectionString", text);
						}
						text = dbconnectionString.Restrictions;
						text = this.EncodeXmlValue(text);
						if (text == null)
						{
							text = ADP.StrEmpty;
						}
						securityElement2.AddAttribute("KeyRestrictions", text);
						text = dbconnectionString.Behavior.ToString();
						securityElement2.AddAttribute("KeyRestrictionBehavior", text);
						securityElement.AddChild(securityElement2);
					}
				}
			}
			return securityElement;
		}

		// Token: 0x040004A6 RID: 1190
		private bool _isUnrestricted;

		// Token: 0x040004A7 RID: 1191
		private bool _allowBlankPassword;

		// Token: 0x040004A8 RID: 1192
		private NameValuePermission _keyvaluetree = NameValuePermission.Default;

		// Token: 0x040004A9 RID: 1193
		private ArrayList _keyvalues;
	}
}
