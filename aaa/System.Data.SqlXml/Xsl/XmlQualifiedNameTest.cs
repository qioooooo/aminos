using System;

namespace System.Xml.Xsl
{
	// Token: 0x0200000F RID: 15
	internal class XmlQualifiedNameTest : XmlQualifiedName
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000313B File Offset: 0x0000213B
		public static XmlQualifiedNameTest Wildcard
		{
			get
			{
				return XmlQualifiedNameTest.wc;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003142 File Offset: 0x00002142
		private XmlQualifiedNameTest(string name, string ns, bool exclude)
			: base(name, ns)
		{
			this.exclude = exclude;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003153 File Offset: 0x00002153
		public static XmlQualifiedNameTest New(string name, string ns)
		{
			if (ns == null && name == null)
			{
				return XmlQualifiedNameTest.Wildcard;
			}
			return new XmlQualifiedNameTest((name == null) ? "*" : name, (ns == null) ? "*" : ns, false);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000317D File Offset: 0x0000217D
		public bool IsWildcard
		{
			get
			{
				return this == XmlQualifiedNameTest.Wildcard;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003187 File Offset: 0x00002187
		public bool IsNameWildcard
		{
			get
			{
				return base.Name == "*";
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003196 File Offset: 0x00002196
		public bool IsNamespaceWildcard
		{
			get
			{
				return base.Namespace == "*";
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000031A5 File Offset: 0x000021A5
		private bool IsNameSubsetOf(XmlQualifiedNameTest other)
		{
			return other.IsNameWildcard || base.Name == other.Name;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000031C4 File Offset: 0x000021C4
		private bool IsNamespaceSubsetOf(XmlQualifiedNameTest other)
		{
			return other.IsNamespaceWildcard || (this.exclude == other.exclude && base.Namespace == other.Namespace) || (other.exclude && !this.exclude && base.Namespace != other.Namespace);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000321F File Offset: 0x0000221F
		public bool IsSubsetOf(XmlQualifiedNameTest other)
		{
			return this.IsNameSubsetOf(other) && this.IsNamespaceSubsetOf(other);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003233 File Offset: 0x00002233
		public bool HasIntersection(XmlQualifiedNameTest other)
		{
			return (this.IsNamespaceSubsetOf(other) || other.IsNamespaceSubsetOf(this)) && (this.IsNameSubsetOf(other) || other.IsNameSubsetOf(this));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000325C File Offset: 0x0000225C
		public override string ToString()
		{
			if (this == XmlQualifiedNameTest.Wildcard)
			{
				return "*";
			}
			if (base.Namespace.Length == 0)
			{
				return base.Name;
			}
			if (base.Namespace == "*")
			{
				return "*:" + base.Name;
			}
			if (this.exclude)
			{
				return "{~" + base.Namespace + "}:" + base.Name;
			}
			return "{" + base.Namespace + "}:" + base.Name;
		}

		// Token: 0x040000D0 RID: 208
		private const string wildcard = "*";

		// Token: 0x040000D1 RID: 209
		private bool exclude;

		// Token: 0x040000D2 RID: 210
		private static XmlQualifiedNameTest wc = XmlQualifiedNameTest.New("*", "*");
	}
}
