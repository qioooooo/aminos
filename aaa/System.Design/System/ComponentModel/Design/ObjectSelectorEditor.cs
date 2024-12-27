using System;
using System.Design;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	// Token: 0x0200013A RID: 314
	public abstract class ObjectSelectorEditor : UITypeEditor
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x000301A1 File Offset: 0x0002F1A1
		public ObjectSelectorEditor()
		{
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000301A9 File Offset: 0x0002F1A9
		public ObjectSelectorEditor(bool subObjectSelector)
		{
			this.SubObjectSelector = subObjectSelector;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000301B8 File Offset: 0x0002F1B8
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.selector == null)
					{
						this.selector = new ObjectSelectorEditor.Selector(this);
					}
					this.prevValue = value;
					this.currValue = value;
					this.FillTreeWithData(this.selector, context, provider);
					this.selector.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.selector);
					this.selector.Stop();
					if (this.prevValue != this.currValue)
					{
						value = this.currValue;
					}
				}
			}
			return value;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0003024F File Offset: 0x0002F24F
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00030252 File Offset: 0x0002F252
		public bool EqualsToValue(object value)
		{
			return value == this.currValue;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00030260 File Offset: 0x0002F260
		protected virtual void FillTreeWithData(ObjectSelectorEditor.Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
		{
			selector.Clear();
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00030268 File Offset: 0x0002F268
		public virtual void SetValue(object value)
		{
			this.currValue = value;
		}

		// Token: 0x04000E92 RID: 3730
		public bool SubObjectSelector;

		// Token: 0x04000E93 RID: 3731
		protected object prevValue;

		// Token: 0x04000E94 RID: 3732
		protected object currValue;

		// Token: 0x04000E95 RID: 3733
		private ObjectSelectorEditor.Selector selector;

		// Token: 0x0200013B RID: 315
		public class Selector : TreeView
		{
			// Token: 0x06000C48 RID: 3144 RVA: 0x00030274 File Offset: 0x0002F274
			public Selector(ObjectSelectorEditor editor)
			{
				this.CreateHandle();
				this.editor = editor;
				base.BorderStyle = BorderStyle.None;
				base.FullRowSelect = !editor.SubObjectSelector;
				base.Scrollable = true;
				base.CheckBoxes = false;
				base.ShowPlusMinus = editor.SubObjectSelector;
				base.ShowLines = editor.SubObjectSelector;
				base.ShowRootLines = editor.SubObjectSelector;
				base.AfterSelect += this.OnAfterSelect;
			}

			// Token: 0x06000C49 RID: 3145 RVA: 0x000302F0 File Offset: 0x0002F2F0
			public ObjectSelectorEditor.SelectorNode AddNode(string label, object value, ObjectSelectorEditor.SelectorNode parent)
			{
				ObjectSelectorEditor.SelectorNode selectorNode = new ObjectSelectorEditor.SelectorNode(label, value);
				if (parent != null)
				{
					parent.Nodes.Add(selectorNode);
				}
				else
				{
					base.Nodes.Add(selectorNode);
				}
				return selectorNode;
			}

			// Token: 0x06000C4A RID: 3146 RVA: 0x00030328 File Offset: 0x0002F328
			private bool ChooseSelectedNodeIfEqual()
			{
				if (this.editor != null && this.edSvc != null)
				{
					this.editor.SetValue(((ObjectSelectorEditor.SelectorNode)base.SelectedNode).value);
					if (this.editor.EqualsToValue(((ObjectSelectorEditor.SelectorNode)base.SelectedNode).value))
					{
						this.edSvc.CloseDropDown();
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000C4B RID: 3147 RVA: 0x0003038B File Offset: 0x0002F38B
			public void Clear()
			{
				this.clickSeen = false;
				base.Nodes.Clear();
			}

			// Token: 0x06000C4C RID: 3148 RVA: 0x0003039F File Offset: 0x0002F39F
			protected void OnAfterSelect(object sender, TreeViewEventArgs e)
			{
				if (this.clickSeen)
				{
					this.ChooseSelectedNodeIfEqual();
					this.clickSeen = false;
				}
			}

			// Token: 0x06000C4D RID: 3149 RVA: 0x000303B8 File Offset: 0x0002F3B8
			protected override void OnKeyDown(KeyEventArgs e)
			{
				Keys keyCode = e.KeyCode;
				Keys keys = keyCode;
				if (keys != Keys.Return)
				{
					if (keys == Keys.Escape)
					{
						this.editor.SetValue(this.editor.prevValue);
						e.Handled = true;
						this.edSvc.CloseDropDown();
					}
				}
				else if (this.ChooseSelectedNodeIfEqual())
				{
					e.Handled = true;
				}
				base.OnKeyDown(e);
			}

			// Token: 0x06000C4E RID: 3150 RVA: 0x0003041C File Offset: 0x0002F41C
			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				char keyChar = e.KeyChar;
				if (keyChar == '\r')
				{
					e.Handled = true;
				}
				base.OnKeyPress(e);
			}

			// Token: 0x06000C4F RID: 3151 RVA: 0x00030443 File Offset: 0x0002F443
			protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
			{
				if (e.Node == base.SelectedNode)
				{
					this.ChooseSelectedNodeIfEqual();
				}
				base.OnNodeMouseClick(e);
			}

			// Token: 0x06000C50 RID: 3152 RVA: 0x00030464 File Offset: 0x0002F464
			public bool SetSelection(object value, TreeNodeCollection nodes)
			{
				TreeNode[] array;
				if (nodes == null)
				{
					array = new TreeNode[base.Nodes.Count];
					base.Nodes.CopyTo(array, 0);
				}
				else
				{
					array = new TreeNode[nodes.Count];
					nodes.CopyTo(array, 0);
				}
				int num = array.Length;
				if (num == 0)
				{
					return false;
				}
				for (int i = 0; i < num; i++)
				{
					if (((ObjectSelectorEditor.SelectorNode)array[i]).value == value)
					{
						base.SelectedNode = array[i];
						return true;
					}
					if (array[i].Nodes != null && array[i].Nodes.Count != 0)
					{
						array[i].Expand();
						if (this.SetSelection(value, array[i].Nodes))
						{
							return true;
						}
						array[i].Collapse();
					}
				}
				return false;
			}

			// Token: 0x06000C51 RID: 3153 RVA: 0x00030515 File Offset: 0x0002F515
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.clickSeen = false;
				this.SetSelection(value, base.Nodes);
			}

			// Token: 0x06000C52 RID: 3154 RVA: 0x00030533 File Offset: 0x0002F533
			public void Stop()
			{
				this.edSvc = null;
			}

			// Token: 0x06000C53 RID: 3155 RVA: 0x0003053C File Offset: 0x0002F53C
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 135)
				{
					if (msg != 512)
					{
						if (msg == 8270)
						{
							NativeMethods.NMTREEVIEW nmtreeview = (NativeMethods.NMTREEVIEW)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.NMTREEVIEW));
							if (nmtreeview.nmhdr.code == -2)
							{
								this.clickSeen = true;
							}
						}
					}
					else if (this.clickSeen)
					{
						this.clickSeen = false;
					}
					base.WndProc(ref m);
					return;
				}
				m.Result = (IntPtr)((long)m.Result | 4L);
			}

			// Token: 0x04000E96 RID: 3734
			private ObjectSelectorEditor editor;

			// Token: 0x04000E97 RID: 3735
			private IWindowsFormsEditorService edSvc;

			// Token: 0x04000E98 RID: 3736
			public bool clickSeen;
		}

		// Token: 0x0200013C RID: 316
		public class SelectorNode : TreeNode
		{
			// Token: 0x06000C54 RID: 3156 RVA: 0x000305CD File Offset: 0x0002F5CD
			public SelectorNode(string label, object value)
				: base(label)
			{
				this.value = value;
			}

			// Token: 0x04000E99 RID: 3737
			public object value;
		}
	}
}
