using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000686 RID: 1670
	public class ContentDisposition
	{
		// Token: 0x060033AB RID: 13227 RVA: 0x000DA2D8 File Offset: 0x000D92D8
		public ContentDisposition()
		{
			this.isChanged = true;
			this.disposition = "attachment";
			this.ParseValue();
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000DA2F8 File Offset: 0x000D92F8
		public ContentDisposition(string disposition)
		{
			if (disposition == null)
			{
				throw new ArgumentNullException("disposition");
			}
			this.isChanged = true;
			this.disposition = disposition;
			this.ParseValue();
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060033AD RID: 13229 RVA: 0x000DA322 File Offset: 0x000D9322
		// (set) Token: 0x060033AE RID: 13230 RVA: 0x000DA32A File Offset: 0x000D932A
		public string DispositionType
		{
			get
			{
				return this.dispositionType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
				}
				this.isChanged = true;
				this.dispositionType = value;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x060033AF RID: 13231 RVA: 0x000DA36A File Offset: 0x000D936A
		public StringDictionary Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new TrackingStringDictionary();
				}
				return this.parameters;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x000DA385 File Offset: 0x000D9385
		// (set) Token: 0x060033B1 RID: 13233 RVA: 0x000DA397 File Offset: 0x000D9397
		public string FileName
		{
			get
			{
				return this.Parameters["filename"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("filename");
					return;
				}
				this.Parameters["filename"] = value;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x000DA3CC File Offset: 0x000D93CC
		// (set) Token: 0x060033B3 RID: 13235 RVA: 0x000DA3FD File Offset: 0x000D93FD
		public DateTime CreationDate
		{
			get
			{
				string text = this.Parameters["creation-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["creation-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000DA418 File Offset: 0x000D9418
		// (set) Token: 0x060033B5 RID: 13237 RVA: 0x000DA449 File Offset: 0x000D9449
		public DateTime ModificationDate
		{
			get
			{
				string text = this.Parameters["modification-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["modification-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060033B6 RID: 13238 RVA: 0x000DA462 File Offset: 0x000D9462
		// (set) Token: 0x060033B7 RID: 13239 RVA: 0x000DA474 File Offset: 0x000D9474
		public bool Inline
		{
			get
			{
				return this.dispositionType == "inline";
			}
			set
			{
				this.isChanged = true;
				if (value)
				{
					this.dispositionType = "inline";
					return;
				}
				this.dispositionType = "attachment";
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000DA498 File Offset: 0x000D9498
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x000DA4C9 File Offset: 0x000D94C9
		public DateTime ReadDate
		{
			get
			{
				string text = this.Parameters["read-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["read-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000DA4E4 File Offset: 0x000D94E4
		// (set) Token: 0x060033BB RID: 13243 RVA: 0x000DA513 File Offset: 0x000D9513
		public long Size
		{
			get
			{
				string text = this.Parameters["size"];
				if (text == null)
				{
					return -1L;
				}
				return long.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				this.Parameters["size"] = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000DA531 File Offset: 0x000D9531
		internal void Set(string contentDisposition, HeaderCollection headers)
		{
			this.disposition = contentDisposition;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
			this.isPersisted = true;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000DA559 File Offset: 0x000D9559
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this.isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
				this.isPersisted = true;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x000DA587 File Offset: 0x000D9587
		internal bool IsChanged
		{
			get
			{
				return this.isChanged || (this.parameters != null && this.parameters.IsChanged);
			}
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000DA5A8 File Offset: 0x000D95A8
		public override string ToString()
		{
			if (this.disposition == null || this.isChanged || (this.parameters != null && this.parameters.IsChanged))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.dispositionType);
				foreach (object obj in this.Parameters.Keys)
				{
					string text = (string)obj;
					stringBuilder.Append("; ");
					stringBuilder.Append(text);
					stringBuilder.Append('=');
					MailBnfHelper.GetTokenOrQuotedString(this.parameters[text], stringBuilder);
				}
				this.disposition = stringBuilder.ToString();
				this.isChanged = false;
				this.parameters.IsChanged = false;
				this.isPersisted = false;
			}
			return this.disposition;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000DA69C File Offset: 0x000D969C
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000DA6B8 File Offset: 0x000D96B8
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000DA6C8 File Offset: 0x000D96C8
		private void ParseValue()
		{
			int num = 0;
			this.parameters = new TrackingStringDictionary();
			Exception ex = null;
			try
			{
				this.dispositionType = MailBnfHelper.ReadToken(this.disposition, ref num, null);
				if (this.dispositionType == null || this.dispositionType.Length == 0)
				{
					ex = new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (ex == null)
				{
					while (MailBnfHelper.SkipCFWS(this.disposition, ref num))
					{
						if (this.disposition[num++] != ';')
						{
							ex = new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
						}
						if (!MailBnfHelper.SkipCFWS(this.disposition, ref num))
						{
							break;
						}
						string text = MailBnfHelper.ReadParameterAttribute(this.disposition, ref num, null);
						if (this.disposition[num++] != '=')
						{
							ex = new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
							break;
						}
						string text2;
						if (!MailBnfHelper.SkipCFWS(this.disposition, ref num))
						{
							text2 = string.Empty;
						}
						else if (this.disposition[num] == '"')
						{
							text2 = MailBnfHelper.ReadQuotedString(this.disposition, ref num, null);
						}
						else
						{
							text2 = MailBnfHelper.ReadToken(this.disposition, ref num, null);
						}
						if (text == null || text2 == null || text.Length == 0 || text2.Length == 0)
						{
							ex = new FormatException(SR.GetString("ContentDispositionInvalid"));
							break;
						}
						if (string.Compare(text, "creation-date", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "modification-date", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "read-date", StringComparison.OrdinalIgnoreCase) == 0)
						{
							int num2 = 0;
							MailBnfHelper.ReadDateTime(text2, ref num2);
						}
						this.parameters.Add(text, text2);
					}
				}
			}
			catch (FormatException)
			{
				throw new FormatException(SR.GetString("ContentDispositionInvalid"));
			}
			if (ex != null)
			{
				throw ex;
			}
			this.parameters.IsChanged = false;
		}

		// Token: 0x04002FAA RID: 12202
		private string dispositionType;

		// Token: 0x04002FAB RID: 12203
		private TrackingStringDictionary parameters;

		// Token: 0x04002FAC RID: 12204
		private bool isChanged;

		// Token: 0x04002FAD RID: 12205
		private bool isPersisted;

		// Token: 0x04002FAE RID: 12206
		private string disposition;
	}
}
