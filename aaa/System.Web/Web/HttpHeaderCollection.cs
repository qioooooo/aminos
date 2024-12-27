using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	internal class HttpHeaderCollection : HttpValueCollection
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x000148BE File Offset: 0x000138BE
		internal HttpHeaderCollection(HttpWorkerRequest wr, HttpRequest request, int capacity)
			: base(capacity)
		{
			this._iis7WorkerRequest = wr as IIS7WorkerRequest;
			this._request = request;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000148DA File Offset: 0x000138DA
		internal HttpHeaderCollection(HttpWorkerRequest wr, HttpResponse response, int capacity)
			: base(capacity)
		{
			this._iis7WorkerRequest = wr as IIS7WorkerRequest;
			this._response = response;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000148F6 File Offset: 0x000138F6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.SetType(typeof(HttpValueCollection));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00014910 File Offset: 0x00013910
		public override void Add(string name, string value)
		{
			if (this._iis7WorkerRequest == null)
			{
				throw new PlatformNotSupportedException();
			}
			this.SetHeader(name, value, false);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00014929 File Offset: 0x00013929
		public override void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00014930 File Offset: 0x00013930
		internal void ClearInternal()
		{
			if (this._request != null)
			{
				throw new NotSupportedException();
			}
			base.Clear();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00014946 File Offset: 0x00013946
		public override void Set(string name, string value)
		{
			if (this._iis7WorkerRequest == null)
			{
				throw new PlatformNotSupportedException();
			}
			this.SetHeader(name, value, true);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00014960 File Offset: 0x00013960
		internal void SetHeader(string name, string value, bool replace)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this._request != null)
			{
				this._iis7WorkerRequest.SetRequestHeader(name, value, replace);
			}
			else
			{
				if (this._response.HeadersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_append_header_after_headers_sent"));
				}
				this._iis7WorkerRequest.SetHeaderEncoding(this._response.HeaderEncoding);
				this._iis7WorkerRequest.SetResponseHeader(name, value, replace);
				if (this._response.HasCachePolicy && StringUtil.EqualsIgnoreCase("Set-Cookie", name))
				{
					this._response.Cache.SetHasSetCookieHeader();
				}
			}
			if (replace)
			{
				base.Set(name, value);
			}
			else
			{
				base.Add(name, value);
			}
			if (this._request != null)
			{
				string text = (replace ? value : base.Get(name));
				HttpServerVarsCollection httpServerVarsCollection = this._request.ServerVariables as HttpServerVarsCollection;
				if (httpServerVarsCollection != null)
				{
					httpServerVarsCollection.SynchronizeServerVariable("HTTP_" + name.ToUpper(CultureInfo.InvariantCulture).Replace('-', '_'), text);
				}
				this._request.InvalidateParams();
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00014A7B File Offset: 0x00013A7B
		internal void SynchronizeHeader(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value != null)
			{
				base.Set(name, value);
			}
			else
			{
				base.Remove(name);
			}
			if (this._request != null)
			{
				this._request.InvalidateParams();
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00014AB4 File Offset: 0x00013AB4
		public override void Remove(string name)
		{
			if (this._iis7WorkerRequest == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._request != null)
			{
				this._iis7WorkerRequest.SetRequestHeader(name, null, false);
			}
			else
			{
				this._iis7WorkerRequest.SetResponseHeader(name, null, false);
			}
			base.Remove(name);
			if (this._request != null)
			{
				HttpServerVarsCollection httpServerVarsCollection = this._request.ServerVariables as HttpServerVarsCollection;
				if (httpServerVarsCollection != null)
				{
					httpServerVarsCollection.SynchronizeServerVariable("HTTP_" + name.ToUpper(CultureInfo.InvariantCulture).Replace('-', '_'), null);
				}
				this._request.InvalidateParams();
			}
		}

		// Token: 0x04001047 RID: 4167
		private HttpRequest _request;

		// Token: 0x04001048 RID: 4168
		private HttpResponse _response;

		// Token: 0x04001049 RID: 4169
		private IIS7WorkerRequest _iis7WorkerRequest;
	}
}
