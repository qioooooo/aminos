using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000072 RID: 114
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeRemoveEventStatement : CodeStatement
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x000145BA File Offset: 0x000135BA
		public CodeRemoveEventStatement()
		{
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000145C2 File Offset: 0x000135C2
		public CodeRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
		{
			this.eventRef = eventRef;
			this.listener = listener;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000145D8 File Offset: 0x000135D8
		public CodeRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
		{
			this.eventRef = new CodeEventReferenceExpression(targetObject, eventName);
			this.listener = listener;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x000145F4 File Offset: 0x000135F4
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0001460F File Offset: 0x0001360F
		public CodeEventReferenceExpression Event
		{
			get
			{
				if (this.eventRef == null)
				{
					this.eventRef = new CodeEventReferenceExpression();
				}
				return this.eventRef;
			}
			set
			{
				this.eventRef = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00014618 File Offset: 0x00013618
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00014620 File Offset: 0x00013620
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

		// Token: 0x04000872 RID: 2162
		private CodeEventReferenceExpression eventRef;

		// Token: 0x04000873 RID: 2163
		private CodeExpression listener;
	}
}
