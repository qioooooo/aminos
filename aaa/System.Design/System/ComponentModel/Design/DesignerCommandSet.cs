using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F8 RID: 248
	public class DesignerCommandSet
	{
		// Token: 0x06000A66 RID: 2662 RVA: 0x00028BC5 File Offset: 0x00027BC5
		public virtual ICollection GetCommands(string name)
		{
			return null;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00028BC8 File Offset: 0x00027BC8
		public DesignerVerbCollection Verbs
		{
			get
			{
				return (DesignerVerbCollection)this.GetCommands("Verbs");
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00028BDA File Offset: 0x00027BDA
		public DesignerActionListCollection ActionLists
		{
			get
			{
				return (DesignerActionListCollection)this.GetCommands("ActionLists");
			}
		}
	}
}
