using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000068 RID: 104
	public class DsmlSoapHttpConnection : DsmlSoapConnection
	{
		// Token: 0x0600020B RID: 523 RVA: 0x00008CD1 File Offset: 0x00007CD1
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DsmlSoapHttpConnection(Uri uri)
			: this(new DsmlDirectoryIdentifier(uri))
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008CE0 File Offset: 0x00007CE0
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		public DsmlSoapHttpConnection(DsmlDirectoryIdentifier identifier)
		{
			if (identifier == null)
			{
				throw new ArgumentNullException("identifier");
			}
			this.directoryIdentifier = identifier;
			this.dsmlHttpConnection = (HttpWebRequest)WebRequest.Create(((DsmlDirectoryIdentifier)this.directoryIdentifier).ServerUri);
			Hashtable hashtable = new Hashtable();
			this.httpConnectionTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008D4C File Offset: 0x00007D4C
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public DsmlSoapHttpConnection(DsmlDirectoryIdentifier identifier, NetworkCredential credential)
			: this(identifier)
		{
			this.directoryCredential = ((credential != null) ? new NetworkCredential(credential.UserName, credential.Password, credential.Domain) : null);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008D78 File Offset: 0x00007D78
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DsmlSoapHttpConnection(DsmlDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType)
			: this(identifier, credential)
		{
			this.AuthType = authType;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008D89 File Offset: 0x00007D89
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00008D94 File Offset: 0x00007D94
		public override TimeSpan Timeout
		{
			get
			{
				return this.connectionTimeOut;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalMilliseconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.connectionTimeOut = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008DF1 File Offset: 0x00007DF1
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00008DF9 File Offset: 0x00007DF9
		public string SoapActionHeader
		{
			get
			{
				return this.dsmlSoapAction;
			}
			set
			{
				this.dsmlSoapAction = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008E02 File Offset: 0x00007E02
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00008E0C File Offset: 0x00007E0C
		public AuthType AuthType
		{
			get
			{
				return this.dsmlAuthType;
			}
			set
			{
				if (value < AuthType.Anonymous || value > AuthType.Kerberos)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AuthType));
				}
				if (value != AuthType.Anonymous && value != AuthType.Ntlm && value != AuthType.Basic && value != AuthType.Negotiate && value != AuthType.Digest)
				{
					throw new ArgumentException(Res.GetString("WrongAuthType", new object[] { value }), "value");
				}
				this.dsmlAuthType = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008E78 File Offset: 0x00007E78
		public override string SessionId
		{
			get
			{
				return this.dsmlSessionID;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00008E80 File Offset: 0x00007E80
		private string ResponseString
		{
			get
			{
				return this.debugResponse;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008E88 File Offset: 0x00007E88
		[NetworkInformationPermission(SecurityAction.Assert, Unrestricted = true)]
		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void BeginSession()
		{
			if (this.dsmlSessionID != null)
			{
				throw new InvalidOperationException(Res.GetString("SessionInUse"));
			}
			try
			{
				this.PrepareHttpWebRequest(this.dsmlHttpConnection);
				StreamWriter webRequestStreamWriter = this.GetWebRequestStreamWriter();
				try
				{
					webRequestStreamWriter.Write("<se:Envelope xmlns:se=\"http://schemas.xmlsoap.org/soap/envelope/\">");
					webRequestStreamWriter.Write("<se:Header>");
					webRequestStreamWriter.Write("<ad:BeginSession xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" se:mustUnderstand=\"1\"/>");
					if (this.soapHeaders != null)
					{
						webRequestStreamWriter.Write(this.soapHeaders.OuterXml);
					}
					webRequestStreamWriter.Write("</se:Header>");
					webRequestStreamWriter.Write("<se:Body xmlns=\"urn:oasis:names:tc:DSML:2:0:core\">");
					webRequestStreamWriter.Write(new DsmlRequestDocument().ToXml().InnerXml);
					webRequestStreamWriter.Write("</se:Body>");
					webRequestStreamWriter.Write("</se:Envelope>");
					webRequestStreamWriter.Flush();
				}
				finally
				{
					webRequestStreamWriter.BaseStream.Close();
					webRequestStreamWriter.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)this.dsmlHttpConnection.GetResponse();
				try
				{
					this.dsmlSessionID = this.ExtractSessionID(httpWebResponse);
				}
				finally
				{
					httpWebResponse.Close();
				}
			}
			finally
			{
				this.dsmlHttpConnection = (HttpWebRequest)WebRequest.Create(((DsmlDirectoryIdentifier)this.directoryIdentifier).ServerUri);
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008FC8 File Offset: 0x00007FC8
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[NetworkInformationPermission(SecurityAction.Assert, Unrestricted = true)]
		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		public override void EndSession()
		{
			if (this.dsmlSessionID == null)
			{
				throw new InvalidOperationException(Res.GetString("NoCurrentSession"));
			}
			try
			{
				try
				{
					this.PrepareHttpWebRequest(this.dsmlHttpConnection);
					StreamWriter webRequestStreamWriter = this.GetWebRequestStreamWriter();
					try
					{
						webRequestStreamWriter.Write("<se:Envelope xmlns:se=\"http://schemas.xmlsoap.org/soap/envelope/\">");
						webRequestStreamWriter.Write("<se:Header>");
						webRequestStreamWriter.Write("<ad:EndSession xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" ad:SessionID=\"");
						webRequestStreamWriter.Write(this.dsmlSessionID);
						webRequestStreamWriter.Write("\" se:mustUnderstand=\"1\"/>");
						if (this.soapHeaders != null)
						{
							webRequestStreamWriter.Write(this.soapHeaders.OuterXml);
						}
						webRequestStreamWriter.Write("</se:Header>");
						webRequestStreamWriter.Write("<se:Body xmlns=\"urn:oasis:names:tc:DSML:2:0:core\">");
						webRequestStreamWriter.Write(new DsmlRequestDocument().ToXml().InnerXml);
						webRequestStreamWriter.Write("</se:Body>");
						webRequestStreamWriter.Write("</se:Envelope>");
						webRequestStreamWriter.Flush();
					}
					finally
					{
						webRequestStreamWriter.BaseStream.Close();
						webRequestStreamWriter.Close();
					}
					HttpWebResponse httpWebResponse = (HttpWebResponse)this.dsmlHttpConnection.GetResponse();
					httpWebResponse.Close();
				}
				catch (WebException ex)
				{
					if (ex.Status != WebExceptionStatus.ConnectFailure && ex.Status != WebExceptionStatus.NameResolutionFailure && ex.Status != WebExceptionStatus.ProxyNameResolutionFailure && ex.Status != WebExceptionStatus.SendFailure && ex.Status != WebExceptionStatus.TrustFailure)
					{
						this.dsmlSessionID = null;
					}
					throw;
				}
				this.dsmlSessionID = null;
			}
			finally
			{
				this.dsmlHttpConnection = (HttpWebRequest)WebRequest.Create(((DsmlDirectoryIdentifier)this.directoryIdentifier).ServerUri);
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00009174 File Offset: 0x00008174
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override DirectoryResponse SendRequest(DirectoryRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			DsmlResponseDocument dsmlResponseDocument = this.SendRequestHelper(new DsmlRequestDocument { request }.ToXml().InnerXml);
			if (dsmlResponseDocument.Count == 0)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("MissingResponse"));
			}
			DirectoryResponse directoryResponse = dsmlResponseDocument[0];
			if (directoryResponse is DsmlErrorResponse)
			{
				ErrorResponseException ex = new ErrorResponseException((DsmlErrorResponse)directoryResponse);
				throw ex;
			}
			ResultCode resultCode = directoryResponse.ResultCode;
			if (resultCode == ResultCode.Success || resultCode == ResultCode.CompareFalse || resultCode == ResultCode.CompareTrue || resultCode == ResultCode.Referral || resultCode == ResultCode.ReferralV2)
			{
				return directoryResponse;
			}
			throw new DirectoryOperationException(directoryResponse, OperationErrorMappings.MapResultCode((int)resultCode));
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009218 File Offset: 0x00008218
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public DsmlResponseDocument SendRequest(DsmlRequestDocument request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			DsmlResponseDocument dsmlResponseDocument = this.SendRequestHelper(request.ToXml().InnerXml);
			if (request.Count > 0 && dsmlResponseDocument.Count == 0)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("MissingResponse"));
			}
			return dsmlResponseDocument;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009268 File Offset: 0x00008268
		[NetworkInformationPermission(SecurityAction.Assert, Unrestricted = true)]
		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		private DsmlResponseDocument SendRequestHelper(string reqstring)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			DsmlResponseDocument dsmlResponseDocument2;
			try
			{
				this.PrepareHttpWebRequest(this.dsmlHttpConnection);
				StreamWriter webRequestStreamWriter = this.GetWebRequestStreamWriter();
				try
				{
					this.BeginSOAPRequest(ref stringBuilder);
					stringBuilder.Append(reqstring);
					this.EndSOAPRequest(ref stringBuilder);
					webRequestStreamWriter.Write(stringBuilder.ToString());
					webRequestStreamWriter.Flush();
				}
				finally
				{
					webRequestStreamWriter.BaseStream.Close();
					webRequestStreamWriter.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)this.dsmlHttpConnection.GetResponse();
				DsmlResponseDocument dsmlResponseDocument;
				try
				{
					dsmlResponseDocument = new DsmlResponseDocument(httpWebResponse, "se:Envelope/se:Body/dsml:batchResponse");
					this.debugResponse = dsmlResponseDocument.ResponseString;
				}
				finally
				{
					httpWebResponse.Close();
				}
				dsmlResponseDocument2 = dsmlResponseDocument;
			}
			finally
			{
				this.dsmlHttpConnection = (HttpWebRequest)WebRequest.Create(((DsmlDirectoryIdentifier)this.directoryIdentifier).ServerUri);
			}
			return dsmlResponseDocument2;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009354 File Offset: 0x00008354
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		private void PrepareHttpWebRequest(HttpWebRequest dsmlConnection)
		{
			if (this.directoryCredential == null)
			{
				dsmlConnection.Credentials = CredentialCache.DefaultCredentials;
			}
			else
			{
				string text = "negotiate";
				if (this.dsmlAuthType == AuthType.Ntlm)
				{
					text = "NTLM";
				}
				else if (this.dsmlAuthType == AuthType.Basic)
				{
					text = "basic";
				}
				else if (this.dsmlAuthType == AuthType.Anonymous)
				{
					text = "anonymous";
				}
				else if (this.dsmlAuthType == AuthType.Digest)
				{
					text = "digest";
				}
				dsmlConnection.Credentials = new CredentialCache { { dsmlConnection.RequestUri, text, this.directoryCredential } };
			}
			foreach (X509Certificate x509Certificate in base.ClientCertificates)
			{
				dsmlConnection.ClientCertificates.Add(x509Certificate);
			}
			if (this.connectionTimeOut.Ticks != 0L)
			{
				dsmlConnection.Timeout = (int)(this.connectionTimeOut.Ticks / 10000L);
			}
			if (this.dsmlSoapAction != null)
			{
				WebHeaderCollection headers = dsmlConnection.Headers;
				headers.Set("SOAPAction", this.dsmlSoapAction);
			}
			dsmlConnection.Method = "POST";
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009484 File Offset: 0x00008484
		private StreamWriter GetWebRequestStreamWriter()
		{
			Stream requestStream = this.dsmlHttpConnection.GetRequestStream();
			return new StreamWriter(requestStream);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000094A8 File Offset: 0x000084A8
		private void BeginSOAPRequest(ref StringBuilder buffer)
		{
			buffer.Append("<se:Envelope xmlns:se=\"http://schemas.xmlsoap.org/soap/envelope/\">");
			if (this.dsmlSessionID != null || this.soapHeaders != null)
			{
				buffer.Append("<se:Header>");
				if (this.dsmlSessionID != null)
				{
					buffer.Append("<ad:Session xmlns:ad=\"urn:schema-microsoft-com:activedirectory:dsmlv2\" ad:SessionID=\"");
					buffer.Append(this.dsmlSessionID);
					buffer.Append("\" se:mustUnderstand=\"1\"/>");
				}
				if (this.soapHeaders != null)
				{
					buffer.Append(this.soapHeaders.OuterXml);
				}
				buffer.Append("</se:Header>");
			}
			buffer.Append("<se:Body xmlns=\"urn:oasis:names:tc:DSML:2:0:core\">");
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009544 File Offset: 0x00008544
		private void EndSOAPRequest(ref StringBuilder buffer)
		{
			buffer.Append("</se:Body>");
			buffer.Append("</se:Envelope>");
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009560 File Offset: 0x00008560
		private string ExtractSessionID(HttpWebResponse resp)
		{
			Stream responseStream = resp.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string value;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.Load(streamReader);
				}
				catch (XmlException)
				{
					throw new DsmlInvalidDocumentException();
				}
				XmlNamespaceManager dsmlNamespaceManager = NamespaceUtils.GetDsmlNamespaceManager();
				XmlAttribute xmlAttribute = (XmlAttribute)xmlDocument.SelectSingleNode("se:Envelope/se:Header/ad:Session/@ad:SessionID", dsmlNamespaceManager);
				if (xmlAttribute == null)
				{
					xmlAttribute = (XmlAttribute)xmlDocument.SelectSingleNode("se:Envelope/se:Header/ad:Session/@SessionID", dsmlNamespaceManager);
					if (xmlAttribute == null)
					{
						throw new DsmlInvalidDocumentException(Res.GetString("NoSessionIDReturned"));
					}
				}
				value = xmlAttribute.Value;
			}
			finally
			{
				streamReader.Close();
			}
			return value;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00009608 File Offset: 0x00008608
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[NetworkInformationPermission(SecurityAction.Assert, Unrestricted = true)]
		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		public IAsyncResult BeginSendRequest(DsmlRequestDocument request, AsyncCallback callback, object state)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(((DsmlDirectoryIdentifier)this.directoryIdentifier).ServerUri);
			this.PrepareHttpWebRequest(httpWebRequest);
			StringBuilder stringBuilder = new StringBuilder(1024);
			this.BeginSOAPRequest(ref stringBuilder);
			stringBuilder.Append(request.ToXml().InnerXml);
			this.EndSOAPRequest(ref stringBuilder);
			RequestState requestState = new RequestState();
			requestState.request = httpWebRequest;
			requestState.requestString = stringBuilder.ToString();
			DsmlAsyncResult dsmlAsyncResult = new DsmlAsyncResult(callback, state);
			dsmlAsyncResult.resultObject = requestState;
			if (request.Count > 0)
			{
				dsmlAsyncResult.hasValidRequest = true;
			}
			requestState.dsmlAsync = dsmlAsyncResult;
			this.httpConnectionTable.Add(dsmlAsyncResult, httpWebRequest);
			httpWebRequest.BeginGetRequestStream(new AsyncCallback(DsmlSoapHttpConnection.RequestStreamCallback), requestState);
			return dsmlAsyncResult;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000096D4 File Offset: 0x000086D4
		private static void RequestStreamCallback(IAsyncResult asyncResult)
		{
			RequestState requestState = (RequestState)asyncResult.AsyncState;
			HttpWebRequest request = requestState.request;
			try
			{
				requestState.requestStream = request.EndGetRequestStream(asyncResult);
				byte[] bytes = requestState.encoder.GetBytes(requestState.requestString);
				requestState.requestStream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(DsmlSoapHttpConnection.WriteCallback), requestState);
			}
			catch (Exception ex)
			{
				if (requestState.requestStream != null)
				{
					requestState.requestStream.Close();
				}
				requestState.exception = ex;
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
			catch
			{
				if (requestState.requestStream != null)
				{
					requestState.requestStream.Close();
				}
				requestState.exception = new Exception(Res.GetString("NonCLSException"));
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000097A4 File Offset: 0x000087A4
		private static void WriteCallback(IAsyncResult asyncResult)
		{
			RequestState requestState = (RequestState)asyncResult.AsyncState;
			try
			{
				requestState.requestStream.EndWrite(asyncResult);
				requestState.request.BeginGetResponse(new AsyncCallback(DsmlSoapHttpConnection.ResponseCallback), requestState);
			}
			catch (Exception ex)
			{
				requestState.exception = ex;
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
			catch
			{
				requestState.exception = new Exception(Res.GetString("NonCLSException"));
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
			finally
			{
				requestState.requestStream.Close();
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00009848 File Offset: 0x00008848
		private static void ResponseCallback(IAsyncResult asyncResult)
		{
			RequestState requestState = (RequestState)asyncResult.AsyncState;
			try
			{
				WebResponse webResponse = requestState.request.EndGetResponse(asyncResult);
				requestState.responseStream = webResponse.GetResponseStream();
				requestState.responseStream.BeginRead(requestState.bufferRead, 0, 1024, new AsyncCallback(DsmlSoapHttpConnection.ReadCallback), requestState);
			}
			catch (Exception ex)
			{
				if (requestState.responseStream != null)
				{
					requestState.responseStream.Close();
				}
				requestState.exception = ex;
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
			catch
			{
				if (requestState.responseStream != null)
				{
					requestState.responseStream.Close();
				}
				requestState.exception = new Exception(Res.GetString("NonCLSException"));
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009914 File Offset: 0x00008914
		private static void ReadCallback(IAsyncResult asyncResult)
		{
			RequestState requestState = (RequestState)asyncResult.AsyncState;
			try
			{
				int num = requestState.responseStream.EndRead(asyncResult);
				if (num > 0)
				{
					string @string = requestState.encoder.GetString(requestState.bufferRead);
					int num2 = Math.Min(@string.Length, num);
					requestState.responseString.Append(@string, 0, num2);
					requestState.responseStream.BeginRead(requestState.bufferRead, 0, 1024, new AsyncCallback(DsmlSoapHttpConnection.ReadCallback), requestState);
				}
				else
				{
					requestState.responseStream.Close();
					DsmlSoapHttpConnection.WakeupRoutine(requestState);
				}
			}
			catch (Exception ex)
			{
				requestState.responseStream.Close();
				requestState.exception = ex;
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
			catch
			{
				requestState.responseStream.Close();
				requestState.exception = new Exception(Res.GetString("NonCLSException"));
				DsmlSoapHttpConnection.WakeupRoutine(requestState);
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00009A0C File Offset: 0x00008A0C
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Abort(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is DsmlAsyncResult))
			{
				throw new ArgumentException(Res.GetString("NotReturnedAsyncResult", new object[] { "asyncResult" }));
			}
			if (!this.httpConnectionTable.Contains(asyncResult))
			{
				throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)this.httpConnectionTable[asyncResult];
			this.httpConnectionTable.Remove(asyncResult);
			httpWebRequest.Abort();
			DsmlAsyncResult dsmlAsyncResult = (DsmlAsyncResult)asyncResult;
			dsmlAsyncResult.resultObject.abortCalled = true;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009AA4 File Offset: 0x00008AA4
		public DsmlResponseDocument EndSendRequest(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is DsmlAsyncResult))
			{
				throw new ArgumentException(Res.GetString("NotReturnedAsyncResult", new object[] { "asyncResult" }));
			}
			if (!this.httpConnectionTable.Contains(asyncResult))
			{
				throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
			}
			this.httpConnectionTable.Remove(asyncResult);
			DsmlAsyncResult dsmlAsyncResult = (DsmlAsyncResult)asyncResult;
			asyncResult.AsyncWaitHandle.WaitOne();
			if (dsmlAsyncResult.resultObject.exception != null)
			{
				throw dsmlAsyncResult.resultObject.exception;
			}
			DsmlResponseDocument dsmlResponseDocument = new DsmlResponseDocument(dsmlAsyncResult.resultObject.responseString, "se:Envelope/se:Body/dsml:batchResponse");
			this.debugResponse = dsmlResponseDocument.ResponseString;
			if (dsmlAsyncResult.hasValidRequest && dsmlResponseDocument.Count == 0)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("MissingResponse"));
			}
			return dsmlResponseDocument;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009B80 File Offset: 0x00008B80
		private static void WakeupRoutine(RequestState rs)
		{
			rs.dsmlAsync.manualResetEvent.Set();
			rs.dsmlAsync.completed = true;
			if (rs.dsmlAsync.callback != null && !rs.abortCalled)
			{
				rs.dsmlAsync.callback(rs.dsmlAsync);
			}
		}

		// Token: 0x040001FC RID: 508
		private HttpWebRequest dsmlHttpConnection;

		// Token: 0x040001FD RID: 509
		private string dsmlSoapAction = "\"#batchRequest\"";

		// Token: 0x040001FE RID: 510
		private AuthType dsmlAuthType = AuthType.Negotiate;

		// Token: 0x040001FF RID: 511
		private string dsmlSessionID;

		// Token: 0x04000200 RID: 512
		private Hashtable httpConnectionTable;

		// Token: 0x04000201 RID: 513
		private string debugResponse;
	}
}
