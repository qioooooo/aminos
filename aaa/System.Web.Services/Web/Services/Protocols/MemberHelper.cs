using System;
using System.Reflection;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200005A RID: 90
	internal class MemberHelper
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000A145 File Offset: 0x00009145
		private MemberHelper()
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A14D File Offset: 0x0000914D
		internal static void SetValue(MemberInfo memberInfo, object target, object value)
		{
			if (memberInfo is FieldInfo)
			{
				((FieldInfo)memberInfo).SetValue(target, value);
				return;
			}
			((PropertyInfo)memberInfo).SetValue(target, value, MemberHelper.emptyObjectArray);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A177 File Offset: 0x00009177
		internal static object GetValue(MemberInfo memberInfo, object target)
		{
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).GetValue(target);
			}
			return ((PropertyInfo)memberInfo).GetValue(target, MemberHelper.emptyObjectArray);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A19F File Offset: 0x0000919F
		internal static bool IsStatic(MemberInfo memberInfo)
		{
			return memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsStatic;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A1B6 File Offset: 0x000091B6
		internal static bool CanRead(MemberInfo memberInfo)
		{
			return memberInfo is FieldInfo || ((PropertyInfo)memberInfo).CanRead;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A1CD File Offset: 0x000091CD
		internal static bool CanWrite(MemberInfo memberInfo)
		{
			return memberInfo is FieldInfo || ((PropertyInfo)memberInfo).CanWrite;
		}

		// Token: 0x040002D1 RID: 721
		private static object[] emptyObjectArray = new object[0];
	}
}
