using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web.Services.Configuration;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class SoapException : SystemException
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000CFBA File Offset: 0x0000BFBA
		public static bool IsServerFaultCode(XmlQualifiedName code)
		{
			return code == SoapException.ServerFaultCode || code == Soap12FaultCodes.ReceiverFaultCode;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000CFD6 File Offset: 0x0000BFD6
		public static bool IsClientFaultCode(XmlQualifiedName code)
		{
			return code == SoapException.ClientFaultCode || code == Soap12FaultCodes.SenderFaultCode;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000CFF2 File Offset: 0x0000BFF2
		public static bool IsVersionMismatchFaultCode(XmlQualifiedName code)
		{
			return code == SoapException.VersionMismatchFaultCode || code == Soap12FaultCodes.VersionMismatchFaultCode;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000D00E File Offset: 0x0000C00E
		public static bool IsMustUnderstandFaultCode(XmlQualifiedName code)
		{
			return code == SoapException.MustUnderstandFaultCode || code == Soap12FaultCodes.MustUnderstandFaultCode;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D02A File Offset: 0x0000C02A
		public SoapException()
			: base(null)
		{
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D03E File Offset: 0x0000C03E
		public SoapException(string message, XmlQualifiedName code, string actor)
			: base(message)
		{
			this.code = code;
			this.actor = actor;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D060 File Offset: 0x0000C060
		public SoapException(string message, XmlQualifiedName code, string actor, Exception innerException)
			: base(message, innerException)
		{
			this.code = code;
			this.actor = actor;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D084 File Offset: 0x0000C084
		public SoapException(string message, XmlQualifiedName code)
			: base(message)
		{
			this.code = code;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D09F File Offset: 0x0000C09F
		public SoapException(string message, XmlQualifiedName code, Exception innerException)
			: base(message, innerException)
		{
			this.code = code;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D0BB File Offset: 0x0000C0BB
		public SoapException(string message, XmlQualifiedName code, string actor, XmlNode detail)
			: base(message)
		{
			this.code = code;
			this.actor = actor;
			this.detail = detail;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D0E5 File Offset: 0x0000C0E5
		public SoapException(string message, XmlQualifiedName code, string actor, XmlNode detail, Exception innerException)
			: base(message, innerException)
		{
			this.code = code;
			this.actor = actor;
			this.detail = detail;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D111 File Offset: 0x0000C111
		public SoapException(string message, XmlQualifiedName code, SoapFaultSubCode subCode)
			: base(message)
		{
			this.code = code;
			this.subCode = subCode;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000D133 File Offset: 0x0000C133
		public SoapException(string message, XmlQualifiedName code, string actor, string role, XmlNode detail, SoapFaultSubCode subCode, Exception innerException)
			: base(message, innerException)
		{
			this.code = code;
			this.actor = actor;
			this.role = role;
			this.detail = detail;
			this.subCode = subCode;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000D170 File Offset: 0x0000C170
		public SoapException(string message, XmlQualifiedName code, string actor, string role, string lang, XmlNode detail, SoapFaultSubCode subCode, Exception innerException)
			: base(message, innerException)
		{
			this.code = code;
			this.actor = actor;
			this.role = role;
			this.detail = detail;
			this.lang = lang;
			this.subCode = subCode;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D1C0 File Offset: 0x0000C1C0
		protected SoapException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			IDictionary data = base.Data;
			this.code = (XmlQualifiedName)data["code"];
			this.actor = (string)data["actor"];
			this.role = (string)data["role"];
			this.subCode = (SoapFaultSubCode)data["subCode"];
			this.lang = (string)data["lang"];
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000D255 File Offset: 0x0000C255
		public string Actor
		{
			get
			{
				if (this.actor != null)
				{
					return this.actor;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000D26B File Offset: 0x0000C26B
		public XmlQualifiedName Code
		{
			get
			{
				return this.code;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000D273 File Offset: 0x0000C273
		public XmlNode Detail
		{
			get
			{
				return this.detail;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000D27B File Offset: 0x0000C27B
		[ComVisible(false)]
		public string Lang
		{
			get
			{
				if (this.lang != null)
				{
					return this.lang;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000D291 File Offset: 0x0000C291
		[ComVisible(false)]
		public string Node
		{
			get
			{
				if (this.actor != null)
				{
					return this.actor;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000D2A7 File Offset: 0x0000C2A7
		[ComVisible(false)]
		public string Role
		{
			get
			{
				if (this.role != null)
				{
					return this.role;
				}
				return string.Empty;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000D2BD File Offset: 0x0000C2BD
		[ComVisible(false)]
		public SoapFaultSubCode SubCode
		{
			get
			{
				return this.subCode;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D2C5 File Offset: 0x0000C2C5
		internal void ClearSubCode()
		{
			if (this.subCode != null)
			{
				this.subCode = this.subCode.SubCode;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D2E0 File Offset: 0x0000C2E0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			IDictionary data = this.Data;
			data["code"] = this.Code;
			data["actor"] = this.Actor;
			data["role"] = this.Role;
			data["subCode"] = this.SubCode;
			data["lang"] = this.Lang;
			base.GetObjectData(info, context);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D351 File Offset: 0x0000C351
		private static SoapException CreateSuppressedException(SoapProtocolVersion soapVersion, string message, Exception innerException)
		{
			return new SoapException(Res.GetString("WebSuppressedExceptionMessage"), (soapVersion == SoapProtocolVersion.Soap12) ? new XmlQualifiedName("Receiver", "http://www.w3.org/2003/05/soap-envelope") : new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000D386 File Offset: 0x0000C386
		internal static SoapException Create(SoapProtocolVersion soapVersion, string message, XmlQualifiedName code, string actor, string role, XmlNode detail, SoapFaultSubCode subCode, Exception innerException)
		{
			if (WebServicesSection.Current.Diagnostics.SuppressReturningExceptions)
			{
				return SoapException.CreateSuppressedException(soapVersion, Res.GetString("WebSuppressedExceptionMessage"), innerException);
			}
			return new SoapException(message, code, actor, role, detail, subCode, innerException);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D3BC File Offset: 0x0000C3BC
		internal static SoapException Create(SoapProtocolVersion soapVersion, string message, XmlQualifiedName code, Exception innerException)
		{
			if (WebServicesSection.Current.Diagnostics.SuppressReturningExceptions)
			{
				return SoapException.CreateSuppressedException(soapVersion, Res.GetString("WebSuppressedExceptionMessage"), innerException);
			}
			return new SoapException(message, code, innerException);
		}

		// Token: 0x04000307 RID: 775
		private XmlQualifiedName code = XmlQualifiedName.Empty;

		// Token: 0x04000308 RID: 776
		private string actor;

		// Token: 0x04000309 RID: 777
		private string role;

		// Token: 0x0400030A RID: 778
		private XmlNode detail;

		// Token: 0x0400030B RID: 779
		private SoapFaultSubCode subCode;

		// Token: 0x0400030C RID: 780
		private string lang;

		// Token: 0x0400030D RID: 781
		public static readonly XmlQualifiedName ServerFaultCode = new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/");

		// Token: 0x0400030E RID: 782
		public static readonly XmlQualifiedName ClientFaultCode = new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/");

		// Token: 0x0400030F RID: 783
		public static readonly XmlQualifiedName VersionMismatchFaultCode = new XmlQualifiedName("VersionMismatch", "http://schemas.xmlsoap.org/soap/envelope/");

		// Token: 0x04000310 RID: 784
		public static readonly XmlQualifiedName MustUnderstandFaultCode = new XmlQualifiedName("MustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/");

		// Token: 0x04000311 RID: 785
		public static readonly XmlQualifiedName DetailElementName = new XmlQualifiedName("detail", "");
	}
}
