using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000BB RID: 187
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ProtocolImporter
	{
		// Token: 0x060004D4 RID: 1236 RVA: 0x00017E59 File Offset: 0x00016E59
		internal void Initialize(ServiceDescriptionImporter importer)
		{
			this.importer = importer;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00017E62 File Offset: 0x00016E62
		public ServiceDescriptionCollection ServiceDescriptions
		{
			get
			{
				return this.importer.ServiceDescriptions;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00017E6F File Offset: 0x00016E6F
		public XmlSchemas Schemas
		{
			get
			{
				return this.importer.AllSchemas;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00017E7C File Offset: 0x00016E7C
		public XmlSchemas AbstractSchemas
		{
			get
			{
				return this.importer.AbstractSchemas;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00017E89 File Offset: 0x00016E89
		public XmlSchemas ConcreteSchemas
		{
			get
			{
				return this.importer.ConcreteSchemas;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00017E96 File Offset: 0x00016E96
		public CodeNamespace CodeNamespace
		{
			get
			{
				return this.codeNamespace;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00017E9E File Offset: 0x00016E9E
		public CodeTypeDeclaration CodeTypeDeclaration
		{
			get
			{
				return this.codeClass;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00017EA6 File Offset: 0x00016EA6
		internal CodeTypeDeclarationCollection ExtraCodeClasses
		{
			get
			{
				if (this.classes == null)
				{
					this.classes = new CodeTypeDeclarationCollection();
				}
				return this.classes;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00017EC1 File Offset: 0x00016EC1
		public ServiceDescriptionImportStyle Style
		{
			get
			{
				return this.importer.Style;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00017ECE File Offset: 0x00016ECE
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00017ED6 File Offset: 0x00016ED6
		public ServiceDescriptionImportWarnings Warnings
		{
			get
			{
				return this.warnings;
			}
			set
			{
				this.warnings = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00017EDF File Offset: 0x00016EDF
		public CodeIdentifiers ClassNames
		{
			get
			{
				return this.importContext.TypeIdentifiers;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00017EEC File Offset: 0x00016EEC
		public string MethodName
		{
			get
			{
				return CodeIdentifier.MakeValid(XmlConvert.DecodeName(this.Operation.Name));
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00017F03 File Offset: 0x00016F03
		public string ClassName
		{
			get
			{
				return this.className;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00017F0B File Offset: 0x00016F0B
		public Port Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00017F13 File Offset: 0x00016F13
		public PortType PortType
		{
			get
			{
				return this.portType;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00017F1B File Offset: 0x00016F1B
		public Binding Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00017F23 File Offset: 0x00016F23
		public Service Service
		{
			get
			{
				return this.service;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00017F2B File Offset: 0x00016F2B
		internal ServiceDescriptionImporter ServiceImporter
		{
			get
			{
				return this.importer;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00017F33 File Offset: 0x00016F33
		public Operation Operation
		{
			get
			{
				return this.operation;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00017F3B File Offset: 0x00016F3B
		public OperationBinding OperationBinding
		{
			get
			{
				return this.operationBinding;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00017F43 File Offset: 0x00016F43
		public Message InputMessage
		{
			get
			{
				return this.inputMessage;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00017F4B File Offset: 0x00016F4B
		public Message OutputMessage
		{
			get
			{
				return this.outputMessage;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00017F53 File Offset: 0x00016F53
		internal ImportContext ImportContext
		{
			get
			{
				return this.importContext;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00017F5B File Offset: 0x00016F5B
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x00017F63 File Offset: 0x00016F63
		internal bool IsEncodedBinding
		{
			get
			{
				return this.encodedBinding;
			}
			set
			{
				this.encodedBinding = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00017F6C File Offset: 0x00016F6C
		internal Hashtable ExportContext
		{
			get
			{
				if (this.exportContext == null)
				{
					this.exportContext = new Hashtable();
				}
				return this.exportContext;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00017F87 File Offset: 0x00016F87
		internal CodeIdentifiers MethodNames
		{
			get
			{
				if (this.methodNames == null)
				{
					this.methodNames = new CodeIdentifiers();
				}
				return this.methodNames;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00017FA4 File Offset: 0x00016FA4
		internal bool GenerateCode(CodeNamespace codeNamespace, ImportContext importContext, Hashtable exportContext)
		{
			this.bindingCount = 0;
			this.anyPorts = false;
			this.codeNamespace = codeNamespace;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			foreach (object obj in this.ServiceDescriptions)
			{
				ServiceDescription serviceDescription = (ServiceDescription)obj;
				foreach (object obj2 in serviceDescription.Services)
				{
					Service service = (Service)obj2;
					foreach (object obj3 in service.Ports)
					{
						Port port = (Port)obj3;
						Binding binding = this.ServiceDescriptions.GetBinding(port.Binding);
						if (!hashtable.Contains(binding))
						{
							PortType portType = this.ServiceDescriptions.GetPortType(binding.Type);
							this.MoveToBinding(service, port, binding, portType);
							if (this.IsBindingSupported())
							{
								this.bindingCount++;
								this.anyPorts = true;
								hashtable.Add(binding, binding);
							}
							else if (binding != null)
							{
								hashtable2[binding] = binding;
							}
						}
					}
				}
			}
			if (this.bindingCount == 0)
			{
				foreach (object obj4 in this.ServiceDescriptions)
				{
					ServiceDescription serviceDescription2 = (ServiceDescription)obj4;
					foreach (object obj5 in serviceDescription2.Bindings)
					{
						Binding binding2 = (Binding)obj5;
						if (!hashtable2.Contains(binding2))
						{
							PortType portType2 = this.ServiceDescriptions.GetPortType(binding2.Type);
							this.MoveToBinding(binding2, portType2);
							if (this.IsBindingSupported())
							{
								this.bindingCount++;
							}
						}
					}
				}
			}
			if (this.bindingCount == 0)
			{
				return codeNamespace.Comments.Count > 0;
			}
			this.importContext = importContext;
			this.exportContext = exportContext;
			this.BeginNamespace();
			hashtable.Clear();
			foreach (object obj6 in this.ServiceDescriptions)
			{
				ServiceDescription serviceDescription3 = (ServiceDescription)obj6;
				if (this.anyPorts)
				{
					using (IEnumerator enumerator7 = serviceDescription3.Services.GetEnumerator())
					{
						while (enumerator7.MoveNext())
						{
							object obj7 = enumerator7.Current;
							Service service2 = (Service)obj7;
							foreach (object obj8 in service2.Ports)
							{
								Port port2 = (Port)obj8;
								Binding binding3 = this.ServiceDescriptions.GetBinding(port2.Binding);
								PortType portType3 = this.ServiceDescriptions.GetPortType(binding3.Type);
								this.MoveToBinding(service2, port2, binding3, portType3);
								if (this.IsBindingSupported() && !hashtable.Contains(binding3))
								{
									this.GenerateClassForBinding();
									hashtable.Add(binding3, binding3);
								}
							}
						}
						continue;
					}
				}
				foreach (object obj9 in serviceDescription3.Bindings)
				{
					Binding binding4 = (Binding)obj9;
					PortType portType4 = this.ServiceDescriptions.GetPortType(binding4.Type);
					this.MoveToBinding(binding4, portType4);
					if (this.IsBindingSupported())
					{
						this.GenerateClassForBinding();
					}
				}
			}
			this.EndNamespace();
			return true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00018480 File Offset: 0x00017480
		private void MoveToBinding(Binding binding, PortType portType)
		{
			this.MoveToBinding(null, null, binding, portType);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001848C File Offset: 0x0001748C
		private void MoveToBinding(Service service, Port port, Binding binding, PortType portType)
		{
			this.service = service;
			this.port = port;
			this.portType = portType;
			this.binding = binding;
			this.encodedBinding = false;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000184B4 File Offset: 0x000174B4
		private void MoveToOperation(Operation operation)
		{
			this.operation = operation;
			this.operationBinding = null;
			foreach (object obj in this.binding.Operations)
			{
				OperationBinding operationBinding = (OperationBinding)obj;
				if (operation.IsBoundBy(operationBinding))
				{
					if (this.operationBinding != null)
					{
						throw this.OperationSyntaxException(Res.GetString("DuplicateInputOutputNames0"));
					}
					this.operationBinding = operationBinding;
				}
			}
			if (this.operationBinding == null)
			{
				throw this.OperationSyntaxException(Res.GetString("MissingBinding0"));
			}
			if (operation.Messages.Input != null && this.operationBinding.Input == null)
			{
				throw this.OperationSyntaxException(Res.GetString("MissingInputBinding0"));
			}
			if (operation.Messages.Output != null && this.operationBinding.Output == null)
			{
				throw this.OperationSyntaxException(Res.GetString("MissingOutputBinding0"));
			}
			this.inputMessage = ((operation.Messages.Input == null) ? null : this.ServiceDescriptions.GetMessage(operation.Messages.Input.Message));
			this.outputMessage = ((operation.Messages.Output == null) ? null : this.ServiceDescriptions.GetMessage(operation.Messages.Output.Message));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018614 File Offset: 0x00017614
		private void GenerateClassForBinding()
		{
			try
			{
				if (this.bindingCount == 1 && this.service != null && this.Style != ServiceDescriptionImportStyle.ServerInterface)
				{
					this.className = XmlConvert.DecodeName(this.service.Name);
				}
				else
				{
					this.className = this.binding.Name;
					if (this.Style == ServiceDescriptionImportStyle.ServerInterface)
					{
						this.className = "I" + CodeIdentifier.MakePascal(this.className);
					}
				}
				this.className = XmlConvert.DecodeName(this.className);
				this.className = this.ClassNames.AddUnique(CodeIdentifier.MakeValid(this.className), null);
				this.codeClass = this.BeginClass();
				int num = 0;
				int i = 0;
				while (i < this.portType.Operations.Count)
				{
					this.MoveToOperation(this.portType.Operations[i]);
					if (this.IsOperationFlowSupported(this.operation.Messages.Flow))
					{
						goto IL_015D;
					}
					switch (this.operation.Messages.Flow)
					{
					case OperationFlow.OneWay:
						this.UnsupportedOperationWarning(Res.GetString("OneWayIsNotSupported0"));
						break;
					case OperationFlow.Notification:
						this.UnsupportedOperationWarning(Res.GetString("NotificationIsNotSupported0"));
						break;
					case OperationFlow.RequestResponse:
						this.UnsupportedOperationWarning(Res.GetString("RequestResponseIsNotSupported0"));
						break;
					case OperationFlow.SolicitResponse:
						this.UnsupportedOperationWarning(Res.GetString("SolicitResponseIsNotSupported0"));
						break;
					default:
						goto IL_015D;
					}
					IL_0255:
					i++;
					continue;
					CodeMemberMethod codeMemberMethod;
					try
					{
						IL_015D:
						codeMemberMethod = this.GenerateMethod();
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
						throw new InvalidOperationException(Res.GetString("UnableToImportOperation1", new object[] { this.operation.Name }), ex);
					}
					catch
					{
						throw new InvalidOperationException(Res.GetString("UnableToImportOperation1", new object[] { this.operation.Name }), null);
					}
					if (codeMemberMethod != null)
					{
						this.AddExtensionWarningComments(this.codeClass.Comments, this.operationBinding.Extensions);
						if (this.operationBinding.Input != null)
						{
							this.AddExtensionWarningComments(this.codeClass.Comments, this.operationBinding.Input.Extensions);
						}
						if (this.operationBinding.Output != null)
						{
							this.AddExtensionWarningComments(this.codeClass.Comments, this.operationBinding.Output.Extensions);
						}
						num++;
						goto IL_0255;
					}
					goto IL_0255;
				}
				bool flag = (this.ServiceImporter.CodeGenerationOptions & CodeGenerationOptions.GenerateNewAsync) != CodeGenerationOptions.None && this.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.DeclareEvents) && this.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.DeclareDelegates);
				if (flag && num > 0 && this.Style == ServiceDescriptionImportStyle.Client)
				{
					CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
					string text = "CancelAsync";
					string text2 = this.MethodNames.AddUnique(text, text);
					CodeMemberMethod codeMemberMethod2 = WebCodeGenerator.AddMethod(this.CodeTypeDeclaration, text2, new CodeFlags[1], new string[] { typeof(object).FullName }, new string[] { "userState" }, typeof(void).FullName, codeAttributeDeclarationCollection, CodeFlags.IsPublic | ((text != text2) ? ((CodeFlags)0) : CodeFlags.IsNew));
					codeMemberMethod2.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
					CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), text, new CodeExpression[0]);
					codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("userState"));
					codeMemberMethod2.Statements.Add(codeMethodInvokeExpression);
				}
				this.EndClass();
				if (this.portType.Operations.Count == 0)
				{
					this.NoMethodsGeneratedWarning();
				}
				this.AddExtensionWarningComments(this.codeClass.Comments, this.binding.Extensions);
				if (this.port != null)
				{
					this.AddExtensionWarningComments(this.codeClass.Comments, this.port.Extensions);
				}
				this.codeNamespace.Types.Add(this.codeClass);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				throw new InvalidOperationException(Res.GetString("UnableToImportBindingFromNamespace2", new object[]
				{
					this.binding.Name,
					this.binding.ServiceDescription.TargetNamespace
				}), ex2);
			}
			catch
			{
				throw new InvalidOperationException(Res.GetString("UnableToImportBindingFromNamespace2", new object[]
				{
					this.binding.Name,
					this.binding.ServiceDescription.TargetNamespace
				}), null);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00018B44 File Offset: 0x00017B44
		public void AddExtensionWarningComments(CodeCommentStatementCollection comments, ServiceDescriptionFormatExtensionCollection extensions)
		{
			foreach (object obj in extensions)
			{
				if (!extensions.IsHandled(obj))
				{
					string text = null;
					string text2 = null;
					if (obj is XmlElement)
					{
						XmlElement xmlElement = (XmlElement)obj;
						text = xmlElement.LocalName;
						text2 = xmlElement.NamespaceURI;
					}
					else if (obj is ServiceDescriptionFormatExtension)
					{
						XmlFormatExtensionAttribute[] array = (XmlFormatExtensionAttribute[])obj.GetType().GetCustomAttributes(typeof(XmlFormatExtensionAttribute), false);
						if (array.Length > 0)
						{
							text = array[0].ElementName;
							text2 = array[0].Namespace;
						}
					}
					if (text != null)
					{
						if (extensions.IsRequired(obj))
						{
							this.warnings |= ServiceDescriptionImportWarnings.RequiredExtensionsIgnored;
							ProtocolImporter.AddWarningComment(comments, Res.GetString("WebServiceDescriptionIgnoredRequired", new object[] { text, text2 }));
						}
						else
						{
							this.warnings |= ServiceDescriptionImportWarnings.OptionalExtensionsIgnored;
							ProtocolImporter.AddWarningComment(comments, Res.GetString("WebServiceDescriptionIgnoredOptional", new object[] { text, text2 }));
						}
					}
				}
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00018C7C File Offset: 0x00017C7C
		public void UnsupportedBindingWarning(string text)
		{
			ProtocolImporter.AddWarningComment((this.codeClass == null) ? this.codeNamespace.Comments : this.codeClass.Comments, Res.GetString("TheBinding0FromNamespace1WasIgnored2", new object[]
			{
				this.Binding.Name,
				this.Binding.ServiceDescription.TargetNamespace,
				text
			}));
			this.warnings |= ServiceDescriptionImportWarnings.UnsupportedBindingsIgnored;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00018CF4 File Offset: 0x00017CF4
		public void UnsupportedOperationWarning(string text)
		{
			ProtocolImporter.AddWarningComment((this.codeClass == null) ? this.codeNamespace.Comments : this.codeClass.Comments, Res.GetString("TheOperation0FromNamespace1WasIgnored2", new object[]
			{
				this.operation.Name,
				this.operation.PortType.ServiceDescription.TargetNamespace,
				text
			}));
			this.warnings |= ServiceDescriptionImportWarnings.UnsupportedOperationsIgnored;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00018D70 File Offset: 0x00017D70
		public void UnsupportedOperationBindingWarning(string text)
		{
			ProtocolImporter.AddWarningComment((this.codeClass == null) ? this.codeNamespace.Comments : this.codeClass.Comments, Res.GetString("TheOperationBinding0FromNamespace1WasIgnored", new object[]
			{
				this.operationBinding.Name,
				this.operationBinding.Binding.ServiceDescription.TargetNamespace,
				text
			}));
			this.warnings |= ServiceDescriptionImportWarnings.UnsupportedOperationsIgnored;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00018DEC File Offset: 0x00017DEC
		private void NoMethodsGeneratedWarning()
		{
			ProtocolImporter.AddWarningComment(this.codeClass.Comments, Res.GetString("NoMethodsWereFoundInTheWSDLForThisProtocol"));
			this.warnings |= ServiceDescriptionImportWarnings.NoMethodsGenerated;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00018E18 File Offset: 0x00017E18
		internal static void AddWarningComment(CodeCommentStatementCollection comments, string text)
		{
			comments.Add(new CodeCommentStatement(Res.GetString("CodegenWarningDetails", new object[] { text })));
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00018E48 File Offset: 0x00017E48
		public Exception OperationSyntaxException(string text)
		{
			return new Exception(Res.GetString("TheOperationFromNamespaceHadInvalidSyntax3", new object[]
			{
				this.operation.Name,
				this.operation.PortType.Name,
				this.operation.PortType.ServiceDescription.TargetNamespace,
				text
			}));
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00018EAC File Offset: 0x00017EAC
		public Exception OperationBindingSyntaxException(string text)
		{
			return new Exception(Res.GetString("TheOperationBindingFromNamespaceHadInvalid3", new object[]
			{
				this.operationBinding.Name,
				this.operationBinding.Binding.ServiceDescription.TargetNamespace,
				text
			}));
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004FD RID: 1277
		public abstract string ProtocolName { get; }

		// Token: 0x060004FE RID: 1278 RVA: 0x00018EFA File Offset: 0x00017EFA
		protected virtual void BeginNamespace()
		{
			this.MethodNames.Clear();
		}

		// Token: 0x060004FF RID: 1279
		protected abstract bool IsBindingSupported();

		// Token: 0x06000500 RID: 1280
		protected abstract bool IsOperationFlowSupported(OperationFlow flow);

		// Token: 0x06000501 RID: 1281
		protected abstract CodeTypeDeclaration BeginClass();

		// Token: 0x06000502 RID: 1282
		protected abstract CodeMemberMethod GenerateMethod();

		// Token: 0x06000503 RID: 1283 RVA: 0x00018F07 File Offset: 0x00017F07
		protected virtual void EndClass()
		{
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00018F0C File Offset: 0x00017F0C
		protected virtual void EndNamespace()
		{
			if (this.classes != null)
			{
				foreach (object obj in this.classes)
				{
					CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
					this.codeNamespace.Types.Add(codeTypeDeclaration);
				}
			}
			CodeGenerator.ValidateIdentifiers(this.codeNamespace);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00018F84 File Offset: 0x00017F84
		internal static string UniqueName(string baseName, string[] scope)
		{
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			for (int i = 0; i < scope.Length; i++)
			{
				codeIdentifiers.AddUnique(scope[i], scope[i]);
			}
			return codeIdentifiers.AddUnique(baseName, baseName);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00018FBC File Offset: 0x00017FBC
		internal static string MethodSignature(string methodName, string returnType, CodeFlags[] parameterFlags, string[] parameterTypes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(returnType);
			stringBuilder.Append(" ");
			stringBuilder.Append(methodName);
			stringBuilder.Append(" (");
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if ((parameterFlags[i] & CodeFlags.IsByRef) != (CodeFlags)0)
				{
					stringBuilder.Append("ref ");
				}
				else if ((parameterFlags[i] & CodeFlags.IsOut) != (CodeFlags)0)
				{
					stringBuilder.Append("out ");
				}
				stringBuilder.Append(parameterTypes[i]);
				if (i > 0)
				{
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040003E5 RID: 997
		private ServiceDescriptionImporter importer;

		// Token: 0x040003E6 RID: 998
		private CodeNamespace codeNamespace;

		// Token: 0x040003E7 RID: 999
		private CodeIdentifiers methodNames;

		// Token: 0x040003E8 RID: 1000
		private CodeTypeDeclaration codeClass;

		// Token: 0x040003E9 RID: 1001
		private CodeTypeDeclarationCollection classes;

		// Token: 0x040003EA RID: 1002
		private ServiceDescriptionImportWarnings warnings;

		// Token: 0x040003EB RID: 1003
		private Port port;

		// Token: 0x040003EC RID: 1004
		private PortType portType;

		// Token: 0x040003ED RID: 1005
		private Binding binding;

		// Token: 0x040003EE RID: 1006
		private Operation operation;

		// Token: 0x040003EF RID: 1007
		private OperationBinding operationBinding;

		// Token: 0x040003F0 RID: 1008
		private bool encodedBinding;

		// Token: 0x040003F1 RID: 1009
		private ImportContext importContext;

		// Token: 0x040003F2 RID: 1010
		private Hashtable exportContext;

		// Token: 0x040003F3 RID: 1011
		private Service service;

		// Token: 0x040003F4 RID: 1012
		private Message inputMessage;

		// Token: 0x040003F5 RID: 1013
		private Message outputMessage;

		// Token: 0x040003F6 RID: 1014
		private string className;

		// Token: 0x040003F7 RID: 1015
		private int bindingCount;

		// Token: 0x040003F8 RID: 1016
		private bool anyPorts;
	}
}
