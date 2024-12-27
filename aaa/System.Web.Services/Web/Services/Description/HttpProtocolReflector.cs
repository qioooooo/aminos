using System;
using System.Reflection;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;

namespace System.Web.Services.Description
{
	// Token: 0x020000C0 RID: 192
	internal abstract class HttpProtocolReflector : ProtocolReflector
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x0001AC10 File Offset: 0x00019C10
		protected HttpProtocolReflector()
		{
			Type[] mimeReflectorTypes = WebServicesSection.Current.MimeReflectorTypes;
			this.reflectors = new MimeReflector[mimeReflectorTypes.Length];
			for (int i = 0; i < this.reflectors.Length; i++)
			{
				MimeReflector mimeReflector = (MimeReflector)Activator.CreateInstance(mimeReflectorTypes[i]);
				mimeReflector.ReflectionContext = this;
				this.reflectors[i] = mimeReflector;
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001AC70 File Offset: 0x00019C70
		protected bool ReflectMimeParameters()
		{
			bool flag = false;
			for (int i = 0; i < this.reflectors.Length; i++)
			{
				if (this.reflectors[i].ReflectParameters())
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001ACA4 File Offset: 0x00019CA4
		protected bool ReflectMimeReturn()
		{
			if (base.Method.ReturnType == typeof(void))
			{
				Message outputMessage = base.OutputMessage;
				return true;
			}
			bool flag = false;
			for (int i = 0; i < this.reflectors.Length; i++)
			{
				if (this.reflectors[i].ReflectReturn())
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001ACFA File Offset: 0x00019CFA
		protected bool ReflectUrlParameters()
		{
			if (!HttpServerProtocol.AreUrlParametersSupported(base.Method))
			{
				return false;
			}
			this.ReflectStringParametersMessage();
			base.OperationBinding.Input.Extensions.Add(new HttpUrlEncodedBinding());
			return true;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001AD30 File Offset: 0x00019D30
		internal void ReflectStringParametersMessage()
		{
			Message inputMessage = base.InputMessage;
			foreach (ParameterInfo parameterInfo in base.Method.InParameters)
			{
				MessagePart messagePart = new MessagePart();
				messagePart.Name = XmlConvert.EncodeLocalName(parameterInfo.Name);
				if (parameterInfo.ParameterType.IsArray)
				{
					string text = base.DefaultNamespace;
					if (text.EndsWith("/", StringComparison.Ordinal))
					{
						text += "AbstractTypes";
					}
					else
					{
						text += "/AbstractTypes";
					}
					string text2 = "StringArray";
					if (!base.ServiceDescription.Types.Schemas.Contains(text))
					{
						XmlSchema xmlSchema = new XmlSchema();
						xmlSchema.TargetNamespace = text;
						base.ServiceDescription.Types.Schemas.Add(xmlSchema);
						XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
						xmlSchemaElement.Name = "String";
						xmlSchemaElement.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
						xmlSchemaElement.MinOccurs = 0m;
						xmlSchemaElement.MaxOccurs = decimal.MaxValue;
						XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
						xmlSchemaSequence.Items.Add(xmlSchemaElement);
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = new XmlSchemaComplexContentRestriction();
						xmlSchemaComplexContentRestriction.BaseTypeName = new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
						xmlSchemaComplexContentRestriction.Particle = xmlSchemaSequence;
						XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
						xmlSchemaImport.Namespace = xmlSchemaComplexContentRestriction.BaseTypeName.Namespace;
						XmlSchemaComplexContent xmlSchemaComplexContent = new XmlSchemaComplexContent();
						xmlSchemaComplexContent.Content = xmlSchemaComplexContentRestriction;
						XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
						xmlSchemaComplexType.Name = text2;
						xmlSchemaComplexType.ContentModel = xmlSchemaComplexContent;
						xmlSchema.Items.Add(xmlSchemaComplexType);
						xmlSchema.Includes.Add(xmlSchemaImport);
					}
					messagePart.Type = new XmlQualifiedName(text2, text);
				}
				else
				{
					messagePart.Type = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
				}
				inputMessage.Parts.Add(messagePart);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001AF28 File Offset: 0x00019F28
		internal string MethodUrl
		{
			get
			{
				WebMethodAttribute methodAttribute = base.Method.MethodAttribute;
				string text = methodAttribute.MessageName;
				if (text.Length == 0)
				{
					text = base.Method.Name;
				}
				return "/" + text;
			}
		}

		// Token: 0x0400040E RID: 1038
		private MimeReflector[] reflectors;
	}
}
