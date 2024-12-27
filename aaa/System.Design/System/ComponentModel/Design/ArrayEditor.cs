using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F3 RID: 243
	public class ArrayEditor : CollectionEditor
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x00026141 File Offset: 0x00025141
		public ArrayEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002614A File Offset: 0x0002514A
		protected override Type CreateCollectionItemType()
		{
			return base.CollectionType.GetElementType();
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00026158 File Offset: 0x00025158
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

		// Token: 0x06000A05 RID: 2565 RVA: 0x00026194 File Offset: 0x00025194
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
