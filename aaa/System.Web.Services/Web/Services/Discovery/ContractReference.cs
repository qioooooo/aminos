using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Services.Description;
using System.Web.Services.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x02000096 RID: 150
	[XmlRoot("contractRef", Namespace = "http://schemas.xmlsoap.org/disco/scl/")]
	public class ContractReference : DiscoveryReference
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0001346A File Offset: 0x0001246A
		public ContractReference()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00013472 File Offset: 0x00012472
		public ContractReference(string href)
		{
			this.Ref = href;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00013481 File Offset: 0x00012481
		public ContractReference(string href, string docRef)
		{
			this.Ref = href;
			this.DocRef = docRef;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00013497 File Offset: 0x00012497
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0001349F File Offset: 0x0001249F
		[XmlAttribute("ref")]
		public string Ref
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000134A8 File Offset: 0x000124A8
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x000134B0 File Offset: 0x000124B0
		[XmlAttribute("docRef")]
		public string DocRef
		{
			get
			{
				return this.docRef;
			}
			set
			{
				this.docRef = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000134B9 File Offset: 0x000124B9
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000134C1 File Offset: 0x000124C1
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

		// Token: 0x060003F1 RID: 1009 RVA: 0x000134CC File Offset: 0x000124CC
		internal override void LoadExternals(Hashtable loadedExternals)
		{
			ServiceDescription serviceDescription = null;
			try
			{
				serviceDescription = this.Contract;
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
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "LoadExternals", ex);
				}
			}
			catch
			{
				base.ClientProtocol.Errors[this.Url] = new Exception(Res.GetString("NonClsCompliantException"));
			}
			if (serviceDescription != null)
			{
				foreach (object obj in this.Contract.Types.Schemas)
				{
					XmlSchema xmlSchema = (XmlSchema)obj;
					SchemaReference.LoadExternals(xmlSchema, this.Url, base.ClientProtocol, loadedExternals);
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000135D8 File Offset: 0x000125D8
		[XmlIgnore]
		public ServiceDescription Contract
		{
			get
			{
				if (base.ClientProtocol == null)
				{
					throw new InvalidOperationException(Res.GetString("WebMissingClientProtocol"));
				}
				object obj = base.ClientProtocol.Documents[this.Url];
				if (obj == null)
				{
					base.Resolve();
					obj = base.ClientProtocol.Documents[this.Url];
				}
				ServiceDescription serviceDescription = obj as ServiceDescription;
				if (serviceDescription == null)
				{
					throw new InvalidOperationException(Res.GetString("WebInvalidDocType", new object[]
					{
						typeof(ServiceDescription).FullName,
						(obj == null) ? string.Empty : obj.GetType().FullName,
						this.Url
					}));
				}
				return serviceDescription;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0001368C File Offset: 0x0001268C
		[XmlIgnore]
		public override string DefaultFilename
		{
			get
			{
				string text = DiscoveryReference.MakeValidFilename(this.Contract.Name);
				if (text == null || text.Length == 0)
				{
					text = DiscoveryReference.FilenameFromUrl(this.Url);
				}
				return Path.ChangeExtension(text, ".wsdl");
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000136CC File Offset: 0x000126CC
		public override void WriteDocument(object document, Stream stream)
		{
			((ServiceDescription)document).Write(new StreamWriter(stream, new UTF8Encoding(false)));
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000136E5 File Offset: 0x000126E5
		public override object ReadDocument(Stream stream)
		{
			return ServiceDescription.Read(stream, true);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000136F0 File Offset: 0x000126F0
		protected internal override void Resolve(string contentType, Stream stream)
		{
			if (ContentType.IsHtml(contentType))
			{
				throw new InvalidContentTypeException(Res.GetString("WebInvalidContentType", new object[] { contentType }), contentType);
			}
			ServiceDescription serviceDescription = base.ClientProtocol.Documents[this.Url] as ServiceDescription;
			if (serviceDescription == null)
			{
				serviceDescription = ServiceDescription.Read(stream, true);
				serviceDescription.RetrievalUrl = this.Url;
				base.ClientProtocol.Documents[this.Url] = serviceDescription;
			}
			base.ClientProtocol.References[this.Url] = this;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in serviceDescription.Imports)
			{
				Import import = (Import)obj;
				if (import.Location != null)
				{
					arrayList.Add(import.Location);
				}
			}
			foreach (object obj2 in serviceDescription.Types.Schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj2;
				foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Includes)
				{
					XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
					if (xmlSchemaExternal.SchemaLocation != null && xmlSchemaExternal.SchemaLocation.Length > 0)
					{
						arrayList.Add(xmlSchemaExternal.SchemaLocation);
					}
				}
			}
			foreach (object obj3 in arrayList)
			{
				string text = (string)obj3;
				string text2 = DiscoveryReference.UriToString(this.Url, text);
				if (base.ClientProtocol.Documents[text2] == null)
				{
					string text3 = text2;
					try
					{
						stream = base.ClientProtocol.Download(ref text2, ref contentType);
						try
						{
							if (base.ClientProtocol.Documents[text2] == null)
							{
								XmlTextReader xmlTextReader = new XmlTextReader(new StreamReader(stream, RequestResponseUtils.GetEncoding(contentType)));
								xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
								xmlTextReader.XmlResolver = null;
								xmlTextReader.ProhibitDtd = true;
								if (ServiceDescription.CanRead(xmlTextReader))
								{
									ServiceDescription serviceDescription2 = ServiceDescription.Read(xmlTextReader, true);
									serviceDescription2.RetrievalUrl = text2;
									base.ClientProtocol.Documents[text2] = serviceDescription2;
									ContractReference contractReference = new ContractReference(text2, null);
									contractReference.ClientProtocol = base.ClientProtocol;
									try
									{
										contractReference.Resolve(contentType, stream);
										continue;
									}
									catch (Exception ex)
									{
										if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
										{
											throw;
										}
										contractReference.Url = text3;
										if (Tracing.On)
										{
											Tracing.ExceptionCatch(TraceEventType.Warning, this, "Resolve", ex);
										}
										continue;
									}
									catch
									{
										contractReference.Url = text3;
										continue;
									}
								}
								if (xmlTextReader.IsStartElement("schema", "http://www.w3.org/2001/XMLSchema"))
								{
									base.ClientProtocol.Documents[text2] = XmlSchema.Read(xmlTextReader, null);
									SchemaReference schemaReference = new SchemaReference(text2);
									schemaReference.ClientProtocol = base.ClientProtocol;
									try
									{
										schemaReference.Resolve(contentType, stream);
									}
									catch (Exception ex2)
									{
										if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
										{
											throw;
										}
										schemaReference.Url = text3;
										if (Tracing.On)
										{
											Tracing.ExceptionCatch(TraceEventType.Warning, this, "Resolve", ex2);
										}
									}
									catch
									{
										schemaReference.Url = text3;
									}
								}
							}
						}
						finally
						{
							stream.Close();
						}
					}
					catch (Exception ex3)
					{
						if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
						{
							throw;
						}
						throw new InvalidDocumentContentsException(Res.GetString("TheWSDLDocumentContainsLinksThatCouldNotBeResolved", new object[] { text2 }), ex3);
					}
					catch
					{
						throw new InvalidDocumentContentsException(Res.GetString("TheWSDLDocumentContainsLinksThatCouldNotBeResolved", new object[] { text2 }), null);
					}
				}
			}
		}

		// Token: 0x04000399 RID: 921
		public const string Namespace = "http://schemas.xmlsoap.org/disco/scl/";

		// Token: 0x0400039A RID: 922
		private string docRef;

		// Token: 0x0400039B RID: 923
		private string reference;
	}
}
