using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000EA RID: 234
	public class XmlNodeReader : XmlReader, IXmlNamespaceResolver
	{
		// Token: 0x06000E3D RID: 3645 RVA: 0x0003F9AB File Offset: 0x0003E9AB
		public XmlNodeReader(XmlNode node)
		{
			this.Init(node);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0003F9BA File Offset: 0x0003E9BA
		private void Init(XmlNode node)
		{
			this.readerNav = new XmlNodeReaderNavigator(node);
			this.curDepth = 0;
			this.readState = ReadState.Initial;
			this.fEOF = false;
			this.nodeType = XmlNodeType.None;
			this.bResolveEntity = false;
			this.bStartFromDocument = false;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0003F9F2 File Offset: 0x0003E9F2
		internal bool IsInReadingStates()
		{
			return this.readState == ReadState.Interactive;
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0003F9FD File Offset: 0x0003E9FD
		public override XmlNodeType NodeType
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return XmlNodeType.None;
				}
				return this.nodeType;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0003FA0F File Offset: 0x0003EA0F
		public override string Name
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.Name;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003FA2A File Offset: 0x0003EA2A
		public override string LocalName
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.LocalName;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0003FA45 File Offset: 0x0003EA45
		public override string NamespaceURI
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.NamespaceURI;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x0003FA60 File Offset: 0x0003EA60
		public override string Prefix
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.Prefix;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0003FA7B File Offset: 0x0003EA7B
		public override bool HasValue
		{
			get
			{
				return this.IsInReadingStates() && this.readerNav.HasValue;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0003FA92 File Offset: 0x0003EA92
		public override string Value
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.Value;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0003FAAD File Offset: 0x0003EAAD
		public override int Depth
		{
			get
			{
				return this.curDepth;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0003FAB5 File Offset: 0x0003EAB5
		public override string BaseURI
		{
			get
			{
				return this.readerNav.BaseURI;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0003FAC2 File Offset: 0x0003EAC2
		public override bool CanResolveEntity
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0003FAC5 File Offset: 0x0003EAC5
		public override bool IsEmptyElement
		{
			get
			{
				return this.IsInReadingStates() && this.readerNav.IsEmptyElement;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0003FADC File Offset: 0x0003EADC
		public override bool IsDefault
		{
			get
			{
				return this.IsInReadingStates() && this.readerNav.IsDefault;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0003FAF3 File Offset: 0x0003EAF3
		public override XmlSpace XmlSpace
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return XmlSpace.None;
				}
				return this.readerNav.XmlSpace;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0003FB0A File Offset: 0x0003EB0A
		public override string XmlLang
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return string.Empty;
				}
				return this.readerNav.XmlLang;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0003FB25 File Offset: 0x0003EB25
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				if (!this.IsInReadingStates())
				{
					return null;
				}
				return this.readerNav.SchemaInfo;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0003FB3C File Offset: 0x0003EB3C
		public override int AttributeCount
		{
			get
			{
				if (!this.IsInReadingStates() || this.nodeType == XmlNodeType.EndElement)
				{
					return 0;
				}
				return this.readerNav.AttributeCount;
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0003FB5D File Offset: 0x0003EB5D
		public override string GetAttribute(string name)
		{
			if (!this.IsInReadingStates())
			{
				return null;
			}
			return this.readerNav.GetAttribute(name);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0003FB78 File Offset: 0x0003EB78
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (!this.IsInReadingStates())
			{
				return null;
			}
			string text = ((namespaceURI == null) ? string.Empty : namespaceURI);
			return this.readerNav.GetAttribute(name, text);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0003FBA8 File Offset: 0x0003EBA8
		public override string GetAttribute(int attributeIndex)
		{
			if (!this.IsInReadingStates())
			{
				throw new ArgumentOutOfRangeException("attributeIndex");
			}
			return this.readerNav.GetAttribute(attributeIndex);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0003FBCC File Offset: 0x0003EBCC
		public override bool MoveToAttribute(string name)
		{
			if (!this.IsInReadingStates())
			{
				return false;
			}
			this.readerNav.ResetMove(ref this.curDepth, ref this.nodeType);
			if (this.readerNav.MoveToAttribute(name))
			{
				this.curDepth++;
				this.nodeType = this.readerNav.NodeType;
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
				return true;
			}
			this.readerNav.RollBackMove(ref this.curDepth);
			return false;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0003FC4C File Offset: 0x0003EC4C
		public override bool MoveToAttribute(string name, string namespaceURI)
		{
			if (!this.IsInReadingStates())
			{
				return false;
			}
			this.readerNav.ResetMove(ref this.curDepth, ref this.nodeType);
			string text = ((namespaceURI == null) ? string.Empty : namespaceURI);
			if (this.readerNav.MoveToAttribute(name, text))
			{
				this.curDepth++;
				this.nodeType = this.readerNav.NodeType;
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
				return true;
			}
			this.readerNav.RollBackMove(ref this.curDepth);
			return false;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0003FCD8 File Offset: 0x0003ECD8
		public override void MoveToAttribute(int attributeIndex)
		{
			if (!this.IsInReadingStates())
			{
				throw new ArgumentOutOfRangeException("attributeIndex");
			}
			this.readerNav.ResetMove(ref this.curDepth, ref this.nodeType);
			try
			{
				if (this.AttributeCount <= 0)
				{
					throw new ArgumentOutOfRangeException("attributeIndex");
				}
				this.readerNav.MoveToAttribute(attributeIndex);
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
			}
			catch
			{
				this.readerNav.RollBackMove(ref this.curDepth);
				throw;
			}
			this.curDepth++;
			this.nodeType = this.readerNav.NodeType;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0003FD84 File Offset: 0x0003ED84
		public override bool MoveToFirstAttribute()
		{
			if (!this.IsInReadingStates())
			{
				return false;
			}
			this.readerNav.ResetMove(ref this.curDepth, ref this.nodeType);
			if (this.AttributeCount > 0)
			{
				this.readerNav.MoveToAttribute(0);
				this.curDepth++;
				this.nodeType = this.readerNav.NodeType;
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
				return true;
			}
			this.readerNav.RollBackMove(ref this.curDepth);
			return false;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0003FE08 File Offset: 0x0003EE08
		public override bool MoveToNextAttribute()
		{
			if (!this.IsInReadingStates() || this.nodeType == XmlNodeType.EndElement)
			{
				return false;
			}
			this.readerNav.LogMove(this.curDepth);
			this.readerNav.ResetToAttribute(ref this.curDepth);
			if (this.readerNav.MoveToNextAttribute(ref this.curDepth))
			{
				this.nodeType = this.readerNav.NodeType;
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
				return true;
			}
			this.readerNav.RollBackMove(ref this.curDepth);
			return false;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0003FE94 File Offset: 0x0003EE94
		public override bool MoveToElement()
		{
			if (!this.IsInReadingStates())
			{
				return false;
			}
			this.readerNav.LogMove(this.curDepth);
			this.readerNav.ResetToAttribute(ref this.curDepth);
			if (this.readerNav.MoveToElement())
			{
				this.curDepth--;
				this.nodeType = this.readerNav.NodeType;
				if (this.bInReadBinary)
				{
					this.FinishReadBinary();
				}
				return true;
			}
			this.readerNav.RollBackMove(ref this.curDepth);
			return false;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0003FF1B File Offset: 0x0003EF1B
		public override bool Read()
		{
			return this.Read(false);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0003FF24 File Offset: 0x0003EF24
		private bool Read(bool fSkipChildren)
		{
			if (this.fEOF)
			{
				return false;
			}
			if (this.readState == ReadState.Initial)
			{
				if (this.readerNav.NodeType == XmlNodeType.Document || this.readerNav.NodeType == XmlNodeType.DocumentFragment)
				{
					this.bStartFromDocument = true;
					if (!this.ReadNextNode(fSkipChildren))
					{
						this.readState = ReadState.Error;
						return false;
					}
				}
				this.ReSetReadingMarks();
				this.readState = ReadState.Interactive;
				this.nodeType = this.readerNav.NodeType;
				this.curDepth = 0;
				return true;
			}
			if (this.bInReadBinary)
			{
				this.FinishReadBinary();
			}
			if (this.readerNav.CreatedOnAttribute)
			{
				return false;
			}
			this.ReSetReadingMarks();
			bool flag = this.ReadNextNode(fSkipChildren);
			if (flag)
			{
				return true;
			}
			if (this.readState == ReadState.Initial || this.readState == ReadState.Interactive)
			{
				this.readState = ReadState.Error;
			}
			if (this.readState == ReadState.EndOfFile)
			{
				this.nodeType = XmlNodeType.None;
			}
			return false;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0003FFFC File Offset: 0x0003EFFC
		private bool ReadNextNode(bool fSkipChildren)
		{
			if (this.readState != ReadState.Interactive && this.readState != ReadState.Initial)
			{
				this.nodeType = XmlNodeType.None;
				return false;
			}
			bool flag = !fSkipChildren;
			XmlNodeType xmlNodeType = this.readerNav.NodeType;
			flag = flag && this.nodeType != XmlNodeType.EndElement && this.nodeType != XmlNodeType.EndEntity && (xmlNodeType == XmlNodeType.Element || (xmlNodeType == XmlNodeType.EntityReference && this.bResolveEntity) || ((this.readerNav.NodeType == XmlNodeType.Document || this.readerNav.NodeType == XmlNodeType.DocumentFragment) && this.readState == ReadState.Initial));
			if (flag)
			{
				if (this.readerNav.MoveToFirstChild())
				{
					this.nodeType = this.readerNav.NodeType;
					this.curDepth++;
					if (this.bResolveEntity)
					{
						this.bResolveEntity = false;
					}
					return true;
				}
				if (this.readerNav.NodeType == XmlNodeType.Element && !this.readerNav.IsEmptyElement)
				{
					this.nodeType = XmlNodeType.EndElement;
					return true;
				}
				return this.ReadForward(fSkipChildren);
			}
			else
			{
				if (this.readerNav.NodeType == XmlNodeType.EntityReference && this.bResolveEntity)
				{
					this.readerNav.MoveToFirstChild();
					this.nodeType = this.readerNav.NodeType;
					this.curDepth++;
					this.bResolveEntity = false;
					return true;
				}
				return this.ReadForward(fSkipChildren);
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004014A File Offset: 0x0003F14A
		private void SetEndOfFile()
		{
			this.fEOF = true;
			this.readState = ReadState.EndOfFile;
			this.nodeType = XmlNodeType.None;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00040161 File Offset: 0x0003F161
		private bool ReadAtZeroLevel(bool fSkipChildren)
		{
			if (!fSkipChildren && this.nodeType != XmlNodeType.EndElement && this.readerNav.NodeType == XmlNodeType.Element && !this.readerNav.IsEmptyElement)
			{
				this.nodeType = XmlNodeType.EndElement;
				return true;
			}
			this.SetEndOfFile();
			return false;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0004019C File Offset: 0x0003F19C
		private bool ReadForward(bool fSkipChildren)
		{
			if (this.readState == ReadState.Error)
			{
				return false;
			}
			if (!this.bStartFromDocument && this.curDepth == 0)
			{
				return this.ReadAtZeroLevel(fSkipChildren);
			}
			if (this.readerNav.MoveToNext())
			{
				this.nodeType = this.readerNav.NodeType;
				return true;
			}
			if (this.curDepth == 0)
			{
				return this.ReadAtZeroLevel(fSkipChildren);
			}
			if (!this.readerNav.MoveToParent())
			{
				return false;
			}
			if (this.readerNav.NodeType == XmlNodeType.Element)
			{
				this.curDepth--;
				this.nodeType = XmlNodeType.EndElement;
				return true;
			}
			if (this.readerNav.NodeType == XmlNodeType.EntityReference)
			{
				this.curDepth--;
				this.nodeType = XmlNodeType.EndEntity;
				return true;
			}
			return true;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00040258 File Offset: 0x0003F258
		private void ReSetReadingMarks()
		{
			this.readerNav.ResetMove(ref this.curDepth, ref this.nodeType);
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00040271 File Offset: 0x0003F271
		public override bool EOF
		{
			get
			{
				return this.readState != ReadState.Closed && this.fEOF;
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00040284 File Offset: 0x0003F284
		public override void Close()
		{
			this.readState = ReadState.Closed;
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0004028D File Offset: 0x0003F28D
		public override ReadState ReadState
		{
			get
			{
				return this.readState;
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00040295 File Offset: 0x0003F295
		public override void Skip()
		{
			this.Read(true);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0004029F File Offset: 0x0003F29F
		public override string ReadString()
		{
			if (this.NodeType == XmlNodeType.EntityReference && this.bResolveEntity && !this.Read())
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
			return base.ReadString();
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x000402D0 File Offset: 0x0003F2D0
		public override bool HasAttributes
		{
			get
			{
				return this.AttributeCount > 0;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x000402DB File Offset: 0x0003F2DB
		public override XmlNameTable NameTable
		{
			get
			{
				return this.readerNav.NameTable;
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000402E8 File Offset: 0x0003F2E8
		public override string LookupNamespace(string prefix)
		{
			if (!this.IsInReadingStates())
			{
				return null;
			}
			string text = this.readerNav.LookupNamespace(prefix);
			if (text != null && text.Length == 0)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0004031A File Offset: 0x0003F31A
		public override void ResolveEntity()
		{
			if (!this.IsInReadingStates() || this.nodeType != XmlNodeType.EntityReference)
			{
				throw new InvalidOperationException(Res.GetString("Xnr_ResolveEntity"));
			}
			this.bResolveEntity = true;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00040344 File Offset: 0x0003F344
		public override bool ReadAttributeValue()
		{
			if (!this.IsInReadingStates())
			{
				return false;
			}
			if (this.readerNav.ReadAttributeValue(ref this.curDepth, ref this.bResolveEntity, ref this.nodeType))
			{
				this.bInReadBinary = false;
				return true;
			}
			return false;
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x00040379 File Offset: 0x0003F379
		public override bool CanReadBinaryContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0004037C File Offset: 0x0003F37C
		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.readState != ReadState.Interactive)
			{
				return 0;
			}
			if (!this.bInReadBinary)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			this.bInReadBinary = false;
			int num = this.readBinaryHelper.ReadContentAsBase64(buffer, index, count);
			this.bInReadBinary = true;
			return num;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000403CC File Offset: 0x0003F3CC
		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.readState != ReadState.Interactive)
			{
				return 0;
			}
			if (!this.bInReadBinary)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			this.bInReadBinary = false;
			int num = this.readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
			this.bInReadBinary = true;
			return num;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004041C File Offset: 0x0003F41C
		public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
		{
			if (this.readState != ReadState.Interactive)
			{
				return 0;
			}
			if (!this.bInReadBinary)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			this.bInReadBinary = false;
			int num = this.readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
			this.bInReadBinary = true;
			return num;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004046C File Offset: 0x0003F46C
		public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
		{
			if (this.readState != ReadState.Interactive)
			{
				return 0;
			}
			if (!this.bInReadBinary)
			{
				this.readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(this.readBinaryHelper, this);
			}
			this.bInReadBinary = false;
			int num = this.readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
			this.bInReadBinary = true;
			return num;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x000404BC File Offset: 0x0003F4BC
		private void FinishReadBinary()
		{
			this.bInReadBinary = false;
			this.readBinaryHelper.Finish();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000404D0 File Offset: 0x0003F4D0
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.readerNav.GetNamespacesInScope(scope);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000404DE File Offset: 0x0003F4DE
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.readerNav.LookupPrefix(namespaceName);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000404EC File Offset: 0x0003F4EC
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			if (!this.IsInReadingStates())
			{
				return this.readerNav.DefaultLookupNamespace(prefix);
			}
			string text = this.readerNav.LookupNamespace(prefix);
			if (text != null)
			{
				text = this.readerNav.NameTable.Add(text);
			}
			return text;
		}

		// Token: 0x04000999 RID: 2457
		private XmlNodeReaderNavigator readerNav;

		// Token: 0x0400099A RID: 2458
		private XmlNodeType nodeType;

		// Token: 0x0400099B RID: 2459
		private int curDepth;

		// Token: 0x0400099C RID: 2460
		private ReadState readState;

		// Token: 0x0400099D RID: 2461
		private bool fEOF;

		// Token: 0x0400099E RID: 2462
		private bool bResolveEntity;

		// Token: 0x0400099F RID: 2463
		private bool bStartFromDocument;

		// Token: 0x040009A0 RID: 2464
		private bool bInReadBinary;

		// Token: 0x040009A1 RID: 2465
		private ReadContentAsBinaryHelper readBinaryHelper;
	}
}
