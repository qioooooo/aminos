using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Management.Instrumentation
{
	// Token: 0x0200008E RID: 142
	[AttributeUsage(AttributeTargets.Assembly)]
	public class InstrumentedAttribute : Attribute
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x0002162B File Offset: 0x0002062B
		public InstrumentedAttribute()
			: this(null, null)
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00021635 File Offset: 0x00020635
		public InstrumentedAttribute(string namespaceName)
			: this(namespaceName, null)
		{
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00021640 File Offset: 0x00020640
		public InstrumentedAttribute(string namespaceName, string securityDescriptor)
		{
			if (namespaceName != null)
			{
				namespaceName = namespaceName.Replace('/', '\\');
			}
			if (namespaceName == null || namespaceName.Length == 0)
			{
				namespaceName = "root\\default";
			}
			bool flag = true;
			foreach (string text in namespaceName.Split(new char[] { '\\' }))
			{
				if (text.Length == 0 || (flag && string.Compare(text, "root", StringComparison.OrdinalIgnoreCase) != 0) || !Regex.Match(text, "^[a-z,A-Z]").Success || Regex.Match(text, "_$").Success || Regex.Match(text, "[^a-z,A-Z,0-9,_,\\u0080-\\uFFFF]").Success)
				{
					ManagementException.ThrowWithExtendedInfo(ManagementStatus.InvalidNamespace);
				}
				flag = false;
			}
			this.namespaceName = namespaceName;
			this.securityDescriptor = securityDescriptor;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0002170A File Offset: 0x0002070A
		public string NamespaceName
		{
			get
			{
				if (this.namespaceName != null)
				{
					return this.namespaceName;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00021720 File Offset: 0x00020720
		public string SecurityDescriptor
		{
			get
			{
				if (this.securityDescriptor == null || this.securityDescriptor.Length == 0)
				{
					return null;
				}
				return this.securityDescriptor;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00021740 File Offset: 0x00020740
		internal static InstrumentedAttribute GetAttribute(Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(typeof(InstrumentedAttribute), false);
			if (customAttributes.Length > 0)
			{
				return (InstrumentedAttribute)customAttributes[0];
			}
			return new InstrumentedAttribute();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00021774 File Offset: 0x00020774
		internal static Type[] GetInstrumentedTypes(Assembly assembly)
		{
			ArrayList arrayList = new ArrayList();
			foreach (Type type in assembly.GetTypes())
			{
				if (InstrumentedAttribute.IsInstrumentationClass(type))
				{
					InstrumentedAttribute.GetInstrumentedParentTypes(arrayList, type);
				}
			}
			return (Type[])arrayList.ToArray(typeof(Type));
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000217C4 File Offset: 0x000207C4
		private static void GetInstrumentedParentTypes(ArrayList types, Type childType)
		{
			if (!types.Contains(childType))
			{
				Type baseInstrumentationType = InstrumentationClassAttribute.GetBaseInstrumentationType(childType);
				if (baseInstrumentationType != null)
				{
					InstrumentedAttribute.GetInstrumentedParentTypes(types, baseInstrumentationType);
				}
				types.Add(childType);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000217F3 File Offset: 0x000207F3
		private static bool IsInstrumentationClass(Type type)
		{
			return null != InstrumentationClassAttribute.GetAttribute(type);
		}

		// Token: 0x04000258 RID: 600
		private string namespaceName;

		// Token: 0x04000259 RID: 601
		private string securityDescriptor;
	}
}
