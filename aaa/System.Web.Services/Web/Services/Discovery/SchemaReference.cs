using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Services.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000B2 RID: 178
	[XmlRoot("schemaRef", Namespace = "http://schemas.xmlsoap.org/disco/schema/")]
	public sealed class SchemaReference : DiscoveryReference
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x000177E5 File Offset: 0x000167E5
		public SchemaReference()
		{
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000177ED File Offset: 0x000167ED
		public SchemaReference(string url)
		{
			this.Ref = url;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x000177FC File Offset: 0x000167FC
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x00017812 File Offset: 0x00016812
		[XmlAttribute("ref")]
		public string Ref
		{
			get
			{
				if (this.reference != null)
				{
					return this.reference;
				}
				return "";
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001781B File Offset: 0x0001681B
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00017823 File Offset: 0x00016823
		[DefaultValue(null)]
		[XmlAttribute("targetNamespace")]
		public string TargetNamespace
		{
			get
			{
				return this.targetNamespace;
			}
			set
			{
				this.targetNamespace = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001782C File Offset: 0x0001682C
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00017834 File Offset: 0x00016834
		[XmlIgnore]
		public override string Url
		{
			get
			{
				return this.Ref;
			}
			set
			{
				this.Ref = value;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00017840 File Offset: 0x00016840
		internal XmlSchema GetSchema()
		{
			try
			{
				return this.Schema;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				base.ClientProtocol.Errors[this.Url] = ex;
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "GetSchema", ex);
				}
			}
			catch
			{
				base.ClientProtocol.Errors[this.Url] = new Exception(Res.GetString("NonClsCompliantException"));
			}
			return null;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000178E8 File Offset: 0x000168E8
		internal override void LoadExternals(Hashtable loadedExternals)
		{
			SchemaReference.LoadExternals(this.GetSchema(), this.Url, base.ClientProtocol, loadedExternals);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017904 File Offset: 0x00016904
		internal static void LoadExternals(XmlSchema schema, string url, DiscoveryClientProtocol client, Hashtable loadedExternals)
		{
			if (schema == null)
			{
				return;
			}
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				if (xmlSchemaExternal.SchemaLocation != null && xmlSchemaExternal.SchemaLocation.Length != 0 && xmlSchemaExternal.Schema == null && (xmlSchemaExternal is XmlSchemaInclude || xmlSchemaExternal is XmlSchemaRedefine))
				{
					string text = DiscoveryReference.UriToString(url, xmlSchemaExternal.SchemaLocation);
					if (client.References[text] is SchemaReference)
					{
						SchemaReference schemaReference = (SchemaReference)client.References[text];
						xmlSchemaExternal.Schema = schemaReference.GetSchema();
						if (xmlSchemaExternal.Schema != null)
						{
							loadedExternals[text] = xmlSchemaExternal.Schema;
						}
						schemaReference.LoadExternals(loadedExternals);
					}
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000179EC File Offset: 0x000169EC
		public override void WriteDocument(object document, Stream stream)
		{
			((XmlSchema)document).Write(new StreamWriter(stream, new UTF8Encoding(false)));
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00017A08 File Offset: 0x00016A08
		public override object ReadDocument(Stream stream)
		{
			return XmlSchema.Read(new XmlTextReader(this.Url, stream)
			{
				XmlResolver = null
			}, null);
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00017A30 File Offset: 0x00016A30
		[XmlIgnore]
		public override string DefaultFilename
		{
			get
			{
				string text = DiscoveryReference.MakeValidFilename(this.Schema.Id);
				if (text == null || text.Length == 0)
				{
					text = DiscoveryReference.FilenameFromUrl(this.Url);
				}
				return Path.ChangeExtension(text, ".xsd");
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00017A70 File Offset: 0x00016A70
		[XmlIgnore]
		public XmlSchema Schema
		{
			get
			{
				if (base.ClientProtocol == null)
				{
					throw new InvalidOperationException(Res.GetString("WebMissingClientProtocol"));
				}
				object obj = base.ClientProtocol.InlinedSchemas[this.Url];
				if (obj == null)
				{
					obj = base.ClientProtocol.Documents[this.Url];
				}
				if (obj == null)
				{
					base.Resolve();
					obj = base.ClientProtocol.Documents[this.Url];
				}
				XmlSchema xmlSchema = obj as XmlSchema;
				if (xmlSchema == null)
				{
					throw new InvalidOperationException(Res.GetString("WebInvalidDocType", new object[]
					{
						typeof(XmlSchema).FullName,
						(obj == null) ? string.Empty : obj.GetType().FullName,
						this.Url
					}));
				}
				return xmlSchema;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00017B3C File Offset: 0x00016B3C
		protected internal override void Resolve(string contentType, Stream stream)
		{
			if (ContentType.IsHtml(contentType))
			{
				base.ClientProtocol.Errors[this.Url] = new InvalidContentTypeException(Res.GetString("WebInvalidContentType", new object[] { contentType }), contentType);
			}
			XmlSchema xmlSchema = base.ClientProtocol.Documents[this.Url] as XmlSchema;
			if (xmlSchema == null)
			{
				if (base.ClientProtocol.Errors[this.Url] != null)
				{
					throw base.ClientProtocol.Errors[this.Url];
				}
				xmlSchema = (XmlSchema)this.ReadDocument(stream);
				base.ClientProtocol.Documents[this.Url] = xmlSchema;
			}
			if (base.ClientProtocol.References[this.Url] != this)
			{
				base.ClientProtocol.References[this.Url] = this;
			}
			foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				string text = null;
				try
				{
					if (xmlSchemaExternal.SchemaLocation != null && xmlSchemaExternal.SchemaLocation.Length > 0)
					{
						text = DiscoveryReference.UriToString(this.Url, xmlSchemaExternal.SchemaLocation);
						SchemaReference schemaReference = new SchemaReference(text);
						schemaReference.ClientProtocol = base.ClientProtocol;
						base.ClientProtocol.References[text] = schemaReference;
						schemaReference.Resolve();
					}
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					throw new InvalidDocumentContentsException(Res.GetString("TheSchemaDocumentContainsLinksThatCouldNotBeResolved", new object[] { text }), ex);
				}
				catch
				{
					throw new InvalidDocumentContentsException(Res.GetString("TheSchemaDocumentContainsLinksThatCouldNotBeResolved", new object[] { text }), null);
				}
			}
		}

		// Token: 0x040003D8 RID: 984
		public const string Namespace = "http://schemas.xmlsoap.org/disco/schema/";

		// Token: 0x040003D9 RID: 985
		private string reference;

		// Token: 0x040003DA RID: 986
		private string targetNamespace;
	}
}
