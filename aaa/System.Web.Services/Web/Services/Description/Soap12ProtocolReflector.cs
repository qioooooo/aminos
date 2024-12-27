using System;
using System.Collections;
using System.Xml;

namespace System.Web.Services.Description
{
	// Token: 0x02000119 RID: 281
	internal class Soap12ProtocolReflector : SoapProtocolReflector
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00040AEC File Offset: 0x0003FAEC
		internal override WsiProfiles ConformsTo
		{
			get
			{
				return WsiProfiles.None;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00040AEF File Offset: 0x0003FAEF
		public override string ProtocolName
		{
			get
			{
				return "Soap12";
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00040AF6 File Offset: 0x0003FAF6
		protected override void BeginClass()
		{
			this.requestElements = new Hashtable();
			this.actions = new Hashtable();
			this.soap11PortType = null;
			base.BeginClass();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00040B1C File Offset: 0x0003FB1C
		protected override bool ReflectMethod()
		{
			if (base.ReflectMethod())
			{
				if (base.Binding != null)
				{
					this.soap11PortType = base.SoapMethod.portType;
					if (this.soap11PortType != base.Binding.Type)
					{
						base.HeaderMessages.Clear();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00040B70 File Offset: 0x0003FB70
		protected override void EndClass()
		{
			if (base.PortType == null || base.Binding == null)
			{
				return;
			}
			if (this.soap11PortType != null && this.soap11PortType != base.Binding.Type)
			{
				foreach (object obj in base.PortType.Operations)
				{
					Operation operation = (Operation)obj;
					foreach (object obj2 in operation.Messages)
					{
						OperationMessage operationMessage = (OperationMessage)obj2;
						ServiceDescription serviceDescription = base.GetServiceDescription(operationMessage.Message.Namespace);
						if (serviceDescription != null)
						{
							Message message = serviceDescription.Messages[operationMessage.Message.Name];
							if (message != null)
							{
								serviceDescription.Messages.Remove(message);
							}
						}
					}
				}
				base.Binding.Type = this.soap11PortType;
				base.PortType.ServiceDescription.PortTypes.Remove(base.PortType);
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00040CC0 File Offset: 0x0003FCC0
		protected override SoapBinding CreateSoapBinding(SoapBindingStyle style)
		{
			return new Soap12Binding
			{
				Transport = "http://schemas.xmlsoap.org/soap/http",
				Style = style
			};
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00040CE8 File Offset: 0x0003FCE8
		protected override SoapAddressBinding CreateSoapAddressBinding(string serviceUrl)
		{
			return new Soap12AddressBinding
			{
				Location = serviceUrl
			};
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00040D04 File Offset: 0x0003FD04
		protected override SoapOperationBinding CreateSoapOperationBinding(SoapBindingStyle style, string action)
		{
			Soap12OperationBinding soap12OperationBinding = new Soap12OperationBinding();
			soap12OperationBinding.SoapAction = action;
			soap12OperationBinding.Style = style;
			soap12OperationBinding.Method = base.SoapMethod;
			this.DealWithAmbiguity(action, base.SoapMethod.requestElementName.ToString(), soap12OperationBinding);
			return soap12OperationBinding;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00040D4C File Offset: 0x0003FD4C
		protected override SoapBodyBinding CreateSoapBodyBinding(SoapBindingUse use, string ns)
		{
			Soap12BodyBinding soap12BodyBinding = new Soap12BodyBinding();
			soap12BodyBinding.Use = use;
			if (use == SoapBindingUse.Encoded)
			{
				soap12BodyBinding.Encoding = "http://www.w3.org/2003/05/soap-encoding";
			}
			soap12BodyBinding.Namespace = ns;
			return soap12BodyBinding;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00040D7D File Offset: 0x0003FD7D
		protected override SoapHeaderBinding CreateSoapHeaderBinding(XmlQualifiedName message, string partName, SoapBindingUse use)
		{
			return this.CreateSoapHeaderBinding(message, partName, null, use);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00040D8C File Offset: 0x0003FD8C
		protected override SoapHeaderBinding CreateSoapHeaderBinding(XmlQualifiedName message, string partName, string ns, SoapBindingUse use)
		{
			Soap12HeaderBinding soap12HeaderBinding = new Soap12HeaderBinding();
			soap12HeaderBinding.Message = message;
			soap12HeaderBinding.Part = partName;
			soap12HeaderBinding.Namespace = ns;
			soap12HeaderBinding.Use = use;
			if (use == SoapBindingUse.Encoded)
			{
				soap12HeaderBinding.Encoding = "http://www.w3.org/2003/05/soap-encoding";
			}
			return soap12HeaderBinding;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00040DD0 File Offset: 0x0003FDD0
		private void DealWithAmbiguity(string action, string requestElement, Soap12OperationBinding operation)
		{
			Soap12OperationBinding soap12OperationBinding = (Soap12OperationBinding)this.actions[action];
			if (soap12OperationBinding != null)
			{
				operation.DuplicateBySoapAction = soap12OperationBinding;
				soap12OperationBinding.DuplicateBySoapAction = operation;
				this.CheckOperationDuplicates(soap12OperationBinding);
			}
			else
			{
				this.actions[action] = operation;
			}
			Soap12OperationBinding soap12OperationBinding2 = (Soap12OperationBinding)this.requestElements[requestElement];
			if (soap12OperationBinding2 != null)
			{
				operation.DuplicateByRequestElement = soap12OperationBinding2;
				soap12OperationBinding2.DuplicateByRequestElement = operation;
				this.CheckOperationDuplicates(soap12OperationBinding2);
			}
			else
			{
				this.requestElements[requestElement] = operation;
			}
			this.CheckOperationDuplicates(operation);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00040E58 File Offset: 0x0003FE58
		private void CheckOperationDuplicates(Soap12OperationBinding operation)
		{
			if (operation.DuplicateByRequestElement == null)
			{
				operation.SoapActionRequired = false;
				return;
			}
			if (operation.DuplicateBySoapAction != null)
			{
				throw new InvalidOperationException(Res.GetString("TheMethodsAndUseTheSameRequestElementAndSoapActionXmlns6", new object[]
				{
					operation.Method.name,
					operation.DuplicateByRequestElement.Method.name,
					operation.Method.requestElementName.Name,
					operation.Method.requestElementName.Namespace,
					operation.DuplicateBySoapAction.Method.name,
					operation.Method.action
				}));
			}
			operation.SoapActionRequired = true;
		}

		// Token: 0x040005B4 RID: 1460
		private Hashtable requestElements;

		// Token: 0x040005B5 RID: 1461
		private Hashtable actions;

		// Token: 0x040005B6 RID: 1462
		private XmlQualifiedName soap11PortType;
	}
}
