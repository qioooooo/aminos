using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200024B RID: 587
	internal class ImageCollectionEditor : CollectionEditor
	{
		// Token: 0x06001655 RID: 5717 RVA: 0x000748B8 File Offset: 0x000738B8
		public ImageCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000748C4 File Offset: 0x000738C4
		protected override string GetDisplayText(object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(value)["Name"];
			string text;
			if (propertyDescriptor != null)
			{
				text = (string)propertyDescriptor.GetValue(value);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			if (value is ImageListImage)
			{
				value = ((ImageListImage)value).Image;
			}
			text = TypeDescriptor.GetConverter(value).ConvertToString(value);
			if (text == null || text.Length == 0)
			{
				text = value.GetType().Name;
			}
			return text;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00074944 File Offset: 0x00073944
		protected override object CreateInstance(Type type)
		{
			UITypeEditor uitypeEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(ImageListImage), typeof(UITypeEditor));
			return uitypeEditor.EditValue(base.Context, null);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00074980 File Offset: 0x00073980
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("ImageCollectionEditorFormText");
			return collectionForm;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x000749A8 File Offset: 0x000739A8
		protected override IList GetObjectsFromInstance(object instance)
		{
			ArrayList arrayList = instance as ArrayList;
			if (arrayList != null)
			{
				return arrayList;
			}
			return null;
		}
	}
}
