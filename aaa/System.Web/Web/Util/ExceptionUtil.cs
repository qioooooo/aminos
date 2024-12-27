using System;

namespace System.Web.Util
{
	// Token: 0x02000761 RID: 1889
	internal static class ExceptionUtil
	{
		// Token: 0x06005BE2 RID: 23522 RVA: 0x00170884 File Offset: 0x0016F884
		internal static ArgumentException ParameterInvalid(string parameter)
		{
			return new ArgumentException(SR.GetString("Parameter_Invalid", new object[] { parameter }), parameter);
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x001708B0 File Offset: 0x0016F8B0
		internal static ArgumentException ParameterNullOrEmpty(string parameter)
		{
			return new ArgumentException(SR.GetString("Parameter_NullOrEmpty", new object[] { parameter }), parameter);
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x001708DC File Offset: 0x0016F8DC
		internal static ArgumentException PropertyInvalid(string property)
		{
			return new ArgumentException(SR.GetString("Property_Invalid", new object[] { property }), property);
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x00170908 File Offset: 0x0016F908
		internal static ArgumentException PropertyNullOrEmpty(string property)
		{
			return new ArgumentException(SR.GetString("Property_NullOrEmpty", new object[] { property }), property);
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x00170934 File Offset: 0x0016F934
		internal static InvalidOperationException UnexpectedError(string methodName)
		{
			return new InvalidOperationException(SR.GetString("Unexpected_Error", new object[] { methodName }));
		}
	}
}
