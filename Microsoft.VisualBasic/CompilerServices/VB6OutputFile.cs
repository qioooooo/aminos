using System;
using System.ComponentModel;
using System.IO;
using System.Security;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class VB6OutputFile : VB6File
	{
		public VB6OutputFile()
		{
		}

		public VB6OutputFile(string FileName, OpenShare share, bool fAppend)
			: base(FileName, OpenAccess.Write, share, -1)
		{
			this.m_fAppend = fAppend;
		}

		internal override void OpenFile()
		{
			try
			{
				if (this.m_fAppend)
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
				else
				{
					this.m_file = new FileStream(this.m_sFullPath, FileMode.Create, (FileAccess)this.m_access, (FileShare)this.m_share);
				}
			}
			catch (FileNotFoundException ex)
			{
				throw ExceptionUtils.VbMakeException(ex, 53);
			}
			catch (SecurityException ex2)
			{
				throw ExceptionUtils.VbMakeException(ex2, 53);
			}
			catch (DirectoryNotFoundException ex3)
			{
				throw ExceptionUtils.VbMakeException(ex3, 76);
			}
			catch (IOException ex4)
			{
				throw ExceptionUtils.VbMakeException(ex4, 75);
			}
			this.m_Encoding = Utils.GetFileIOEncoding();
			this.m_sw = new StreamWriter(this.m_file, this.m_Encoding);
			this.m_sw.AutoFlush = true;
			if (this.m_fAppend)
			{
				long length = this.m_file.Length;
				this.m_file.Position = length;
				this.m_position = length;
			}
		}

		internal override void WriteLine(string s)
		{
			checked
			{
				if (s == null)
				{
					this.m_sw.WriteLine();
					this.m_position += 2L;
				}
				else
				{
					if (this.m_bPrint && this.m_lWidth != 0 && this.m_lCurrentColumn >= this.m_lWidth)
					{
						this.m_sw.WriteLine();
						this.m_position += 2L;
					}
					this.m_sw.WriteLine(s);
					this.m_position += unchecked((long)(checked(this.m_Encoding.GetByteCount(s) + 2)));
				}
				this.m_lCurrentColumn = 0;
			}
		}

		internal override void WriteString(string s)
		{
			checked
			{
				if (s != null)
				{
					if (s.Length == 0)
					{
						return;
					}
					if (this.m_bPrint && this.m_lWidth != 0 && (this.m_lCurrentColumn >= this.m_lWidth || (this.m_lCurrentColumn != 0 && this.m_lCurrentColumn + s.Length > this.m_lWidth)))
					{
						this.m_sw.WriteLine();
						this.m_position += 2L;
						this.m_lCurrentColumn = 0;
					}
					this.m_sw.Write(s);
					int byteCount = this.m_Encoding.GetByteCount(s);
					this.m_position += unchecked((long)byteCount);
					this.m_lCurrentColumn += s.Length;
				}
			}
		}

		internal override bool CanWrite()
		{
			return true;
		}

		public override OpenMode GetMode()
		{
			OpenMode openMode;
			if (this.m_fAppend)
			{
				openMode = OpenMode.Append;
			}
			else
			{
				openMode = OpenMode.Output;
			}
			return openMode;
		}

		internal override bool EOF()
		{
			return true;
		}

		internal override long LOC()
		{
			return checked(this.m_position + 127L) / 128L;
		}
	}
}
