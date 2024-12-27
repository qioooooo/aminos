using System;
using System.Reflection;

namespace System.Management.Instrumentation
{
	// Token: 0x02000091 RID: 145
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public class ManagedNameAttribute : Attribute
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x000218B2 File Offset: 0x000208B2
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000218BA File Offset: 0x000208BA
		public ManagedNameAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000218CC File Offset: 0x000208CC
		internal static string GetMemberName(MemberInfo member)
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(ManagedNameAttribute), false);
			if (customAttributes.Length > 0)
			{
				ManagedNameAttribute managedNameAttribute = (ManagedNameAttribute)customAttributes[0];
				if (managedNameAttribute.name != null && managedNameAttribute.name.Length != 0)
				{
					return managedNameAttribute.name;
				}
			}
			return member.Name;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0002191C File Offset: 0x0002091C
		internal static string GetBaseClassName(Type type)
		{
			InstrumentationClassAttribute attribute = InstrumentationClassAttribute.GetAttribute(type);
			string managedBaseClassName = attribute.ManagedBaseClassName;
			if (managedBaseClassName != null)
			{
				return managedBaseClassName;
			}
			if (InstrumentationClassAttribute.GetAttribute(type.BaseType) == null)
			{
				switch (attribute.InstrumentationType)
				{
				case InstrumentationType.Instance:
					return null;
				case InstrumentationType.Event:
					return "__ExtrinsicEvent";
				case InstrumentationType.Abstract:
					return null;
				}
			}
			return ManagedNameAttribute.GetMemberName(type.BaseType);
		}

		// Token: 0x04000260 RID: 608
		private string name;
	}
}
