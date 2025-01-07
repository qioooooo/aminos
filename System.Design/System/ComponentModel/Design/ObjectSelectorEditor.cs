using System;
using System.Design;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	public abstract class ObjectSelectorEditor : UITypeEditor
	{
		public ObjectSelectorEditor()
		{
		}

		public ObjectSelectorEditor(bool subObjectSelector)
		{
			this.SubObjectSelector = subObjectSelector;
		}

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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public bool EqualsToValue(object value)
		{
			return value == this.currValue;
		}

		protected virtual void FillTreeWithData(ObjectSelectorEditor.Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
		{
			selector.Clear();
		}

		public virtual void SetValue(object value)
		{
			this.currValue = value;
		}

		public bool SubObjectSelector;

		protected object prevValue;

		protected object currValue;

		private ObjectSelectorEditor.Selector selector;

		public class Selector : TreeView
		{
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

			public void Clear()
			{
				this.clickSeen = false;
				base.Nodes.Clear();
			}

			protected void OnAfterSelect(object sender, TreeViewEventArgs e)
			{
				if (this.clickSeen)
				{
					this.ChooseSelectedNodeIfEqual();
					this.clickSeen = false;
				}
			}

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

			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				char keyChar = e.KeyChar;
				if (keyChar == '\r')
				{
					e.Handled = true;
				}
				base.OnKeyPress(e);
			}

			protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
			{
				if (e.Node == base.SelectedNode)
				{
					this.ChooseSelectedNodeIfEqual();
				}
				base.OnNodeMouseClick(e);
			}

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

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.clickSeen = false;
				this.SetSelection(value, base.Nodes);
			}

			public void Stop()
			{
				this.edSvc = null;
			}

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

			private ObjectSelectorEditor editor;

			private IWindowsFormsEditorService edSvc;

			public bool clickSeen;
		}

		public class SelectorNode : TreeNode
		{
			public SelectorNode(string label, object value)
				: base(label)
			{
				this.value = value;
			}

			public object value;
		}
	}
}
