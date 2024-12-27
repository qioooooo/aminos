using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000248 RID: 584
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SessionPageStateSection : ConfigurationSection
	{
		// Token: 0x06001EDB RID: 7899 RVA: 0x00089CF4 File Offset: 0x00088CF4
		static SessionPageStateSection()
		{
			SessionPageStateSection._properties.Add(SessionPageStateSection._propHistorySize);
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x00089D41 File Offset: 0x00088D41
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SessionPageStateSection._properties;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x00089D48 File Offset: 0x00088D48
		// (set) Token: 0x06001EDE RID: 7902 RVA: 0x00089D5A File Offset: 0x00088D5A
		[IntegerValidator(MinValue = 1)]
		[ConfigurationProperty("historySize", DefaultValue = 9)]
		public int HistorySize
		{
			get
			{
				return (int)base[SessionPageStateSection._propHistorySize];
			}
			set
			{
				base[SessionPageStateSection._propHistorySize] = value;
			}
		}

		// Token: 0x04001A1F RID: 6687
		public const int DefaultHistorySize = 9;

		// Token: 0x04001A20 RID: 6688
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A21 RID: 6689
		private static readonly ConfigurationProperty _propHistorySize = new ConfigurationProperty("historySize", typeof(int), 9, null, StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);
	}
}
