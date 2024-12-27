using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000297 RID: 663
	internal class StringArrayEditor : StringCollectionEditor
	{
		// Token: 0x0600188B RID: 6283 RVA: 0x00081AD2 File Offset: 0x00080AD2
		public StringArrayEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00081ADB File Offset: 0x00080ADB
		protected override Type CreateCollectionItemType()
		{
			return base.CollectionType.GetElementType();
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00081AE8 File Offset: 0x00080AE8
		protected override object[] GetItems(object editValue)
		{
			Array array = editValue as Array;
			if (array == null)
			{
				return new object[0];
			}
			object[] array2 = new object[array.GetLength(0)];
			Array.Copy(array, array2, array2.Length);
			return array2;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00081B20 File Offset: 0x00080B20
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
