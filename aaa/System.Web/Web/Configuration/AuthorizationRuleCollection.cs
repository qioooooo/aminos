using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Configuration
{
	// Token: 0x020001A4 RID: 420
	[ConfigurationCollection(typeof(AuthorizationRule), AddItemName = "allow,deny", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AuthorizationRuleCollection : ConfigurationElementCollection
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0004E730 File Offset: 0x0004D730
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AuthorizationRuleCollection._properties;
			}
		}

		// Token: 0x1700044E RID: 1102
		public AuthorizationRule this[int index]
		{
			get
			{
				return (AuthorizationRule)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0004E75F File Offset: 0x0004D75F
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthorizationRule();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0004E768 File Offset: 0x0004D768
		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			AuthorizationRule authorizationRule = new AuthorizationRule();
			string text;
			if ((text = elementName.ToLower(CultureInfo.InvariantCulture)) != null)
			{
				if (!(text == "allow"))
				{
					if (text == "deny")
					{
						authorizationRule.Action = AuthorizationRuleAction.Deny;
					}
				}
				else
				{
					authorizationRule.Action = AuthorizationRuleAction.Allow;
				}
			}
			return authorizationRule;
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004E7B8 File Offset: 0x0004D7B8
		protected override object GetElementKey(ConfigurationElement element)
		{
			AuthorizationRule authorizationRule = (AuthorizationRule)element;
			return authorizationRule._ActionString;
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x0004E7D2 File Offset: 0x0004D7D2
		protected override string ElementName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x0004E7D9 File Offset: 0x0004D7D9
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMapAlternate;
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004E7DC File Offset: 0x0004D7DC
		protected override bool IsElementName(string elementname)
		{
			bool flag = false;
			string text;
			if ((text = elementname.ToLower(CultureInfo.InvariantCulture)) != null && (text == "allow" || text == "deny"))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004E818 File Offset: 0x0004D818
		internal bool IsUserAllowed(IPrincipal user, string verb)
		{
			if (user == null)
			{
				return false;
			}
			if (!this._fCheckForCommonCasesDone)
			{
				this.DoCheckForCommonCases();
				this._fCheckForCommonCasesDone = true;
			}
			if (!user.Identity.IsAuthenticated && this._iAnonymousAllowed != 0)
			{
				return this._iAnonymousAllowed > 0;
			}
			if (this._iAllUsersAllowed != 0)
			{
				return this._iAllUsersAllowed > 0;
			}
			foreach (object obj in this)
			{
				AuthorizationRule authorizationRule = (AuthorizationRule)obj;
				int num = authorizationRule.IsUserAllowed(user, verb);
				if (num != 0)
				{
					return num > 0;
				}
			}
			return false;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0004E8CC File Offset: 0x0004D8CC
		private void DoCheckForCommonCases()
		{
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			foreach (object obj in this)
			{
				AuthorizationRule authorizationRule = (AuthorizationRule)obj;
				if (authorizationRule.Everyone)
				{
					if (!flag2 && authorizationRule.Action == AuthorizationRuleAction.Deny)
					{
						this._iAllUsersAllowed = -1;
					}
					if (!flag3 && authorizationRule.Action == AuthorizationRuleAction.Allow)
					{
						this._iAllUsersAllowed = 1;
					}
					break;
				}
				if (flag && authorizationRule.IncludesAnonymous)
				{
					if (!flag2 && authorizationRule.Action == AuthorizationRuleAction.Deny)
					{
						this._iAnonymousAllowed = -1;
					}
					if (!flag3 && authorizationRule.Action == AuthorizationRuleAction.Allow)
					{
						this._iAnonymousAllowed = 1;
					}
					flag = false;
				}
				if (!flag2 && authorizationRule.Action == AuthorizationRuleAction.Allow)
				{
					flag2 = true;
				}
				if (!flag3 && authorizationRule.Action == AuthorizationRuleAction.Deny)
				{
					flag3 = true;
				}
				if (!flag && flag2 && flag3)
				{
					break;
				}
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004E9B4 File Offset: 0x0004D9B4
		public void Add(AuthorizationRule rule)
		{
			this.BaseAdd(-1, rule);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004E9BE File Offset: 0x0004D9BE
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004E9C6 File Offset: 0x0004D9C6
		public AuthorizationRule Get(int index)
		{
			return (AuthorizationRule)base.BaseGet(index);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004E9D4 File Offset: 0x0004D9D4
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004E9DD File Offset: 0x0004D9DD
		public void Set(int index, AuthorizationRule rule)
		{
			this.BaseAdd(index, rule);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004E9E8 File Offset: 0x0004D9E8
		public int IndexOf(AuthorizationRule rule)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (object.Equals(this.Get(i), rule))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004EA18 File Offset: 0x0004DA18
		public void Remove(AuthorizationRule rule)
		{
			int num = this.IndexOf(rule);
			if (num >= 0)
			{
				base.BaseRemoveAt(num);
			}
		}

		// Token: 0x040016CC RID: 5836
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016CD RID: 5837
		private int _iAllUsersAllowed;

		// Token: 0x040016CE RID: 5838
		private int _iAnonymousAllowed;

		// Token: 0x040016CF RID: 5839
		private bool _fCheckForCommonCasesDone;
	}
}
