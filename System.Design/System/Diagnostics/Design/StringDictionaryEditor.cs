using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Design;

namespace System.Diagnostics.Design
{
	internal class StringDictionaryEditor : CollectionEditor
	{
		public StringDictionaryEditor(Type type)
			: base(type)
		{
		}

		protected override Type CreateCollectionItemType()
		{
			return typeof(EditableDictionaryEntry);
		}

		protected override object CreateInstance(Type itemType)
		{
			return new EditableDictionaryEntry("name", "value");
		}

		protected override object SetItems(object editValue, object[] value)
		{
			StringDictionary stringDictionary = editValue as StringDictionary;
			if (stringDictionary == null)
			{
				throw new ArgumentNullException("editValue");
			}
			stringDictionary.Clear();
			foreach (EditableDictionaryEntry editableDictionaryEntry in value)
			{
				stringDictionary[editableDictionaryEntry.Name] = editableDictionaryEntry.Value;
			}
			return stringDictionary;
		}

		protected override object[] GetItems(object editValue)
		{
			if (editValue == null)
			{
				return new object[0];
			}
			StringDictionary stringDictionary = editValue as StringDictionary;
			if (stringDictionary == null)
			{
				throw new ArgumentNullException("editValue");
			}
			object[] array = new object[stringDictionary.Count];
			int num = 0;
			foreach (object obj in stringDictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				EditableDictionaryEntry editableDictionaryEntry = new EditableDictionaryEntry((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				array[num++] = editableDictionaryEntry;
			}
			return array;
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("StringDictionaryEditorTitle");
			collectionForm.CollectionEditable = true;
			return collectionForm;
		}
	}
}
