using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.VisualBasic.FileIO
{
	internal class QuoteDelimitedFieldBuilder
	{
		public QuoteDelimitedFieldBuilder(Regex DelimiterRegex, string SpaceChars)
		{
			this.m_Field = new StringBuilder();
			this.m_DelimiterRegex = DelimiterRegex;
			this.m_SpaceChars = SpaceChars;
		}

		public bool FieldFinished
		{
			get
			{
				return this.m_FieldFinished;
			}
		}

		public string Field
		{
			get
			{
				return this.m_Field.ToString();
			}
		}

		public int Index
		{
			get
			{
				return this.m_Index;
			}
		}

		public int DelimiterLength
		{
			get
			{
				return this.m_DelimiterLength;
			}
		}

		public bool MalformedLine
		{
			get
			{
				return this.m_MalformedLine;
			}
		}

		public void BuildField(string Line, int StartAt)
		{
			this.m_Index = StartAt;
			int length = Line.Length;
			checked
			{
				while (this.m_Index < length)
				{
					if (Line[this.m_Index] == '"')
					{
						if (this.m_Index + 1 == length)
						{
							this.m_FieldFinished = true;
							this.m_DelimiterLength = 1;
							this.m_Index++;
							return;
						}
						if (!((this.m_Index + 1 < Line.Length) & (Line[this.m_Index + 1] == '"')))
						{
							Match match = this.m_DelimiterRegex.Match(Line, this.m_Index + 1);
							int num;
							if (!match.Success)
							{
								num = length - 1;
							}
							else
							{
								num = match.Index - 1;
							}
							int num2 = this.m_Index + 1;
							int num3 = num;
							for (int i = num2; i <= num3; i++)
							{
								if (this.m_SpaceChars.IndexOf(Line[i]) < 0)
								{
									this.m_MalformedLine = true;
									return;
								}
							}
							this.m_DelimiterLength = 1 + num - this.m_Index;
							if (match.Success)
							{
								this.m_DelimiterLength += match.Length;
							}
							this.m_FieldFinished = true;
							return;
						}
						this.m_Field.Append('"');
						this.m_Index += 2;
					}
					else
					{
						this.m_Field.Append(Line[this.m_Index]);
						this.m_Index++;
					}
				}
			}
		}

		private StringBuilder m_Field;

		private bool m_FieldFinished;

		private int m_Index;

		private int m_DelimiterLength;

		private Regex m_DelimiterRegex;

		private string m_SpaceChars;

		private bool m_MalformedLine;
	}
}
