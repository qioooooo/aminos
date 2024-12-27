using System;

namespace System.Management.Instrumentation
{
	// Token: 0x02000090 RID: 144
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class InstrumentationClassAttribute : Attribute
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00021801 File Offset: 0x00020801
		public InstrumentationClassAttribute(InstrumentationType instrumentationType)
		{
			this.instrumentationType = instrumentationType;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00021810 File Offset: 0x00020810
		public InstrumentationClassAttribute(InstrumentationType instrumentationType, string managedBaseClassName)
		{
			this.instrumentationType = instrumentationType;
			this.managedBaseClassName = managedBaseClassName;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00021826 File Offset: 0x00020826
		public InstrumentationType InstrumentationType
		{
			get
			{
				return this.instrumentationType;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0002182E File Offset: 0x0002082E
		public string ManagedBaseClassName
		{
			get
			{
				if (this.managedBaseClassName == null || this.managedBaseClassName.Length == 0)
				{
					return null;
				}
				return this.managedBaseClassName;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00021850 File Offset: 0x00020850
		internal static InstrumentationClassAttribute GetAttribute(Type type)
		{
			if (type == typeof(BaseEvent) || type == typeof(Instance))
			{
				return null;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(InstrumentationClassAttribute), true);
			if (customAttributes.Length > 0)
			{
				return (InstrumentationClassAttribute)customAttributes[0];
			}
			return null;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0002189B File Offset: 0x0002089B
		internal static Type GetBaseInstrumentationType(Type type)
		{
			if (InstrumentationClassAttribute.GetAttribute(type.BaseType) != null)
			{
				return type.BaseType;
			}
			return null;
		}

		// Token: 0x0400025E RID: 606
		private InstrumentationType instrumentationType;

		// Token: 0x0400025F RID: 607
		private string managedBaseClassName;
	}
}
