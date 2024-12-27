using System;
using System.Reflection;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200003A RID: 58
	internal static class RegistrationHelper
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x00009A70 File Offset: 0x00008A70
		public static void RegisterType(string machineAndAppName, Type type, string uri)
		{
			RemotingConfiguration.RegisterWellKnownServiceType(type, uri, WellKnownObjectMode.SingleCall);
			Type[] types = type.Assembly.GetTypes();
			foreach (Type type2 in types)
			{
				RegistrationHelper.RegisterSingleType(machineAndAppName, type2);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009AAC File Offset: 0x00008AAC
		private static void RegisterSingleType(string machineAndAppName, Type type)
		{
			string name = type.Name;
			string text = "http://" + machineAndAppName + "/" + type.FullName;
			SoapServices.RegisterInteropXmlElement(name, text, type);
			SoapServices.RegisterInteropXmlType(name, text, type);
			if (typeof(MarshalByRefObject).IsAssignableFrom(type))
			{
				MethodInfo[] methods = type.GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					SoapServices.RegisterSoapActionForMethodBase(methodInfo, text + "#" + methodInfo.Name);
				}
			}
		}
	}
}
