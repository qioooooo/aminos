using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class ImageCollectionEditor : CollectionEditor
	{
		public ImageCollectionEditor(Type type)
			: base(type)
		{
		}

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

		protected override object CreateInstance(Type type)
		{
			UITypeEditor uitypeEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(ImageListImage), typeof(UITypeEditor));
			return uitypeEditor.EditValue(base.Context, null);
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("ImageCollectionEditorFormText");
			return collectionForm;
		}

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
