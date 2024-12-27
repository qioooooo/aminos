using System;
using System.Reflection;
using System.Text;

namespace System.Runtime.Remoting
{
	// Token: 0x02000761 RID: 1889
	internal static class XmlNamespaceEncoder
	{
		// Token: 0x060043DC RID: 17372 RVA: 0x000E9368 File Offset: 0x000E8368
		internal static string GetXmlNamespaceForType(Type type, string dynamicUrl)
		{
			string fullName = type.FullName;
			Assembly assembly = type.Module.Assembly;
			StringBuilder stringBuilder = new StringBuilder(256);
			Assembly assembly2 = typeof(string).Module.Assembly;
			if (assembly == assembly2)
			{
				stringBuilder.Append(SoapServices.namespaceNS);
				stringBuilder.Append(fullName);
			}
			else
			{
				stringBuilder.Append(SoapServices.fullNS);
				stringBuilder.Append(fullName);
				stringBuilder.Append('/');
				stringBuilder.Append(assembly.nGetSimpleName());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x000E93F4 File Offset: 0x000E83F4
		internal static string GetXmlNamespaceForTypeNamespace(Type type, string dynamicUrl)
		{
			string @namespace = type.Namespace;
			Assembly assembly = type.Module.Assembly;
			StringBuilder stringBuilder = new StringBuilder(256);
			Assembly assembly2 = typeof(string).Module.Assembly;
			if (assembly == assembly2)
			{
				stringBuilder.Append(SoapServices.namespaceNS);
				stringBuilder.Append(@namespace);
			}
			else
			{
				stringBuilder.Append(SoapServices.fullNS);
				stringBuilder.Append(@namespace);
				stringBuilder.Append('/');
				stringBuilder.Append(assembly.nGetSimpleName());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x000E9480 File Offset: 0x000E8480
		internal static string GetTypeNameForSoapActionNamespace(string uri, out bool assemblyIncluded)
		{
			assemblyIncluded = false;
			string fullNS = SoapServices.fullNS;
			string namespaceNS = SoapServices.namespaceNS;
			if (uri.StartsWith(fullNS, StringComparison.Ordinal))
			{
				uri = uri.Substring(fullNS.Length);
				char[] array = new char[] { '/' };
				string[] array2 = uri.Split(array);
				if (array2.Length != 2)
				{
					return null;
				}
				assemblyIncluded = true;
				return array2[0] + ", " + array2[1];
			}
			else
			{
				if (uri.StartsWith(namespaceNS, StringComparison.Ordinal))
				{
					string text = typeof(string).Module.Assembly.nGetSimpleName();
					assemblyIncluded = true;
					return uri.Substring(namespaceNS.Length) + ", " + text;
				}
				return null;
			}
		}
	}
}
