using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000090 RID: 144
	internal class HttpServerVarsCollection : HttpValueCollection
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x000222FB File Offset: 0x000212FB
		internal HttpServerVarsCollection(HttpWorkerRequest wr, HttpRequest request)
			: base(59)
		{
			this._iis7workerRequest = wr as IIS7WorkerRequest;
			this._request = request;
			this._populated = false;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002231F File Offset: 0x0002131F
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new SerializationException();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00022326 File Offset: 0x00021326
		internal void Dispose()
		{
			this._request = null;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002232F File Offset: 0x0002132F
		internal void AddStatic(string name, string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			base.InvalidateCachedArrays();
			base.BaseAdd(name, new HttpServerVarsCollectionEntry(name, value));
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002234F File Offset: 0x0002134F
		internal void AddDynamic(string name, DynamicServerVariable var)
		{
			base.InvalidateCachedArrays();
			base.BaseAdd(name, new HttpServerVarsCollectionEntry(name, var));
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00022368 File Offset: 0x00021368
		private string GetServerVar(object e)
		{
			HttpServerVarsCollectionEntry httpServerVarsCollectionEntry = (HttpServerVarsCollectionEntry)e;
			if (httpServerVarsCollectionEntry == null)
			{
				return null;
			}
			return httpServerVarsCollectionEntry.GetValue(this._request);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002238D File Offset: 0x0002138D
		private void Populate()
		{
			if (!this._populated)
			{
				if (this._request != null)
				{
					base.MakeReadWrite();
					this._request.FillInServerVariablesCollection();
					if (this._iis7workerRequest == null)
					{
						base.MakeReadOnly();
					}
				}
				this._populated = true;
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000223C8 File Offset: 0x000213C8
		private string GetSimpleServerVar(string name)
		{
			if (name != null && name.Length > 1 && this._request != null)
			{
				char c = name[0];
				if (c <= 'S')
				{
					if (c != 'A')
					{
						if (c == 'H')
						{
							goto IL_00B1;
						}
						switch (c)
						{
						case 'P':
							goto IL_00E9;
						case 'Q':
							goto IL_00CD;
						case 'R':
							goto IL_011E;
						case 'S':
							goto IL_0183;
						default:
							goto IL_019C;
						}
					}
				}
				else if (c != 'a')
				{
					if (c == 'h')
					{
						goto IL_00B1;
					}
					switch (c)
					{
					case 'p':
						goto IL_00E9;
					case 'q':
						goto IL_00CD;
					case 'r':
						goto IL_011E;
					case 's':
						goto IL_0183;
					default:
						goto IL_019C;
					}
				}
				if (StringUtil.EqualsIgnoreCase(name, "AUTH_TYPE"))
				{
					return this._request.CalcDynamicServerVariable(DynamicServerVariable.AUTH_TYPE);
				}
				if (StringUtil.EqualsIgnoreCase(name, "AUTH_USER"))
				{
					return this._request.CalcDynamicServerVariable(DynamicServerVariable.AUTH_USER);
				}
				goto IL_019C;
				IL_00B1:
				if (StringUtil.EqualsIgnoreCase(name, "HTTP_USER_AGENT"))
				{
					return this._request.UserAgent;
				}
				goto IL_019C;
				IL_00CD:
				if (StringUtil.EqualsIgnoreCase(name, "QUERY_STRING"))
				{
					return this._request.QueryStringText;
				}
				goto IL_019C;
				IL_00E9:
				if (StringUtil.EqualsIgnoreCase(name, "PATH_INFO"))
				{
					return this._request.Path;
				}
				if (StringUtil.EqualsIgnoreCase(name, "PATH_TRANSLATED"))
				{
					return this._request.PhysicalPath;
				}
				goto IL_019C;
				IL_011E:
				if (StringUtil.EqualsIgnoreCase(name, "REQUEST_METHOD"))
				{
					return this._request.HttpMethod;
				}
				if (StringUtil.EqualsIgnoreCase(name, "REMOTE_USER"))
				{
					return this._request.CalcDynamicServerVariable(DynamicServerVariable.AUTH_USER);
				}
				if (StringUtil.EqualsIgnoreCase(name, "REMOTE_HOST"))
				{
					return this._request.UserHostName;
				}
				if (StringUtil.EqualsIgnoreCase(name, "REMOTE_ADDRESS"))
				{
					return this._request.UserHostAddress;
				}
				goto IL_019C;
				IL_0183:
				if (StringUtil.EqualsIgnoreCase(name, "SCRIPT_NAME"))
				{
					return this._request.FilePath;
				}
			}
			IL_019C:
			return null;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00022572 File Offset: 0x00021572
		public override IEnumerator GetEnumerator()
		{
			this.Populate();
			return base.GetEnumerator();
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00022580 File Offset: 0x00021580
		public override int Count
		{
			get
			{
				this.Populate();
				return base.Count;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002258E File Offset: 0x0002158E
		public override void Add(string name, string value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00022595 File Offset: 0x00021595
		public override void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002259C File Offset: 0x0002159C
		public override string Get(string name)
		{
			if (!this._populated)
			{
				string simpleServerVar = this.GetSimpleServerVar(name);
				if (simpleServerVar != null)
				{
					return simpleServerVar;
				}
				this.Populate();
			}
			if (this._iis7workerRequest != null)
			{
				string text = this.GetServerVar(base.BaseGet(name));
				if (string.IsNullOrEmpty(text))
				{
					text = this._request.FetchServerVariable(name);
				}
				return text;
			}
			return this.GetServerVar(base.BaseGet(name));
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00022600 File Offset: 0x00021600
		public override string[] GetValues(string name)
		{
			string text = this.Get(name);
			if (text == null)
			{
				return null;
			}
			return new string[] { text };
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00022626 File Offset: 0x00021626
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
		public override void Set(string name, string value)
		{
			if (this._iis7workerRequest == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.SetNoDemand(name, value);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002264C File Offset: 0x0002164C
		internal void SetNoDemand(string name, string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			this._iis7workerRequest.SetServerVariable(name, value);
			this.SetServerVariableManagedOnly(name, value);
			this.SynchronizeHeader(name, value);
			this._request.InvalidateParams();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00022680 File Offset: 0x00021680
		private void SynchronizeHeader(string name, string value)
		{
			if (StringUtil.StringStartsWith(name, "HTTP_"))
			{
				string text = name.Substring("HTTP_".Length);
				text = text.Replace('_', '-');
				int knownRequestHeaderIndex = HttpWorkerRequest.GetKnownRequestHeaderIndex(text);
				if (knownRequestHeaderIndex > -1)
				{
					text = HttpWorkerRequest.GetKnownRequestHeaderName(knownRequestHeaderIndex);
				}
				HttpHeaderCollection httpHeaderCollection = this._request.Headers as HttpHeaderCollection;
				if (httpHeaderCollection != null)
				{
					httpHeaderCollection.SynchronizeHeader(text, value);
				}
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000226E4 File Offset: 0x000216E4
		internal void SynchronizeServerVariable(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value != null)
			{
				this.SetServerVariableManagedOnly(name, value);
			}
			else
			{
				base.Remove(name);
			}
			this._request.InvalidateParams();
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00022714 File Offset: 0x00021714
		private void SetServerVariableManagedOnly(string name, string value)
		{
			this.Populate();
			HttpServerVarsCollectionEntry httpServerVarsCollectionEntry = (HttpServerVarsCollectionEntry)base.BaseGet(name);
			if (httpServerVarsCollectionEntry != null && httpServerVarsCollectionEntry.IsDynamic)
			{
				throw new HttpException(SR.GetString("Server_variable_cannot_be_modified"));
			}
			base.InvalidateCachedArrays();
			base.BaseSet(name, new HttpServerVarsCollectionEntry(name, value));
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00022763 File Offset: 0x00021763
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
		public override void Remove(string name)
		{
			if (this._iis7workerRequest == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.RemoveNoDemand(name);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00022788 File Offset: 0x00021788
		internal void RemoveNoDemand(string name)
		{
			this._iis7workerRequest.SetServerVariable(name, null);
			base.Remove(name);
			this.SynchronizeHeader(name, null);
			this._request.InvalidateParams();
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000227B1 File Offset: 0x000217B1
		public override string Get(int index)
		{
			this.Populate();
			return this.GetServerVar(base.BaseGet(index));
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000227C8 File Offset: 0x000217C8
		public override string[] GetValues(int index)
		{
			string text = this.Get(index);
			if (text == null)
			{
				return null;
			}
			return new string[] { text };
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x000227EE File Offset: 0x000217EE
		public override string GetKey(int index)
		{
			this.Populate();
			return base.GetKey(index);
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000227FD File Offset: 0x000217FD
		public override string[] AllKeys
		{
			get
			{
				this.Populate();
				return base.AllKeys;
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002280C File Offset: 0x0002180C
		internal override string ToString(bool urlencoded)
		{
			this.Populate();
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('&');
				}
				string text = this.GetKey(i);
				if (urlencoded)
				{
					text = HttpUtility.UrlEncodeUnicode(text);
				}
				stringBuilder.Append(text);
				stringBuilder.Append('=');
				string text2 = this.Get(i);
				if (urlencoded)
				{
					text2 = HttpUtility.UrlEncodeUnicode(text2);
				}
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001159 RID: 4441
		private bool _populated;

		// Token: 0x0400115A RID: 4442
		private HttpRequest _request;

		// Token: 0x0400115B RID: 4443
		private IIS7WorkerRequest _iis7workerRequest;
	}
}
