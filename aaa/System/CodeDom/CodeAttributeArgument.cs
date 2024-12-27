using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200003D RID: 61
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeAttributeArgument
	{
		// Token: 0x06000284 RID: 644 RVA: 0x00012717 File Offset: 0x00011717
		public CodeAttributeArgument()
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0001271F File Offset: 0x0001171F
		public CodeAttributeArgument(CodeExpression value)
		{
			this.Value = value;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0001272E File Offset: 0x0001172E
		public CodeAttributeArgument(string name, CodeExpression value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00012744 File Offset: 0x00011744
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0001275A File Offset: 0x0001175A
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00012763 File Offset: 0x00011763
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0001276B File Offset: 0x0001176B
		public CodeExpression Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x040007E3 RID: 2019
		private string name;

		// Token: 0x040007E4 RID: 2020
		private CodeExpression value;
	}
}
