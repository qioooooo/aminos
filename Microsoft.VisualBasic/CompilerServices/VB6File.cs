using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal abstract class VB6File
	{
		protected VB6File()
		{
		}

		protected VB6File(string sPath, OpenAccess access, OpenShare share, int lRecordLen)
		{
			if (access != OpenAccess.Read && access != OpenAccess.ReadWrite && access != OpenAccess.Write)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Access" }));
			}
			this.m_access = access;
			if (share != OpenShare.Shared && share != OpenShare.LockRead && share != OpenShare.LockReadWrite && share != OpenShare.LockWrite)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Share" }));
			}
			this.m_share = share;
			this.m_lRecordLen = lRecordLen;
			this.m_sFullPath = new FileInfo(sPath).FullName;
		}

		internal string GetAbsolutePath()
		{
			return this.m_sFullPath;
		}

		internal virtual void OpenFile()
		{
			try
			{
				if (File.Exists(this.m_sFullPath))
				{
					this.m_file = new FileStream(this.m_sFullPath, FileMode.Open, (FileAccess)this.m_access, (FileShare)this.m_share);
				}
				else
				{
					this.m_file = new FileStream(this.m_sFullPath, FileMode.Create, (FileAccess)this.m_access, (FileShare)this.m_share);
				}
			}
			catch (SecurityException ex)
			{
				throw ExceptionUtils.VbMakeException(53);
			}
		}

		internal virtual void CloseFile()
		{
			this.CloseTheFile();
		}

		protected void CloseTheFile()
		{
			if (this.m_sw != null)
			{
				this.m_sw.Close();
				this.m_sw = null;
			}
			if (this.m_sr != null)
			{
				this.m_sr.Close();
				this.m_sr = null;
			}
			if (this.m_file != null)
			{
				this.m_file.Close();
				this.m_file = null;
			}
		}

		internal int GetColumn()
		{
			return this.m_lCurrentColumn;
		}

		internal void SetColumn(int lColumn)
		{
			checked
			{
				if (this.m_lWidth != 0 && this.m_lCurrentColumn != 0 && lColumn + 14 > this.m_lWidth)
				{
					this.WriteLine(null);
				}
				else
				{
					this.SPC(lColumn - this.m_lCurrentColumn);
				}
			}
		}

		internal int GetWidth()
		{
			return this.m_lWidth;
		}

		internal void SetWidth(int RecordWidth)
		{
			if (RecordWidth < 0 || RecordWidth > 255)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			this.m_lWidth = RecordWidth;
		}

		internal virtual void WriteLine(string s)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void WriteString(string s)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual bool EOF()
		{
			return this.m_eof;
		}

		internal long LOF()
		{
			return this.m_file.Length;
		}

		internal virtual long LOC()
		{
			checked
			{
				if (this.m_lRecordLen == -1 || this.GetMode() != OpenMode.Random)
				{
					return this.m_position + 1L;
				}
				if (this.m_lRecordLen == 0)
				{
					throw ExceptionUtils.VbMakeException(51);
				}
				long position = this.m_position;
				if (position == 0L)
				{
					return 0L;
				}
				return this.m_position / unchecked((long)this.m_lRecordLen) + 1L;
			}
		}

		internal virtual StreamReader GetStreamReader()
		{
			return this.m_sr;
		}

		internal void SetRecord(long RecordNumber)
		{
			if (this.m_lRecordLen == 0)
			{
				return;
			}
			if (RecordNumber == 0L)
			{
				return;
			}
			checked
			{
				long num;
				if (this.m_lRecordLen == -1)
				{
					if (RecordNumber == -1L)
					{
						return;
					}
					num = RecordNumber - 1L;
				}
				else if (RecordNumber == -1L)
				{
					num = this.GetPos();
					if (num == 0L)
					{
						this.m_lRecordStart = 0L;
						return;
					}
					if (num % unchecked((long)this.m_lRecordLen) == 0L)
					{
						this.m_lRecordStart = num;
						return;
					}
					num = unchecked((long)this.m_lRecordLen) * (num / unchecked((long)this.m_lRecordLen) + 1L);
				}
				else if (RecordNumber != 0L)
				{
					if (this.m_lRecordLen == -1)
					{
						num = RecordNumber;
					}
					else
					{
						num = (RecordNumber - 1L) * unchecked((long)this.m_lRecordLen);
					}
				}
				this.SeekOffset(num);
				this.m_lRecordStart = num;
			}
		}

		internal virtual void Seek(long BaseOnePosition)
		{
			if (BaseOnePosition <= 0L)
			{
				throw ExceptionUtils.VbMakeException(63);
			}
			long num = checked(BaseOnePosition - 1L);
			if (num > this.m_file.Length)
			{
				this.m_file.SetLength(num);
			}
			this.m_file.Position = num;
			this.m_position = num;
			this.m_eof = this.m_position >= this.m_file.Length;
			if (this.m_sr != null)
			{
				this.m_sr.DiscardBufferedData();
			}
		}

		internal virtual long Seek()
		{
			return checked(this.m_position + 1L);
		}

		internal void SeekOffset(long offset)
		{
			this.m_position = offset;
			this.m_file.Position = offset;
			if (this.m_sr != null)
			{
				this.m_sr.DiscardBufferedData();
			}
		}

		internal long GetPos()
		{
			return this.m_position;
		}

		internal virtual void Lock()
		{
			this.m_file.Lock(0L, 2147483647L);
		}

		internal virtual void Unlock()
		{
			this.m_file.Unlock(0L, 2147483647L);
		}

		internal virtual void Lock(long Record)
		{
			checked
			{
				if (this.m_lRecordLen == -1)
				{
					this.m_file.Lock(Record - 1L, 1L);
				}
				else
				{
					this.m_file.Lock((Record - 1L) * unchecked((long)this.m_lRecordLen), unchecked((long)this.m_lRecordLen));
				}
			}
		}

		internal virtual void Unlock(long Record)
		{
			checked
			{
				if (this.m_lRecordLen == -1)
				{
					this.m_file.Unlock(Record - 1L, 1L);
				}
				else
				{
					this.m_file.Unlock((Record - 1L) * unchecked((long)this.m_lRecordLen), unchecked((long)this.m_lRecordLen));
				}
			}
		}

		internal virtual void Lock(long RecordStart, long RecordEnd)
		{
			checked
			{
				if (this.m_lRecordLen == -1)
				{
					this.m_file.Lock(RecordStart - 1L, RecordEnd - RecordStart + 1L);
				}
				else
				{
					this.m_file.Lock((RecordStart - 1L) * unchecked((long)this.m_lRecordLen), (RecordEnd - RecordStart + 1L) * unchecked((long)this.m_lRecordLen));
				}
			}
		}

		internal virtual void Unlock(long RecordStart, long RecordEnd)
		{
			checked
			{
				if (this.m_lRecordLen == -1)
				{
					this.m_file.Unlock(RecordStart - 1L, RecordEnd - RecordStart + 1L);
				}
				else
				{
					this.m_file.Unlock((RecordStart - 1L) * unchecked((long)this.m_lRecordLen), (RecordEnd - RecordStart + 1L) * unchecked((long)this.m_lRecordLen));
				}
			}
		}

		internal string LineInput()
		{
			this.ValidateReadable();
			string text = this.m_sr.ReadLine();
			if (text == null)
			{
				text = "";
			}
			checked
			{
				this.m_position += unchecked((long)(checked(this.m_Encoding.GetByteCount(text) + 2)));
				this.m_eof = this.CheckEOF(this.m_sr.Peek());
				return text;
			}
		}

		internal virtual bool CanInput()
		{
			return false;
		}

		internal virtual bool CanWrite()
		{
			return false;
		}

		protected virtual void InputObject(ref object Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		protected virtual string InputStr()
		{
			this.ValidateReadable();
			int num = this.SkipWhiteSpaceEOF();
			checked
			{
				string text;
				if (num == 34)
				{
					num = this.m_sr.Read();
					this.m_position += 1L;
					text = this.ReadInField(1);
				}
				else
				{
					text = this.ReadInField(2);
				}
				this.SkipTrailingWhiteSpace();
				return text;
			}
		}

		protected virtual object InputNum(VariantType vt)
		{
			this.ValidateReadable();
			this.SkipWhiteSpaceEOF();
			string text = this.ReadInField(3);
			object obj = text;
			this.SkipTrailingWhiteSpace();
			return obj;
		}

		public abstract OpenMode GetMode();

		internal string InputString(int lLen)
		{
			this.ValidateReadable();
			StringBuilder stringBuilder = new StringBuilder(lLen);
			OpenMode mode = this.GetMode();
			checked
			{
				for (int i = 1; i <= lLen; i++)
				{
					int num;
					if (mode == OpenMode.Binary)
					{
						num = this.m_br.Read();
						this.m_position += 1L;
						if (num == -1)
						{
							break;
						}
					}
					else
					{
						if (mode != OpenMode.Input)
						{
							throw ExceptionUtils.VbMakeException(54);
						}
						num = this.m_sr.Read();
						this.m_position += 1L;
						if ((num == -1) | (num == 26))
						{
							this.m_eof = true;
							throw ExceptionUtils.VbMakeException(62);
						}
					}
					if (num != 0)
					{
						stringBuilder.Append(Strings.ChrW(num));
					}
				}
				if (mode == OpenMode.Binary)
				{
					this.m_eof = this.m_br.PeekChar() == -1;
				}
				else
				{
					this.m_eof = this.CheckEOF(this.m_sr.Peek());
				}
				return stringBuilder.ToString();
			}
		}

		internal void SPC(int iCount)
		{
			if (iCount <= 0)
			{
				return;
			}
			int num = this.GetColumn();
			int width = this.GetWidth();
			checked
			{
				if (width != 0)
				{
					if (iCount >= width)
					{
						iCount %= width;
					}
					if (iCount + num > width)
					{
						iCount -= width - num;
						goto IL_0038;
					}
				}
				iCount += num;
				if (iCount >= num)
				{
					goto IL_0041;
				}
			}
			IL_0038:
			this.WriteLine(null);
			num = 0;
			IL_0041:
			if (iCount > num)
			{
				string text = new string(' ', checked(iCount - num));
				this.WriteString(text);
			}
		}

		internal void Tab(int Column)
		{
			if (Column < 1)
			{
				Column = 1;
			}
			checked
			{
				Column--;
				int num = this.GetColumn();
				int width = this.GetWidth();
				if (width != 0 && Column >= width)
				{
					Column %= width;
				}
				if (Column < num)
				{
					this.WriteLine(null);
					num = 0;
				}
				if (Column > num)
				{
					string text = new string(' ', Column - num);
					this.WriteString(text);
				}
			}
		}

		internal void SetPrintMode()
		{
			OpenMode mode = this.GetMode();
			if (mode == OpenMode.Input || mode == OpenMode.Binary || mode == OpenMode.Random)
			{
				throw ExceptionUtils.VbMakeException(54);
			}
			this.m_bPrint = true;
		}

		internal static VT VTType(object VarName)
		{
			if (VarName == null)
			{
				return VT.Variant;
			}
			return VB6File.VTFromComType(VarName.GetType());
		}

		internal static VT VTFromComType(Type typ)
		{
			if (typ == null)
			{
				return VT.Variant;
			}
			if (typ.IsArray)
			{
				typ = typ.GetElementType();
				if (typ.IsArray)
				{
					return (VT)8204;
				}
				VT vt = VB6File.VTFromComType(typ);
				if ((vt & VT.Array) != VT.Empty)
				{
					return (VT)8204;
				}
				return vt | VT.Array;
			}
			else
			{
				if (typ.IsEnum)
				{
					typ = Enum.GetUnderlyingType(typ);
				}
				if (typ == null)
				{
					return VT.Empty;
				}
				switch (Type.GetTypeCode(typ))
				{
				case TypeCode.DBNull:
					return VT.DBNull;
				case TypeCode.Boolean:
					return VT.Boolean;
				case TypeCode.Char:
					return VT.Char;
				case TypeCode.Byte:
					return VT.Byte;
				case TypeCode.Int16:
					return VT.Short;
				case TypeCode.Int32:
					return VT.Integer;
				case TypeCode.Int64:
					return VT.Long;
				case TypeCode.Single:
					return VT.Single;
				case TypeCode.Double:
					return VT.Double;
				case TypeCode.Decimal:
					return VT.Decimal;
				case TypeCode.DateTime:
					return VT.Date;
				case TypeCode.String:
					return VT.String;
				}
				if (typ == typeof(Missing))
				{
					return VT.Error;
				}
				if (typ == typeof(Exception) || typ.IsSubclassOf(typeof(Exception)))
				{
					return VT.Error;
				}
				if (typ.IsValueType)
				{
					return VT.Structure;
				}
				return VT.Variant;
			}
		}

		internal void PutFixedArray(long RecordNumber, Array arr, Type ElementType, int FixedStringLength = -1, int FirstBound = -1, int SecondBound = -1)
		{
			this.SetRecord(RecordNumber);
			if (ElementType == null)
			{
				ElementType = arr.GetType().GetElementType();
			}
			this.PutArrayData(arr, ElementType, FixedStringLength, FirstBound, SecondBound);
		}

		internal void PutDynamicArray(long RecordNumber, Array arr, bool ContainedInVariant = true, int FixedStringLength = -1)
		{
			int num;
			int upperBound;
			if (arr == null)
			{
				num = 0;
			}
			else
			{
				num = arr.Rank;
				upperBound = arr.GetUpperBound(0);
			}
			int num2;
			if (num == 1)
			{
				num2 = -1;
			}
			else if (num == 2)
			{
				num2 = arr.GetUpperBound(1);
			}
			else if (num != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_UnsupportedArrayDimensions"));
			}
			this.SetRecord(RecordNumber);
			checked
			{
				if (ContainedInVariant)
				{
					VT vt = VB6File.VTType(arr);
					this.m_bw.Write((short)vt);
					this.m_position += 2L;
					if ((vt & VT.Array) == VT.Empty)
					{
						throw ExceptionUtils.VbMakeException(458);
					}
				}
				this.PutArrayDesc(arr);
				if (num != 0)
				{
					this.PutArrayData(arr, arr.GetType().GetElementType(), FixedStringLength, upperBound, num2);
				}
			}
		}

		internal void LengthCheck(int Length)
		{
			if (this.m_lRecordLen == -1)
			{
				return;
			}
			if (Length > this.m_lRecordLen)
			{
				throw ExceptionUtils.VbMakeException(59);
			}
			checked
			{
				if (this.GetPos() + unchecked((long)Length) > this.m_lRecordStart + unchecked((long)this.m_lRecordLen))
				{
					throw ExceptionUtils.VbMakeException(59);
				}
			}
		}

		internal void PutFixedLengthString(long RecordNumber, string s, int lengthToWrite)
		{
			char c = ' ';
			if (s == null)
			{
				s = "";
			}
			if (Operators.CompareString(s, "", false) == 0)
			{
				c = '\0';
			}
			int num = this.m_Encoding.GetByteCount(s);
			checked
			{
				if (num > lengthToWrite)
				{
					if (num == s.Length)
					{
						s = Strings.Left(s, lengthToWrite);
					}
					else
					{
						byte[] bytes = this.m_Encoding.GetBytes(s);
						s = this.m_Encoding.GetString(bytes, 0, lengthToWrite);
						num = this.m_Encoding.GetByteCount(s);
						if (num > lengthToWrite)
						{
							for (int i = lengthToWrite - 1; i >= 0; i += -1)
							{
								bytes[i] = 0;
								s = this.m_Encoding.GetString(bytes, 0, lengthToWrite);
								num = this.m_Encoding.GetByteCount(s);
								if (num <= lengthToWrite)
								{
									break;
								}
							}
						}
					}
				}
				if (num < lengthToWrite)
				{
					s += Strings.StrDup(lengthToWrite - num, c);
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(lengthToWrite);
				this.m_sw.Write(s);
				this.m_position += unchecked((long)lengthToWrite);
			}
		}

		internal void PutVariantString(long RecordNumber, string s)
		{
			if (s == null)
			{
				s = "";
			}
			int byteCount = this.m_Encoding.GetByteCount(s);
			this.SetRecord(RecordNumber);
			checked
			{
				this.LengthCheck(byteCount + 2 + 2);
				this.m_bw.Write(8);
				this.m_bw.Write((short)byteCount);
				if (byteCount != 0)
				{
					this.m_sw.Write(s);
				}
				this.m_position += unchecked((long)(checked(byteCount + 2 + 2)));
			}
		}

		internal void PutString(long RecordNumber, string s)
		{
			if (s == null)
			{
				s = "";
			}
			int byteCount = this.m_Encoding.GetByteCount(s);
			this.SetRecord(RecordNumber);
			this.LengthCheck(byteCount);
			if (byteCount != 0)
			{
				this.m_sw.Write(s);
			}
			checked
			{
				this.m_position += unchecked((long)byteCount);
			}
		}

		internal void PutStringWithLength(long RecordNumber, string s)
		{
			if (s == null)
			{
				s = "";
			}
			int byteCount = this.m_Encoding.GetByteCount(s);
			this.SetRecord(RecordNumber);
			checked
			{
				this.LengthCheck(byteCount + 2);
				this.m_bw.Write((short)byteCount);
				if (byteCount != 0)
				{
					this.m_sw.Write(s);
				}
				this.m_position += unchecked((long)(checked(byteCount + 2)));
			}
		}

		internal void PutDate(long RecordNumber, DateTime dt, bool ContainedInVariant = false)
		{
			int num = 8;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(7);
				}
				double num2 = dt.ToOADate();
				this.m_bw.Write(num2);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutShort(long RecordNumber, short i, bool ContainedInVariant = false)
		{
			int num = 2;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(2);
				}
				this.m_bw.Write(i);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutInteger(long RecordNumber, int l, bool ContainedInVariant = false)
		{
			int num = 4;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(3);
				}
				this.m_bw.Write(l);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutLong(long RecordNumber, long l, bool ContainedInVariant = false)
		{
			int num = 8;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(20);
				}
				this.m_bw.Write(l);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutByte(long RecordNumber, byte byt, bool ContainedInVariant = false)
		{
			int num = 1;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(17);
				}
				this.m_bw.Write(byt);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutChar(long RecordNumber, char ch, bool ContainedInVariant = false)
		{
			int num = 2;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(18);
				}
				this.m_bw.Write(ch);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutSingle(long RecordNumber, float sng, bool ContainedInVariant = false)
		{
			int num = 4;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(4);
				}
				this.m_bw.Write(sng);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutDouble(long RecordNumber, double dbl, bool ContainedInVariant = false)
		{
			int num = 8;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(5);
				}
				this.m_bw.Write(dbl);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutEmpty(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			this.LengthCheck(2);
			this.m_bw.Write(0);
			checked
			{
				this.m_position += 2L;
			}
		}

		internal void PutBoolean(long RecordNumber, bool b, bool ContainedInVariant = false)
		{
			int num = 2;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(11);
				}
				if (b)
				{
					this.m_bw.Write(-1);
				}
				else
				{
					this.m_bw.Write(0);
				}
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutDecimal(long RecordNumber, decimal dec, bool ContainedInVariant = false)
		{
			int num = 16;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(14);
				}
				int[] bits = decimal.GetBits(dec);
				byte b = (byte)((bits[3] & int.MaxValue) / 65536);
				int num2 = bits[0];
				int num3 = bits[1];
				int num4 = bits[2];
				byte b2;
				if ((bits[3] & -2147483648) != 0)
				{
					b2 = 128;
				}
				this.m_bw.Write(14);
				this.m_bw.Write(b);
				this.m_bw.Write(b2);
				this.m_bw.Write(num4);
				this.m_bw.Write(num2);
				this.m_bw.Write(num3);
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutCurrency(long RecordNumber, decimal dec, bool ContainedInVariant = false)
		{
			int num = 16;
			checked
			{
				if (ContainedInVariant)
				{
					num += 2;
				}
				this.SetRecord(RecordNumber);
				this.LengthCheck(num);
				if (ContainedInVariant)
				{
					this.m_bw.Write(6);
				}
				this.m_bw.Write(decimal.ToOACurrency(dec));
				this.m_position += unchecked((long)num);
			}
		}

		internal void PutRecord(long RecordNumber, ValueType o)
		{
			if (o == null)
			{
				throw new NullReferenceException();
			}
			this.SetRecord(RecordNumber);
			PutHandler putHandler = new PutHandler(this);
			IRecordEnum recordEnum = putHandler;
			if (recordEnum == null)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			StructUtils.EnumerateUDT(o, recordEnum, false);
		}

		internal Type ComTypeFromVT(VT vtype)
		{
			switch (vtype)
			{
			case VT.Empty:
				return null;
			case VT.DBNull:
				return typeof(DBNull);
			case VT.Short:
				return typeof(short);
			case VT.Integer:
				return typeof(int);
			case VT.Single:
				return typeof(float);
			case VT.Double:
				return typeof(double);
			case VT.Date:
				return typeof(DateTime);
			case VT.String:
				return typeof(string);
			case VT.Error:
				return typeof(Exception);
			case VT.Boolean:
				return typeof(bool);
			case VT.Variant:
				return typeof(object);
			case VT.Decimal:
				return typeof(decimal);
			case VT.Byte:
				return typeof(byte);
			case VT.Char:
				return typeof(char);
			case VT.Long:
				return typeof(long);
			}
			throw ExceptionUtils.VbMakeException(458);
		}

		internal void GetFixedArray(long RecordNumber, ref Array arr, Type FieldType, int FirstBound = -1, int SecondBound = -1, int FixedStringLength = -1)
		{
			checked
			{
				if (SecondBound == -1)
				{
					arr = Array.CreateInstance(FieldType, FirstBound + 1);
				}
				else
				{
					arr = Array.CreateInstance(FieldType, FirstBound + 1, SecondBound + 1);
				}
				this.SetRecord(RecordNumber);
				this.GetArrayData(arr, FieldType, FirstBound, SecondBound, FixedStringLength);
			}
		}

		internal void GetDynamicArray(ref Array arr, Type t, int FixedStringLength = -1)
		{
			arr = this.GetArrayDesc(t);
			int rank = arr.Rank;
			int upperBound = arr.GetUpperBound(0);
			int num;
			if (rank == 1)
			{
				num = -1;
			}
			else
			{
				num = arr.GetUpperBound(1);
			}
			this.GetArrayData(arr, t, upperBound, num, FixedStringLength);
		}

		private void PutArrayDesc(Array arr)
		{
			checked
			{
				short num;
				if (arr == null)
				{
					num = 0;
				}
				else
				{
					num = (short)arr.Rank;
				}
				this.m_bw.Write(num);
				this.m_position += 2L;
				if (num == 0)
				{
					return;
				}
				int num2 = 0;
				int num3 = (int)(num - 1);
				for (int i = num2; i <= num3; i++)
				{
					this.m_bw.Write(arr.GetLength(i));
					this.m_bw.Write(arr.GetLowerBound(i));
					this.m_position += 8L;
				}
			}
		}

		internal Array GetArrayDesc(Type typ)
		{
			int num = (int)this.m_br.ReadInt16();
			checked
			{
				this.m_position += 2L;
				if (num == 0)
				{
					return Array.CreateInstance(typ, 0);
				}
				int[] array = new int[num - 1 + 1];
				int[] array2 = new int[num - 1 + 1];
				int num2 = 0;
				int num3 = num - 1;
				for (int i = num2; i <= num3; i++)
				{
					array[i] = this.m_br.ReadInt32();
					array2[i] = this.m_br.ReadInt32();
					this.m_position += 8L;
				}
				return Array.CreateInstance(typ, array, array2);
			}
		}

		internal virtual string GetLengthPrefixedString(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			if (this.EOF())
			{
				return "";
			}
			return this.ReadString();
		}

		internal virtual string GetFixedLengthString(long RecordNumber, int ByteLength)
		{
			this.SetRecord(RecordNumber);
			return this.ReadString(ByteLength);
		}

		protected string ReadString(int ByteLength)
		{
			if (ByteLength == 0)
			{
				return null;
			}
			byte[] array = this.m_br.ReadBytes(ByteLength);
			checked
			{
				this.m_position += unchecked((long)ByteLength);
				return this.m_Encoding.GetString(array);
			}
		}

		protected string ReadString()
		{
			int num = (int)this.m_br.ReadInt16();
			checked
			{
				this.m_position += 2L;
				if (num == 0)
				{
					return null;
				}
				this.LengthCheck(num);
				return this.ReadString(num);
			}
		}

		internal DateTime GetDate(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			double num = this.m_br.ReadDouble();
			checked
			{
				this.m_position += 8L;
				return DateTime.FromOADate(num);
			}
		}

		internal short GetShort(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			short num = this.m_br.ReadInt16();
			checked
			{
				this.m_position += 2L;
				return num;
			}
		}

		internal int GetInteger(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			int num = this.m_br.ReadInt32();
			checked
			{
				this.m_position += 4L;
				return num;
			}
		}

		internal long GetLong(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			long num = this.m_br.ReadInt64();
			checked
			{
				this.m_position += 8L;
				return num;
			}
		}

		internal byte GetByte(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			byte b = this.m_br.ReadByte();
			checked
			{
				this.m_position += 1L;
				return b;
			}
		}

		internal char GetChar(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			char c = this.m_br.ReadChar();
			checked
			{
				this.m_position += 1L;
				return c;
			}
		}

		internal float GetSingle(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			float num = this.m_br.ReadSingle();
			checked
			{
				this.m_position += 4L;
				return num;
			}
		}

		internal double GetDouble(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			double num = this.m_br.ReadDouble();
			checked
			{
				this.m_position += 8L;
				return num;
			}
		}

		internal decimal GetDecimal(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			int num = (int)this.m_br.ReadInt16();
			byte b = this.m_br.ReadByte();
			byte b2 = this.m_br.ReadByte();
			int num2 = this.m_br.ReadInt32();
			int num3 = this.m_br.ReadInt32();
			int num4 = this.m_br.ReadInt32();
			checked
			{
				this.m_position += 16L;
				bool flag;
				if (b2 != 0)
				{
					flag = true;
				}
				decimal num5 = new decimal(num3, num4, num2, flag, b);
				return num5;
			}
		}

		internal decimal GetCurrency(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			long num = this.m_br.ReadInt64();
			checked
			{
				this.m_position += 8L;
				return decimal.FromOACurrency(num);
			}
		}

		internal bool GetBoolean(long RecordNumber)
		{
			this.SetRecord(RecordNumber);
			short num = this.m_br.ReadInt16();
			checked
			{
				this.m_position += 2L;
				return num != 0;
			}
		}

		internal void GetRecord(long RecordNumber, ref ValueType o, bool ContainedInVariant = false)
		{
			if (o == null)
			{
				throw new NullReferenceException();
			}
			this.SetRecord(RecordNumber);
			GetHandler getHandler = new GetHandler(this);
			IRecordEnum recordEnum = getHandler;
			if (recordEnum == null)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			StructUtils.EnumerateUDT(o, recordEnum, true);
		}

		internal void PutArrayData(Array arr, Type typ, int FixedStringLength, int FirstBound, int SecondBound)
		{
			string text = null;
			char[] array = null;
			int num;
			int num2;
			if (arr == null)
			{
				num = -1;
				num2 = -1;
			}
			else if (arr.GetUpperBound(0) > FirstBound)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_ArrayDimensionsDontMatch"));
			}
			if (typ == null)
			{
				typ = arr.GetType().GetElementType();
			}
			VT vt = VB6File.VTFromComType(typ);
			int num3;
			int num4;
			if (SecondBound == -1)
			{
				num3 = 0;
				num4 = FirstBound;
				if (arr != null)
				{
					num = arr.GetUpperBound(0);
				}
			}
			else
			{
				num3 = SecondBound;
				num4 = FirstBound;
				if (arr != null)
				{
					if (arr.Rank != 2 || arr.GetUpperBound(1) != SecondBound)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_ArrayDimensionsDontMatch"));
					}
					num = arr.GetUpperBound(0);
					num2 = arr.GetUpperBound(1);
				}
			}
			if (vt == VT.String)
			{
				if (FixedStringLength == 0)
				{
					object obj;
					if (SecondBound == -1)
					{
						obj = arr.GetValue(0);
					}
					else
					{
						obj = arr.GetValue(0, 0);
					}
					if (obj != null)
					{
						FixedStringLength = obj.ToString().Length;
					}
				}
				if (FixedStringLength == 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidFixedLengthString"));
				}
				if (FixedStringLength > 0)
				{
					text = Strings.StrDup(FixedStringLength, ' ');
					array = text.ToCharArray();
				}
			}
			int byteLength = this.GetByteLength(vt);
			checked
			{
				if (SecondBound == -1 && byteLength > 0 && num4 == num)
				{
					int num5 = byteLength * (num4 + 1);
					if (this.GetPos() + unchecked((long)num5) <= this.m_lRecordStart + unchecked((long)this.m_lRecordLen))
					{
						byte[] array2 = new byte[num5 - 1 + 1];
						Buffer.BlockCopy(arr, 0, array2, 0, num5);
						this.m_bw.Write(array2);
						this.m_position += unchecked((long)num5);
						return;
					}
				}
				int num6 = 0;
				int num7 = num3;
				for (int i = num6; i <= num7; i++)
				{
					int num8 = 0;
					int num9 = num4;
					int j = num8;
					while (j <= num9)
					{
						object obj;
						try
						{
							if (SecondBound == -1)
							{
								if (j > num)
								{
									obj = null;
								}
								else
								{
									obj = arr.GetValue(j);
								}
							}
							else if (j > num || i > num2)
							{
								obj = null;
							}
							else
							{
								obj = arr.GetValue(j, i);
							}
						}
						catch (IndexOutOfRangeException ex)
						{
							obj = 0;
						}
						switch (vt)
						{
						case VT.Empty:
						case VT.DBNull:
							break;
						case VT.Short:
							this.LengthCheck(2);
							this.m_bw.Write(ShortType.FromObject(obj));
							this.m_position += 2L;
							break;
						case VT.Integer:
							this.LengthCheck(4);
							this.m_bw.Write(IntegerType.FromObject(obj));
							this.m_position += 4L;
							break;
						case VT.Single:
							this.LengthCheck(4);
							this.m_bw.Write(SingleType.FromObject(obj));
							this.m_position += 4L;
							break;
						case VT.Double:
							this.LengthCheck(8);
							this.m_bw.Write(DoubleType.FromObject(obj));
							this.m_position += 8L;
							break;
						case VT.Currency:
						case (VT)9:
						case (VT)13:
						case (VT)15:
						case (VT)16:
						case (VT)19:
						case (VT)21:
						case (VT)22:
						case (VT)23:
						case (VT)24:
						case (VT)25:
						case (VT)26:
						case (VT)27:
						case (VT)28:
						case (VT)29:
						case (VT)30:
						case (VT)31:
						case (VT)32:
						case (VT)33:
						case (VT)34:
						case (VT)35:
							goto IL_058E;
						case VT.Date:
							this.LengthCheck(8);
							this.m_bw.Write(DateType.FromObject(obj).ToOADate());
							this.m_position += 8L;
							break;
						case VT.String:
						{
							string text2;
							int num10;
							if (obj == null)
							{
								if (FixedStringLength > 0)
								{
									text2 = text;
									num10 = FixedStringLength;
								}
								else
								{
									text2 = "";
									num10 = 0;
								}
							}
							else
							{
								text2 = obj.ToString();
								num10 = this.m_Encoding.GetByteCount(text2);
								if (FixedStringLength > 0 && num10 > FixedStringLength)
								{
									if (num10 == text2.Length)
									{
										text2 = Strings.Left(text2, FixedStringLength);
										num10 = FixedStringLength;
									}
									else
									{
										byte[] bytes = this.m_Encoding.GetBytes(text2);
										text2 = this.m_Encoding.GetString(bytes, 0, FixedStringLength);
										num10 = this.m_Encoding.GetByteCount(text2);
									}
								}
							}
							if (num10 > 32767)
							{
								throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("FileIO_StringLengthExceeded")), 5);
							}
							if (FixedStringLength > 0)
							{
								this.LengthCheck(FixedStringLength);
								this.m_sw.Write(text2);
								if (num10 < FixedStringLength)
								{
									this.m_sw.Write(array, 0, FixedStringLength - num10);
								}
								this.m_position += unchecked((long)FixedStringLength);
							}
							else
							{
								this.LengthCheck(num10 + 2);
								this.m_bw.Write((short)num10);
								this.m_sw.Write(text2);
								this.m_position += unchecked((long)(checked(2 + num10)));
							}
							break;
						}
						case VT.Error:
							throw ExceptionUtils.VbMakeException(13);
						case VT.Boolean:
						{
							this.LengthCheck(2);
							bool flag = BooleanType.FromObject(obj);
							if (flag)
							{
								this.m_bw.Write(-1);
							}
							else
							{
								this.m_bw.Write(0);
							}
							this.m_position += 2L;
							break;
						}
						case VT.Variant:
							this.PutObject(obj, 0L, true);
							break;
						case VT.Decimal:
							this.LengthCheck(8);
							this.m_bw.Write(decimal.ToOACurrency(DecimalType.FromObject(obj)));
							this.m_position += 8L;
							break;
						case VT.Byte:
							this.LengthCheck(1);
							this.m_bw.Write(ByteType.FromObject(obj));
							this.m_position += 1L;
							break;
						case VT.Char:
							this.LengthCheck(2);
							this.m_bw.Write(CharType.FromObject(obj));
							this.m_position += 2L;
							break;
						case VT.Long:
							this.LengthCheck(8);
							this.m_bw.Write(LongType.FromObject(obj));
							this.m_position += 8L;
							break;
						case VT.Structure:
							this.PutObject(obj, 0L, false);
							break;
						default:
							goto IL_058E;
						}
						j++;
						continue;
						IL_058E:
						if ((vt & VT.Array) != VT.Empty)
						{
							throw ExceptionUtils.VbMakeException(13);
						}
						throw ExceptionUtils.VbMakeException(458);
					}
				}
			}
		}

		internal void GetArrayData(Array arr, Type typ, int FirstBound = -1, int SecondBound = -1, int FixedStringLength = -1)
		{
			object obj = null;
			if (arr == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_ArrayNotInitialized"));
			}
			if (typ == null)
			{
				typ = arr.GetType().GetElementType();
			}
			VT vt = VB6File.VTFromComType(typ);
			int num;
			int num2;
			if (SecondBound == -1)
			{
				num = 0;
				num2 = FirstBound;
			}
			else
			{
				num = SecondBound;
				num2 = FirstBound;
			}
			int byteLength = this.GetByteLength(vt);
			checked
			{
				if (SecondBound == -1 && byteLength > 0 && num2 == arr.GetUpperBound(0))
				{
					int num3 = byteLength * (num2 + 1);
					if (num3 <= arr.Length * byteLength)
					{
						Buffer.BlockCopy(this.m_br.ReadBytes(num3), 0, arr, 0, num3);
						this.m_position += unchecked((long)num3);
						return;
					}
				}
				int num4 = 0;
				int num5 = num;
				for (int i = num4; i <= num5; i++)
				{
					int num6 = 0;
					int num7 = num2;
					int j = num6;
					while (j <= num7)
					{
						switch (vt)
						{
						case VT.Empty:
						case VT.DBNull:
							break;
						case VT.Short:
							obj = this.m_br.ReadInt16();
							this.m_position += 2L;
							break;
						case VT.Integer:
							obj = this.m_br.ReadInt32();
							this.m_position += 4L;
							break;
						case VT.Single:
							obj = this.m_br.ReadSingle();
							this.m_position += 4L;
							break;
						case VT.Double:
							obj = this.m_br.ReadDouble();
							this.m_position += 8L;
							break;
						case VT.Currency:
						case (VT)9:
						case (VT)13:
						case (VT)15:
						case (VT)16:
						case (VT)19:
						case (VT)21:
						case (VT)22:
						case (VT)23:
						case (VT)24:
						case (VT)25:
						case (VT)26:
						case (VT)27:
						case (VT)28:
						case (VT)29:
						case (VT)30:
						case (VT)31:
						case (VT)32:
						case (VT)33:
						case (VT)34:
						case (VT)35:
							goto IL_0353;
						case VT.Date:
							obj = DateTime.FromOADate(this.m_br.ReadDouble());
							this.m_position += 8L;
							break;
						case VT.String:
							if (FixedStringLength >= 0)
							{
								obj = this.ReadString(FixedStringLength);
							}
							else
							{
								obj = this.ReadString();
							}
							break;
						case VT.Error:
							break;
						case VT.Boolean:
							obj = this.m_br.ReadInt16() != 0;
							this.m_position += 2L;
							break;
						case VT.Variant:
							if (SecondBound == -1)
							{
								obj = arr.GetValue(j);
							}
							else
							{
								obj = arr.GetValue(j, i);
							}
							this.GetObject(ref obj, 0L, true);
							break;
						case VT.Decimal:
						{
							long num8 = this.m_br.ReadInt64();
							this.m_position += 8L;
							obj = decimal.FromOACurrency(num8);
							break;
						}
						case VT.Byte:
							obj = this.m_br.ReadByte();
							this.m_position += 1L;
							break;
						case VT.Char:
							obj = this.m_br.ReadChar();
							this.m_position += 1L;
							break;
						case VT.Long:
							obj = this.m_br.ReadInt64();
							this.m_position += 8L;
							break;
						case VT.Structure:
							if (SecondBound == -1)
							{
								obj = arr.GetValue(j);
							}
							else
							{
								obj = arr.GetValue(j, i);
							}
							this.GetObject(ref obj, 0L, false);
							break;
						default:
							goto IL_0353;
						}
						try
						{
							IL_03AA:
							if (SecondBound == -1)
							{
								arr.SetValue(obj, j);
							}
							else
							{
								arr.SetValue(obj, j, i);
							}
						}
						catch (IndexOutOfRangeException ex)
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_ArrayDimensionsDontMatch"));
						}
						j++;
						continue;
						IL_0353:
						if ((vt & VT.Array) == VT.Empty)
						{
							throw ExceptionUtils.VbMakeException(458);
						}
						vt ^= VT.Array;
						if (vt == VT.Variant)
						{
							throw ExceptionUtils.VbMakeException(13);
						}
						if (vt > VT.Variant && vt != VT.Byte && vt != VT.Decimal && vt != VT.Char && vt != VT.Long)
						{
							throw ExceptionUtils.VbMakeException(458);
						}
						goto IL_03AA;
					}
				}
			}
		}

		private int GetByteLength(VT vtype)
		{
			switch (vtype)
			{
			case VT.Short:
				return 2;
			case VT.Integer:
				return 4;
			case VT.Single:
				return 4;
			case VT.Double:
				return 8;
			case VT.Byte:
				return 1;
			case VT.Long:
				return 8;
			}
			return -1;
		}

		private void PrintTab(TabInfo ti)
		{
			checked
			{
				if (ti.Column == -1)
				{
					int num = this.GetColumn();
					num += 14 - num % 14;
					this.SetColumn(num);
				}
				else
				{
					this.Tab((int)ti.Column);
				}
			}
		}

		private string AddSpaces(string s)
		{
			string negativeSign = Thread.CurrentThread.CurrentCulture.NumberFormat.NegativeSign;
			if (negativeSign.Length == 1)
			{
				if (s[0] == negativeSign[0])
				{
					return s + " ";
				}
			}
			else if (Operators.CompareString(Strings.Left(s, negativeSign.Length), negativeSign, false) == 0)
			{
				return s + " ";
			}
			return " " + s + " ";
		}

		internal void PrintLine(params object[] Output)
		{
			this.Print(Output);
			this.WriteLine(null);
		}

		internal void Print(params object[] Output)
		{
			this.SetPrintMode();
			checked
			{
				if (Output != null)
				{
					if (Output.Length == 0)
					{
						return;
					}
					int upperBound = Output.GetUpperBound(0);
					int num = -1;
					int num2 = 0;
					int num3 = upperBound;
					int i = num2;
					while (i <= num3)
					{
						object obj = Output[i];
						Type type;
						if (obj == null)
						{
							type = null;
						}
						else
						{
							type = obj.GetType();
							if (type.IsEnum)
							{
								type = Enum.GetUnderlyingType(type);
							}
						}
						if (obj == null)
						{
						}
						string text;
						if (type == null)
						{
							text = "";
							goto IL_0260;
						}
						switch (Type.GetTypeCode(type))
						{
						case TypeCode.DBNull:
							text = "Null";
							goto IL_0260;
						case TypeCode.Boolean:
							text = StringType.FromBoolean(BooleanType.FromObject(obj));
							goto IL_0260;
						case TypeCode.Char:
							text = StringType.FromChar(CharType.FromObject(obj));
							goto IL_0260;
						case TypeCode.Byte:
							text = this.AddSpaces(StringType.FromByte(ByteType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Int16:
							text = this.AddSpaces(StringType.FromShort(ShortType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Int32:
							text = this.AddSpaces(StringType.FromInteger(IntegerType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Int64:
							text = this.AddSpaces(StringType.FromLong(LongType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Single:
							text = this.AddSpaces(StringType.FromSingle(SingleType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Double:
							text = this.AddSpaces(StringType.FromDouble(DoubleType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.Decimal:
							text = this.AddSpaces(StringType.FromDecimal(DecimalType.FromObject(obj)));
							goto IL_0260;
						case TypeCode.DateTime:
							text = StringType.FromDate(DateType.FromObject(obj)) + " ";
							goto IL_0260;
						case TypeCode.String:
							text = obj.ToString();
							goto IL_0260;
						}
						if (type == typeof(TabInfo))
						{
							object obj2 = obj;
							TabInfo tabInfo;
							this.PrintTab((obj2 != null) ? ((TabInfo)obj2) : tabInfo);
							num = i;
						}
						else if (type == typeof(SpcInfo))
						{
							object obj3 = obj;
							SpcInfo spcInfo;
							this.SPC((int)((obj3 != null) ? ((SpcInfo)obj3) : spcInfo).Count);
							num = i;
						}
						else
						{
							if (type == typeof(Missing))
							{
								text = "Error 448";
								goto IL_0260;
							}
							throw new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { Utils.VBFriendlyName(type) }));
						}
						IL_0287:
						i++;
						continue;
						IL_0260:
						if (num != i - 1)
						{
							int column = this.GetColumn();
							this.SetColumn(column + (14 - column % 14));
						}
						this.WriteString(text);
						goto IL_0287;
					}
				}
			}
		}

		internal void WriteLineHelper(params object[] Output)
		{
			this.InternalWriteHelper(Output);
			this.WriteLine(null);
		}

		internal void WriteHelper(params object[] Output)
		{
			this.InternalWriteHelper(Output);
			this.WriteString(",");
		}

		private void InternalWriteHelper(params object[] Output)
		{
			Type typeFromHandle = typeof(SpcInfo);
			Type type = typeFromHandle;
			NumberFormatInfo numberFormat = Utils.GetInvariantCultureInfo().NumberFormat;
			int num = 0;
			int upperBound = Output.GetUpperBound(0);
			checked
			{
				for (int i = num; i <= upperBound; i++)
				{
					object obj = Output[i];
					if (obj == null)
					{
						this.WriteString("#ERROR 448#");
					}
					else
					{
						if (type != typeFromHandle)
						{
							this.WriteString(",");
						}
						type = obj.GetType();
						if (type == typeFromHandle)
						{
							object obj2 = obj;
							SpcInfo spcInfo;
							this.SPC((int)((obj2 != null) ? ((SpcInfo)obj2) : spcInfo).Count);
						}
						else if (type == typeof(TabInfo))
						{
							object obj3 = obj;
							TabInfo tabInfo2;
							TabInfo tabInfo = ((obj3 != null) ? ((TabInfo)obj3) : tabInfo2);
							if (tabInfo.Column >= 0)
							{
								this.PrintTab(tabInfo);
							}
						}
						else if (type == typeof(Missing))
						{
							this.WriteString("#ERROR 448#");
						}
						else
						{
							switch (Type.GetTypeCode(type))
							{
							case TypeCode.DBNull:
								this.WriteString("#NULL#");
								goto IL_027F;
							case TypeCode.Boolean:
								if (BooleanType.FromObject(obj))
								{
									this.WriteString("#TRUE#");
									goto IL_027F;
								}
								this.WriteString("#FALSE#");
								goto IL_027F;
							case TypeCode.Char:
								this.WriteString(StringType.FromChar(CharType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.Byte:
								this.WriteString(StringType.FromByte(ByteType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.Int16:
								this.WriteString(StringType.FromShort(ShortType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.Int32:
								this.WriteString(StringType.FromInteger(IntegerType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.Int64:
								this.WriteString(StringType.FromLong(LongType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.Single:
								this.WriteString(this.IOStrFromSingle(SingleType.FromObject(obj), numberFormat));
								goto IL_027F;
							case TypeCode.Double:
								this.WriteString(this.IOStrFromDouble(DoubleType.FromObject(obj), numberFormat));
								goto IL_027F;
							case TypeCode.Decimal:
								this.WriteString(this.IOStrFromDecimal(DecimalType.FromObject(obj), numberFormat));
								goto IL_027F;
							case TypeCode.DateTime:
								this.WriteString(this.FormatUniversalDate(DateType.FromObject(obj)));
								goto IL_027F;
							case TypeCode.String:
								this.WriteString(this.GetQuotedString(obj.ToString()));
								goto IL_027F;
							}
							if (!(obj is char[]) || ((Array)obj).Rank != 1)
							{
								throw ExceptionUtils.VbMakeException(5);
							}
							this.WriteString(new string(CharArrayType.FromObject(obj)));
						}
					}
					IL_027F:;
				}
			}
		}

		private string IOStrFromSingle(float Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString(null, NumberFormat);
		}

		private string IOStrFromDouble(double Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString(null, NumberFormat);
		}

		private string IOStrFromDecimal(decimal Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G29", NumberFormat);
		}

		internal string FormatUniversalDate(DateTime dt)
		{
			string text = "T";
			bool flag;
			if (dt.Year != 0 || dt.Month != 1 || dt.Day != 1)
			{
				flag = true;
				text = "d";
			}
			if (checked(dt.Hour + dt.Minute + dt.Second) != 0 && flag)
			{
				text = "F";
			}
			return dt.ToString(text, FileSystem.m_WriteDateFormatInfo);
		}

		protected string GetQuotedString(string Value)
		{
			return "\"" + Value.Replace("\"", "\"\"") + "\"";
		}

		protected void ValidateRec(long RecordNumber)
		{
			if (RecordNumber < 1L)
			{
				throw ExceptionUtils.VbMakeException(63);
			}
		}

		internal virtual void GetObject(ref object Value, long RecordNumber = 0L, bool ContainedInVariant = true)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref ValueType Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref Array Value, long RecordNumber = 0L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref bool Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref byte Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref short Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref int Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref long Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref char Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref float Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref double Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref decimal Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Get(ref DateTime Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void PutObject(object Value, long RecordNumber = 0L, bool ContainedInVariant = true)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(object Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(ValueType Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(Array Value, long RecordNumber = 0L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(bool Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(byte Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(short Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(int Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(long Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(char Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(float Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(double Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(decimal Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Put(DateTime Value, long RecordNumber = 0L)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref object obj)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref bool Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref byte Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref short Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref int Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref long Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref char Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref float Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref double Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref decimal Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref string Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		internal virtual void Input(ref DateTime Value)
		{
			throw ExceptionUtils.VbMakeException(54);
		}

		protected int SkipWhiteSpace()
		{
			int num = this.m_sr.Peek();
			checked
			{
				if (this.CheckEOF(num))
				{
					this.m_eof = true;
				}
				else
				{
					while (this.IntlIsSpace(num) || num == 9)
					{
						this.m_sr.Read();
						this.m_position += 1L;
						num = this.m_sr.Peek();
						if (this.CheckEOF(num))
						{
							this.m_eof = true;
							break;
						}
					}
				}
				return num;
			}
		}

		private string GetFileInTerm(short iTermType)
		{
			switch (iTermType)
			{
			case 0:
				return "\r";
			case 1:
				return "\"";
			case 2:
				return ",\r";
			case 3:
				return " ,\t\r";
			case 6:
				return " ,\t\r";
			}
			throw ExceptionUtils.VbMakeException(5);
		}

		protected bool IntlIsSpace(int lch)
		{
			return (lch == 32) | (lch == 12288);
		}

		protected bool IntlIsDoubleQuote(int lch)
		{
			return lch == 34;
		}

		protected bool IntlIsComma(int lch)
		{
			return lch == 44;
		}

		protected int SkipWhiteSpaceEOF()
		{
			int num = this.SkipWhiteSpace();
			if (this.CheckEOF(num))
			{
				throw ExceptionUtils.VbMakeException(62);
			}
			return num;
		}

		protected void SkipTrailingWhiteSpace()
		{
			int num = this.m_sr.Peek();
			if (this.CheckEOF(num))
			{
				this.m_eof = true;
				return;
			}
			if (!this.IntlIsSpace(num))
			{
				if (!this.IntlIsDoubleQuote(num))
				{
					if (num != 9)
					{
						goto IL_00B4;
					}
				}
			}
			num = this.m_sr.Read();
			checked
			{
				this.m_position += 1L;
				num = this.m_sr.Peek();
				if (this.CheckEOF(num))
				{
					this.m_eof = true;
					return;
				}
				while (this.IntlIsSpace(num) || num == 9)
				{
					this.m_sr.Read();
					this.m_position += 1L;
					num = this.m_sr.Peek();
					if (this.CheckEOF(num))
					{
						this.m_eof = true;
						return;
					}
				}
			}
			IL_00B4:
			checked
			{
				if (num == 13)
				{
					num = this.m_sr.Read();
					this.m_position += 1L;
					if (this.CheckEOF(num))
					{
						this.m_eof = true;
						return;
					}
					if (this.m_sr.Peek() == 10)
					{
						num = this.m_sr.Read();
						this.m_position += 1L;
					}
				}
				else if (this.IntlIsComma(num))
				{
					num = this.m_sr.Read();
					this.m_position += 1L;
				}
				num = this.m_sr.Peek();
				if (this.CheckEOF(num))
				{
					this.m_eof = true;
					return;
				}
			}
		}

		protected string ReadInField(short iTermType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string fileInTerm = this.GetFileInTerm(iTermType);
			int num = this.m_sr.Peek();
			checked
			{
				if (this.CheckEOF(num))
				{
					this.m_eof = true;
				}
				else
				{
					while (fileInTerm.IndexOf(Strings.ChrW(num)) == -1)
					{
						num = this.m_sr.Read();
						this.m_position += 1L;
						if (num != 0)
						{
							stringBuilder.Append(Strings.ChrW(num));
						}
						num = this.m_sr.Peek();
						if (this.CheckEOF(num))
						{
							this.m_eof = true;
							break;
						}
					}
				}
				string text;
				if (iTermType == 2 || iTermType == 3)
				{
					text = Strings.RTrim(stringBuilder.ToString());
				}
				else
				{
					text = stringBuilder.ToString();
				}
				return text;
			}
		}

		protected bool CheckEOF(int lChar)
		{
			return lChar == -1 || lChar == 26;
		}

		private void ValidateReadable()
		{
			if (this.m_access != OpenAccess.ReadWrite && this.m_access != OpenAccess.Read)
			{
				NullReferenceException ex = new NullReferenceException();
				throw new NullReferenceException(ex.Message, new IOException(Utils.GetResourceString("FileOpenedNoRead")));
			}
		}

		internal int m_lCurrentColumn;

		internal int m_lWidth;

		internal int m_lRecordLen;

		internal long m_lRecordStart;

		internal string m_sFullPath;

		internal OpenShare m_share;

		internal OpenAccess m_access;

		internal bool m_eof;

		internal long m_position;

		internal FileStream m_file;

		internal bool m_fAppend;

		internal bool m_bPrint;

		protected StreamWriter m_sw;

		protected StreamReader m_sr;

		protected BinaryWriter m_bw;

		protected BinaryReader m_br;

		protected Encoding m_Encoding;

		protected const int lchTab = 9;

		protected const int lchCR = 13;

		protected const int lchLF = 10;

		protected const int lchSpace = 32;

		protected const int lchIntlSpace = 12288;

		protected const int lchDoubleQuote = 34;

		protected const int lchPound = 35;

		protected const int lchComma = 44;

		protected const int EOF_INDICATOR = -1;

		protected const int EOF_CHAR = 26;

		protected const short FIN_NUMTERMCHAR = 6;

		protected const short FIN_LINEINP = 0;

		protected const short FIN_QSTRING = 1;

		protected const short FIN_STRING = 2;

		protected const short FIN_NUMBER = 3;
	}
}
