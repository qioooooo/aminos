using System;
using System.Web.Services.Protocols;

namespace System.Web.Services
{
	// Token: 0x02000018 RID: 24
	internal class WebServiceBindingReflector
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002C31 File Offset: 0x00001C31
		private WebServiceBindingReflector()
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002C3C File Offset: 0x00001C3C
		internal static WebServiceBindingAttribute GetAttribute(Type type)
		{
			while (type != null)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(WebServiceBindingAttribute), false);
				if (customAttributes.Length != 0)
				{
					if (customAttributes.Length > 1)
					{
						throw new ArgumentException(Res.GetString("OnlyOneWebServiceBindingAttributeMayBeSpecified1", new object[] { type.FullName }), "type");
					}
					return (WebServiceBindingAttribute)customAttributes[0];
				}
				else
				{
					type = type.BaseType;
				}
			}
			return null;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002CA4 File Offset: 0x00001CA4
		internal static WebServiceBindingAttribute GetAttribute(LogicalMethodInfo methodInfo, string binding)
		{
			if (methodInfo.Binding != null)
			{
				if (binding.Length > 0 && methodInfo.Binding.Name != binding)
				{
					throw new InvalidOperationException(Res.GetString("WebInvalidBindingName", new object[]
					{
						binding,
						methodInfo.Binding.Name
					}));
				}
				return methodInfo.Binding;
			}
			else
			{
				Type declaringType = methodInfo.DeclaringType;
				object[] customAttributes = declaringType.GetCustomAttributes(typeof(WebServiceBindingAttribute), false);
				WebServiceBindingAttribute webServiceBindingAttribute = null;
				foreach (WebServiceBindingAttribute webServiceBindingAttribute2 in customAttributes)
				{
					if (webServiceBindingAttribute2.Name == binding)
					{
						if (webServiceBindingAttribute != null)
						{
							throw new ArgumentException(Res.GetString("MultipleBindingsWithSameName2", new object[] { declaringType.FullName, binding, "methodInfo" }));
						}
						webServiceBindingAttribute = webServiceBindingAttribute2;
					}
				}
				if (webServiceBindingAttribute == null && binding != null && binding.Length > 0)
				{
					throw new ArgumentException(Res.GetString("TypeIsMissingWebServiceBindingAttributeThat2", new object[] { declaringType.FullName, binding }), "methodInfo");
				}
				return webServiceBindingAttribute;
			}
		}
	}
}
