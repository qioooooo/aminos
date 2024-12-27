using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000016 RID: 22
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public class SortKey
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00003B37 File Offset: 0x00002B37
		public SortKey()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003B44 File Offset: 0x00002B44
		public SortKey(string attributeName, string matchingRule, bool reverseOrder)
		{
			Utility.CheckOSVersion();
			this.AttributeName = attributeName;
			this.rule = matchingRule;
			this.order = reverseOrder;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003B66 File Offset: 0x00002B66
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003B6E File Offset: 0x00002B6E
		public string AttributeName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.name = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003B85 File Offset: 0x00002B85
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003B8D File Offset: 0x00002B8D
		public string MatchingRule
		{
			get
			{
				return this.rule;
			}
			set
			{
				this.rule = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003B96 File Offset: 0x00002B96
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003B9E File Offset: 0x00002B9E
		public bool ReverseOrder
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x040000DA RID: 218
		private string name;

		// Token: 0x040000DB RID: 219
		private string rule;

		// Token: 0x040000DC RID: 220
		private bool order;
	}
}
