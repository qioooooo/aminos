using System;
using System.Reflection;

namespace System.Xml.Serialization
{
	// Token: 0x020002DC RID: 732
	internal class ConstantModel
	{
		// Token: 0x0600225C RID: 8796 RVA: 0x000A0E74 File Offset: 0x0009FE74
		internal ConstantModel(FieldInfo fieldInfo, long value)
		{
			this.fieldInfo = fieldInfo;
			this.value = value;
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x000A0E8A File Offset: 0x0009FE8A
		internal string Name
		{
			get
			{
				return this.fieldInfo.Name;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x000A0E97 File Offset: 0x0009FE97
		internal long Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x000A0E9F File Offset: 0x0009FE9F
		internal FieldInfo FieldInfo
		{
			get
			{
				return this.fieldInfo;
			}
		}

		// Token: 0x040014BD RID: 5309
		private FieldInfo fieldInfo;

		// Token: 0x040014BE RID: 5310
		private long value;
	}
}
