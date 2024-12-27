using System;
using System.Collections;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Mail
{
	// Token: 0x0200078E RID: 1934
	[Obsolete("The recommended alternative is System.Net.Mail.MailMessage. http://go.microsoft.com/fwlink/?linkid=14202")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MailMessage
	{
		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x06005CF6 RID: 23798 RVA: 0x00174C78 File Offset: 0x00173C78
		// (set) Token: 0x06005CF7 RID: 23799 RVA: 0x00174C80 File Offset: 0x00173C80
		public string From
		{
			get
			{
				return this.from;
			}
			set
			{
				this.from = value;
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x06005CF8 RID: 23800 RVA: 0x00174C89 File Offset: 0x00173C89
		// (set) Token: 0x06005CF9 RID: 23801 RVA: 0x00174C91 File Offset: 0x00173C91
		public string To
		{
			get
			{
				return this.to;
			}
			set
			{
				this.to = value;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x00174C9A File Offset: 0x00173C9A
		// (set) Token: 0x06005CFB RID: 23803 RVA: 0x00174CA2 File Offset: 0x00173CA2
		public string Cc
		{
			get
			{
				return this.cc;
			}
			set
			{
				this.cc = value;
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x06005CFC RID: 23804 RVA: 0x00174CAB File Offset: 0x00173CAB
		// (set) Token: 0x06005CFD RID: 23805 RVA: 0x00174CB3 File Offset: 0x00173CB3
		public string Bcc
		{
			get
			{
				return this.bcc;
			}
			set
			{
				this.bcc = value;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x06005CFE RID: 23806 RVA: 0x00174CBC File Offset: 0x00173CBC
		// (set) Token: 0x06005CFF RID: 23807 RVA: 0x00174CC4 File Offset: 0x00173CC4
		public string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06005D00 RID: 23808 RVA: 0x00174CCD File Offset: 0x00173CCD
		// (set) Token: 0x06005D01 RID: 23809 RVA: 0x00174CD5 File Offset: 0x00173CD5
		public MailPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x00174CDE File Offset: 0x00173CDE
		// (set) Token: 0x06005D03 RID: 23811 RVA: 0x00174CE6 File Offset: 0x00173CE6
		public string UrlContentBase
		{
			get
			{
				return this.urlContentBase;
			}
			set
			{
				this.urlContentBase = value;
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x00174CEF File Offset: 0x00173CEF
		// (set) Token: 0x06005D05 RID: 23813 RVA: 0x00174CF7 File Offset: 0x00173CF7
		public string UrlContentLocation
		{
			get
			{
				return this.urlContentLocation;
			}
			set
			{
				this.urlContentLocation = value;
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x00174D00 File Offset: 0x00173D00
		// (set) Token: 0x06005D07 RID: 23815 RVA: 0x00174D08 File Offset: 0x00173D08
		public string Body
		{
			get
			{
				return this.body;
			}
			set
			{
				this.body = value;
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x06005D08 RID: 23816 RVA: 0x00174D11 File Offset: 0x00173D11
		// (set) Token: 0x06005D09 RID: 23817 RVA: 0x00174D19 File Offset: 0x00173D19
		public MailFormat BodyFormat
		{
			get
			{
				return this.bodyFormat;
			}
			set
			{
				this.bodyFormat = value;
			}
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x06005D0A RID: 23818 RVA: 0x00174D22 File Offset: 0x00173D22
		// (set) Token: 0x06005D0B RID: 23819 RVA: 0x00174D2A File Offset: 0x00173D2A
		public Encoding BodyEncoding
		{
			get
			{
				return this.bodyEncoding;
			}
			set
			{
				this.bodyEncoding = value;
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06005D0C RID: 23820 RVA: 0x00174D33 File Offset: 0x00173D33
		public IDictionary Headers
		{
			get
			{
				return this._headers;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06005D0D RID: 23821 RVA: 0x00174D3B File Offset: 0x00173D3B
		public IDictionary Fields
		{
			get
			{
				return this._fields;
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06005D0E RID: 23822 RVA: 0x00174D43 File Offset: 0x00173D43
		public IList Attachments
		{
			get
			{
				return this._attachments;
			}
		}

		// Token: 0x040031AC RID: 12716
		private Hashtable _headers = new Hashtable();

		// Token: 0x040031AD RID: 12717
		private Hashtable _fields = new Hashtable();

		// Token: 0x040031AE RID: 12718
		private ArrayList _attachments = new ArrayList();

		// Token: 0x040031AF RID: 12719
		private string from;

		// Token: 0x040031B0 RID: 12720
		private string to;

		// Token: 0x040031B1 RID: 12721
		private string cc;

		// Token: 0x040031B2 RID: 12722
		private string bcc;

		// Token: 0x040031B3 RID: 12723
		private string subject;

		// Token: 0x040031B4 RID: 12724
		private MailPriority priority;

		// Token: 0x040031B5 RID: 12725
		private string urlContentBase;

		// Token: 0x040031B6 RID: 12726
		private string urlContentLocation;

		// Token: 0x040031B7 RID: 12727
		private string body;

		// Token: 0x040031B8 RID: 12728
		private MailFormat bodyFormat;

		// Token: 0x040031B9 RID: 12729
		private Encoding bodyEncoding = Encoding.Default;
	}
}
