using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000063 RID: 99
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpClientCertificate : NameValueCollection
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00011322 File Offset: 0x00010322
		public string Cookie
		{
			get
			{
				return this._Cookie;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001132A File Offset: 0x0001032A
		public byte[] Certificate
		{
			get
			{
				return this._Certificate;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00011332 File Offset: 0x00010332
		public int Flags
		{
			get
			{
				return this._Flags;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001133A File Offset: 0x0001033A
		public int KeySize
		{
			get
			{
				return this._KeySize;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00011342 File Offset: 0x00010342
		public int SecretKeySize
		{
			get
			{
				return this._SecretKeySize;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0001134A File Offset: 0x0001034A
		public string Issuer
		{
			get
			{
				return this._Issuer;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00011352 File Offset: 0x00010352
		public string ServerIssuer
		{
			get
			{
				return this._ServerIssuer;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0001135A File Offset: 0x0001035A
		public string Subject
		{
			get
			{
				return this._Subject;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011362 File Offset: 0x00010362
		public string ServerSubject
		{
			get
			{
				return this._ServerSubject;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0001136A File Offset: 0x0001036A
		public string SerialNumber
		{
			get
			{
				return this._SerialNumber;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00011372 File Offset: 0x00010372
		public DateTime ValidFrom
		{
			get
			{
				return this._ValidFrom;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0001137A File Offset: 0x0001037A
		public DateTime ValidUntil
		{
			get
			{
				return this._ValidUntil;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00011382 File Offset: 0x00010382
		public int CertEncoding
		{
			get
			{
				return this._CertEncoding;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001138A File Offset: 0x0001038A
		public byte[] PublicKey
		{
			get
			{
				return this._PublicKey;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00011392 File Offset: 0x00010392
		public byte[] BinaryIssuer
		{
			get
			{
				return this._BinaryIssuer;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001139A File Offset: 0x0001039A
		public bool IsPresent
		{
			get
			{
				return (this._Flags & 1) == 1;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000113A7 File Offset: 0x000103A7
		public bool IsValid
		{
			get
			{
				return (this._Flags & 2) == 0;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000113B4 File Offset: 0x000103B4
		internal HttpClientCertificate(HttpContext context)
		{
			string text = context.Request.ServerVariables["CERT_FLAGS"];
			if (!string.IsNullOrEmpty(text))
			{
				this._Flags = int.Parse(text, CultureInfo.InvariantCulture);
			}
			else
			{
				this._Flags = 0;
			}
			if (!this.IsPresent)
			{
				return;
			}
			this._Cookie = context.Request.ServerVariables["CERT_COOKIE"];
			this._Issuer = context.Request.ServerVariables["CERT_ISSUER"];
			this._ServerIssuer = context.Request.ServerVariables["CERT_SERVER_ISSUER"];
			this._Subject = context.Request.ServerVariables["CERT_SUBJECT"];
			this._ServerSubject = context.Request.ServerVariables["CERT_SERVER_SUBJECT"];
			this._SerialNumber = context.Request.ServerVariables["CERT_SERIALNUMBER"];
			this._Certificate = context.WorkerRequest.GetClientCertificate();
			this._ValidFrom = context.WorkerRequest.GetClientCertificateValidFrom();
			this._ValidUntil = context.WorkerRequest.GetClientCertificateValidUntil();
			this._BinaryIssuer = context.WorkerRequest.GetClientCertificateBinaryIssuer();
			this._PublicKey = context.WorkerRequest.GetClientCertificatePublicKey();
			this._CertEncoding = context.WorkerRequest.GetClientCertificateEncoding();
			string text2 = context.Request.ServerVariables["CERT_KEYSIZE"];
			string text3 = context.Request.ServerVariables["CERT_SECRETKEYSIZE"];
			if (!string.IsNullOrEmpty(text2))
			{
				this._KeySize = int.Parse(text2, CultureInfo.InvariantCulture);
			}
			if (!string.IsNullOrEmpty(text3))
			{
				this._SecretKeySize = int.Parse(text3, CultureInfo.InvariantCulture);
			}
			base.Add("ISSUER", null);
			base.Add("SUBJECTEMAIL", null);
			base.Add("BINARYISSUER", null);
			base.Add("FLAGS", null);
			base.Add("ISSUERO", null);
			base.Add("PUBLICKEY", null);
			base.Add("ISSUEROU", null);
			base.Add("ENCODING", null);
			base.Add("ISSUERCN", null);
			base.Add("SERIALNUMBER", null);
			base.Add("SUBJECT", null);
			base.Add("SUBJECTCN", null);
			base.Add("CERTIFICATE", null);
			base.Add("SUBJECTO", null);
			base.Add("SUBJECTOU", null);
			base.Add("VALIDUNTIL", null);
			base.Add("VALIDFROM", null);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000116B8 File Offset: 0x000106B8
		public override string Get(string field)
		{
			if (field == null)
			{
				return string.Empty;
			}
			field = field.ToLower(CultureInfo.InvariantCulture);
			string text;
			switch (text = field)
			{
			case "cookie":
				return this.Cookie;
			case "flags":
				return this.Flags.ToString("G", CultureInfo.InvariantCulture);
			case "keysize":
				return this.KeySize.ToString("G", CultureInfo.InvariantCulture);
			case "secretkeysize":
				return this.SecretKeySize.ToString(CultureInfo.InvariantCulture);
			case "issuer":
				return this.Issuer;
			case "serverissuer":
				return this.ServerIssuer;
			case "subject":
				return this.Subject;
			case "serversubject":
				return this.ServerSubject;
			case "serialnumber":
				return this.SerialNumber;
			case "certificate":
				return Encoding.Default.GetString(this.Certificate);
			case "binaryissuer":
				return Encoding.Default.GetString(this.BinaryIssuer);
			case "publickey":
				return Encoding.Default.GetString(this.PublicKey);
			case "encoding":
				return this.CertEncoding.ToString("G", CultureInfo.InvariantCulture);
			case "validfrom":
				return HttpUtility.FormatHttpDateTime(this.ValidFrom);
			case "validuntil":
				return HttpUtility.FormatHttpDateTime(this.ValidUntil);
			}
			if (StringUtil.StringStartsWith(field, "issuer"))
			{
				return this.ExtractString(this.Issuer, field.Substring(6));
			}
			if (StringUtil.StringStartsWith(field, "subject"))
			{
				if (field.Equals("subjectemail"))
				{
					return this.ExtractString(this.Subject, "e");
				}
				return this.ExtractString(this.Subject, field.Substring(7));
			}
			else
			{
				if (StringUtil.StringStartsWith(field, "serversubject"))
				{
					return this.ExtractString(this.ServerSubject, field.Substring(13));
				}
				if (StringUtil.StringStartsWith(field, "serverissuer"))
				{
					return this.ExtractString(this.ServerIssuer, field.Substring(12));
				}
				return string.Empty;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00011994 File Offset: 0x00010994
		private string ExtractString(string strAll, string strSubject)
		{
			if (strAll == null || strSubject == null)
			{
				return string.Empty;
			}
			string text = string.Empty;
			int i = 0;
			string text2 = strAll.ToLower(CultureInfo.InvariantCulture);
			while (i < text2.Length)
			{
				i = text2.IndexOf(strSubject + "=", i, StringComparison.Ordinal);
				if (i < 0)
				{
					return text;
				}
				if (text.Length > 0)
				{
					text += ";";
				}
				i += strSubject.Length + 1;
				int num;
				if (strAll[i] == '"')
				{
					i++;
					num = strAll.IndexOf('"', i);
				}
				else
				{
					num = strAll.IndexOf(',', i);
				}
				if (num < 0)
				{
					num = strAll.Length;
				}
				text += strAll.Substring(i, num - i);
				i = num + 1;
			}
			return text;
		}

		// Token: 0x04000FCE RID: 4046
		private string _Cookie = string.Empty;

		// Token: 0x04000FCF RID: 4047
		private byte[] _Certificate = new byte[0];

		// Token: 0x04000FD0 RID: 4048
		private int _Flags;

		// Token: 0x04000FD1 RID: 4049
		private int _KeySize;

		// Token: 0x04000FD2 RID: 4050
		private int _SecretKeySize;

		// Token: 0x04000FD3 RID: 4051
		private string _Issuer = string.Empty;

		// Token: 0x04000FD4 RID: 4052
		private string _ServerIssuer = string.Empty;

		// Token: 0x04000FD5 RID: 4053
		private string _Subject = string.Empty;

		// Token: 0x04000FD6 RID: 4054
		private string _ServerSubject = string.Empty;

		// Token: 0x04000FD7 RID: 4055
		private string _SerialNumber = string.Empty;

		// Token: 0x04000FD8 RID: 4056
		private DateTime _ValidFrom = DateTime.Now;

		// Token: 0x04000FD9 RID: 4057
		private DateTime _ValidUntil = DateTime.Now;

		// Token: 0x04000FDA RID: 4058
		private int _CertEncoding;

		// Token: 0x04000FDB RID: 4059
		private byte[] _PublicKey = new byte[0];

		// Token: 0x04000FDC RID: 4060
		private byte[] _BinaryIssuer = new byte[0];
	}
}
