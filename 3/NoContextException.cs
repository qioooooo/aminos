using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.JScript
{
	// Token: 0x020000A5 RID: 165
	[Serializable]
	public class NoContextException : ApplicationException
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x00035853 File Offset: 0x00034853
		public NoContextException()
			: base(JScriptException.Localize("No Source Context available", CultureInfo.CurrentUICulture))
		{
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0003586A File Offset: 0x0003486A
		public NoContextException(string m)
			: base(m)
		{
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00035873 File Offset: 0x00034873
		public NoContextException(string m, Exception e)
			: base(m, e)
		{
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003587D File Offset: 0x0003487D
		protected NoContextException(SerializationInfo s, StreamingContext c)
			: base(s, c)
		{
		}
	}
}
