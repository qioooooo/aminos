using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000045 RID: 69
	public class DsmlErrorResponse : DirectoryResponse
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00006C80 File Offset: 0x00005C80
		internal DsmlErrorResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006C90 File Offset: 0x00005C90
		public string Message
		{
			get
			{
				if (this.message == null)
				{
					XmlElement xmlElement = (XmlElement)this.dsmlNode.SelectSingleNode("dsml:message", this.dsmlNS);
					if (xmlElement != null)
					{
						this.message = xmlElement.InnerText;
					}
				}
				return this.message;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006CD8 File Offset: 0x00005CD8
		public string Detail
		{
			get
			{
				if (this.detail == null)
				{
					XmlElement xmlElement = (XmlElement)this.dsmlNode.SelectSingleNode("dsml:detail", this.dsmlNS);
					if (xmlElement != null)
					{
						this.detail = xmlElement.InnerXml;
					}
				}
				return this.detail;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006D20 File Offset: 0x00005D20
		public ErrorResponseCategory Type
		{
			get
			{
				if (this.category == (ErrorResponseCategory)(-1))
				{
					XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode("@dsml:type", this.dsmlNS);
					if (xmlAttribute == null)
					{
						xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode("@type", this.dsmlNS);
					}
					if (xmlAttribute == null)
					{
						throw new DsmlInvalidDocumentException(Res.GetString("MissingErrorResponseType"));
					}
					string value;
					switch (value = xmlAttribute.Value)
					{
					case "notAttempted":
						this.category = ErrorResponseCategory.NotAttempted;
						goto IL_017F;
					case "couldNotConnect":
						this.category = ErrorResponseCategory.CouldNotConnect;
						goto IL_017F;
					case "connectionClosed":
						this.category = ErrorResponseCategory.ConnectionClosed;
						goto IL_017F;
					case "malformedRequest":
						this.category = ErrorResponseCategory.MalformedRequest;
						goto IL_017F;
					case "gatewayInternalError":
						this.category = ErrorResponseCategory.GatewayInternalError;
						goto IL_017F;
					case "authenticationFailed":
						this.category = ErrorResponseCategory.AuthenticationFailed;
						goto IL_017F;
					case "unresolvableURI":
						this.category = ErrorResponseCategory.UnresolvableUri;
						goto IL_017F;
					case "other":
						this.category = ErrorResponseCategory.Other;
						goto IL_017F;
					}
					throw new DsmlInvalidDocumentException(Res.GetString("ErrorResponseInvalidValue", new object[] { xmlAttribute.Value }));
				}
				IL_017F:
				return this.category;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006EB2 File Offset: 0x00005EB2
		public override string MatchedDN
		{
			get
			{
				throw new NotSupportedException(Res.GetString("NotSupportOnDsmlErrRes"));
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006EC3 File Offset: 0x00005EC3
		public override DirectoryControl[] Controls
		{
			get
			{
				throw new NotSupportedException(Res.GetString("NotSupportOnDsmlErrRes"));
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006ED4 File Offset: 0x00005ED4
		public override ResultCode ResultCode
		{
			get
			{
				throw new NotSupportedException(Res.GetString("NotSupportOnDsmlErrRes"));
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00006EE5 File Offset: 0x00005EE5
		public override string ErrorMessage
		{
			get
			{
				throw new NotSupportedException(Res.GetString("NotSupportOnDsmlErrRes"));
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006EF6 File Offset: 0x00005EF6
		public override Uri[] Referral
		{
			get
			{
				throw new NotSupportedException(Res.GetString("NotSupportOnDsmlErrRes"));
			}
		}

		// Token: 0x04000128 RID: 296
		private string message;

		// Token: 0x04000129 RID: 297
		private string detail;

		// Token: 0x0400012A RID: 298
		private ErrorResponseCategory category = (ErrorResponseCategory)(-1);
	}
}
