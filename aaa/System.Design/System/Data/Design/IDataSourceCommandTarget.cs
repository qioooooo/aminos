using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000095 RID: 149
	internal interface IDataSourceCommandTarget
	{
		// Token: 0x0600063C RID: 1596
		bool CanAddChildOfType(Type childType);

		// Token: 0x0600063D RID: 1597
		void AddChild(object child, bool fixName);

		// Token: 0x0600063E RID: 1598
		bool CanInsertChildOfType(Type childType, object refChild);

		// Token: 0x0600063F RID: 1599
		void InsertChild(object child, object refChild);

		// Token: 0x06000640 RID: 1600
		bool CanRemoveChildren(ICollection children);

		// Token: 0x06000641 RID: 1601
		void RemoveChildren(ICollection children);

		// Token: 0x06000642 RID: 1602
		int IndexOf(object child);

		// Token: 0x06000643 RID: 1603
		object GetObject(int index, bool getSiblingIfOutOfRange);
	}
}
