using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001D7 RID: 471
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class EventMappingSettings : ConfigurationElement
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x0007B493 File Offset: 0x0007A493
		// (set) Token: 0x06001A57 RID: 6743 RVA: 0x0007B49B File Offset: 0x0007A49B
		internal Type RealType
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0007B4A4 File Offset: 0x0007A4A4
		static EventMappingSettings()
		{
			EventMappingSettings._properties.Add(EventMappingSettings._propName);
			EventMappingSettings._properties.Add(EventMappingSettings._propType);
			EventMappingSettings._properties.Add(EventMappingSettings._propStartEventCode);
			EventMappingSettings._properties.Add(EventMappingSettings._propEndEventCode);
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0007B587 File Offset: 0x0007A587
		internal EventMappingSettings()
		{
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0007B58F File Offset: 0x0007A58F
		public EventMappingSettings(string name, string type, int startEventCode, int endEventCode)
			: this()
		{
			this.Name = name;
			this.Type = type;
			this.StartEventCode = startEventCode;
			this.EndEventCode = endEventCode;
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0007B5B4 File Offset: 0x0007A5B4
		public EventMappingSettings(string name, string type)
			: this()
		{
			this.Name = name;
			this.Type = type;
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0007B5CA File Offset: 0x0007A5CA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return EventMappingSettings._properties;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0007B5D1 File Offset: 0x0007A5D1
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x0007B5E3 File Offset: 0x0007A5E3
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[EventMappingSettings._propName];
			}
			set
			{
				base[EventMappingSettings._propName] = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x0007B5F1 File Offset: 0x0007A5F1
		// (set) Token: 0x06001A60 RID: 6752 RVA: 0x0007B603 File Offset: 0x0007A603
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get
			{
				return (string)base[EventMappingSettings._propType];
			}
			set
			{
				base[EventMappingSettings._propType] = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0007B611 File Offset: 0x0007A611
		// (set) Token: 0x06001A62 RID: 6754 RVA: 0x0007B623 File Offset: 0x0007A623
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("startEventCode", DefaultValue = 0)]
		public int StartEventCode
		{
			get
			{
				return (int)base[EventMappingSettings._propStartEventCode];
			}
			set
			{
				base[EventMappingSettings._propStartEventCode] = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0007B636 File Offset: 0x0007A636
		// (set) Token: 0x06001A64 RID: 6756 RVA: 0x0007B648 File Offset: 0x0007A648
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("endEventCode", DefaultValue = 2147483647)]
		public int EndEventCode
		{
			get
			{
				return (int)base[EventMappingSettings._propEndEventCode];
			}
			set
			{
				base[EventMappingSettings._propEndEventCode] = value;
			}
		}

		// Token: 0x040017D3 RID: 6099
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017D4 RID: 6100
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040017D5 RID: 6101
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040017D6 RID: 6102
		private static readonly ConfigurationProperty _propStartEventCode = new ConfigurationProperty("startEventCode", typeof(int), 0, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017D7 RID: 6103
		private static readonly ConfigurationProperty _propEndEventCode = new ConfigurationProperty("endEventCode", typeof(int), int.MaxValue, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017D8 RID: 6104
		private Type _type;
	}
}
