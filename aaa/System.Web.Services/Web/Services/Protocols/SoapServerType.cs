using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Services.Configuration;
using System.Web.Services.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000080 RID: 128
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SoapServerType : ServerType
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000F9BD File Offset: 0x0000E9BD
		public string ServiceNamespace
		{
			get
			{
				return this.serviceNamespace;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000F9C5 File Offset: 0x0000E9C5
		public bool ServiceDefaultIsEncoded
		{
			get
			{
				return this.serviceDefaultIsEncoded;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000F9CD File Offset: 0x0000E9CD
		public bool ServiceRoutingOnSoapAction
		{
			get
			{
				return this.routingOnSoapAction;
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000F9D8 File Offset: 0x0000E9D8
		public SoapServerType(Type type, WebServiceProtocols protocolsSupported)
			: base(type)
		{
			this.protocolsSupported = protocolsSupported;
			bool flag = (protocolsSupported & WebServiceProtocols.HttpSoap) != WebServiceProtocols.Unknown;
			LogicalMethodInfo[] array = WebMethodReflector.GetMethods(type);
			ArrayList arrayList = new ArrayList();
			WebServiceAttribute attribute = WebServiceReflector.GetAttribute(type);
			object soapServiceAttribute = SoapReflector.GetSoapServiceAttribute(type);
			this.routingOnSoapAction = SoapReflector.GetSoapServiceRoutingStyle(soapServiceAttribute) == SoapServiceRoutingStyle.SoapAction;
			this.serviceNamespace = attribute.Namespace;
			this.serviceDefaultIsEncoded = SoapReflector.ServiceDefaultIsEncoded(type);
			SoapReflectionImporter soapReflectionImporter = SoapReflector.CreateSoapImporter(this.serviceNamespace, this.serviceDefaultIsEncoded);
			XmlReflectionImporter xmlReflectionImporter = SoapReflector.CreateXmlImporter(this.serviceNamespace, this.serviceDefaultIsEncoded);
			SoapReflector.IncludeTypes(array, soapReflectionImporter);
			WebMethodReflector.IncludeTypes(array, xmlReflectionImporter);
			SoapReflectedMethod[] array2 = new SoapReflectedMethod[array.Length];
			SoapExtensionTypeElementCollection soapExtensionTypes = WebServicesSection.Current.SoapExtensionTypes;
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			for (int i = 0; i < soapExtensionTypes.Count; i++)
			{
				SoapExtensionTypeElement soapExtensionTypeElement = soapExtensionTypes[i];
				if (soapExtensionTypeElement != null)
				{
					SoapReflectedExtension soapReflectedExtension = new SoapReflectedExtension(soapExtensionTypeElement.Type, null, soapExtensionTypeElement.Priority);
					if (soapExtensionTypeElement.Group == PriorityGroup.High)
					{
						arrayList2.Add(soapReflectedExtension);
					}
					else
					{
						arrayList3.Add(soapReflectedExtension);
					}
				}
			}
			this.HighPriExtensions = (SoapReflectedExtension[])arrayList2.ToArray(typeof(SoapReflectedExtension));
			this.LowPriExtensions = (SoapReflectedExtension[])arrayList3.ToArray(typeof(SoapReflectedExtension));
			Array.Sort<SoapReflectedExtension>(this.HighPriExtensions);
			Array.Sort<SoapReflectedExtension>(this.LowPriExtensions);
			this.HighPriExtensionInitializers = SoapReflectedExtension.GetInitializers(type, this.HighPriExtensions);
			this.LowPriExtensionInitializers = SoapReflectedExtension.GetInitializers(type, this.LowPriExtensions);
			for (int j = 0; j < array.Length; j++)
			{
				LogicalMethodInfo logicalMethodInfo = array[j];
				SoapReflectedMethod soapReflectedMethod = SoapReflector.ReflectMethod(logicalMethodInfo, false, xmlReflectionImporter, soapReflectionImporter, attribute.Namespace);
				arrayList.Add(soapReflectedMethod.requestMappings);
				if (soapReflectedMethod.responseMappings != null)
				{
					arrayList.Add(soapReflectedMethod.responseMappings);
				}
				arrayList.Add(soapReflectedMethod.inHeaderMappings);
				if (soapReflectedMethod.outHeaderMappings != null)
				{
					arrayList.Add(soapReflectedMethod.outHeaderMappings);
				}
				array2[j] = soapReflectedMethod;
			}
			XmlMapping[] array3 = (XmlMapping[])arrayList.ToArray(typeof(XmlMapping));
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, ".ctor", new object[] { type, protocolsSupported }) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceCreateSerializer"), traceMethod, new TraceMethod(typeof(XmlSerializer), "FromMappings", new object[] { array3, base.Evidence }));
			}
			XmlSerializer[] array4 = XmlSerializer.FromMappings(array3, base.Evidence);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceCreateSerializer"), traceMethod);
			}
			int num = 0;
			for (int k = 0; k < array2.Length; k++)
			{
				SoapServerMethod soapServerMethod = new SoapServerMethod();
				SoapReflectedMethod soapReflectedMethod2 = array2[k];
				soapServerMethod.parameterSerializer = array4[num++];
				if (soapReflectedMethod2.responseMappings != null)
				{
					soapServerMethod.returnSerializer = array4[num++];
				}
				soapServerMethod.inHeaderSerializer = array4[num++];
				if (soapReflectedMethod2.outHeaderMappings != null)
				{
					soapServerMethod.outHeaderSerializer = array4[num++];
				}
				soapServerMethod.methodInfo = soapReflectedMethod2.methodInfo;
				soapServerMethod.action = soapReflectedMethod2.action;
				soapServerMethod.extensions = soapReflectedMethod2.extensions;
				soapServerMethod.extensionInitializers = SoapReflectedExtension.GetInitializers(soapServerMethod.methodInfo, soapReflectedMethod2.extensions);
				soapServerMethod.oneWay = soapReflectedMethod2.oneWay;
				soapServerMethod.rpc = soapReflectedMethod2.rpc;
				soapServerMethod.use = soapReflectedMethod2.use;
				soapServerMethod.paramStyle = soapReflectedMethod2.paramStyle;
				soapServerMethod.wsiClaims = ((soapReflectedMethod2.binding == null) ? WsiProfiles.None : soapReflectedMethod2.binding.ConformsTo);
				ArrayList arrayList4 = new ArrayList();
				ArrayList arrayList5 = new ArrayList();
				for (int l = 0; l < soapReflectedMethod2.headers.Length; l++)
				{
					SoapHeaderMapping soapHeaderMapping = new SoapHeaderMapping();
					SoapReflectedHeader soapReflectedHeader = soapReflectedMethod2.headers[l];
					soapHeaderMapping.memberInfo = soapReflectedHeader.memberInfo;
					soapHeaderMapping.repeats = soapReflectedHeader.repeats;
					soapHeaderMapping.custom = soapReflectedHeader.custom;
					soapHeaderMapping.direction = soapReflectedHeader.direction;
					soapHeaderMapping.headerType = soapReflectedHeader.headerType;
					if (soapHeaderMapping.direction == SoapHeaderDirection.In)
					{
						arrayList4.Add(soapHeaderMapping);
					}
					else if (soapHeaderMapping.direction == SoapHeaderDirection.Out)
					{
						arrayList5.Add(soapHeaderMapping);
					}
					else
					{
						arrayList4.Add(soapHeaderMapping);
						arrayList5.Add(soapHeaderMapping);
					}
				}
				soapServerMethod.inHeaderMappings = (SoapHeaderMapping[])arrayList4.ToArray(typeof(SoapHeaderMapping));
				if (soapServerMethod.outHeaderSerializer != null)
				{
					soapServerMethod.outHeaderMappings = (SoapHeaderMapping[])arrayList5.ToArray(typeof(SoapHeaderMapping));
				}
				if (flag && !this.routingOnSoapAction && soapReflectedMethod2.requestElementName.IsEmpty)
				{
					throw new SoapException(Res.GetString("TheMethodDoesNotHaveARequestElementEither1", new object[] { soapServerMethod.methodInfo.Name }), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
				}
				if (this.methods[soapReflectedMethod2.action] == null)
				{
					this.methods[soapReflectedMethod2.action] = soapServerMethod;
				}
				else
				{
					if (flag && this.routingOnSoapAction)
					{
						SoapServerMethod soapServerMethod2 = (SoapServerMethod)this.methods[soapReflectedMethod2.action];
						throw new SoapException(Res.GetString("TheMethodsAndUseTheSameSoapActionWhenTheService3", new object[]
						{
							soapServerMethod.methodInfo.Name,
							soapServerMethod2.methodInfo.Name,
							soapReflectedMethod2.action
						}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
					}
					this.duplicateMethods[soapReflectedMethod2.action] = soapServerMethod;
				}
				if (this.methods[soapReflectedMethod2.requestElementName] == null)
				{
					this.methods[soapReflectedMethod2.requestElementName] = soapServerMethod;
				}
				else
				{
					if (flag && !this.routingOnSoapAction)
					{
						SoapServerMethod soapServerMethod3 = (SoapServerMethod)this.methods[soapReflectedMethod2.requestElementName];
						throw new SoapException(Res.GetString("TheMethodsAndUseTheSameRequestElementXmlns4", new object[]
						{
							soapServerMethod.methodInfo.Name,
							soapServerMethod3.methodInfo.Name,
							soapReflectedMethod2.requestElementName.Name,
							soapReflectedMethod2.requestElementName.Namespace
						}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
					}
					this.duplicateMethods[soapReflectedMethod2.requestElementName] = soapServerMethod;
				}
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000100B0 File Offset: 0x0000F0B0
		public SoapServerMethod GetMethod(object key)
		{
			return (SoapServerMethod)this.methods[key];
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000100C3 File Offset: 0x0000F0C3
		public SoapServerMethod GetDuplicateMethod(object key)
		{
			return (SoapServerMethod)this.duplicateMethods[key];
		}

		// Token: 0x04000374 RID: 884
		private Hashtable methods = new Hashtable();

		// Token: 0x04000375 RID: 885
		private Hashtable duplicateMethods = new Hashtable();

		// Token: 0x04000376 RID: 886
		internal SoapReflectedExtension[] HighPriExtensions;

		// Token: 0x04000377 RID: 887
		internal SoapReflectedExtension[] LowPriExtensions;

		// Token: 0x04000378 RID: 888
		internal object[] HighPriExtensionInitializers;

		// Token: 0x04000379 RID: 889
		internal object[] LowPriExtensionInitializers;

		// Token: 0x0400037A RID: 890
		internal string serviceNamespace;

		// Token: 0x0400037B RID: 891
		internal bool serviceDefaultIsEncoded;

		// Token: 0x0400037C RID: 892
		internal bool routingOnSoapAction;

		// Token: 0x0400037D RID: 893
		internal WebServiceProtocols protocolsSupported;
	}
}
