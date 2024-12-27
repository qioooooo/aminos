using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027F RID: 639
	internal class RichTextBoxContextMenu : ContextMenu
	{
		// Token: 0x060017C6 RID: 6086 RVA: 0x0007BB48 File Offset: 0x0007AB48
		public RichTextBoxContextMenu(RichTextBox parent)
		{
			this.undoMenu = new MenuItem(SR.GetString("StandardMenuUndo"), new EventHandler(this.undoMenu_Clicked));
			this.cutMenu = new MenuItem(SR.GetString("StandardMenuCut"), new EventHandler(this.cutMenu_Clicked));
			this.copyMenu = new MenuItem(SR.GetString("StandardMenuCopy"), new EventHandler(this.copyMenu_Clicked));
			this.pasteMenu = new MenuItem(SR.GetString("StandardMenuPaste"), new EventHandler(this.pasteMenu_Clicked));
			this.deleteMenu = new MenuItem(SR.GetString("StandardMenuDelete"), new EventHandler(this.deleteMenu_Clicked));
			this.selectAllMenu = new MenuItem(SR.GetString("StandardMenuSelectAll"), new EventHandler(this.selectAllMenu_Clicked));
			MenuItem menuItem = new MenuItem("-");
			MenuItem menuItem2 = new MenuItem("-");
			base.MenuItems.Add(this.undoMenu);
			base.MenuItems.Add(menuItem);
			base.MenuItems.Add(this.cutMenu);
			base.MenuItems.Add(this.copyMenu);
			base.MenuItems.Add(this.pasteMenu);
			base.MenuItems.Add(this.deleteMenu);
			base.MenuItems.Add(menuItem2);
			base.MenuItems.Add(this.selectAllMenu);
			this.parent = parent;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0007BCC4 File Offset: 0x0007ACC4
		protected override void OnPopup(EventArgs e)
		{
			if (this.parent.SelectionLength > 0)
			{
				this.cutMenu.Enabled = true;
				this.copyMenu.Enabled = true;
				this.deleteMenu.Enabled = true;
			}
			else
			{
				this.cutMenu.Enabled = false;
				this.copyMenu.Enabled = false;
				this.deleteMenu.Enabled = false;
			}
			if (Clipboard.GetText() != null)
			{
				this.pasteMenu.Enabled = true;
			}
			else
			{
				this.pasteMenu.Enabled = false;
			}
			if (this.parent.CanUndo)
			{
				this.undoMenu.Enabled = true;
				return;
			}
			this.undoMenu.Enabled = false;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0007BD70 File Offset: 0x0007AD70
		private void cutMenu_Clicked(object sender, EventArgs e)
		{
			Clipboard.SetText(this.parent.SelectedText);
			this.parent.SelectedText = "";
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0007BD92 File Offset: 0x0007AD92
		private void copyMenu_Clicked(object sender, EventArgs e)
		{
			Clipboard.SetText(this.parent.SelectedText);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0007BDA4 File Offset: 0x0007ADA4
		private void deleteMenu_Clicked(object sender, EventArgs e)
		{
			this.parent.SelectedText = "";
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0007BDB6 File Offset: 0x0007ADB6
		private void pasteMenu_Clicked(object sender, EventArgs e)
		{
			this.parent.SelectedText = Clipboard.GetText();
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0007BDC8 File Offset: 0x0007ADC8
		private void selectAllMenu_Clicked(object sender, EventArgs e)
		{
			this.parent.SelectAll();
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0007BDD5 File Offset: 0x0007ADD5
		private void undoMenu_Clicked(object sender, EventArgs e)
		{
			this.parent.Undo();
		}

		// Token: 0x040013AD RID: 5037
		private MenuItem undoMenu;

		// Token: 0x040013AE RID: 5038
		private MenuItem cutMenu;

		// Token: 0x040013AF RID: 5039
		private MenuItem copyMenu;

		// Token: 0x040013B0 RID: 5040
		private MenuItem pasteMenu;

		// Token: 0x040013B1 RID: 5041
		private MenuItem deleteMenu;

		// Token: 0x040013B2 RID: 5042
		private MenuItem selectAllMenu;

		// Token: 0x040013B3 RID: 5043
		private RichTextBox parent;
	}
}
