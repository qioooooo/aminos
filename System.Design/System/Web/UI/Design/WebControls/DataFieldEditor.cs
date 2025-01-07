using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DataFieldEditor : DataFieldCollectionEditor
	{
		public DataFieldEditor(Type type)
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
