using System;
using System.Collections;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services
{
	// Token: 0x02000012 RID: 18
	internal class WebMethodReflector
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002553 File Offset: 0x00001553
		private WebMethodReflector()
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000255C File Offset: 0x0000155C
		internal static WebMethodAttribute GetAttribute(MethodInfo implementation, MethodInfo declaration)
		{
			WebMethodAttribute webMethodAttribute = null;
			WebMethodAttribute webMethodAttribute2 = null;
			object[] array;
			if (declaration != null)
			{
				array = declaration.GetCustomAttributes(typeof(WebMethodAttribute), false);
				if (array.Length > 0)
				{
					webMethodAttribute = (WebMethodAttribute)array[0];
				}
			}
			array = implementation.GetCustomAttributes(typeof(WebMethodAttribute), false);
			if (array.Length > 0)
			{
				webMethodAttribute2 = (WebMethodAttribute)array[0];
			}
			if (webMethodAttribute == null)
			{
				return webMethodAttribute2;
			}
			if (webMethodAttribute2 == null)
			{
				return webMethodAttribute;
			}
			if (webMethodAttribute2.MessageNameSpecified)
			{
				throw new InvalidOperationException(Res.GetString("ContractOverride", new object[]
				{
					implementation.Name,
					implementation.DeclaringType.FullName,
					declaration.DeclaringType.FullName,
					declaration.ToString(),
					"WebMethod.MessageName"
				}));
			}
			return new WebMethodAttribute(webMethodAttribute2.EnableSessionSpecified ? webMethodAttribute2.EnableSession : webMethodAttribute.EnableSession)
			{
				TransactionOption = (webMethodAttribute2.TransactionOptionSpecified ? webMethodAttribute2.TransactionOption : webMethodAttribute.TransactionOption),
				CacheDuration = (webMethodAttribute2.CacheDurationSpecified ? webMethodAttribute2.CacheDuration : webMethodAttribute.CacheDuration),
				BufferResponse = (webMethodAttribute2.BufferResponseSpecified ? webMethodAttribute2.BufferResponse : webMethodAttribute.BufferResponse),
				Description = (webMethodAttribute2.DescriptionSpecified ? webMethodAttribute2.Description : webMethodAttribute.Description)
			};
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000026A8 File Offset: 0x000016A8
		internal static MethodInfo FindInterfaceMethodInfo(Type type, string signature)
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				InterfaceMapping interfaceMap = type.GetInterfaceMap(type2);
				MethodInfo[] targetMethods = interfaceMap.TargetMethods;
				for (int j = 0; j < targetMethods.Length; j++)
				{
					if (targetMethods[j].ToString() == signature)
					{
						return interfaceMap.InterfaceMethods[j];
					}
				}
			}
			return null;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002720 File Offset: 0x00001720
		internal static LogicalMethodInfo[] GetMethods(Type type)
		{
			if (type.IsInterface)
			{
				throw new InvalidOperationException(Res.GetString("NeedConcreteType", new object[] { type.FullName }));
			}
			ArrayList arrayList = new ArrayList();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			for (int i = 0; i < methods.Length; i++)
			{
				Type declaringType = methods[i].DeclaringType;
				if (declaringType != typeof(object) && declaringType != typeof(WebService))
				{
					string text = methods[i].ToString();
					MethodInfo methodInfo = WebMethodReflector.FindInterfaceMethodInfo(declaringType, text);
					WebServiceBindingAttribute webServiceBindingAttribute = null;
					if (methodInfo != null)
					{
						object[] customAttributes = methodInfo.DeclaringType.GetCustomAttributes(typeof(WebServiceBindingAttribute), false);
						if (customAttributes.Length > 0)
						{
							if (customAttributes.Length > 1)
							{
								throw new ArgumentException(Res.GetString("OnlyOneWebServiceBindingAttributeMayBeSpecified1", new object[] { methodInfo.DeclaringType.FullName }), "type");
							}
							webServiceBindingAttribute = (WebServiceBindingAttribute)customAttributes[0];
							if (webServiceBindingAttribute.Name == null || webServiceBindingAttribute.Name.Length == 0)
							{
								webServiceBindingAttribute.Name = methodInfo.DeclaringType.Name;
							}
						}
						else
						{
							methodInfo = null;
						}
					}
					else if (!methods[i].IsPublic)
					{
						goto IL_01C7;
					}
					WebMethodAttribute attribute = WebMethodReflector.GetAttribute(methods[i], methodInfo);
					if (attribute != null)
					{
						WebMethod webMethod = new WebMethod(methodInfo, webServiceBindingAttribute, attribute);
						hashtable2.Add(methods[i], webMethod);
						MethodInfo methodInfo2 = (MethodInfo)hashtable[text];
						if (methodInfo2 == null)
						{
							hashtable.Add(text, methods[i]);
							arrayList.Add(methods[i]);
						}
						else if (methodInfo2.DeclaringType.IsAssignableFrom(methods[i].DeclaringType))
						{
							hashtable[text] = methods[i];
							arrayList[arrayList.IndexOf(methodInfo2)] = methods[i];
						}
					}
				}
				IL_01C7:;
			}
			return LogicalMethodInfo.Create((MethodInfo[])arrayList.ToArray(typeof(MethodInfo)), (LogicalMethodTypes)3, hashtable2);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002920 File Offset: 0x00001920
		internal static void IncludeTypes(LogicalMethodInfo[] methods, XmlReflectionImporter importer)
		{
			foreach (LogicalMethodInfo logicalMethodInfo in methods)
			{
				WebMethodReflector.IncludeTypes(logicalMethodInfo, importer);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002946 File Offset: 0x00001946
		internal static void IncludeTypes(LogicalMethodInfo method, XmlReflectionImporter importer)
		{
			if (method.Declaration != null)
			{
				importer.IncludeTypes(method.Declaration.DeclaringType);
				importer.IncludeTypes(method.Declaration);
			}
			importer.IncludeTypes(method.DeclaringType);
			importer.IncludeTypes(method.CustomAttributeProvider);
		}
	}
}
