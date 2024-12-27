using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x0200060B RID: 1547
	[ComVisible(true)]
	[Serializable]
	public sealed class XmlSyntaxException : SystemException
	{
		// Token: 0x06003844 RID: 14404 RVA: 0x000BEA92 File Offset: 0x000BDA92
		public XmlSyntaxException()
			: base(Environment.GetResourceString("XMLSyntax_InvalidSyntax"))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000BEAAF File Offset: 0x000BDAAF
		public XmlSyntaxException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000BEAC3 File Offset: 0x000BDAC3
		public XmlSyntaxException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000BEAD8 File Offset: 0x000BDAD8
		public XmlSyntaxException(int lineNumber)
			: base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XMLSyntax_SyntaxError"), new object[] { lineNumber }))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000BEB1C File Offset: 0x000BDB1C
		public XmlSyntaxException(int lineNumber, string message)
			: base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XMLSyntax_SyntaxErrorEx"), new object[] { lineNumber, message }))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000BEB63 File Offset: 0x000BDB63
		internal XmlSyntaxException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
