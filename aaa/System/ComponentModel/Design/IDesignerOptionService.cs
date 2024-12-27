using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000164 RID: 356
	public interface IDesignerOptionService
	{
		// Token: 0x06000B7F RID: 2943
		object GetOptionValue(string pageName, string valueName);

		// Token: 0x06000B80 RID: 2944
		void SetOptionValue(string pageName, string valueName, object value);
	}
}
