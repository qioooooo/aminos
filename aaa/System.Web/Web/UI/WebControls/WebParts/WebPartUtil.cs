using System;
using System.Reflection;
using System.Web.Compilation;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000745 RID: 1861
	internal static class WebPartUtil
	{
		// Token: 0x06005A46 RID: 23110 RVA: 0x0016C481 File Offset: 0x0016B481
		internal static object CreateObjectFromType(Type type)
		{
			return HttpRuntime.FastCreatePublicInstance(type);
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x0016C489 File Offset: 0x0016B489
		internal static Type DeserializeType(string typeName, bool throwOnError)
		{
			return BuildManager.GetType(typeName, throwOnError);
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x0016C494 File Offset: 0x0016B494
		internal static Type[] GetTypesForConstructor(ConstructorInfo constructor)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return array;
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x0016C4CC File Offset: 0x0016B4CC
		internal static bool IsConnectionPointTypeValid(Type connectionPointType, bool isConsumer)
		{
			if (connectionPointType == null)
			{
				return true;
			}
			if (!connectionPointType.IsPublic && !connectionPointType.IsNestedPublic)
			{
				return false;
			}
			Type type = (isConsumer ? typeof(ConsumerConnectionPoint) : typeof(ProviderConnectionPoint));
			if (!connectionPointType.IsSubclassOf(type))
			{
				return false;
			}
			Type[] array = (isConsumer ? ConsumerConnectionPoint.ConstructorTypes : ProviderConnectionPoint.ConstructorTypes);
			return connectionPointType.GetConstructor(array) != null;
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x0016C533 File Offset: 0x0016B533
		internal static string SerializeType(Type type)
		{
			if (type.Assembly.GlobalAssemblyCache)
			{
				return type.AssemblyQualifiedName;
			}
			return type.FullName;
		}
	}
}
