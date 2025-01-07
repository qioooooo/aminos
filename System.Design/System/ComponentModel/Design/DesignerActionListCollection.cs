﻿using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerActionListCollection : CollectionBase
	{
		public DesignerActionListCollection()
		{
		}

		internal DesignerActionListCollection(DesignerActionList actionList)
		{
			this.Add(actionList);
		}

		public DesignerActionListCollection(DesignerActionList[] value)
		{
			this.AddRange(value);
		}

		public DesignerActionList this[int index]
		{
			get
			{
				return (DesignerActionList)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		public int Add(DesignerActionList value)
		{
			return base.List.Add(value);
		}

		public void AddRange(DesignerActionList[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		public void AddRange(DesignerActionListCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		public void Insert(int index, DesignerActionList value)
		{
			base.List.Insert(index, value);
		}

		public int IndexOf(DesignerActionList value)
		{
			return base.List.IndexOf(value);
		}

		public bool Contains(DesignerActionList value)
		{
			return base.List.Contains(value);
		}

		public void Remove(DesignerActionList value)
		{
			base.List.Remove(value);
		}

		public void CopyTo(DesignerActionList[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		protected override void OnInsert(int index, object value)
		{
		}

		protected override void OnClear()
		{
		}

		protected override void OnRemove(int index, object value)
		{
		}

		protected override void OnValidate(object value)
		{
		}
	}
}
