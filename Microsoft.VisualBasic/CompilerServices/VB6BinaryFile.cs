using System;
using System.ComponentModel;
using System.IO;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class VB6BinaryFile : VB6RandomFile
	{
		public VB6BinaryFile(string FileName, OpenAccess access, OpenShare share)
			: base(FileName, access, share, -1)
		{
		}

		internal override void Lock(long lStart, long lEnd)
		{
			if (lStart > lEnd)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
			}
			long num;
			if (this.m_lRecordLen == -1)
			{
				num = 1L;
			}
			else
			{
				num = (long)this.m_lRecordLen;
			}
			checked
			{
				long num2 = (lStart - 1L) * num;
				long num3 = (lEnd - lStart + 1L) * num;
				this.m_file.Lock(num2, num3);
			}
		}

		internal override void Unlock(long lStart, long lEnd)
		{
			if (lStart > lEnd)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
			}
			long num;
			if (this.m_lRecordLen == -1)
			{
				num = 1L;
			}
			else
			{
				num = (long)this.m_lRecordLen;
			}
			checked
			{
				long num2 = (lStart - 1L) * num;
				long num3 = (lEnd - lStart + 1L) * num;
				this.m_file.Unlock(num2, num3);
			}
		}

		public override OpenMode GetMode()
		{
			return OpenMode.Binary;
		}

		internal override long Seek()
		{
			return checked(this.m_position + 1L);
		}

		internal override void Seek(long BaseOnePosition)
		{
			if (BaseOnePosition <= 0L)
			{
				throw ExceptionUtils.VbMakeException(63);
			}
			long num = checked(BaseOnePosition - 1L);
			this.m_file.Position = num;
			this.m_position = num;
			if (this.m_sr != null)
			{
				this.m_sr.DiscardBufferedData();
			}
		}

		internal override long LOC()
		{
			return this.m_position;
		}

		internal override bool CanInput()
		{
			return true;
		}

		internal override bool CanWrite()
		{
			return true;
		}

		internal override void Input(ref object Value)
		{
			Value = this.InputStr();
		}

		internal override void Input(ref string Value)
		{
			Value = this.InputStr();
		}

		internal override void Input(ref char Value)
		{
			string text = this.InputStr();
			if (text.Length > 0)
			{
				Value = text[0];
			}
			else
			{
				Value = '\0';
			}
		}

		internal override void Input(ref bool Value)
		{
			Value = BooleanType.FromString(this.InputStr());
		}

		internal override void Input(ref byte Value)
		{
			Value = ByteType.FromObject(this.InputNum(VariantType.Byte));
		}

		internal override void Input(ref short Value)
		{
			Value = ShortType.FromObject(this.InputNum(VariantType.Short));
		}

		internal override void Input(ref int Value)
		{
			Value = IntegerType.FromObject(this.InputNum(VariantType.Integer));
		}

		internal override void Input(ref long Value)
		{
			Value = LongType.FromObject(this.InputNum(VariantType.Long));
		}

		internal override void Input(ref float Value)
		{
			Value = SingleType.FromObject(this.InputNum(VariantType.Single));
		}

		internal override void Input(ref double Value)
		{
			Value = DoubleType.FromObject(this.InputNum(VariantType.Double));
		}

		internal override void Input(ref decimal Value)
		{
			Value = DecimalType.FromObject(this.InputNum(VariantType.Decimal));
		}

		internal override void Input(ref DateTime Value)
		{
			Value = DateType.FromString(this.InputStr(), Utils.GetCultureInfo());
		}

		internal override void Put(string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			this.ValidateWriteable();
			this.PutString(RecordNumber, Value);
		}

		internal override void Get(ref string Value, long RecordNumber = 0L, bool StringIsFixedLength = false)
		{
			this.ValidateReadable();
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

		protected override string InputStr()
		{
			if (this.m_access != OpenAccess.ReadWrite && this.m_access != OpenAccess.Read)
			{
				NullReferenceException ex = new NullReferenceException();
				throw new NullReferenceException(ex.Message, new IOException(Utils.GetResourceString("FileOpenedNoRead")));
			}
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
	}
}
