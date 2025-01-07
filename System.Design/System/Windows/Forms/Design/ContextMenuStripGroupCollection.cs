using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	internal class ContextMenuStripGroupCollection : DictionaryBase
	{
		public ContextMenuStripGroup this[string key]
		{
			get
			{
				if (!base.InnerHashtable.ContainsKey(key))
				{
					base.InnerHashtable[key] = new ContextMenuStripGroup(key);
				}
				return base.InnerHashtable[key] as ContextMenuStripGroup;
			}
		}

		public bool ContainsKey(string key)
		{
			return base.InnerHashtable.ContainsKey(key);
		}

		protected override void OnInsert(object key, object value)
		{
			if (!(value is ContextMenuStripGroup))
			{
				throw new NotSupportedException();
			}
			base.OnInsert(key, value);
		}

		protected override void OnSet(object key, object oldValue, object newValue)
		{
			if (!(newValue is ContextMenuStripGroup))
			{
				throw new NotSupportedException();
			}
			base.OnSet(key, oldValue, newValue);
		}
	}
}
