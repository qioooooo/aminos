using System;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000F9 RID: 249
	internal sealed class DataTextReader : XmlReader
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x0020B2C4 File Offset: 0x0020A6C4
		internal static XmlReader CreateReader(XmlReader xr)
		{
			return new DataTextReader(xr);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0020B2D8 File Offset: 0x0020A6D8
		private DataTextReader(XmlReader input)
		{
			this._xmlreader = input;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0020B2F4 File Offset: 0x0020A6F4
		public override XmlReaderSettings Settings
		{
			get
			{
				return this._xmlreader.Settings;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0020B30C File Offset: 0x0020A70C
		public override XmlNodeType NodeType
		{
			get
			{
				return this._xmlreader.NodeType;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0020B324 File Offset: 0x0020A724
		public override string Name
		{
			get
			{
				return this._xmlreader.Name;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0020B33C File Offset: 0x0020A73C
		public override string LocalName
		{
			get
			{
				return this._xmlreader.LocalName;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0020B354 File Offset: 0x0020A754
		public override string NamespaceURI
		{
			get
			{
				return this._xmlreader.NamespaceURI;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0020B36C File Offset: 0x0020A76C
		public override string Prefix
		{
			get
			{
				return this._xmlreader.Prefix;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0020B384 File Offset: 0x0020A784
		public override bool HasValue
		{
			get
			{
				return this._xmlreader.HasValue;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0020B39C File Offset: 0x0020A79C
		public override string Value
		{
			get
			{
				return this._xmlreader.Value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0020B3B4 File Offset: 0x0020A7B4
		public override int Depth
		{
			get
			{
				return this._xmlreader.Depth;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0020B3CC File Offset: 0x0020A7CC
		public override string BaseURI
		{
			get
			{
				return this._xmlreader.BaseURI;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0020B3E4 File Offset: 0x0020A7E4
		public override bool IsEmptyElement
		{
			get
			{
				return this._xmlreader.IsEmptyElement;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0020B3FC File Offset: 0x0020A7FC
		public override bool IsDefault
		{
			get
			{
				return this._xmlreader.IsDefault;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0020B414 File Offset: 0x0020A814
		public override char QuoteChar
		{
			get
			{
				return this._xmlreader.QuoteChar;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0020B42C File Offset: 0x0020A82C
		public override XmlSpace XmlSpace
		{
			get
			{
				return this._xmlreader.XmlSpace;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0020B444 File Offset: 0x0020A844
		public override string XmlLang
		{
			get
			{
				return this._xmlreader.XmlLang;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0020B45C File Offset: 0x0020A85C
		public override int AttributeCount
		{
			get
			{
				return this._xmlreader.AttributeCount;
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0020B474 File Offset: 0x0020A874
		public override string GetAttribute(string name)
		{
			return this._xmlreader.GetAttribute(name);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0020B490 File Offset: 0x0020A890
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this._xmlreader.GetAttribute(localName, namespaceURI);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0020B4AC File Offset: 0x0020A8AC
		public override string GetAttribute(int i)
		{
			return this._xmlreader.GetAttribute(i);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0020B4C8 File Offset: 0x0020A8C8
		public override bool MoveToAttribute(string name)
		{
			return this._xmlreader.MoveToAttribute(name);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0020B4E4 File Offset: 0x0020A8E4
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this._xmlreader.MoveToAttribute(localName, namespaceURI);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0020B500 File Offset: 0x0020A900
		public override void MoveToAttribute(int i)
		{
			this._xmlreader.MoveToAttribute(i);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0020B51C File Offset: 0x0020A91C
		public override bool MoveToFirstAttribute()
		{
			return this._xmlreader.MoveToFirstAttribute();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0020B534 File Offset: 0x0020A934
		public override bool MoveToNextAttribute()
		{
			return this._xmlreader.MoveToNextAttribute();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0020B54C File Offset: 0x0020A94C
		public override bool MoveToElement()
		{
			return this._xmlreader.MoveToElement();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0020B564 File Offset: 0x0020A964
		public override bool ReadAttributeValue()
		{
			return this._xmlreader.ReadAttributeValue();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0020B57C File Offset: 0x0020A97C
		public override bool Read()
		{
			return this._xmlreader.Read();
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0020B594 File Offset: 0x0020A994
		public override bool EOF
		{
			get
			{
				return this._xmlreader.EOF;
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0020B5AC File Offset: 0x0020A9AC
		public override void Close()
		{
			this._xmlreader.Close();
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0020B5C4 File Offset: 0x0020A9C4
		public override ReadState ReadState
		{
			get
			{
				return this._xmlreader.ReadState;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0020B5DC File Offset: 0x0020A9DC
		public override void Skip()
		{
			this._xmlreader.Skip();
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0020B5F4 File Offset: 0x0020A9F4
		public override XmlNameTable NameTable
		{
			get
			{
				return this._xmlreader.NameTable;
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0020B60C File Offset: 0x0020AA0C
		public override string LookupNamespace(string prefix)
		{
			return this._xmlreader.LookupNamespace(prefix);
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0020B628 File Offset: 0x0020AA28
		public override bool CanResolveEntity
		{
			get
			{
				return this._xmlreader.CanResolveEntity;
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0020B640 File Offset: 0x0020AA40
		public override void ResolveEntity()
		{
			this._xmlreader.ResolveEntity();
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0020B658 File Offset: 0x0020AA58
		public override bool CanReadBinaryContent
		{
			get
			{
				return this._xmlreader.CanReadBinaryContent;
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0020B670 File Offset: 0x0020AA70
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			return this._xmlreader.ReadContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0020B68C File Offset: 0x0020AA8C
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			return this._xmlreader.ReadElementContentAsBase64(buffer, index, count);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0020B6A8 File Offset: 0x0020AAA8
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this._xmlreader.ReadContentAsBinHex(buffer, index, count);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0020B6C4 File Offset: 0x0020AAC4
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			return this._xmlreader.ReadElementContentAsBinHex(buffer, index, count);
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0020B6E0 File Offset: 0x0020AAE0
		public override bool CanReadValueChunk
		{
			get
			{
				return this._xmlreader.CanReadValueChunk;
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0020B6F8 File Offset: 0x0020AAF8
		public override string ReadString()
		{
			return this._xmlreader.ReadString();
		}

		// Token: 0x04000A80 RID: 2688
		private XmlReader _xmlreader;
	}
}
