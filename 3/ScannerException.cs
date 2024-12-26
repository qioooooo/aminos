using System;
using System.Globalization;

namespace Microsoft.JScript
{
	// Token: 0x02000110 RID: 272
	internal class ScannerException : Exception
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x00056DC7 File Offset: 0x00055DC7
		internal ScannerException(JSError errorId)
			: base(JScriptException.Localize("Scanner Exception", CultureInfo.CurrentUICulture))
		{
			this.m_errorId = errorId;
		}

		// Token: 0x040006DB RID: 1755
		internal JSError m_errorId;
	}
}
