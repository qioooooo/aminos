using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001A2 RID: 418
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AuthorizationRule : ConfigurationElement
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0004DC53 File Offset: 0x0004CC53
		internal bool Everyone
		{
			get
			{
				return this._Everyone;
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0004DC5C File Offset: 0x0004CC5C
		static AuthorizationRule()
		{
			AuthorizationRule._properties.Add(AuthorizationRule._propVerbs);
			AuthorizationRule._properties.Add(AuthorizationRule._propUsers);
			AuthorizationRule._properties.Add(AuthorizationRule._propRoles);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0004DD10 File Offset: 0x0004CD10
		internal AuthorizationRule()
		{
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0004DD59 File Offset: 0x0004CD59
		public AuthorizationRule(AuthorizationRuleAction action)
			: this()
		{
			this.Action = action;
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x0004DD68 File Offset: 0x0004CD68
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AuthorizationRule._properties;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0004DD70 File Offset: 0x0004CD70
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			AuthorizationRule authorizationRule = parentElement as AuthorizationRule;
			AuthorizationRule authorizationRule2 = sourceElement as AuthorizationRule;
			if (authorizationRule != null)
			{
				authorizationRule.UpdateUsersRolesVerbs();
			}
			if (authorizationRule2 != null)
			{
				authorizationRule2.UpdateUsersRolesVerbs();
			}
			base.Unmerge(sourceElement, parentElement, saveMode);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004DDA8 File Offset: 0x0004CDA8
		protected override void Reset(ConfigurationElement parentElement)
		{
			AuthorizationRule authorizationRule = parentElement as AuthorizationRule;
			if (authorizationRule != null)
			{
				authorizationRule.UpdateUsersRolesVerbs();
			}
			base.Reset(parentElement);
			this.EvaluateData();
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004DDD2 File Offset: 0x0004CDD2
		internal void AddRole(string role)
		{
			if (!string.IsNullOrEmpty(role))
			{
				role = role.ToLower(CultureInfo.InvariantCulture);
			}
			this.Roles.Add(role);
			this.RolesExpanded.Add(this.ExpandName(role));
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0004DE09 File Offset: 0x0004CE09
		internal void AddUser(string user)
		{
			if (!string.IsNullOrEmpty(user))
			{
				user = user.ToLower(CultureInfo.InvariantCulture);
			}
			this.Users.Add(user);
			this.UsersExpanded.Add(this.ExpandName(user));
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0004DE40 File Offset: 0x0004CE40
		private void UpdateUsersRolesVerbs()
		{
			CommaDelimitedStringCollection commaDelimitedStringCollection = (CommaDelimitedStringCollection)this.Roles;
			CommaDelimitedStringCollection commaDelimitedStringCollection2 = (CommaDelimitedStringCollection)this.Users;
			CommaDelimitedStringCollection commaDelimitedStringCollection3 = (CommaDelimitedStringCollection)this.Verbs;
			if (commaDelimitedStringCollection.IsModified)
			{
				this._RolesExpanded = null;
				base[AuthorizationRule._propRoles] = commaDelimitedStringCollection;
			}
			if (commaDelimitedStringCollection2.IsModified)
			{
				this._UsersExpanded = null;
				base[AuthorizationRule._propUsers] = commaDelimitedStringCollection2;
			}
			if (commaDelimitedStringCollection3.IsModified)
			{
				base[AuthorizationRule._propVerbs] = commaDelimitedStringCollection3;
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0004DEBC File Offset: 0x0004CEBC
		protected override bool IsModified()
		{
			this.UpdateUsersRolesVerbs();
			return this._ActionModified || base.IsModified() || ((CommaDelimitedStringCollection)this.Users).IsModified || ((CommaDelimitedStringCollection)this.Roles).IsModified || ((CommaDelimitedStringCollection)this.Verbs).IsModified;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0004DF15 File Offset: 0x0004CF15
		protected override void ResetModified()
		{
			this._ActionModified = false;
			base.ResetModified();
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0004DF24 File Offset: 0x0004CF24
		public override bool Equals(object obj)
		{
			AuthorizationRule authorizationRule = obj as AuthorizationRule;
			bool flag = false;
			if (authorizationRule != null)
			{
				this.UpdateUsersRolesVerbs();
				flag = authorizationRule.Verbs.ToString() == this.Verbs.ToString() && authorizationRule.Roles.ToString() == this.Roles.ToString() && authorizationRule.Users.ToString() == this.Users.ToString() && authorizationRule.Action == this.Action;
			}
			return flag;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0004DFB0 File Offset: 0x0004CFB0
		public override int GetHashCode()
		{
			string text = this.Verbs.ToString();
			string text2 = this.Roles.ToString();
			string text3 = this.Users.ToString();
			if (text == null)
			{
				text = string.Empty;
			}
			if (text2 == null)
			{
				text2 = string.Empty;
			}
			if (text3 == null)
			{
				text3 = string.Empty;
			}
			return HashCodeCombiner.CombineHashCodes(text.GetHashCode(), text2.GetHashCode(), text3.GetHashCode(), (int)this.Action);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0004E01B File Offset: 0x0004D01B
		protected override void SetReadOnly()
		{
			((CommaDelimitedStringCollection)this.Users).SetReadOnly();
			((CommaDelimitedStringCollection)this.Roles).SetReadOnly();
			((CommaDelimitedStringCollection)this.Verbs).SetReadOnly();
			base.SetReadOnly();
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x0004E053 File Offset: 0x0004D053
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x0004E05C File Offset: 0x0004D05C
		public AuthorizationRuleAction Action
		{
			get
			{
				return this._Action;
			}
			set
			{
				this._ElementName = value.ToString().ToLower(CultureInfo.InvariantCulture);
				this._Action = value;
				this._ActionString = this._Action.ToString();
				this._ActionModified = true;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x0004E0A8 File Offset: 0x0004D0A8
		[ConfigurationProperty("verbs")]
		[TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
		public StringCollection Verbs
		{
			get
			{
				if (this._Verbs == null)
				{
					CommaDelimitedStringCollection commaDelimitedStringCollection = (CommaDelimitedStringCollection)base[AuthorizationRule._propVerbs];
					if (commaDelimitedStringCollection == null)
					{
						this._Verbs = new CommaDelimitedStringCollection();
					}
					else
					{
						this._Verbs = commaDelimitedStringCollection.Clone();
					}
				}
				return this._Verbs;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x0004E0F0 File Offset: 0x0004D0F0
		[ConfigurationProperty("users")]
		[TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
		public StringCollection Users
		{
			get
			{
				if (this._Users == null)
				{
					CommaDelimitedStringCollection commaDelimitedStringCollection = (CommaDelimitedStringCollection)base[AuthorizationRule._propUsers];
					if (commaDelimitedStringCollection == null)
					{
						this._Users = new CommaDelimitedStringCollection();
					}
					else
					{
						this._Users = commaDelimitedStringCollection.Clone();
					}
					this._UsersExpanded = null;
				}
				return this._Users;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x0004E140 File Offset: 0x0004D140
		[TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
		[ConfigurationProperty("roles")]
		public StringCollection Roles
		{
			get
			{
				if (this._Roles == null)
				{
					CommaDelimitedStringCollection commaDelimitedStringCollection = (CommaDelimitedStringCollection)base[AuthorizationRule._propRoles];
					if (commaDelimitedStringCollection == null)
					{
						this._Roles = new CommaDelimitedStringCollection();
					}
					else
					{
						this._Roles = commaDelimitedStringCollection.Clone();
					}
					this._RolesExpanded = null;
				}
				return this._Roles;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x0004E18F File Offset: 0x0004D18F
		internal StringCollection UsersExpanded
		{
			get
			{
				if (this._UsersExpanded == null)
				{
					this._UsersExpanded = this.CreateExpandedCollection(this.Users);
				}
				return this._UsersExpanded;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x0004E1B1 File Offset: 0x0004D1B1
		internal StringCollection RolesExpanded
		{
			get
			{
				if (this._RolesExpanded == null)
				{
					this._RolesExpanded = this.CreateExpandedCollection(this.Roles);
				}
				return this._RolesExpanded;
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0004E1D4 File Offset: 0x0004D1D4
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			bool flag = false;
			this.UpdateUsersRolesVerbs();
			if (base.SerializeElement(null, false))
			{
				if (writer != null)
				{
					writer.WriteStartElement(this._ElementName);
					flag |= base.SerializeElement(writer, false);
					writer.WriteEndElement();
				}
				else
				{
					flag |= base.SerializeElement(writer, false);
				}
			}
			return flag;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004E224 File Offset: 0x0004D224
		private string ExpandName(string name)
		{
			string text = name;
			if (StringUtil.StringStartsWith(name, ".\\"))
			{
				if (this.machineName == null)
				{
					this.machineName = HttpServerUtility.GetMachineNameInternal().ToLower(CultureInfo.InvariantCulture);
				}
				text = this.machineName + name.Substring(1);
			}
			return text;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0004E274 File Offset: 0x0004D274
		private StringCollection CreateExpandedCollection(StringCollection collection)
		{
			StringCollection stringCollection = new StringCollection();
			foreach (string text in collection)
			{
				string text2 = this.ExpandName(text);
				stringCollection.Add(text2);
			}
			return stringCollection;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0004E2D8 File Offset: 0x0004D2D8
		private void EvaluateData()
		{
			if (!this.DataReady)
			{
				if (this.Users.Count > 0)
				{
					foreach (string text in this.Users)
					{
						if (text.Length > 1)
						{
							int num = text.IndexOfAny(new char[] { '*', '?' });
							if (num >= 0)
							{
								throw new ConfigurationErrorsException(SR.GetString("Auth_rule_names_cant_contain_char", new object[] { text[num].ToString(CultureInfo.InvariantCulture) }));
							}
						}
						if (text.Equals("*"))
						{
							this._AllUsersSpecified = true;
						}
						if (text.Equals("?"))
						{
							this._AnonUserSpecified = true;
						}
					}
				}
				if (this.Roles.Count > 0)
				{
					foreach (string text2 in this.Roles)
					{
						if (text2.Length > 0)
						{
							int num2 = text2.IndexOfAny(new char[] { '*', '?' });
							if (num2 >= 0)
							{
								throw new ConfigurationErrorsException(SR.GetString("Auth_rule_names_cant_contain_char", new object[] { text2[num2].ToString(CultureInfo.InvariantCulture) }));
							}
						}
					}
				}
				this._Everyone = this._AllUsersSpecified && this.Verbs.Count == 0;
				this._RolesExpanded = this.CreateExpandedCollection(this.Roles);
				this._UsersExpanded = this.CreateExpandedCollection(this.Users);
				if (this.Roles.Count == 0 && this.Users.Count == 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Auth_rule_must_specify_users_andor_roles"));
				}
				this.DataReady = true;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x0004E4F8 File Offset: 0x0004D4F8
		internal bool IncludesAnonymous
		{
			get
			{
				this.EvaluateData();
				return this._AnonUserSpecified && this.Verbs.Count == 0;
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0004E518 File Offset: 0x0004D518
		protected override void PreSerialize(XmlWriter writer)
		{
			this.EvaluateData();
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004E520 File Offset: 0x0004D520
		protected override void PostDeserialize()
		{
			this.EvaluateData();
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0004E528 File Offset: 0x0004D528
		internal int IsUserAllowed(IPrincipal user, string verb)
		{
			this.EvaluateData();
			int num = ((this.Action == AuthorizationRuleAction.Allow) ? 1 : (-1));
			if (this.Everyone)
			{
				return num;
			}
			if (!this.FindVerb(verb))
			{
				return 0;
			}
			if (this._AllUsersSpecified)
			{
				return num;
			}
			if (this._AnonUserSpecified && !user.Identity.IsAuthenticated)
			{
				return num;
			}
			StringCollection stringCollection;
			StringCollection stringCollection2;
			if (user.Identity is WindowsIdentity)
			{
				stringCollection = this.UsersExpanded;
				stringCollection2 = this.RolesExpanded;
			}
			else
			{
				stringCollection = this.Users;
				stringCollection2 = this.Roles;
			}
			if (stringCollection.Count > 0 && this.FindUser(stringCollection, user.Identity.Name))
			{
				return num;
			}
			if (stringCollection2.Count > 0 && this.IsTheUserInAnyRole(stringCollection2, user))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004E5E0 File Offset: 0x0004D5E0
		private bool FindVerb(string verb)
		{
			if (this.Verbs.Count < 1)
			{
				return true;
			}
			foreach (string text in this.Verbs)
			{
				if (string.Equals(text, verb, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004E650 File Offset: 0x0004D650
		private bool FindUser(StringCollection users, string principal)
		{
			foreach (string text in users)
			{
				if (string.Equals(text, principal, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004E6AC File Offset: 0x0004D6AC
		private bool IsTheUserInAnyRole(StringCollection roles, IPrincipal principal)
		{
			if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
			{
				HttpRuntime.NamedPermissionSet.PermitOnly();
			}
			foreach (string text in roles)
			{
				if (principal.IsInRole(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040016B3 RID: 5811
		private const string _strAnonUserTag = "?";

		// Token: 0x040016B4 RID: 5812
		private const string _strAllUsersTag = "*";

		// Token: 0x040016B5 RID: 5813
		private static readonly TypeConverter s_PropConverter = new CommaDelimitedStringCollectionConverter();

		// Token: 0x040016B6 RID: 5814
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016B7 RID: 5815
		private static readonly ConfigurationProperty _propVerbs = new ConfigurationProperty("verbs", typeof(CommaDelimitedStringCollection), null, AuthorizationRule.s_PropConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040016B8 RID: 5816
		private static readonly ConfigurationProperty _propUsers = new ConfigurationProperty("users", typeof(CommaDelimitedStringCollection), null, AuthorizationRule.s_PropConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040016B9 RID: 5817
		private static readonly ConfigurationProperty _propRoles = new ConfigurationProperty("roles", typeof(CommaDelimitedStringCollection), null, AuthorizationRule.s_PropConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040016BA RID: 5818
		private AuthorizationRuleAction _Action = AuthorizationRuleAction.Allow;

		// Token: 0x040016BB RID: 5819
		internal string _ActionString = AuthorizationRuleAction.Allow.ToString();

		// Token: 0x040016BC RID: 5820
		private string _ElementName = "allow";

		// Token: 0x040016BD RID: 5821
		private CommaDelimitedStringCollection _Roles;

		// Token: 0x040016BE RID: 5822
		private CommaDelimitedStringCollection _Verbs;

		// Token: 0x040016BF RID: 5823
		private CommaDelimitedStringCollection _Users;

		// Token: 0x040016C0 RID: 5824
		private StringCollection _RolesExpanded;

		// Token: 0x040016C1 RID: 5825
		private StringCollection _UsersExpanded;

		// Token: 0x040016C2 RID: 5826
		private char[] _delimiters = new char[] { ',' };

		// Token: 0x040016C3 RID: 5827
		private string machineName;

		// Token: 0x040016C4 RID: 5828
		private bool _AllUsersSpecified;

		// Token: 0x040016C5 RID: 5829
		private bool _AnonUserSpecified;

		// Token: 0x040016C6 RID: 5830
		private bool DataReady;

		// Token: 0x040016C7 RID: 5831
		private bool _Everyone;

		// Token: 0x040016C8 RID: 5832
		private bool _ActionModified;
	}
}
