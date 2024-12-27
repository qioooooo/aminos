using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.JScript
{
	// Token: 0x02000015 RID: 21
	public class ArrayWrapper : ArrayObject
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00006928 File Offset: 0x00005928
		internal ArrayWrapper(ScriptObject prototype, Array value, bool implicitWrapper)
			: base(prototype, typeof(ArrayWrapper))
		{
			this.value = value;
			this.implicitWrapper = implicitWrapper;
			if (value == null)
			{
				this.len = 0U;
				return;
			}
			if (value.Rank != 1)
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			this.len = (uint)value.Length;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000697C File Offset: 0x0000597C
		internal override void Concat(ArrayObject source)
		{
			throw new JScriptException(JSError.ActionNotSupported);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006988 File Offset: 0x00005988
		internal override void Concat(object value)
		{
			throw new JScriptException(JSError.ActionNotSupported);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006994 File Offset: 0x00005994
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			enums.Add(new ArrayEnumerator(this, new RangeEnumerator(0, (int)(this.len - 1U))));
			objects.Add(this);
			if (this.parent != null)
			{
				this.parent.GetPropertyEnumerator(enums, objects);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000069CE File Offset: 0x000059CE
		public new Type GetType()
		{
			if (!this.implicitWrapper)
			{
				return typeof(ArrayObject);
			}
			return this.value.GetType();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000069EE File Offset: 0x000059EE
		internal override object GetValueAtIndex(uint index)
		{
			return this.value.GetValue(checked((int)index));
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000069FD File Offset: 0x000059FD
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00006A0A File Offset: 0x00005A0A
		public override object length
		{
			get
			{
				return this.len;
			}
			set
			{
				throw new JScriptException(JSError.AssignmentToReadOnly);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006A18 File Offset: 0x00005A18
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override void SetMemberValue(string name, object val)
		{
			if (name.Equals("length"))
			{
				throw new JScriptException(JSError.AssignmentToReadOnly);
			}
			long num = ArrayObject.Array_index_for(name);
			if (num < 0L)
			{
				base.SetMemberValue(name, val);
				return;
			}
			this.value.SetValue(val, (int)num);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006A60 File Offset: 0x00005A60
		internal override void SetValueAtIndex(uint index, object val)
		{
			Type type = this.value.GetType();
			this.value.SetValue(Convert.CoerceT(val, type.GetElementType()), checked((int)index));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006A92 File Offset: 0x00005A92
		internal override object Shift()
		{
			throw new JScriptException(JSError.ActionNotSupported);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006A9E File Offset: 0x00005A9E
		internal override void Splice(uint start, uint deleteCount, object[] args, ArrayObject outArray, uint oldLength, uint newLength)
		{
			if (oldLength != newLength)
			{
				throw new JScriptException(JSError.ActionNotSupported);
			}
			base.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006AC0 File Offset: 0x00005AC0
		internal override void Sort(ScriptFunction compareFn)
		{
			ArrayWrapper.SortComparer sortComparer = new ArrayWrapper.SortComparer(compareFn);
			Array.Sort(this.value, sortComparer);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006AE0 File Offset: 0x00005AE0
		internal override void SwapValues(uint pi, uint qi)
		{
			object valueAtIndex = this.GetValueAtIndex(pi);
			object valueAtIndex2 = this.GetValueAtIndex(qi);
			this.SetValueAtIndex(pi, valueAtIndex2);
			this.SetValueAtIndex(qi, valueAtIndex);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006B0D File Offset: 0x00005B0D
		internal override Array ToNativeArray(Type elementType)
		{
			return this.value;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006B18 File Offset: 0x00005B18
		internal override object[] ToArray()
		{
			object[] array = new object[checked((int)this.len)];
			for (uint num = 0U; num < this.len; num += 1U)
			{
				array[(int)((UIntPtr)num)] = this.GetValueAtIndex(num);
			}
			return array;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006B4F File Offset: 0x00005B4F
		internal override ArrayObject Unshift(object[] args)
		{
			throw new JScriptException(JSError.ActionNotSupported);
		}

		// Token: 0x0400003B RID: 59
		internal Array value;

		// Token: 0x0400003C RID: 60
		private bool implicitWrapper;

		// Token: 0x02000016 RID: 22
		internal sealed class SortComparer : IComparer
		{
			// Token: 0x06000110 RID: 272 RVA: 0x00006B5B File Offset: 0x00005B5B
			internal SortComparer(ScriptFunction compareFn)
			{
				this.compareFn = compareFn;
			}

			// Token: 0x06000111 RID: 273 RVA: 0x00006B6C File Offset: 0x00005B6C
			public int Compare(object x, object y)
			{
				if (x == null || x is Missing)
				{
					if (y == null || y is Missing)
					{
						return 0;
					}
					return 1;
				}
				else
				{
					if (y == null || y is Missing)
					{
						return -1;
					}
					if (this.compareFn == null)
					{
						return string.CompareOrdinal(Convert.ToString(x), Convert.ToString(y));
					}
					double num = Convert.ToNumber(this.compareFn.Call(new object[] { x, y }, null));
					if (num != num)
					{
						throw new JScriptException(JSError.NumberExpected);
					}
					return (int)Runtime.DoubleToInt64(num);
				}
			}

			// Token: 0x0400003D RID: 61
			internal ScriptFunction compareFn;
		}
	}
}
