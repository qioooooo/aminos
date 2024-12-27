using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000016 RID: 22
	internal sealed class DesignerToolboxInfo : IDisposable
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00005138 File Offset: 0x00004138
		internal DesignerToolboxInfo(ToolboxService toolboxService, IDesignerHost host)
		{
			this._toolboxService = toolboxService;
			this._host = host;
			this._selectionService = host.GetService(typeof(ISelectionService)) as ISelectionService;
			if (this._selectionService != null)
			{
				this._selectionService.SelectionChanged += this.OnSelectionChanged;
			}
			if (this._host.RootComponent != null)
			{
				this._host.RootComponent.Disposed += this.OnDesignerDisposed;
			}
			TypeDescriptor.Refreshed += this.OnTypeDescriptorRefresh;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000051CD File Offset: 0x000041CD
		internal IDesignerHost DesignerHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000051D5 File Offset: 0x000041D5
		internal ICollection Filter
		{
			get
			{
				if (this._filter == null)
				{
					this.Update();
				}
				return this._filter;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000051EC File Offset: 0x000041EC
		internal IToolboxUser ToolboxUser
		{
			get
			{
				if (this._toolboxUser == null)
				{
					this.Update();
				}
				return this._toolboxUser;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005203 File Offset: 0x00004203
		private void OnTypeDescriptorRefresh(RefreshEventArgs r)
		{
			if (r.ComponentChanged == this._filterDesigner)
			{
				this._filter = null;
				this._filterDesigner = null;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005224 File Offset: 0x00004224
		public AttributeCollection GetDesignerAttributes(IDesigner designer)
		{
			if (designer == null)
			{
				throw new ArgumentNullException("designer");
			}
			if (this._attributeHash == null)
			{
				this._attributeHash = new Hashtable();
			}
			else
			{
				this._attributeHash.Clear();
			}
			if (!(designer is ITreeDesigner))
			{
				IComponent rootComponent = this._host.RootComponent;
				if (rootComponent != null)
				{
					this.RecurseDesignerTree(this._host.GetDesigner(rootComponent), this._attributeHash);
				}
			}
			this.RecurseDesignerTree(designer, this._attributeHash);
			Attribute[] array = new Attribute[this._attributeHash.Values.Count];
			this._attributeHash.Values.CopyTo(array, 0);
			return new AttributeCollection(array);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000052CC File Offset: 0x000042CC
		private void RecurseDesignerTree(IDesigner designer, Hashtable table)
		{
			ITreeDesigner treeDesigner = designer as ITreeDesigner;
			if (treeDesigner != null)
			{
				IDesigner parent = treeDesigner.Parent;
				if (parent != null)
				{
					this.RecurseDesignerTree(parent, table);
				}
			}
			foreach (object obj in TypeDescriptor.GetAttributes(designer))
			{
				Attribute attribute = (Attribute)obj;
				table[attribute.TypeId] = attribute;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000534C File Offset: 0x0000434C
		private void OnDesignerDisposed(object sender, EventArgs e)
		{
			this._host.RemoveService(typeof(DesignerToolboxInfo));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005363 File Offset: 0x00004363
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (this.Update())
			{
				this._toolboxService.OnDesignerInfoChanged(this);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000537C File Offset: 0x0000437C
		private bool Update()
		{
			bool flag = false;
			IDesigner designer = null;
			IComponent component = this._selectionService.PrimarySelection as IComponent;
			if (component != null)
			{
				designer = this._host.GetDesigner(component);
			}
			if (designer == null)
			{
				component = this._host.RootComponent;
				if (component != null)
				{
					designer = this._host.GetDesigner(component);
				}
			}
			if (designer != this._filterDesigner)
			{
				ArrayList arrayList;
				if (designer != null)
				{
					AttributeCollection designerAttributes = this.GetDesignerAttributes(designer);
					arrayList = new ArrayList(designerAttributes.Count);
					using (IEnumerator enumerator = designerAttributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Attribute attribute = (Attribute)obj;
							if (attribute is ToolboxItemFilterAttribute)
							{
								arrayList.Add(attribute);
							}
						}
						goto IL_00BA;
					}
				}
				arrayList = new ArrayList();
				IL_00BA:
				if (this._filter == null)
				{
					flag = true;
				}
				else if (this._filter.Count != arrayList.Count)
				{
					flag = true;
				}
				else
				{
					IEnumerator enumerator2 = this._filter.GetEnumerator();
					IEnumerator enumerator3 = arrayList.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						enumerator2.MoveNext();
						if (!enumerator3.Current.Equals(enumerator2.Current))
						{
							flag = true;
							break;
						}
						ToolboxItemFilterAttribute toolboxItemFilterAttribute = (ToolboxItemFilterAttribute)enumerator3.Current;
						if (toolboxItemFilterAttribute.FilterType == ToolboxItemFilterType.Custom)
						{
							flag = true;
							break;
						}
					}
				}
				this._filter = arrayList;
				this._filterDesigner = designer;
				this._toolboxUser = this._filterDesigner as IToolboxUser;
				if (this._toolboxUser == null)
				{
					ITreeDesigner treeDesigner = this._filterDesigner as ITreeDesigner;
					while (this._toolboxUser == null && treeDesigner != null)
					{
						IDesigner parent = treeDesigner.Parent;
						this._toolboxUser = parent as IToolboxUser;
						treeDesigner = parent as ITreeDesigner;
					}
				}
				if (this._toolboxUser == null && this._host.RootComponent != null)
				{
					this._toolboxUser = this._host.GetDesigner(this._host.RootComponent) as IToolboxUser;
				}
			}
			if (this._filter == null)
			{
				this._filter = new ArrayList();
			}
			return flag;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005580 File Offset: 0x00004580
		void IDisposable.Dispose()
		{
			if (this._selectionService != null)
			{
				this._selectionService.SelectionChanged -= this.OnSelectionChanged;
			}
			if (this._host.RootComponent != null)
			{
				this._host.RootComponent.Disposed -= this.OnDesignerDisposed;
			}
			TypeDescriptor.Refreshed -= this.OnTypeDescriptorRefresh;
		}

		// Token: 0x0400007C RID: 124
		private ToolboxService _toolboxService;

		// Token: 0x0400007D RID: 125
		private IDesignerHost _host;

		// Token: 0x0400007E RID: 126
		private ISelectionService _selectionService;

		// Token: 0x0400007F RID: 127
		private ArrayList _filter;

		// Token: 0x04000080 RID: 128
		private IDesigner _filterDesigner;

		// Token: 0x04000081 RID: 129
		private IToolboxUser _toolboxUser;

		// Token: 0x04000082 RID: 130
		private Hashtable _attributeHash;
	}
}
