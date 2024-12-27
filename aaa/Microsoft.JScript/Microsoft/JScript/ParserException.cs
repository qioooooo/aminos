using System;
using System.Globalization;

namespace Microsoft.JScript
{
	// Token: 0x020000BA RID: 186
	[Serializable]
	public class ParserException : Exception
	{
		// Token: 0x06000885 RID: 2181 RVA: 0x00040E16 File Offset: 0x0003FE16
		internal ParserException()
			: base(JScriptException.Localize("Parser Exception", CultureInfo.CurrentUICulture))
		{
		}
	}
}
