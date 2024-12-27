using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x0200068A RID: 1674
	internal class HeaderCollection : NameValueCollection
	{
		// Token: 0x060033D6 RID: 13270 RVA: 0x000DAF18 File Offset: 0x000D9F18
		internal HeaderCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000DAF28 File Offset: 0x000D9F28
		public override void Remove(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Remove", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType = null;
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition = null;
			}
			base.Remove(name);
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000DAFD8 File Offset: 0x000D9FD8
		public override string Get(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.Get(name);
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000DB094 File Offset: 0x000DA094
		public override string[] GetValues(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.GetValues(name);
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000DB14F File Offset: 0x000DA14F
		internal void InternalRemove(string name)
		{
			base.Remove(name);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000DB158 File Offset: 0x000DA158
		internal void InternalSet(string name, string value)
		{
			base.Set(name, value);
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000DB164 File Offset: 0x000DA164
		public override void Set(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Set", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "name");
			}
			if (!MimeBasePart.IsAscii(name, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderName"));
			}
			if (!MimeBasePart.IsAnsi(value, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderValue"));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			base.Set(name, value);
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000DB2C4 File Offset: 0x000DA2C4
		public override void Add(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Add", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "name");
			}
			MailBnfHelper.ValidateHeaderName(name);
			if (!MimeBasePart.IsAnsi(value, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderValue"));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (MailHeaderInfo.IsSingleton(name))
			{
				base.Set(name, value);
				return;
			}
			base.Add(name, value);
		}

		// Token: 0x04002FC0 RID: 12224
		private MimeBasePart part;
	}
}
