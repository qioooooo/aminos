using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200034B RID: 843
	internal class XmlSerializationPrimitiveReader : XmlSerializationReader
	{
		// Token: 0x060028FB RID: 10491 RVA: 0x000D274C File Offset: 0x000D174C
		internal object Read_string()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id1_string || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				if (base.ReadNull())
				{
					obj = null;
				}
				else
				{
					obj = base.Reader.ReadElementString();
				}
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000D27C4 File Offset: 0x000D17C4
		internal object Read_int()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id3_int || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToInt32(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000D283C File Offset: 0x000D183C
		internal object Read_boolean()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id4_boolean || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToBoolean(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000D28B4 File Offset: 0x000D18B4
		internal object Read_short()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id5_short || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToInt16(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000D292C File Offset: 0x000D192C
		internal object Read_long()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id6_long || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToInt64(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000D29A4 File Offset: 0x000D19A4
		internal object Read_float()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id7_float || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToSingle(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000D2A1C File Offset: 0x000D1A1C
		internal object Read_double()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id8_double || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToDouble(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000D2A94 File Offset: 0x000D1A94
		internal object Read_decimal()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id9_decimal || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToDecimal(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000D2B0C File Offset: 0x000D1B0C
		internal object Read_dateTime()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id10_dateTime || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlSerializationReader.ToDateTime(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000D2B84 File Offset: 0x000D1B84
		internal object Read_unsignedByte()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id11_unsignedByte || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToByte(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000D2BFC File Offset: 0x000D1BFC
		internal object Read_byte()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id12_byte || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToSByte(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000D2C74 File Offset: 0x000D1C74
		internal object Read_unsignedShort()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id13_unsignedShort || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToUInt16(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000D2CEC File Offset: 0x000D1CEC
		internal object Read_unsignedInt()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id14_unsignedInt || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToUInt32(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000D2D64 File Offset: 0x000D1D64
		internal object Read_unsignedLong()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id15_unsignedLong || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToUInt64(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000D2DDC File Offset: 0x000D1DDC
		internal object Read_base64Binary()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id16_base64Binary || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				if (base.ReadNull())
				{
					obj = null;
				}
				else
				{
					obj = base.ToByteArrayBase64(false);
				}
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000D2E50 File Offset: 0x000D1E50
		internal object Read_guid()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id17_guid || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlConvert.ToGuid(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000D2EC8 File Offset: 0x000D1EC8
		internal object Read_char()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id18_char || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				obj = XmlSerializationReader.ToChar(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000D2F40 File Offset: 0x000D1F40
		internal object Read_QName()
		{
			object obj = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id1_QName || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				if (base.ReadNull())
				{
					obj = null;
				}
				else
				{
					obj = base.ReadElementQualifiedName();
				}
			}
			else
			{
				base.UnknownNode(null);
			}
			return obj;
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000D2FB3 File Offset: 0x000D1FB3
		protected override void InitCallbacks()
		{
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000D2FB8 File Offset: 0x000D1FB8
		protected override void InitIDs()
		{
			this.id4_boolean = base.Reader.NameTable.Add("boolean");
			this.id14_unsignedInt = base.Reader.NameTable.Add("unsignedInt");
			this.id15_unsignedLong = base.Reader.NameTable.Add("unsignedLong");
			this.id7_float = base.Reader.NameTable.Add("float");
			this.id10_dateTime = base.Reader.NameTable.Add("dateTime");
			this.id6_long = base.Reader.NameTable.Add("long");
			this.id9_decimal = base.Reader.NameTable.Add("decimal");
			this.id8_double = base.Reader.NameTable.Add("double");
			this.id17_guid = base.Reader.NameTable.Add("guid");
			this.id2_Item = base.Reader.NameTable.Add("");
			this.id13_unsignedShort = base.Reader.NameTable.Add("unsignedShort");
			this.id18_char = base.Reader.NameTable.Add("char");
			this.id3_int = base.Reader.NameTable.Add("int");
			this.id12_byte = base.Reader.NameTable.Add("byte");
			this.id16_base64Binary = base.Reader.NameTable.Add("base64Binary");
			this.id11_unsignedByte = base.Reader.NameTable.Add("unsignedByte");
			this.id5_short = base.Reader.NameTable.Add("short");
			this.id1_string = base.Reader.NameTable.Add("string");
			this.id1_QName = base.Reader.NameTable.Add("QName");
		}

		// Token: 0x040016A6 RID: 5798
		private string id4_boolean;

		// Token: 0x040016A7 RID: 5799
		private string id14_unsignedInt;

		// Token: 0x040016A8 RID: 5800
		private string id15_unsignedLong;

		// Token: 0x040016A9 RID: 5801
		private string id7_float;

		// Token: 0x040016AA RID: 5802
		private string id10_dateTime;

		// Token: 0x040016AB RID: 5803
		private string id6_long;

		// Token: 0x040016AC RID: 5804
		private string id9_decimal;

		// Token: 0x040016AD RID: 5805
		private string id8_double;

		// Token: 0x040016AE RID: 5806
		private string id17_guid;

		// Token: 0x040016AF RID: 5807
		private string id2_Item;

		// Token: 0x040016B0 RID: 5808
		private string id13_unsignedShort;

		// Token: 0x040016B1 RID: 5809
		private string id18_char;

		// Token: 0x040016B2 RID: 5810
		private string id3_int;

		// Token: 0x040016B3 RID: 5811
		private string id12_byte;

		// Token: 0x040016B4 RID: 5812
		private string id16_base64Binary;

		// Token: 0x040016B5 RID: 5813
		private string id11_unsignedByte;

		// Token: 0x040016B6 RID: 5814
		private string id5_short;

		// Token: 0x040016B7 RID: 5815
		private string id1_string;

		// Token: 0x040016B8 RID: 5816
		private string id1_QName;
	}
}
