using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Utils
	{
		private Utils()
		{
		}

		internal static ResourceManager VBAResourceManager
		{
			get
			{
				if (Utils.m_VBAResourceManager != null)
				{
					return Utils.m_VBAResourceManager;
				}
				object resourceManagerSyncObj = Utils.ResourceManagerSyncObj;
				ObjectFlowControl.CheckForSyncLockOnValueType(resourceManagerSyncObj);
				lock (resourceManagerSyncObj)
				{
					if (!Utils.m_TriedLoadingResourceManager)
					{
						try
						{
							Utils.m_VBAResourceManager = new ResourceManager("Microsoft.VisualBasic", Assembly.GetExecutingAssembly());
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
						Utils.m_TriedLoadingResourceManager = true;
					}
				}
				return Utils.m_VBAResourceManager;
			}
		}

		internal static string GetResourceString(vbErrors ResourceId)
		{
			return Utils.GetResourceString("ID" + Conversions.ToString((int)ResourceId));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static string GetResourceString(string ResourceKey)
		{
			if (Utils.VBAResourceManager == null)
			{
				return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
			}
			string text;
			try
			{
				text = Utils.VBAResourceManager.GetString(ResourceKey, Utils.GetCultureInfo());
				if (text == null)
				{
					text = Utils.VBAResourceManager.GetString("ID95");
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
				text = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
			}
			return text;
		}

		internal static string GetResourceString(string ResourceKey, bool NotUsed)
		{
			if (Utils.VBAResourceManager == null)
			{
				return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
			}
			string text;
			try
			{
				text = Utils.VBAResourceManager.GetString(ResourceKey, Utils.GetCultureInfo());
				if (text == null)
				{
					text = Utils.VBAResourceManager.GetString(ResourceKey);
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
				text = null;
			}
			return text;
		}

		public static string GetResourceString(string ResourceKey, params string[] Args)
		{
			string text = null;
			string text2 = null;
			try
			{
				text = Utils.GetResourceString(ResourceKey);
				text2 = string.Format(Thread.CurrentThread.CurrentUICulture, text, Args);
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
			catch (Exception ex4)
			{
			}
			finally
			{
			}
			if (Operators.CompareString(text2, "", false) != 0)
			{
				return text2;
			}
			return text;
		}

		internal static string StdFormat(string s)
		{
			NumberFormatInfo numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
			int num = s.IndexOf(numberFormat.NumberDecimalSeparator);
			if (num == -1)
			{
				return s;
			}
			char c;
			char c2;
			char c3;
			try
			{
				c = s[0];
				c2 = s[1];
				c3 = s[2];
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
			if (s[num] == '.')
			{
				if (c == '0' && c2 == '.')
				{
					return s.Substring(1);
				}
				if (c != '-')
				{
					if (c != '+')
					{
						if (c != ' ')
						{
							return s;
						}
					}
				}
				if (c2 == '0' && c3 == '.')
				{
					goto IL_008D;
				}
				return s;
			}
			IL_008D:
			StringBuilder stringBuilder = new StringBuilder(s);
			stringBuilder[num] = '.';
			string text;
			if (c == '0' && c2 == '.')
			{
				text = stringBuilder.ToString(1, checked(stringBuilder.Length - 1));
			}
			else
			{
				if (c != '-')
				{
					if (c != '+')
					{
						if (c != ' ')
						{
							goto IL_00EE;
						}
					}
				}
				if (c2 == '0' && c3 == '.')
				{
					stringBuilder.Remove(1, 1);
					return stringBuilder.ToString();
				}
				IL_00EE:
				text = stringBuilder.ToString();
			}
			return text;
		}

		internal static string OctFromLong(long Val)
		{
			string text = "";
			int num = Convert.ToInt32('0');
			checked
			{
				bool flag;
				if (Val < 0L)
				{
					Val = long.MaxValue + Val + 1L;
					flag = true;
				}
				do
				{
					int num2 = (int)(Val % 8L);
					Val >>= 3;
					text += Conversions.ToString(Strings.ChrW(num2 + num));
				}
				while (Val > 0L);
				text = Strings.StrReverse(text);
				if (flag)
				{
					text = "1" + text;
				}
				return text;
			}
		}

		internal static string OctFromULong(ulong Val)
		{
			string text = "";
			int num = Convert.ToInt32('0');
			checked
			{
				do
				{
					int num2 = (int)(Val % 8UL);
					Val >>= 3;
					text += Conversions.ToString(Strings.ChrW(num2 + num));
				}
				while (Val != 0UL);
				return Strings.StrReverse(text);
			}
		}

		[DebuggerHidden]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static void SetTime(DateTime dtTime)
		{
			NativeTypes.SystemTime systemTime = new NativeTypes.SystemTime();
			SafeNativeMethods.GetLocalTime(systemTime);
			checked
			{
				systemTime.wHour = (short)dtTime.Hour;
				systemTime.wMinute = (short)dtTime.Minute;
				systemTime.wSecond = (short)dtTime.Second;
				systemTime.wMilliseconds = (short)dtTime.Millisecond;
				if (UnsafeNativeMethods.SetLocalTime(systemTime) != 0)
				{
					return;
				}
				if (Marshal.GetLastWin32Error() == 87)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				throw new SecurityException(Utils.GetResourceString("SetLocalTimeFailure"));
			}
		}

		[DebuggerHidden]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static void SetDate(DateTime vDate)
		{
			NativeTypes.SystemTime systemTime = new NativeTypes.SystemTime();
			SafeNativeMethods.GetLocalTime(systemTime);
			checked
			{
				systemTime.wYear = (short)vDate.Year;
				systemTime.wMonth = (short)vDate.Month;
				systemTime.wDay = (short)vDate.Day;
				if (UnsafeNativeMethods.SetLocalTime(systemTime) != 0)
				{
					return;
				}
				if (Marshal.GetLastWin32Error() == 87)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
				}
				throw new SecurityException(Utils.GetResourceString("SetLocalDateFailure"));
			}
		}

		internal static DateTimeFormatInfo GetDateTimeFormatInfo()
		{
			return Thread.CurrentThread.CurrentCulture.DateTimeFormat;
		}

		public static void ThrowException(int hr)
		{
			throw ExceptionUtils.VbMakeException(hr);
		}

		internal static int MapHRESULT(int lNumber)
		{
			if (lNumber > 0)
			{
				return lNumber;
			}
			if ((lNumber & 536805376) == 655360)
			{
				return lNumber & 65535;
			}
			int num;
			if (lNumber == -2147467263)
			{
				num = 32768;
			}
			else if (lNumber == -2147467262)
			{
				num = 430;
			}
			else if (lNumber == -2147467260)
			{
				num = 287;
			}
			else if (lNumber == -2147352575)
			{
				num = 438;
			}
			else if (lNumber == -2147352573)
			{
				num = 438;
			}
			else if (lNumber == -2147352572)
			{
				num = 448;
			}
			else if (lNumber == -2147352571)
			{
				num = 13;
			}
			else if (lNumber == -2147352570)
			{
				num = 438;
			}
			else if (lNumber == -2147352569)
			{
				num = 446;
			}
			else if (lNumber == -2147352568)
			{
				num = 458;
			}
			else if (lNumber == -2147352566)
			{
				num = 6;
			}
			else if (lNumber == -2147352565)
			{
				num = 9;
			}
			else if (lNumber == -2147352564)
			{
				num = 447;
			}
			else if (lNumber == -2147352563)
			{
				num = 10;
			}
			else if (lNumber == -2147352562)
			{
				num = 450;
			}
			else if (lNumber == -2147352561)
			{
				num = 449;
			}
			else if (lNumber == -2147352559)
			{
				num = 451;
			}
			else if (lNumber == -2147352558)
			{
				num = 11;
			}
			else if (lNumber == -2147319786)
			{
				num = 32790;
			}
			else if (lNumber == -2147319785)
			{
				num = 461;
			}
			else if (lNumber == -2147319784)
			{
				num = 32792;
			}
			else if (lNumber == -2147319783)
			{
				num = 32793;
			}
			else if (lNumber == -2147319780)
			{
				num = 32796;
			}
			else if (lNumber == -2147319779)
			{
				num = 32797;
			}
			else if (lNumber == -2147319769)
			{
				num = 32807;
			}
			else if (lNumber == -2147319768)
			{
				num = 32808;
			}
			else if (lNumber == -2147319767)
			{
				num = 32809;
			}
			else if (lNumber == -2147319766)
			{
				num = 32810;
			}
			else if (lNumber == -2147319765)
			{
				num = 32811;
			}
			else if (lNumber == -2147319764)
			{
				num = 32812;
			}
			else if (lNumber == -2147319763)
			{
				num = 32813;
			}
			else if (lNumber == -2147319762)
			{
				num = 32814;
			}
			else if (lNumber == -2147319761)
			{
				num = 453;
			}
			else if (lNumber == -2147317571)
			{
				num = 35005;
			}
			else if (lNumber == -2147317563)
			{
				num = 35013;
			}
			else if (lNumber == -2147316576)
			{
				num = 13;
			}
			else if (lNumber == -2147316575)
			{
				num = 9;
			}
			else if (lNumber == -2147316574)
			{
				num = 57;
			}
			else if (lNumber == -2147316573)
			{
				num = 322;
			}
			else if (lNumber == -2147312566)
			{
				num = 48;
			}
			else if (lNumber == -2147312509)
			{
				num = 40067;
			}
			else if (lNumber == -2147312508)
			{
				num = 40068;
			}
			else if (lNumber == -2147287039)
			{
				num = 32774;
			}
			else if (lNumber == -2147287038)
			{
				num = 53;
			}
			else if (lNumber == -2147287037)
			{
				num = 76;
			}
			else if (lNumber == -2147287036)
			{
				num = 67;
			}
			else if (lNumber == -2147287035)
			{
				num = 70;
			}
			else if (lNumber == -2147287034)
			{
				num = 32772;
			}
			else if (lNumber == -2147287032)
			{
				num = 7;
			}
			else if (lNumber == -2147287022)
			{
				num = 67;
			}
			else if (lNumber == -2147287021)
			{
				num = 70;
			}
			else if (lNumber == -2147287015)
			{
				num = 32771;
			}
			else if (lNumber == -2147287011)
			{
				num = 32773;
			}
			else if (lNumber == -2147287010)
			{
				num = 32772;
			}
			else if (lNumber == -2147287008)
			{
				num = 75;
			}
			else if (lNumber == -2147287007)
			{
				num = 70;
			}
			else if (lNumber == -2147286960)
			{
				num = 58;
			}
			else if (lNumber == -2147286928)
			{
				num = 61;
			}
			else if (lNumber == -2147286789)
			{
				num = 32792;
			}
			else if (lNumber == -2147286788)
			{
				num = 53;
			}
			else if (lNumber == -2147286787)
			{
				num = 32792;
			}
			else if (lNumber == -2147286786)
			{
				num = 32768;
			}
			else if (lNumber == -2147286784)
			{
				num = 70;
			}
			else if (lNumber == -2147286783)
			{
				num = 70;
			}
			else if (lNumber == -2147286782)
			{
				num = 32773;
			}
			else if (lNumber == -2147286781)
			{
				num = 57;
			}
			else if (lNumber == -2147286780)
			{
				num = 32793;
			}
			else if (lNumber == -2147286779)
			{
				num = 32793;
			}
			else if (lNumber == -2147286778)
			{
				num = 32789;
			}
			else if (lNumber == -2147286777)
			{
				num = 32793;
			}
			else if (lNumber == -2147286776)
			{
				num = 32793;
			}
			else if (lNumber == -2147221230)
			{
				num = 429;
			}
			else if (lNumber == -2147221164)
			{
				num = 429;
			}
			else if (lNumber == -2147221021)
			{
				num = 429;
			}
			else if (lNumber == -2147221018)
			{
				num = 432;
			}
			else if (lNumber == -2147221014)
			{
				num = 432;
			}
			else if (lNumber == -2147221005)
			{
				num = 429;
			}
			else if (lNumber == -2147221003)
			{
				num = 429;
			}
			else if (lNumber == -2147220994)
			{
				num = 429;
			}
			else if (lNumber == -2147024891)
			{
				num = 70;
			}
			else if (lNumber == -2147024882)
			{
				num = 7;
			}
			else if (lNumber == -2147024809)
			{
				num = 5;
			}
			else if (lNumber == -2147023174)
			{
				num = 462;
			}
			else if (lNumber == -2146959355)
			{
				num = 429;
			}
			else
			{
				num = lNumber;
			}
			return num;
		}

		internal static CultureInfo GetCultureInfo()
		{
			return Thread.CurrentThread.CurrentCulture;
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.SelfAffectingThreading)]
		public static object SetCultureInfo(CultureInfo Culture)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = Culture;
			return currentCulture;
		}

		internal static CultureInfo GetInvariantCultureInfo()
		{
			return CultureInfo.InvariantCulture;
		}

		internal static Encoding GetFileIOEncoding()
		{
			return Encoding.Default;
		}

		internal static int GetLocaleCodePage()
		{
			return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
		}

		internal static Assembly VBRuntimeAssembly
		{
			get
			{
				if (Utils.m_VBRuntimeAssembly != null)
				{
					return Utils.m_VBRuntimeAssembly;
				}
				Utils.m_VBRuntimeAssembly = Assembly.GetExecutingAssembly();
				return Utils.m_VBRuntimeAssembly;
			}
		}

		public static Array CopyArray(Array arySrc, Array aryDest)
		{
			if (arySrc == null)
			{
				return aryDest;
			}
			int num = arySrc.Length;
			if (num == 0)
			{
				return aryDest;
			}
			if (aryDest.Rank != arySrc.Rank)
			{
				throw ExceptionUtils.VbMakeException(new InvalidCastException(Utils.GetResourceString("Array_RankMismatch")), 9);
			}
			int num2 = 0;
			checked
			{
				int num3 = aryDest.Rank - 2;
				for (int i = num2; i <= num3; i++)
				{
					if (aryDest.GetUpperBound(i) != arySrc.GetUpperBound(i))
					{
						throw ExceptionUtils.VbMakeException(new ArrayTypeMismatchException(Utils.GetResourceString("Array_TypeMismatch")), 9);
					}
				}
				if (num > aryDest.Length)
				{
					num = aryDest.Length;
				}
				if (arySrc.Rank > 1)
				{
					int rank = arySrc.Rank;
					int length = arySrc.GetLength(rank - 1);
					int length2 = aryDest.GetLength(rank - 1);
					if (length2 == 0)
					{
						return aryDest;
					}
					int num4 = Math.Min(length, length2);
					int num5 = 0;
					int num6 = arySrc.Length / length - 1;
					for (int j = num5; j <= num6; j++)
					{
						Array.Copy(arySrc, j * length, aryDest, j * length2, num4);
					}
				}
				else
				{
					Array.Copy(arySrc, aryDest, num);
				}
				return aryDest;
			}
		}

		internal static string ToHalfwidthNumbers(string s, CultureInfo culture)
		{
			int lcid = culture.LCID;
			int num = lcid & 1023;
			if (num != 4 && num != 17 && num != 18)
			{
				return s;
			}
			return Strings.vbLCMapString(culture, 4194304, s);
		}

		internal static bool IsHexOrOctValue(string Value, ref long i64Value)
		{
			int length = Value.Length;
			checked
			{
				int i;
				while (i < length)
				{
					char c = Value[i];
					if (c == '&' && i + 2 < length)
					{
						c = char.ToLower(Value[i + 1], CultureInfo.InvariantCulture);
						string text = Utils.ToHalfwidthNumbers(Value.Substring(i + 2), Utils.GetCultureInfo());
						if (c == 'h')
						{
							i64Value = Convert.ToInt64(text, 16);
						}
						else
						{
							if (c != 'o')
							{
								throw new FormatException();
							}
							i64Value = Convert.ToInt64(text, 8);
						}
						return true;
					}
					if (c != ' ' && c != '\u3000')
					{
						return false;
					}
					i++;
				}
				return false;
			}
		}

		internal static bool IsHexOrOctValue(string Value, ref ulong ui64Value)
		{
			int length = Value.Length;
			checked
			{
				int i;
				while (i < length)
				{
					char c = Value[i];
					if (c == '&' && i + 2 < length)
					{
						c = char.ToLower(Value[i + 1], CultureInfo.InvariantCulture);
						string text = Utils.ToHalfwidthNumbers(Value.Substring(i + 2), Utils.GetCultureInfo());
						if (c == 'h')
						{
							ui64Value = Convert.ToUInt64(text, 16);
						}
						else
						{
							if (c != 'o')
							{
								throw new FormatException();
							}
							ui64Value = Convert.ToUInt64(text, 8);
						}
						return true;
					}
					if (c != ' ' && c != '\u3000')
					{
						return false;
					}
					i++;
				}
				return false;
			}
		}

		internal static string VBFriendlyName(object Obj)
		{
			if (Obj == null)
			{
				return "Nothing";
			}
			return Utils.VBFriendlyName(Obj.GetType(), Obj);
		}

		internal static string VBFriendlyName(Type typ)
		{
			return Utils.VBFriendlyNameOfType(typ, false);
		}

		internal static string VBFriendlyName(Type typ, object o)
		{
			if (typ.IsCOMObject && Operators.CompareString(typ.FullName, "System.__ComObject", false) == 0)
			{
				return Information.TypeNameOfCOMObject(o, false);
			}
			return Utils.VBFriendlyNameOfType(typ, false);
		}

		internal static string VBFriendlyNameOfType(Type typ, bool FullName = false)
		{
			string arraySuffixAndElementType = Utils.GetArraySuffixAndElementType(ref typ);
			TypeCode typeCode;
			if (typ.IsEnum)
			{
				typeCode = TypeCode.Object;
			}
			else
			{
				typeCode = Type.GetTypeCode(typ);
			}
			string text;
			switch (typeCode)
			{
			case TypeCode.DBNull:
				text = "DBNull";
				goto IL_01B1;
			case TypeCode.Boolean:
				text = "Boolean";
				goto IL_01B1;
			case TypeCode.Char:
				text = "Char";
				goto IL_01B1;
			case TypeCode.SByte:
				text = "SByte";
				goto IL_01B1;
			case TypeCode.Byte:
				text = "Byte";
				goto IL_01B1;
			case TypeCode.Int16:
				text = "Short";
				goto IL_01B1;
			case TypeCode.UInt16:
				text = "UShort";
				goto IL_01B1;
			case TypeCode.Int32:
				text = "Integer";
				goto IL_01B1;
			case TypeCode.UInt32:
				text = "UInteger";
				goto IL_01B1;
			case TypeCode.Int64:
				text = "Long";
				goto IL_01B1;
			case TypeCode.UInt64:
				text = "ULong";
				goto IL_01B1;
			case TypeCode.Single:
				text = "Single";
				goto IL_01B1;
			case TypeCode.Double:
				text = "Double";
				goto IL_01B1;
			case TypeCode.Decimal:
				text = "Decimal";
				goto IL_01B1;
			case TypeCode.DateTime:
				text = "Date";
				goto IL_01B1;
			case TypeCode.String:
				text = "String";
				goto IL_01B1;
			}
			if (Symbols.IsGenericParameter(typ))
			{
				text = typ.Name;
			}
			else
			{
				string text2 = null;
				string genericArgsSuffix = Utils.GetGenericArgsSuffix(typ);
				string text3;
				if (FullName)
				{
					if (typ.IsNested)
					{
						text2 = Utils.VBFriendlyNameOfType(typ.DeclaringType, true);
						text3 = typ.Name;
					}
					else
					{
						text3 = typ.FullName;
					}
				}
				else
				{
					text3 = typ.Name;
				}
				if (genericArgsSuffix != null)
				{
					int num = text3.LastIndexOf('`');
					if (num != -1)
					{
						text3 = text3.Substring(0, num);
					}
					text = text3 + genericArgsSuffix;
				}
				else
				{
					text = text3;
				}
				if (text2 != null)
				{
					text = text2 + "." + text;
				}
			}
			IL_01B1:
			if (arraySuffixAndElementType != null)
			{
				text += arraySuffixAndElementType;
			}
			return text;
		}

		private static string GetArraySuffixAndElementType(ref Type typ)
		{
			if (!typ.IsArray)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				stringBuilder.Append("(");
				stringBuilder.Append(',', checked(typ.GetArrayRank() - 1));
				stringBuilder.Append(")");
				typ = typ.GetElementType();
			}
			while (typ.IsArray);
			return stringBuilder.ToString();
		}

		private static string GetGenericArgsSuffix(Type typ)
		{
			if (!typ.IsGenericType)
			{
				return null;
			}
			Type[] genericArguments = typ.GetGenericArguments();
			int num = genericArguments.Length;
			int num2 = num;
			checked
			{
				if (typ.IsNested && typ.DeclaringType.IsGenericType)
				{
					num2 -= typ.DeclaringType.GetGenericArguments().Length;
				}
				if (num2 == 0)
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(Of ");
				int num3 = num - num2;
				int num4 = num - 1;
				for (int i = num3; i <= num4; i++)
				{
					stringBuilder.Append(Utils.VBFriendlyNameOfType(genericArguments[i], false));
					if (i != num - 1)
					{
						stringBuilder.Append(',');
					}
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}
		}

		internal static string ParameterToString(ParameterInfo Parameter)
		{
			string text = "";
			Type type = Parameter.ParameterType;
			if (Parameter.IsOptional)
			{
				text += "[";
			}
			if (type.IsByRef)
			{
				text += "ByRef ";
				type = type.GetElementType();
			}
			else if (Symbols.IsParamArray(Parameter))
			{
				text += "ParamArray ";
			}
			text = text + Parameter.Name + " As " + Utils.VBFriendlyNameOfType(type, true);
			if (Parameter.IsOptional)
			{
				object defaultValue = Parameter.DefaultValue;
				if (defaultValue == null)
				{
					text += " = Nothing";
				}
				else
				{
					Type type2 = defaultValue.GetType();
					if (type2 != Utils.VoidType)
					{
						if (Symbols.IsEnum(type2))
						{
							text = text + " = " + Enum.GetName(type2, defaultValue);
						}
						else
						{
							text = text + " = " + Conversions.ToString(defaultValue);
						}
					}
				}
				text += "]";
			}
			return text;
		}

		public static string MethodToString(MethodBase Method)
		{
			Type type = null;
			string text = "";
			if (Method.MemberType == MemberTypes.Method)
			{
				type = ((MethodInfo)Method).ReturnType;
			}
			if (Method.IsPublic)
			{
				text += "Public ";
			}
			else if (Method.IsPrivate)
			{
				text += "Private ";
			}
			else if (Method.IsAssembly)
			{
				text += "Friend ";
			}
			if ((Method.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
			{
				if (!Method.DeclaringType.IsInterface)
				{
					text += "Overrides ";
				}
			}
			else if (Symbols.IsShared(Method))
			{
				text += "Shared ";
			}
			Symbols.UserDefinedOperator userDefinedOperator = Symbols.UserDefinedOperator.UNDEF;
			if (Symbols.IsUserDefinedOperator(Method))
			{
				userDefinedOperator = Symbols.MapToUserDefinedOperator(Method);
			}
			if (userDefinedOperator != Symbols.UserDefinedOperator.UNDEF)
			{
				if (userDefinedOperator == Symbols.UserDefinedOperator.Narrow)
				{
					text += "Narrowing ";
				}
				else if (userDefinedOperator == Symbols.UserDefinedOperator.Widen)
				{
					text += "Widening ";
				}
				text += "Operator ";
			}
			else if (type == null || type == Utils.VoidType)
			{
				text += "Sub ";
			}
			else
			{
				text += "Function ";
			}
			if (userDefinedOperator != Symbols.UserDefinedOperator.UNDEF)
			{
				text += Symbols.OperatorNames[(int)userDefinedOperator];
			}
			else if (Method.MemberType == MemberTypes.Constructor)
			{
				text += "New";
			}
			else
			{
				text += Method.Name;
			}
			bool flag;
			if (Symbols.IsGeneric(Method))
			{
				text += "(Of ";
				flag = true;
				foreach (Type type2 in Symbols.GetTypeParameters(Method))
				{
					if (!flag)
					{
						text += ", ";
					}
					else
					{
						flag = false;
					}
					text += Utils.VBFriendlyNameOfType(type2, false);
				}
				text += ")";
			}
			text += "(";
			flag = true;
			foreach (ParameterInfo parameterInfo in Method.GetParameters())
			{
				if (!flag)
				{
					text += ", ";
				}
				else
				{
					flag = false;
				}
				text += Utils.ParameterToString(parameterInfo);
			}
			text += ")";
			if (type != null)
			{
				if (type != Utils.VoidType)
				{
					text = text + " As " + Utils.VBFriendlyNameOfType(type, true);
				}
			}
			return text;
		}

		internal static string PropertyToString(PropertyInfo Prop)
		{
			string text = "";
			MethodInfo methodInfo = Prop.GetGetMethod();
			checked
			{
				Utils.PropertyKind propertyKind;
				ParameterInfo[] array;
				Type type;
				if (methodInfo != null)
				{
					if (Prop.GetSetMethod() != null)
					{
						propertyKind = Utils.PropertyKind.ReadWrite;
					}
					else
					{
						propertyKind = Utils.PropertyKind.ReadOnly;
					}
					array = methodInfo.GetParameters();
					type = methodInfo.ReturnType;
				}
				else
				{
					propertyKind = Utils.PropertyKind.WriteOnly;
					methodInfo = Prop.GetSetMethod();
					ParameterInfo[] parameters = methodInfo.GetParameters();
					array = new ParameterInfo[parameters.Length - 2 + 1];
					Array.Copy(parameters, array, array.Length);
					type = parameters[parameters.Length - 1].ParameterType;
				}
				text += "Public ";
				if ((methodInfo.Attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope)
				{
					if (!Prop.DeclaringType.IsInterface)
					{
						text += "Overrides ";
					}
				}
				else if (Symbols.IsShared(methodInfo))
				{
					text += "Shared ";
				}
				if (propertyKind == Utils.PropertyKind.ReadOnly)
				{
					text += "ReadOnly ";
				}
				if (propertyKind == Utils.PropertyKind.WriteOnly)
				{
					text += "WriteOnly ";
				}
				text = text + "Property " + Prop.Name + "(";
				bool flag = true;
				foreach (ParameterInfo parameterInfo in array)
				{
					if (!flag)
					{
						text += ", ";
					}
					else
					{
						flag = false;
					}
					text += Utils.ParameterToString(parameterInfo);
				}
				return text + ") As " + Utils.VBFriendlyNameOfType(type, true);
			}
		}

		internal static string AdjustArraySuffix(string sRank)
		{
			string text = null;
			int i = sRank.Length;
			checked
			{
				while (i > 0)
				{
					char c = sRank[i - 1];
					switch (c)
					{
					case '(':
						text += ")";
						break;
					case ')':
						text += "(";
						break;
					case '*':
					case '+':
						goto IL_005F;
					case ',':
						text += Conversions.ToString(c);
						break;
					default:
						goto IL_005F;
					}
					IL_006C:
					i--;
					continue;
					IL_005F:
					text = Conversions.ToString(c) + text;
					goto IL_006C;
				}
				return text;
			}
		}

		internal static string MemberToString(MemberInfo Member)
		{
			switch (Member.MemberType)
			{
			case MemberTypes.Constructor:
			case MemberTypes.Method:
				return Utils.MethodToString((MethodBase)Member);
			case MemberTypes.Field:
				return Utils.FieldToString((FieldInfo)Member);
			case MemberTypes.Property:
				return Utils.PropertyToString((PropertyInfo)Member);
			}
			return Member.Name;
		}

		internal static string FieldToString(FieldInfo Field)
		{
			string text = "";
			Type fieldType = Field.FieldType;
			if (Field.IsPublic)
			{
				text += "Public ";
			}
			else if (Field.IsPrivate)
			{
				text += "Private ";
			}
			else if (Field.IsAssembly)
			{
				text += "Friend ";
			}
			else if (Field.IsFamily)
			{
				text += "Protected ";
			}
			else if (Field.IsFamilyOrAssembly)
			{
				text += "Protected Friend ";
			}
			text += Field.Name;
			text += " As ";
			return text + Utils.VBFriendlyNameOfType(fieldType, true);
		}

		internal const int SEVERITY_ERROR = -2147483648;

		internal const int FACILITY_CONTROL = 655360;

		internal const int FACILITY_RPC = 65536;

		internal const int FACILITY_ITF = 262144;

		internal const int SCODE_FACILITY = 536805376;

		private const int ERROR_INVALID_PARAMETER = 87;

		internal const char chPeriod = '.';

		internal const char chSpace = ' ';

		internal const char chIntlSpace = '\u3000';

		internal const char chZero = '0';

		internal const char chHyphen = '-';

		internal const char chPlus = '+';

		internal const char chLetterA = 'A';

		internal const char chLetterZ = 'Z';

		internal const char chColon = ':';

		internal const char chSlash = '/';

		internal const char chBackslash = '\\';

		internal const char chTab = '\t';

		internal const char chCharH0A = '\n';

		internal const char chCharH0B = '\v';

		internal const char chCharH0C = '\f';

		internal const char chCharH0D = '\r';

		internal const char chLineFeed = '\n';

		internal const char chDblQuote = '"';

		internal const char chGenericManglingChar = '`';

		internal const CompareOptions OptionCompareTextFlags = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		private static ResourceManager m_VBAResourceManager;

		private static bool m_TriedLoadingResourceManager;

		private const string ResourceMsgDefault = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";

		private const string VBDefaultErrorID = "ID95";

		internal static char[] m_achIntlSpace = new char[] { ' ', '\u3000' };

		private static readonly Type VoidType = Type.GetType("System.Void");

		private static readonly object ResourceManagerSyncObj = new object();

		private static Assembly m_VBRuntimeAssembly;

		private enum PropertyKind
		{
			ReadWrite,
			ReadOnly,
			WriteOnly
		}
	}
}
