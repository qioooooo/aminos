using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TemplateEditingVerb : DesignerVerb, IDisposable
	{
		public TemplateEditingVerb(string text, int index, TemplatedControlDesigner designer)
			: this(text, index, designer.TemplateEditingVerbHandler)
		{
		}

		public TemplateEditingVerb(string text, int index)
			: this(text, index, TemplateEditingVerb.dummyEventHandler)
		{
		}

		private TemplateEditingVerb(string text, int index, EventHandler handler)
			: base(text, handler)
		{
			this.index = index;
		}

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

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TemplateEditingVerb()
		{
			this.Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.editingFrame != null)
			{
				this.editingFrame.Dispose();
				this.editingFrame = null;
			}
		}

		private static void OnDummyEventHandler(object sender, EventArgs e)
		{
		}

		private static readonly EventHandler dummyEventHandler = new EventHandler(TemplateEditingVerb.OnDummyEventHandler);

		private ITemplateEditingFrame editingFrame;

		private int index;
	}
}
