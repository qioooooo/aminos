using System;
using System.Configuration;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Web.Configuration
{
	// Token: 0x020001A5 RID: 421
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AuthorizationSection : ConfigurationSection
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x0004EA38 File Offset: 0x0004DA38
		internal bool EveryoneAllowed
		{
			get
			{
				return this._EveryoneAllowed;
			}
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0004EA40 File Offset: 0x0004DA40
		static AuthorizationSection()
		{
			AuthorizationSection._properties.Add(AuthorizationSection._propRules);
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x0004EA7A File Offset: 0x0004DA7A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AuthorizationSection._properties;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x0004EA81 File Offset: 0x0004DA81
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AuthorizationRuleCollection Rules
		{
			get
			{
				return (AuthorizationRuleCollection)base[AuthorizationSection._propRules];
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0004EA93 File Offset: 0x0004DA93
		protected override void PostDeserialize()
		{
			if (this.Rules.Count > 0)
			{
				this._EveryoneAllowed = this.Rules[0].Action == AuthorizationRuleAction.Allow && this.Rules[0].Everyone;
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0004EAD1 File Offset: 0x0004DAD1
		internal bool IsUserAllowed(IPrincipal user, string verb)
		{
			return this.Rules.IsUserAllowed(user, verb);
		}

		// Token: 0x040016D0 RID: 5840
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016D1 RID: 5841
		private static readonly ConfigurationProperty _propRules = new ConfigurationProperty(null, typeof(AuthorizationRuleCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x040016D2 RID: 5842
		private bool _EveryoneAllowed;
	}
}
