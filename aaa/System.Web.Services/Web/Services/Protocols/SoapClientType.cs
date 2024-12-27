using System;
using System.Collections;
using System.Reflection;
using System.Web.Services.Configuration;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000060 RID: 96
	internal class SoapClientType
	{
		// Token: 0x0600025C RID: 604 RVA: 0x0000B3D4 File Offset: 0x0000A3D4
		internal SoapClientType(Type type)
		{
			this.binding = WebServiceBindingReflector.GetAttribute(type);
			if (this.binding == null)
			{
				throw new InvalidOperationException(Res.GetString("WebClientBindingAttributeRequired"));
			}
			this.serviceNamespace = this.binding.Namespace;
			this.serviceDefaultIsEncoded = SoapReflector.ServiceDefaultIsEncoded(type);
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			SoapClientType.GenerateXmlMappings(type, arrayList, this.serviceNamespace, this.serviceDefaultIsEncoded, arrayList2);
			XmlMapping[] array = (XmlMapping[])arrayList2.ToArray(typeof(XmlMapping));
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, ".ctor", new object[] { type }) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceCreateSerializer"), traceMethod, new TraceMethod(typeof(XmlSerializer), "FromMappings", new object[] { array, type }));
			}
			XmlSerializer[] array2 = XmlSerializer.FromMappings(array, type);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceCreateSerializer"), traceMethod);
			}
			SoapExtensionTypeElementCollection soapExtensionTypes = WebServicesSection.Current.SoapExtensionTypes;
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			for (int i = 0; i < soapExtensionTypes.Count; i++)
			{
				SoapExtensionTypeElement soapExtensionTypeElement = soapExtensionTypes[i];
				SoapReflectedExtension soapReflectedExtension = new SoapReflectedExtension(soapExtensionTypes[i].Type, null, soapExtensionTypes[i].Priority);
				if (soapExtensionTypes[i].Group == PriorityGroup.High)
				{
					arrayList3.Add(soapReflectedExtension);
				}
				else
				{
					arrayList4.Add(soapReflectedExtension);
				}
			}
			this.HighPriExtensions = (SoapReflectedExtension[])arrayList3.ToArray(typeof(SoapReflectedExtension));
			this.LowPriExtensions = (SoapReflectedExtension[])arrayList4.ToArray(typeof(SoapReflectedExtension));
			Array.Sort<SoapReflectedExtension>(this.HighPriExtensions);
			Array.Sort<SoapReflectedExtension>(this.LowPriExtensions);
			this.HighPriExtensionInitializers = SoapReflectedExtension.GetInitializers(type, this.HighPriExtensions);
			this.LowPriExtensionInitializers = SoapReflectedExtension.GetInitializers(type, this.LowPriExtensions);
			int num = 0;
			for (int j = 0; j < arrayList.Count; j++)
			{
				SoapReflectedMethod soapReflectedMethod = (SoapReflectedMethod)arrayList[j];
				SoapClientMethod soapClientMethod = new SoapClientMethod();
				soapClientMethod.parameterSerializer = array2[num++];
				if (soapReflectedMethod.responseMappings != null)
				{
					soapClientMethod.returnSerializer = array2[num++];
				}
				soapClientMethod.inHeaderSerializer = array2[num++];
				if (soapReflectedMethod.outHeaderMappings != null)
				{
					soapClientMethod.outHeaderSerializer = array2[num++];
				}
				soapClientMethod.action = soapReflectedMethod.action;
				soapClientMethod.oneWay = soapReflectedMethod.oneWay;
				soapClientMethod.rpc = soapReflectedMethod.rpc;
				soapClientMethod.use = soapReflectedMethod.use;
				soapClientMethod.paramStyle = soapReflectedMethod.paramStyle;
				soapClientMethod.methodInfo = soapReflectedMethod.methodInfo;
				soapClientMethod.extensions = soapReflectedMethod.extensions;
				soapClientMethod.extensionInitializers = SoapReflectedExtension.GetInitializers(soapClientMethod.methodInfo, soapReflectedMethod.extensions);
				ArrayList arrayList5 = new ArrayList();
				ArrayList arrayList6 = new ArrayList();
				for (int k = 0; k < soapReflectedMethod.headers.Length; k++)
				{
					SoapHeaderMapping soapHeaderMapping = new SoapHeaderMapping();
					SoapReflectedHeader soapReflectedHeader = soapReflectedMethod.headers[k];
					soapHeaderMapping.memberInfo = soapReflectedHeader.memberInfo;
					soapHeaderMapping.repeats = soapReflectedHeader.repeats;
					soapHeaderMapping.custom = soapReflectedHeader.custom;
					soapHeaderMapping.direction = soapReflectedHeader.direction;
					soapHeaderMapping.headerType = soapReflectedHeader.headerType;
					if ((soapHeaderMapping.direction & SoapHeaderDirection.In) != (SoapHeaderDirection)0)
					{
						arrayList5.Add(soapHeaderMapping);
					}
					if ((soapHeaderMapping.direction & (SoapHeaderDirection.Out | SoapHeaderDirection.Fault)) != (SoapHeaderDirection)0)
					{
						arrayList6.Add(soapHeaderMapping);
					}
				}
				soapClientMethod.inHeaderMappings = (SoapHeaderMapping[])arrayList5.ToArray(typeof(SoapHeaderMapping));
				if (soapClientMethod.outHeaderSerializer != null)
				{
					soapClientMethod.outHeaderMappings = (SoapHeaderMapping[])arrayList6.ToArray(typeof(SoapHeaderMapping));
				}
				this.methods.Add(soapReflectedMethod.name, soapClientMethod);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000B7F0 File Offset: 0x0000A7F0
		internal static void GenerateXmlMappings(Type type, ArrayList soapMethodList, string serviceNamespace, bool serviceDefaultIsEncoded, ArrayList mappings)
		{
			LogicalMethodInfo[] array = LogicalMethodInfo.Create(type.GetMethods(BindingFlags.Instance | BindingFlags.Public), LogicalMethodTypes.Sync);
			SoapReflectionImporter soapReflectionImporter = SoapReflector.CreateSoapImporter(serviceNamespace, serviceDefaultIsEncoded);
			XmlReflectionImporter xmlReflectionImporter = SoapReflector.CreateXmlImporter(serviceNamespace, serviceDefaultIsEncoded);
			WebMethodReflector.IncludeTypes(array, xmlReflectionImporter);
			SoapReflector.IncludeTypes(array, soapReflectionImporter);
			foreach (LogicalMethodInfo logicalMethodInfo in array)
			{
				SoapReflectedMethod soapReflectedMethod = SoapReflector.ReflectMethod(logicalMethodInfo, true, xmlReflectionImporter, soapReflectionImporter, serviceNamespace);
				if (soapReflectedMethod != null)
				{
					soapMethodList.Add(soapReflectedMethod);
					mappings.Add(soapReflectedMethod.requestMappings);
					if (soapReflectedMethod.responseMappings != null)
					{
						mappings.Add(soapReflectedMethod.responseMappings);
					}
					mappings.Add(soapReflectedMethod.inHeaderMappings);
					if (soapReflectedMethod.outHeaderMappings != null)
					{
						mappings.Add(soapReflectedMethod.outHeaderMappings);
					}
				}
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B8A5 File Offset: 0x0000A8A5
		internal SoapClientMethod GetMethod(string name)
		{
			return (SoapClientMethod)this.methods[name];
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000B8B8 File Offset: 0x0000A8B8
		internal WebServiceBindingAttribute Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x040002E0 RID: 736
		private Hashtable methods = new Hashtable();

		// Token: 0x040002E1 RID: 737
		private WebServiceBindingAttribute binding;

		// Token: 0x040002E2 RID: 738
		internal SoapReflectedExtension[] HighPriExtensions;

		// Token: 0x040002E3 RID: 739
		internal SoapReflectedExtension[] LowPriExtensions;

		// Token: 0x040002E4 RID: 740
		internal object[] HighPriExtensionInitializers;

		// Token: 0x040002E5 RID: 741
		internal object[] LowPriExtensionInitializers;

		// Token: 0x040002E6 RID: 742
		internal string serviceNamespace;

		// Token: 0x040002E7 RID: 743
		internal bool serviceDefaultIsEncoded;
	}
}
