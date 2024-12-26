using System;

namespace Microsoft.JScript
{
	// Token: 0x02000067 RID: 103
	public sealed class DynamicFieldInfo
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x00024B00 File Offset: 0x00023B00
		public DynamicFieldInfo(string name, object value)
		{
			this.name = name;
			this.value = value;
			this.fieldTypeName = "";
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00024B21 File Offset: 0x00023B21
		public DynamicFieldInfo(string name, object value, string fieldTypeName)
		{
			this.name = name;
			this.value = value;
			this.fieldTypeName = fieldTypeName;
		}

		// Token: 0x0400022F RID: 559
		public string name;

		// Token: 0x04000230 RID: 560
		public object value;

		// Token: 0x04000231 RID: 561
		public string fieldTypeName;
	}
}
