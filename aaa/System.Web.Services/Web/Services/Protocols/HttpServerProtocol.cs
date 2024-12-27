using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Services.Configuration;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000041 RID: 65
	internal abstract class HttpServerProtocol : ServerProtocol
	{
		// Token: 0x06000165 RID: 357 RVA: 0x0000606D File Offset: 0x0000506D
		protected HttpServerProtocol(bool hasInputPayload)
		{
			this.hasInputPayload = hasInputPayload;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000607C File Offset: 0x0000507C
		internal override bool Initialize()
		{
			string text = base.Request.PathInfo.Substring(1);
			this.serverType = (HttpServerType)base.GetFromCache(typeof(HttpServerProtocol), base.Type);
			if (this.serverType == null)
			{
				lock (ServerProtocol.InternalSyncObject)
				{
					this.serverType = (HttpServerType)base.GetFromCache(typeof(HttpServerProtocol), base.Type);
					if (this.serverType == null)
					{
						this.serverType = new HttpServerType(base.Type);
						base.AddToCache(typeof(HttpServerProtocol), base.Type, this.serverType);
					}
				}
			}
			this.serverMethod = this.serverType.GetMethod(text);
			if (this.serverMethod == null)
			{
				this.serverMethod = this.serverType.GetMethodIgnoreCase(text);
				if (this.serverMethod != null)
				{
					throw new ArgumentException(Res.GetString("WebInvalidMethodNameCase", new object[]
					{
						text,
						this.serverMethod.name
					}), "methodName");
				}
				string @string = Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
				this.serverMethod = this.serverType.GetMethod(@string);
				if (this.serverMethod == null)
				{
					throw new InvalidOperationException(Res.GetString("WebInvalidMethodName", new object[] { text }));
				}
			}
			return true;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000061F8 File Offset: 0x000051F8
		internal override bool IsOneWay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000168 RID: 360 RVA: 0x000061FB File Offset: 0x000051FB
		internal override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.serverMethod.methodInfo;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006208 File Offset: 0x00005208
		internal override ServerType ServerType
		{
			get
			{
				return this.serverType;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006210 File Offset: 0x00005210
		internal override object[] ReadParameters()
		{
			if (this.serverMethod.readerTypes == null)
			{
				return new object[0];
			}
			int i = 0;
			while (i < this.serverMethod.readerTypes.Length)
			{
				if (!this.hasInputPayload)
				{
					if (this.serverMethod.readerTypes[i] == typeof(UrlParameterReader))
					{
						goto IL_0054;
					}
				}
				else if (this.serverMethod.readerTypes[i] != typeof(UrlParameterReader))
				{
					goto IL_0054;
				}
				IL_008B:
				i++;
				continue;
				IL_0054:
				MimeParameterReader mimeParameterReader = (MimeParameterReader)MimeFormatter.CreateInstance(this.serverMethod.readerTypes[i], this.serverMethod.readerInitializers[i]);
				object[] array = mimeParameterReader.Read(base.Request);
				if (array != null)
				{
					return array;
				}
				goto IL_008B;
			}
			if (!this.hasInputPayload)
			{
				throw new InvalidOperationException(Res.GetString("WebInvalidRequestFormat"));
			}
			throw new InvalidOperationException(Res.GetString("WebInvalidRequestFormatDetails", new object[] { base.Request.ContentType }));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000062FC File Offset: 0x000052FC
		internal override void WriteReturns(object[] returnValues, Stream outputStream)
		{
			if (this.serverMethod.writerType == null)
			{
				return;
			}
			MimeReturnWriter mimeReturnWriter = (MimeReturnWriter)MimeFormatter.CreateInstance(this.serverMethod.writerType, this.serverMethod.writerInitializer);
			mimeReturnWriter.Write(base.Response, outputStream, returnValues[0]);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006348 File Offset: 0x00005348
		internal override bool WriteException(Exception e, Stream outputStream)
		{
			base.Response.Clear();
			base.Response.ClearHeaders();
			base.Response.ContentType = ContentType.Compose("text/plain", Encoding.UTF8);
			ServerProtocol.SetHttpResponseStatusCode(base.Response, 500);
			base.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription(base.Response.StatusCode);
			StreamWriter streamWriter = new StreamWriter(outputStream, new UTF8Encoding(false));
			if (WebServicesSection.Current.Diagnostics.SuppressReturningExceptions)
			{
				streamWriter.WriteLine(Res.GetString("WebSuppressedExceptionMessage"));
			}
			else
			{
				streamWriter.WriteLine(base.GenerateFaultString(e, true));
			}
			streamWriter.Flush();
			return true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000063F8 File Offset: 0x000053F8
		internal static bool AreUrlParametersSupported(LogicalMethodInfo methodInfo)
		{
			if (methodInfo.OutParameters.Length > 0)
			{
				return false;
			}
			foreach (ParameterInfo parameterInfo in methodInfo.InParameters)
			{
				Type parameterType = parameterInfo.ParameterType;
				if (parameterType.IsArray)
				{
					if (!ScalarFormatter.IsTypeSupported(parameterType.GetElementType()))
					{
						return false;
					}
				}
				else if (!ScalarFormatter.IsTypeSupported(parameterType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400028A RID: 650
		private HttpServerMethod serverMethod;

		// Token: 0x0400028B RID: 651
		private HttpServerType serverType;

		// Token: 0x0400028C RID: 652
		private bool hasInputPayload;
	}
}
