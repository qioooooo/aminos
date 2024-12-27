using System;
using System.Collections;
using System.Reflection;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000118 RID: 280
	internal class SoapProtocolReflector : ProtocolReflector
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0003FFD9 File Offset: 0x0003EFD9
		internal override WsiProfiles ConformsTo
		{
			get
			{
				return WsiProfiles.BasicProfile1_1;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0003FFDC File Offset: 0x0003EFDC
		public override string ProtocolName
		{
			get
			{
				return "Soap";
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0003FFE3 File Offset: 0x0003EFE3
		internal SoapReflectedMethod SoapMethod
		{
			get
			{
				return this.soapMethod;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0003FFEC File Offset: 0x0003EFEC
		internal SoapReflectionImporter SoapImporter
		{
			get
			{
				SoapReflectionImporter soapReflectionImporter = base.ReflectionContext[typeof(SoapReflectionImporter)] as SoapReflectionImporter;
				if (soapReflectionImporter == null)
				{
					soapReflectionImporter = SoapReflector.CreateSoapImporter(base.DefaultNamespace, SoapReflector.ServiceDefaultIsEncoded(base.ServiceType));
					base.ReflectionContext[typeof(SoapReflectionImporter)] = soapReflectionImporter;
				}
				return soapReflectionImporter;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00040048 File Offset: 0x0003F048
		internal SoapSchemaExporter SoapExporter
		{
			get
			{
				SoapSchemaExporter soapSchemaExporter = base.ReflectionContext[typeof(SoapSchemaExporter)] as SoapSchemaExporter;
				if (soapSchemaExporter == null)
				{
					soapSchemaExporter = new SoapSchemaExporter(base.ServiceDescription.Types.Schemas);
					base.ReflectionContext[typeof(SoapSchemaExporter)] = soapSchemaExporter;
				}
				return soapSchemaExporter;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000400A0 File Offset: 0x0003F0A0
		protected override bool ReflectMethod()
		{
			this.soapMethod = base.ReflectionContext[base.Method] as SoapReflectedMethod;
			if (this.soapMethod == null)
			{
				this.soapMethod = SoapReflector.ReflectMethod(base.Method, false, base.ReflectionImporter, this.SoapImporter, base.DefaultNamespace);
				base.ReflectionContext[base.Method] = this.soapMethod;
				this.soapMethod.portType = ((base.Binding != null) ? base.Binding.Type : null);
			}
			WebMethodAttribute methodAttribute = base.Method.MethodAttribute;
			base.OperationBinding.Extensions.Add(this.CreateSoapOperationBinding(this.soapMethod.rpc ? SoapBindingStyle.Rpc : SoapBindingStyle.Document, this.soapMethod.action));
			this.CreateMessage(this.soapMethod.rpc, this.soapMethod.use, this.soapMethod.paramStyle, base.InputMessage, base.OperationBinding.Input, this.soapMethod.requestMappings);
			if (!this.soapMethod.oneWay)
			{
				this.CreateMessage(this.soapMethod.rpc, this.soapMethod.use, this.soapMethod.paramStyle, base.OutputMessage, base.OperationBinding.Output, this.soapMethod.responseMappings);
			}
			this.CreateHeaderMessages(this.soapMethod.name, this.soapMethod.use, this.soapMethod.inHeaderMappings, this.soapMethod.outHeaderMappings, this.soapMethod.headers, this.soapMethod.rpc);
			if (this.soapMethod.rpc && this.soapMethod.use == SoapBindingUse.Encoded && this.soapMethod.methodInfo.OutParameters.Length > 0)
			{
				base.Operation.ParameterOrder = SoapProtocolReflector.GetParameterOrder(this.soapMethod.methodInfo);
			}
			this.AllowExtensionsToReflectMethod();
			return true;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00040299 File Offset: 0x0003F299
		protected override void ReflectDescription()
		{
			this.AllowExtensionsToReflectDescription();
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000402A4 File Offset: 0x0003F2A4
		private void CreateHeaderMessages(string methodName, SoapBindingUse use, XmlMembersMapping inHeaderMappings, XmlMembersMapping outHeaderMappings, SoapReflectedHeader[] headers, bool rpc)
		{
			if (use == SoapBindingUse.Encoded)
			{
				this.SoapExporter.ExportMembersMapping(inHeaderMappings, false);
				if (outHeaderMappings != null)
				{
					this.SoapExporter.ExportMembersMapping(outHeaderMappings, false);
				}
			}
			else
			{
				base.SchemaExporter.ExportMembersMapping(inHeaderMappings);
				if (outHeaderMappings != null)
				{
					base.SchemaExporter.ExportMembersMapping(outHeaderMappings);
				}
			}
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			int num = 0;
			int num2 = 0;
			foreach (SoapReflectedHeader soapReflectedHeader in headers)
			{
				if (soapReflectedHeader.custom)
				{
					XmlMemberMapping xmlMemberMapping;
					if ((soapReflectedHeader.direction & SoapHeaderDirection.In) != (SoapHeaderDirection)0)
					{
						xmlMemberMapping = inHeaderMappings[num++];
						if (soapReflectedHeader.direction != SoapHeaderDirection.In)
						{
							num2++;
						}
					}
					else
					{
						xmlMemberMapping = outHeaderMappings[num2++];
					}
					MessagePart messagePart = new MessagePart();
					messagePart.Name = xmlMemberMapping.XsdElementName;
					if (use == SoapBindingUse.Encoded)
					{
						messagePart.Type = new XmlQualifiedName(xmlMemberMapping.TypeName, xmlMemberMapping.TypeNamespace);
					}
					else
					{
						messagePart.Element = new XmlQualifiedName(xmlMemberMapping.XsdElementName, xmlMemberMapping.Namespace);
					}
					Message message = new Message();
					message.Name = codeIdentifiers.AddUnique(methodName + messagePart.Name, message);
					message.Parts.Add(messagePart);
					base.HeaderMessages.Add(message);
					ServiceDescriptionFormatExtension serviceDescriptionFormatExtension = this.CreateSoapHeaderBinding(new XmlQualifiedName(message.Name, base.Binding.ServiceDescription.TargetNamespace), messagePart.Name, rpc ? xmlMemberMapping.Namespace : null, use);
					if ((soapReflectedHeader.direction & SoapHeaderDirection.In) != (SoapHeaderDirection)0)
					{
						base.OperationBinding.Input.Extensions.Add(serviceDescriptionFormatExtension);
					}
					if ((soapReflectedHeader.direction & SoapHeaderDirection.Out) != (SoapHeaderDirection)0)
					{
						base.OperationBinding.Output.Extensions.Add(serviceDescriptionFormatExtension);
					}
					if ((soapReflectedHeader.direction & SoapHeaderDirection.Fault) != (SoapHeaderDirection)0)
					{
						if (this.soapMethod.IsClaimsConformance)
						{
							throw new InvalidOperationException(Res.GetString("BPConformanceHeaderFault", new object[]
							{
								this.soapMethod.methodInfo.ToString(),
								this.soapMethod.methodInfo.DeclaringType.FullName,
								"Direction",
								typeof(SoapHeaderDirection).Name,
								SoapHeaderDirection.Fault.ToString()
							}));
						}
						base.OperationBinding.Output.Extensions.Add(serviceDescriptionFormatExtension);
					}
				}
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00040510 File Offset: 0x0003F510
		private void CreateMessage(bool rpc, SoapBindingUse use, SoapParameterStyle paramStyle, Message message, MessageBinding messageBinding, XmlMembersMapping members)
		{
			bool flag = paramStyle != SoapParameterStyle.Bare;
			if (use == SoapBindingUse.Encoded)
			{
				this.CreateEncodedMessage(message, messageBinding, members, flag && !rpc);
				return;
			}
			this.CreateLiteralMessage(message, messageBinding, members, flag && !rpc, rpc);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00040558 File Offset: 0x0003F558
		private void CreateEncodedMessage(Message message, MessageBinding messageBinding, XmlMembersMapping members, bool wrapped)
		{
			this.SoapExporter.ExportMembersMapping(members, wrapped);
			if (wrapped)
			{
				MessagePart messagePart = new MessagePart();
				messagePart.Name = "parameters";
				messagePart.Type = new XmlQualifiedName(members.TypeName, members.TypeNamespace);
				message.Parts.Add(messagePart);
			}
			else
			{
				for (int i = 0; i < members.Count; i++)
				{
					XmlMemberMapping xmlMemberMapping = members[i];
					MessagePart messagePart2 = new MessagePart();
					messagePart2.Name = xmlMemberMapping.XsdElementName;
					messagePart2.Type = new XmlQualifiedName(xmlMemberMapping.TypeName, xmlMemberMapping.TypeNamespace);
					message.Parts.Add(messagePart2);
				}
			}
			messageBinding.Extensions.Add(this.CreateSoapBodyBinding(SoapBindingUse.Encoded, members.Namespace));
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00040618 File Offset: 0x0003F618
		private void CreateLiteralMessage(Message message, MessageBinding messageBinding, XmlMembersMapping members, bool wrapped, bool rpc)
		{
			if (members.Count == 1 && members[0].Any && members[0].ElementName.Length == 0 && !wrapped)
			{
				string text = base.SchemaExporter.ExportAnyType(members[0].Namespace);
				MessagePart messagePart = new MessagePart();
				messagePart.Name = members[0].MemberName;
				messagePart.Type = new XmlQualifiedName(text, members[0].Namespace);
				message.Parts.Add(messagePart);
			}
			else
			{
				base.SchemaExporter.ExportMembersMapping(members, !rpc);
				if (wrapped)
				{
					MessagePart messagePart2 = new MessagePart();
					messagePart2.Name = "parameters";
					messagePart2.Element = new XmlQualifiedName(members.XsdElementName, members.Namespace);
					message.Parts.Add(messagePart2);
				}
				else
				{
					for (int i = 0; i < members.Count; i++)
					{
						XmlMemberMapping xmlMemberMapping = members[i];
						MessagePart messagePart3 = new MessagePart();
						if (rpc)
						{
							if (xmlMemberMapping.TypeName == null || xmlMemberMapping.TypeName.Length == 0)
							{
								throw new InvalidOperationException(Res.GetString("WsdlGenRpcLitAnonimousType", new object[]
								{
									base.Method.DeclaringType.Name,
									base.Method.Name,
									xmlMemberMapping.MemberName
								}));
							}
							messagePart3.Name = xmlMemberMapping.XsdElementName;
							messagePart3.Type = new XmlQualifiedName(xmlMemberMapping.TypeName, xmlMemberMapping.TypeNamespace);
						}
						else
						{
							messagePart3.Name = XmlConvert.EncodeLocalName(xmlMemberMapping.MemberName);
							messagePart3.Element = new XmlQualifiedName(xmlMemberMapping.XsdElementName, xmlMemberMapping.Namespace);
						}
						message.Parts.Add(messagePart3);
					}
				}
			}
			messageBinding.Extensions.Add(this.CreateSoapBodyBinding(SoapBindingUse.Literal, rpc ? members.Namespace : null));
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00040810 File Offset: 0x0003F810
		private static string[] GetParameterOrder(LogicalMethodInfo methodInfo)
		{
			ParameterInfo[] parameters = methodInfo.Parameters;
			string[] array = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].Name;
			}
			return array;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00040847 File Offset: 0x0003F847
		protected override string ReflectMethodBinding()
		{
			return SoapReflector.GetSoapMethodBinding(base.Method);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00040854 File Offset: 0x0003F854
		protected override void BeginClass()
		{
			if (base.Binding != null)
			{
				SoapBindingStyle soapBindingStyle;
				if (SoapReflector.GetSoapServiceAttribute(base.ServiceType) is SoapRpcServiceAttribute)
				{
					soapBindingStyle = SoapBindingStyle.Rpc;
				}
				else
				{
					soapBindingStyle = SoapBindingStyle.Document;
				}
				base.Binding.Extensions.Add(this.CreateSoapBinding(soapBindingStyle));
				SoapReflector.IncludeTypes(base.Methods, this.SoapImporter);
			}
			base.Port.Extensions.Add(this.CreateSoapAddressBinding(base.ServiceUrl));
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000408C8 File Offset: 0x0003F8C8
		private void AllowExtensionsToReflectMethod()
		{
			if (this.extensions == null)
			{
				TypeElementCollection soapExtensionReflectorTypes = WebServicesSection.Current.SoapExtensionReflectorTypes;
				this.extensions = new SoapExtensionReflector[soapExtensionReflectorTypes.Count];
				for (int i = 0; i < this.extensions.Length; i++)
				{
					SoapExtensionReflector soapExtensionReflector = (SoapExtensionReflector)Activator.CreateInstance(soapExtensionReflectorTypes[i].Type);
					soapExtensionReflector.ReflectionContext = this;
					this.extensions[i] = soapExtensionReflector;
				}
			}
			foreach (SoapExtensionReflector soapExtensionReflector2 in this.extensions)
			{
				soapExtensionReflector2.ReflectMethod();
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0004095C File Offset: 0x0003F95C
		private void AllowExtensionsToReflectDescription()
		{
			if (this.extensions == null)
			{
				TypeElementCollection soapExtensionReflectorTypes = WebServicesSection.Current.SoapExtensionReflectorTypes;
				this.extensions = new SoapExtensionReflector[soapExtensionReflectorTypes.Count];
				for (int i = 0; i < this.extensions.Length; i++)
				{
					SoapExtensionReflector soapExtensionReflector = (SoapExtensionReflector)Activator.CreateInstance(soapExtensionReflectorTypes[i].Type);
					soapExtensionReflector.ReflectionContext = this;
					this.extensions[i] = soapExtensionReflector;
				}
			}
			foreach (SoapExtensionReflector soapExtensionReflector2 in this.extensions)
			{
				soapExtensionReflector2.ReflectDescription();
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000409F0 File Offset: 0x0003F9F0
		protected virtual SoapBinding CreateSoapBinding(SoapBindingStyle style)
		{
			return new SoapBinding
			{
				Transport = "http://schemas.xmlsoap.org/soap/http",
				Style = style
			};
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00040A18 File Offset: 0x0003FA18
		protected virtual SoapAddressBinding CreateSoapAddressBinding(string serviceUrl)
		{
			return new SoapAddressBinding
			{
				Location = serviceUrl
			};
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00040A34 File Offset: 0x0003FA34
		protected virtual SoapOperationBinding CreateSoapOperationBinding(SoapBindingStyle style, string action)
		{
			return new SoapOperationBinding
			{
				SoapAction = action,
				Style = style
			};
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00040A58 File Offset: 0x0003FA58
		protected virtual SoapBodyBinding CreateSoapBodyBinding(SoapBindingUse use, string ns)
		{
			SoapBodyBinding soapBodyBinding = new SoapBodyBinding();
			soapBodyBinding.Use = use;
			if (use == SoapBindingUse.Encoded)
			{
				soapBodyBinding.Encoding = "http://schemas.xmlsoap.org/soap/encoding/";
			}
			soapBodyBinding.Namespace = ns;
			return soapBodyBinding;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00040A89 File Offset: 0x0003FA89
		protected virtual SoapHeaderBinding CreateSoapHeaderBinding(XmlQualifiedName message, string partName, SoapBindingUse use)
		{
			return this.CreateSoapHeaderBinding(message, partName, null, use);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00040A98 File Offset: 0x0003FA98
		protected virtual SoapHeaderBinding CreateSoapHeaderBinding(XmlQualifiedName message, string partName, string ns, SoapBindingUse use)
		{
			SoapHeaderBinding soapHeaderBinding = new SoapHeaderBinding();
			soapHeaderBinding.Message = message;
			soapHeaderBinding.Part = partName;
			soapHeaderBinding.Use = use;
			if (use == SoapBindingUse.Encoded)
			{
				soapHeaderBinding.Encoding = "http://schemas.xmlsoap.org/soap/encoding/";
				soapHeaderBinding.Namespace = ns;
			}
			return soapHeaderBinding;
		}

		// Token: 0x040005B1 RID: 1457
		private ArrayList mappings = new ArrayList();

		// Token: 0x040005B2 RID: 1458
		private SoapExtensionReflector[] extensions;

		// Token: 0x040005B3 RID: 1459
		private SoapReflectedMethod soapMethod;
	}
}
