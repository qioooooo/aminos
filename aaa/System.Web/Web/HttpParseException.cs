using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200006E RID: 110
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class HttpParseException : HttpException
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x00014068 File Offset: 0x00013068
		public HttpParseException()
		{
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00014070 File Offset: 0x00013070
		public HttpParseException(string message)
			: base(message)
		{
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00014079 File Offset: 0x00013079
		public HttpParseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00014083 File Offset: 0x00013083
		public HttpParseException(string message, Exception innerException, string virtualPath, string sourceCode, int line)
			: this(message, innerException, global::System.Web.VirtualPath.CreateAllowNull(virtualPath), sourceCode, line)
		{
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00014098 File Offset: 0x00013098
		internal HttpParseException(string message, Exception innerException, VirtualPath virtualPath, string sourceCode, int line)
			: base(message, innerException)
		{
			this._virtualPath = virtualPath;
			this._line = line;
			string text;
			if (innerException != null)
			{
				text = innerException.Message;
			}
			else
			{
				text = message;
			}
			base.SetFormatter(new ParseErrorFormatter(this, global::System.Web.VirtualPath.GetVirtualPathString(virtualPath), sourceCode, line, text));
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000140E4 File Offset: 0x000130E4
		private HttpParseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._virtualPath = (VirtualPath)info.GetValue("_virtualPath", typeof(VirtualPath));
			this._line = info.GetInt32("_line");
			this._parserErrors = (ParserErrorCollection)info.GetValue("_parserErrors", typeof(ParserErrorCollection));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001414A File Offset: 0x0001314A
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_virtualPath", this._virtualPath);
			info.AddValue("_line", this._line);
			info.AddValue("_parserErrors", this._parserErrors);
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00014188 File Offset: 0x00013188
		public string FileName
		{
			get
			{
				string text = this._virtualPath.MapPathInternal();
				if (text == null)
				{
					return null;
				}
				InternalSecurityPermissions.PathDiscovery(text).Demand();
				return text;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000141B2 File Offset: 0x000131B2
		public string VirtualPath
		{
			get
			{
				return global::System.Web.VirtualPath.GetVirtualPathString(this._virtualPath);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000141BF File Offset: 0x000131BF
		internal VirtualPath VirtualPathObject
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000141C7 File Offset: 0x000131C7
		public int Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000141D0 File Offset: 0x000131D0
		public ParserErrorCollection ParserErrors
		{
			get
			{
				if (this._parserErrors == null)
				{
					this._parserErrors = new ParserErrorCollection();
					ParserError parserError = new ParserError(this.Message, this._virtualPath, this._line);
					this._parserErrors.Add(parserError);
				}
				return this._parserErrors;
			}
		}

		// Token: 0x0400103E RID: 4158
		private VirtualPath _virtualPath;

		// Token: 0x0400103F RID: 4159
		private int _line;

		// Token: 0x04001040 RID: 4160
		private ParserErrorCollection _parserErrors;
	}
}
