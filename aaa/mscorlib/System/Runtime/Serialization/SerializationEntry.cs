using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000363 RID: 867
	[ComVisible(true)]
	public struct SerializationEntry
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x00057EAE File Offset: 0x00056EAE
		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x00057EB6 File Offset: 0x00056EB6
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x00057EBE File Offset: 0x00056EBE
		public Type ObjectType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00057EC6 File Offset: 0x00056EC6
		internal SerializationEntry(string entryName, object entryValue, Type entryType)
		{
			this.m_value = entryValue;
			this.m_name = entryName;
			this.m_type = entryType;
		}

		// Token: 0x04000E54 RID: 3668
		private Type m_type;

		// Token: 0x04000E55 RID: 3669
		private object m_value;

		// Token: 0x04000E56 RID: 3670
		private string m_name;
	}
}
