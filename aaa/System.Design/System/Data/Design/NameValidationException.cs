using System;

namespace System.Data.Design
{
	// Token: 0x020000AC RID: 172
	[Serializable]
	internal sealed class NameValidationException : ApplicationException
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x00012B27 File Offset: 0x00011B27
		public NameValidationException(string message)
			: base(message)
		{
		}
	}
}
