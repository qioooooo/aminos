using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200048B RID: 1163
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class ViewStateException : Exception, ISerializable
	{
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06003688 RID: 13960 RVA: 0x000EB634 File Offset: 0x000EA634
		public override string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06003689 RID: 13961 RVA: 0x000EB63C File Offset: 0x000EA63C
		public string RemoteAddress
		{
			get
			{
				return this._remoteAddr;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x0600368A RID: 13962 RVA: 0x000EB644 File Offset: 0x000EA644
		public string RemotePort
		{
			get
			{
				return this._remotePort;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600368B RID: 13963 RVA: 0x000EB64C File Offset: 0x000EA64C
		public string UserAgent
		{
			get
			{
				return this._userAgent;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600368C RID: 13964 RVA: 0x000EB654 File Offset: 0x000EA654
		public string PersistedState
		{
			get
			{
				return this._persistedState;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x0600368D RID: 13965 RVA: 0x000EB65C File Offset: 0x000EA65C
		public string Referer
		{
			get
			{
				return this._referer;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x0600368E RID: 13966 RVA: 0x000EB664 File Offset: 0x000EA664
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x000EB66C File Offset: 0x000EA66C
		public bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000EB674 File Offset: 0x000EA674
		private ViewStateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000EB685 File Offset: 0x000EA685
		public ViewStateException()
		{
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000EB694 File Offset: 0x000EA694
		private ViewStateException(string message)
		{
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000EB6A3 File Offset: 0x000EA6A3
		private ViewStateException(string message, Exception e)
		{
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000EB6B2 File Offset: 0x000EA6B2
		private ViewStateException(Exception innerException, string persistedState)
			: base(null, innerException)
		{
			this.Initialize(persistedState);
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000EB6CC File Offset: 0x000EA6CC
		private void Initialize(string persistedState)
		{
			this._persistedState = persistedState;
			HttpContext httpContext = HttpContext.Current;
			HttpRequest httpRequest = ((httpContext != null) ? httpContext.Request : null);
			HttpResponse httpResponse = ((httpContext != null) ? httpContext.Response : null);
			if (httpRequest == null || httpResponse == null || !HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low))
			{
				this._message = this.ShortMessage;
				return;
			}
			this._isConnected = httpResponse.IsClientConnected;
			this._remoteAddr = httpRequest.ServerVariables["REMOTE_ADDR"];
			this._remotePort = httpRequest.ServerVariables["REMOTE_PORT"];
			this._userAgent = httpRequest.ServerVariables["HTTP_USER_AGENT"];
			this._referer = httpRequest.ServerVariables["HTTP_REFERER"];
			this._path = httpRequest.ServerVariables["PATH_INFO"];
			string text = string.Format(CultureInfo.InvariantCulture, "\r\n\tClient IP: {0}\r\n\tPort: {1}\r\n\tUser-Agent: {2}\r\n\tViewState: {3}\r\n\tReferer: {4}\r\n\tPath: {5}", new object[] { this._remoteAddr, this._remotePort, this._userAgent, this._persistedState, this._referer, this._path });
			this._message = SR.GetString("ViewState_InvalidViewStatePlus", new object[] { text });
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000EB80E File Offset: 0x000EA80E
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003697 RID: 13975 RVA: 0x000EB818 File Offset: 0x000EA818
		internal string ShortMessage
		{
			get
			{
				return "ViewState_InvalidViewState";
			}
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000EB81F File Offset: 0x000EA81F
		private static string GetCorrectErrorPageMessage(ViewStateException vse, string message)
		{
			if (!vse.IsConnected)
			{
				return SR.GetString("ViewState_ClientDisconnected");
			}
			return SR.GetString(message);
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000EB83C File Offset: 0x000EA83C
		private static void ThrowError(Exception inner, string persistedState, string errorPageMessage, bool macValidationError)
		{
			ViewStateException ex = new ViewStateException(inner, persistedState);
			ex._macValidationError = macValidationError;
			string text = ViewStateException.GetCorrectErrorPageMessage(ex, errorPageMessage);
			if (macValidationError)
			{
				text += "\r\n\r\nhttp://go.microsoft.com/fwlink/?LinkID=314055";
			}
			HttpException ex2 = new HttpException(text, ex);
			ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
			throw ex2;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000EB884 File Offset: 0x000EA884
		internal static void ThrowMacValidationError(Exception inner, string persistedState)
		{
			ViewStateException.ThrowError(inner, persistedState, "ViewState_AuthenticationFailed", true);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000EB893 File Offset: 0x000EA893
		internal static void ThrowViewStateError(Exception inner, string persistedState)
		{
			ViewStateException.ThrowError(inner, persistedState, "Invalid_ControlState", false);
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000EB8A4 File Offset: 0x000EA8A4
		internal static bool IsMacValidationException(Exception e)
		{
			while (e != null)
			{
				ViewStateException ex = e as ViewStateException;
				if (ex != null && ex._macValidationError)
				{
					return true;
				}
				e = e.InnerException;
			}
			return false;
		}

		// Token: 0x0400259D RID: 9629
		private const string _format = "\r\n\tClient IP: {0}\r\n\tPort: {1}\r\n\tUser-Agent: {2}\r\n\tViewState: {3}\r\n\tReferer: {4}\r\n\tPath: {5}";

		// Token: 0x0400259E RID: 9630
		private bool _isConnected = true;

		// Token: 0x0400259F RID: 9631
		private string _remoteAddr;

		// Token: 0x040025A0 RID: 9632
		private string _remotePort;

		// Token: 0x040025A1 RID: 9633
		private string _userAgent;

		// Token: 0x040025A2 RID: 9634
		private string _persistedState;

		// Token: 0x040025A3 RID: 9635
		private string _referer;

		// Token: 0x040025A4 RID: 9636
		private string _path;

		// Token: 0x040025A5 RID: 9637
		private string _message;

		// Token: 0x040025A6 RID: 9638
		internal bool _macValidationError;
	}
}
