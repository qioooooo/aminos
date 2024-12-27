using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200003A RID: 58
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeStatement : CodeObject
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0001261E File Offset: 0x0001161E
		// (set) Token: 0x06000273 RID: 627 RVA: 0x00012626 File Offset: 0x00011626
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0001262F File Offset: 0x0001162F
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				if (this.startDirectives == null)
				{
					this.startDirectives = new CodeDirectiveCollection();
				}
				return this.startDirectives;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0001264A File Offset: 0x0001164A
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				if (this.endDirectives == null)
				{
					this.endDirectives = new CodeDirectiveCollection();
				}
				return this.endDirectives;
			}
		}

		// Token: 0x040007DC RID: 2012
		private CodeLinePragma linePragma;

		// Token: 0x040007DD RID: 2013
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x040007DE RID: 2014
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
