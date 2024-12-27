using System;

namespace System.Xml.Schema
{
	// Token: 0x02000219 RID: 537
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x060019B7 RID: 6583 RVA: 0x0007BBD9 File Offset: 0x0007ABD9
		internal ValidationEventArgs(XmlSchemaException ex)
		{
			this.ex = ex;
			this.severity = XmlSeverityType.Error;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0007BBEF File Offset: 0x0007ABEF
		internal ValidationEventArgs(XmlSchemaException ex, XmlSeverityType severity)
		{
			this.ex = ex;
			this.severity = severity;
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0007BC05 File Offset: 0x0007AC05
		public XmlSeverityType Severity
		{
			get
			{
				return this.severity;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x0007BC0D File Offset: 0x0007AC0D
		public XmlSchemaException Exception
		{
			get
			{
				return this.ex;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0007BC15 File Offset: 0x0007AC15
		public string Message
		{
			get
			{
				return this.ex.Message;
			}
		}

		// Token: 0x0400100E RID: 4110
		private XmlSchemaException ex;

		// Token: 0x0400100F RID: 4111
		private XmlSeverityType severity;
	}
}
