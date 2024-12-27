using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000082 RID: 130
	internal class OMObjectClass
	{
		// Token: 0x060003C1 RID: 961 RVA: 0x000141B7 File Offset: 0x000131B7
		public OMObjectClass(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000141C8 File Offset: 0x000131C8
		public bool Equals(OMObjectClass OMObjectClass)
		{
			bool flag = true;
			if (this.data.Length == OMObjectClass.data.Length)
			{
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i] != OMObjectClass.data[i])
					{
						flag = false;
						break;
					}
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00014217 File Offset: 0x00013217
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04000391 RID: 913
		public byte[] data;
	}
}
