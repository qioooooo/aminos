using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web.Services.Description;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000148 RID: 328
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SoapServerMethod
	{
		// Token: 0x06000A42 RID: 2626 RVA: 0x00047DE7 File Offset: 0x00046DE7
		public SoapServerMethod()
		{
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00047DF0 File Offset: 0x00046DF0
		public SoapServerMethod(Type serverType, LogicalMethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;
			WebServiceAttribute attribute = WebServiceReflector.GetAttribute(serverType);
			string @namespace = attribute.Namespace;
			bool flag = SoapReflector.ServiceDefaultIsEncoded(serverType);
			SoapReflectionImporter soapReflectionImporter = SoapReflector.CreateSoapImporter(@namespace, flag);
			XmlReflectionImporter xmlReflectionImporter = SoapReflector.CreateXmlImporter(@namespace, flag);
			SoapReflector.IncludeTypes(methodInfo, soapReflectionImporter);
			WebMethodReflector.IncludeTypes(methodInfo, xmlReflectionImporter);
			SoapReflectedMethod soapReflectedMethod = SoapReflector.ReflectMethod(methodInfo, false, xmlReflectionImporter, soapReflectionImporter, @namespace);
			this.ImportReflectedMethod(soapReflectedMethod);
			this.ImportSerializers(soapReflectedMethod, this.GetServerTypeEvidence(serverType));
			this.ImportHeaderSerializers(soapReflectedMethod);
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00047E6B File Offset: 0x00046E6B
		public LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.methodInfo;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00047E73 File Offset: 0x00046E73
		public XmlSerializer ReturnSerializer
		{
			get
			{
				return this.returnSerializer;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00047E7B File Offset: 0x00046E7B
		public XmlSerializer ParameterSerializer
		{
			get
			{
				return this.parameterSerializer;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00047E83 File Offset: 0x00046E83
		public XmlSerializer InHeaderSerializer
		{
			get
			{
				return this.inHeaderSerializer;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00047E8B File Offset: 0x00046E8B
		public XmlSerializer OutHeaderSerializer
		{
			get
			{
				return this.outHeaderSerializer;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00047E93 File Offset: 0x00046E93
		public SoapHeaderMapping[] InHeaderMappings
		{
			get
			{
				return this.inHeaderMappings;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00047E9B File Offset: 0x00046E9B
		public SoapHeaderMapping[] OutHeaderMappings
		{
			get
			{
				return this.outHeaderMappings;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00047EA3 File Offset: 0x00046EA3
		public string Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00047EAB File Offset: 0x00046EAB
		public bool OneWay
		{
			get
			{
				return this.oneWay;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00047EB3 File Offset: 0x00046EB3
		public bool Rpc
		{
			get
			{
				return this.rpc;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00047EBB File Offset: 0x00046EBB
		public SoapBindingUse BindingUse
		{
			get
			{
				return this.use;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00047EC3 File Offset: 0x00046EC3
		public SoapParameterStyle ParameterStyle
		{
			get
			{
				return this.paramStyle;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00047ECB File Offset: 0x00046ECB
		public WsiProfiles WsiClaims
		{
			get
			{
				return this.wsiClaims;
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00047ED3 File Offset: 0x00046ED3
		[SecurityPermission(SecurityAction.Assert, ControlEvidence = true)]
		private Evidence GetServerTypeEvidence(Type type)
		{
			return type.Assembly.Evidence;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00047EE0 File Offset: 0x00046EE0
		private List<XmlMapping> GetXmlMappingsForMethod(SoapReflectedMethod soapMethod)
		{
			List<XmlMapping> list = new List<XmlMapping>();
			list.Add(soapMethod.requestMappings);
			if (soapMethod.responseMappings != null)
			{
				list.Add(soapMethod.responseMappings);
			}
			list.Add(soapMethod.inHeaderMappings);
			if (soapMethod.outHeaderMappings != null)
			{
				list.Add(soapMethod.outHeaderMappings);
			}
			return list;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00047F34 File Offset: 0x00046F34
		private void ImportReflectedMethod(SoapReflectedMethod soapMethod)
		{
			this.action = soapMethod.action;
			this.extensions = soapMethod.extensions;
			this.extensionInitializers = SoapReflectedExtension.GetInitializers(this.methodInfo, soapMethod.extensions);
			this.oneWay = soapMethod.oneWay;
			this.rpc = soapMethod.rpc;
			this.use = soapMethod.use;
			this.paramStyle = soapMethod.paramStyle;
			this.wsiClaims = ((soapMethod.binding == null) ? WsiProfiles.None : soapMethod.binding.ConformsTo);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00047FBC File Offset: 0x00046FBC
		private void ImportHeaderSerializers(SoapReflectedMethod soapMethod)
		{
			List<SoapHeaderMapping> list = new List<SoapHeaderMapping>();
			List<SoapHeaderMapping> list2 = new List<SoapHeaderMapping>();
			for (int i = 0; i < soapMethod.headers.Length; i++)
			{
				SoapHeaderMapping soapHeaderMapping = new SoapHeaderMapping();
				SoapReflectedHeader soapReflectedHeader = soapMethod.headers[i];
				soapHeaderMapping.memberInfo = soapReflectedHeader.memberInfo;
				soapHeaderMapping.repeats = soapReflectedHeader.repeats;
				soapHeaderMapping.custom = soapReflectedHeader.custom;
				soapHeaderMapping.direction = soapReflectedHeader.direction;
				soapHeaderMapping.headerType = soapReflectedHeader.headerType;
				if (soapHeaderMapping.direction == SoapHeaderDirection.In)
				{
					list.Add(soapHeaderMapping);
				}
				else if (soapHeaderMapping.direction == SoapHeaderDirection.Out)
				{
					list2.Add(soapHeaderMapping);
				}
				else
				{
					list.Add(soapHeaderMapping);
					list2.Add(soapHeaderMapping);
				}
			}
			this.inHeaderMappings = list.ToArray();
			if (this.outHeaderSerializer != null)
			{
				this.outHeaderMappings = list2.ToArray();
			}
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00048094 File Offset: 0x00047094
		private void ImportSerializers(SoapReflectedMethod soapMethod, Evidence serverEvidence)
		{
			List<XmlMapping> xmlMappingsForMethod = this.GetXmlMappingsForMethod(soapMethod);
			XmlMapping[] array = xmlMappingsForMethod.ToArray();
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ImportSerializers", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceCreateSerializer"), traceMethod, new TraceMethod(typeof(XmlSerializer), "FromMappings", new object[] { array, serverEvidence }));
			}
			XmlSerializer[] array2 = XmlSerializer.FromMappings(array, serverEvidence);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceCreateSerializer"), traceMethod);
			}
			int num = 0;
			this.parameterSerializer = array2[num++];
			if (soapMethod.responseMappings != null)
			{
				this.returnSerializer = array2[num++];
			}
			this.inHeaderSerializer = array2[num++];
			if (soapMethod.outHeaderMappings != null)
			{
				this.outHeaderSerializer = array2[num++];
			}
		}

		// Token: 0x0400064D RID: 1613
		internal LogicalMethodInfo methodInfo;

		// Token: 0x0400064E RID: 1614
		internal XmlSerializer returnSerializer;

		// Token: 0x0400064F RID: 1615
		internal XmlSerializer parameterSerializer;

		// Token: 0x04000650 RID: 1616
		internal XmlSerializer inHeaderSerializer;

		// Token: 0x04000651 RID: 1617
		internal XmlSerializer outHeaderSerializer;

		// Token: 0x04000652 RID: 1618
		internal SoapHeaderMapping[] inHeaderMappings;

		// Token: 0x04000653 RID: 1619
		internal SoapHeaderMapping[] outHeaderMappings;

		// Token: 0x04000654 RID: 1620
		internal SoapReflectedExtension[] extensions;

		// Token: 0x04000655 RID: 1621
		internal object[] extensionInitializers;

		// Token: 0x04000656 RID: 1622
		internal string action;

		// Token: 0x04000657 RID: 1623
		internal bool oneWay;

		// Token: 0x04000658 RID: 1624
		internal bool rpc;

		// Token: 0x04000659 RID: 1625
		internal SoapBindingUse use;

		// Token: 0x0400065A RID: 1626
		internal SoapParameterStyle paramStyle;

		// Token: 0x0400065B RID: 1627
		internal WsiProfiles wsiClaims;
	}
}
