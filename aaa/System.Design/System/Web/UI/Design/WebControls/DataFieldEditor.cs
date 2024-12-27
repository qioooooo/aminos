using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200043C RID: 1084
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DataFieldEditor : DataFieldCollectionEditor
	{
		// Token: 0x0600273F RID: 10047 RVA: 0x000D63CD File Offset: 0x000D53CD
		public DataFieldEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000D63D6 File Offset: 0x000D53D6
		protected override Type CreateCollectionItemType()
		{
			return base.CollectionType.GetElementType();
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000D63E4 File Offset: 0x000D53E4
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

		// Token: 0x06002742 RID: 10050 RVA: 0x000D6420 File Offset: 0x000D5420
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
