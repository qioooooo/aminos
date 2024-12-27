using System;

namespace System.Xml.Xsl
{
	// Token: 0x020000EE RID: 238
	internal interface IErrorHelper
	{
		// Token: 0x06000AC6 RID: 2758
		void ReportError(string res, params string[] args);

		// Token: 0x06000AC7 RID: 2759
		void ReportWarning(string res, params string[] args);
	}
}
