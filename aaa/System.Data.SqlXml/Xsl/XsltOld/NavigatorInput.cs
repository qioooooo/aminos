using System;
using System.Diagnostics;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000181 RID: 385
	internal class NavigatorInput
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x0004F1D3 File Offset: 0x0004E1D3
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x0004F1DB File Offset: 0x0004E1DB
		internal NavigatorInput Next
		{
			get
			{
				return this._Next;
			}
			set
			{
				this._Next = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0004F1E4 File Offset: 0x0004E1E4
		internal string Href
		{
			get
			{
				return this._Href;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0004F1EC File Offset: 0x0004E1EC
		internal Keywords Atoms
		{
			get
			{
				return this._Atoms;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0004F1F4 File Offset: 0x0004E1F4
		internal XPathNavigator Navigator
		{
			get
			{
				return this._Navigator;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0004F1FC File Offset: 0x0004E1FC
		internal InputScopeManager InputScopeManager
		{
			get
			{
				return this._Manager;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004F204 File Offset: 0x0004E204
		internal bool Advance()
		{
			return this._Navigator.MoveToNext();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004F211 File Offset: 0x0004E211
		internal bool Recurse()
		{
			return this._Navigator.MoveToFirstChild();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0004F21E File Offset: 0x0004E21E
		internal bool ToParent()
		{
			return this._Navigator.MoveToParent();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0004F22B File Offset: 0x0004E22B
		internal void Close()
		{
			this._Navigator = null;
			this._PositionInfo = null;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0004F23B File Offset: 0x0004E23B
		internal int LineNumber
		{
			get
			{
				return this._PositionInfo.LineNumber;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0004F248 File Offset: 0x0004E248
		internal int LinePosition
		{
			get
			{
				return this._PositionInfo.LinePosition;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0004F255 File Offset: 0x0004E255
		internal XPathNodeType NodeType
		{
			get
			{
				return this._Navigator.NodeType;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x0004F262 File Offset: 0x0004E262
		internal string Name
		{
			get
			{
				return this._Navigator.Name;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0004F26F File Offset: 0x0004E26F
		internal string LocalName
		{
			get
			{
				return this._Navigator.LocalName;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0004F27C File Offset: 0x0004E27C
		internal string NamespaceURI
		{
			get
			{
				return this._Navigator.NamespaceURI;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0004F289 File Offset: 0x0004E289
		internal string Prefix
		{
			get
			{
				return this._Navigator.Prefix;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0004F296 File Offset: 0x0004E296
		internal string Value
		{
			get
			{
				return this._Navigator.Value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0004F2A3 File Offset: 0x0004E2A3
		internal bool IsEmptyTag
		{
			get
			{
				return this._Navigator.IsEmptyElement;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0004F2B0 File Offset: 0x0004E2B0
		internal string BaseURI
		{
			get
			{
				return this._Navigator.BaseURI;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0004F2BD File Offset: 0x0004E2BD
		internal bool MoveToFirstAttribute()
		{
			return this._Navigator.MoveToFirstAttribute();
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0004F2CA File Offset: 0x0004E2CA
		internal bool MoveToNextAttribute()
		{
			return this._Navigator.MoveToNextAttribute();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0004F2D7 File Offset: 0x0004E2D7
		internal bool MoveToFirstNamespace()
		{
			return this._Navigator.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0004F2E5 File Offset: 0x0004E2E5
		internal bool MoveToNextNamespace()
		{
			return this._Navigator.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0004F2F4 File Offset: 0x0004E2F4
		internal NavigatorInput(XPathNavigator navigator, string baseUri, InputScope rootScope)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			if (baseUri == null)
			{
				throw new ArgumentNullException("baseUri");
			}
			this._Next = null;
			this._Href = baseUri;
			this._Atoms = new Keywords(navigator.NameTable);
			this._Atoms.LookupKeywords();
			this._Navigator = navigator;
			this._Manager = new InputScopeManager(this._Navigator, rootScope);
			this._PositionInfo = PositionInfo.GetPositionInfo(this._Navigator);
			if (this.NodeType == XPathNodeType.Root)
			{
				this._Navigator.MoveToFirstChild();
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004F38B File Offset: 0x0004E38B
		internal NavigatorInput(XPathNavigator navigator)
			: this(navigator, navigator.BaseURI, null)
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0004F39B File Offset: 0x0004E39B
		[Conditional("DEBUG")]
		internal void AssertInput()
		{
		}

		// Token: 0x04000ADE RID: 2782
		private XPathNavigator _Navigator;

		// Token: 0x04000ADF RID: 2783
		private PositionInfo _PositionInfo;

		// Token: 0x04000AE0 RID: 2784
		private InputScopeManager _Manager;

		// Token: 0x04000AE1 RID: 2785
		private NavigatorInput _Next;

		// Token: 0x04000AE2 RID: 2786
		private string _Href;

		// Token: 0x04000AE3 RID: 2787
		private Keywords _Atoms;
	}
}
