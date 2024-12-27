using System;
using System.IO;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000F8 RID: 248
	internal sealed class DataTextWriter : XmlWriter
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x0020AF30 File Offset: 0x0020A330
		internal static XmlWriter CreateWriter(XmlWriter xw)
		{
			return new DataTextWriter(xw);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0020AF44 File Offset: 0x0020A344
		private DataTextWriter(XmlWriter w)
		{
			this._xmltextWriter = w;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0020AF60 File Offset: 0x0020A360
		internal Stream BaseStream
		{
			get
			{
				XmlTextWriter xmlTextWriter = this._xmltextWriter as XmlTextWriter;
				if (xmlTextWriter != null)
				{
					return xmlTextWriter.BaseStream;
				}
				return null;
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0020AF84 File Offset: 0x0020A384
		public override void WriteStartDocument()
		{
			this._xmltextWriter.WriteStartDocument();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0020AF9C File Offset: 0x0020A39C
		public override void WriteStartDocument(bool standalone)
		{
			this._xmltextWriter.WriteStartDocument(standalone);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0020AFB8 File Offset: 0x0020A3B8
		public override void WriteEndDocument()
		{
			this._xmltextWriter.WriteEndDocument();
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0020AFD0 File Offset: 0x0020A3D0
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this._xmltextWriter.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0020AFF0 File Offset: 0x0020A3F0
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this._xmltextWriter.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0020B00C File Offset: 0x0020A40C
		public override void WriteEndElement()
		{
			this._xmltextWriter.WriteEndElement();
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0020B024 File Offset: 0x0020A424
		public override void WriteFullEndElement()
		{
			this._xmltextWriter.WriteFullEndElement();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0020B03C File Offset: 0x0020A43C
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this._xmltextWriter.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0020B058 File Offset: 0x0020A458
		public override void WriteEndAttribute()
		{
			this._xmltextWriter.WriteEndAttribute();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0020B070 File Offset: 0x0020A470
		public override void WriteCData(string text)
		{
			this._xmltextWriter.WriteCData(text);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0020B08C File Offset: 0x0020A48C
		public override void WriteComment(string text)
		{
			this._xmltextWriter.WriteComment(text);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0020B0A8 File Offset: 0x0020A4A8
		public override void WriteProcessingInstruction(string name, string text)
		{
			this._xmltextWriter.WriteProcessingInstruction(name, text);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0020B0C4 File Offset: 0x0020A4C4
		public override void WriteEntityRef(string name)
		{
			this._xmltextWriter.WriteEntityRef(name);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0020B0E0 File Offset: 0x0020A4E0
		public override void WriteCharEntity(char ch)
		{
			this._xmltextWriter.WriteCharEntity(ch);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0020B0FC File Offset: 0x0020A4FC
		public override void WriteWhitespace(string ws)
		{
			this._xmltextWriter.WriteWhitespace(ws);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0020B118 File Offset: 0x0020A518
		public override void WriteString(string text)
		{
			this._xmltextWriter.WriteString(text);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0020B134 File Offset: 0x0020A534
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this._xmltextWriter.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0020B150 File Offset: 0x0020A550
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this._xmltextWriter.WriteChars(buffer, index, count);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0020B16C File Offset: 0x0020A56C
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this._xmltextWriter.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0020B188 File Offset: 0x0020A588
		public override void WriteRaw(string data)
		{
			this._xmltextWriter.WriteRaw(data);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0020B1A4 File Offset: 0x0020A5A4
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this._xmltextWriter.WriteBase64(buffer, index, count);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0020B1C0 File Offset: 0x0020A5C0
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this._xmltextWriter.WriteBinHex(buffer, index, count);
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0020B1DC File Offset: 0x0020A5DC
		public override WriteState WriteState
		{
			get
			{
				return this._xmltextWriter.WriteState;
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0020B1F4 File Offset: 0x0020A5F4
		public override void Close()
		{
			this._xmltextWriter.Close();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0020B20C File Offset: 0x0020A60C
		public override void Flush()
		{
			this._xmltextWriter.Flush();
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0020B224 File Offset: 0x0020A624
		public override void WriteName(string name)
		{
			this._xmltextWriter.WriteName(name);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0020B240 File Offset: 0x0020A640
		public override void WriteQualifiedName(string localName, string ns)
		{
			this._xmltextWriter.WriteQualifiedName(localName, ns);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0020B25C File Offset: 0x0020A65C
		public override string LookupPrefix(string ns)
		{
			return this._xmltextWriter.LookupPrefix(ns);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0020B278 File Offset: 0x0020A678
		public override XmlSpace XmlSpace
		{
			get
			{
				return this._xmltextWriter.XmlSpace;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0020B290 File Offset: 0x0020A690
		public override string XmlLang
		{
			get
			{
				return this._xmltextWriter.XmlLang;
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0020B2A8 File Offset: 0x0020A6A8
		public override void WriteNmToken(string name)
		{
			this._xmltextWriter.WriteNmToken(name);
		}

		// Token: 0x04000A7F RID: 2687
		private XmlWriter _xmltextWriter;
	}
}
