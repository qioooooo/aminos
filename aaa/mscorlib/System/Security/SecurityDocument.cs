using System;
using System.Collections;
using System.Security.Util;
using System.Text;

namespace System.Security
{
	// Token: 0x020005FD RID: 1533
	[Serializable]
	internal sealed class SecurityDocument
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x000BBF1A File Offset: 0x000BAF1A
		public SecurityDocument(int numData)
		{
			this.m_data = new byte[numData];
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000BBF2E File Offset: 0x000BAF2E
		public SecurityDocument(byte[] data)
		{
			this.m_data = data;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000BBF40 File Offset: 0x000BAF40
		public SecurityDocument(SecurityElement elRoot)
		{
			this.m_data = new byte[32];
			int num = 0;
			this.ConvertElement(elRoot, ref num);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000BBF6C File Offset: 0x000BAF6C
		public void GuaranteeSize(int size)
		{
			if (this.m_data.Length < size)
			{
				byte[] array = new byte[(size / 32 + 1) * 32];
				Array.Copy(this.m_data, 0, array, 0, this.m_data.Length);
				this.m_data = array;
			}
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000BBFB0 File Offset: 0x000BAFB0
		public void AddString(string str, ref int position)
		{
			this.GuaranteeSize(position + str.Length * 2 + 2);
			for (int i = 0; i < str.Length; i++)
			{
				this.m_data[position + 2 * i] = (byte)(str[i] >> 8);
				this.m_data[position + 2 * i + 1] = (byte)(str[i] & 'ÿ');
			}
			this.m_data[position + str.Length * 2] = 0;
			this.m_data[position + str.Length * 2 + 1] = 0;
			position += str.Length * 2 + 2;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000BC04C File Offset: 0x000BB04C
		public void AppendString(string str, ref int position)
		{
			if (position <= 1 || this.m_data[position - 1] != 0 || this.m_data[position - 2] != 0)
			{
				throw new XmlSyntaxException();
			}
			position -= 2;
			this.AddString(str, ref position);
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000BC081 File Offset: 0x000BB081
		public static int EncodedStringSize(string str)
		{
			return str.Length * 2 + 2;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000BC08D File Offset: 0x000BB08D
		public string GetString(ref int position)
		{
			return this.GetString(ref position, true);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000BC098 File Offset: 0x000BB098
		public string GetString(ref int position, bool bCreate)
		{
			int num = position;
			while (num < this.m_data.Length - 1 && (this.m_data[num] != 0 || this.m_data[num + 1] != 0))
			{
				num += 2;
			}
			Tokenizer.StringMaker sharedStringMaker = SharedStatics.GetSharedStringMaker();
			string text;
			try
			{
				if (bCreate)
				{
					sharedStringMaker._outStringBuilder = null;
					sharedStringMaker._outIndex = 0;
					for (int i = position; i < num; i += 2)
					{
						char c = (char)(((int)this.m_data[i] << 8) | (int)this.m_data[i + 1]);
						if (sharedStringMaker._outIndex < 512)
						{
							sharedStringMaker._outChars[sharedStringMaker._outIndex++] = c;
						}
						else
						{
							if (sharedStringMaker._outStringBuilder == null)
							{
								sharedStringMaker._outStringBuilder = new StringBuilder();
							}
							sharedStringMaker._outStringBuilder.Append(sharedStringMaker._outChars, 0, 512);
							sharedStringMaker._outChars[0] = c;
							sharedStringMaker._outIndex = 1;
						}
					}
				}
				position = num + 2;
				if (bCreate)
				{
					text = sharedStringMaker.MakeString();
				}
				else
				{
					text = null;
				}
			}
			finally
			{
				SharedStatics.ReleaseSharedStringMaker(ref sharedStringMaker);
			}
			return text;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000BC1AC File Offset: 0x000BB1AC
		public void AddToken(byte b, ref int position)
		{
			this.GuaranteeSize(position + 1);
			this.m_data[position++] = b;
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000BC1D4 File Offset: 0x000BB1D4
		public void ConvertElement(SecurityElement elCurrent, ref int position)
		{
			this.AddToken(1, ref position);
			this.AddString(elCurrent.m_strTag, ref position);
			if (elCurrent.m_lAttributes != null)
			{
				for (int i = 0; i < elCurrent.m_lAttributes.Count; i += 2)
				{
					this.AddToken(2, ref position);
					this.AddString((string)elCurrent.m_lAttributes[i], ref position);
					this.AddString((string)elCurrent.m_lAttributes[i + 1], ref position);
				}
			}
			if (elCurrent.m_strText != null)
			{
				this.AddToken(3, ref position);
				this.AddString(elCurrent.m_strText, ref position);
			}
			if (elCurrent.InternalChildren != null)
			{
				for (int j = 0; j < elCurrent.InternalChildren.Count; j++)
				{
					this.ConvertElement((SecurityElement)elCurrent.Children[j], ref position);
				}
			}
			this.AddToken(4, ref position);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000BC2A9 File Offset: 0x000BB2A9
		public SecurityElement GetRootElement()
		{
			return this.GetElement(0, true);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000BC2B4 File Offset: 0x000BB2B4
		public SecurityElement GetElement(int position, bool bCreate)
		{
			return this.InternalGetElement(ref position, bCreate);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000BC2CC File Offset: 0x000BB2CC
		internal SecurityElement InternalGetElement(ref int position, bool bCreate)
		{
			if (this.m_data.Length <= position)
			{
				throw new XmlSyntaxException();
			}
			if (this.m_data[position++] != 1)
			{
				throw new XmlSyntaxException();
			}
			SecurityElement securityElement = null;
			string @string = this.GetString(ref position, bCreate);
			if (bCreate)
			{
				securityElement = new SecurityElement(@string);
			}
			while (this.m_data[position] == 2)
			{
				position++;
				string string2 = this.GetString(ref position, bCreate);
				string string3 = this.GetString(ref position, bCreate);
				if (bCreate)
				{
					securityElement.AddAttribute(string2, string3);
				}
			}
			if (this.m_data[position] == 3)
			{
				position++;
				string string4 = this.GetString(ref position, bCreate);
				if (bCreate)
				{
					securityElement.m_strText = string4;
				}
			}
			while (this.m_data[position] != 4)
			{
				SecurityElement securityElement2 = this.InternalGetElement(ref position, bCreate);
				if (bCreate)
				{
					securityElement.AddChild(securityElement2);
				}
			}
			position++;
			return securityElement;
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000BC3A0 File Offset: 0x000BB3A0
		public string GetTagForElement(int position)
		{
			if (this.m_data.Length <= position)
			{
				throw new XmlSyntaxException();
			}
			if (this.m_data[position++] != 1)
			{
				throw new XmlSyntaxException();
			}
			return this.GetString(ref position);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000BC3E0 File Offset: 0x000BB3E0
		public ArrayList GetChildrenPositionForElement(int position)
		{
			if (this.m_data.Length <= position)
			{
				throw new XmlSyntaxException();
			}
			if (this.m_data[position++] != 1)
			{
				throw new XmlSyntaxException();
			}
			ArrayList arrayList = new ArrayList();
			this.GetString(ref position);
			while (this.m_data[position] == 2)
			{
				position++;
				this.GetString(ref position, false);
				this.GetString(ref position, false);
			}
			if (this.m_data[position] == 3)
			{
				position++;
				this.GetString(ref position, false);
			}
			while (this.m_data[position] != 4)
			{
				arrayList.Add(position);
				this.InternalGetElement(ref position, false);
			}
			position++;
			return arrayList;
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x000BC490 File Offset: 0x000BB490
		public string GetAttributeForElement(int position, string attributeName)
		{
			if (this.m_data.Length <= position)
			{
				throw new XmlSyntaxException();
			}
			if (this.m_data[position++] != 1)
			{
				throw new XmlSyntaxException();
			}
			string text = null;
			this.GetString(ref position, false);
			while (this.m_data[position] == 2)
			{
				position++;
				string @string = this.GetString(ref position);
				string string2 = this.GetString(ref position);
				if (string.Equals(@string, attributeName))
				{
					text = string2;
					break;
				}
			}
			return text;
		}

		// Token: 0x04001CC8 RID: 7368
		internal const byte c_element = 1;

		// Token: 0x04001CC9 RID: 7369
		internal const byte c_attribute = 2;

		// Token: 0x04001CCA RID: 7370
		internal const byte c_text = 3;

		// Token: 0x04001CCB RID: 7371
		internal const byte c_children = 4;

		// Token: 0x04001CCC RID: 7372
		internal const int c_growthSize = 32;

		// Token: 0x04001CCD RID: 7373
		internal byte[] m_data;
	}
}
