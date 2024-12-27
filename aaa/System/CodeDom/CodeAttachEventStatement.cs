using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200003C RID: 60
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttachEventStatement : CodeStatement
	{
		// Token: 0x0600027D RID: 637 RVA: 0x000126AD File Offset: 0x000116AD
		public CodeAttachEventStatement()
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000126B5 File Offset: 0x000116B5
		public CodeAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
		{
			this.eventRef = eventRef;
			this.listener = listener;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000126CB File Offset: 0x000116CB
		public CodeAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
		{
			this.eventRef = new CodeEventReferenceExpression(targetObject, eventName);
			this.listener = listener;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000126E7 File Offset: 0x000116E7
		// (set) Token: 0x06000281 RID: 641 RVA: 0x000126FD File Offset: 0x000116FD
		public CodeEventReferenceExpression Event
		{
			get
			{
				if (this.eventRef == null)
				{
					return new CodeEventReferenceExpression();
				}
				return this.eventRef;
			}
			set
			{
				this.eventRef = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00012706 File Offset: 0x00011706
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0001270E File Offset: 0x0001170E
		public CodeExpression Listener
		{
			get
			{
				return this.listener;
			}
			set
			{
				this.listener = value;
			}
		}

		// Token: 0x040007E1 RID: 2017
		private CodeEventReferenceExpression eventRef;

		// Token: 0x040007E2 RID: 2018
		private CodeExpression listener;
	}
}
