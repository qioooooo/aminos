using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000BE RID: 190
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ProtocolReflector
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x0001A087 File Offset: 0x00019087
		internal void Initialize(ServiceDescriptionReflector reflector)
		{
			this.reflector = reflector;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001A090 File Offset: 0x00019090
		internal bool IsEmptyBinding
		{
			get
			{
				return this.emptyBinding;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001A098 File Offset: 0x00019098
		public Service Service
		{
			get
			{
				return this.reflector.Service;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001A0A5 File Offset: 0x000190A5
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.reflector.ServiceDescription;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001A0B2 File Offset: 0x000190B2
		public ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.reflector.ServiceDescriptions;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001A0BF File Offset: 0x000190BF
		public XmlSchemas Schemas
		{
			get
			{
				return this.reflector.Schemas;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001A0CC File Offset: 0x000190CC
		public XmlSchemaExporter SchemaExporter
		{
			get
			{
				return this.reflector.SchemaExporter;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001A0D9 File Offset: 0x000190D9
		public XmlReflectionImporter ReflectionImporter
		{
			get
			{
				return this.reflector.ReflectionImporter;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0001A0E6 File Offset: 0x000190E6
		public string DefaultNamespace
		{
			get
			{
				return this.reflector.ServiceAttribute.Namespace;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001A0F8 File Offset: 0x000190F8
		public string ServiceUrl
		{
			get
			{
				return this.reflector.ServiceUrl;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001A105 File Offset: 0x00019105
		public Type ServiceType
		{
			get
			{
				return this.reflector.ServiceType;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001A112 File Offset: 0x00019112
		public LogicalMethodInfo Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001A11A File Offset: 0x0001911A
		public Binding Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001A122 File Offset: 0x00019122
		public PortType PortType
		{
			get
			{
				return this.portType;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001A12A File Offset: 0x0001912A
		public Port Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001A132 File Offset: 0x00019132
		public Operation Operation
		{
			get
			{
				return this.operation;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001A13A File Offset: 0x0001913A
		public OperationBinding OperationBinding
		{
			get
			{
				return this.operationBinding;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001A142 File Offset: 0x00019142
		public WebMethodAttribute MethodAttribute
		{
			get
			{
				return this.methodAttr;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001A14A File Offset: 0x0001914A
		public LogicalMethodInfo[] Methods
		{
			get
			{
				return this.reflector.Methods;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001A157 File Offset: 0x00019157
		internal Hashtable ReflectionContext
		{
			get
			{
				return this.reflector.ReflectionContext;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001A164 File Offset: 0x00019164
		public Message InputMessage
		{
			get
			{
				if (this.inputMessage == null)
				{
					string text = XmlConvert.EncodeLocalName((this.methodAttr.MessageName.Length == 0) ? this.Method.Name : this.methodAttr.MessageName);
					bool flag = text != this.Method.Name;
					this.inputMessage = new Message();
					this.inputMessage.Name = text + this.ProtocolName + "In";
					OperationInput operationInput = new OperationInput();
					if (flag)
					{
						operationInput.Name = text;
					}
					operationInput.Message = new XmlQualifiedName(this.inputMessage.Name, this.bindingServiceDescription.TargetNamespace);
					this.operation.Messages.Add(operationInput);
					this.OperationBinding.Input = new InputBinding();
					if (flag)
					{
						this.OperationBinding.Input.Name = text;
					}
				}
				return this.inputMessage;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001A254 File Offset: 0x00019254
		public Message OutputMessage
		{
			get
			{
				if (this.outputMessage == null)
				{
					string text = XmlConvert.EncodeLocalName((this.methodAttr.MessageName.Length == 0) ? this.Method.Name : this.methodAttr.MessageName);
					bool flag = text != this.Method.Name;
					this.outputMessage = new Message();
					this.outputMessage.Name = text + this.ProtocolName + "Out";
					OperationOutput operationOutput = new OperationOutput();
					if (flag)
					{
						operationOutput.Name = text;
					}
					operationOutput.Message = new XmlQualifiedName(this.outputMessage.Name, this.bindingServiceDescription.TargetNamespace);
					this.operation.Messages.Add(operationOutput);
					this.OperationBinding.Output = new OutputBinding();
					if (flag)
					{
						this.OperationBinding.Output.Name = text;
					}
				}
				return this.outputMessage;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001A342 File Offset: 0x00019342
		public MessageCollection HeaderMessages
		{
			get
			{
				if (this.headerMessages == null)
				{
					this.headerMessages = new MessageCollection(this.bindingServiceDescription);
				}
				return this.headerMessages;
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001A363 File Offset: 0x00019363
		private void MoveToMethod(LogicalMethodInfo method)
		{
			this.method = method;
			this.methodAttr = method.MethodAttribute;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001A378 File Offset: 0x00019378
		internal void Reflect()
		{
			this.emptyBinding = false;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			for (int i = 0; i < this.reflector.Methods.Length; i++)
			{
				this.MoveToMethod(this.reflector.Methods[i]);
				string text = this.ReflectMethodBinding();
				if (text == null)
				{
					text = string.Empty;
				}
				ProtocolReflector.ReflectedBinding reflectedBinding = (ProtocolReflector.ReflectedBinding)hashtable2[text];
				if (reflectedBinding == null)
				{
					reflectedBinding = new ProtocolReflector.ReflectedBinding();
					reflectedBinding.bindingAttr = WebServiceBindingReflector.GetAttribute(this.method, text);
					if (reflectedBinding.bindingAttr == null || (text.Length == 0 && reflectedBinding.bindingAttr.Location.Length > 0))
					{
						reflectedBinding.bindingAttr = new WebServiceBindingAttribute();
					}
					hashtable2.Add(text, reflectedBinding);
				}
				if (reflectedBinding.bindingAttr.Location.Length == 0)
				{
					if (reflectedBinding.methodList == null)
					{
						reflectedBinding.methodList = new ArrayList();
					}
					reflectedBinding.methodList.Add(this.method);
					hashtable[reflectedBinding.bindingAttr.Name] = this.method;
				}
				else
				{
					this.AddImport(reflectedBinding.bindingAttr.Namespace, reflectedBinding.bindingAttr.Location);
				}
			}
			foreach (object obj in hashtable2.Values)
			{
				ProtocolReflector.ReflectedBinding reflectedBinding2 = (ProtocolReflector.ReflectedBinding)obj;
				this.ReflectBinding(reflectedBinding2);
			}
			if (hashtable2.Count == 0)
			{
				this.emptyBinding = true;
				ProtocolReflector.ReflectedBinding reflectedBinding3 = null;
				foreach (WebServiceBindingAttribute webServiceBindingAttribute in this.ServiceType.GetCustomAttributes(typeof(WebServiceBindingAttribute), false))
				{
					if (hashtable[webServiceBindingAttribute.Name] == null)
					{
						if (reflectedBinding3 != null)
						{
							reflectedBinding3 = null;
							break;
						}
						reflectedBinding3 = new ProtocolReflector.ReflectedBinding(webServiceBindingAttribute);
					}
				}
				if (reflectedBinding3 != null)
				{
					this.ReflectBinding(reflectedBinding3);
				}
			}
			Type[] interfaces = this.ServiceType.GetInterfaces();
			foreach (Type type in interfaces)
			{
				foreach (WebServiceBindingAttribute webServiceBindingAttribute2 in type.GetCustomAttributes(typeof(WebServiceBindingAttribute), false))
				{
					if (hashtable[webServiceBindingAttribute2.Name] == null)
					{
						this.ReflectBinding(new ProtocolReflector.ReflectedBinding(webServiceBindingAttribute2));
					}
				}
			}
			this.ReflectDescription();
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001A600 File Offset: 0x00019600
		private void AddImport(string ns, string location)
		{
			foreach (object obj in this.ServiceDescription.Imports)
			{
				Import import = (Import)obj;
				if (import.Namespace == ns && import.Location == location)
				{
					return;
				}
			}
			Import import2 = new Import();
			import2.Namespace = ns;
			import2.Location = location;
			this.ServiceDescription.Imports.Add(import2);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001A69C File Offset: 0x0001969C
		public ServiceDescription GetServiceDescription(string ns)
		{
			ServiceDescription serviceDescription = this.ServiceDescriptions[ns];
			if (serviceDescription == null)
			{
				serviceDescription = new ServiceDescription();
				serviceDescription.TargetNamespace = ns;
				this.ServiceDescriptions.Add(serviceDescription);
			}
			return serviceDescription;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001A6D4 File Offset: 0x000196D4
		private void ReflectBinding(ProtocolReflector.ReflectedBinding reflectedBinding)
		{
			string text = XmlConvert.EncodeLocalName(reflectedBinding.bindingAttr.Name);
			string text2 = reflectedBinding.bindingAttr.Namespace;
			if (text.Length == 0)
			{
				text = this.Service.Name + this.ProtocolName;
			}
			if (text2.Length == 0)
			{
				text2 = this.ServiceDescription.TargetNamespace;
			}
			WsiProfiles wsiProfiles = WsiProfiles.None;
			if (reflectedBinding.bindingAttr.Location.Length > 0)
			{
				this.portType = null;
				this.binding = null;
			}
			else
			{
				this.bindingServiceDescription = this.GetServiceDescription(text2);
				CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
				foreach (object obj in this.bindingServiceDescription.Bindings)
				{
					Binding binding = (Binding)obj;
					codeIdentifiers.AddReserved(binding.Name);
				}
				text = codeIdentifiers.AddUnique(text, this.binding);
				this.portType = new PortType();
				this.binding = new Binding();
				this.portType.Name = text;
				this.binding.Name = text;
				this.binding.Type = new XmlQualifiedName(this.portType.Name, text2);
				wsiProfiles = reflectedBinding.bindingAttr.ConformsTo & this.ConformsTo;
				if (reflectedBinding.bindingAttr.EmitConformanceClaims && wsiProfiles != WsiProfiles.None)
				{
					ServiceDescription.AddConformanceClaims(this.binding.GetDocumentationElement(), wsiProfiles);
				}
				this.bindingServiceDescription.Bindings.Add(this.binding);
				this.bindingServiceDescription.PortTypes.Add(this.portType);
			}
			if (this.portNames == null)
			{
				this.portNames = new CodeIdentifiers();
				foreach (object obj2 in this.Service.Ports)
				{
					Port port = (Port)obj2;
					this.portNames.AddReserved(port.Name);
				}
			}
			this.port = new Port();
			this.port.Binding = new XmlQualifiedName(text, text2);
			this.port.Name = this.portNames.AddUnique(text, this.port);
			this.Service.Ports.Add(this.port);
			this.BeginClass();
			if (reflectedBinding.methodList != null && reflectedBinding.methodList.Count > 0)
			{
				foreach (object obj3 in reflectedBinding.methodList)
				{
					LogicalMethodInfo logicalMethodInfo = (LogicalMethodInfo)obj3;
					this.MoveToMethod(logicalMethodInfo);
					this.operation = new Operation();
					this.operation.Name = XmlConvert.EncodeLocalName(logicalMethodInfo.Name);
					if (this.methodAttr.Description != null && this.methodAttr.Description.Length > 0)
					{
						this.operation.Documentation = this.methodAttr.Description;
					}
					this.operationBinding = new OperationBinding();
					this.operationBinding.Name = this.operation.Name;
					this.inputMessage = null;
					this.outputMessage = null;
					this.headerMessages = null;
					if (this.ReflectMethod())
					{
						if (this.inputMessage != null)
						{
							this.bindingServiceDescription.Messages.Add(this.inputMessage);
						}
						if (this.outputMessage != null)
						{
							this.bindingServiceDescription.Messages.Add(this.outputMessage);
						}
						if (this.headerMessages != null)
						{
							foreach (object obj4 in this.headerMessages)
							{
								Message message = (Message)obj4;
								this.bindingServiceDescription.Messages.Add(message);
							}
						}
						this.binding.Operations.Add(this.operationBinding);
						this.portType.Operations.Add(this.operation);
					}
				}
			}
			if (this.binding != null && wsiProfiles == WsiProfiles.BasicProfile1_1 && this.ProtocolName == "Soap")
			{
				BasicProfileViolationCollection basicProfileViolationCollection = new BasicProfileViolationCollection();
				WebServicesInteroperability.AnalyzeBinding(this.binding, this.bindingServiceDescription, this.ServiceDescriptions, basicProfileViolationCollection);
				if (basicProfileViolationCollection.Count > 0)
				{
					throw new InvalidOperationException(Res.GetString("WebWsiViolation", new object[]
					{
						this.ServiceType.FullName,
						basicProfileViolationCollection.ToString()
					}));
				}
			}
			this.EndClass();
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001ABE0 File Offset: 0x00019BE0
		internal virtual WsiProfiles ConformsTo
		{
			get
			{
				return WsiProfiles.None;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000538 RID: 1336
		public abstract string ProtocolName { get; }

		// Token: 0x06000539 RID: 1337 RVA: 0x0001ABE3 File Offset: 0x00019BE3
		protected virtual void BeginClass()
		{
		}

		// Token: 0x0600053A RID: 1338
		protected abstract bool ReflectMethod();

		// Token: 0x0600053B RID: 1339 RVA: 0x0001ABE5 File Offset: 0x00019BE5
		protected virtual string ReflectMethodBinding()
		{
			return string.Empty;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001ABEC File Offset: 0x00019BEC
		protected virtual void EndClass()
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001ABEE File Offset: 0x00019BEE
		protected virtual void ReflectDescription()
		{
		}

		// Token: 0x040003FE RID: 1022
		private ServiceDescriptionReflector reflector;

		// Token: 0x040003FF RID: 1023
		private LogicalMethodInfo method;

		// Token: 0x04000400 RID: 1024
		private Operation operation;

		// Token: 0x04000401 RID: 1025
		private OperationBinding operationBinding;

		// Token: 0x04000402 RID: 1026
		private Port port;

		// Token: 0x04000403 RID: 1027
		private PortType portType;

		// Token: 0x04000404 RID: 1028
		private Binding binding;

		// Token: 0x04000405 RID: 1029
		private WebMethodAttribute methodAttr;

		// Token: 0x04000406 RID: 1030
		private Message inputMessage;

		// Token: 0x04000407 RID: 1031
		private Message outputMessage;

		// Token: 0x04000408 RID: 1032
		private MessageCollection headerMessages;

		// Token: 0x04000409 RID: 1033
		private ServiceDescription bindingServiceDescription;

		// Token: 0x0400040A RID: 1034
		private CodeIdentifiers portNames;

		// Token: 0x0400040B RID: 1035
		private bool emptyBinding;

		// Token: 0x020000BF RID: 191
		private class ReflectedBinding
		{
			// Token: 0x0600053F RID: 1343 RVA: 0x0001ABF8 File Offset: 0x00019BF8
			internal ReflectedBinding()
			{
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x0001AC00 File Offset: 0x00019C00
			internal ReflectedBinding(WebServiceBindingAttribute bindingAttr)
			{
				this.bindingAttr = bindingAttr;
			}

			// Token: 0x0400040C RID: 1036
			public WebServiceBindingAttribute bindingAttr;

			// Token: 0x0400040D RID: 1037
			public ArrayList methodList;
		}
	}
}
