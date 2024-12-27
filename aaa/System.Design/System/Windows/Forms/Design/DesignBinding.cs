using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001FA RID: 506
	[Editor("System.Windows.Forms.Design.DesignBindingEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	internal class DesignBinding
	{
		// Token: 0x06001350 RID: 4944 RVA: 0x0006291F File Offset: 0x0006191F
		public DesignBinding(object dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00062935 File Offset: 0x00061935
		public bool IsNull
		{
			get
			{
				return this.dataSource == null;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x00062940 File Offset: 0x00061940
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00062948 File Offset: 0x00061948
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00062950 File Offset: 0x00061950
		public string DataField
		{
			get
			{
				if (string.IsNullOrEmpty(this.dataMember))
				{
					return string.Empty;
				}
				int num = this.dataMember.LastIndexOf(".");
				if (num == -1)
				{
					return this.dataMember;
				}
				return this.dataMember.Substring(num + 1);
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0006299A File Offset: 0x0006199A
		public bool Equals(object dataSource, string dataMember)
		{
			return dataSource == this.dataSource && string.Equals(dataMember, this.dataMember, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0400119E RID: 4510
		private object dataSource;

		// Token: 0x0400119F RID: 4511
		private string dataMember;

		// Token: 0x040011A0 RID: 4512
		public static DesignBinding Null = new DesignBinding(null, null);
	}
}
