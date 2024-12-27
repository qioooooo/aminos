using System;
using System.Web.Services.Protocols;

namespace System.Web.Services
{
	// Token: 0x02000016 RID: 22
	internal class WebServiceReflector
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002AC1 File Offset: 0x00001AC1
		private WebServiceReflector()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002ACC File Offset: 0x00001ACC
		internal static WebServiceAttribute GetAttribute(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(WebServiceAttribute), false);
			if (customAttributes.Length == 0)
			{
				return new WebServiceAttribute();
			}
			return (WebServiceAttribute)customAttributes[0];
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002B00 File Offset: 0x00001B00
		internal static WebServiceAttribute GetAttribute(LogicalMethodInfo[] methodInfos)
		{
			if (methodInfos.Length == 0)
			{
				return new WebServiceAttribute();
			}
			Type mostDerivedType = WebServiceReflector.GetMostDerivedType(methodInfos);
			return WebServiceReflector.GetAttribute(mostDerivedType);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002B28 File Offset: 0x00001B28
		internal static Type GetMostDerivedType(LogicalMethodInfo[] methodInfos)
		{
			if (methodInfos.Length == 0)
			{
				return null;
			}
			Type type = methodInfos[0].DeclaringType;
			for (int i = 1; i < methodInfos.Length; i++)
			{
				Type declaringType = methodInfos[i].DeclaringType;
				if (declaringType.IsSubclassOf(type))
				{
					type = declaringType;
				}
			}
			return type;
		}
	}
}
