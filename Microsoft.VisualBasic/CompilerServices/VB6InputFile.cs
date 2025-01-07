using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class VB6InputFile : VB6File
	{
		public VB6InputFile(string FileName, OpenShare share)
			: base(FileName, OpenAccess.Read, share, -1)
		{
		}

		internal override void OpenFile()
		{
			try
			{
				this.m_file = new FileStream(this.m_sFullPath, FileMode.Open, (FileAccess)this.m_access, (FileShare)this.m_share);
			}
			catch (FileNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(ex, 53);
			}
			catch (SecurityException ex2)
			{
				throw ExceptionUtils.VbMakeException(53);
			}
			catch (DirectoryNotFoundException ex3)
			{
				throw ExceptionUtils.VbMakeException(ex3, 76);
			}
			catch (IOException ex4)
			{
				throw ExceptionUtils.VbMakeException(ex4, 75);
			}
			catch (StackOverflowException ex5)
			{
				throw ex5;
			}
			catch (OutOfMemoryException ex6)
			{
				throw ex6;
			}
			catch (ThreadAbortException ex7)
			{
				throw ex7;
			}
			catch (Exception ex8)
			{
				throw ExceptionUtils.VbMakeException(ex8, 76);
			}
			this.m_Encoding = Utils.GetFileIOEncoding();
			this.m_sr = new StreamReader(this.m_file, this.m_Encoding, false, 128);
			this.m_eof = this.m_file.Length == 0L;
		}

		public string ReadLine()
		{
			string text = this.m_sr.ReadLine();
			checked
			{
				this.m_position += unchecked((long)(checked(this.m_Encoding.GetByteCount(text) + 2)));
				return null;
			}
		}

		internal override bool CanInput()
		{
			return true;
		}

		internal override bool EOF()
		{
			return this.m_eof;
		}

		public override OpenMode GetMode()
		{
			return OpenMode.Input;
		}

		internal object ParseInputString(ref string sInput)
		{
			object obj = sInput;
			if (sInput[0] == '#' && sInput.Length != 1)
			{
				sInput = sInput.Substring(1, checked(sInput.Length - 2));
				if (Operators.CompareString(sInput, "NULL", false) == 0)
				{
					obj = DBNull.Value;
				}
				else if (Operators.CompareString(sInput, "TRUE", false) == 0)
				{
					obj = true;
				}
				else if (Operators.CompareString(sInput, "FALSE", false) == 0)
				{
					obj = false;
				}
				else if (Operators.CompareString(Strings.Left(sInput, 6), "ERROR ", false) == 0)
				{
					int num;
					if (sInput.Length > 6)
					{
						num = IntegerType.FromString(Strings.Mid(sInput, 7));
					}
					obj = num;
				}
				else
				{
					try
					{
						obj = DateTime.Parse(Utils.ToHalfwidthNumbers(sInput, Utils.GetCultureInfo()));
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
				}
			}
			return obj;
		}

		internal override void Input(ref object obj)
		{
			int num = this.SkipWhiteSpaceEOF();
			checked
			{
				if (num == 34)
				{
					num = this.m_sr.Read();
					this.m_position += 1L;
					obj = this.ReadInField(1);
					this.SkipTrailingWhiteSpace();
				}
				else if (num == 35)
				{
					string text = this.InputStr();
					obj = this.ParseInputString(ref text);
				}
				else
				{
					string text2 = this.ReadInField(3);
					obj = Conversion.ParseInputField(text2, VariantType.Empty);
					this.SkipTrailingWhiteSpace();
				}
			}
		}

		internal override void Input(ref bool Value)
		{
			string text = this.InputStr();
			Value = BooleanType.FromObject(this.ParseInputString(ref text));
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

		internal override void Input(ref float Value)
		{
			Value = SingleType.FromObject(this.InputNum(VariantType.Single), Utils.GetInvariantCultureInfo().NumberFormat);
		}

		internal override void Input(ref double Value)
		{
			Value = DoubleType.FromObject(this.InputNum(VariantType.Double), Utils.GetInvariantCultureInfo().NumberFormat);
		}

		internal override void Input(ref decimal Value)
		{
			Value = DecimalType.FromObject(this.InputNum(VariantType.Decimal), Utils.GetInvariantCultureInfo().NumberFormat);
		}

		internal override void Input(ref string Value)
		{
			Value = this.InputStr();
		}

		internal override void Input(ref DateTime Value)
		{
			string text = this.InputStr();
			Value = DateType.FromObject(this.ParseInputString(ref text));
		}

		internal override long LOC()
		{
			return checked(this.m_position + 127L) / 128L;
		}
	}
}
