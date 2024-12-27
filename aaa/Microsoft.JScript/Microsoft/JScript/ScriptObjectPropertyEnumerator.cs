using System;
using System.Collections;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000112 RID: 274
	internal class ScriptObjectPropertyEnumerator : IEnumerator
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00057308 File Offset: 0x00056308
		internal ScriptObjectPropertyEnumerator(ScriptObject obj)
		{
			obj.GetPropertyEnumerator(this.enumerators = new ArrayList(), this.objects = new ArrayList());
			this.index = 0;
			this.visited_names = new SimpleHashtable(16U);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00057354 File Offset: 0x00056354
		public virtual bool MoveNext()
		{
			if (this.index >= this.enumerators.Count)
			{
				return false;
			}
			IEnumerator enumerator = (IEnumerator)this.enumerators[this.index];
			if (!enumerator.MoveNext())
			{
				this.index++;
				return this.MoveNext();
			}
			object obj = enumerator.Current;
			FieldInfo fieldInfo = obj as FieldInfo;
			string text;
			if (fieldInfo != null)
			{
				JSPrototypeField jsprototypeField = obj as JSPrototypeField;
				if (jsprototypeField != null && jsprototypeField.value is Missing)
				{
					return this.MoveNext();
				}
				text = fieldInfo.Name;
				object value = fieldInfo.GetValue(this.objects[this.index]);
				if (value is Missing)
				{
					return this.MoveNext();
				}
			}
			else if (obj is string)
			{
				text = (string)obj;
			}
			else if (obj is MemberInfo)
			{
				text = ((MemberInfo)obj).Name;
			}
			else
			{
				text = obj.ToString();
			}
			if (this.visited_names[text] != null)
			{
				return this.MoveNext();
			}
			this.visited_names[text] = text;
			return true;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00057464 File Offset: 0x00056464
		public virtual object Current
		{
			get
			{
				object obj = ((IEnumerator)this.enumerators[this.index]).Current;
				if (obj is MemberInfo)
				{
					return ((MemberInfo)obj).Name;
				}
				return obj.ToString();
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x000574A8 File Offset: 0x000564A8
		public virtual void Reset()
		{
			this.index = 0;
			foreach (object obj in this.enumerators)
			{
				IEnumerator enumerator2 = (IEnumerator)obj;
				enumerator2.Reset();
			}
			this.visited_names = new SimpleHashtable(16U);
		}

		// Token: 0x040006DF RID: 1759
		private ArrayList enumerators;

		// Token: 0x040006E0 RID: 1760
		private ArrayList objects;

		// Token: 0x040006E1 RID: 1761
		private int index;

		// Token: 0x040006E2 RID: 1762
		private SimpleHashtable visited_names;
	}
}
