using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000391 RID: 913
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class TemplatedControlDesigner : ControlDesigner
	{
		// Token: 0x0600218E RID: 8590 RVA: 0x000B9DB2 File Offset: 0x000B8DB2
		public TemplatedControlDesigner()
		{
			this.enableTemplateEditing = true;
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000B9DC1 File Offset: 0x000B8DC1
		[Obsolete("Use of this property is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public ITemplateEditingFrame ActiveTemplateEditingFrame
		{
			get
			{
				if (this._currentTemplateGroup != null)
				{
					return this._currentTemplateGroup.Frame;
				}
				return null;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000B9DD8 File Offset: 0x000B8DD8
		public bool CanEnterTemplateMode
		{
			get
			{
				return this.enableTemplateEditing;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000B9DE0 File Offset: 0x000B8DE0
		protected override bool DataBindingsEnabled
		{
			get
			{
				return (!this.InTemplateModeInternal || !this.HidePropertiesInTemplateMode) && base.DataBindingsEnabled;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000B9DFA File Offset: 0x000B8DFA
		[Obsolete("The recommended alternative is System.Web.UI.Design.ControlDesigner.InTemplateMode. http://go.microsoft.com/fwlink/?linkid=14202")]
		public new bool InTemplateMode
		{
			get
			{
				return this._currentTemplateGroup != null;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000B9E08 File Offset: 0x000B8E08
		internal bool InTemplateModeInternal
		{
			get
			{
				return this.InTemplateMode;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000B9E10 File Offset: 0x000B8E10
		internal EventHandler TemplateEditingVerbHandler
		{
			get
			{
				return new EventHandler(this.OnTemplateEditingVerbInvoked);
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x000B9E20 File Offset: 0x000B8E20
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				this.TemplateGroupTable.Clear();
				TemplatedControlDesigner.TemplateEditingVerbCollection templateEditingVerbsInternal = this.GetTemplateEditingVerbsInternal();
				foreach (object obj in ((IEnumerable)templateEditingVerbsInternal))
				{
					TemplateEditingVerb templateEditingVerb = (TemplateEditingVerb)obj;
					if (templateEditingVerb.Enabled && templateEditingVerb.Visible)
					{
						ITemplateEditingFrame templateEditingFrame = this.CreateTemplateEditingFrame(templateEditingVerb);
						templateEditingFrame.Verb = templateEditingVerb;
						TemplateGroup templateGroup = new TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup(templateEditingVerb, templateEditingFrame);
						bool flag = templateEditingFrame.TemplateStyles != null;
						for (int i = 0; i < templateEditingFrame.TemplateNames.Length; i++)
						{
							Style style = (flag ? templateEditingFrame.TemplateStyles[i] : null);
							templateGroup.AddTemplateDefinition(new TemplatedControlDesigner.TemplatedControlDesignerTemplateDefinition(templateEditingFrame.TemplateNames[i], style, this, templateEditingFrame)
							{
								SupportsDataBinding = true
							});
						}
						templateGroups.Add(templateGroup);
						this.TemplateGroupTable[templateEditingFrame] = templateGroup;
					}
				}
				return templateGroups;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000B9F38 File Offset: 0x000B8F38
		private IDictionary TemplateGroupTable
		{
			get
			{
				if (this._templateGroupTable == null)
				{
					this._templateGroupTable = new HybridDictionary();
				}
				return this._templateGroupTable;
			}
		}

		// Token: 0x06002197 RID: 8599
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected abstract ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb);

		// Token: 0x06002198 RID: 8600 RVA: 0x000B9F53 File Offset: 0x000B8F53
		private void EnableTemplateEditing(bool enable)
		{
			this.enableTemplateEditing = enable;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000B9F5C File Offset: 0x000B8F5C
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public void EnterTemplateMode(ITemplateEditingFrame newTemplateEditingFrame)
		{
			if (this.ActiveTemplateEditingFrame == newTemplateEditingFrame)
			{
				return;
			}
			if (this.BehaviorInternal != null)
			{
				IControlDesignerBehavior controlDesignerBehavior = (IControlDesignerBehavior)this.BehaviorInternal;
				try
				{
					bool flag = false;
					if (this.InTemplateModeInternal)
					{
						flag = true;
						this.ExitTemplateModeInternal(flag, false, true);
					}
					else if (controlDesignerBehavior != null)
					{
						controlDesignerBehavior.DesignTimeHtml = string.Empty;
					}
					this._currentTemplateGroup = (TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup)this.TemplateGroupTable[newTemplateEditingFrame];
					if (this._currentTemplateGroup == null)
					{
						this._currentTemplateGroup = new TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup(null, newTemplateEditingFrame);
					}
					if (!flag)
					{
						this.RaiseTemplateModeChanged();
					}
					this.ActiveTemplateEditingFrame.Open();
					base.IsDirtyInternal = true;
					TypeDescriptor.Refresh(base.Component);
				}
				catch
				{
				}
				IWebFormsDocumentService webFormsDocumentService = (IWebFormsDocumentService)this.GetService(typeof(IWebFormsDocumentService));
				if (webFormsDocumentService != null)
				{
					webFormsDocumentService.UpdateSelection();
				}
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000BA038 File Offset: 0x000B9038
		private void EnterTemplateModeInternal(ITemplateEditingFrame newTemplateEditingFrame)
		{
			this.EnterTemplateMode(newTemplateEditingFrame);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000BA044 File Offset: 0x000B9044
		private void ExitNestedTemplates(bool fSave)
		{
			try
			{
				IComponent viewControl = base.ViewControl;
				IDesignerHost designerHost = (IDesignerHost)viewControl.Site.GetService(typeof(IDesignerHost));
				ControlCollection controls = ((Control)viewControl).Controls;
				for (int i = 0; i < controls.Count; i++)
				{
					IDesigner designer = designerHost.GetDesigner(controls[i]);
					if (designer is TemplatedControlDesigner)
					{
						TemplatedControlDesigner templatedControlDesigner = (TemplatedControlDesigner)designer;
						if (templatedControlDesigner.InTemplateModeInternal)
						{
							templatedControlDesigner.ExitTemplateModeInternal(false, true, fSave);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000BA0DC File Offset: 0x000B90DC
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public void ExitTemplateMode(bool fSwitchingTemplates, bool fNested, bool fSave)
		{
			try
			{
				this.ExitNestedTemplates(fSave);
				this.ActiveTemplateEditingFrame.Close(fSave);
				if (!fSwitchingTemplates)
				{
					this._currentTemplateGroup = null;
					this.RaiseTemplateModeChanged();
					if (!fNested)
					{
						this.UpdateDesignTimeHtml();
						TypeDescriptor.Refresh(base.Component);
						IWebFormsDocumentService webFormsDocumentService = (IWebFormsDocumentService)this.GetService(typeof(IWebFormsDocumentService));
						if (webFormsDocumentService != null)
						{
							webFormsDocumentService.UpdateSelection();
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000BA154 File Offset: 0x000B9154
		private void ExitTemplateModeInternal(bool fSwitchingTemplates, bool fNested, bool fSave)
		{
			this.ExitTemplateMode(fSwitchingTemplates, fNested, fSave);
		}

		// Token: 0x0600219E RID: 8606
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected abstract TemplateEditingVerb[] GetCachedTemplateEditingVerbs();

		// Token: 0x0600219F RID: 8607 RVA: 0x000BA160 File Offset: 0x000B9160
		internal override string GetPersistInnerHtmlInternal()
		{
			if (this.InTemplateModeInternal)
			{
				this.SaveActiveTemplateEditingFrame();
			}
			string persistInnerHtmlInternal = base.GetPersistInnerHtmlInternal();
			if (this.InTemplateModeInternal)
			{
				base.IsDirtyInternal = true;
			}
			return persistInnerHtmlInternal;
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000BA192 File Offset: 0x000B9192
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetTemplateContainerDataItemProperty(string templateName)
		{
			return string.Empty;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000BA199 File Offset: 0x000B9199
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual IEnumerable GetTemplateContainerDataSource(string templateName)
		{
			return null;
		}

		// Token: 0x060021A2 RID: 8610
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public abstract string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing);

		// Token: 0x060021A3 RID: 8611 RVA: 0x000BA19C File Offset: 0x000B919C
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public TemplateEditingVerb[] GetTemplateEditingVerbs()
		{
			if ((ITemplateEditingService)this.GetService(typeof(ITemplateEditingService)) == null)
			{
				return null;
			}
			TemplatedControlDesigner.TemplateEditingVerbCollection templateEditingVerbsInternal = this.GetTemplateEditingVerbsInternal();
			TemplateEditingVerb[] array = new TemplateEditingVerb[templateEditingVerbsInternal.Count];
			((ICollection)templateEditingVerbsInternal).CopyTo(array, 0);
			return array;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000BA1E0 File Offset: 0x000B91E0
		private TemplatedControlDesigner.TemplateEditingVerbCollection GetTemplateEditingVerbsInternal()
		{
			TemplatedControlDesigner.TemplateEditingVerbCollection templateEditingVerbCollection = new TemplatedControlDesigner.TemplateEditingVerbCollection();
			TemplateEditingVerb[] cachedTemplateEditingVerbs = this.GetCachedTemplateEditingVerbs();
			if (cachedTemplateEditingVerbs != null && cachedTemplateEditingVerbs.Length > 0)
			{
				for (int i = 0; i < cachedTemplateEditingVerbs.Length; i++)
				{
					if (this._currentTemplateGroup != null && this._currentTemplateGroup.Verb == cachedTemplateEditingVerbs[i])
					{
						cachedTemplateEditingVerbs[i].Checked = true;
					}
					else
					{
						cachedTemplateEditingVerbs[i].Checked = false;
					}
					templateEditingVerbCollection.Add(cachedTemplateEditingVerbs[i]);
				}
			}
			return templateEditingVerbCollection;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000BA248 File Offset: 0x000B9248
		protected ITemplate GetTemplateFromText(string text)
		{
			return this.GetTemplateFromText(text, null);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000BA254 File Offset: 0x000B9254
		internal ITemplate GetTemplateFromText(string text, ITemplate currentTemplate)
		{
			if (text == null || text.Length == 0)
			{
				throw new ArgumentNullException("text");
			}
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			try
			{
				ITemplate template = ControlParser.ParseTemplate(designerHost, text);
				if (template != null)
				{
					return template;
				}
			}
			catch
			{
			}
			return currentTemplate;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000BA2C0 File Offset: 0x000B92C0
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual Type GetTemplatePropertyParentType(string templateName)
		{
			return base.Component.GetType();
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000BA2CD File Offset: 0x000B92CD
		protected string GetTextFromTemplate(ITemplate template)
		{
			if (template == null)
			{
				throw new ArgumentNullException("template");
			}
			if (template is TemplateBuilder)
			{
				return ((TemplateBuilder)template).Text;
			}
			return string.Empty;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000BA2F6 File Offset: 0x000B92F6
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.ViewEvent += this.OnViewEvent;
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000BA32B File Offset: 0x000B932B
		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override void OnBehaviorAttached()
		{
			if (this.InTemplateModeInternal)
			{
				this.ActiveTemplateEditingFrame.Close(false);
				this.ActiveTemplateEditingFrame.Dispose();
				this._currentTemplateGroup = null;
				TypeDescriptor.Refresh(base.Component);
			}
			base.OnBehaviorAttached();
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000BA364 File Offset: 0x000B9364
		public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			base.OnComponentChanged(sender, ce);
			if (this.InTemplateModeInternal && ce.Member != null && ce.NewValue != null && ce.Member.Name.Equals("ID"))
			{
				this.ActiveTemplateEditingFrame.UpdateControlName(ce.NewValue.ToString());
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000BA3C0 File Offset: 0x000B93C0
		public override void OnSetParent()
		{
			Control control = (Control)base.Component;
			bool flag = false;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ITemplateEditingService templateEditingService = (ITemplateEditingService)designerHost.GetService(typeof(ITemplateEditingService));
			if (templateEditingService != null)
			{
				flag = true;
				Control control2 = control.Parent;
				Control page = control.Page;
				while (control2 != null && control2 != page)
				{
					IDesigner designer = designerHost.GetDesigner(control2);
					TemplatedControlDesigner templatedControlDesigner = designer as TemplatedControlDesigner;
					if (templatedControlDesigner != null)
					{
						flag = templateEditingService.SupportsNestedTemplateEditing;
						break;
					}
					control2 = control2.Parent;
				}
			}
			this.EnableTemplateEditing(flag);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x000BA458 File Offset: 0x000B9458
		private void OnTemplateEditingVerbInvoked(object sender, EventArgs e)
		{
			TemplateEditingVerb templateEditingVerb = (TemplateEditingVerb)sender;
			if (templateEditingVerb.EditingFrame == null)
			{
				templateEditingVerb.EditingFrame = this.CreateTemplateEditingFrame(templateEditingVerb);
			}
			if (templateEditingVerb.EditingFrame != null)
			{
				templateEditingVerb.EditingFrame.Verb = templateEditingVerb;
				this.EnterTemplateModeInternal(templateEditingVerb.EditingFrame);
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000BA4A1 File Offset: 0x000B94A1
		protected virtual void OnTemplateModeChanged()
		{
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000BA4A4 File Offset: 0x000B94A4
		internal void OnTemplateModeChangedInternal(TemplateModeChangedEventArgs e)
		{
			TemplateGroup newTemplateGroup = e.NewTemplateGroup;
			if (newTemplateGroup != null)
			{
				if (this._currentTemplateGroup != newTemplateGroup)
				{
					this.EnterTemplateModeInternal(((TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup)newTemplateGroup).Frame);
					return;
				}
			}
			else
			{
				this.ExitTemplateModeInternal(false, false, true);
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000BA4DF File Offset: 0x000B94DF
		private void OnViewEvent(object sender, ViewEventArgs e)
		{
			if (e.EventType == ViewEvent.TemplateModeChanged)
			{
				this.OnTemplateModeChangedInternal((TemplateModeChangedEventArgs)e.EventArgs);
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000BA4FF File Offset: 0x000B94FF
		private void RaiseTemplateModeChanged()
		{
			if (this.BehaviorInternal != null)
			{
				((IControlDesignerBehavior)this.BehaviorInternal).OnTemplateModeChanged();
			}
			this.OnTemplateModeChanged();
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000BA51F File Offset: 0x000B951F
		protected void SaveActiveTemplateEditingFrame()
		{
			this.ActiveTemplateEditingFrame.Save();
		}

		// Token: 0x060021B3 RID: 8627
		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public abstract void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent);

		// Token: 0x060021B4 RID: 8628 RVA: 0x000BA52C File Offset: 0x000B952C
		public override void UpdateDesignTimeHtml()
		{
			if (!this.InTemplateModeInternal)
			{
				base.UpdateDesignTimeHtml();
			}
		}

		// Token: 0x0400181A RID: 6170
		private bool enableTemplateEditing;

		// Token: 0x0400181B RID: 6171
		private TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup _currentTemplateGroup;

		// Token: 0x0400181C RID: 6172
		private IDictionary _templateGroupTable;

		// Token: 0x02000393 RID: 915
		private class TemplatedControlDesignerTemplateDefinition : TemplateDefinition
		{
			// Token: 0x060021C3 RID: 8643 RVA: 0x000BA6FC File Offset: 0x000B96FC
			public TemplatedControlDesignerTemplateDefinition(string name, Style style, TemplatedControlDesigner parent, ITemplateEditingFrame frame)
				: base(parent, name, parent.Component, name, style)
			{
				this._parent = parent;
				this._frame = frame;
				base.Properties[typeof(Control)] = (Control)this._parent.Component;
			}

			// Token: 0x17000626 RID: 1574
			// (get) Token: 0x060021C4 RID: 8644 RVA: 0x000BA750 File Offset: 0x000B9750
			public override bool AllowEditing
			{
				get
				{
					bool flag;
					this._parent.GetTemplateContent(this._frame, base.Name, out flag);
					return flag;
				}
			}

			// Token: 0x17000627 RID: 1575
			// (get) Token: 0x060021C5 RID: 8645 RVA: 0x000BA778 File Offset: 0x000B9778
			// (set) Token: 0x060021C6 RID: 8646 RVA: 0x000BA79E File Offset: 0x000B979E
			public override string Content
			{
				get
				{
					bool flag;
					return this._parent.GetTemplateContent(this._frame, base.Name, out flag);
				}
				set
				{
					this._parent.SetTemplateContent(this._frame, base.Name, value);
					this._parent.Tag.SetDirty(true);
					this._parent.UpdateDesignTimeHtml();
				}
			}

			// Token: 0x04001823 RID: 6179
			private TemplatedControlDesigner _parent;

			// Token: 0x04001824 RID: 6180
			private ITemplateEditingFrame _frame;
		}

		// Token: 0x02000395 RID: 917
		private class TemplatedControlDesignerTemplateGroup : TemplateGroup
		{
			// Token: 0x060021CF RID: 8655 RVA: 0x000BA877 File Offset: 0x000B9877
			public TemplatedControlDesignerTemplateGroup(TemplateEditingVerb verb, ITemplateEditingFrame frame)
				: base(verb.Text, frame.ControlStyle)
			{
				this._frame = frame;
				this._verb = verb;
			}

			// Token: 0x1700062C RID: 1580
			// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000BA899 File Offset: 0x000B9899
			public ITemplateEditingFrame Frame
			{
				get
				{
					return this._frame;
				}
			}

			// Token: 0x1700062D RID: 1581
			// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000BA8A1 File Offset: 0x000B98A1
			public TemplateEditingVerb Verb
			{
				get
				{
					return this._verb;
				}
			}

			// Token: 0x04001829 RID: 6185
			private ITemplateEditingFrame _frame;

			// Token: 0x0400182A RID: 6186
			private TemplateEditingVerb _verb;
		}

		// Token: 0x02000396 RID: 918
		private class TemplateEditingVerbCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060021D2 RID: 8658 RVA: 0x000BA8A9 File Offset: 0x000B98A9
			public TemplateEditingVerbCollection()
			{
			}

			// Token: 0x060021D3 RID: 8659 RVA: 0x000BA8B4 File Offset: 0x000B98B4
			internal TemplateEditingVerbCollection(TemplateEditingVerb[] verbs)
			{
				for (int i = 0; i < verbs.Length; i++)
				{
					this.Add(verbs[i]);
				}
			}

			// Token: 0x1700062E RID: 1582
			// (get) Token: 0x060021D4 RID: 8660 RVA: 0x000BA8DF File Offset: 0x000B98DF
			public int Count
			{
				get
				{
					return this.InternalList.Count;
				}
			}

			// Token: 0x1700062F RID: 1583
			// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000BA8EC File Offset: 0x000B98EC
			private ArrayList InternalList
			{
				get
				{
					if (this._list == null)
					{
						this._list = new ArrayList();
					}
					return this._list;
				}
			}

			// Token: 0x17000630 RID: 1584
			public TemplateEditingVerb this[int index]
			{
				get
				{
					return (TemplateEditingVerb)this.InternalList[index];
				}
				set
				{
					this.InternalList[index] = value;
				}
			}

			// Token: 0x060021D8 RID: 8664 RVA: 0x000BA929 File Offset: 0x000B9929
			public int Add(TemplateEditingVerb verb)
			{
				return this.InternalList.Add(verb);
			}

			// Token: 0x060021D9 RID: 8665 RVA: 0x000BA937 File Offset: 0x000B9937
			public void Clear()
			{
				this.InternalList.Clear();
			}

			// Token: 0x060021DA RID: 8666 RVA: 0x000BA944 File Offset: 0x000B9944
			public bool Contains(TemplateEditingVerb verb)
			{
				return this.InternalList.Contains(verb);
			}

			// Token: 0x060021DB RID: 8667 RVA: 0x000BA952 File Offset: 0x000B9952
			public int IndexOf(TemplateEditingVerb verb)
			{
				return this.InternalList.IndexOf(verb);
			}

			// Token: 0x060021DC RID: 8668 RVA: 0x000BA960 File Offset: 0x000B9960
			public void Insert(int index, TemplateEditingVerb verb)
			{
				this.InternalList.Insert(index, verb);
			}

			// Token: 0x060021DD RID: 8669 RVA: 0x000BA96F File Offset: 0x000B996F
			public void Remove(TemplateEditingVerb verb)
			{
				this.InternalList.Remove(verb);
			}

			// Token: 0x060021DE RID: 8670 RVA: 0x000BA97D File Offset: 0x000B997D
			public void RemoveAt(int index)
			{
				this.InternalList.RemoveAt(index);
			}

			// Token: 0x17000631 RID: 1585
			// (get) Token: 0x060021DF RID: 8671 RVA: 0x000BA98B File Offset: 0x000B998B
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x17000632 RID: 1586
			// (get) Token: 0x060021E0 RID: 8672 RVA: 0x000BA993 File Offset: 0x000B9993
			bool IList.IsFixedSize
			{
				get
				{
					return this.InternalList.IsFixedSize;
				}
			}

			// Token: 0x17000633 RID: 1587
			// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000BA9A0 File Offset: 0x000B99A0
			bool IList.IsReadOnly
			{
				get
				{
					return this.InternalList.IsReadOnly;
				}
			}

			// Token: 0x17000634 RID: 1588
			// (get) Token: 0x060021E2 RID: 8674 RVA: 0x000BA9AD File Offset: 0x000B99AD
			bool ICollection.IsSynchronized
			{
				get
				{
					return this.InternalList.IsSynchronized;
				}
			}

			// Token: 0x17000635 RID: 1589
			// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000BA9BA File Offset: 0x000B99BA
			object ICollection.SyncRoot
			{
				get
				{
					return this.InternalList.SyncRoot;
				}
			}

			// Token: 0x17000636 RID: 1590
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (!(value is TemplateEditingVerb))
					{
						throw new ArgumentException();
					}
					this[index] = (TemplateEditingVerb)value;
				}
			}

			// Token: 0x060021E6 RID: 8678 RVA: 0x000BA9ED File Offset: 0x000B99ED
			int IList.Add(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.Add((TemplateEditingVerb)o);
			}

			// Token: 0x060021E7 RID: 8679 RVA: 0x000BAA09 File Offset: 0x000B9A09
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x060021E8 RID: 8680 RVA: 0x000BAA11 File Offset: 0x000B9A11
			bool IList.Contains(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.Contains((TemplateEditingVerb)o);
			}

			// Token: 0x060021E9 RID: 8681 RVA: 0x000BAA2D File Offset: 0x000B9A2D
			void ICollection.CopyTo(Array array, int index)
			{
				this.InternalList.CopyTo(array, index);
			}

			// Token: 0x060021EA RID: 8682 RVA: 0x000BAA3C File Offset: 0x000B9A3C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.InternalList.GetEnumerator();
			}

			// Token: 0x060021EB RID: 8683 RVA: 0x000BAA49 File Offset: 0x000B9A49
			int IList.IndexOf(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.IndexOf((TemplateEditingVerb)o);
			}

			// Token: 0x060021EC RID: 8684 RVA: 0x000BAA65 File Offset: 0x000B9A65
			void IList.Insert(int index, object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				this.Insert(index, (TemplateEditingVerb)o);
			}

			// Token: 0x060021ED RID: 8685 RVA: 0x000BAA82 File Offset: 0x000B9A82
			void IList.Remove(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				this.Remove((TemplateEditingVerb)o);
			}

			// Token: 0x060021EE RID: 8686 RVA: 0x000BAA9E File Offset: 0x000B9A9E
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			// Token: 0x0400182B RID: 6187
			private ArrayList _list;
		}
	}
}
