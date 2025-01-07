using System;

namespace System.ComponentModel.Design
{
	public class ArrayEditor : CollectionEditor
	{
		public ArrayEditor(Type type)
			: base(type)
		{
		}

		protected override Type CreateCollectionItemType()
		{
			return base.CollectionType.GetElementType();
		}

		protected override object[] GetItems(object editValue)
		{
			if (editValue is Array)
			{
				Array array = (Array)editValue;
				object[] array2 = new object[array.GetLength(0)];
				Array.Copy(array, array2, array2.Length);
				return array2;
			}
			return new object[0];
		}

		protected override object SetItems(object editValue, object[] value)
		{
			if (editValue is Array || editValue == null)
			{
				Array array = Array.CreateInstance(base.CollectionItemType, value.Length);
				Array.Copy(value, array, value.Length);
				return array;
			}
			return editValue;
		}
	}
}
