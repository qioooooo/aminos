using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020003F1 RID: 1009
	internal static class ValidationHelper
	{
		// Token: 0x06002090 RID: 8336 RVA: 0x00080998 File Offset: 0x0007F998
		public static string[] MakeEmptyArrayNull(string[] stringArray)
		{
			if (stringArray == null || stringArray.Length == 0)
			{
				return null;
			}
			return stringArray;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000809A5 File Offset: 0x0007F9A5
		public static string MakeStringNull(string stringValue)
		{
			if (stringValue == null || stringValue.Length == 0)
			{
				return null;
			}
			return stringValue;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000809B5 File Offset: 0x0007F9B5
		public static string ExceptionMessage(Exception exception)
		{
			if (exception == null)
			{
				return string.Empty;
			}
			if (exception.InnerException == null)
			{
				return exception.Message;
			}
			return exception.Message + " (" + ValidationHelper.ExceptionMessage(exception.InnerException) + ")";
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000809F0 File Offset: 0x0007F9F0
		public static string ToString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			if (objectValue is Exception)
			{
				return ValidationHelper.ExceptionMessage(objectValue as Exception);
			}
			if (objectValue is IntPtr)
			{
				return "0x" + ((IntPtr)objectValue).ToString("x");
			}
			return objectValue.ToString();
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00080A64 File Offset: 0x0007FA64
		public static string HashString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00080AA8 File Offset: 0x0007FAA8
		public static bool IsInvalidHttpString(string stringValue)
		{
			return stringValue.IndexOfAny(ValidationHelper.InvalidParamChars) != -1;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00080ABB File Offset: 0x0007FABB
		public static bool IsBlankString(string stringValue)
		{
			return stringValue == null || stringValue.Length == 0;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00080ACB File Offset: 0x0007FACB
		public static bool ValidateTcpPort(int port)
		{
			return port >= 0 && port <= 65535;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00080ADE File Offset: 0x0007FADE
		public static bool ValidateRange(int actual, int fromAllowed, int toAllowed)
		{
			return actual >= fromAllowed && actual <= toAllowed;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00080AED File Offset: 0x0007FAED
		internal static void ValidateSegment(ArraySegment<byte> segment)
		{
			if (segment.Offset < 0 || segment.Count < 0 || segment.Count > segment.Array.Length - segment.Offset)
			{
				throw new ArgumentOutOfRangeException("segment");
			}
		}

		// Token: 0x04001FD3 RID: 8147
		public static string[] EmptyArray = new string[0];

		// Token: 0x04001FD4 RID: 8148
		internal static readonly char[] InvalidMethodChars = new char[] { ' ', '\r', '\n', '\t' };

		// Token: 0x04001FD5 RID: 8149
		internal static readonly char[] InvalidParamChars = new char[]
		{
			'(', ')', '<', '>', '@', ',', ';', ':', '\\', '"',
			'\'', '/', '[', ']', '?', '=', '{', '}', ' ', '\t',
			'\r', '\n'
		};
	}
}
