using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000082 RID: 130
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpRequest
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x000155F5 File Offset: 0x000145F5
		internal HttpRequest(HttpWorkerRequest wr, HttpContext context)
		{
			this._wr = wr;
			this._context = context;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00015614 File Offset: 0x00014614
		public HttpRequest(string filename, string url, string queryString)
		{
			this._wr = null;
			this._pathTranslated = filename;
			this._httpMethod = "GET";
			this._url = new Uri(url);
			this._path = VirtualPath.CreateAbsolute(this._url.AbsolutePath);
			this._queryStringText = queryString;
			this._queryStringOverriden = true;
			this._queryString = new HttpValueCollection(this._queryStringText, true, true, Encoding.Default);
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_EXECUTING);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00015698 File Offset: 0x00014698
		internal HttpRequest(VirtualPath virtualPath, string queryString)
		{
			this._wr = null;
			this._pathTranslated = virtualPath.MapPath();
			this._httpMethod = "GET";
			this._url = new Uri("http://localhost" + virtualPath.VirtualPathString);
			this._path = virtualPath;
			this._queryStringText = queryString;
			this._queryStringOverriden = true;
			this._queryString = new HttpValueCollection(this._queryStringText, true, true, Encoding.Default);
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_EXECUTING);
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001571F File Offset: 0x0001471F
		internal byte[] EntityBody
		{
			get
			{
				if (!this._readEntityBody)
				{
					return null;
				}
				return this._rawContent.GetAsByteArray();
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00015736 File Offset: 0x00014736
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001574C File Offset: 0x0001474C
		internal string ClientTarget
		{
			get
			{
				if (this._clientTarget != null)
				{
					return this._clientTarget;
				}
				return string.Empty;
			}
			set
			{
				this._clientTarget = value;
				this._browsercaps = null;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001575C File Offset: 0x0001475C
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00015764 File Offset: 0x00014764
		internal HttpContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001576D File Offset: 0x0001476D
		internal HttpResponse Response
		{
			get
			{
				if (this._context == null)
				{
					return null;
				}
				return this._context.Response;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00015784 File Offset: 0x00014784
		public bool IsLocal
		{
			get
			{
				string userHostAddress = this.UserHostAddress;
				return !string.IsNullOrEmpty(userHostAddress) && (userHostAddress == "127.0.0.1" || userHostAddress == "::1" || userHostAddress == this.LocalAddress);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000157CF File Offset: 0x000147CF
		internal void Dispose()
		{
			if (this._serverVariables != null)
			{
				this._serverVariables.Dispose();
			}
			if (this._rawContent != null)
			{
				this._rawContent.Dispose();
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000157F8 File Offset: 0x000147F8
		private static string[] ParseMultivalueHeader(string s)
		{
			int num = ((s != null) ? s.Length : 0);
			if (num == 0)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			int i = 0;
			while (i < num)
			{
				int num2 = s.IndexOf(',', i);
				if (num2 < 0)
				{
					num2 = num;
				}
				arrayList.Add(s.Substring(i, num2 - i));
				i = num2 + 1;
				if (i < num && s[i] == ' ')
				{
					i++;
				}
			}
			int count = arrayList.Count;
			if (count == 0)
			{
				return null;
			}
			string[] array = new string[count];
			arrayList.CopyTo(0, array, 0, count);
			return array;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00015884 File Offset: 0x00014884
		private void FillInQueryStringCollection()
		{
			byte[] queryStringBytes = this.QueryStringBytes;
			if (queryStringBytes != null)
			{
				if (queryStringBytes.Length != 0)
				{
					this._queryString.FillFromEncodedBytes(queryStringBytes, this.QueryStringEncoding);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.QueryStringText))
			{
				this._queryString.FillFromString(this.QueryStringText, true, this.QueryStringEncoding);
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000158D8 File Offset: 0x000148D8
		private void FillInFormCollection()
		{
			if (this._wr == null)
			{
				return;
			}
			if (!this._wr.HasEntityBody())
			{
				return;
			}
			string contentType = this.ContentType;
			if (contentType == null)
			{
				return;
			}
			if (StringUtil.StringStartsWithIgnoreCase(contentType, "application/x-www-form-urlencoded"))
			{
				byte[] array = null;
				HttpRawUploadedContent entireRawContent = this.GetEntireRawContent();
				if (entireRawContent != null)
				{
					array = entireRawContent.GetAsByteArray();
				}
				if (array == null)
				{
					return;
				}
				try
				{
					this._form.FillFromEncodedBytes(array, this.ContentEncoding);
					return;
				}
				catch (Exception ex)
				{
					throw new HttpException(SR.GetString("Invalid_urlencoded_form_data"), ex);
				}
			}
			if (StringUtil.StringStartsWithIgnoreCase(contentType, "multipart/form-data"))
			{
				MultipartContentElement[] multipartContent = this.GetMultipartContent();
				if (multipartContent != null)
				{
					for (int i = 0; i < multipartContent.Length; i++)
					{
						if (this._form.Count >= AppSettings.MaxHttpCollectionKeys)
						{
							throw new InvalidOperationException();
						}
						if (multipartContent[i].IsFormItem)
						{
							this._form.Add(multipartContent[i].Name, multipartContent[i].GetAsString(this.ContentEncoding));
						}
					}
				}
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000159DC File Offset: 0x000149DC
		private void FillInHeadersCollection()
		{
			if (this._wr == null)
			{
				return;
			}
			for (int i = 0; i < 40; i++)
			{
				string knownRequestHeader = this._wr.GetKnownRequestHeader(i);
				if (!string.IsNullOrEmpty(knownRequestHeader))
				{
					string knownRequestHeaderName = HttpWorkerRequest.GetKnownRequestHeaderName(i);
					this._headers.SynchronizeHeader(knownRequestHeaderName, knownRequestHeader);
				}
			}
			string[][] unknownRequestHeaders = this._wr.GetUnknownRequestHeaders();
			if (unknownRequestHeaders != null)
			{
				for (int j = 0; j < unknownRequestHeaders.Length; j++)
				{
					this._headers.SynchronizeHeader(unknownRequestHeaders[j][0], unknownRequestHeaders[j][1]);
				}
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00015A60 File Offset: 0x00014A60
		private static string ServerVariableNameFromHeader(string header)
		{
			return "HTTP_" + header.ToUpper(CultureInfo.InvariantCulture).Replace('-', '_');
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00015A80 File Offset: 0x00014A80
		private string CombineAllHeaders(bool asRaw)
		{
			if (this._wr == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			for (int i = 0; i < 40; i++)
			{
				string knownRequestHeader = this._wr.GetKnownRequestHeader(i);
				if (!string.IsNullOrEmpty(knownRequestHeader))
				{
					string text;
					if (!asRaw)
					{
						text = HttpWorkerRequest.GetServerVariableNameFromKnownRequestHeaderIndex(i);
					}
					else
					{
						text = HttpWorkerRequest.GetKnownRequestHeaderName(i);
					}
					if (text != null)
					{
						stringBuilder.Append(text);
						stringBuilder.Append(asRaw ? ": " : ":");
						stringBuilder.Append(knownRequestHeader);
						stringBuilder.Append("\r\n");
					}
				}
			}
			string[][] unknownRequestHeaders = this._wr.GetUnknownRequestHeaders();
			if (unknownRequestHeaders != null)
			{
				for (int j = 0; j < unknownRequestHeaders.Length; j++)
				{
					string text2 = unknownRequestHeaders[j][0];
					if (!asRaw)
					{
						text2 = HttpRequest.ServerVariableNameFromHeader(text2);
					}
					stringBuilder.Append(text2);
					stringBuilder.Append(asRaw ? ": " : ":");
					stringBuilder.Append(unknownRequestHeaders[j][1]);
					stringBuilder.Append("\r\n");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00015B88 File Offset: 0x00014B88
		internal string CalcDynamicServerVariable(DynamicServerVariable var)
		{
			string text = null;
			switch (var)
			{
			case DynamicServerVariable.AUTH_TYPE:
				if (this._context.User != null && this._context.User.Identity.IsAuthenticated)
				{
					text = this._context.User.Identity.AuthenticationType;
				}
				else
				{
					text = string.Empty;
				}
				break;
			case DynamicServerVariable.AUTH_USER:
				if (this._context.User != null && this._context.User.Identity.IsAuthenticated)
				{
					text = this._context.User.Identity.Name;
				}
				else
				{
					text = string.Empty;
				}
				break;
			case DynamicServerVariable.PATH_INFO:
				text = this.Path;
				break;
			case DynamicServerVariable.PATH_TRANSLATED:
				text = this.PhysicalPathInternal;
				break;
			case DynamicServerVariable.QUERY_STRING:
				text = this.QueryStringText;
				break;
			case DynamicServerVariable.SCRIPT_NAME:
				text = this.FilePath;
				break;
			}
			return text;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00015C69 File Offset: 0x00014C69
		private void AddServerVariableToCollection(string name, DynamicServerVariable var)
		{
			this._serverVariables.AddDynamic(name, var);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00015C78 File Offset: 0x00014C78
		private void AddServerVariableToCollection(string name, string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			this._serverVariables.AddStatic(name, value);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00015C91 File Offset: 0x00014C91
		private void AddServerVariableToCollection(string name)
		{
			this._serverVariables.AddStatic(name, this._wr.GetServerVariable(name));
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00015CAC File Offset: 0x00014CAC
		internal void FillInServerVariablesCollection()
		{
			if (this._wr == null)
			{
				return;
			}
			this.AddServerVariableToCollection("ALL_HTTP", this.CombineAllHeaders(false));
			this.AddServerVariableToCollection("ALL_RAW", this.CombineAllHeaders(true));
			this.AddServerVariableToCollection("APPL_MD_PATH");
			this.AddServerVariableToCollection("APPL_PHYSICAL_PATH", this._wr.GetAppPathTranslated());
			this.AddServerVariableToCollection("AUTH_TYPE", DynamicServerVariable.AUTH_TYPE);
			this.AddServerVariableToCollection("AUTH_USER", DynamicServerVariable.AUTH_USER);
			this.AddServerVariableToCollection("AUTH_PASSWORD");
			this.AddServerVariableToCollection("LOGON_USER");
			this.AddServerVariableToCollection("REMOTE_USER", DynamicServerVariable.AUTH_USER);
			this.AddServerVariableToCollection("CERT_COOKIE");
			this.AddServerVariableToCollection("CERT_FLAGS");
			this.AddServerVariableToCollection("CERT_ISSUER");
			this.AddServerVariableToCollection("CERT_KEYSIZE");
			this.AddServerVariableToCollection("CERT_SECRETKEYSIZE");
			this.AddServerVariableToCollection("CERT_SERIALNUMBER");
			this.AddServerVariableToCollection("CERT_SERVER_ISSUER");
			this.AddServerVariableToCollection("CERT_SERVER_SUBJECT");
			this.AddServerVariableToCollection("CERT_SUBJECT");
			string knownRequestHeader = this._wr.GetKnownRequestHeader(11);
			this.AddServerVariableToCollection("CONTENT_LENGTH", (knownRequestHeader != null) ? knownRequestHeader : "0");
			this.AddServerVariableToCollection("CONTENT_TYPE", this.ContentType);
			this.AddServerVariableToCollection("GATEWAY_INTERFACE");
			this.AddServerVariableToCollection("HTTPS");
			this.AddServerVariableToCollection("HTTPS_KEYSIZE");
			this.AddServerVariableToCollection("HTTPS_SECRETKEYSIZE");
			this.AddServerVariableToCollection("HTTPS_SERVER_ISSUER");
			this.AddServerVariableToCollection("HTTPS_SERVER_SUBJECT");
			this.AddServerVariableToCollection("INSTANCE_ID");
			this.AddServerVariableToCollection("INSTANCE_META_PATH");
			this.AddServerVariableToCollection("LOCAL_ADDR", this._wr.GetLocalAddress());
			this.AddServerVariableToCollection("PATH_INFO", DynamicServerVariable.PATH_INFO);
			this.AddServerVariableToCollection("PATH_TRANSLATED", DynamicServerVariable.PATH_TRANSLATED);
			this.AddServerVariableToCollection("QUERY_STRING", DynamicServerVariable.QUERY_STRING);
			this.AddServerVariableToCollection("REMOTE_ADDR", this.UserHostAddress);
			this.AddServerVariableToCollection("REMOTE_HOST", this.UserHostName);
			this.AddServerVariableToCollection("REMOTE_PORT");
			this.AddServerVariableToCollection("REQUEST_METHOD", this.HttpMethod);
			this.AddServerVariableToCollection("SCRIPT_NAME", DynamicServerVariable.SCRIPT_NAME);
			this.AddServerVariableToCollection("SERVER_NAME", this._wr.GetServerName());
			this.AddServerVariableToCollection("SERVER_PORT", this._wr.GetLocalPortAsString());
			this.AddServerVariableToCollection("SERVER_PORT_SECURE", this._wr.IsSecure() ? "1" : "0");
			this.AddServerVariableToCollection("SERVER_PROTOCOL", this._wr.GetHttpVersion());
			this.AddServerVariableToCollection("SERVER_SOFTWARE");
			this.AddServerVariableToCollection("URL", DynamicServerVariable.SCRIPT_NAME);
			for (int i = 0; i < 40; i++)
			{
				string knownRequestHeader2 = this._wr.GetKnownRequestHeader(i);
				if (!string.IsNullOrEmpty(knownRequestHeader2))
				{
					this.AddServerVariableToCollection(HttpWorkerRequest.GetServerVariableNameFromKnownRequestHeaderIndex(i), knownRequestHeader2);
				}
			}
			string[][] unknownRequestHeaders = this._wr.GetUnknownRequestHeaders();
			if (unknownRequestHeaders != null)
			{
				for (int j = 0; j < unknownRequestHeaders.Length; j++)
				{
					this.AddServerVariableToCollection(HttpRequest.ServerVariableNameFromHeader(unknownRequestHeaders[j][0]), unknownRequestHeaders[j][1]);
				}
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00015F9C File Offset: 0x00014F9C
		internal static HttpCookie CreateCookieFromString(string s)
		{
			HttpCookie httpCookie = new HttpCookie();
			int num = ((s != null) ? s.Length : 0);
			int i = 0;
			bool flag = true;
			int num2 = 1;
			while (i < num)
			{
				int num3 = s.IndexOf('&', i);
				if (num3 < 0)
				{
					num3 = num;
				}
				int num4;
				if (flag)
				{
					num4 = s.IndexOf('=', i);
					if (num4 >= 0 && num4 < num3)
					{
						httpCookie.Name = s.Substring(i, num4 - i);
						i = num4 + 1;
					}
					else if (num3 == num)
					{
						httpCookie.Name = s;
						break;
					}
					flag = false;
				}
				num4 = s.IndexOf('=', i);
				if (num4 < 0 && num3 == num && num2 == 0)
				{
					httpCookie.Value = s.Substring(i, num - i);
				}
				else if (num4 >= 0 && num4 < num3)
				{
					httpCookie.Values.Add(s.Substring(i, num4 - i), s.Substring(num4 + 1, num3 - num4 - 1));
					num2++;
				}
				else
				{
					httpCookie.Values.Add(null, s.Substring(i, num3 - i));
					num2++;
				}
				i = num3 + 1;
			}
			return httpCookie;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000160A8 File Offset: 0x000150A8
		internal void FillInCookiesCollection(HttpCookieCollection cookieCollection, bool includeResponse)
		{
			if (this._wr == null)
			{
				return;
			}
			string knownRequestHeader = this._wr.GetKnownRequestHeader(25);
			int num = ((knownRequestHeader != null) ? knownRequestHeader.Length : 0);
			int i = 0;
			HttpCookie httpCookie = null;
			while (i < num)
			{
				int j;
				for (j = i; j < num; j++)
				{
					char c = knownRequestHeader[j];
					if (c == ';')
					{
						break;
					}
				}
				string text = knownRequestHeader.Substring(i, j - i).Trim();
				i = j + 1;
				if (text.Length != 0)
				{
					HttpCookie httpCookie2 = HttpRequest.CreateCookieFromString(text);
					if (httpCookie != null)
					{
						string name = httpCookie2.Name;
						if (name != null && name.Length > 0 && name[0] == '$')
						{
							if (StringUtil.EqualsIgnoreCase(name, "$Path"))
							{
								httpCookie.Path = httpCookie2.Value;
								continue;
							}
							if (StringUtil.EqualsIgnoreCase(name, "$Domain"))
							{
								httpCookie.Domain = httpCookie2.Value;
								continue;
							}
							continue;
						}
					}
					cookieCollection.AddCookie(httpCookie2, true);
					httpCookie = httpCookie2;
				}
			}
			if (includeResponse && this.Response != null)
			{
				HttpCookieCollection cookies = this.Response.Cookies;
				if (cookies.Count > 0)
				{
					HttpCookie[] array = new HttpCookie[cookies.Count];
					cookies.CopyTo(array, 0);
					for (int k = 0; k < array.Length; k++)
					{
						cookieCollection.AddCookie(array[k], true);
					}
				}
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000161F8 File Offset: 0x000151F8
		private void FillInParamsCollection()
		{
			this._params.Add(this.QueryString);
			this._params.Add(this.Form);
			this._params.Add(this.Cookies);
			this._params.Add(this.ServerVariables);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001624C File Offset: 0x0001524C
		private void FillInFilesCollection()
		{
			if (this._wr == null)
			{
				return;
			}
			if (!StringUtil.StringStartsWithIgnoreCase(this.ContentType, "multipart/form-data"))
			{
				return;
			}
			MultipartContentElement[] multipartContent = this.GetMultipartContent();
			if (multipartContent == null)
			{
				return;
			}
			bool flag = false;
			if (this._flags[64])
			{
				this._flags.Clear(64);
				flag = true;
			}
			for (int i = 0; i < multipartContent.Length; i++)
			{
				if (multipartContent[i].IsFile)
				{
					HttpPostedFile asPostedFile = multipartContent[i].GetAsPostedFile();
					if (flag)
					{
						HttpRequest.ValidateString(asPostedFile.FileName, "filename", "Request.Files");
					}
					this._files.AddFile(multipartContent[i].Name, asPostedFile);
				}
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000162F0 File Offset: 0x000152F0
		private static string GetAttributeFromHeader(string headerValue, string attrName)
		{
			if (headerValue == null)
			{
				return null;
			}
			int length = headerValue.Length;
			int length2 = attrName.Length;
			int i;
			for (i = 1; i < length; i += length2)
			{
				i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, i, CompareOptions.IgnoreCase);
				if (i < 0 || i + length2 >= length)
				{
					break;
				}
				char c = headerValue[i - 1];
				char c2 = headerValue[i + length2];
				if ((c == ';' || c == ',' || char.IsWhiteSpace(c)) && (c2 == '=' || char.IsWhiteSpace(c2)))
				{
					break;
				}
			}
			if (i < 0 || i >= length)
			{
				return null;
			}
			i += length2;
			while (i < length && char.IsWhiteSpace(headerValue[i]))
			{
				i++;
			}
			if (i >= length || headerValue[i] != '=')
			{
				return null;
			}
			i++;
			while (i < length && char.IsWhiteSpace(headerValue[i]))
			{
				i++;
			}
			if (i >= length)
			{
				return null;
			}
			string text;
			if (i < length && headerValue[i] == '"')
			{
				if (i == length - 1)
				{
					return null;
				}
				int num = headerValue.IndexOf('"', i + 1);
				if (num < 0 || num == i + 1)
				{
					return null;
				}
				text = headerValue.Substring(i + 1, num - i - 1).Trim();
			}
			else
			{
				int num = i;
				while (num < length && headerValue[num] != ' ' && headerValue[num] != ',')
				{
					num++;
				}
				if (num == i)
				{
					return null;
				}
				text = headerValue.Substring(i, num - i).Trim();
			}
			return text;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001645C File Offset: 0x0001545C
		private Encoding GetEncodingFromHeaders()
		{
			if (this.UserAgent != null && CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this.UserAgent, "UP"))
			{
				string text = this.Headers["x-up-devcap-post-charset"];
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						return Encoding.GetEncoding(text);
					}
					catch
					{
					}
				}
			}
			if (!this._wr.HasEntityBody())
			{
				return null;
			}
			string contentType = this.ContentType;
			if (contentType == null)
			{
				return null;
			}
			string attributeFromHeader = HttpRequest.GetAttributeFromHeader(contentType, "charset");
			if (attributeFromHeader == null)
			{
				return null;
			}
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding(attributeFromHeader);
			}
			catch
			{
			}
			return encoding;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00016510 File Offset: 0x00015510
		private HttpRawUploadedContent GetEntireRawContent()
		{
			if (this._wr == null)
			{
				return null;
			}
			if (this._rawContent != null)
			{
				return this._rawContent;
			}
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetConfig(this._context).HttpRuntime;
			int maxRequestLengthBytes = httpRuntime.MaxRequestLengthBytes;
			if (this.ContentLength > maxRequestLengthBytes)
			{
				if (!(this._wr is IIS7WorkerRequest))
				{
					this.Response.CloseConnectionAfterError();
				}
				throw new HttpException(SR.GetString("Max_request_length_exceeded"), null, 3004);
			}
			int requestLengthDiskThresholdBytes = httpRuntime.RequestLengthDiskThresholdBytes;
			HttpRawUploadedContent httpRawUploadedContent = new HttpRawUploadedContent(requestLengthDiskThresholdBytes, this.ContentLength);
			byte[] preloadedEntityBody = this._wr.GetPreloadedEntityBody();
			if (preloadedEntityBody != null)
			{
				this._wr.UpdateRequestCounters(preloadedEntityBody.Length);
				httpRawUploadedContent.AddBytes(preloadedEntityBody, 0, preloadedEntityBody.Length);
			}
			if (!this._wr.IsEntireEntityBodyIsPreloaded())
			{
				int i = ((this.ContentLength > 0) ? (this.ContentLength - httpRawUploadedContent.Length) : int.MaxValue);
				HttpApplication applicationInstance = this._context.ApplicationInstance;
				byte[] array = ((applicationInstance != null) ? applicationInstance.EntityBuffer : new byte[8192]);
				int num = httpRawUploadedContent.Length;
				IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
				while (i > 0)
				{
					int num2 = array.Length;
					if (num2 > i)
					{
						num2 = i;
					}
					int num3;
					if (iis7WorkerRequest != null)
					{
						num3 = iis7WorkerRequest.ReadEntityBodyWithTimeout(array, num2, this._context.TimeLeft);
					}
					else
					{
						num3 = this._wr.ReadEntityBody(array, num2);
					}
					if (num3 <= 0)
					{
						break;
					}
					this._wr.UpdateRequestCounters(num3);
					this._readEntityBody = true;
					httpRawUploadedContent.AddBytes(array, 0, num3);
					i -= num3;
					num += num3;
					if (num > maxRequestLengthBytes)
					{
						throw new HttpException(SR.GetString("Max_request_length_exceeded"), null, 3004);
					}
					if (i > 0 && this._context.TimeLeft <= 0L)
					{
						throw new HttpException(SR.GetString("Request_timed_out"));
					}
				}
			}
			httpRawUploadedContent.DoneAddingBytes();
			if (this._installedFilter != null && httpRawUploadedContent.Length > 0)
			{
				try
				{
					try
					{
						this._filterSource.SetContent(httpRawUploadedContent);
						HttpRawUploadedContent httpRawUploadedContent2 = new HttpRawUploadedContent(requestLengthDiskThresholdBytes, httpRawUploadedContent.Length);
						HttpApplication applicationInstance2 = this._context.ApplicationInstance;
						byte[] array2 = ((applicationInstance2 != null) ? applicationInstance2.EntityBuffer : new byte[8192]);
						for (;;)
						{
							int num4 = this._installedFilter.Read(array2, 0, array2.Length);
							if (num4 == 0)
							{
								break;
							}
							httpRawUploadedContent2.AddBytes(array2, 0, num4);
						}
						httpRawUploadedContent2.DoneAddingBytes();
						httpRawUploadedContent = httpRawUploadedContent2;
					}
					finally
					{
						this._filterSource.SetContent(null);
					}
				}
				catch
				{
					throw;
				}
			}
			this._rawContent = httpRawUploadedContent;
			return this._rawContent;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000167B8 File Offset: 0x000157B8
		private MultipartContentElement[] GetMultipartContent()
		{
			if (this._multipartContentElements != null)
			{
				return this._multipartContentElements;
			}
			byte[] multipartBoundary = this.GetMultipartBoundary();
			if (multipartBoundary == null)
			{
				return new MultipartContentElement[0];
			}
			HttpRawUploadedContent entireRawContent = this.GetEntireRawContent();
			if (entireRawContent == null)
			{
				return new MultipartContentElement[0];
			}
			this._multipartContentElements = HttpMultipartContentTemplateParser.Parse(entireRawContent, entireRawContent.Length, multipartBoundary, this.ContentEncoding);
			return this._multipartContentElements;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00016818 File Offset: 0x00015818
		private byte[] GetMultipartBoundary()
		{
			string text = HttpRequest.GetAttributeFromHeader(this.ContentType, "boundary");
			if (text == null)
			{
				return null;
			}
			text = "--" + text;
			return Encoding.ASCII.GetBytes(text.ToCharArray());
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00016858 File Offset: 0x00015858
		internal void AddResponseCookie(HttpCookie cookie)
		{
			if (this._cookies != null)
			{
				this._cookies.AddCookie(cookie, true);
			}
			if (this._params != null)
			{
				this._params.MakeReadWrite();
				this._params.Add(cookie.Name, cookie.Value);
				this._params.MakeReadOnly();
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000168B0 File Offset: 0x000158B0
		internal void ResetCookies()
		{
			if (this._cookies != null)
			{
				this._cookies.Reset();
				this.FillInCookiesCollection(this._cookies, true);
			}
			if (this._params != null)
			{
				this._params.MakeReadWrite();
				this._params.Reset();
				this.FillInParamsCollection();
				this._params.MakeReadOnly();
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001690C File Offset: 0x0001590C
		public string HttpMethod
		{
			get
			{
				if (this._httpMethod == null)
				{
					this._httpMethod = this._wr.GetHttpVerbName();
				}
				return this._httpMethod;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00016930 File Offset: 0x00015930
		internal HttpVerb HttpVerb
		{
			get
			{
				if (this._httpVerb == HttpVerb.Unparsed)
				{
					this._httpVerb = HttpVerb.Unknown;
					string httpMethod = this.HttpMethod;
					if (httpMethod != null)
					{
						switch (httpMethod.Length)
						{
						case 3:
							if (httpMethod == "GET")
							{
								this._httpVerb = HttpVerb.GET;
							}
							else if (httpMethod == "PUT")
							{
								this._httpVerb = HttpVerb.PUT;
							}
							break;
						case 4:
							if (httpMethod == "POST")
							{
								this._httpVerb = HttpVerb.POST;
							}
							else if (httpMethod == "HEAD")
							{
								this._httpVerb = HttpVerb.HEAD;
							}
							break;
						case 5:
							if (httpMethod == "DEBUG")
							{
								this._httpVerb = HttpVerb.DEBUG;
							}
							break;
						case 6:
							if (httpMethod == "DELETE")
							{
								this._httpVerb = HttpVerb.DELETE;
							}
							break;
						}
					}
				}
				return this._httpVerb;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00016A08 File Offset: 0x00015A08
		internal bool IsDebuggingRequest
		{
			get
			{
				return this.HttpVerb == HttpVerb.DEBUG;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00016A13 File Offset: 0x00015A13
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00016A2A File Offset: 0x00015A2A
		public string RequestType
		{
			get
			{
				if (this._requestType == null)
				{
					return this.HttpMethod;
				}
				return this._requestType;
			}
			set
			{
				this._requestType = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00016A33 File Offset: 0x00015A33
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00016A71 File Offset: 0x00015A71
		public string ContentType
		{
			get
			{
				if (this._contentType == null)
				{
					if (this._wr != null)
					{
						this._contentType = this._wr.GetKnownRequestHeader(12);
					}
					if (this._contentType == null)
					{
						this._contentType = string.Empty;
					}
				}
				return this._contentType;
			}
			set
			{
				this._contentType = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00016A7C File Offset: 0x00015A7C
		public int ContentLength
		{
			get
			{
				if (this._contentLength == -1 && this._wr != null)
				{
					string knownRequestHeader = this._wr.GetKnownRequestHeader(11);
					if (knownRequestHeader != null)
					{
						try
						{
							this._contentLength = int.Parse(knownRequestHeader, CultureInfo.InvariantCulture);
							goto IL_005D;
						}
						catch
						{
							goto IL_005D;
						}
					}
					if (this._wr.IsEntireEntityBodyIsPreloaded())
					{
						byte[] preloadedEntityBody = this._wr.GetPreloadedEntityBody();
						if (preloadedEntityBody != null)
						{
							this._contentLength = preloadedEntityBody.Length;
						}
					}
				}
				IL_005D:
				if (this._contentLength < 0)
				{
					return 0;
				}
				return this._contentLength;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00016B08 File Offset: 0x00015B08
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00016B77 File Offset: 0x00015B77
		public Encoding ContentEncoding
		{
			get
			{
				if (this._flags[32] && this._encoding != null)
				{
					return this._encoding;
				}
				this._encoding = this.GetEncodingFromHeaders();
				if (this._encoding == null)
				{
					GlobalizationSection globalization = RuntimeConfig.GetLKGConfig(this._context).Globalization;
					this._encoding = globalization.RequestEncoding;
				}
				this._flags.Set(32);
				return this._encoding;
			}
			set
			{
				this._encoding = value;
				this._flags.Set(32);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00016B90 File Offset: 0x00015B90
		internal Encoding QueryStringEncoding
		{
			get
			{
				Encoding contentEncoding = this.ContentEncoding;
				if (!contentEncoding.Equals(Encoding.Unicode))
				{
					return contentEncoding;
				}
				return Encoding.UTF8;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00016BB8 File Offset: 0x00015BB8
		public string[] AcceptTypes
		{
			get
			{
				if (this._acceptTypes == null && this._wr != null)
				{
					this._acceptTypes = HttpRequest.ParseMultivalueHeader(this._wr.GetKnownRequestHeader(20));
				}
				return this._acceptTypes;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00016BE8 File Offset: 0x00015BE8
		public bool IsAuthenticated
		{
			get
			{
				return this._context.User != null && this._context.User.Identity != null && this._context.User.Identity.IsAuthenticated;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00016C20 File Offset: 0x00015C20
		public bool IsSecureConnection
		{
			get
			{
				return this._wr != null && this._wr.IsSecure();
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00016C37 File Offset: 0x00015C37
		public string Path
		{
			get
			{
				return this.PathObject.VirtualPathString;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00016C44 File Offset: 0x00015C44
		internal VirtualPath PathObject
		{
			get
			{
				if (this._path == null)
				{
					this._path = VirtualPath.Create(this._wr.GetUriPath(), VirtualPathOptions.AllowAbsolutePath);
				}
				return this._path;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00016C71 File Offset: 0x00015C71
		public string AnonymousID
		{
			get
			{
				return this._AnonymousId;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00016C7C File Offset: 0x00015C7C
		internal string PathWithQueryString
		{
			get
			{
				string queryStringText = this.QueryStringText;
				if (string.IsNullOrEmpty(queryStringText))
				{
					return this.Path;
				}
				return this.Path + "?" + queryStringText;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00016CB0 File Offset: 0x00015CB0
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00016D31 File Offset: 0x00015D31
		internal VirtualPath ClientFilePath
		{
			get
			{
				if (this._clientFilePath == null)
				{
					if (this._wr != null)
					{
						if (this._wr.IsRewriteModuleEnabled)
						{
							string text = this.RawUrl;
							int num = text.IndexOf('?');
							if (num > -1)
							{
								text = text.Substring(0, num);
							}
							this._clientFilePath = VirtualPath.Create(text, VirtualPathOptions.AllowAbsolutePath);
						}
						else
						{
							this._clientFilePath = this._wr.GetFilePathObject();
						}
					}
					else
					{
						this._clientFilePath = this.PathObject;
					}
				}
				return this._clientFilePath;
			}
			set
			{
				this._clientFilePath = value;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00016D3C File Offset: 0x00015D3C
		internal VirtualPath ClientBaseDir
		{
			get
			{
				if (this._clientBaseDir == null)
				{
					if (this.ClientFilePath.HasTrailingSlash)
					{
						this._clientBaseDir = this.ClientFilePath;
					}
					else
					{
						this._clientBaseDir = this.ClientFilePath.Parent;
					}
				}
				return this._clientBaseDir;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00016D89 File Offset: 0x00015D89
		public string FilePath
		{
			get
			{
				return VirtualPath.GetVirtualPathString(this.FilePathObject);
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00016D98 File Offset: 0x00015D98
		internal VirtualPath FilePathObject
		{
			get
			{
				if (this._filePath != null)
				{
					return this._filePath;
				}
				if (!this._computePathInfo)
				{
					if (this._wr != null)
					{
						this._filePath = this._wr.GetFilePathObject();
						if (!this._wr.IsRewriteModuleEnabled)
						{
							this._clientFilePath = this._filePath;
						}
					}
					else
					{
						this._filePath = this.PathObject;
					}
				}
				else if (this._context != null)
				{
					this._filePath = this.PathObject;
					int length = this._context.GetFilePathData().Path.VirtualPathStringNoTrailingSlash.Length;
					if (this.Path.Length == length)
					{
						this._filePath = this.PathObject;
					}
					else
					{
						this._filePath = VirtualPath.CreateAbsolute(this.Path.Substring(0, length));
					}
				}
				return this._filePath;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00016E6D File Offset: 0x00015E6D
		public string CurrentExecutionFilePath
		{
			get
			{
				return this.CurrentExecutionFilePathObject.VirtualPathString;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00016E7A File Offset: 0x00015E7A
		internal VirtualPath CurrentExecutionFilePathObject
		{
			get
			{
				if (this._currentExecutionFilePath != null)
				{
					return this._currentExecutionFilePath;
				}
				return this.FilePathObject;
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00016E98 File Offset: 0x00015E98
		internal VirtualPath SwitchCurrentExecutionFilePath(VirtualPath path)
		{
			VirtualPath currentExecutionFilePath = this._currentExecutionFilePath;
			this._currentExecutionFilePath = path;
			return currentExecutionFilePath;
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00016EB4 File Offset: 0x00015EB4
		public string AppRelativeCurrentExecutionFilePath
		{
			get
			{
				return UrlPath.MakeVirtualPathAppRelative(this.CurrentExecutionFilePath);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00016EC4 File Offset: 0x00015EC4
		public string PathInfo
		{
			get
			{
				VirtualPath pathInfoObject = this.PathInfoObject;
				if (!(pathInfoObject == null))
				{
					return this.PathInfoObject.VirtualPathString;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00016EF4 File Offset: 0x00015EF4
		internal VirtualPath PathInfoObject
		{
			get
			{
				if (this._pathInfo != null)
				{
					return this._pathInfo;
				}
				if (!this._computePathInfo && this._wr != null)
				{
					this._pathInfo = VirtualPath.CreateAbsoluteAllowNull(this._wr.GetPathInfo());
				}
				if (this._pathInfo == null && this._context != null)
				{
					VirtualPath pathObject = this.PathObject;
					int length = pathObject.VirtualPathString.Length;
					VirtualPath filePathObject = this.FilePathObject;
					int length2 = filePathObject.VirtualPathString.Length;
					if (filePathObject == null)
					{
						this._pathInfo = pathObject;
					}
					else if (pathObject == null || length <= length2)
					{
						this._pathInfo = null;
					}
					else
					{
						string text = pathObject.VirtualPathString.Substring(length2, length - length2);
						this._pathInfo = VirtualPath.CreateAbsolute(text);
					}
				}
				return this._pathInfo;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00016FC4 File Offset: 0x00015FC4
		public string PhysicalPath
		{
			get
			{
				string physicalPathInternal = this.PhysicalPathInternal;
				InternalSecurityPermissions.PathDiscovery(physicalPathInternal).Demand();
				return physicalPathInternal;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00016FE4 File Offset: 0x00015FE4
		internal string PhysicalPathInternal
		{
			get
			{
				if (this._pathTranslated == null)
				{
					if (!this._computePathInfo)
					{
						this._pathTranslated = this._wr.GetFilePathTranslated();
					}
					if (this._pathTranslated == null && this._wr != null)
					{
						this._pathTranslated = HostingEnvironment.MapPathInternal(this.FilePath);
					}
				}
				return this._pathTranslated;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00017039 File Offset: 0x00016039
		public string ApplicationPath
		{
			get
			{
				return HttpRuntime.AppDomainAppVirtualPath;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00017040 File Offset: 0x00016040
		internal VirtualPath ApplicationPathObject
		{
			get
			{
				return HttpRuntime.AppDomainAppVirtualPathObject;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00017047 File Offset: 0x00016047
		public string PhysicalApplicationPath
		{
			get
			{
				InternalSecurityPermissions.AppPathDiscovery.Demand();
				if (this._wr != null)
				{
					return this._wr.GetAppPathTranslated();
				}
				return null;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00017068 File Offset: 0x00016068
		public string UserAgent
		{
			get
			{
				if (this._wr != null)
				{
					return this._wr.GetKnownRequestHeader(39);
				}
				return null;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00017081 File Offset: 0x00016081
		public string[] UserLanguages
		{
			get
			{
				if (this._userLanguages == null && this._wr != null)
				{
					this._userLanguages = HttpRequest.ParseMultivalueHeader(this._wr.GetKnownRequestHeader(23));
				}
				return this._userLanguages;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x000170B4 File Offset: 0x000160B4
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00017124 File Offset: 0x00016124
		public HttpBrowserCapabilities Browser
		{
			get
			{
				if (this._browsercaps != null)
				{
					return this._browsercaps;
				}
				if (!HttpRequest.s_browserCapsEvaled)
				{
					lock (HttpRequest.s_browserLock)
					{
						if (!HttpRequest.s_browserCapsEvaled)
						{
							HttpCapabilitiesBase.GetBrowserCapabilities(this);
						}
						HttpRequest.s_browserCapsEvaled = true;
					}
				}
				this._browsercaps = HttpCapabilitiesBase.GetBrowserCapabilities(this);
				return this._browsercaps;
			}
			set
			{
				this._browsercaps = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00017130 File Offset: 0x00016130
		public string UserHostName
		{
			get
			{
				string text = ((this._wr != null) ? this._wr.GetRemoteName() : null);
				if (string.IsNullOrEmpty(text))
				{
					text = this.UserHostAddress;
				}
				return text;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00017164 File Offset: 0x00016164
		public string UserHostAddress
		{
			get
			{
				if (this._wr != null)
				{
					return this._wr.GetRemoteAddress();
				}
				return null;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001717B File Offset: 0x0001617B
		internal string LocalAddress
		{
			get
			{
				if (this._wr != null)
				{
					return this._wr.GetLocalAddress();
				}
				return null;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00017194 File Offset: 0x00016194
		public string RawUrl
		{
			get
			{
				string text;
				if (this._wr != null)
				{
					text = this._wr.GetRawUrl();
				}
				else
				{
					string path = this.Path;
					string queryStringText = this.QueryStringText;
					if (!string.IsNullOrEmpty(queryStringText))
					{
						text = path + "?" + queryStringText;
					}
					else
					{
						text = path;
					}
				}
				if (this._flags[128])
				{
					this._flags.Clear(128);
					HttpRequest.ValidateString(text, null, "Request.RawUrl");
				}
				return text;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00017210 File Offset: 0x00016210
		internal string UrlInternal
		{
			get
			{
				string text = this.QueryStringText;
				if (!string.IsNullOrEmpty(text))
				{
					text = "?" + HttpUtility.CollapsePercentUFromStringInternal(text, this.QueryStringEncoding);
				}
				if (AppSettings.UseHostHeaderForRequestUrl)
				{
					string knownRequestHeader = this._wr.GetKnownRequestHeader(28);
					try
					{
						if (!string.IsNullOrEmpty(knownRequestHeader))
						{
							string text2 = string.Concat(new string[]
							{
								this._wr.GetProtocol(),
								"://",
								knownRequestHeader,
								this.Path,
								text
							});
							this._url = new Uri(text2);
							return text2;
						}
					}
					catch (UriFormatException)
					{
					}
				}
				string text3 = this._wr.GetServerName();
				if (text3.IndexOf(':') >= 0 && text3[0] != '[')
				{
					text3 = "[" + text3 + "]";
				}
				if (this._wr.GetLocalPortAsString() == "80")
				{
					return string.Concat(new string[]
					{
						this._wr.GetProtocol(),
						"://",
						text3,
						this.Path,
						text
					});
				}
				return string.Concat(new string[]
				{
					this._wr.GetProtocol(),
					"://",
					text3,
					":",
					this._wr.GetLocalPortAsString(),
					this.Path,
					text
				});
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x000173A4 File Offset: 0x000163A4
		public Uri Url
		{
			get
			{
				if (this._url == null && this._wr != null)
				{
					string text = this.QueryStringText;
					if (!string.IsNullOrEmpty(text))
					{
						text = "?" + HttpUtility.CollapsePercentUFromStringInternal(text, this.QueryStringEncoding);
					}
					if (AppSettings.UseHostHeaderForRequestUrl)
					{
						string knownRequestHeader = this._wr.GetKnownRequestHeader(28);
						try
						{
							if (!string.IsNullOrEmpty(knownRequestHeader))
							{
								this._url = new Uri(string.Concat(new string[]
								{
									this._wr.GetProtocol(),
									"://",
									knownRequestHeader,
									this.Path,
									text
								}));
							}
						}
						catch (UriFormatException)
						{
						}
					}
					if (this._url == null)
					{
						string text2 = this._wr.GetServerName();
						if (text2.IndexOf(':') >= 0 && text2[0] != '[')
						{
							text2 = "[" + text2 + "]";
						}
						this._url = new Uri(string.Concat(new string[]
						{
							this._wr.GetProtocol(),
							"://",
							text2,
							":",
							this._wr.GetLocalPortAsString(),
							this.Path,
							text
						}));
					}
				}
				return this._url;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001750C File Offset: 0x0001650C
		public Uri UrlReferrer
		{
			get
			{
				if (this._referrer == null && this._wr != null)
				{
					string knownRequestHeader = this._wr.GetKnownRequestHeader(36);
					if (!string.IsNullOrEmpty(knownRequestHeader))
					{
						try
						{
							if (knownRequestHeader.IndexOf("://", StringComparison.Ordinal) >= 0)
							{
								this._referrer = new Uri(knownRequestHeader);
							}
							else
							{
								this._referrer = new Uri(this.Url, knownRequestHeader);
							}
						}
						catch (HttpException)
						{
							this._referrer = null;
						}
					}
				}
				return this._referrer;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00017598 File Offset: 0x00016598
		internal string IfModifiedSince
		{
			get
			{
				if (this._wr == null)
				{
					return null;
				}
				return this._wr.GetKnownRequestHeader(30);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x000175B1 File Offset: 0x000165B1
		internal string IfNoneMatch
		{
			get
			{
				if (this._wr == null)
				{
					return null;
				}
				return this._wr.GetKnownRequestHeader(31);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x000175CA File Offset: 0x000165CA
		public NameValueCollection Params
		{
			get
			{
				if (HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low))
				{
					return this.GetParams();
				}
				return this.GetParamsWithDemand();
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x000175E5 File Offset: 0x000165E5
		internal void InvalidateParams()
		{
			this._params = null;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000175EE File Offset: 0x000165EE
		private NameValueCollection GetParams()
		{
			if (this._params == null)
			{
				this._params = new HttpValueCollection(64);
				this.FillInParamsCollection();
				this._params.MakeReadOnly();
			}
			return this._params;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001761C File Offset: 0x0001661C
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
		private NameValueCollection GetParamsWithDemand()
		{
			return this.GetParams();
		}

		// Token: 0x17000226 RID: 550
		public string this[string key]
		{
			get
			{
				string text = this.QueryString[key];
				if (text != null)
				{
					return text;
				}
				text = this.Form[key];
				if (text != null)
				{
					return text;
				}
				HttpCookie httpCookie = this.Cookies[key];
				if (httpCookie != null)
				{
					return httpCookie.Value;
				}
				text = this.ServerVariables[key];
				if (text != null)
				{
					return text;
				}
				return null;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00017680 File Offset: 0x00016680
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x000176F8 File Offset: 0x000166F8
		internal string QueryStringText
		{
			get
			{
				if (this._queryStringText == null)
				{
					if (this._wr != null)
					{
						byte[] queryStringBytes = this.QueryStringBytes;
						if (queryStringBytes != null)
						{
							if (queryStringBytes.Length > 0)
							{
								this._queryStringText = this.QueryStringEncoding.GetString(queryStringBytes);
							}
							else
							{
								this._queryStringText = string.Empty;
							}
						}
						else
						{
							this._queryStringText = this._wr.GetQueryString();
						}
					}
					if (this._queryStringText == null)
					{
						this._queryStringText = string.Empty;
					}
				}
				return this._queryStringText;
			}
			set
			{
				this._queryStringText = value;
				this._queryStringOverriden = true;
				if (this._queryString != null)
				{
					this._params = null;
					this._queryString.MakeReadWrite();
					this._queryString.Reset();
					this.FillInQueryStringCollection();
					this._queryString.MakeReadOnly();
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00017749 File Offset: 0x00016749
		internal byte[] QueryStringBytes
		{
			get
			{
				if (this._queryStringOverriden)
				{
					return null;
				}
				if (this._queryStringBytes == null && this._wr != null)
				{
					this._queryStringBytes = this._wr.GetQueryStringRawBytes();
				}
				return this._queryStringBytes;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001777C File Offset: 0x0001677C
		public NameValueCollection QueryString
		{
			get
			{
				if (this._queryString == null)
				{
					this._queryString = new HttpValueCollection();
					if (this._wr != null)
					{
						this.FillInQueryStringCollection();
					}
					this._queryString.MakeReadOnly();
				}
				if (this._flags[1])
				{
					this._flags.Clear(1);
					HttpRequest.ValidateNameValueCollection(this._queryString, "Request.QueryString");
				}
				return this._queryString;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000177E8 File Offset: 0x000167E8
		internal bool HasQueryString
		{
			get
			{
				if (this._queryString != null)
				{
					return this._queryString.Count > 0;
				}
				byte[] queryStringBytes = this.QueryStringBytes;
				if (queryStringBytes != null)
				{
					return queryStringBytes.Length > 0;
				}
				return this.QueryStringText.Length > 0;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001782C File Offset: 0x0001682C
		public NameValueCollection Form
		{
			get
			{
				if (this._form == null)
				{
					this._form = new HttpValueCollection();
					if (this._wr != null)
					{
						this.FillInFormCollection();
					}
					this._form.MakeReadOnly();
				}
				if (this._flags[2])
				{
					this._flags.Clear(2);
					HttpRequest.ValidateNameValueCollection(this._form, "Request.Form");
				}
				return this._form;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00017895 File Offset: 0x00016895
		internal bool HasForm
		{
			get
			{
				if (this._form != null)
				{
					return this._form.Count > 0;
				}
				return (this._wr == null || this._wr.HasEntityBody()) && this.Form.Count > 0;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000178D4 File Offset: 0x000168D4
		internal HttpValueCollection SwitchForm(HttpValueCollection form)
		{
			HttpValueCollection form2 = this._form;
			this._form = form;
			return form2;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x000178F0 File Offset: 0x000168F0
		public NameValueCollection Headers
		{
			get
			{
				if (this._headers == null)
				{
					this._headers = new HttpHeaderCollection(this._wr, this, 8);
					if (this._wr != null)
					{
						this.FillInHeadersCollection();
					}
					if (!(this._wr is IIS7WorkerRequest))
					{
						this._headers.MakeReadOnly();
					}
				}
				return this._headers;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00017944 File Offset: 0x00016944
		public NameValueCollection ServerVariables
		{
			get
			{
				if (HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low))
				{
					return this.GetServerVars();
				}
				return this.GetServerVarsWithDemand();
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001795F File Offset: 0x0001695F
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
		private NameValueCollection GetServerVarsWithDemand()
		{
			return this.GetServerVars();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00017967 File Offset: 0x00016967
		private NameValueCollection GetServerVars()
		{
			if (this._serverVariables == null)
			{
				this._serverVariables = new HttpServerVarsCollection(this._wr, this);
				if (!(this._wr is IIS7WorkerRequest))
				{
					this._serverVariables.MakeReadOnly();
				}
			}
			return this._serverVariables;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000179A4 File Offset: 0x000169A4
		internal void SetSkipAuthorization(bool value)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest == null)
			{
				return;
			}
			if (this._serverVariables == null)
			{
				iis7WorkerRequest.SetServerVariable("IS_LOGIN_PAGE", value ? "1" : null);
				return;
			}
			this._serverVariables.SetNoDemand("IS_LOGIN_PAGE", value ? "1" : null);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000179FC File Offset: 0x000169FC
		internal void SetDynamicCompression(bool enable)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest == null)
			{
				return;
			}
			if (this._serverVariables == null)
			{
				iis7WorkerRequest.SetServerVariable("IIS_EnableDynamicCompression", enable ? null : "0");
				return;
			}
			this._serverVariables.SetNoDemand("IIS_EnableDynamicCompression", enable ? null : "0");
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00017A54 File Offset: 0x00016A54
		internal void AppendToLogQueryString(string logData)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest == null || string.IsNullOrEmpty(logData))
			{
				return;
			}
			if (this._serverVariables == null)
			{
				string serverVariable = iis7WorkerRequest.GetServerVariable("LOG_QUERY_STRING");
				if (string.IsNullOrEmpty(serverVariable))
				{
					iis7WorkerRequest.SetServerVariable("LOG_QUERY_STRING", this.QueryStringText + logData);
					return;
				}
				iis7WorkerRequest.SetServerVariable("LOG_QUERY_STRING", serverVariable + logData);
				return;
			}
			else
			{
				string text = this._serverVariables.Get("LOG_QUERY_STRING");
				if (string.IsNullOrEmpty(text))
				{
					this._serverVariables.SetNoDemand("LOG_QUERY_STRING", this.QueryStringText + logData);
					return;
				}
				this._serverVariables.SetNoDemand("LOG_QUERY_STRING", text + logData);
				return;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00017B10 File Offset: 0x00016B10
		public HttpCookieCollection Cookies
		{
			get
			{
				if (this._cookies == null)
				{
					this._cookies = new HttpCookieCollection(null, false);
					if (this._wr != null)
					{
						this.FillInCookiesCollection(this._cookies, true);
					}
				}
				if (this._flags[4])
				{
					this._flags.Clear(4);
					HttpRequest.ValidateCookieCollection(this._cookies);
				}
				return this._cookies;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00017B72 File Offset: 0x00016B72
		public HttpFileCollection Files
		{
			get
			{
				if (this._files == null)
				{
					this._files = new HttpFileCollection();
					if (this._wr != null)
					{
						this.FillInFilesCollection();
					}
				}
				return this._files;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00017B9C File Offset: 0x00016B9C
		public Stream InputStream
		{
			get
			{
				if (this._inputStream == null)
				{
					HttpRawUploadedContent httpRawUploadedContent = null;
					if (this._wr != null)
					{
						httpRawUploadedContent = this.GetEntireRawContent();
					}
					if (httpRawUploadedContent != null)
					{
						this._inputStream = new HttpInputStream(httpRawUploadedContent, 0, httpRawUploadedContent.Length);
					}
					else
					{
						this._inputStream = new HttpInputStream(null, 0, 0);
					}
				}
				return this._inputStream;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00017BF0 File Offset: 0x00016BF0
		public int TotalBytes
		{
			get
			{
				Stream inputStream = this.InputStream;
				if (inputStream == null)
				{
					return 0;
				}
				return (int)inputStream.Length;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00017C10 File Offset: 0x00016C10
		public byte[] BinaryRead(int count)
		{
			if (count < 0 || count > this.TotalBytes)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[count];
			int num = this.InputStream.Read(array, 0, count);
			if (num != count)
			{
				byte[] array2 = new byte[num];
				if (num > 0)
				{
					Array.Copy(array, array2, num);
				}
				array = array2;
			}
			return array;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00017C6F File Offset: 0x00016C6F
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00017C99 File Offset: 0x00016C99
		public Stream Filter
		{
			get
			{
				if (this._installedFilter != null)
				{
					return this._installedFilter;
				}
				if (this._filterSource == null)
				{
					this._filterSource = new HttpInputStreamFilterSource();
				}
				return this._filterSource;
			}
			set
			{
				if (this._filterSource == null)
				{
					throw new HttpException(SR.GetString("Invalid_request_filter"));
				}
				this._installedFilter = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00017CBA File Offset: 0x00016CBA
		public HttpClientCertificate ClientCertificate
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
			get
			{
				if (this._clientCertificate == null)
				{
					this._clientCertificate = this.CreateHttpClientCertificateWithAssert();
				}
				return this._clientCertificate;
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00017CD6 File Offset: 0x00016CD6
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private HttpClientCertificate CreateHttpClientCertificateWithAssert()
		{
			return new HttpClientCertificate(this._context);
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00017CE4 File Offset: 0x00016CE4
		public WindowsIdentity LogonUserIdentity
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
			get
			{
				if (this._logonUserIdentity == null && this._wr != null)
				{
					if (this._wr is IIS7WorkerRequest && ((this._context.NotificationContext.CurrentNotification == RequestNotification.AuthenticateRequest && !this._context.NotificationContext.IsPostNotification) || this._context.NotificationContext.CurrentNotification < RequestNotification.AuthenticateRequest))
					{
						throw new InvalidOperationException(SR.GetString("Invalid_before_authentication"));
					}
					IntPtr userToken = this._wr.GetUserToken();
					if (userToken != IntPtr.Zero)
					{
						string serverVariable = this._wr.GetServerVariable("LOGON_USER");
						string serverVariable2 = this._wr.GetServerVariable("AUTH_TYPE");
						bool flag = !string.IsNullOrEmpty(serverVariable) || (!string.IsNullOrEmpty(serverVariable2) && !StringUtil.EqualsIgnoreCase(serverVariable2, "basic"));
						this._logonUserIdentity = HttpRequest.CreateWindowsIdentityWithAssert(userToken, (serverVariable2 == null) ? "" : serverVariable2, WindowsAccountType.Normal, flag);
					}
				}
				return this._logonUserIdentity;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00017DDB File Offset: 0x00016DDB
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private static WindowsIdentity CreateWindowsIdentityWithAssert(IntPtr token, string authType, WindowsAccountType accountType, bool isAuthenticated)
		{
			return new WindowsIdentity(token, authType, accountType, isAuthenticated);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00017DE8 File Offset: 0x00016DE8
		public void ValidateInput()
		{
			this._flags.Set(1);
			this._flags.Set(2);
			this._flags.Set(4);
			this._flags.Set(64);
			this._flags.Set(128);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00017E36 File Offset: 0x00016E36
		internal string ValidateRawUrl()
		{
			return this.RawUrl;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00017E3E File Offset: 0x00016E3E
		private static string RemoveNullCharacters(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('\0') > -1)
			{
				return s.Replace("\0", string.Empty);
			}
			return s;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00017E64 File Offset: 0x00016E64
		private static void ValidateString(string s, string valueName, string collectionName)
		{
			s = HttpRequest.RemoveNullCharacters(s);
			int num = 0;
			if (CrossSiteScriptingValidation.IsDangerousString(s, out num))
			{
				string text = valueName + "=\"";
				int num2 = num - 10;
				if (num2 <= 0)
				{
					num2 = 0;
				}
				else
				{
					text += "...";
				}
				int num3 = num + 20;
				if (num3 >= s.Length)
				{
					num3 = s.Length;
					text = text + s.Substring(num2, num3 - num2) + "\"";
				}
				else
				{
					text = text + s.Substring(num2, num3 - num2) + "...\"";
				}
				throw new HttpRequestValidationException(SR.GetString("Dangerous_input_detected", new object[] { collectionName, text }));
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00017F14 File Offset: 0x00016F14
		private static void ValidateNameValueCollection(NameValueCollection nvc, string collectionName)
		{
			int count = nvc.Count;
			for (int i = 0; i < count; i++)
			{
				string key = nvc.GetKey(i);
				if (key == null || !key.StartsWith("__", StringComparison.Ordinal))
				{
					string text = nvc.Get(i);
					if (!string.IsNullOrEmpty(text))
					{
						HttpRequest.ValidateString(text, key, collectionName);
					}
				}
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00017F68 File Offset: 0x00016F68
		private static void ValidateCookieCollection(HttpCookieCollection cc)
		{
			int count = cc.Count;
			for (int i = 0; i < count; i++)
			{
				string key = cc.GetKey(i);
				string value = cc.Get(i).Value;
				if (!string.IsNullOrEmpty(value))
				{
					HttpRequest.ValidateString(value, key, "Request.Cookies");
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00017FB4 File Offset: 0x00016FB4
		public int[] MapImageCoordinates(string imageFieldName)
		{
			switch (this.HttpVerb)
			{
			case HttpVerb.GET:
			case HttpVerb.HEAD:
			{
				NameValueCollection nameValueCollection = this.QueryString;
				goto IL_0039;
			}
			case HttpVerb.POST:
			{
				NameValueCollection nameValueCollection = this.Form;
				goto IL_0039;
			}
			}
			return null;
			IL_0039:
			int[] array = null;
			try
			{
				NameValueCollection nameValueCollection;
				string text = nameValueCollection[imageFieldName + ".x"];
				string text2 = nameValueCollection[imageFieldName + ".y"];
				if (text != null && text2 != null)
				{
					array = new int[]
					{
						int.Parse(text, CultureInfo.InvariantCulture),
						int.Parse(text2, CultureInfo.InvariantCulture)
					};
				}
			}
			catch
			{
			}
			return array;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00018068 File Offset: 0x00017068
		public void SaveAs(string filename, bool includeHeaders)
		{
			if (!global::System.IO.Path.IsPathRooted(filename))
			{
				HttpRuntimeSection httpRuntime = RuntimeConfig.GetConfig(this._context).HttpRuntime;
				if (httpRuntime.RequireRootedSaveAsPath)
				{
					throw new HttpException(SR.GetString("SaveAs_requires_rooted_path", new object[] { filename }));
				}
			}
			FileStream fileStream = new FileStream(filename, FileMode.Create);
			try
			{
				if (includeHeaders)
				{
					TextWriter textWriter = new StreamWriter(fileStream);
					textWriter.Write(this.HttpMethod + " " + this.Path);
					string queryStringText = this.QueryStringText;
					if (!string.IsNullOrEmpty(queryStringText))
					{
						textWriter.Write("?" + queryStringText);
					}
					if (this._wr != null)
					{
						textWriter.Write(" " + this._wr.GetHttpVersion() + "\r\n");
						textWriter.Write(this.CombineAllHeaders(true));
					}
					else
					{
						textWriter.Write("\r\n");
					}
					textWriter.Write("\r\n");
					textWriter.Flush();
				}
				HttpInputStream httpInputStream = (HttpInputStream)this.InputStream;
				httpInputStream.WriteTo(fileStream);
				fileStream.Flush();
			}
			finally
			{
				fileStream.Close();
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00018190 File Offset: 0x00017190
		public string MapPath(string virtualPath)
		{
			return this.MapPath(VirtualPath.CreateAllowNull(virtualPath));
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001819E File Offset: 0x0001719E
		internal string MapPath(VirtualPath virtualPath)
		{
			if (this._wr != null)
			{
				return this.MapPath(virtualPath, this.FilePathObject, true);
			}
			return virtualPath.MapPath();
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000181C0 File Offset: 0x000171C0
		public string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
		{
			VirtualPath virtualPath2;
			if (string.IsNullOrEmpty(baseVirtualDir))
			{
				virtualPath2 = this.FilePathObject;
			}
			else
			{
				virtualPath2 = VirtualPath.CreateTrailingSlash(baseVirtualDir);
			}
			return this.MapPath(VirtualPath.CreateAllowNull(virtualPath), virtualPath2, allowCrossAppMapping);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000181F4 File Offset: 0x000171F4
		internal string MapPath(VirtualPath virtualPath, VirtualPath baseVirtualDir, bool allowCrossAppMapping)
		{
			if (this._wr == null)
			{
				throw new HttpException(SR.GetString("Cannot_map_path_without_context"));
			}
			if (virtualPath == null)
			{
				virtualPath = VirtualPath.Create(".");
			}
			VirtualPath virtualPath2 = virtualPath;
			if (baseVirtualDir != null)
			{
				virtualPath = baseVirtualDir.Combine(virtualPath);
			}
			if (!allowCrossAppMapping)
			{
				virtualPath.FailIfNotWithinAppRoot();
			}
			string text = virtualPath.MapPathInternal();
			if (virtualPath.VirtualPathString == "/" && virtualPath2.VirtualPathString != "/" && !virtualPath2.HasTrailingSlash && UrlPath.PathEndsWithExtraSlash(text))
			{
				text = text.Substring(0, text.Length - 1);
			}
			InternalSecurityPermissions.PathDiscovery(text).Demand();
			return text;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000182A4 File Offset: 0x000172A4
		internal void InternalRewritePath(VirtualPath newPath, string newQueryString, bool rebaseClientPath)
		{
			this._pathTranslated = null;
			this._pathInfo = null;
			this._filePath = null;
			this._url = null;
			if (this._wr != null)
			{
				this._wr.GetRawUrl();
			}
			this._path = newPath;
			if (rebaseClientPath)
			{
				this._clientBaseDir = null;
				this._clientFilePath = newPath;
			}
			this._computePathInfo = true;
			if (newQueryString != null)
			{
				this.QueryStringText = newQueryString;
			}
			this._rewrittenUrl = this._path.VirtualPathString;
			string queryStringText = this.QueryStringText;
			if (!string.IsNullOrEmpty(queryStringText))
			{
				this._rewrittenUrl = this._rewrittenUrl + "?" + queryStringText;
			}
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				iis7WorkerRequest.RewriteNotifyPipeline(this._path.VirtualPathString, newQueryString, rebaseClientPath);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00018364 File Offset: 0x00017364
		internal void InternalRewritePath(VirtualPath newFilePath, VirtualPath newPathInfo, string newQueryString, bool setClientFilePath)
		{
			this._pathTranslated = ((this._wr != null) ? newFilePath.MapPathInternal() : null);
			this._pathInfo = newPathInfo;
			this._filePath = newFilePath;
			this._url = null;
			if (this._wr != null)
			{
				this._wr.GetRawUrl();
			}
			if (newPathInfo == null)
			{
				this._path = newFilePath;
			}
			else
			{
				string text = newFilePath.VirtualPathStringWhicheverAvailable + "/" + newPathInfo.VirtualPathString;
				this._path = VirtualPath.Create(text);
			}
			if (newQueryString != null)
			{
				this.QueryStringText = newQueryString;
			}
			this._rewrittenUrl = this._path.VirtualPathString;
			string queryStringText = this.QueryStringText;
			if (!string.IsNullOrEmpty(queryStringText))
			{
				this._rewrittenUrl = this._rewrittenUrl + "?" + queryStringText;
			}
			this._computePathInfo = false;
			if (setClientFilePath)
			{
				this._clientFilePath = newFilePath;
			}
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				string text2 = ((this._path != null && this._path.VirtualPathString != null) ? this._path.VirtualPathString : string.Empty);
				iis7WorkerRequest.RewriteNotifyPipeline(text2, newQueryString, setClientFilePath);
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00018480 File Offset: 0x00017480
		internal string RewrittenUrl
		{
			get
			{
				return this._rewrittenUrl;
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00018488 File Offset: 0x00017488
		internal string FetchServerVariable(string variable)
		{
			return this._wr.GetServerVariable(variable);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00018498 File Offset: 0x00017498
		internal void SynchronizeServerVariable(string name, string value)
		{
			if (name == "IS_LOGIN_PAGE")
			{
				bool flag = value != null && value != "0";
				this._context.SetSkipAuthorizationNoDemand(flag, true);
			}
			HttpServerVarsCollection httpServerVarsCollection = this.ServerVariables as HttpServerVarsCollection;
			if (httpServerVarsCollection != null)
			{
				httpServerVarsCollection.SynchronizeServerVariable(name, value);
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000184EC File Offset: 0x000174EC
		internal void SynchronizeHeader(string name, string value)
		{
			HttpHeaderCollection httpHeaderCollection = this.Headers as HttpHeaderCollection;
			if (httpHeaderCollection != null)
			{
				httpHeaderCollection.SynchronizeHeader(name, value);
			}
			HttpServerVarsCollection httpServerVarsCollection = this.ServerVariables as HttpServerVarsCollection;
			if (httpServerVarsCollection != null)
			{
				string text = "HTTP_" + name.ToUpper(CultureInfo.InvariantCulture).Replace('-', '_');
				httpServerVarsCollection.SynchronizeServerVariable(text, value);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00018548 File Offset: 0x00017548
		public ChannelBinding HttpChannelBinding
		{
			[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
			get
			{
				if (this._wr is IIS7WorkerRequest)
				{
					return ((IIS7WorkerRequest)this._wr).HttpChannelBindingToken;
				}
				if (this._wr is ISAPIWorkerRequestInProc)
				{
					return ((ISAPIWorkerRequestInProc)this._wr).HttpChannelBindingToken;
				}
				throw new PlatformNotSupportedException();
			}
		}

		// Token: 0x04001073 RID: 4211
		private const int needToValidateQueryString = 1;

		// Token: 0x04001074 RID: 4212
		private const int needToValidateForm = 2;

		// Token: 0x04001075 RID: 4213
		private const int needToValidateCookies = 4;

		// Token: 0x04001076 RID: 4214
		private const int needToValidateHeaders = 8;

		// Token: 0x04001077 RID: 4215
		private const int needToValidateServerVariables = 16;

		// Token: 0x04001078 RID: 4216
		private const int contentEncodingResolved = 32;

		// Token: 0x04001079 RID: 4217
		private const int needToValidatePostedFiles = 64;

		// Token: 0x0400107A RID: 4218
		private const int needToValidateRawUrl = 128;

		// Token: 0x0400107B RID: 4219
		private HttpWorkerRequest _wr;

		// Token: 0x0400107C RID: 4220
		private HttpContext _context;

		// Token: 0x0400107D RID: 4221
		private string _httpMethod;

		// Token: 0x0400107E RID: 4222
		private HttpVerb _httpVerb;

		// Token: 0x0400107F RID: 4223
		private string _requestType;

		// Token: 0x04001080 RID: 4224
		private VirtualPath _path;

		// Token: 0x04001081 RID: 4225
		private string _rewrittenUrl;

		// Token: 0x04001082 RID: 4226
		private bool _computePathInfo;

		// Token: 0x04001083 RID: 4227
		private VirtualPath _filePath;

		// Token: 0x04001084 RID: 4228
		private VirtualPath _currentExecutionFilePath;

		// Token: 0x04001085 RID: 4229
		private VirtualPath _pathInfo;

		// Token: 0x04001086 RID: 4230
		private string _queryStringText;

		// Token: 0x04001087 RID: 4231
		private bool _queryStringOverriden;

		// Token: 0x04001088 RID: 4232
		private byte[] _queryStringBytes;

		// Token: 0x04001089 RID: 4233
		private string _pathTranslated;

		// Token: 0x0400108A RID: 4234
		private string _contentType;

		// Token: 0x0400108B RID: 4235
		private int _contentLength = -1;

		// Token: 0x0400108C RID: 4236
		private string _clientTarget;

		// Token: 0x0400108D RID: 4237
		private string[] _acceptTypes;

		// Token: 0x0400108E RID: 4238
		private string[] _userLanguages;

		// Token: 0x0400108F RID: 4239
		private HttpBrowserCapabilities _browsercaps;

		// Token: 0x04001090 RID: 4240
		private Uri _url;

		// Token: 0x04001091 RID: 4241
		private Uri _referrer;

		// Token: 0x04001092 RID: 4242
		private HttpInputStream _inputStream;

		// Token: 0x04001093 RID: 4243
		private HttpClientCertificate _clientCertificate;

		// Token: 0x04001094 RID: 4244
		private WindowsIdentity _logonUserIdentity;

		// Token: 0x04001095 RID: 4245
		private HttpValueCollection _params;

		// Token: 0x04001096 RID: 4246
		private HttpValueCollection _queryString;

		// Token: 0x04001097 RID: 4247
		private HttpValueCollection _form;

		// Token: 0x04001098 RID: 4248
		private HttpHeaderCollection _headers;

		// Token: 0x04001099 RID: 4249
		private HttpServerVarsCollection _serverVariables;

		// Token: 0x0400109A RID: 4250
		private HttpCookieCollection _cookies;

		// Token: 0x0400109B RID: 4251
		private HttpFileCollection _files;

		// Token: 0x0400109C RID: 4252
		private HttpRawUploadedContent _rawContent;

		// Token: 0x0400109D RID: 4253
		private bool _readEntityBody;

		// Token: 0x0400109E RID: 4254
		private MultipartContentElement[] _multipartContentElements;

		// Token: 0x0400109F RID: 4255
		private Encoding _encoding;

		// Token: 0x040010A0 RID: 4256
		private HttpInputStreamFilterSource _filterSource;

		// Token: 0x040010A1 RID: 4257
		private Stream _installedFilter;

		// Token: 0x040010A2 RID: 4258
		private SimpleBitVector32 _flags;

		// Token: 0x040010A3 RID: 4259
		internal static object s_browserLock = new object();

		// Token: 0x040010A4 RID: 4260
		internal static bool s_browserCapsEvaled = false;

		// Token: 0x040010A5 RID: 4261
		internal string _AnonymousId;

		// Token: 0x040010A6 RID: 4262
		private VirtualPath _clientFilePath;

		// Token: 0x040010A7 RID: 4263
		private VirtualPath _clientBaseDir;
	}
}
