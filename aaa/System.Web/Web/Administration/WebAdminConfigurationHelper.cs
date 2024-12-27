using System;
using System.Globalization;
using System.Reflection;
using System.Web.Hosting;

namespace System.Web.Administration
{
	// Token: 0x020000F4 RID: 244
	[Serializable]
	internal sealed class WebAdminConfigurationHelper : MarshalByRefObject, IRegisteredObject
	{
		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002E4E0 File Offset: 0x0002D4E0
		public WebAdminConfigurationHelper()
		{
			HostingEnvironment.RegisterObject(this);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002E4EE File Offset: 0x0002D4EE
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002E4F1 File Offset: 0x0002D4F1
		public VirtualDirectory GetVirtualDirectory(string path)
		{
			if (HttpRuntime.NamedPermissionSet != null)
			{
				HttpRuntime.NamedPermissionSet.PermitOnly();
			}
			return HostingEnvironment.VirtualPathProvider.GetDirectory(path);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002E510 File Offset: 0x0002D510
		public object CallMembershipProviderMethod(string methodName, object[] parameters, Type[] paramTypes)
		{
			Type type = typeof(HttpContext).Assembly.GetType("System.Web.Security.Membership");
			object obj = null;
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			MethodInfo methodInfo;
			if (paramTypes != null)
			{
				methodInfo = type.GetMethod(methodName, bindingFlags, null, paramTypes, null);
			}
			else
			{
				methodInfo = type.GetMethod(methodName, bindingFlags);
			}
			if (methodInfo != null)
			{
				if (HttpRuntime.NamedPermissionSet != null)
				{
					HttpRuntime.NamedPermissionSet.PermitOnly();
				}
				obj = methodInfo.Invoke(null, parameters);
			}
			object[] array = new object[parameters.Length + 1];
			array[0] = obj;
			int num = 1;
			for (int i = 0; i < parameters.Length; i++)
			{
				array[num++] = parameters[i];
			}
			return array;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002E5B0 File Offset: 0x0002D5B0
		public object GetMembershipProviderProperty(string propertyName)
		{
			Type type = typeof(HttpContext).Assembly.GetType("System.Web.Security.Membership");
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;
			if (HttpRuntime.NamedPermissionSet != null)
			{
				HttpRuntime.NamedPermissionSet.PermitOnly();
			}
			return type.InvokeMember(propertyName, bindingFlags, null, null, null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002E604 File Offset: 0x0002D604
		public object CallRoleProviderMethod(string methodName, object[] parameters, Type[] paramTypes)
		{
			Type type = typeof(HttpContext).Assembly.GetType("System.Web.Security.Roles");
			object obj = null;
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			MethodInfo methodInfo;
			if (paramTypes != null)
			{
				methodInfo = type.GetMethod(methodName, bindingFlags, null, paramTypes, null);
			}
			else
			{
				methodInfo = type.GetMethod(methodName, bindingFlags);
			}
			if (methodInfo != null)
			{
				if (HttpRuntime.NamedPermissionSet != null)
				{
					HttpRuntime.NamedPermissionSet.PermitOnly();
				}
				obj = methodInfo.Invoke(null, parameters);
			}
			object[] array = new object[parameters.Length + 1];
			array[0] = obj;
			int num = 1;
			for (int i = 0; i < parameters.Length; i++)
			{
				array[num++] = parameters[i];
			}
			return array;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002E6A1 File Offset: 0x0002D6A1
		void IRegisteredObject.Stop(bool immediate)
		{
			HostingEnvironment.UnregisterObject(this);
		}
	}
}
