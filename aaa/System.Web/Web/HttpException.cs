using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200006B RID: 107
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class HttpException : ExternalException
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x00013C3C File Offset: 0x00012C3C
		internal static int HResultFromLastError(int lastError)
		{
			int num;
			if (lastError < 0)
			{
				num = lastError;
			}
			else
			{
				num = (lastError & 65535) | 458752 | int.MinValue;
			}
			return num;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00013C66 File Offset: 0x00012C66
		public static HttpException CreateFromLastError(string message)
		{
			return new HttpException(message, HttpException.HResultFromLastError(Marshal.GetLastWin32Error()));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00013C78 File Offset: 0x00012C78
		public HttpException()
		{
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00013C80 File Offset: 0x00012C80
		public HttpException(string message)
			: base(message)
		{
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00013C89 File Offset: 0x00012C89
		internal HttpException(string message, Exception innerException, int code)
			: base(message, innerException)
		{
			this._webEventCode = code;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00013C9A File Offset: 0x00012C9A
		public HttpException(string message, int hr)
			: base(message)
		{
			base.HResult = hr;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00013CAA File Offset: 0x00012CAA
		public HttpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00013CB4 File Offset: 0x00012CB4
		public HttpException(int httpCode, string message, Exception innerException)
			: base(message, innerException)
		{
			this._httpCode = httpCode;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00013CC5 File Offset: 0x00012CC5
		public HttpException(int httpCode, string message)
			: base(message)
		{
			this._httpCode = httpCode;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00013CD5 File Offset: 0x00012CD5
		public HttpException(int httpCode, string message, int hr)
			: base(message)
		{
			base.HResult = hr;
			this._httpCode = httpCode;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00013CEC File Offset: 0x00012CEC
		protected HttpException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._httpCode = info.GetInt32("_httpCode");
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00013D07 File Offset: 0x00012D07
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_httpCode", this._httpCode);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00013D22 File Offset: 0x00012D22
		public int GetHttpCode()
		{
			return HttpException.GetHttpCodeForException(this);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00013D2A File Offset: 0x00012D2A
		internal void SetFormatter(ErrorFormatter errorFormatter)
		{
			this._errorFormatter = errorFormatter;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00013D34 File Offset: 0x00012D34
		internal static int GetHttpCodeForException(Exception e)
		{
			if (e is HttpException)
			{
				HttpException ex = (HttpException)e;
				if (ex._httpCode > 0)
				{
					return ex._httpCode;
				}
			}
			else
			{
				if (e is UnauthorizedAccessException)
				{
					return 401;
				}
				if (e is PathTooLongException)
				{
					return 414;
				}
			}
			if (e.InnerException != null)
			{
				return HttpException.GetHttpCodeForException(e.InnerException);
			}
			return 500;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00013D98 File Offset: 0x00012D98
		internal static ErrorFormatter GetErrorFormatter(Exception e)
		{
			Exception innerException = e.InnerException;
			ErrorFormatter errorFormatter = null;
			if (innerException != null)
			{
				errorFormatter = HttpException.GetErrorFormatter(innerException);
				if (errorFormatter != null)
				{
					return errorFormatter;
				}
				if (innerException is ConfigurationException)
				{
					ConfigurationException ex = innerException as ConfigurationException;
					if (ex != null && ex.Filename != null)
					{
						errorFormatter = new ConfigErrorFormatter((ConfigurationException)innerException);
					}
				}
				else if (innerException is SecurityException)
				{
					errorFormatter = new SecurityErrorFormatter(innerException);
				}
			}
			if (errorFormatter != null)
			{
				return errorFormatter;
			}
			HttpException ex2 = e as HttpException;
			if (ex2 != null)
			{
				return ex2._errorFormatter;
			}
			return null;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00013E0C File Offset: 0x00012E0C
		public string GetHtmlErrorMessage()
		{
			ErrorFormatter errorFormatter = HttpException.GetErrorFormatter(this);
			if (errorFormatter == null)
			{
				return null;
			}
			return errorFormatter.GetHtmlErrorMessage();
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00013E2B File Offset: 0x00012E2B
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x00013E33 File Offset: 0x00012E33
		internal int WebEventCode
		{
			get
			{
				return this._webEventCode;
			}
			set
			{
				this._webEventCode = value;
			}
		}

		// Token: 0x04001035 RID: 4149
		private const int FACILITY_WIN32 = 7;

		// Token: 0x04001036 RID: 4150
		private int _httpCode;

		// Token: 0x04001037 RID: 4151
		private ErrorFormatter _errorFormatter;

		// Token: 0x04001038 RID: 4152
		private int _webEventCode;
	}
}
