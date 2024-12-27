using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000070 RID: 112
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeRegionDirective : CodeDirective
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x0001456C File Offset: 0x0001356C
		public CodeRegionDirective()
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00014574 File Offset: 0x00013574
		public CodeRegionDirective(CodeRegionMode regionMode, string regionText)
		{
			this.RegionText = regionText;
			this.regionMode = regionMode;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001458A File Offset: 0x0001358A
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x000145A0 File Offset: 0x000135A0
		public string RegionText
		{
			get
			{
				if (this.regionText != null)
				{
					return this.regionText;
				}
				return string.Empty;
			}
			set
			{
				this.regionText = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x000145A9 File Offset: 0x000135A9
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x000145B1 File Offset: 0x000135B1
		public CodeRegionMode RegionMode
		{
			get
			{
				return this.regionMode;
			}
			set
			{
				this.regionMode = value;
			}
		}

		// Token: 0x0400086C RID: 2156
		private string regionText;

		// Token: 0x0400086D RID: 2157
		private CodeRegionMode regionMode;
	}
}
