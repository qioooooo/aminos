using System;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000699 RID: 1689
	public class MailAddress
	{
		// Token: 0x06003415 RID: 13333 RVA: 0x000DB969 File Offset: 0x000DA969
		internal MailAddress(string address, string encodedDisplayName, uint bogusParam)
		{
			this.encodedDisplayName = encodedDisplayName;
			this.GetParts(address);
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000DB97F File Offset: 0x000DA97F
		public MailAddress(string address)
			: this(address, null, null)
		{
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000DB98A File Offset: 0x000DA98A
		public MailAddress(string address, string displayName)
			: this(address, displayName, null)
		{
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000DB998 File Offset: 0x000DA998
		public MailAddress(string address, string displayName, Encoding displayNameEncoding)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "address" }), "address");
			}
			this.displayNameEncoding = displayNameEncoding;
			this.displayName = displayName;
			this.ParseValue(address);
			if (this.displayName != null && this.displayName != string.Empty)
			{
				if (this.displayName[0] == '"' && this.displayName[this.displayName.Length - 1] == '"')
				{
					this.displayName = this.displayName.Substring(1, this.displayName.Length - 2);
				}
				this.displayName = this.displayName.Trim();
			}
			if (this.displayName != null && this.displayName.Length > 0)
			{
				if (!MimeBasePart.IsAscii(this.displayName, false) || this.displayNameEncoding != null)
				{
					if (this.displayNameEncoding == null)
					{
						this.displayNameEncoding = Encoding.GetEncoding("utf-8");
					}
					this.encodedDisplayName = MimeBasePart.EncodeHeaderValue(this.displayName, this.displayNameEncoding, MimeBasePart.ShouldUseBase64Encoding(displayNameEncoding));
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					MailBnfHelper.ReadUnQuotedString(this.encodedDisplayName, ref num, stringBuilder);
					this.encodedDisplayName = stringBuilder.ToString();
					return;
				}
				this.encodedDisplayName = this.displayName;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x000DBB0C File Offset: 0x000DAB0C
		public string DisplayName
		{
			get
			{
				if (this.displayName == null)
				{
					if (this.encodedDisplayName != null && this.encodedDisplayName.Length > 0)
					{
						this.displayName = MimeBasePart.DecodeHeaderValue(this.encodedDisplayName);
					}
					else
					{
						this.displayName = string.Empty;
					}
				}
				return this.displayName;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x000DBB5B File Offset: 0x000DAB5B
		public string User
		{
			get
			{
				return this.userName;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x000DBB63 File Offset: 0x000DAB63
		public string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000DBB6B File Offset: 0x000DAB6B
		public string Address
		{
			get
			{
				if (this.address == null)
				{
					this.CombineParts();
				}
				return this.address;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x000DBB84 File Offset: 0x000DAB84
		internal string SmtpAddress
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('<');
				stringBuilder.Append(this.Address);
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000DBBBC File Offset: 0x000DABBC
		internal string ToEncodedString()
		{
			if (this.fullAddress == null)
			{
				if (this.encodedDisplayName != null && this.encodedDisplayName != string.Empty)
				{
					StringBuilder stringBuilder = new StringBuilder();
					MailBnfHelper.GetDotAtomOrQuotedString(this.encodedDisplayName, stringBuilder);
					stringBuilder.Append(" <");
					stringBuilder.Append(this.Address);
					stringBuilder.Append('>');
					this.fullAddress = stringBuilder.ToString();
				}
				else
				{
					this.fullAddress = this.Address;
				}
			}
			return this.fullAddress;
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000DBC40 File Offset: 0x000DAC40
		public override string ToString()
		{
			if (this.fullAddress == null)
			{
				if (this.encodedDisplayName != null && this.encodedDisplayName != string.Empty)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (this.DisplayName.StartsWith("\"") && this.DisplayName.EndsWith("\""))
					{
						stringBuilder.Append(this.DisplayName);
					}
					else
					{
						stringBuilder.Append('"');
						stringBuilder.Append(this.DisplayName);
						stringBuilder.Append('"');
					}
					stringBuilder.Append(" <");
					stringBuilder.Append(this.Address);
					stringBuilder.Append('>');
					this.fullAddress = stringBuilder.ToString();
				}
				else
				{
					this.fullAddress = this.Address;
				}
			}
			return this.fullAddress;
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x000DBD12 File Offset: 0x000DAD12
		public override bool Equals(object value)
		{
			return value != null && this.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000DBD2B File Offset: 0x000DAD2B
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000DBD38 File Offset: 0x000DAD38
		private void GetParts(string address)
		{
			if (address == null)
			{
				return;
			}
			int num = address.IndexOf('@');
			if (num < 0)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			this.userName = address.Substring(0, num);
			this.host = address.Substring(num + 1);
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000DBD84 File Offset: 0x000DAD84
		private void ParseValue(string address)
		{
			string text = null;
			int num = 0;
			MailBnfHelper.SkipFWS(address, ref num);
			int num2 = address.IndexOf('"', num);
			if (num2 == num)
			{
				num2 = address.IndexOf('"', num2 + 1);
				if (num2 > num)
				{
					int num3 = num2 + 1;
					MailBnfHelper.SkipFWS(address, ref num3);
					if (address.Length > num3 && address[num3] != '@')
					{
						text = address.Substring(num, num2 + 1 - num);
						address = address.Substring(num3);
					}
				}
			}
			if (text == null)
			{
				int num4 = address.IndexOf('<', num);
				if (num4 >= num)
				{
					text = address.Substring(num, num4 - num);
					address = address.Substring(num4);
				}
			}
			if (text == null)
			{
				num2 = address.IndexOf('"', num);
				if (num2 > num)
				{
					text = address.Substring(num, num2 - num);
					address = address.Substring(num2);
				}
			}
			if (this.displayName == null)
			{
				this.displayName = text;
			}
			int num5 = 0;
			address = MailBnfHelper.ReadMailAddress(address, ref num5, out this.encodedDisplayName);
			this.GetParts(address);
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000DBE6C File Offset: 0x000DAE6C
		private void CombineParts()
		{
			if (this.userName == null || this.host == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			MailBnfHelper.GetDotAtomOrQuotedString(this.User, stringBuilder);
			stringBuilder.Append('@');
			MailBnfHelper.GetDotAtomOrDomainLiteral(this.Host, stringBuilder);
			this.address = stringBuilder.ToString();
		}

		// Token: 0x04002FF6 RID: 12278
		private string displayName;

		// Token: 0x04002FF7 RID: 12279
		private Encoding displayNameEncoding;

		// Token: 0x04002FF8 RID: 12280
		private string encodedDisplayName;

		// Token: 0x04002FF9 RID: 12281
		private string address;

		// Token: 0x04002FFA RID: 12282
		private string fullAddress;

		// Token: 0x04002FFB RID: 12283
		private string userName;

		// Token: 0x04002FFC RID: 12284
		private string host;
	}
}
