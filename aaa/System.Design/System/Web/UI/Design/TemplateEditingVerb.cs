using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200039A RID: 922
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TemplateEditingVerb : DesignerVerb, IDisposable
	{
		// Token: 0x06002217 RID: 8727 RVA: 0x000BB5DC File Offset: 0x000BA5DC
		public TemplateEditingVerb(string text, int index, TemplatedControlDesigner designer)
			: this(text, index, designer.TemplateEditingVerbHandler)
		{
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000BB5EC File Offset: 0x000BA5EC
		public TemplateEditingVerb(string text, int index)
			: this(text, index, TemplateEditingVerb.dummyEventHandler)
		{
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000BB5FB File Offset: 0x000BA5FB
		private TemplateEditingVerb(string text, int index, EventHandler handler)
			: base(text, handler)
		{
			this.index = index;
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000BB60C File Offset: 0x000BA60C
		// (set) Token: 0x0600221B RID: 8731 RVA: 0x000BB614 File Offset: 0x000BA614
		internal ITemplateEditingFrame EditingFrame
		{
			get
			{
				return this.editingFrame;
			}
			set
			{
				this.editingFrame = value;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000BB61D File Offset: 0x000BA61D
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000BB625 File Offset: 0x000BA625
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000BB634 File Offset: 0x000BA634
		~TemplateEditingVerb()
		{
			this.Dispose(false);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000BB664 File Offset: 0x000BA664
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.editingFrame != null)
			{
				this.editingFrame.Dispose();
				this.editingFrame = null;
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000BB683 File Offset: 0x000BA683
		private static void OnDummyEventHandler(object sender, EventArgs e)
		{
		}

		// Token: 0x04001844 RID: 6212
		private static readonly EventHandler dummyEventHandler = new EventHandler(TemplateEditingVerb.OnDummyEventHandler);

		// Token: 0x04001845 RID: 6213
		private ITemplateEditingFrame editingFrame;

		// Token: 0x04001846 RID: 6214
		private int index;
	}
}
