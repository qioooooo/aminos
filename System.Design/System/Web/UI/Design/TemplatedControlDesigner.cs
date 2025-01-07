using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class TemplatedControlDesigner : ControlDesigner
	{
		public TemplatedControlDesigner()
		{
			this.enableTemplateEditing = true;
		}

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

		public bool CanEnterTemplateMode
		{
			get
			{
				return this.enableTemplateEditing;
			}
		}

		protected override bool DataBindingsEnabled
		{
			get
			{
				return (!this.InTemplateModeInternal || !this.HidePropertiesInTemplateMode) && base.DataBindingsEnabled;
			}
		}

		[Obsolete("The recommended alternative is System.Web.UI.Design.ControlDesigner.InTemplateMode. http://go.microsoft.com/fwlink/?linkid=14202")]
		public new bool InTemplateMode
		{
			get
			{
				return this._currentTemplateGroup != null;
			}
		}

		internal bool InTemplateModeInternal
		{
			get
			{
				return this.InTemplateMode;
			}
		}

		internal EventHandler TemplateEditingVerbHandler
		{
			get
			{
				return new EventHandler(this.OnTemplateEditingVerbInvoked);
			}
		}

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

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected abstract ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb);

		private void EnableTemplateEditing(bool enable)
		{
			this.enableTemplateEditing = enable;
		}

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

		private void EnterTemplateModeInternal(ITemplateEditingFrame newTemplateEditingFrame)
		{
			this.EnterTemplateMode(newTemplateEditingFrame);
		}

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

		private void ExitTemplateModeInternal(bool fSwitchingTemplates, bool fNested, bool fSave)
		{
			this.ExitTemplateMode(fSwitchingTemplates, fNested, fSave);
		}

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		protected abstract TemplateEditingVerb[] GetCachedTemplateEditingVerbs();

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

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetTemplateContainerDataItemProperty(string templateName)
		{
			return string.Empty;
		}

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual IEnumerable GetTemplateContainerDataSource(string templateName)
		{
			return null;
		}

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public abstract string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing);

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

		protected ITemplate GetTemplateFromText(string text)
		{
			return this.GetTemplateFromText(text, null);
		}

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

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual Type GetTemplatePropertyParentType(string templateName)
		{
			return base.Component.GetType();
		}

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

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (base.View != null)
			{
				base.View.ViewEvent += this.OnViewEvent;
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

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

		public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			base.OnComponentChanged(sender, ce);
			if (this.InTemplateModeInternal && ce.Member != null && ce.NewValue != null && ce.Member.Name.Equals("ID"))
			{
				this.ActiveTemplateEditingFrame.UpdateControlName(ce.NewValue.ToString());
			}
		}

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

		protected virtual void OnTemplateModeChanged()
		{
		}

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

		private void OnViewEvent(object sender, ViewEventArgs e)
		{
			if (e.EventType == ViewEvent.TemplateModeChanged)
			{
				this.OnTemplateModeChangedInternal((TemplateModeChangedEventArgs)e.EventArgs);
			}
		}

		private void RaiseTemplateModeChanged()
		{
			if (this.BehaviorInternal != null)
			{
				((IControlDesignerBehavior)this.BehaviorInternal).OnTemplateModeChanged();
			}
			this.OnTemplateModeChanged();
		}

		protected void SaveActiveTemplateEditingFrame()
		{
			this.ActiveTemplateEditingFrame.Save();
		}

		[Obsolete("Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public abstract void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent);

		public override void UpdateDesignTimeHtml()
		{
			if (!this.InTemplateModeInternal)
			{
				base.UpdateDesignTimeHtml();
			}
		}

		private bool enableTemplateEditing;

		private TemplatedControlDesigner.TemplatedControlDesignerTemplateGroup _currentTemplateGroup;

		private IDictionary _templateGroupTable;

		private class TemplatedControlDesignerTemplateDefinition : TemplateDefinition
		{
			public TemplatedControlDesignerTemplateDefinition(string name, Style style, TemplatedControlDesigner parent, ITemplateEditingFrame frame)
				: base(parent, name, parent.Component, name, style)
			{
				this._parent = parent;
				this._frame = frame;
				base.Properties[typeof(Control)] = (Control)this._parent.Component;
			}

			public override bool AllowEditing
			{
				get
				{
					bool flag;
					this._parent.GetTemplateContent(this._frame, base.Name, out flag);
					return flag;
				}
			}

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

			private TemplatedControlDesigner _parent;

			private ITemplateEditingFrame _frame;
		}

		private class TemplatedControlDesignerTemplateGroup : TemplateGroup
		{
			public TemplatedControlDesignerTemplateGroup(TemplateEditingVerb verb, ITemplateEditingFrame frame)
				: base(verb.Text, frame.ControlStyle)
			{
				this._frame = frame;
				this._verb = verb;
			}

			public ITemplateEditingFrame Frame
			{
				get
				{
					return this._frame;
				}
			}

			public TemplateEditingVerb Verb
			{
				get
				{
					return this._verb;
				}
			}

			private ITemplateEditingFrame _frame;

			private TemplateEditingVerb _verb;
		}

		private class TemplateEditingVerbCollection : IList, ICollection, IEnumerable
		{
			public TemplateEditingVerbCollection()
			{
			}

			internal TemplateEditingVerbCollection(TemplateEditingVerb[] verbs)
			{
				for (int i = 0; i < verbs.Length; i++)
				{
					this.Add(verbs[i]);
				}
			}

			public int Count
			{
				get
				{
					return this.InternalList.Count;
				}
			}

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

			public int Add(TemplateEditingVerb verb)
			{
				return this.InternalList.Add(verb);
			}

			public void Clear()
			{
				this.InternalList.Clear();
			}

			public bool Contains(TemplateEditingVerb verb)
			{
				return this.InternalList.Contains(verb);
			}

			public int IndexOf(TemplateEditingVerb verb)
			{
				return this.InternalList.IndexOf(verb);
			}

			public void Insert(int index, TemplateEditingVerb verb)
			{
				this.InternalList.Insert(index, verb);
			}

			public void Remove(TemplateEditingVerb verb)
			{
				this.InternalList.Remove(verb);
			}

			public void RemoveAt(int index)
			{
				this.InternalList.RemoveAt(index);
			}

			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					return this.InternalList.IsFixedSize;
				}
			}

			bool IList.IsReadOnly
			{
				get
				{
					return this.InternalList.IsReadOnly;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return this.InternalList.IsSynchronized;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return this.InternalList.SyncRoot;
				}
			}

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

			int IList.Add(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.Add((TemplateEditingVerb)o);
			}

			void IList.Clear()
			{
				this.Clear();
			}

			bool IList.Contains(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.Contains((TemplateEditingVerb)o);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				this.InternalList.CopyTo(array, index);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.InternalList.GetEnumerator();
			}

			int IList.IndexOf(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				return this.IndexOf((TemplateEditingVerb)o);
			}

			void IList.Insert(int index, object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				this.Insert(index, (TemplateEditingVerb)o);
			}

			void IList.Remove(object o)
			{
				if (!(o is TemplateEditingVerb))
				{
					throw new ArgumentException();
				}
				this.Remove((TemplateEditingVerb)o);
			}

			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			private ArrayList _list;
		}
	}
}
