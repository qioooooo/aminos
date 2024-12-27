using System;

namespace System.ComponentModel.Design
{
	// Token: 0x0200012A RID: 298
	internal class DesignerActionVerbItem : DesignerActionMethodItem
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x0002E01B File Offset: 0x0002D01B
		public DesignerActionVerbItem(DesignerVerb verb)
		{
			if (verb == null)
			{
				throw new ArgumentNullException();
			}
			this._targetVerb = verb;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002E033 File Offset: 0x0002D033
		public override string Category
		{
			get
			{
				return "Verbs";
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0002E03A File Offset: 0x0002D03A
		public override string Description
		{
			get
			{
				return this._targetVerb.Description;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0002E047 File Offset: 0x0002D047
		public override string DisplayName
		{
			get
			{
				return this._targetVerb.Text;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0002E054 File Offset: 0x0002D054
		public override string MemberName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0002E057 File Offset: 0x0002D057
		public override bool IncludeAsDesignerVerb
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002E05A File Offset: 0x0002D05A
		public override void Invoke()
		{
			this._targetVerb.Invoke();
		}

		// Token: 0x04000E5A RID: 3674
		private DesignerVerb _targetVerb;
	}
}
