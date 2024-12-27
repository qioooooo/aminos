using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200012A RID: 298
	internal class WebReferenceOptionsSerializationWriter : XmlSerializationWriter
	{
		// Token: 0x0600091C RID: 2332 RVA: 0x00042928 File Offset: 0x00041928
		private string Write1_CodeGenerationOptions(CodeGenerationOptions v)
		{
			switch (v)
			{
			case CodeGenerationOptions.GenerateProperties:
				return "properties";
			case CodeGenerationOptions.GenerateNewAsync:
				return "newAsync";
			case CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync:
				break;
			case CodeGenerationOptions.GenerateOldAsync:
				return "oldAsync";
			default:
				if (v == CodeGenerationOptions.GenerateOrder)
				{
					return "order";
				}
				if (v == CodeGenerationOptions.EnableDataBinding)
				{
					return "enableDataBinding";
				}
				break;
			}
			return XmlSerializationWriter.FromEnum((long)v, new string[] { "properties", "newAsync", "oldAsync", "order", "enableDataBinding" }, new long[] { 1L, 2L, 4L, 8L, 16L }, "System.Xml.Serialization.CodeGenerationOptions");
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x000429D4 File Offset: 0x000419D4
		private string Write2_ServiceDescriptionImportStyle(ServiceDescriptionImportStyle v)
		{
			string text;
			switch (v)
			{
			case ServiceDescriptionImportStyle.Client:
				text = "client";
				break;
			case ServiceDescriptionImportStyle.Server:
				text = "server";
				break;
			case ServiceDescriptionImportStyle.ServerInterface:
				text = "serverInterface";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "System.Web.Services.Description.ServiceDescriptionImportStyle");
			}
			return text;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00042A30 File Offset: 0x00041A30
		private void Write4_WebReferenceOptions(string n, string ns, WebReferenceOptions o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (type != typeof(WebReferenceOptions))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.EscapeName = false;
			base.WriteStartElement(n, ns, o);
			if (needType)
			{
				base.WriteXsiType("webReferenceOptions", "http://microsoft.com/webReference/");
			}
			if (o.CodeGenerationOptions != CodeGenerationOptions.GenerateOldAsync)
			{
				base.WriteElementString("codeGenerationOptions", "http://microsoft.com/webReference/", this.Write1_CodeGenerationOptions(o.CodeGenerationOptions));
			}
			StringCollection schemaImporterExtensions = o.SchemaImporterExtensions;
			if (schemaImporterExtensions != null)
			{
				base.WriteStartElement("schemaImporterExtensions", "http://microsoft.com/webReference/");
				for (int i = 0; i < schemaImporterExtensions.Count; i++)
				{
					base.WriteNullableStringLiteral("type", "http://microsoft.com/webReference/", schemaImporterExtensions[i]);
				}
				base.WriteEndElement();
			}
			if (o.Style != ServiceDescriptionImportStyle.Client)
			{
				base.WriteElementString("style", "http://microsoft.com/webReference/", this.Write2_ServiceDescriptionImportStyle(o.Style));
			}
			base.WriteElementStringRaw("verbose", "http://microsoft.com/webReference/", XmlConvert.ToString(o.Verbose));
			base.WriteEndElement(o);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00042B44 File Offset: 0x00041B44
		protected override void InitCallbacks()
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00042B46 File Offset: 0x00041B46
		internal void Write5_webReferenceOptions(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("webReferenceOptions", "http://microsoft.com/webReference/");
				return;
			}
			base.TopLevelElement();
			this.Write4_WebReferenceOptions("webReferenceOptions", "http://microsoft.com/webReference/", (WebReferenceOptions)o, true, false);
		}
	}
}
