using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002A RID: 42
	public class SortResponseControl : DirectoryControl
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00004CCC File Offset: 0x00003CCC
		internal SortResponseControl(ResultCode result, string attributeName, bool critical, byte[] value)
			: base("1.2.840.113556.1.4.474", value, critical, true)
		{
			this.result = result;
			this.name = attributeName;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004CEB File Offset: 0x00003CEB
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004CF3 File Offset: 0x00003CF3
		public string AttributeName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x040000F4 RID: 244
		private ResultCode result;

		// Token: 0x040000F5 RID: 245
		private string name;
	}
}
