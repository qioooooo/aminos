using System;
using System.IO;
using System.Security.Permissions;

namespace System.Web.Mail
{
	// Token: 0x0200078D RID: 1933
	[Obsolete("The recommended alternative is System.Net.Mail.Attachment. http://go.microsoft.com/fwlink/?linkid=14202")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MailAttachment
	{
		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06005CF1 RID: 23793 RVA: 0x00174BDC File Offset: 0x00173BDC
		public string Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06005CF2 RID: 23794 RVA: 0x00174BE4 File Offset: 0x00173BE4
		public MailEncoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x00174BEC File Offset: 0x00173BEC
		public MailAttachment(string filename)
		{
			this._filename = filename;
			this._encoding = MailEncoding.Base64;
			this.VerifyFile();
		}

		// Token: 0x06005CF4 RID: 23796 RVA: 0x00174C08 File Offset: 0x00173C08
		public MailAttachment(string filename, MailEncoding encoding)
		{
			this._filename = filename;
			this._encoding = encoding;
			this.VerifyFile();
		}

		// Token: 0x06005CF5 RID: 23797 RVA: 0x00174C24 File Offset: 0x00173C24
		private void VerifyFile()
		{
			try
			{
				File.Open(this._filename, FileMode.Open, FileAccess.Read, FileShare.Read).Close();
			}
			catch
			{
				throw new HttpException(SR.GetString("Bad_attachment", new object[] { this._filename }));
			}
		}

		// Token: 0x040031AA RID: 12714
		private string _filename;

		// Token: 0x040031AB RID: 12715
		private MailEncoding _encoding;
	}
}
