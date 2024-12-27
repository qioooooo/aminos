using System;
using System.ComponentModel;
using System.Text;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000EE RID: 238
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Operation : NamedItem
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x0001D56A File Offset: 0x0001C56A
		internal void SetParent(PortType parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001D573 File Offset: 0x0001C573
		[XmlIgnore]
		public override ServiceDescriptionFormatExtensionCollection Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new ServiceDescriptionFormatExtensionCollection(this);
				}
				return this.extensions;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0001D58F File Offset: 0x0001C58F
		public PortType PortType
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001D598 File Offset: 0x0001C598
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001D5F0 File Offset: 0x0001C5F0
		[XmlAttribute("parameterOrder")]
		[DefaultValue("")]
		public string ParameterOrderString
		{
			get
			{
				if (this.parameters == null)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.parameters.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(this.parameters[i]);
				}
				return stringBuilder.ToString();
			}
			set
			{
				if (value == null)
				{
					this.parameters = null;
					return;
				}
				this.parameters = value.Split(new char[] { ' ' });
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0001D621 File Offset: 0x0001C621
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0001D629 File Offset: 0x0001C629
		[XmlIgnore]
		public string[] ParameterOrder
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001D632 File Offset: 0x0001C632
		[XmlElement("input", typeof(OperationInput))]
		[XmlElement("output", typeof(OperationOutput))]
		public OperationMessageCollection Messages
		{
			get
			{
				if (this.messages == null)
				{
					this.messages = new OperationMessageCollection(this);
				}
				return this.messages;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001D64E File Offset: 0x0001C64E
		[XmlElement("fault")]
		public OperationFaultCollection Faults
		{
			get
			{
				if (this.faults == null)
				{
					this.faults = new OperationFaultCollection(this);
				}
				return this.faults;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001D66C File Offset: 0x0001C66C
		public bool IsBoundBy(OperationBinding operationBinding)
		{
			if (operationBinding.Name != base.Name)
			{
				return false;
			}
			OperationMessage input = this.Messages.Input;
			if (input != null)
			{
				if (operationBinding.Input == null)
				{
					return false;
				}
				string messageName = this.GetMessageName(base.Name, input.Name, true);
				string messageName2 = this.GetMessageName(operationBinding.Name, operationBinding.Input.Name, true);
				if (messageName2 != messageName)
				{
					return false;
				}
			}
			else if (operationBinding.Input != null)
			{
				return false;
			}
			OperationMessage output = this.Messages.Output;
			if (output != null)
			{
				if (operationBinding.Output == null)
				{
					return false;
				}
				string messageName3 = this.GetMessageName(base.Name, output.Name, false);
				string messageName4 = this.GetMessageName(operationBinding.Name, operationBinding.Output.Name, false);
				if (messageName4 != messageName3)
				{
					return false;
				}
			}
			else if (operationBinding.Output != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001D74C File Offset: 0x0001C74C
		private string GetMessageName(string operationName, string messageName, bool isInput)
		{
			if (messageName != null && messageName.Length > 0)
			{
				return messageName;
			}
			switch (this.Messages.Flow)
			{
			case OperationFlow.OneWay:
				if (isInput)
				{
					return operationName;
				}
				return null;
			case OperationFlow.RequestResponse:
				if (isInput)
				{
					return operationName + "Request";
				}
				return operationName + "Response";
			}
			return null;
		}

		// Token: 0x0400046D RID: 1133
		private string[] parameters;

		// Token: 0x0400046E RID: 1134
		private OperationMessageCollection messages;

		// Token: 0x0400046F RID: 1135
		private OperationFaultCollection faults;

		// Token: 0x04000470 RID: 1136
		private PortType parent;

		// Token: 0x04000471 RID: 1137
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
