using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x02000104 RID: 260
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class UITypeEditor
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x00029384 File Offset: 0x00028384
		static UITypeEditor()
		{
			Hashtable hashtable = new Hashtable();
			hashtable[typeof(DateTime)] = "System.ComponentModel.Design.DateTimeEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Array)] = "System.ComponentModel.Design.ArrayEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(IList)] = "System.ComponentModel.Design.CollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(ICollection)] = "System.ComponentModel.Design.CollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(byte[])] = "System.ComponentModel.Design.BinaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Stream)] = "System.ComponentModel.Design.BinaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(string[])] = "System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			hashtable[typeof(Collection<string>)] = "System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			TypeDescriptor.AddEditorTable(typeof(UITypeEditor), hashtable);
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00029457 File Offset: 0x00028457
		public virtual bool IsDropDownResizable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0002945A File Offset: 0x0002845A
		public object EditValue(IServiceProvider provider, object value)
		{
			return this.EditValue(null, provider, value);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00029465 File Offset: 0x00028465
		public virtual object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			return value;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00029468 File Offset: 0x00028468
		public UITypeEditorEditStyle GetEditStyle()
		{
			return this.GetEditStyle(null);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00029471 File Offset: 0x00028471
		public bool GetPaintValueSupported()
		{
			return this.GetPaintValueSupported(null);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0002947A File Offset: 0x0002847A
		public virtual bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0002947D File Offset: 0x0002847D
		public virtual UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.None;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00029480 File Offset: 0x00028480
		public void PaintValue(object value, Graphics canvas, Rectangle rectangle)
		{
			this.PaintValue(new PaintValueEventArgs(null, value, canvas, rectangle));
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00029491 File Offset: 0x00028491
		public virtual void PaintValue(PaintValueEventArgs e)
		{
		}
	}
}
