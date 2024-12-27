using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000139 RID: 313
	internal abstract class ExtensionQuery : Query
	{
		// Token: 0x060011F5 RID: 4597 RVA: 0x0004EE26 File Offset: 0x0004DE26
		public ExtensionQuery(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004EE3C File Offset: 0x0004DE3C
		protected ExtensionQuery(ExtensionQuery other)
			: base(other)
		{
			this.prefix = other.prefix;
			this.name = other.name;
			this.xsltContext = other.xsltContext;
			this.queryIterator = (ResetableIterator)Query.Clone(other.queryIterator);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0004EE8A File Offset: 0x0004DE8A
		public override void Reset()
		{
			if (this.queryIterator != null)
			{
				this.queryIterator.Reset();
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0004EE9F File Offset: 0x0004DE9F
		public override XPathNavigator Current
		{
			get
			{
				if (this.queryIterator == null)
				{
					throw XPathException.Create("Xp_NodeSetExpected");
				}
				if (this.queryIterator.CurrentPosition == 0)
				{
					this.Advance();
				}
				return this.queryIterator.Current;
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0004EED3 File Offset: 0x0004DED3
		public override XPathNavigator Advance()
		{
			if (this.queryIterator == null)
			{
				throw XPathException.Create("Xp_NodeSetExpected");
			}
			if (this.queryIterator.MoveNext())
			{
				return this.queryIterator.Current;
			}
			return null;
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0004EF02 File Offset: 0x0004DF02
		public override int CurrentPosition
		{
			get
			{
				if (this.queryIterator != null)
				{
					return this.queryIterator.CurrentPosition;
				}
				return 0;
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0004EF1C File Offset: 0x0004DF1C
		protected object ProcessResult(object value)
		{
			if (value is string)
			{
				return value;
			}
			if (value is double)
			{
				return value;
			}
			if (value is bool)
			{
				return value;
			}
			if (value is XPathNavigator)
			{
				return value;
			}
			if (value is int)
			{
				return (double)((int)value);
			}
			if (value == null)
			{
				this.queryIterator = XPathEmptyIterator.Instance;
				return this;
			}
			ResetableIterator resetableIterator = value as ResetableIterator;
			if (resetableIterator != null)
			{
				this.queryIterator = (ResetableIterator)resetableIterator.Clone();
				return this;
			}
			XPathNodeIterator xpathNodeIterator = value as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				this.queryIterator = new XPathArrayIterator(xpathNodeIterator);
				return this;
			}
			IXPathNavigable ixpathNavigable = value as IXPathNavigable;
			if (ixpathNavigable != null)
			{
				return ixpathNavigable.CreateNavigator();
			}
			if (value is short)
			{
				return (double)((short)value);
			}
			if (value is long)
			{
				return (double)((long)value);
			}
			if (value is uint)
			{
				return (uint)value;
			}
			if (value is ushort)
			{
				return (double)((ushort)value);
			}
			if (value is ulong)
			{
				return (ulong)value;
			}
			if (value is float)
			{
				return (double)((float)value);
			}
			if (value is decimal)
			{
				return (double)((decimal)value);
			}
			return value.ToString();
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0004F05C File Offset: 0x0004E05C
		protected string QName
		{
			get
			{
				if (this.prefix.Length == 0)
				{
					return this.name;
				}
				return this.prefix + ":" + this.name;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0004F088 File Offset: 0x0004E088
		public override int Count
		{
			get
			{
				if (this.queryIterator != null)
				{
					return this.queryIterator.Count;
				}
				return 1;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0004F09F File Offset: 0x0004E09F
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x04000B56 RID: 2902
		protected string prefix;

		// Token: 0x04000B57 RID: 2903
		protected string name;

		// Token: 0x04000B58 RID: 2904
		protected XsltContext xsltContext;

		// Token: 0x04000B59 RID: 2905
		private ResetableIterator queryIterator;
	}
}
