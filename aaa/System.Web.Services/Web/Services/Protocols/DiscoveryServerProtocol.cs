using System;
using System.IO;
using System.Text;
using System.Web.Services.Description;
using System.Xml.Schema;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000031 RID: 49
	internal sealed class DiscoveryServerProtocol : ServerProtocol
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00004BB4 File Offset: 0x00003BB4
		internal override bool Initialize()
		{
			this.serverType = (DiscoveryServerType)base.GetFromCache(typeof(DiscoveryServerProtocol), base.Type);
			if (this.serverType == null)
			{
				lock (ServerProtocol.InternalSyncObject)
				{
					this.serverType = (DiscoveryServerType)base.GetFromCache(typeof(DiscoveryServerProtocol), base.Type);
					if (this.serverType == null)
					{
						string text = base.Request.Url.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped).Replace("#", "%23");
						this.serverType = new DiscoveryServerType(base.Type, text);
						base.AddToCache(typeof(DiscoveryServerProtocol), base.Type, this.serverType);
					}
				}
			}
			return true;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00004C90 File Offset: 0x00003C90
		internal override ServerType ServerType
		{
			get
			{
				return this.serverType;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00004C98 File Offset: 0x00003C98
		internal override bool IsOneWay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00004C9B File Offset: 0x00003C9B
		internal override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.serverType.MethodInfo;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004CA8 File Offset: 0x00003CA8
		internal override object[] ReadParameters()
		{
			return new object[0];
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004CB0 File Offset: 0x00003CB0
		internal override void WriteReturns(object[] returnValues, Stream outputStream)
		{
			string text = base.Request.QueryString["schema"];
			Encoding encoding = new UTF8Encoding(false);
			if (text != null)
			{
				XmlSchema schema = this.serverType.GetSchema(text);
				if (schema == null)
				{
					throw new InvalidOperationException(Res.GetString("WebSchemaNotFound"));
				}
				base.Response.ContentType = ContentType.Compose("text/xml", encoding);
				schema.Write(new StreamWriter(outputStream, encoding));
				return;
			}
			else
			{
				text = base.Request.QueryString["wsdl"];
				if (text != null)
				{
					ServiceDescription serviceDescription = this.serverType.GetServiceDescription(text);
					if (serviceDescription == null)
					{
						throw new InvalidOperationException(Res.GetString("ServiceDescriptionWasNotFound0"));
					}
					base.Response.ContentType = ContentType.Compose("text/xml", encoding);
					serviceDescription.Write(new StreamWriter(outputStream, encoding));
					return;
				}
				else
				{
					string text2 = base.Request.QueryString[null];
					if (text2 != null && string.Compare(text2, "wsdl", StringComparison.OrdinalIgnoreCase) == 0)
					{
						base.Response.ContentType = ContentType.Compose("text/xml", encoding);
						this.serverType.Description.Write(new StreamWriter(outputStream, encoding));
						return;
					}
					if (text2 != null && string.Compare(text2, "disco", StringComparison.OrdinalIgnoreCase) == 0)
					{
						base.Response.ContentType = ContentType.Compose("text/xml", encoding);
						this.serverType.Disco.Write(new StreamWriter(outputStream, encoding));
						return;
					}
					throw new InvalidOperationException(Res.GetString("internalError0"));
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004E24 File Offset: 0x00003E24
		internal override bool WriteException(Exception e, Stream outputStream)
		{
			base.Response.Clear();
			base.Response.ClearHeaders();
			base.Response.ContentType = ContentType.Compose("text/plain", Encoding.UTF8);
			base.Response.StatusCode = 500;
			base.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription(base.Response.StatusCode);
			StreamWriter streamWriter = new StreamWriter(outputStream, new UTF8Encoding(false));
			streamWriter.WriteLine(base.GenerateFaultString(e, true));
			streamWriter.Flush();
			return true;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004EAE File Offset: 0x00003EAE
		internal void Discover()
		{
		}

		// Token: 0x04000272 RID: 626
		private DiscoveryServerType serverType;
	}
}
