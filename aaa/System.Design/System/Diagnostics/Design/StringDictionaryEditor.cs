using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Design;

namespace System.Diagnostics.Design
{
	// Token: 0x020000EF RID: 239
	internal class StringDictionaryEditor : CollectionEditor
	{
		// Token: 0x060009EF RID: 2543 RVA: 0x00025E7C File Offset: 0x00024E7C
		public StringDictionaryEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00025E85 File Offset: 0x00024E85
		protected override Type CreateCollectionItemType()
		{
			return typeof(EditableDictionaryEntry);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00025E91 File Offset: 0x00024E91
		protected override object CreateInstance(Type itemType)
		{
			return new EditableDictionaryEntry("name", "value");
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00025EA4 File Offset: 0x00024EA4
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

		// Token: 0x060009F3 RID: 2547 RVA: 0x00025EF8 File Offset: 0x00024EF8
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

		// Token: 0x060009F4 RID: 2548 RVA: 0x00025FA8 File Offset: 0x00024FA8
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
			collectionForm.Text = SR.GetString("StringDictionaryEditorTitle");
			collectionForm.CollectionEditable = true;
			return collectionForm;
		}
	}
}
