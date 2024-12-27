using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x0200009A RID: 154
	public class DiscoveryClientProtocol : HttpWebClientProtocol
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x00013C94 File Offset: 0x00012C94
		public DiscoveryClientProtocol()
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00013CD3 File Offset: 0x00012CD3
		internal DiscoveryClientProtocol(HttpWebClientProtocol protocol)
			: base(protocol)
		{
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00013D13 File Offset: 0x00012D13
		public IList AdditionalInformation
		{
			get
			{
				return this.additionalInformation;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00013D1B File Offset: 0x00012D1B
		public DiscoveryClientDocumentCollection Documents
		{
			get
			{
				return this.documents;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00013D23 File Offset: 0x00012D23
		public DiscoveryExceptionDictionary Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00013D2B File Offset: 0x00012D2B
		public DiscoveryClientReferenceCollection References
		{
			get
			{
				return this.references;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00013D33 File Offset: 0x00012D33
		internal Hashtable InlinedSchemas
		{
			get
			{
				return this.inlinedSchemas;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00013D3C File Offset: 0x00012D3C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public DiscoveryDocument Discover(string url)
		{
			DiscoveryDocument discoveryDocument = this.Documents[url] as DiscoveryDocument;
			if (discoveryDocument != null)
			{
				return discoveryDocument;
			}
			DiscoveryDocumentReference discoveryDocumentReference = new DiscoveryDocumentReference(url);
			discoveryDocumentReference.ClientProtocol = this;
			this.References[url] = discoveryDocumentReference;
			this.Errors.Clear();
			return discoveryDocumentReference.Document;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00013D8C File Offset: 0x00012D8C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public DiscoveryDocument DiscoverAny(string url)
		{
			Type[] discoveryReferenceTypes = WebServicesSection.Current.DiscoveryReferenceTypes;
			DiscoveryReference discoveryReference = null;
			string text = null;
			Stream stream = this.Download(ref url, ref text);
			this.Errors.Clear();
			bool flag = true;
			Exception ex = null;
			ArrayList arrayList = new ArrayList();
			foreach (Type type in discoveryReferenceTypes)
			{
				if (typeof(DiscoveryReference).IsAssignableFrom(type))
				{
					discoveryReference = (DiscoveryReference)Activator.CreateInstance(type);
					discoveryReference.Url = url;
					discoveryReference.ClientProtocol = this;
					stream.Position = 0L;
					Exception ex2 = discoveryReference.AttemptResolve(text, stream);
					if (ex2 == null)
					{
						break;
					}
					this.Errors[type.FullName] = ex2;
					discoveryReference = null;
					InvalidContentTypeException ex3 = ex2 as InvalidContentTypeException;
					if (ex3 == null || !ContentType.MatchesBase(ex3.ContentType, "text/html"))
					{
						flag = false;
					}
					InvalidDocumentContentsException ex4 = ex2 as InvalidDocumentContentsException;
					if (ex4 != null)
					{
						ex = ex2;
						break;
					}
					if (ex2.InnerException != null && ex2.InnerException.InnerException == null)
					{
						arrayList.Add(ex2.InnerException.Message);
					}
				}
			}
			if (discoveryReference == null)
			{
				if (ex != null)
				{
					StringBuilder stringBuilder = new StringBuilder(Res.GetString("TheDocumentWasUnderstoodButContainsErrors"));
					while (ex != null)
					{
						stringBuilder.Append("\n  - ").Append(ex.Message);
						ex = ex.InnerException;
					}
					throw new InvalidOperationException(stringBuilder.ToString());
				}
				if (flag)
				{
					throw new InvalidOperationException(Res.GetString("TheHTMLDocumentDoesNotContainDiscoveryInformation"));
				}
				bool flag2 = arrayList.Count == this.Errors.Count && this.Errors.Count > 0;
				int num = 1;
				while (flag2 && num < arrayList.Count)
				{
					if ((string)arrayList[num - 1] != (string)arrayList[num])
					{
						flag2 = false;
					}
					num++;
				}
				if (flag2)
				{
					throw new InvalidOperationException(Res.GetString("TheDocumentWasNotRecognizedAsAKnownDocumentType", new object[] { arrayList[0] }));
				}
				StringBuilder stringBuilder2 = new StringBuilder(Res.GetString("WebMissingResource", new object[] { url }));
				foreach (object obj in this.Errors)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					Exception ex5 = (Exception)dictionaryEntry.Value;
					string text2 = (string)dictionaryEntry.Key;
					if (string.Compare(text2, typeof(ContractReference).FullName, StringComparison.Ordinal) == 0)
					{
						text2 = Res.GetString("WebContractReferenceName");
					}
					else if (string.Compare(text2, typeof(SchemaReference).FullName, StringComparison.Ordinal) == 0)
					{
						text2 = Res.GetString("WebShemaReferenceName");
					}
					else if (string.Compare(text2, typeof(DiscoveryDocumentReference).FullName, StringComparison.Ordinal) == 0)
					{
						text2 = Res.GetString("WebDiscoveryDocumentReferenceName");
					}
					stringBuilder2.Append("\n- ").Append(Res.GetString("WebDiscoRefReport", new object[] { text2, ex5.Message }));
					while (ex5.InnerException != null)
					{
						stringBuilder2.Append("\n  - ").Append(ex5.InnerException.Message);
						ex5 = ex5.InnerException;
					}
				}
				throw new InvalidOperationException(stringBuilder2.ToString());
			}
			else
			{
				if (discoveryReference is DiscoveryDocumentReference)
				{
					return ((DiscoveryDocumentReference)discoveryReference).Document;
				}
				this.References[discoveryReference.Url] = discoveryReference;
				return new DiscoveryDocument
				{
					References = { discoveryReference }
				};
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00014168 File Offset: 0x00013168
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public Stream Download(ref string url)
		{
			string text = null;
			return this.Download(ref url, ref text);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00014180 File Offset: 0x00013180
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public Stream Download(ref string url, ref string contentType)
		{
			WebRequest webRequest = this.GetWebRequest(new Uri(url));
			webRequest.Method = "GET";
			WebResponse webResponse = null;
			try
			{
				webResponse = this.GetWebResponse(webRequest);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new WebException(Res.GetString("ThereWasAnErrorDownloading0", new object[] { url }), ex);
			}
			catch
			{
				throw new WebException(Res.GetString("ThereWasAnErrorDownloading0", new object[] { url }), null);
			}
			HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
			if (httpWebResponse != null && httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				string text = RequestResponseUtils.CreateResponseExceptionString(httpWebResponse);
				throw new WebException(Res.GetString("ThereWasAnErrorDownloading0", new object[] { url }), new WebException(text, null, WebExceptionStatus.ProtocolError, webResponse));
			}
			Stream responseStream = webResponse.GetResponseStream();
			Stream stream;
			try
			{
				url = webResponse.ResponseUri.ToString();
				contentType = webResponse.ContentType;
				if (webResponse.ResponseUri.Scheme == Uri.UriSchemeFtp || webResponse.ResponseUri.Scheme == Uri.UriSchemeFile)
				{
					int num = webResponse.ResponseUri.AbsolutePath.LastIndexOf('.');
					string text2;
					if (num != -1 && (text2 = webResponse.ResponseUri.AbsolutePath.Substring(num + 1).ToLower(CultureInfo.InvariantCulture)) != null && (text2 == "xml" || text2 == "wsdl" || text2 == "xsd" || text2 == "disco"))
					{
						contentType = "text/xml";
					}
				}
				stream = RequestResponseUtils.StreamToMemoryStream(responseStream);
			}
			finally
			{
				responseStream.Close();
			}
			return stream;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001435C File Offset: 0x0001335C
		[Obsolete("This method will be removed from a future version. The method call is no longer required for resource discovery", false)]
		[ComVisible(false)]
		public void LoadExternals()
		{
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00014360 File Offset: 0x00013360
		internal void FixupReferences()
		{
			foreach (object obj in this.References.Values)
			{
				DiscoveryReference discoveryReference = (DiscoveryReference)obj;
				discoveryReference.LoadExternals(this.InlinedSchemas);
			}
			foreach (object obj2 in this.InlinedSchemas.Keys)
			{
				string text = (string)obj2;
				this.Documents.Remove(text);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001441C File Offset: 0x0001341C
		private static bool IsFilenameInUse(Hashtable filenames, string path)
		{
			return filenames[path.ToLower(CultureInfo.InvariantCulture)] != null;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00014435 File Offset: 0x00013435
		private static void AddFilename(Hashtable filenames, string path)
		{
			filenames.Add(path.ToLower(CultureInfo.InvariantCulture), path);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001444C File Offset: 0x0001344C
		private static string GetUniqueFilename(Hashtable filenames, string path)
		{
			if (DiscoveryClientProtocol.IsFilenameInUse(filenames, path))
			{
				string extension = Path.GetExtension(path);
				string text = path.Substring(0, path.Length - extension.Length);
				int num = 0;
				do
				{
					path = text + num.ToString(CultureInfo.InvariantCulture) + extension;
					num++;
				}
				while (DiscoveryClientProtocol.IsFilenameInUse(filenames, path));
			}
			DiscoveryClientProtocol.AddFilename(filenames, path);
			return path;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000144AC File Offset: 0x000134AC
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public DiscoveryClientResultCollection ReadAll(string topLevelFilename)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiscoveryClientProtocol.DiscoveryClientResultsFile));
			Stream stream = File.OpenRead(topLevelFilename);
			string directoryName = Path.GetDirectoryName(topLevelFilename);
			DiscoveryClientProtocol.DiscoveryClientResultsFile discoveryClientResultsFile = null;
			try
			{
				discoveryClientResultsFile = (DiscoveryClientProtocol.DiscoveryClientResultsFile)xmlSerializer.Deserialize(stream);
				for (int i = 0; i < discoveryClientResultsFile.Results.Count; i++)
				{
					if (discoveryClientResultsFile.Results[i] == null)
					{
						throw new InvalidOperationException(Res.GetString("WebNullRef"));
					}
					string referenceTypeName = discoveryClientResultsFile.Results[i].ReferenceTypeName;
					if (referenceTypeName == null || referenceTypeName.Length == 0)
					{
						throw new InvalidOperationException(Res.GetString("WebRefInvalidAttribute", new object[] { "referenceType" }));
					}
					DiscoveryReference discoveryReference = (DiscoveryReference)Activator.CreateInstance(Type.GetType(referenceTypeName));
					discoveryReference.ClientProtocol = this;
					string url = discoveryClientResultsFile.Results[i].Url;
					if (url == null || url.Length == 0)
					{
						throw new InvalidOperationException(Res.GetString("WebRefInvalidAttribute2", new object[]
						{
							discoveryReference.GetType().FullName,
							"url"
						}));
					}
					discoveryReference.Url = url;
					string filename = discoveryClientResultsFile.Results[i].Filename;
					if (filename == null || filename.Length == 0)
					{
						throw new InvalidOperationException(Res.GetString("WebRefInvalidAttribute2", new object[]
						{
							discoveryReference.GetType().FullName,
							"filename"
						}));
					}
					Stream stream2 = File.OpenRead(Path.Combine(directoryName, discoveryClientResultsFile.Results[i].Filename));
					try
					{
						this.Documents[discoveryReference.Url] = discoveryReference.ReadDocument(stream2);
					}
					finally
					{
						stream2.Close();
					}
					this.References[discoveryReference.Url] = discoveryReference;
				}
				this.ResolveAll();
			}
			finally
			{
				stream.Close();
			}
			return discoveryClientResultsFile.Results;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000146D8 File Offset: 0x000136D8
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void ResolveAll()
		{
			this.Errors.Clear();
			int num = this.InlinedSchemas.Keys.Count;
			while (num != this.References.Count)
			{
				num = this.References.Count;
				DiscoveryReference[] array = new DiscoveryReference[this.References.Count];
				this.References.Values.CopyTo(array, 0);
				int i = 0;
				while (i < array.Length)
				{
					DiscoveryReference discoveryReference = array[i];
					if (discoveryReference is DiscoveryDocumentReference)
					{
						try
						{
							((DiscoveryDocumentReference)discoveryReference).ResolveAll(true);
							goto IL_015C;
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							this.Errors[discoveryReference.Url] = ex;
							if (Tracing.On)
							{
								Tracing.ExceptionCatch(TraceEventType.Warning, this, "ResolveAll", ex);
							}
							goto IL_015C;
						}
						catch
						{
							this.Errors[discoveryReference.Url] = new Exception(Res.GetString("NonClsCompliantException"));
							goto IL_015C;
						}
						goto Block_2;
					}
					goto IL_00E7;
					IL_015C:
					i++;
					continue;
					Block_2:
					try
					{
						IL_00E7:
						discoveryReference.Resolve();
					}
					catch (Exception ex2)
					{
						if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
						{
							throw;
						}
						this.Errors[discoveryReference.Url] = ex2;
						if (Tracing.On)
						{
							Tracing.ExceptionCatch(TraceEventType.Warning, this, "ResolveAll", ex2);
						}
					}
					catch
					{
						this.Errors[discoveryReference.Url] = new Exception(Res.GetString("NonClsCompliantException"));
					}
					goto IL_015C;
				}
			}
			this.FixupReferences();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001489C File Offset: 0x0001389C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void ResolveOneLevel()
		{
			this.Errors.Clear();
			DiscoveryReference[] array = new DiscoveryReference[this.References.Count];
			this.References.Values.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					array[i].Resolve();
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					this.Errors[array[i].Url] = ex;
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "ResolveOneLevel", ex);
					}
				}
				catch
				{
					this.Errors[array[i].Url] = new Exception(Res.GetString("NonClsCompliantException"));
				}
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00014978 File Offset: 0x00013978
		private static string GetRelativePath(string fullPath, string relativeTo)
		{
			string text = Path.GetDirectoryName(Path.GetFullPath(relativeTo));
			string text2 = "";
			while (text.Length > 0)
			{
				if (text.Length <= fullPath.Length && string.Compare(text, fullPath.Substring(0, text.Length), StringComparison.OrdinalIgnoreCase) == 0)
				{
					text2 += fullPath.Substring(text.Length);
					if (text2.StartsWith("\\", StringComparison.Ordinal))
					{
						text2 = text2.Substring(1);
					}
					return text2;
				}
				text2 += "..\\";
				if (text.Length < 2)
				{
					break;
				}
				int num = text.LastIndexOf('\\', text.Length - 2);
				text = text.Substring(0, num + 1);
			}
			return fullPath;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00014A2C File Offset: 0x00013A2C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public DiscoveryClientResultCollection WriteAll(string directory, string topLevelFilename)
		{
			DiscoveryClientProtocol.DiscoveryClientResultsFile discoveryClientResultsFile = new DiscoveryClientProtocol.DiscoveryClientResultsFile();
			Hashtable hashtable = new Hashtable();
			string text = Path.Combine(directory, topLevelFilename);
			DictionaryEntry[] array = new DictionaryEntry[this.Documents.Count + this.InlinedSchemas.Keys.Count];
			int num = 0;
			foreach (object obj in this.Documents)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				array[num++] = dictionaryEntry;
			}
			foreach (object obj2 in this.InlinedSchemas)
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				array[num++] = dictionaryEntry2;
			}
			foreach (DictionaryEntry dictionaryEntry3 in array)
			{
				string text2 = (string)dictionaryEntry3.Key;
				object value = dictionaryEntry3.Value;
				if (value != null)
				{
					DiscoveryReference discoveryReference = this.References[text2];
					string text3 = ((discoveryReference == null) ? DiscoveryReference.FilenameFromUrl(base.Url) : discoveryReference.DefaultFilename);
					text3 = DiscoveryClientProtocol.GetUniqueFilename(hashtable, Path.GetFullPath(Path.Combine(directory, text3)));
					discoveryClientResultsFile.Results.Add(new DiscoveryClientResult((discoveryReference == null) ? null : discoveryReference.GetType(), text2, DiscoveryClientProtocol.GetRelativePath(text3, text)));
					Stream stream = File.Create(text3);
					try
					{
						discoveryReference.WriteDocument(value, stream);
					}
					finally
					{
						stream.Close();
					}
				}
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiscoveryClientProtocol.DiscoveryClientResultsFile));
			Stream stream2 = File.Create(text);
			try
			{
				xmlSerializer.Serialize(new StreamWriter(stream2, new UTF8Encoding(false)), discoveryClientResultsFile);
			}
			finally
			{
				stream2.Close();
			}
			return discoveryClientResultsFile.Results;
		}

		// Token: 0x0400039C RID: 924
		private DiscoveryClientReferenceCollection references = new DiscoveryClientReferenceCollection();

		// Token: 0x0400039D RID: 925
		private DiscoveryClientDocumentCollection documents = new DiscoveryClientDocumentCollection();

		// Token: 0x0400039E RID: 926
		private Hashtable inlinedSchemas = new Hashtable();

		// Token: 0x0400039F RID: 927
		private ArrayList additionalInformation = new ArrayList();

		// Token: 0x040003A0 RID: 928
		private DiscoveryExceptionDictionary errors = new DiscoveryExceptionDictionary();

		// Token: 0x0200009B RID: 155
		public sealed class DiscoveryClientResultsFile
		{
			// Token: 0x1700011E RID: 286
			// (get) Token: 0x0600041A RID: 1050 RVA: 0x00014C58 File Offset: 0x00013C58
			public DiscoveryClientResultCollection Results
			{
				get
				{
					return this.results;
				}
			}

			// Token: 0x040003A1 RID: 929
			private DiscoveryClientResultCollection results = new DiscoveryClientResultCollection();
		}
	}
}
