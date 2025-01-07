﻿using System;

namespace System.Xml
{
	internal class ValidatingReaderNodeData
	{
		public ValidatingReaderNodeData()
		{
			this.Clear(XmlNodeType.None);
		}

		public ValidatingReaderNodeData(XmlNodeType nodeType)
		{
			this.Clear(nodeType);
		}

		public string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		public string Namespace
		{
			get
			{
				return this.namespaceUri;
			}
			set
			{
				this.namespaceUri = value;
			}
		}

		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		public string GetAtomizedNameWPrefix(XmlNameTable nameTable)
		{
			if (this.nameWPrefix == null)
			{
				if (this.prefix.Length == 0)
				{
					this.nameWPrefix = this.localName;
				}
				else
				{
					this.nameWPrefix = nameTable.Add(this.prefix + ":" + this.localName);
				}
			}
			return this.nameWPrefix;
		}

		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		public string RawValue
		{
			get
			{
				return this.rawValue;
			}
			set
			{
				this.rawValue = value;
			}
		}

		public string OriginalStringValue
		{
			get
			{
				return this.originalStringValue;
			}
			set
			{
				this.originalStringValue = value;
			}
		}

		public XmlNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
			set
			{
				this.nodeType = value;
			}
		}

		public AttributePSVIInfo AttInfo
		{
			get
			{
				return this.attributePSVIInfo;
			}
			set
			{
				this.attributePSVIInfo = value;
			}
		}

		public int LineNumber
		{
			get
			{
				return this.lineNo;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.linePos;
			}
		}

		internal void Clear(XmlNodeType nodeType)
		{
			this.nodeType = nodeType;
			this.localName = string.Empty;
			this.prefix = string.Empty;
			this.namespaceUri = string.Empty;
			this.rawValue = string.Empty;
			if (this.attributePSVIInfo != null)
			{
				this.attributePSVIInfo.Reset();
			}
			this.nameWPrefix = null;
			this.lineNo = 0;
			this.linePos = 0;
		}

		internal void ClearName()
		{
			this.localName = string.Empty;
			this.prefix = string.Empty;
			this.namespaceUri = string.Empty;
		}

		internal void SetLineInfo(int lineNo, int linePos)
		{
			this.lineNo = lineNo;
			this.linePos = linePos;
		}

		internal void SetLineInfo(IXmlLineInfo lineInfo)
		{
			if (lineInfo != null)
			{
				this.lineNo = lineInfo.LineNumber;
				this.linePos = lineInfo.LinePosition;
			}
		}

		internal void SetItemData(string localName, string prefix, string ns, string value)
		{
			this.localName = localName;
			this.prefix = prefix;
			this.namespaceUri = ns;
			this.rawValue = value;
		}

		internal void SetItemData(string localName, string prefix, string ns, int depth)
		{
			this.localName = localName;
			this.prefix = prefix;
			this.namespaceUri = ns;
			this.depth = depth;
			this.rawValue = string.Empty;
		}

		internal void SetItemData(string value)
		{
			this.SetItemData(value, value);
		}

		internal void SetItemData(string value, string originalStringValue)
		{
			this.rawValue = value;
			this.originalStringValue = originalStringValue;
		}

		private string localName;

		private string namespaceUri;

		private string prefix;

		private string nameWPrefix;

		private string rawValue;

		private string originalStringValue;

		private int depth;

		private AttributePSVIInfo attributePSVIInfo;

		private XmlNodeType nodeType;

		private int lineNo;

		private int linePos;
	}
}
