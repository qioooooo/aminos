using System;

namespace System.ComponentModel.Design
{
	public class DesignerActionListsChangedEventArgs : EventArgs
	{
		public DesignerActionListsChangedEventArgs(object relatedObject, DesignerActionListsChangedType changeType, DesignerActionListCollection actionLists)
		{
			this.relatedObject = relatedObject;
			this.changeType = changeType;
			this.actionLists = actionLists;
		}

		public DesignerActionListsChangedType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		public object RelatedObject
		{
			get
			{
				return this.relatedObject;
			}
		}

		public DesignerActionListCollection ActionLists
		{
			get
			{
				return this.actionLists;
			}
		}

		private object relatedObject;

		private DesignerActionListCollection actionLists;

		private DesignerActionListsChangedType changeType;
	}
}
