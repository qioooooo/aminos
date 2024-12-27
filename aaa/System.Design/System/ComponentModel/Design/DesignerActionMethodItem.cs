using System;
using System.Design;
using System.Reflection;

namespace System.ComponentModel.Design
{
	// Token: 0x02000106 RID: 262
	public class DesignerActionMethodItem : DesignerActionItem
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x0002944B File Offset: 0x0002844B
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description, bool includeAsDesignerVerb)
			: base(displayName, category, description)
		{
			this.actionList = actionList;
			this.memberName = memberName;
			this.includeAsDesignerVerb = includeAsDesignerVerb;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002946E File Offset: 0x0002846E
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName)
			: this(actionList, memberName, displayName, null, null, false)
		{
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002947C File Offset: 0x0002847C
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, bool includeAsDesignerVerb)
			: this(actionList, memberName, displayName, null, null, includeAsDesignerVerb)
		{
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002948B File Offset: 0x0002848B
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category)
			: this(actionList, memberName, displayName, category, null, false)
		{
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002949A File Offset: 0x0002849A
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, bool includeAsDesignerVerb)
			: this(actionList, memberName, displayName, category, null, includeAsDesignerVerb)
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000294AA File Offset: 0x000284AA
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description)
			: this(actionList, memberName, displayName, category, description, false)
		{
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000294BA File Offset: 0x000284BA
		internal DesignerActionMethodItem()
		{
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000294C2 File Offset: 0x000284C2
		public virtual string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000294CA File Offset: 0x000284CA
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x000294D2 File Offset: 0x000284D2
		public IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
			set
			{
				this.relatedComponent = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x000294DB File Offset: 0x000284DB
		public virtual bool IncludeAsDesignerVerb
		{
			get
			{
				return this.includeAsDesignerVerb;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000294E3 File Offset: 0x000284E3
		internal void Invoke(object sender, EventArgs args)
		{
			this.Invoke();
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000294EC File Offset: 0x000284EC
		public virtual void Invoke()
		{
			if (this.methodInfo == null)
			{
				this.methodInfo = this.actionList.GetType().GetMethod(this.memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (this.methodInfo != null)
			{
				this.methodInfo.Invoke(this.actionList, null);
				return;
			}
			throw new InvalidOperationException(SR.GetString("DesignerActionPanel_CouldNotFindMethod", new object[] { this.MemberName }));
		}

		// Token: 0x04000D9D RID: 3485
		private string memberName;

		// Token: 0x04000D9E RID: 3486
		private bool includeAsDesignerVerb;

		// Token: 0x04000D9F RID: 3487
		private DesignerActionList actionList;

		// Token: 0x04000DA0 RID: 3488
		private MethodInfo methodInfo;

		// Token: 0x04000DA1 RID: 3489
		private IComponent relatedComponent;
	}
}
