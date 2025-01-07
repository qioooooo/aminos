using System;
using System.Collections;

namespace System.Data.Design
{
	internal interface IDataSourceCommandTarget
	{
		bool CanAddChildOfType(Type childType);

		void AddChild(object child, bool fixName);

		bool CanInsertChildOfType(Type childType, object refChild);

		void InsertChild(object child, object refChild);

		bool CanRemoveChildren(ICollection children);

		void RemoveChildren(ICollection children);

		int IndexOf(object child);

		object GetObject(int index, bool getSiblingIfOutOfRange);
	}
}
