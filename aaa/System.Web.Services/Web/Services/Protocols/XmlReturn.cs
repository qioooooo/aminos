using System;
using System.Collections;
using System.Security.Policy;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000092 RID: 146
	internal class XmlReturn
	{
		// Token: 0x060003CA RID: 970 RVA: 0x00012E03 File Offset: 0x00011E03
		private XmlReturn()
		{
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00012E0C File Offset: 0x00011E0C
		internal static object[] GetInitializers(LogicalMethodInfo[] methodInfos)
		{
			if (methodInfos.Length == 0)
			{
				return new object[0];
			}
			WebServiceAttribute attribute = WebServiceReflector.GetAttribute(methodInfos);
			bool flag = SoapReflector.ServiceDefaultIsEncoded(WebServiceReflector.GetMostDerivedType(methodInfos));
			XmlReflectionImporter xmlReflectionImporter = SoapReflector.CreateXmlImporter(attribute.Namespace, flag);
			WebMethodReflector.IncludeTypes(methodInfos, xmlReflectionImporter);
			ArrayList arrayList = new ArrayList();
			bool[] array = new bool[methodInfos.Length];
			for (int i = 0; i < methodInfos.Length; i++)
			{
				LogicalMethodInfo logicalMethodInfo = methodInfos[i];
				Type returnType = logicalMethodInfo.ReturnType;
				if (XmlReturn.IsSupported(returnType) && HttpServerProtocol.AreUrlParametersSupported(logicalMethodInfo))
				{
					XmlAttributes xmlAttributes = new XmlAttributes(logicalMethodInfo.ReturnTypeCustomAttributeProvider);
					XmlTypeMapping xmlTypeMapping = xmlReflectionImporter.ImportTypeMapping(returnType, xmlAttributes.XmlRoot);
					xmlTypeMapping.SetKey(logicalMethodInfo.GetKey() + ":Return");
					arrayList.Add(xmlTypeMapping);
					array[i] = true;
				}
			}
			if (arrayList.Count == 0)
			{
				return new object[0];
			}
			XmlMapping[] array2 = (XmlMapping[])arrayList.ToArray(typeof(XmlMapping));
			Evidence evidence = methodInfos[0].DeclaringType.Assembly.Evidence;
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(typeof(XmlReturn), "GetInitializers", methodInfos) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceCreateSerializer"), traceMethod, new TraceMethod(typeof(XmlSerializer), "FromMappings", new object[] { array2, evidence }));
			}
			XmlSerializer[] array3 = XmlSerializer.FromMappings(array2, evidence);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceCreateSerializer"), traceMethod);
			}
			object[] array4 = new object[methodInfos.Length];
			int num = 0;
			for (int j = 0; j < array4.Length; j++)
			{
				if (array[j])
				{
					array4[j] = array3[num++];
				}
			}
			return array4;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00012FCD File Offset: 0x00011FCD
		private static bool IsSupported(Type returnType)
		{
			return returnType != typeof(void);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00012FE0 File Offset: 0x00011FE0
		internal static object GetInitializer(LogicalMethodInfo methodInfo)
		{
			return XmlReturn.GetInitializers(new LogicalMethodInfo[] { methodInfo });
		}
	}
}
