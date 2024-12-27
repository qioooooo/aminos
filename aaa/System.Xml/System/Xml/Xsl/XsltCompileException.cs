using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Xsl
{
	// Token: 0x02000178 RID: 376
	[Serializable]
	public class XsltCompileException : XsltException
	{
		// Token: 0x06001403 RID: 5123 RVA: 0x0005639C File Offset: 0x0005539C
		protected XsltCompileException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000563A6 File Offset: 0x000553A6
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000563B0 File Offset: 0x000553B0
		public XsltCompileException()
		{
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000563B8 File Offset: 0x000553B8
		public XsltCompileException(string message)
			: base(message)
		{
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000563C1 File Offset: 0x000553C1
		public XsltCompileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000563CC File Offset: 0x000553CC
		public XsltCompileException(Exception inner, string sourceUri, int lineNumber, int linePosition)
			: base((lineNumber != 0) ? "Xslt_CompileError" : "Xslt_CompileError2", new string[]
			{
				sourceUri,
				lineNumber.ToString(CultureInfo.InvariantCulture),
				linePosition.ToString(CultureInfo.InvariantCulture)
			}, sourceUri, lineNumber, linePosition, inner)
		{
		}
	}
}
