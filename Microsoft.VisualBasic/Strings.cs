using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Strings
	{
		private static string CachedYesNoFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = Strings.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				string cachedYesNoFormatStyle;
				lock (syncObject)
				{
					if (Strings.m_LastUsedYesNoCulture != cultureInfo)
					{
						Strings.m_LastUsedYesNoCulture = cultureInfo;
						Strings.m_CachedYesNoFormatStyle = Utils.GetResourceString("YesNoFormatStyle");
					}
					cachedYesNoFormatStyle = Strings.m_CachedYesNoFormatStyle;
				}
				return cachedYesNoFormatStyle;
			}
		}

		private static string CachedOnOffFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = Strings.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				string cachedOnOffFormatStyle;
				lock (syncObject)
				{
					if (Strings.m_LastUsedOnOffCulture != cultureInfo)
					{
						Strings.m_LastUsedOnOffCulture = cultureInfo;
						Strings.m_CachedOnOffFormatStyle = Utils.GetResourceString("OnOffFormatStyle");
					}
					cachedOnOffFormatStyle = Strings.m_CachedOnOffFormatStyle;
				}
				return cachedOnOffFormatStyle;
			}
		}

		private static string CachedTrueFalseFormatStyle
		{
			get
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				object syncObject = Strings.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				string cachedTrueFalseFormatStyle;
				lock (syncObject)
				{
					if (Strings.m_LastUsedTrueFalseCulture != cultureInfo)
					{
						Strings.m_LastUsedTrueFalseCulture = cultureInfo;
						Strings.m_CachedTrueFalseFormatStyle = Utils.GetResourceString("TrueFalseFormatStyle");
					}
					cachedTrueFalseFormatStyle = Strings.m_CachedTrueFalseFormatStyle;
				}
				return cachedTrueFalseFormatStyle;
			}
		}

		private static int PRIMARYLANGID(int lcid)
		{
			return lcid & 1023;
		}

		public static int Asc(char String)
		{
			int num = Convert.ToInt32(String);
			if (num < 128)
			{
				return num;
			}
			int num3;
			try
			{
				Encoding fileIOEncoding = Utils.GetFileIOEncoding();
				char[] array = new char[] { String };
				if (fileIOEncoding.IsSingleByte)
				{
					byte[] array2 = new byte[1];
					int num2 = fileIOEncoding.GetBytes(array, 0, 1, array2, 0);
					num3 = (int)array2[0];
				}
				else
				{
					byte[] array2 = new byte[2];
					int num2 = fileIOEncoding.GetBytes(array, 0, 1, array2, 0);
					if (num2 == 1)
					{
						num3 = (int)array2[0];
					}
					else
					{
						if (BitConverter.IsLittleEndian)
						{
							byte b = array2[0];
							array2[0] = array2[1];
							array2[1] = b;
						}
						num3 = (int)BitConverter.ToInt16(array2, 0);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return num3;
		}

		public static int Asc(string String)
		{
			if (String == null || String.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", new string[] { "String" }));
			}
			char c = String[0];
			return Strings.Asc(c);
		}

		public static int AscW(string String)
		{
			if (String == null || String.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", new string[] { "String" }));
			}
			return (int)String[0];
		}

		public static int AscW(char String)
		{
			return (int)String;
		}

		public static char Chr(int CharCode)
		{
			if (CharCode < -32768 || CharCode > 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RangeTwoBytes1", new string[] { "CharCode" }));
			}
			if (CharCode >= 0 && CharCode <= 127)
			{
				return Convert.ToChar(CharCode);
			}
			checked
			{
				char c;
				try
				{
					Encoding encoding = Encoding.GetEncoding(Utils.GetLocaleCodePage());
					if (encoding.IsSingleByte && (CharCode < 0 || CharCode > 255))
					{
						throw ExceptionUtils.VbMakeException(5);
					}
					char[] array = new char[2];
					byte[] array2 = new byte[2];
					Decoder decoder = encoding.GetDecoder();
					if (CharCode >= 0 && CharCode <= 255)
					{
						array2[0] = (byte)(CharCode & 255);
						int num = decoder.GetChars(array2, 0, 1, array, 0);
					}
					else
					{
						array2[0] = (byte)((CharCode & 65280) >> 8);
						array2[1] = (byte)(CharCode & 255);
						int num = decoder.GetChars(array2, 0, 2, array, 0);
					}
					c = array[0];
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return c;
			}
		}

		public static char ChrW(int CharCode)
		{
			if (CharCode < -32768 || CharCode > 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_RangeTwoBytes1", new string[] { "CharCode" }));
			}
			return Convert.ToChar(CharCode & 65535);
		}

		public static string[] Filter(object[] Source, string Match, bool Include = true, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			int num = Information.UBound(Source, 1);
			checked
			{
				string[] array = new string[num + 1];
				try
				{
					int num2 = 0;
					int num3 = num;
					for (int i = num2; i <= num3; i++)
					{
						array[i] = Conversions.ToString(Source[i]);
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "Source", "String" }));
				}
				return Strings.Filter(array, Match, Include, Compare);
			}
		}

		public static string[] Filter(string[] Source, string Match, bool Include = true, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			checked
			{
				string[] array;
				try
				{
					if (Source.Rank != 1)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1"));
					}
					if (Match == null || Match.Length == 0)
					{
						array = null;
					}
					else
					{
						int num = Source.Length;
						CultureInfo cultureInfo = Utils.GetCultureInfo();
						CompareInfo compareInfo = cultureInfo.CompareInfo;
						CompareOptions compareOptions;
						if (Compare == CompareMethod.Text)
						{
							compareOptions = CompareOptions.IgnoreCase;
						}
						string[] array2 = new string[num - 1 + 1];
						int num2 = 0;
						int num3 = num - 1;
						int num4;
						for (int i = num2; i <= num3; i++)
						{
							string text = Source[i];
							if (text != null)
							{
								if (compareInfo.IndexOf(text, Match, compareOptions) >= 0 == Include)
								{
									array2[num4] = text;
									num4++;
								}
							}
						}
						if (num4 == 0)
						{
							array2 = new string[0];
							array = array2;
						}
						else if (num4 == array2.Length)
						{
							array = array2;
						}
						else
						{
							array2 = (string[])Utils.CopyArray((Array)array2, new string[num4 - 1 + 1]);
							array = array2;
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return array;
			}
		}

		public static int InStr(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			checked
			{
				if (Compare == CompareMethod.Binary)
				{
					return Strings.InternalInStrBinary(0, String1, String2) + 1;
				}
				return Strings.InternalInStrText(0, String1, String2) + 1;
			}
		}

		public static int InStr(int Start, string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			if (Start < 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", new string[] { "Start" }));
			}
			checked
			{
				if (Compare == CompareMethod.Binary)
				{
					return Strings.InternalInStrBinary(Start - 1, String1, String2) + 1;
				}
				return Strings.InternalInStrText(Start - 1, String1, String2) + 1;
			}
		}

		private static int InternalInStrBinary(int StartPos, string sSrc, string sFind)
		{
			int num;
			if (sSrc != null)
			{
				num = sSrc.Length;
			}
			else
			{
				num = 0;
			}
			if (StartPos > num || num == 0)
			{
				return -1;
			}
			if (sFind == null || sFind.Length == 0)
			{
				return StartPos;
			}
			return Strings.m_InvariantCompareInfo.IndexOf(sSrc, sFind, StartPos, CompareOptions.Ordinal);
		}

		private static int InternalInStrText(int lStartPos, string sSrc, string sFind)
		{
			int num;
			if (sSrc != null)
			{
				num = sSrc.Length;
			}
			else
			{
				num = 0;
			}
			if (lStartPos > num || num == 0)
			{
				return -1;
			}
			if (sFind == null || sFind.Length == 0)
			{
				return lStartPos;
			}
			return Utils.GetCultureInfo().CompareInfo.IndexOf(sSrc, sFind, lStartPos, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		public static int InStrRev(string StringCheck, string StringMatch, int Start = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			checked
			{
				int num2;
				try
				{
					if (Start == 0 || Start < -1)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_MinusOneOrGTZero1", new string[] { "Start" }));
					}
					int num;
					if (StringCheck == null)
					{
						num = 0;
					}
					else
					{
						num = StringCheck.Length;
					}
					if (Start == -1)
					{
						Start = num;
					}
					if (Start > num || num == 0)
					{
						num2 = 0;
					}
					else
					{
						if (StringMatch != null)
						{
							if (StringMatch.Length != 0)
							{
								if (Compare == CompareMethod.Binary)
								{
									return Strings.m_InvariantCompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.Ordinal) + 1;
								}
								return Utils.GetCultureInfo().CompareInfo.LastIndexOf(StringCheck, StringMatch, Start - 1, Start, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) + 1;
							}
						}
						num2 = Start;
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return num2;
			}
		}

		public static string Join(object[] SourceArray, string Delimiter = " ")
		{
			int num = Information.UBound(SourceArray, 1);
			checked
			{
				string[] array = new string[num + 1];
				try
				{
					int num2 = 0;
					int num3 = num;
					for (int i = num2; i <= num3; i++)
					{
						array[i] = Conversions.ToString(SourceArray[i]);
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "SourceArray", "String" }));
				}
				return Strings.Join(array, Delimiter);
			}
		}

		public static string Join(string[] SourceArray, string Delimiter = " ")
		{
			string text;
			try
			{
				if (Strings.IsArrayEmpty(SourceArray))
				{
					text = null;
				}
				else
				{
					if (SourceArray.Rank != 1)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1"));
					}
					text = string.Join(Delimiter, SourceArray);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static string LCase(string Value)
		{
			string text;
			try
			{
				if (Value == null)
				{
					text = null;
				}
				else
				{
					text = Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static char LCase(char Value)
		{
			char c;
			try
			{
				c = Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return c;
		}

		public static int Len(bool Expression)
		{
			return 2;
		}

		[CLSCompliant(false)]
		public static int Len(sbyte Expression)
		{
			return 1;
		}

		public static int Len(byte Expression)
		{
			return 1;
		}

		public static int Len(short Expression)
		{
			return 2;
		}

		[CLSCompliant(false)]
		public static int Len(ushort Expression)
		{
			return 2;
		}

		public static int Len(int Expression)
		{
			return 4;
		}

		[CLSCompliant(false)]
		public static int Len(uint Expression)
		{
			return 4;
		}

		public static int Len(long Expression)
		{
			return 8;
		}

		[CLSCompliant(false)]
		public static int Len(ulong Expression)
		{
			return 8;
		}

		public static int Len(decimal Expression)
		{
			return 8;
		}

		public static int Len(float Expression)
		{
			return 4;
		}

		public static int Len(double Expression)
		{
			return 8;
		}

		public static int Len(DateTime Expression)
		{
			return 8;
		}

		public static int Len(char Expression)
		{
			return 2;
		}

		public static int Len(string Expression)
		{
			if (Expression == null)
			{
				return 0;
			}
			return Expression.Length;
		}

		public static int Len(object Expression)
		{
			if (Expression == null)
			{
				return 0;
			}
			IConvertible convertible = Expression as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return 2;
				case TypeCode.Char:
					return 2;
				case TypeCode.SByte:
					return 1;
				case TypeCode.Byte:
					return 1;
				case TypeCode.Int16:
					return 2;
				case TypeCode.UInt16:
					return 2;
				case TypeCode.Int32:
					return 4;
				case TypeCode.UInt32:
					return 4;
				case TypeCode.Int64:
					return 8;
				case TypeCode.UInt64:
					return 8;
				case TypeCode.Single:
					return 4;
				case TypeCode.Double:
					return 8;
				case TypeCode.Decimal:
					return 16;
				case TypeCode.DateTime:
					return 8;
				case TypeCode.String:
					return Expression.ToString().Length;
				}
			}
			else
			{
				char[] array = Expression as char[];
				if (array != null)
				{
					return array.Length;
				}
			}
			if (Expression is ValueType)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
				int recordLength = StructUtils.GetRecordLength(Expression, 1);
				PermissionSet.RevertAssert();
				return recordLength;
			}
			throw ExceptionUtils.VbMakeException(13);
		}

		public static string Replace(string Expression, string Find, string Replacement, int Start = 1, int Count = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			string text;
			try
			{
				if (Count < -1)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_GEMinusOne1", new string[] { "Count" }));
				}
				if (Start <= 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", new string[] { "Start" }));
				}
				if (Expression == null || Start > Expression.Length)
				{
					text = null;
				}
				else
				{
					if (Start != 1)
					{
						Expression = Expression.Substring(checked(Start - 1));
					}
					if (Find != null)
					{
						if (Find.Length != 0 && Count != 0)
						{
							if (Count == -1)
							{
								Count = Expression.Length;
							}
							return Strings.ReplaceInternal(Expression, Find, Replacement, Count, Compare);
						}
					}
					text = Expression;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		private static string ReplaceInternal(string Expression, string Find, string Replacement, int Count, CompareMethod Compare)
		{
			int length = Expression.Length;
			int length2 = Find.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			CompareInfo compareInfo;
			CompareOptions compareOptions;
			if (Compare == CompareMethod.Text)
			{
				compareInfo = Utils.GetCultureInfo().CompareInfo;
				compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
			}
			else
			{
				compareInfo = Strings.m_InvariantCompareInfo;
				compareOptions = CompareOptions.Ordinal;
			}
			checked
			{
				int i;
				while (i < length)
				{
					int num;
					if (num == Count)
					{
						stringBuilder.Append(Expression.Substring(i));
						break;
					}
					int num2 = compareInfo.IndexOf(Expression, Find, i, compareOptions);
					if (num2 < 0)
					{
						stringBuilder.Append(Expression.Substring(i));
						break;
					}
					stringBuilder.Append(Expression.Substring(i, num2 - i));
					stringBuilder.Append(Replacement);
					num++;
					i = num2 + length2;
				}
				return stringBuilder.ToString();
			}
		}

		public static string Space(int Number)
		{
			if (Number >= 0)
			{
				return new string(' ', Number);
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Number" }));
		}

		public static string[] Split(string Expression, string Delimiter = " ", int Limit = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			string[] array;
			try
			{
				if (Expression != null)
				{
					if (Expression.Length != 0)
					{
						if (Limit == -1)
						{
							Limit = checked(Expression.Length + 1);
						}
						int num;
						if (Delimiter == null)
						{
							num = 0;
						}
						else
						{
							num = Delimiter.Length;
						}
						if (num == 0)
						{
							return new string[] { Expression };
						}
						return Strings.SplitHelper(Expression, Delimiter, Limit, (int)Compare);
					}
				}
				array = new string[] { "" };
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return array;
		}

		private static string[] SplitHelper(string sSrc, string sFind, int cMaxSubStrings, int Compare)
		{
			int num;
			if (sFind == null)
			{
				num = 0;
			}
			else
			{
				num = sFind.Length;
			}
			int num2;
			if (sSrc == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = sSrc.Length;
			}
			if (num == 0)
			{
				return new string[] { sSrc };
			}
			if (num2 == 0)
			{
				return new string[] { sSrc };
			}
			int num3 = 20;
			if (num3 > cMaxSubStrings)
			{
				num3 = cMaxSubStrings;
			}
			checked
			{
				string[] array = new string[num3 + 1];
				CompareOptions compareOptions;
				CompareInfo compareInfo;
				if (Compare == 0)
				{
					compareOptions = CompareOptions.Ordinal;
					compareInfo = Strings.m_InvariantCompareInfo;
				}
				else
				{
					compareInfo = Utils.GetCultureInfo().CompareInfo;
					compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
				}
				int i;
				int num5;
				while (i < num2)
				{
					int num4 = compareInfo.IndexOf(sSrc, sFind, i, num2 - i, compareOptions);
					string text;
					if (num4 == -1 || num5 + 1 == cMaxSubStrings)
					{
						text = sSrc.Substring(i);
						if (text == null)
						{
							text = "";
						}
						array[num5] = text;
						break;
					}
					text = sSrc.Substring(i, num4 - i);
					if (text == null)
					{
						text = "";
					}
					array[num5] = text;
					i = num4 + num;
					num5++;
					if (num5 > num3)
					{
						num3 += 20;
						if (num3 > cMaxSubStrings)
						{
							num3 = cMaxSubStrings + 1;
						}
						array = (string[])Utils.CopyArray((Array)array, new string[num3 + 1]);
					}
					array[num5] = "";
					if (num5 == cMaxSubStrings)
					{
						text = sSrc.Substring(i);
						if (text == null)
						{
							text = "";
						}
						array[num5] = text;
						break;
					}
				}
				if (num5 + 1 == array.Length)
				{
					return array;
				}
				return (string[])Utils.CopyArray((Array)array, new string[num5 + 1]);
			}
		}

		public static string LSet(string Source, int Length)
		{
			if (Length == 0)
			{
				return "";
			}
			if (Source == null)
			{
				return new string(' ', Length);
			}
			if (Length > Source.Length)
			{
				return Source.PadRight(Length);
			}
			return Source.Substring(0, Length);
		}

		public static string RSet(string Source, int Length)
		{
			if (Length == 0)
			{
				return "";
			}
			if (Source == null)
			{
				return new string(' ', Length);
			}
			if (Length > Source.Length)
			{
				return Source.PadLeft(Length);
			}
			return Source.Substring(0, Length);
		}

		public static object StrDup(int Number, object Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Number" }));
			}
			if (Character == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Character" }));
			}
			string text = Character as string;
			char c;
			if (text != null)
			{
				if (text.Length == 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", new string[] { "Character" }));
				}
				c = text[0];
			}
			else
			{
				try
				{
					c = Conversions.ToChar(Character);
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Character" }));
				}
			}
			return new string(c, Number);
		}

		public static string StrDup(int Number, char Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Number" }));
			}
			return new string(Character, Number);
		}

		public static string StrDup(int Number, string Character)
		{
			if (Number < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Number" }));
			}
			if (Character == null || Character.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", new string[] { "Character" }));
			}
			return new string(Character[0], Number);
		}

		public static string StrReverse(string Expression)
		{
			if (Expression == null)
			{
				return "";
			}
			int length = Expression.Length;
			if (length == 0)
			{
				return "";
			}
			int num = 0;
			checked
			{
				int num2 = length - 1;
				for (int i = num; i <= num2; i++)
				{
					char c = Expression[i];
					UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
					if (unicodeCategory == UnicodeCategory.Surrogate || unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.EnclosingMark)
					{
						return Strings.InternalStrReverse(Expression, i, length);
					}
				}
				char[] array = Expression.ToCharArray();
				Array.Reverse(array);
				return new string(array);
			}
		}

		private static string InternalStrReverse(string Expression, int SrcIndex, int Length)
		{
			StringBuilder stringBuilder = new StringBuilder(Length);
			stringBuilder.Length = Length;
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(Expression, SrcIndex);
			if (!textElementEnumerator.MoveNext())
			{
				return "";
			}
			int i = 0;
			checked
			{
				int j = Length - 1;
				while (i < SrcIndex)
				{
					stringBuilder[j] = Expression[i];
					j--;
					i++;
				}
				int num = textElementEnumerator.ElementIndex;
				while (j >= 0)
				{
					SrcIndex = num;
					if (textElementEnumerator.MoveNext())
					{
						num = textElementEnumerator.ElementIndex;
					}
					else
					{
						num = Length;
					}
					for (i = num - 1; i >= SrcIndex; i--)
					{
						stringBuilder[j] = Expression[i];
						j--;
					}
				}
				return stringBuilder.ToString();
			}
		}

		public static string UCase(string Value)
		{
			string text;
			try
			{
				if (Value == null)
				{
					text = "";
				}
				else
				{
					text = Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static char UCase(char Value)
		{
			char c;
			try
			{
				c = Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return c;
		}

		private static bool FormatNamed(object Expression, string Style, ref string ReturnValue)
		{
			int length = Style.Length;
			ReturnValue = null;
			switch (length)
			{
			case 5:
			{
				char c = Style[0];
				if (c == 'f' || c == 'F')
				{
					if (string.Compare(Style, "fixed", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDouble(Expression).ToString("0.00", null);
						return true;
					}
				}
				break;
			}
			case 6:
			{
				char c2 = Style[0];
				if (c2 == 'y' || c2 == 'Y')
				{
					if (string.Compare(Style, "yes/no", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = ((-((Conversions.ToBoolean(Expression) > false) ? 1 : 0)) ? 1 : 0).ToString(Strings.CachedYesNoFormatStyle, null);
						return true;
					}
				}
				else if (c2 == 'o' || c2 == 'O')
				{
					if (string.Compare(Style, "on/off", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = ((-((Conversions.ToBoolean(Expression) > false) ? 1 : 0)) ? 1 : 0).ToString(Strings.CachedOnOffFormatStyle, null);
						return true;
					}
				}
				break;
			}
			case 7:
			{
				char c3 = Style[0];
				if (c3 == 'p' || c3 == 'P')
				{
					if (string.Compare(Style, "percent", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDouble(Expression).ToString("0.00%", null);
						return true;
					}
				}
				break;
			}
			case 8:
			{
				char c4 = Style[0];
				if (c4 == 's' || c4 == 'S')
				{
					if (string.Compare(Style, "standard", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDouble(Expression).ToString("N2", null);
						return true;
					}
				}
				else if (c4 == 'c' || c4 == 'C')
				{
					if (string.Compare(Style, "currency", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDouble(Expression).ToString("C", null);
						return true;
					}
				}
				break;
			}
			case 9:
			{
				char c5 = Style[5];
				if (c5 == 't' || c5 == 'T')
				{
					if (string.Compare(Style, "long time", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("T", null);
						return true;
					}
				}
				else if (c5 == 'd' || c5 == 'D')
				{
					if (string.Compare(Style, "long date", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("D", null);
						return true;
					}
				}
				break;
			}
			case 10:
				switch (Style[6])
				{
				case 'A':
				case 'a':
					if (string.Compare(Style, "true/false", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = ((-((Conversions.ToBoolean(Expression) > false) ? 1 : 0)) ? 1 : 0).ToString(Strings.CachedTrueFalseFormatStyle, null);
						return true;
					}
					break;
				case 'D':
				case 'd':
					if (string.Compare(Style, "short date", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("d", null);
						return true;
					}
					break;
				case 'I':
				case 'i':
					if (string.Compare(Style, "scientific", StringComparison.OrdinalIgnoreCase) == 0)
					{
						double num = Conversions.ToDouble(Expression);
						if (double.IsNaN(num) || double.IsInfinity(num))
						{
							ReturnValue = num.ToString("G", null);
						}
						else
						{
							ReturnValue = num.ToString("0.00E+00", null);
						}
						return true;
					}
					break;
				case 'T':
				case 't':
					if (string.Compare(Style, "short time", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("t", null);
						return true;
					}
					break;
				}
				break;
			case 11:
			{
				char c6 = Style[7];
				if (c6 == 't' || c6 == 'T')
				{
					if (string.Compare(Style, "medium time", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("T", null);
						return true;
					}
				}
				else if (c6 == 'd' || c6 == 'D')
				{
					if (string.Compare(Style, "medium date", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("D", null);
						return true;
					}
				}
				break;
			}
			case 12:
			{
				char c7 = Style[0];
				if (c7 == 'g' || c7 == 'G')
				{
					if (string.Compare(Style, "general date", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ReturnValue = Conversions.ToDate(Expression).ToString("G", null);
						return true;
					}
				}
				break;
			}
			case 14:
			{
				char c8 = Style[0];
				if ((c8 == 'g' || c8 == 'G') && string.Compare(Style, "general number", StringComparison.OrdinalIgnoreCase) == 0)
				{
					ReturnValue = Conversions.ToDouble(Expression).ToString("G", null);
					return true;
				}
				break;
			}
			}
			return false;
		}

		public static string Format(object Expression, string Style = "")
		{
			string text;
			try
			{
				IFormatProvider formatProvider = null;
				if (Expression == null || Expression.GetType() == null)
				{
					text = "";
				}
				else if (Style == null || Style.Length == 0)
				{
					text = Conversions.ToString(Expression);
				}
				else
				{
					IConvertible convertible = (IConvertible)Expression;
					TypeCode typeCode = convertible.GetTypeCode();
					if (Style.Length > 0)
					{
						try
						{
							string text2 = null;
							if (Strings.FormatNamed(Expression, Style, ref text2))
							{
								return text2;
							}
						}
						catch (StackOverflowException ex)
						{
							throw ex;
						}
						catch (OutOfMemoryException ex2)
						{
							throw ex2;
						}
						catch (ThreadAbortException ex3)
						{
							throw ex3;
						}
						catch (Exception)
						{
							return Conversions.ToString(Expression);
						}
					}
					IFormattable formattable = Expression as IFormattable;
					if (formattable == null)
					{
						typeCode = Convert.GetTypeCode(Expression);
						if (typeCode != TypeCode.String && typeCode != TypeCode.Boolean)
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Expression" }));
						}
					}
					switch (typeCode)
					{
					case TypeCode.Empty:
						return "";
					case TypeCode.Object:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
						return formattable.ToString(Style, formatProvider);
					case TypeCode.DBNull:
						return "";
					case TypeCode.Boolean:
						return string.Format(formatProvider, Style, new object[] { Conversions.ToString(convertible.ToBoolean(null)) });
					case TypeCode.Single:
					{
						float num = convertible.ToSingle(null);
						if (Style == null || Style.Length == 0)
						{
							return Conversions.ToString(num);
						}
						if (num == 0f)
						{
							num = 0f;
						}
						return num.ToString(Style, formatProvider);
					}
					case TypeCode.Double:
					{
						double num2 = convertible.ToDouble(null);
						if (Style == null || Style.Length == 0)
						{
							return Conversions.ToString(num2);
						}
						if (num2 == 0.0)
						{
							num2 = 0.0;
						}
						return num2.ToString(Style, formatProvider);
					}
					case TypeCode.String:
						return string.Format(formatProvider, Style, new object[] { Expression });
					}
					text = formattable.ToString(Style, formatProvider);
				}
			}
			catch (Exception ex4)
			{
				throw ex4;
			}
			return text;
		}

		public static string FormatCurrency(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			IFormatProvider formatProvider = null;
			string text;
			try
			{
				Strings.ValidateTriState(IncludeLeadingDigit);
				Strings.ValidateTriState(UseParensForNegativeNumbers);
				Strings.ValidateTriState(GroupDigits);
				if (NumDigitsAfterDecimal > 99)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", new string[] { "NumDigitsAfterDecimal" }));
				}
				if (Expression == null)
				{
					text = "";
				}
				else
				{
					Type type = Expression.GetType();
					if (type == typeof(string))
					{
						Expression = Conversions.ToDouble(Expression);
					}
					else if (!Symbols.IsNumericType(type))
					{
						throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
						{
							Utils.VBFriendlyName(type),
							"Currency"
						}));
					}
					IFormattable formattable = (IFormattable)Expression;
					if (IncludeLeadingDigit == TriState.False)
					{
						double num = Conversions.ToDouble(Expression);
						if (num >= 1.0 || num <= -1.0)
						{
							IncludeLeadingDigit = TriState.True;
						}
					}
					string currencyFormatString = Strings.GetCurrencyFormatString(IncludeLeadingDigit, NumDigitsAfterDecimal, UseParensForNegativeNumbers, GroupDigits, ref formatProvider);
					text = formattable.ToString(currencyFormatString, formatProvider);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static string FormatDateTime(DateTime Expression, DateFormat NamedFormat = DateFormat.GeneralDate)
		{
			string text2;
			try
			{
				string text;
				switch (NamedFormat)
				{
				case DateFormat.GeneralDate:
					if (Expression.TimeOfDay.Ticks == Expression.Ticks)
					{
						text = "T";
					}
					else if (Expression.TimeOfDay.Ticks == 0L)
					{
						text = "d";
					}
					else
					{
						text = "G";
					}
					break;
				case DateFormat.LongDate:
					text = "D";
					break;
				case DateFormat.ShortDate:
					text = "d";
					break;
				case DateFormat.LongTime:
					text = "T";
					break;
				case DateFormat.ShortTime:
					text = "HH:mm";
					break;
				default:
					throw ExceptionUtils.VbMakeException(5);
				}
				text2 = Expression.ToString(text, null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text2;
		}

		public static string FormatNumber(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			string text;
			try
			{
				Strings.ValidateTriState(IncludeLeadingDigit);
				Strings.ValidateTriState(UseParensForNegativeNumbers);
				Strings.ValidateTriState(GroupDigits);
				if (Expression == null)
				{
					text = "";
				}
				else
				{
					Type type = Expression.GetType();
					if (type == typeof(string))
					{
						Expression = Conversions.ToDouble(Expression);
					}
					else if (type == typeof(bool))
					{
						if (Conversions.ToBoolean(Expression))
						{
							Expression = -1.0;
						}
						else
						{
							Expression = 0.0;
						}
					}
					else if (!Symbols.IsNumericType(type))
					{
						throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
						{
							Utils.VBFriendlyName(type),
							"Currency"
						}));
					}
					IFormattable formattable = (IFormattable)Expression;
					text = formattable.ToString(Strings.GetNumberFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits), null);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		internal static string GetFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits, Strings.FormatType FormatTypeValue)
		{
			StringBuilder stringBuilder = new StringBuilder(30);
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)cultureInfo.GetFormat(typeof(NumberFormatInfo));
			if (NumDigitsAfterDecimal < -1)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			if (NumDigitsAfterDecimal == -1)
			{
				if (FormatTypeValue == Strings.FormatType.Percent)
				{
					NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
				}
				else if (FormatTypeValue == Strings.FormatType.Number)
				{
					NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
				}
				else if (FormatTypeValue == Strings.FormatType.Currency)
				{
					NumDigitsAfterDecimal = numberFormatInfo.CurrencyDecimalDigits;
				}
			}
			if (GroupDigits == TriState.UseDefault)
			{
				GroupDigits = TriState.True;
				if (FormatTypeValue == Strings.FormatType.Percent)
				{
					if (Strings.IsArrayEmpty(numberFormatInfo.PercentGroupSizes))
					{
						GroupDigits = TriState.False;
					}
				}
				else if (FormatTypeValue == Strings.FormatType.Number)
				{
					if (Strings.IsArrayEmpty(numberFormatInfo.NumberGroupSizes))
					{
						GroupDigits = TriState.False;
					}
				}
				else if (FormatTypeValue == Strings.FormatType.Currency && Strings.IsArrayEmpty(numberFormatInfo.CurrencyGroupSizes))
				{
					GroupDigits = TriState.False;
				}
			}
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				UseParensForNegativeNumbers = TriState.False;
				if (FormatTypeValue == Strings.FormatType.Number)
				{
					if (numberFormatInfo.NumberNegativePattern == 0)
					{
						UseParensForNegativeNumbers = TriState.True;
					}
				}
				else if (FormatTypeValue == Strings.FormatType.Currency && numberFormatInfo.CurrencyNegativePattern == 0)
				{
					UseParensForNegativeNumbers = TriState.True;
				}
			}
			string text;
			if (GroupDigits == TriState.True)
			{
				text = "#,##";
			}
			else
			{
				text = "";
			}
			string text2;
			if (IncludeLeadingDigit != TriState.False)
			{
				text2 = "0";
			}
			else
			{
				text2 = "#";
			}
			string text3;
			if (NumDigitsAfterDecimal > 0)
			{
				text3 = "." + new string('0', NumDigitsAfterDecimal);
			}
			else
			{
				text3 = "";
			}
			if (FormatTypeValue == Strings.FormatType.Currency)
			{
				stringBuilder.Append(numberFormatInfo.CurrencySymbol);
			}
			stringBuilder.Append(text);
			stringBuilder.Append(text2);
			stringBuilder.Append(text3);
			if (FormatTypeValue == Strings.FormatType.Percent)
			{
				stringBuilder.Append(numberFormatInfo.PercentSymbol);
			}
			if (UseParensForNegativeNumbers == TriState.True)
			{
				string text4 = stringBuilder.ToString();
				stringBuilder.Append(";(");
				stringBuilder.Append(text4);
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		internal static string GetCurrencyFormatString(TriState IncludeLeadingDigit, int NumDigitsAfterDecimal, TriState UseParensForNegativeNumbers, TriState GroupDigits, ref IFormatProvider formatProvider)
		{
			string text = "C";
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)cultureInfo.GetFormat(typeof(NumberFormatInfo));
			numberFormatInfo = (NumberFormatInfo)numberFormatInfo.Clone();
			if (GroupDigits == TriState.False)
			{
				numberFormatInfo.CurrencyGroupSizes = new int[] { 0 };
			}
			int currencyPositivePattern = numberFormatInfo.CurrencyPositivePattern;
			int num = numberFormatInfo.CurrencyNegativePattern;
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				switch (num)
				{
				case 0:
				case 4:
				case 14:
				case 15:
					UseParensForNegativeNumbers = TriState.True;
					goto IL_0168;
				}
				UseParensForNegativeNumbers = TriState.False;
			}
			else if (UseParensForNegativeNumbers == TriState.False)
			{
				switch (num)
				{
				case 0:
					num = 1;
					break;
				case 4:
					num = 5;
					break;
				case 14:
					num = 9;
					break;
				case 15:
					num = 10;
					break;
				}
			}
			else
			{
				UseParensForNegativeNumbers = TriState.True;
				switch (num)
				{
				case 1:
				case 2:
				case 3:
					num = 0;
					break;
				case 5:
				case 6:
				case 7:
					num = 4;
					break;
				case 8:
				case 10:
				case 13:
					num = 15;
					break;
				case 9:
				case 11:
				case 12:
					num = 14;
					break;
				}
			}
			IL_0168:
			numberFormatInfo.CurrencyNegativePattern = num;
			if (NumDigitsAfterDecimal == -1)
			{
				NumDigitsAfterDecimal = numberFormatInfo.CurrencyDecimalDigits;
			}
			numberFormatInfo.CurrencyDecimalDigits = NumDigitsAfterDecimal;
			formatProvider = new FormatInfoHolder(numberFormatInfo);
			if (IncludeLeadingDigit == TriState.False)
			{
				numberFormatInfo.NumberGroupSizes = numberFormatInfo.CurrencyGroupSizes;
				string text2 = Strings.CurrencyPositiveFormatStrings[currencyPositivePattern] + ";" + Strings.CurrencyNegativeFormatStrings[num];
				string text3;
				if (GroupDigits == TriState.False)
				{
					if (IncludeLeadingDigit == TriState.False)
					{
						text3 = "#";
					}
					else
					{
						text3 = "0";
					}
				}
				else if (IncludeLeadingDigit == TriState.False)
				{
					text3 = "#,###";
				}
				else
				{
					text3 = "#,##0";
				}
				if (NumDigitsAfterDecimal > 0)
				{
					text3 = text3 + "." + new string('0', NumDigitsAfterDecimal);
				}
				if (string.CompareOrdinal("$", numberFormatInfo.CurrencySymbol) != 0)
				{
					text2 = text2.Replace("$", numberFormatInfo.CurrencySymbol.Replace("'", "''"));
				}
				return text2.Replace("n", text3);
			}
			return text;
		}

		internal static string GetNumberFormatString(int NumDigitsAfterDecimal, TriState IncludeLeadingDigit, TriState UseParensForNegativeNumbers, TriState GroupDigits)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)cultureInfo.GetFormat(typeof(NumberFormatInfo));
			if (NumDigitsAfterDecimal == -1)
			{
				NumDigitsAfterDecimal = numberFormatInfo.NumberDecimalDigits;
			}
			else if (NumDigitsAfterDecimal > 99 || NumDigitsAfterDecimal < -1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_Range0to99_1", new string[] { "NumDigitsAfterDecimal" }));
			}
			if (GroupDigits == TriState.UseDefault)
			{
				if (numberFormatInfo.NumberGroupSizes == null || numberFormatInfo.NumberGroupSizes.Length == 0)
				{
					GroupDigits = TriState.False;
				}
				else
				{
					GroupDigits = TriState.True;
				}
			}
			int num = numberFormatInfo.NumberNegativePattern;
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				int num2 = num;
				if (num2 == 0)
				{
					UseParensForNegativeNumbers = TriState.True;
				}
				else
				{
					UseParensForNegativeNumbers = TriState.False;
				}
			}
			else if (UseParensForNegativeNumbers == TriState.False)
			{
				if (num == 0)
				{
					num = 1;
				}
			}
			else
			{
				UseParensForNegativeNumbers = TriState.True;
				switch (num)
				{
				case 1:
				case 2:
				case 3:
				case 4:
					num = 0;
					break;
				}
			}
			if (UseParensForNegativeNumbers == TriState.UseDefault)
			{
				UseParensForNegativeNumbers = TriState.True;
			}
			string text = "n;" + Strings.NumberNegativeFormatStrings[num];
			if (string.CompareOrdinal("-", numberFormatInfo.NegativeSign) != 0)
			{
				text = text.Replace("-", "\"" + numberFormatInfo.NegativeSign + "\"");
			}
			string text2;
			if (IncludeLeadingDigit != TriState.False)
			{
				text2 = "0";
			}
			else
			{
				text2 = "#";
			}
			checked
			{
				if (GroupDigits != TriState.False && numberFormatInfo.NumberGroupSizes.Length != 0)
				{
					if (numberFormatInfo.NumberGroupSizes.Length == 1)
					{
						text2 = "#," + new string('#', numberFormatInfo.NumberGroupSizes[0]) + text2;
					}
					else
					{
						text2 = new string('#', numberFormatInfo.NumberGroupSizes[0] - 1) + text2;
						int num3 = 1;
						int upperBound = numberFormatInfo.NumberGroupSizes.GetUpperBound(0);
						for (int i = num3; i <= upperBound; i++)
						{
							text2 = "," + new string('#', numberFormatInfo.NumberGroupSizes[i]) + "," + text2;
						}
					}
				}
				if (NumDigitsAfterDecimal > 0)
				{
					text2 = text2 + "." + new string('0', NumDigitsAfterDecimal);
				}
				return Strings.Replace(text, "n", text2, 1, -1, CompareMethod.Binary);
			}
		}

		public static string FormatPercent(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
		{
			Strings.ValidateTriState(IncludeLeadingDigit);
			Strings.ValidateTriState(UseParensForNegativeNumbers);
			Strings.ValidateTriState(GroupDigits);
			if (Expression == null)
			{
				return "";
			}
			Type type = Expression.GetType();
			if (type == typeof(string))
			{
				Expression = Conversions.ToDouble(Expression);
			}
			else if (!Symbols.IsNumericType(type))
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(type),
					"numeric"
				}));
			}
			IFormattable formattable = (IFormattable)Expression;
			string formatString = Strings.GetFormatString(NumDigitsAfterDecimal, IncludeLeadingDigit, UseParensForNegativeNumbers, GroupDigits, Strings.FormatType.Percent);
			return formattable.ToString(formatString, null);
		}

		public static char GetChar(string str, int Index)
		{
			if (str == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_LengthGTZero1", new string[] { "String" }));
			}
			if (Index < 1)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEOne1", new string[] { "Index" }));
			}
			if (Index > str.Length)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_IndexLELength2", new string[] { "Index", "String" }));
			}
			return str[checked(Index - 1)];
		}

		public static string Left(string str, int Length)
		{
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Length" }));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			if (Length >= str.Length)
			{
				return str;
			}
			return str.Substring(0, Length);
		}

		public static string LTrim(string str)
		{
			if (str == null || str.Length == 0)
			{
				return "";
			}
			char c = str[0];
			if (c == ' ' || c == '\u3000')
			{
				return str.TrimStart(Utils.m_achIntlSpace);
			}
			return str;
		}

		public static string Mid(string str, int Start)
		{
			string text;
			try
			{
				if (str == null)
				{
					text = null;
				}
				else
				{
					text = Strings.Mid(str, Start, str.Length);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static string Mid(string str, int Start, int Length)
		{
			if (Start <= 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GTZero1", new string[] { "Start" }));
			}
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Length" }));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			int length = str.Length;
			if (Start > length)
			{
				return "";
			}
			checked
			{
				if (Start + Length > length)
				{
					return str.Substring(Start - 1);
				}
				return str.Substring(Start - 1, Length);
			}
		}

		public static string Right(string str, int Length)
		{
			if (Length < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_GEZero1", new string[] { "Length" }));
			}
			if (Length == 0 || str == null)
			{
				return "";
			}
			int length = str.Length;
			if (Length >= length)
			{
				return str;
			}
			return str.Substring(checked(length - Length), Length);
		}

		public static string RTrim(string str)
		{
			string text;
			try
			{
				if (str == null || str.Length == 0)
				{
					text = "";
				}
				else
				{
					char c = str[checked(str.Length - 1)];
					if (c == ' ' || c == '\u3000')
					{
						text = str.TrimEnd(Utils.m_achIntlSpace);
					}
					else
					{
						text = str;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static string Trim(string str)
		{
			string text;
			try
			{
				if (str == null || str.Length == 0)
				{
					text = "";
				}
				else
				{
					char c = str[0];
					if (c == ' ' || c == '\u3000')
					{
						text = str.Trim(Utils.m_achIntlSpace);
					}
					else
					{
						c = str[checked(str.Length - 1)];
						if (c == ' ' || c == '\u3000')
						{
							text = str.Trim(Utils.m_achIntlSpace);
						}
						else
						{
							text = str;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return text;
		}

		public static int StrComp(string String1, string String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
		{
			int num;
			try
			{
				if (Compare == CompareMethod.Binary)
				{
					num = Operators.CompareString(String1, String2, false);
				}
				else
				{
					if (Compare != CompareMethod.Text)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Compare" }));
					}
					num = Operators.CompareString(String1, String2, true);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return num;
		}

		internal static bool IsValidCodePage(int codepage)
		{
			bool flag = false;
			try
			{
				if (Encoding.GetEncoding(codepage) != null)
				{
					flag = true;
				}
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
			}
			return flag;
		}

		public static string StrConv(string str, VbStrConv Conversion, int LocaleID = 0)
		{
			string text;
			try
			{
				CultureInfo cultureInfo;
				if (LocaleID == 0 || LocaleID == 1)
				{
					cultureInfo = Utils.GetCultureInfo();
					LocaleID = cultureInfo.LCID;
				}
				else
				{
					try
					{
						cultureInfo = new CultureInfo(LocaleID & 65535);
					}
					catch (StackOverflowException ex)
					{
						throw ex;
					}
					catch (OutOfMemoryException ex2)
					{
						throw ex2;
					}
					catch (ThreadAbortException ex3)
					{
						throw ex3;
					}
					catch (Exception)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_LCIDNotSupported1", new string[] { Conversions.ToString(LocaleID) }));
					}
				}
				int num = Strings.PRIMARYLANGID(LocaleID);
				if ((Conversion & ~(VbStrConv.Uppercase | VbStrConv.Lowercase | VbStrConv.Wide | VbStrConv.Narrow | VbStrConv.Katakana | VbStrConv.Hiragana | VbStrConv.SimplifiedChinese | VbStrConv.TraditionalChinese | VbStrConv.LinguisticCasing)) != VbStrConv.None)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidVbStrConv"));
				}
				int num2 = (int)(Conversion & (VbStrConv.SimplifiedChinese | VbStrConv.TraditionalChinese));
				int num3;
				if (num2 != 0)
				{
					if (num2 == 768)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_StrConvSCandTC"));
					}
					if (num2 == 256)
					{
						if (!Strings.IsValidCodePage(936) || !Strings.IsValidCodePage(950))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_SCNotSupported"));
						}
						num3 |= 33554432;
					}
					else if (num2 == 512)
					{
						if (!Strings.IsValidCodePage(936) || !Strings.IsValidCodePage(950))
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_TCNotSupported"));
						}
						num3 |= 67108864;
					}
				}
				switch (Conversion & VbStrConv.ProperCase)
				{
				case VbStrConv.None:
					if ((Conversion & VbStrConv.LinguisticCasing) != VbStrConv.None)
					{
						throw new ArgumentException(Utils.GetResourceString("LinguisticRequirements"));
					}
					break;
				case VbStrConv.Uppercase:
					if (Conversion == VbStrConv.Uppercase)
					{
						return cultureInfo.TextInfo.ToUpper(str);
					}
					num3 |= 512;
					break;
				case VbStrConv.Lowercase:
					if (Conversion == VbStrConv.Lowercase)
					{
						return cultureInfo.TextInfo.ToLower(str);
					}
					num3 |= 256;
					break;
				case VbStrConv.ProperCase:
					num3 = 0;
					break;
				}
				if ((Conversion & (VbStrConv.Katakana | VbStrConv.Hiragana)) != VbStrConv.None && (num != 17 || !Strings.ValidLCID(LocaleID)))
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_JPNNotSupported"));
				}
				if ((Conversion & (VbStrConv.Wide | VbStrConv.Narrow)) != VbStrConv.None)
				{
					if (num != 17 && num != 18 && num != 4)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_WideNarrowNotApplicable"));
					}
					if (!Strings.ValidLCID(LocaleID))
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_LocalNotSupported"));
					}
				}
				switch (Conversion & (VbStrConv.Wide | VbStrConv.Narrow))
				{
				case VbStrConv.Wide:
					num3 |= 8388608;
					break;
				case VbStrConv.Narrow:
					num3 |= 4194304;
					break;
				case VbStrConv.Wide | VbStrConv.Narrow:
					throw new ArgumentException(Utils.GetResourceString("Argument_IllegalWideNarrow"));
				}
				VbStrConv vbStrConv = Conversion & (VbStrConv.Katakana | VbStrConv.Hiragana);
				if (vbStrConv != VbStrConv.None)
				{
					if (vbStrConv == (VbStrConv.Katakana | VbStrConv.Hiragana))
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_IllegalKataHira"));
					}
					if (vbStrConv == VbStrConv.Katakana)
					{
						num3 |= 2097152;
					}
					else if (vbStrConv == VbStrConv.Hiragana)
					{
						num3 |= 1048576;
					}
				}
				if ((Conversion & VbStrConv.ProperCase) == VbStrConv.ProperCase)
				{
					text = Strings.ProperCaseString(cultureInfo, num3, str);
				}
				else if (num3 != 0)
				{
					text = Strings.vbLCMapString(cultureInfo, num3, str);
				}
				else
				{
					text = str;
				}
			}
			catch (Exception ex4)
			{
				throw ex4;
			}
			return text;
		}

		internal static bool ValidLCID(int LocaleID)
		{
			bool flag;
			try
			{
				CultureInfo cultureInfo = new CultureInfo(LocaleID);
				flag = true;
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		private static string ProperCaseString(CultureInfo loc, int dwMapFlags, string sSrc)
		{
			int num;
			if (sSrc == null)
			{
				num = 0;
			}
			else
			{
				num = sSrc.Length;
			}
			if (num == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(Strings.vbLCMapString(loc, dwMapFlags | 256, sSrc));
			return loc.TextInfo.ToTitleCase(stringBuilder.ToString());
		}

		internal static string vbLCMapString(CultureInfo loc, int dwMapFlags, string sSrc)
		{
			int num;
			if (sSrc == null)
			{
				num = 0;
			}
			else
			{
				num = sSrc.Length;
			}
			if (num == 0)
			{
				return "";
			}
			int lcid = loc.LCID;
			Encoding encoding = Encoding.GetEncoding(loc.TextInfo.ANSICodePage);
			int num2;
			string text2;
			if (!encoding.IsSingleByte)
			{
				string text = sSrc;
				byte[] bytes = encoding.GetBytes(text);
				num2 = UnsafeNativeMethods.LCMapStringA(lcid, dwMapFlags, bytes, bytes.Length, null, 0);
				byte[] array = new byte[checked(num2 - 1 + 1)];
				num2 = UnsafeNativeMethods.LCMapStringA(lcid, dwMapFlags, bytes, bytes.Length, array, num2);
				text2 = encoding.GetString(array);
				return text2;
			}
			text2 = new string(' ', num);
			num2 = UnsafeNativeMethods.LCMapString(lcid, dwMapFlags, ref sSrc, num, ref text2, num);
			return text2;
		}

		private static void ValidateTriState(TriState Param)
		{
			if (Param != TriState.True && Param != TriState.False && Param != TriState.UseDefault)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
		}

		private static bool IsArrayEmpty(Array array)
		{
			return array == null || array.Length == 0;
		}

		private static readonly string[] CurrencyPositiveFormatStrings = new string[] { "'$'n", "n'$'", "'$' n", "n '$'" };

		private static readonly string[] CurrencyNegativeFormatStrings = new string[]
		{
			"('$'n)", "-'$'n", "'$'-n", "'$'n-", "(n'$')", "-n'$'", "n-'$'", "n'$'-", "-n '$'", "-'$' n",
			"n '$'-", "'$' n-", "'$'- n", "n- '$'", "('$' n)", "(n '$')"
		};

		private static readonly string[] NumberNegativeFormatStrings = new string[] { "(n)", "-n", "- n", "n-", "n -" };

		private const int CODEPAGE_SIMPLIFIED_CHINESE = 936;

		private const int CODEPAGE_TRADITIONAL_CHINESE = 950;

		private const CompareOptions STANDARD_COMPARE_FLAGS = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		private const int InvariantCultureID = 127;

		private const string NAMEDFORMAT_FIXED = "fixed";

		private const string NAMEDFORMAT_YES_NO = "yes/no";

		private const string NAMEDFORMAT_ON_OFF = "on/off";

		private const string NAMEDFORMAT_PERCENT = "percent";

		private const string NAMEDFORMAT_STANDARD = "standard";

		private const string NAMEDFORMAT_CURRENCY = "currency";

		private const string NAMEDFORMAT_LONG_TIME = "long time";

		private const string NAMEDFORMAT_LONG_DATE = "long date";

		private const string NAMEDFORMAT_SCIENTIFIC = "scientific";

		private const string NAMEDFORMAT_TRUE_FALSE = "true/false";

		private const string NAMEDFORMAT_SHORT_TIME = "short time";

		private const string NAMEDFORMAT_SHORT_DATE = "short date";

		private const string NAMEDFORMAT_MEDIUM_DATE = "medium date";

		private const string NAMEDFORMAT_MEDIUM_TIME = "medium time";

		private const string NAMEDFORMAT_GENERAL_DATE = "general date";

		private const string NAMEDFORMAT_GENERAL_NUMBER = "general number";

		internal static readonly CompareInfo m_InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

		private static object m_SyncObject = new object();

		private static CultureInfo m_LastUsedYesNoCulture;

		private static string m_CachedYesNoFormatStyle;

		private static CultureInfo m_LastUsedOnOffCulture;

		private static string m_CachedOnOffFormatStyle;

		private static CultureInfo m_LastUsedTrueFalseCulture;

		private static string m_CachedTrueFalseFormatStyle;

		private enum NamedFormats
		{
			UNKNOWN,
			GENERAL_NUMBER,
			LONG_TIME,
			MEDIUM_TIME,
			SHORT_TIME,
			GENERAL_DATE,
			LONG_DATE,
			MEDIUM_DATE,
			SHORT_DATE,
			FIXED,
			STANDARD,
			PERCENT,
			SCIENTIFIC,
			CURRENCY,
			TRUE_FALSE,
			YES_NO,
			ON_OFF
		}

		internal enum FormatType
		{
			Number,
			Percent,
			Currency
		}
	}
}
