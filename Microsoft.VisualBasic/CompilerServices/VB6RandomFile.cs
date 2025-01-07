using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class VB6RandomFile : VB6File
	{
		public VB6RandomFile(string FileName, OpenAccess access, OpenShare share, int lRecordLen)
			: base(FileName, access, share, lRecordLen)
		{
		}

		private void OpenFileHelper(FileMode fm, OpenAccess fa)
		{
			try
			{
				this.m_file = new FileStream(this.m_sFullPath, fm, (FileAccess)fa, (FileShare)this.m_share);
			}
			catch (FileNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(ex, 53);
			}
			catch (DirectoryNotFoundException ex2)
			{
				throw ExceptionUtils.VbMakeException(ex2, 76);
			}
			catch (SecurityException ex3)
			{
				throw ExceptionUtils.VbMakeException(ex3, 53);
			}
			catch (IOException ex4)
			{
				throw ExceptionUtils.VbMakeException(ex4, 75);
			}
			catch (UnauthorizedAccessException ex5)
			{
				throw ExceptionUtils.VbMakeException(ex5, 75);
			}
			catch (ArgumentException ex6)
			{
				throw ExceptionUtils.VbMakeException(ex6, 75);
			}
			catch (StackOverflowException ex7)
			{
				throw ex7;
			}
			catch (OutOfMemoryException ex8)
			{
				throw ex8;
			}
			catch (ThreadAbortException ex9)
			{
				throw ex9;
			}
			catch (Exception ex10)
			{
				throw ExceptionUtils.VbMakeException(51);
			}
		}

		internal override void OpenFile()
		{
			FileMode fileMode;
			if (File.Exists(this.m_sFullPath))
			{
				fileMode = FileMode.Open;
			}
			else if (this.m_access == OpenAccess.Read)
			{
				fileMode = FileMode.OpenOrCreate;
			}
			else
			{
				fileMode = FileMode.Create;
			}
			if (this.m_access == OpenAccess.Default)
			{
				this.m_access = OpenAccess.ReadWrite;
				try
				{
					this.OpenFileHelper(fileMode, this.m_access);
					goto IL_0094;
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
					this.m_access = OpenAccess.Write;
					try
					{
						this.OpenFileHelper(fileMode, this.m_access);
						goto IL_0094;
					}
					catch (StackOverflowException ex4)
					{
						throw ex4;
					}
					catch (OutOfMemoryException ex5)
					{
						throw ex5;
					}
					catch (ThreadAbortException ex6)
					{
						throw ex6;
					}
					catch (Exception)
					{
						this.m_access = OpenAccess.Read;
						this.OpenFileHelper(fileMode, this.m_access);
						goto IL_0094;
					}
				}
			}
			this.OpenFileHelper(fileMode, this.m_access);
			IL_0094:
			this.m_Encoding = Utils.GetFileIOEncoding();
			Stream file = this.m_file;
			if (this.m_access == OpenAccess.Write || this.m_access == OpenAccess.ReadWrite)
			{
				this.m_sw = new StreamWriter(file, this.m_Encoding);
				this.m_sw.AutoFlush = true;
				this.m_bw = new BinaryWriter(file, this.m_Encoding);
			}
			if (this.m_access == OpenAccess.Read || this.m_access == OpenAccess.ReadWrite)
			{
				this.m_br = new BinaryReader(file, this.m_Encoding);
				if (this.GetMode() == OpenMode.Binary)
				{
					this.m_sr = new StreamReader(file, this.m_Encoding, false, 128);
				}
			}
		}

		internal override void CloseFile()
		{
			if (this.m_sw != null)
			{
				this.m_sw.Flush();
			}
			this.CloseTheFile();
		}

		internal override void Lock(long lStart, long lEnd)
		{
			if (lStart > lEnd)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
			}
			checked
			{
				long num = (lStart - 1L) * unchecked((long)this.m_lRecordLen);
				long num2 = (lEnd - lStart + 1L) * unchecked((long)this.m_lRecordLen);
				this.m_file.Lock(num, num2);
			}
		}

		internal override void Unlock(long lStart, long lEnd)
		{
			if (lStart > lEnd)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
			}
			checked
			{
				long num = (lStart - 1L) * unchecked((long)this.m_lRecordLen);
				long num2 = (lEnd - lStart + 1L) * unchecked((long)this.m_lRecordLen);
				this.m_file.Unlock(num, num2);
			}
		}

		public override OpenMode GetMode()
		{
			return OpenMode.Random;
		}

		internal override StreamReader GetStreamReader()
		{
			return new StreamReader(this.m_file, this.m_Encoding);
		}

		internal override bool EOF()
		{
			this.m_eof = this.m_position >= this.m_file.Length;
			return this.m_eof;
		}

		internal override long LOC()
		{
			if (this.m_lRecordLen == 0)
			{
				throw ExceptionUtils.VbMakeException(51);
			}
			long position = this.m_position;
			return checked(position + unchecked((long)this.m_lRecordLen) - 1L) / (long)this.m_lRecordLen;
		}

		internal override void Seek(long Position)
		{
			this.SetRecord(Position);
		}

		internal override long Seek()
		{
			return checked(this.LOC() + 1L);
		}

		internal override void GetObject(ref object Value, long RecordNumber = 0L, bool ContainedInVariant = true)
		{
			Type type = null;
			this.ValidateReadable();
			this.SetRecord(RecordNumber);
			VT vt;
			checked
			{
				if (ContainedInVariant)
				{
					vt = (VT)this.m_br.ReadInt16();
					this.m_position += 2L;
				}
				else
				{
					type = Value.GetType();
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Object:
						if (type.IsValueType)
						{
							vt = VT.Structure;
							goto IL_00D7;
						}
						vt = VT.Variant;
						goto IL_00D7;
					case TypeCode.Boolean:
						vt = VT.Boolean;
						goto IL_00D7;
					case TypeCode.Char:
						vt = VT.Char;
						goto IL_00D7;
					case TypeCode.Byte:
						vt = VT.Byte;
						goto IL_00D7;
					case TypeCode.Int16:
						vt = VT.Short;
						goto IL_00D7;
					case TypeCode.Int32:
						vt = VT.Integer;
						goto IL_00D7;
					case TypeCode.Int64:
						vt = VT.Long;
						goto IL_00D7;
					case TypeCode.Single:
						vt = VT.Single;
						goto IL_00D7;
					case TypeCode.Double:
						vt = VT.Double;
						goto IL_00D7;
					case TypeCode.Decimal:
						vt = VT.Decimal;
						goto IL_00D7;
					case TypeCode.DateTime:
						vt = VT.Date;
						goto IL_00D7;
					case TypeCode.String:
						vt = VT.String;
						goto IL_00D7;
					}
					vt = VT.Variant;
				}
			}
			IL_00D7:
			if ((vt & VT.Array) != VT.Empty)
			{
				Array array = null;
				VT vt2 = vt ^ VT.Array;
				this.GetDynamicArray(ref array, this.ComTypeFromVT(vt2), -1);
				Value = array;
			}
			else if (vt == VT.String)
			{
				Value = this.GetLengthPrefixedString(0L);
			}
			else if (vt == VT.Short)
			{
				Value = this.GetShort(0L);
			}
			else if (vt == VT.Integer)
			{
				Value = this.GetInteger(0L);
			}
			else if (vt == VT.Long)
			{
				Value = this.GetLong(0L);
			}
			else if (vt == VT.Byte)
			{
				Value = this.GetByte(0L);
			}
			else if (vt == VT.Date)
			{
				Value = this.GetDate(0L);
			}
			else if (vt == VT.Double)
			{
				Value = this.GetDouble(0L);
			}
			else if (vt == VT.Single)
			{
				Value = this.GetSingle(0L);
			}
			else if (vt == VT.Currency)
			{
				Value = this.GetCurrency(0L);
			}
			else if (vt == VT.Decimal)
			{
				Value = this.GetDecimal(0L);
			}
			else if (vt == VT.Boolean)
			{
				Value = this.GetBoolean(0L);
			}
			else if (vt == VT.Char)
			{
				Value = this.GetChar(0L);
			}
			else if (vt == VT.Structure)
			{
				ValueType valueType = (ValueType)Value;
				this.GetRecord(0L, ref valueType, false);
				Value = valueType;
			}
			else if (vt == VT.DBNull && ContainedInVariant)
			{
				Value = DBNull.Value;
			}
			else
			{
				if (vt == VT.DBNull)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { "DBNull" })), 5);
				}
				if (vt == VT.Empty)
				{
					Value = null;
				}
				else
				{
					if (vt == VT.Currency)
					{
						throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { "Currency" })), 5);
					}
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { type.FullName })), 5);
				}
			}
		}

		internal override void Get(ref ValueType Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			this.GetRecord(RecordNumber, ref Value, false);
		}

		internal override void Get(ref Array Value, long RecordNumber = 0L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			this.ValidateReadable();
			if (Value == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_ArrayNotInitialized"));
			}
			Type elementType = Value.GetType().GetElementType();
			int num = -1;
			int num2 = Value.Rank;
			int num3 = -1;
			this.SetRecord(RecordNumber);
			if (this.m_file.Position >= this.m_file.Length)
			{
				return;
			}
			if (StringIsFixedLength && elementType == typeof(string))
			{
				object obj;
				if (num2 == 1)
				{
					obj = Value.GetValue(0);
				}
				else
				{
					if (num2 != 2)
					{
						throw new ArgumentException(Utils.GetResourceString("Argument_UnsupportedArrayDimensions"));
					}
					obj = Value.GetValue(0, 0);
				}
				if (obj == null)
				{
					num = 0;
				}
				else
				{
					num = ((string)obj).Length;
				}
				if (num == 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidFixedLengthString"));
				}
			}
			if (ArrayIsDynamic)
			{
				Value = this.GetArrayDesc(elementType);
				num2 = Value.Rank;
			}
			int upperBound = Value.GetUpperBound(0);
			if (num2 != 1)
			{
				if (num2 != 2)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_UnsupportedArrayDimensions"));
				}
				num3 = Value.GetUpperBound(1);
			}
			if (ArrayIsDynamic)
			{
				this.GetArrayData(Value, elementType, upperBound, num3, num);
			}
			else
			{
				this.GetFixedArray(RecordNumber, ref Value, elementType, upperBound, num3, num);
			}
		}

		internal override void Get(ref bool Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetBoolean(RecordNumber);
		}

		internal override void Get(ref byte Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetByte(RecordNumber);
		}

		internal override void Get(ref short Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetShort(RecordNumber);
		}

		internal override void Get(ref int Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetInteger(RecordNumber);
		}

		internal override void Get(ref long Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetLong(RecordNumber);
		}

		internal override void Get(ref char Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetChar(RecordNumber);
		}

		internal override void Get(ref float Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetSingle(RecordNumber);
		}

		internal override void Get(ref double Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetDouble(RecordNumber);
		}

		internal override void Get(ref decimal Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetCurrency(RecordNumber);
		}

		internal override void Get(ref string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			this.ValidateReadable();
			if (StringIsFixedLength)
			{
				int num;
				if (Value == null)
				{
					num = 0;
				}
				else
				{
					num = this.m_Encoding.GetByteCount(Value);
				}
				Value = this.GetFixedLengthString(RecordNumber, num);
			}
			else
			{
				Value = this.GetLengthPrefixedString(RecordNumber);
			}
		}

		internal override void Get(ref DateTime Value, long RecordNumber = 0L)
		{
			this.ValidateReadable();
			Value = this.GetDate(RecordNumber);
		}

		internal override void PutObject(object Value, long RecordNumber = 0L, bool ContainedInVariant = true)
		{
			this.ValidateWriteable();
			if (Value == null)
			{
				this.PutEmpty(RecordNumber);
				return;
			}
			Type type = Value.GetType();
			if (type == null)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { "Empty" })), 5);
			}
			if (type.IsArray)
			{
				this.PutDynamicArray(RecordNumber, (Array)Value, true, -1);
				return;
			}
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.DBNull:
				this.PutShort(RecordNumber, 1, false);
				return;
			case TypeCode.Boolean:
				this.PutBoolean(RecordNumber, BooleanType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Char:
				this.PutChar(RecordNumber, CharType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Byte:
				this.PutByte(RecordNumber, ByteType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Int16:
				this.PutShort(RecordNumber, ShortType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Int32:
				this.PutInteger(RecordNumber, IntegerType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Int64:
				this.PutLong(RecordNumber, LongType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Single:
				this.PutSingle(RecordNumber, SingleType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Double:
				this.PutDouble(RecordNumber, DoubleType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.Decimal:
				this.PutDecimal(RecordNumber, DecimalType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.DateTime:
				this.PutDate(RecordNumber, DateType.FromObject(Value), ContainedInVariant);
				return;
			case TypeCode.String:
				this.PutVariantString(RecordNumber, Value.ToString());
				return;
			}
			if (type == typeof(Missing))
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { "Missing" })), 5);
			}
			if (type.IsValueType && !ContainedInVariant)
			{
				this.PutRecord(RecordNumber, (ValueType)Value);
				return;
			}
			if (ContainedInVariant && type.IsValueType)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_PutObjectOfValueType1", new string[] { Utils.VBFriendlyName(type, Value) })), 5);
			}
			throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedIOType1", new string[] { Utils.VBFriendlyName(type, Value) })), 5);
		}

		internal override void Put(ValueType Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutRecord(RecordNumber, Value);
		}

		internal override void Put(Array Value, long RecordNumber = 0L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
		{
			this.ValidateWriteable();
			if (Value == null)
			{
				this.PutEmpty(RecordNumber);
				return;
			}
			int upperBound = Value.GetUpperBound(0);
			int num = -1;
			int num2 = -1;
			if (Value.Rank == 2)
			{
				num = Value.GetUpperBound(1);
			}
			if (StringIsFixedLength)
			{
				num2 = 0;
			}
			Type elementType = Value.GetType().GetElementType();
			if (ArrayIsDynamic)
			{
				this.PutDynamicArray(RecordNumber, Value, false, num2);
			}
			else
			{
				this.PutFixedArray(RecordNumber, Value, elementType, num2, upperBound, num);
			}
		}

		internal override void Put(bool Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutBoolean(RecordNumber, Value, false);
		}

		internal override void Put(byte Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutByte(RecordNumber, Value, false);
		}

		internal override void Put(short Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutShort(RecordNumber, Value, false);
		}

		internal override void Put(int Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutInteger(RecordNumber, Value, false);
		}

		internal override void Put(long Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutLong(RecordNumber, Value, false);
		}

		internal override void Put(char Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutChar(RecordNumber, Value, false);
		}

		internal override void Put(float Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutSingle(RecordNumber, Value, false);
		}

		internal override void Put(double Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutDouble(RecordNumber, Value, false);
		}

		internal override void Put(decimal Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutCurrency(RecordNumber, Value, false);
		}

		internal override void Put(string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			this.ValidateWriteable();
			if (StringIsFixedLength)
			{
				this.PutString(RecordNumber, Value);
			}
			else
			{
				this.PutStringWithLength(RecordNumber, Value);
			}
		}

		internal override void Put(DateTime Value, long RecordNumber = 0L)
		{
			this.ValidateWriteable();
			this.PutDate(RecordNumber, Value, false);
		}

		protected void ValidateWriteable()
		{
			if (this.m_access != OpenAccess.ReadWrite && this.m_access != OpenAccess.Write)
			{
				throw ExceptionUtils.VbMakeExceptionEx(75, Utils.GetResourceString("FileOpenedNoWrite"));
			}
		}

		protected void ValidateReadable()
		{
			if (this.m_access != OpenAccess.ReadWrite && this.m_access != OpenAccess.Read)
			{
				throw ExceptionUtils.VbMakeExceptionEx(75, Utils.GetResourceString("FileOpenedNoRead"));
			}
		}
	}
}
