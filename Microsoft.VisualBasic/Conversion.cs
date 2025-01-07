using System;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Conversion
	{
		public static string ErrorToString()
		{
			return Information.Err().Description;
		}

		public static string ErrorToString(int ErrorNumber)
		{
			if (ErrorNumber >= 65535)
			{
				throw new ArgumentException(Utils.GetResourceString("MaxErrNumber"));
			}
			if (ErrorNumber > 0)
			{
				ErrorNumber = -2146828288 | ErrorNumber;
			}
			if ((ErrorNumber & 536805376) == 655360)
			{
				ErrorNumber &= 65535;
				return Utils.GetResourceString((vbErrors)ErrorNumber);
			}
			if (ErrorNumber != 0)
			{
				return Utils.GetResourceString(vbErrors.UserDefined);
			}
			return "";
		}

		public static short Fix(short Number)
		{
			return Number;
		}

		public static int Fix(int Number)
		{
			return Number;
		}

		public static long Fix(long Number)
		{
			return Number;
		}

		public static double Fix(double Number)
		{
			if (Number >= 0.0)
			{
				return Math.Floor(Number);
			}
			return -Math.Floor(-Number);
		}

		public static float Fix(float Number)
		{
			if (Number >= 0f)
			{
				return (float)Math.Floor((double)Number);
			}
			return (float)(-(float)Math.Floor((double)(-(double)Number)));
		}

		public static decimal Fix(decimal Number)
		{
			if (Number < 0m)
			{
				return decimal.Negate(decimal.Floor(decimal.Negate(Number)));
			}
			return decimal.Floor(Number);
		}

		public static object Fix(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return convertible.ToInt32(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return Number;
				case TypeCode.Single:
					return Conversion.Fix(convertible.ToSingle(null));
				case TypeCode.Double:
					return Conversion.Fix(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return Conversion.Fix(convertible.ToDecimal(null));
				case TypeCode.String:
					return Conversion.Fix(Conversions.ToDouble(convertible.ToString(null)));
				}
			}
			throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", new string[]
			{
				"Number",
				Number.GetType().FullName
			})), 13);
		}

		public static short Int(short Number)
		{
			return Number;
		}

		public static int Int(int Number)
		{
			return Number;
		}

		public static long Int(long Number)
		{
			return Number;
		}

		public static double Int(double Number)
		{
			return Math.Floor(Number);
		}

		public static float Int(float Number)
		{
			return (float)Math.Floor((double)Number);
		}

		public static decimal Int(decimal Number)
		{
			return decimal.Floor(Number);
		}

		public static object Int(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return convertible.ToInt32(null);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return Number;
				case TypeCode.Single:
					return Conversion.Int(convertible.ToSingle(null));
				case TypeCode.Double:
					return Conversion.Int(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return Conversion.Int(convertible.ToDecimal(null));
				case TypeCode.String:
					return Conversion.Int(Conversions.ToDouble(convertible.ToString(null)));
				}
			}
			throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", new string[]
			{
				"Number",
				Number.GetType().FullName
			})), 13);
		}

		[CLSCompliant(false)]
		public static string Hex(sbyte Number)
		{
			return Number.ToString("X");
		}

		public static string Hex(byte Number)
		{
			return Number.ToString("X");
		}

		public static string Hex(short Number)
		{
			return Number.ToString("X");
		}

		[CLSCompliant(false)]
		public static string Hex(ushort Number)
		{
			return Number.ToString("X");
		}

		public static string Hex(int Number)
		{
			return Number.ToString("X");
		}

		[CLSCompliant(false)]
		public static string Hex(uint Number)
		{
			return Number.ToString("X");
		}

		public static string Hex(long Number)
		{
			return Number.ToString("X");
		}

		[CLSCompliant(false)]
		public static string Hex(ulong Number)
		{
			return Number.ToString("X");
		}

		public static string Hex(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				long num;
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.DateTime:
				case (TypeCode)17:
					goto IL_013D;
				case TypeCode.SByte:
					return Conversion.Hex(convertible.ToSByte(null));
				case TypeCode.Byte:
					return Conversion.Hex(convertible.ToByte(null));
				case TypeCode.Int16:
					return Conversion.Hex(convertible.ToInt16(null));
				case TypeCode.UInt16:
					return Conversion.Hex(convertible.ToUInt16(null));
				case TypeCode.Int32:
					return Conversion.Hex(convertible.ToInt32(null));
				case TypeCode.UInt32:
					return Conversion.Hex(convertible.ToUInt32(null));
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					num = convertible.ToInt64(null);
					break;
				case TypeCode.UInt64:
					return Conversion.Hex(convertible.ToUInt64(null));
				case TypeCode.String:
					try
					{
						num = Conversions.ToLong(convertible.ToString(null));
					}
					catch (OverflowException ex)
					{
						return Conversion.Hex(Conversions.ToULong(convertible.ToString(null)));
					}
					break;
				default:
					goto IL_013D;
				}
				if (num == 0L)
				{
					return "0";
				}
				if (num > 0L)
				{
					return Conversion.Hex(num);
				}
				if (num >= -2147483648L)
				{
					return Conversion.Hex(checked((int)num));
				}
				return Conversion.Hex(num);
			}
			IL_013D:
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[]
			{
				"Number",
				Utils.VBFriendlyName(Number)
			}));
		}

		[CLSCompliant(false)]
		public static string Oct(sbyte Number)
		{
			return Utils.OctFromLong((long)Number & 255L);
		}

		public static string Oct(byte Number)
		{
			return Utils.OctFromULong((ulong)Number);
		}

		public static string Oct(short Number)
		{
			return Utils.OctFromLong((long)Number & 65535L);
		}

		[CLSCompliant(false)]
		public static string Oct(ushort Number)
		{
			return Utils.OctFromULong((ulong)Number);
		}

		public static string Oct(int Number)
		{
			return Utils.OctFromLong((long)Number & (long)((ulong)(-1)));
		}

		[CLSCompliant(false)]
		public static string Oct(uint Number)
		{
			return Utils.OctFromULong((ulong)Number);
		}

		public static string Oct(long Number)
		{
			return Utils.OctFromLong(Number);
		}

		[CLSCompliant(false)]
		public static string Oct(ulong Number)
		{
			return Utils.OctFromULong(Number);
		}

		public static string Oct(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible != null)
			{
				long num;
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.DateTime:
				case (TypeCode)17:
					goto IL_013D;
				case TypeCode.SByte:
					return Conversion.Oct(convertible.ToSByte(null));
				case TypeCode.Byte:
					return Conversion.Oct(convertible.ToByte(null));
				case TypeCode.Int16:
					return Conversion.Oct(convertible.ToInt16(null));
				case TypeCode.UInt16:
					return Conversion.Oct(convertible.ToUInt16(null));
				case TypeCode.Int32:
					return Conversion.Oct(convertible.ToInt32(null));
				case TypeCode.UInt32:
					return Conversion.Oct(convertible.ToUInt32(null));
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					num = convertible.ToInt64(null);
					break;
				case TypeCode.UInt64:
					return Conversion.Oct(convertible.ToUInt64(null));
				case TypeCode.String:
					try
					{
						num = Conversions.ToLong(convertible.ToString(null));
					}
					catch (OverflowException ex)
					{
						return Conversion.Oct(Conversions.ToULong(convertible.ToString(null)));
					}
					break;
				default:
					goto IL_013D;
				}
				if (num == 0L)
				{
					return "0";
				}
				if (num > 0L)
				{
					return Conversion.Oct(num);
				}
				if (num >= -2147483648L)
				{
					return Conversion.Oct(checked((int)num));
				}
				return Conversion.Oct(num);
			}
			IL_013D:
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[]
			{
				"Number",
				Utils.VBFriendlyName(Number)
			}));
		}

		public static string Str(object Number)
		{
			if (Number == null)
			{
				throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
			}
			IConvertible convertible = Number as IConvertible;
			if (convertible == null)
			{
				throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", new string[] { "Number" }));
			}
			TypeCode typeCode = convertible.GetTypeCode();
			string text;
			switch (typeCode)
			{
			case TypeCode.DBNull:
				return "Null";
			case TypeCode.Boolean:
				if (convertible.ToBoolean(null))
				{
					return "True";
				}
				return "False";
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				text = Conversions.ToString(Number);
				goto IL_010E;
			}
			if (typeCode == TypeCode.String)
			{
				try
				{
					text = Conversions.ToString(Conversions.ToDouble(convertible.ToString(null)));
					goto IL_010E;
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
			}
			throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", new string[] { "Number" }));
			IL_010E:
			if (text.Length > 0 && text[0] != '-')
			{
				return " " + Utils.StdFormat(text);
			}
			return Utils.StdFormat(text);
		}

		private static double HexOrOctValue(string InputStr, int i)
		{
			int num = 0;
			int length = InputStr.Length;
			char c = InputStr[i];
			long num3;
			checked
			{
				i++;
				if (c != 'H')
				{
					if (c != 'h')
					{
						if (c != 'O')
						{
							if (c != 'o')
							{
								return 0.0;
							}
						}
						while (i < length && num < 22)
						{
							c = InputStr[i];
							i++;
							char c2 = c;
							if (c2 != '\t')
							{
								if (c2 != '\n')
								{
									if (c2 != '\r')
									{
										if (c2 != ' ')
										{
											if (c2 != '\u3000')
											{
												int num2;
												if (c2 == '0')
												{
													if (num == 0)
													{
														continue;
													}
													num2 = 0;
												}
												else
												{
													if (c2 < '1' || c2 > '7')
													{
														break;
													}
													num2 = (int)(c - '0');
												}
												if (num3 >= 1152921504606846976L)
												{
													num3 = (num3 & 1152921504606846975L) * 8L;
													num3 |= 1152921504606846976L;
												}
												else
												{
													num3 *= 8L;
												}
												num3 += unchecked((long)num2);
												num++;
											}
										}
									}
								}
							}
						}
						if (num == 22)
						{
							i++;
							if (i < length)
							{
								c = InputStr[i];
							}
						}
						if (num3 <= 4294967296L)
						{
							if (num3 > 65535L || c == '&')
							{
								if (num3 > 2147483647L)
								{
									num3 = -2147483648L + (num3 & 2147483647L);
								}
							}
							else if ((num3 > 255L || c == '%') && num3 > 32767L)
							{
								num3 = -32768L + (num3 & 32767L);
							}
						}
						unchecked
						{
							if (c == '%')
							{
								num3 = (long)(checked((short)num3));
							}
							else if (c == '&')
							{
								num3 = (long)(checked((int)num3));
							}
							return (double)num3;
						}
					}
				}
				while (i < length && num < 17)
				{
					c = InputStr[i];
					i++;
					char c3 = c;
					if (c3 != '\t')
					{
						if (c3 != '\n')
						{
							if (c3 != '\r')
							{
								if (c3 != ' ')
								{
									if (c3 != '\u3000')
									{
										int num2;
										if (c3 == '0')
										{
											if (num == 0)
											{
												continue;
											}
											num2 = 0;
										}
										else if (c3 >= '1' && c3 <= '9')
										{
											num2 = (int)(c - '0');
										}
										else if (c3 >= 'A' && c3 <= 'F')
										{
											num2 = (int)(c - '7');
										}
										else
										{
											if (c3 < 'a' || c3 > 'f')
											{
												break;
											}
											num2 = (int)(c - 'W');
										}
										if (num == 15 && num3 > 576460752303423487L)
										{
											num3 = (num3 & 576460752303423487L) * 16L;
											num3 |= long.MinValue;
										}
										else
										{
											num3 *= 16L;
										}
										num3 += unchecked((long)num2);
										num++;
									}
								}
							}
						}
					}
				}
				if (num == 16)
				{
					i++;
					if (i < length)
					{
						c = InputStr[i];
					}
				}
				if (num <= 8)
				{
					if (num > 4 || c == '&')
					{
						if (num3 > 2147483647L)
						{
							num3 = -2147483648L + (num3 & 2147483647L);
						}
					}
					else if ((num > 2 || c == '%') && num3 > 32767L)
					{
						num3 = -32768L + (num3 & 32767L);
					}
				}
			}
			if (c == '%')
			{
				num3 = (long)(checked((short)num3));
			}
			else if (c == '&')
			{
				num3 = (long)(checked((int)num3));
			}
			return (double)num3;
		}

		public static double Val(string InputStr)
		{
			int num;
			if (InputStr == null)
			{
				num = 0;
			}
			else
			{
				num = InputStr.Length;
			}
			checked
			{
				int i;
				char c;
				for (i = 0; i < num; i++)
				{
					c = InputStr[i];
					char c2 = c;
					if (c2 != '\t')
					{
						if (c2 != '\n')
						{
							if (c2 != '\r')
							{
								if (c2 != ' ')
								{
									if (c2 != '\u3000')
									{
										break;
									}
								}
							}
						}
					}
				}
				if (i >= num)
				{
					return 0.0;
				}
				c = InputStr[i];
				if (c == '&')
				{
					return Conversion.HexOrOctValue(InputStr, i + 1);
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				double num2 = 0.0;
				c = InputStr[i];
				if (c == '-')
				{
					flag3 = true;
					i++;
				}
				else if (c == '+')
				{
					i++;
				}
				int num3;
				double num4;
				int num5;
				while (i < num)
				{
					c = InputStr[i];
					char c3 = c;
					if (c3 != '\t')
					{
						if (c3 != '\n')
						{
							if (c3 != '\r')
							{
								if (c3 != ' ')
								{
									if (c3 != '\u3000')
									{
										if (c3 == '0')
										{
											if (num3 != 0 || flag)
											{
												num4 = unchecked(num4 * 10.0 + (double)c - 48.0);
												i++;
												num3++;
												continue;
											}
											i++;
											continue;
										}
										else
										{
											if (c3 >= '1' && c3 <= '9')
											{
												num4 = unchecked(num4 * 10.0 + (double)c - 48.0);
												i++;
												num3++;
												continue;
											}
											if (c3 != '.')
											{
												if (c3 != 'e')
												{
													if (c3 != 'E')
													{
														if (c3 != 'd')
														{
															if (c3 != 'D')
															{
																break;
															}
														}
													}
												}
												flag2 = true;
												i++;
												break;
											}
											i++;
											if (!flag)
											{
												flag = true;
												num5 = num3;
												continue;
											}
											break;
										}
									}
								}
							}
						}
					}
					i++;
				}
				int num6;
				if (flag)
				{
					num6 = num3 - num5;
				}
				if (flag2)
				{
					bool flag4 = false;
					bool flag5 = false;
					while (i < num)
					{
						c = InputStr[i];
						char c4 = c;
						if (c4 != '\t')
						{
							if (c4 != '\n')
							{
								if (c4 != '\r')
								{
									if (c4 != ' ')
									{
										if (c4 != '\u3000')
										{
											if (c4 >= '0' && c4 <= '9')
											{
												num2 = unchecked(num2 * 10.0 + (double)c - 48.0);
												i++;
												continue;
											}
											if (c4 == '+')
											{
												if (!flag4)
												{
													flag4 = true;
													i++;
													continue;
												}
												break;
											}
											else
											{
												if (c4 == '-' && !flag4)
												{
													flag4 = true;
													flag5 = true;
													i++;
													continue;
												}
												break;
											}
										}
									}
								}
							}
						}
						i++;
					}
					unchecked
					{
						if (flag5)
						{
							num2 += (double)num6;
							num4 *= Math.Pow(10.0, -num2);
						}
						else
						{
							num2 -= (double)num6;
							num4 *= Math.Pow(10.0, num2);
						}
					}
				}
				else if (flag && num6 != 0)
				{
					num4 /= Math.Pow(10.0, (double)num6);
				}
				if (double.IsInfinity(num4))
				{
					throw ExceptionUtils.VbMakeException(6);
				}
				if (flag3)
				{
					num4 = unchecked(-num4);
				}
				char c5 = c;
				if (c5 == '%')
				{
					if (num6 > 0)
					{
						throw ExceptionUtils.VbMakeException(13);
					}
					num4 = (double)((short)Math.Round(num4));
				}
				else if (c5 == '&')
				{
					if (num6 > 0)
					{
						throw ExceptionUtils.VbMakeException(13);
					}
					num4 = (double)((int)Math.Round(num4));
				}
				else if (c5 == '!')
				{
					num4 = (double)((float)num4);
				}
				else if (c5 == '@')
				{
					num4 = Convert.ToDouble(new decimal(num4));
				}
				return num4;
			}
		}

		public static int Val(char Expression)
		{
			if (Expression >= '1' && Expression <= '9')
			{
				return (int)(checked(Expression - '0'));
			}
			return 0;
		}

		public static double Val(object Expression)
		{
			string text = Expression as string;
			if (text != null)
			{
				return Conversion.Val(text);
			}
			if (Expression is char)
			{
				return (double)Conversion.Val((char)Expression);
			}
			if (Versioned.IsNumeric(Expression))
			{
				return Conversions.ToDouble(Expression);
			}
			string text2;
			try
			{
				text2 = Conversions.ToString(Expression);
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
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[]
				{
					"Expression",
					Utils.VBFriendlyName(Expression)
				})), 438);
			}
			return Conversion.Val(text2);
		}

		internal static object ParseInputField(object Value, VariantType vtInput)
		{
			string text = Conversions.ToString(Value);
			if (vtInput == VariantType.Empty && (Value == null || Strings.Len(Conversions.ToString(Value)) == 0))
			{
				return null;
			}
			ProjectData projectData = ProjectData.GetProjectData();
			byte[] numprsPtr = projectData.m_numprsPtr;
			byte[] digitArray = projectData.m_DigitArray;
			Array.Copy(BitConverter.GetBytes(Convert.ToInt32(digitArray.Length)), 0, numprsPtr, 0, 4);
			Array.Copy(BitConverter.GetBytes(Convert.ToInt32(2388)), 0, numprsPtr, 4, 4);
			checked
			{
				if (UnsafeNativeMethods.VarParseNumFromStr(text, 1033, -2147483648, numprsPtr, digitArray) < 0)
				{
					if (vtInput != VariantType.Empty)
					{
						return 0;
					}
					return text;
				}
				else
				{
					int num = BitConverter.ToInt32(numprsPtr, 8);
					int num2 = BitConverter.ToInt32(numprsPtr, 12);
					int num3 = BitConverter.ToInt32(numprsPtr, 16);
					int num4 = BitConverter.ToInt32(numprsPtr, 20);
					char c;
					if (num2 < text.Length)
					{
						c = text[num2];
					}
					char c2 = c;
					int num5;
					int num6;
					if (c2 == '%')
					{
						num5 = 2;
						num6 = 0;
					}
					else if (c2 == '&')
					{
						num5 = 3;
						num6 = 0;
					}
					else if (c2 == '@')
					{
						num5 = 14;
						num6 = 4;
					}
					else if (c2 == '!')
					{
						if (vtInput == VariantType.Double)
						{
							num5 = 5;
						}
						else
						{
							num5 = 4;
						}
						num6 = int.MaxValue;
					}
					else if (c2 == '#')
					{
						num5 = 5;
						num6 = int.MaxValue;
					}
					else
					{
						if (vtInput == VariantType.Empty)
						{
							int num7 = 16428;
							if ((num & 2048) != 0)
							{
								num7 = 32;
							}
							return UnsafeNativeMethods.VarNumFromParseNum(numprsPtr, digitArray, num7);
						}
						if (num3 != 0)
						{
							Value = UnsafeNativeMethods.VarNumFromParseNum(numprsPtr, digitArray, 8);
							int num8 = Conversions.ToInteger(Value);
							if ((num8 & -65536) == 0)
							{
								num8 = (int)((short)num8);
							}
							UnsafeNativeMethods.VariantChangeType(out Value, ref Value, 0, (short)vtInput);
							return Value;
						}
						return UnsafeNativeMethods.VarNumFromParseNum(numprsPtr, digitArray, Conversion.ShiftVTBits((int)vtInput));
					}
					if (0 - num4 > num6)
					{
						throw ExceptionUtils.VbMakeException(13);
					}
					Value = UnsafeNativeMethods.VarNumFromParseNum(numprsPtr, digitArray, Conversion.ShiftVTBits(num5));
					if (vtInput == VariantType.Empty)
					{
						return Value;
					}
					UnsafeNativeMethods.VariantChangeType(out Value, ref Value, 0, (short)vtInput);
					return Value;
				}
			}
		}

		private static int ShiftVTBits(int vt)
		{
			switch (vt)
			{
			case 2:
				return 4;
			case 3:
				return 8;
			case 4:
				return 16;
			case 5:
				return 32;
			case 6:
			case 14:
				return 16384;
			case 7:
				return 128;
			case 8:
				return 256;
			case 9:
				return 512;
			case 10:
				return 1024;
			case 11:
				return 2048;
			case 12:
				return 4096;
			case 13:
				return 8192;
			case 17:
				return 131072;
			case 18:
				return 262144;
			case 20:
				return 1048576;
			}
			return 0;
		}

		private const int NUMPRS_LEADING_WHITE = 1;

		private const int NUMPRS_TRAILING_WHITE = 2;

		private const int NUMPRS_LEADING_PLUS = 4;

		private const int NUMPRS_TRAILING_PLUS = 8;

		private const int NUMPRS_LEADING_MINUS = 16;

		private const int NUMPRS_TRAILING_MINUS = 32;

		private const int NUMPRS_HEX_OCT = 64;

		private const int NUMPRS_PARENS = 128;

		private const int NUMPRS_DECIMAL = 256;

		private const int NUMPRS_THOUSANDS = 512;

		private const int NUMPRS_CURRENCY = 1024;

		private const int NUMPRS_EXPONENT = 2048;

		private const int NUMPRS_USE_ALL = 4096;

		private const int NUMPRS_STD = 8191;

		private const int NUMPRS_NEG = 65536;

		private const int NUMPRS_INEXACT = 131072;

		private const int VTBIT_EMPTY = 0;

		private const int VTBIT_NULL = 2;

		private const int VTBIT_I2 = 4;

		private const int VTBIT_I4 = 8;

		private const int VTBIT_R4 = 16;

		private const int VTBIT_R8 = 32;

		private const int VTBIT_CY = 64;

		private const int VTBIT_DATE = 128;

		private const int VTBIT_BSTR = 256;

		private const int VTBIT_OBJECT = 512;

		private const int VTBIT_ERROR = 1024;

		private const int VTBIT_BOOL = 2048;

		private const int VTBIT_VARIANT = 4096;

		private const int VTBIT_DATAOBJECT = 8192;

		private const int VTBIT_DECIMAL = 16384;

		private const int VTBIT_BYTE = 131072;

		private const int VTBIT_CHAR = 262144;

		private const int VTBIT_LONG = 1048576;

		private const int MAX_ERR_NUMBER = 65535;

		private const int LOCALE_NOUSEROVERRIDE = -2147483648;

		private const int LCID_US_ENGLISH = 1033;

		private const int PRSFLAGS = 2388;

		private const int VTBITS = 16428;

		private const char TYPE_INDICATOR_INT16 = '%';

		private const char TYPE_INDICATOR_INT32 = '&';

		private const char TYPE_INDICATOR_SINGLE = '!';

		private const char TYPE_INDICATOR_DECIMAL = '@';
	}
}
