using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class RegexCompilationInfo
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00006715 File Offset: 0x00005715
		public RegexCompilationInfo(string pattern, RegexOptions options, string name, string fullnamespace, bool ispublic)
		{
			this.Pattern = pattern;
			this.Name = name;
			this.Namespace = fullnamespace;
			this.options = options;
			this.isPublic = ispublic;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00006742 File Offset: 0x00005742
		// (set) Token: 0x060000BE RID: 190 RVA: 0x0000674A File Offset: 0x0000574A
		public string Pattern
		{
			get
			{
				return this.pattern;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.pattern = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00006761 File Offset: 0x00005761
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00006769 File Offset: 0x00005769
		public RegexOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006772 File Offset: 0x00005772
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000677C File Offset: 0x0000577C
		public string Name
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
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "value" }), "value");
				}
				this.name = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000067CB File Offset: 0x000057CB
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000067D3 File Offset: 0x000057D3
		public string Namespace
		{
			get
			{
				return this.nspace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.nspace = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000067EA File Offset: 0x000057EA
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000067F2 File Offset: 0x000057F2
		public bool IsPublic
		{
			get
			{
				return this.isPublic;
			}
			set
			{
				this.isPublic = value;
			}
		}

		// Token: 0x040006C0 RID: 1728
		private string pattern;

		// Token: 0x040006C1 RID: 1729
		private RegexOptions options;

		// Token: 0x040006C2 RID: 1730
		private string name;

		// Token: 0x040006C3 RID: 1731
		private string nspace;

		// Token: 0x040006C4 RID: 1732
		private bool isPublic;
	}
}
