using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000136 RID: 310
	public sealed class SoapExtensionTypeElement : ConfigurationElement
	{
		// Token: 0x0600098E RID: 2446 RVA: 0x00045A6C File Offset: 0x00044A6C
		public SoapExtensionTypeElement()
		{
			this.properties.Add(this.group);
			this.properties.Add(this.priority);
			this.properties.Add(this.type);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00045B3D File Offset: 0x00044B3D
		public SoapExtensionTypeElement(string type, int priority, PriorityGroup group)
			: this()
		{
			this.Type = Type.GetType(type, true, true);
			this.Priority = priority;
			this.Group = group;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00045B61 File Offset: 0x00044B61
		public SoapExtensionTypeElement(Type type, int priority, PriorityGroup group)
			: this(type.AssemblyQualifiedName, priority, group)
		{
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x00045B71 File Offset: 0x00044B71
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x00045B84 File Offset: 0x00044B84
		[ConfigurationProperty("group", IsKey = true, DefaultValue = PriorityGroup.Low)]
		public PriorityGroup Group
		{
			get
			{
				return (PriorityGroup)base[this.group];
			}
			set
			{
				if (Enum.IsDefined(typeof(PriorityGroup), value))
				{
					base[this.group] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("Invalid_priority_group_value"), "value");
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x00045BC4 File Offset: 0x00044BC4
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x00045BD7 File Offset: 0x00044BD7
		[ConfigurationProperty("priority", IsKey = true, DefaultValue = 0)]
		[IntegerValidator(MinValue = 0)]
		public int Priority
		{
			get
			{
				return (int)base[this.priority];
			}
			set
			{
				base[this.priority] = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x00045BEB File Offset: 0x00044BEB
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x00045BFE File Offset: 0x00044BFE
		[TypeConverter(typeof(TypeTypeConverter))]
		[ConfigurationProperty("type", IsKey = true)]
		public Type Type
		{
			get
			{
				return (Type)base[this.type];
			}
			set
			{
				base[this.type] = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x00045C0D File Offset: 0x00044C0D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04000610 RID: 1552
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000611 RID: 1553
		private readonly ConfigurationProperty group = new ConfigurationProperty("group", typeof(PriorityGroup), PriorityGroup.Low, new EnumConverter(typeof(PriorityGroup)), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000612 RID: 1554
		private readonly ConfigurationProperty priority = new ConfigurationProperty("priority", typeof(int), 0, null, new IntegerValidator(0, int.MaxValue), ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000613 RID: 1555
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(Type), null, new TypeTypeConverter(), null, ConfigurationPropertyOptions.IsKey);
	}
}
