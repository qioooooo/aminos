using System;
using System.Collections;
using System.Reflection;

namespace System.Diagnostics
{
	// Token: 0x020001D0 RID: 464
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	public sealed class SwitchAttribute : Attribute
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x0002DD28 File Offset: 0x0002CD28
		public SwitchAttribute(string switchName, Type switchType)
		{
			this.SwitchName = switchName;
			this.SwitchType = switchType;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0002DD3E File Offset: 0x0002CD3E
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0002DD48 File Offset: 0x0002CD48
		public string SwitchName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "value" }), "value");
				}
				this.name = value;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002DD97 File Offset: 0x0002CD97
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0002DD9F File Offset: 0x0002CD9F
		public Type SwitchType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.type = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0002DDB6 File Offset: 0x0002CDB6
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x0002DDBE File Offset: 0x0002CDBE
		public string SwitchDescription
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0002DDC8 File Offset: 0x0002CDC8
		public static SwitchAttribute[] GetAll(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			ArrayList arrayList = new ArrayList();
			object[] customAttributes = assembly.GetCustomAttributes(typeof(SwitchAttribute), false);
			arrayList.AddRange(customAttributes);
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				SwitchAttribute.GetAllRecursive(types[i], arrayList);
			}
			SwitchAttribute[] array = new SwitchAttribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0002DE38 File Offset: 0x0002CE38
		private static void GetAllRecursive(Type type, ArrayList switchAttribs)
		{
			SwitchAttribute.GetAllRecursive(type, switchAttribs);
			MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < members.Length; i++)
			{
				if (!(members[i] is Type))
				{
					SwitchAttribute.GetAllRecursive(members[i], switchAttribs);
				}
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0002DE78 File Offset: 0x0002CE78
		private static void GetAllRecursive(MemberInfo member, ArrayList switchAttribs)
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(SwitchAttribute), false);
			switchAttribs.AddRange(customAttributes);
		}

		// Token: 0x04000F04 RID: 3844
		private Type type;

		// Token: 0x04000F05 RID: 3845
		private string name;

		// Token: 0x04000F06 RID: 3846
		private string description;
	}
}
