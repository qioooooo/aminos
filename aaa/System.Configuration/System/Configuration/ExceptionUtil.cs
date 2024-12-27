using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000065 RID: 101
	internal static class ExceptionUtil
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00013548 File Offset: 0x00012548
		internal static ArgumentException ParameterInvalid(string parameter)
		{
			return new ArgumentException(SR.GetString("Parameter_Invalid", new object[] { parameter }), parameter);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00013574 File Offset: 0x00012574
		internal static ArgumentException ParameterNullOrEmpty(string parameter)
		{
			return new ArgumentException(SR.GetString("Parameter_NullOrEmpty", new object[] { parameter }), parameter);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000135A0 File Offset: 0x000125A0
		internal static ArgumentException PropertyInvalid(string property)
		{
			return new ArgumentException(SR.GetString("Property_Invalid", new object[] { property }), property);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000135CC File Offset: 0x000125CC
		internal static ArgumentException PropertyNullOrEmpty(string property)
		{
			return new ArgumentException(SR.GetString("Property_NullOrEmpty", new object[] { property }), property);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000135F8 File Offset: 0x000125F8
		internal static InvalidOperationException UnexpectedError(string methodName)
		{
			return new InvalidOperationException(SR.GetString("Unexpected_Error", new object[] { methodName }));
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00013620 File Offset: 0x00012620
		internal static string NoExceptionInformation
		{
			get
			{
				return SR.GetString("No_exception_information_available");
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001362C File Offset: 0x0001262C
		internal static ConfigurationErrorsException WrapAsConfigException(string outerMessage, Exception e, IConfigErrorInfo errorInfo)
		{
			if (errorInfo != null)
			{
				return ExceptionUtil.WrapAsConfigException(outerMessage, e, errorInfo.Filename, errorInfo.LineNumber);
			}
			return ExceptionUtil.WrapAsConfigException(outerMessage, e, null, 0);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00013650 File Offset: 0x00012650
		internal static ConfigurationErrorsException WrapAsConfigException(string outerMessage, Exception e, string filename, int line)
		{
			ConfigurationErrorsException ex = e as ConfigurationErrorsException;
			if (ex != null)
			{
				return ex;
			}
			ConfigurationException ex2 = e as ConfigurationException;
			if (ex2 != null)
			{
				return new ConfigurationErrorsException(ex2);
			}
			XmlException ex3 = e as XmlException;
			if (ex3 != null)
			{
				if (ex3.LineNumber != 0)
				{
					line = ex3.LineNumber;
				}
				return new ConfigurationErrorsException(ex3.Message, ex3, filename, line);
			}
			if (e != null)
			{
				return new ConfigurationErrorsException(SR.GetString("Wrapped_exception_message", new object[] { outerMessage, e.Message }), e, filename, line);
			}
			return new ConfigurationErrorsException(SR.GetString("Wrapped_exception_message", new object[]
			{
				outerMessage,
				ExceptionUtil.NoExceptionInformation
			}), filename, line);
		}
	}
}
