using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.JScript
{
	// Token: 0x02000012 RID: 18
	public class ArrayObject : JSObject
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000053F7 File Offset: 0x000043F7
		internal ArrayObject(ScriptObject prototype)
			: base(prototype)
		{
			this.len = 0U;
			this.denseArray = null;
			this.denseArrayLength = 0U;
			this.noExpando = false;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000541C File Offset: 0x0000441C
		internal ArrayObject(ScriptObject prototype, Type subType)
			: base(prototype, subType)
		{
			this.len = 0U;
			this.denseArray = null;
			this.denseArrayLength = 0U;
			this.noExpando = false;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005444 File Offset: 0x00004444
		internal static long Array_index_for(object index)
		{
			if (index is int)
			{
				return (long)((int)index);
			}
			IConvertible iconvertible = Convert.GetIConvertible(index);
			switch (Convert.GetTypeCode(index, iconvertible))
			{
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			{
				double num = iconvertible.ToDouble(null);
				long num2 = (long)num;
				if (num2 >= 0L && (double)num2 == num)
				{
					return num2;
				}
				break;
			}
			}
			return -1L;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000054C4 File Offset: 0x000044C4
		internal static long Array_index_for(string name)
		{
			int length = name.Length;
			if (length <= 0)
			{
				return -1L;
			}
			char c = name[0];
			if (c >= '1' && c <= '9')
			{
				long num = (long)(c - '0');
				for (int i = 1; i < length; i++)
				{
					c = name[i];
					if (c < '0' || c > '9')
					{
						return -1L;
					}
					num = num * 10L + (long)(c - '0');
					if (num > (long)((ulong)(-1)))
					{
						return -1L;
					}
				}
				return num;
			}
			if (c == '0' && length == 1)
			{
				return 0L;
			}
			return -1L;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000553C File Offset: 0x0000453C
		internal virtual void Concat(ArrayObject source)
		{
			uint num = source.len;
			if (num == 0U)
			{
				return;
			}
			uint num2 = this.len;
			this.SetLength((ulong)num2 + (ulong)num);
			uint num3 = num;
			if (!(source is ArrayWrapper) && num > source.denseArrayLength)
			{
				num3 = source.denseArrayLength;
			}
			uint num4 = num2;
			for (uint num5 = 0U; num5 < num3; num5 += 1U)
			{
				this.SetValueAtIndex(num4++, source.GetValueAtIndex(num5));
			}
			if (num3 == num)
			{
				return;
			}
			IDictionaryEnumerator enumerator = source.NameTable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				long num6 = ArrayObject.Array_index_for(enumerator.Key.ToString());
				if (num6 >= 0L)
				{
					this.SetValueAtIndex(num2 + (uint)num6, ((JSField)enumerator.Value).GetValue(null));
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000055F8 File Offset: 0x000045F8
		internal virtual void Concat(object value)
		{
			Array array = value as Array;
			if (array != null && array.Rank == 1)
			{
				this.Concat(new ArrayWrapper(ArrayPrototype.ob, array, true));
				return;
			}
			uint num = this.len;
			this.SetLength(1UL + (ulong)num);
			this.SetValueAtIndex(num, value);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005648 File Offset: 0x00004648
		internal override bool DeleteMember(string name)
		{
			long num = ArrayObject.Array_index_for(name);
			if (num >= 0L)
			{
				return this.DeleteValueAtIndex((uint)num);
			}
			return base.DeleteMember(name);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005671 File Offset: 0x00004671
		internal virtual bool DeleteValueAtIndex(uint index)
		{
			if (index >= this.denseArrayLength)
			{
				return base.DeleteMember(index.ToString(CultureInfo.InvariantCulture));
			}
			if (this.denseArray[(int)index] is Missing)
			{
				return false;
			}
			this.denseArray[(int)index] = Missing.Value;
			return true;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000056B0 File Offset: 0x000046B0
		private void DeleteRange(uint start, uint end)
		{
			uint num = this.denseArrayLength;
			if (num > end)
			{
				num = end;
			}
			while (start < num)
			{
				this.denseArray[(int)start] = Missing.Value;
				start += 1U;
			}
			if (num == end)
			{
				return;
			}
			IDictionaryEnumerator enumerator = base.NameTable.GetEnumerator();
			ArrayList arrayList = new ArrayList(this.name_table.count);
			while (enumerator.MoveNext())
			{
				long num2 = ArrayObject.Array_index_for(enumerator.Key.ToString());
				if (num2 >= (long)((ulong)start) && num2 <= (long)((ulong)end))
				{
					arrayList.Add(enumerator.Key);
				}
			}
			foreach (object obj in arrayList)
			{
				this.DeleteMember((string)obj);
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000575C File Offset: 0x0000475C
		internal override string GetClassName()
		{
			return "Array";
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005764 File Offset: 0x00004764
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (base.GetParent() is LenientArrayPrototype)
			{
				return base.GetDefaultValue(preferred_type);
			}
			if (preferred_type == PreferredType.String)
			{
				if (!this.noExpando)
				{
					object obj = base.NameTable["toString"];
					if (obj != null)
					{
						return base.GetDefaultValue(preferred_type);
					}
				}
				return ArrayPrototype.toString(this);
			}
			if (preferred_type == PreferredType.LocaleString)
			{
				if (!this.noExpando)
				{
					object obj2 = base.NameTable["toLocaleString"];
					if (obj2 != null)
					{
						return base.GetDefaultValue(preferred_type);
					}
				}
				return ArrayPrototype.toLocaleString(this);
			}
			if (!this.noExpando)
			{
				object obj3 = base.NameTable["valueOf"];
				if (obj3 == null && preferred_type == PreferredType.Either)
				{
					obj3 = base.NameTable["toString"];
				}
				if (obj3 != null)
				{
					return base.GetDefaultValue(preferred_type);
				}
			}
			return ArrayPrototype.toString(this);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005828 File Offset: 0x00004828
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			if (this.field_table == null)
			{
				this.field_table = new ArrayList();
			}
			enums.Add(new ArrayEnumerator(this, new ListEnumerator(this.field_table)));
			objects.Add(this);
			if (this.parent != null)
			{
				this.parent.GetPropertyEnumerator(enums, objects);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005880 File Offset: 0x00004880
		internal override object GetValueAtIndex(uint index)
		{
			if (index < this.denseArrayLength)
			{
				object obj = this.denseArray[(int)index];
				if (obj != Missing.Value)
				{
					return obj;
				}
			}
			return base.GetValueAtIndex(index);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000058B0 File Offset: 0x000048B0
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object GetMemberValue(string name)
		{
			long num = ArrayObject.Array_index_for(name);
			if (num < 0L)
			{
				return base.GetMemberValue(name);
			}
			return this.GetValueAtIndex((uint)num);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000058D9 File Offset: 0x000048D9
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005904 File Offset: 0x00004904
		public virtual object length
		{
			get
			{
				if (this.len < 2147483647U)
				{
					return (int)this.len;
				}
				return this.len;
			}
			set
			{
				IConvertible iconvertible = Convert.GetIConvertible(value);
				uint num = Convert.ToUint32(value, iconvertible);
				if (num != Convert.ToNumber(value, iconvertible))
				{
					throw new JScriptException(JSError.ArrayLengthAssignIncorrect);
				}
				this.SetLength((ulong)num);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005940 File Offset: 0x00004940
		private void Realloc(uint newLength)
		{
			uint num = this.denseArrayLength;
			uint num2 = num * 2U;
			if (num2 < newLength)
			{
				num2 = newLength;
			}
			object[] array = new object[num2];
			if (num > 0U)
			{
				ArrayObject.Copy(this.denseArray, array, (int)num);
			}
			int num3 = (int)num;
			while ((long)num3 < (long)((ulong)num2))
			{
				array[num3] = Missing.Value;
				num3++;
			}
			this.denseArray = array;
			this.denseArrayLength = num2;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000599C File Offset: 0x0000499C
		private void SetLength(ulong newLength)
		{
			uint num = this.len;
			if (newLength < (ulong)num)
			{
				this.DeleteRange((uint)newLength, num);
			}
			else
			{
				if (newLength > (ulong)(-1))
				{
					throw new JScriptException(JSError.ArrayLengthAssignIncorrect);
				}
				if (newLength > (ulong)this.denseArrayLength && num <= this.denseArrayLength && newLength <= 100000UL && (newLength <= 128UL || newLength <= (ulong)(num * 2U)))
				{
					this.Realloc((uint)newLength);
				}
			}
			this.len = (uint)newLength;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005A0C File Offset: 0x00004A0C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override void SetMemberValue(string name, object value)
		{
			if (name.Equals("length"))
			{
				this.length = value;
				return;
			}
			long num = ArrayObject.Array_index_for(name);
			if (num < 0L)
			{
				base.SetMemberValue(name, value);
				return;
			}
			this.SetValueAtIndex((uint)num, value);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005A4C File Offset: 0x00004A4C
		internal override void SetValueAtIndex(uint index, object value)
		{
			if (index >= this.len && index < 4294967295U)
			{
				this.SetLength((ulong)(index + 1U));
			}
			if (index < this.denseArrayLength)
			{
				this.denseArray[(int)index] = value;
				return;
			}
			base.SetMemberValue(index.ToString(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005A8C File Offset: 0x00004A8C
		internal virtual object Shift()
		{
			object obj = null;
			uint num = this.len;
			if (num == 0U)
			{
				return obj;
			}
			uint num2 = ((this.denseArrayLength >= num) ? num : this.denseArrayLength);
			if (num2 > 0U)
			{
				obj = this.denseArray[0];
				ArrayObject.Copy(this.denseArray, 1, this.denseArray, 0, (int)(num2 - 1U));
			}
			else
			{
				obj = base.GetValueAtIndex(0U);
			}
			for (uint num3 = num2; num3 < num; num3 += 1U)
			{
				this.SetValueAtIndex(num3 - 1U, this.GetValueAtIndex(num3));
			}
			this.SetValueAtIndex(num - 1U, Missing.Value);
			this.SetLength((ulong)(num - 1U));
			if (obj is Missing)
			{
				return null;
			}
			return obj;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005B28 File Offset: 0x00004B28
		internal virtual void Sort(ScriptFunction compareFn)
		{
			QuickSort quickSort = new QuickSort(this, compareFn);
			uint num = this.len;
			if (num <= this.denseArrayLength)
			{
				quickSort.SortArray(0, (int)(num - 1U));
				return;
			}
			quickSort.SortObject(0L, (long)((ulong)(num - 1U)));
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005B64 File Offset: 0x00004B64
		internal virtual void Splice(uint start, uint deleteCount, object[] args, ArrayObject outArray, uint oldLength, uint newLength)
		{
			if (oldLength > this.denseArrayLength)
			{
				this.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
				return;
			}
			if (newLength > oldLength)
			{
				this.SetLength((ulong)newLength);
				if (newLength > this.denseArrayLength)
				{
					this.SpliceSlowly(start, deleteCount, args, outArray, oldLength, newLength);
					return;
				}
			}
			if (deleteCount > oldLength)
			{
				deleteCount = oldLength;
			}
			if (deleteCount > 0U)
			{
				ArrayObject.Copy(this.denseArray, (int)start, outArray.denseArray, 0, (int)deleteCount);
			}
			if (oldLength > 0U)
			{
				ArrayObject.Copy(this.denseArray, (int)(start + deleteCount), this.denseArray, (int)(start + (uint)args.Length), (int)(oldLength - start - deleteCount));
			}
			if (args != null)
			{
				int num = args.Length;
				if (num > 0)
				{
					ArrayObject.Copy(args, 0, this.denseArray, (int)start, num);
				}
				if ((long)num < (long)((ulong)deleteCount))
				{
					this.SetLength((ulong)newLength);
					return;
				}
			}
			else if (deleteCount > 0U)
			{
				this.SetLength((ulong)newLength);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005C34 File Offset: 0x00004C34
		protected void SpliceSlowly(uint start, uint deleteCount, object[] args, ArrayObject outArray, uint oldLength, uint newLength)
		{
			for (uint num = 0U; num < deleteCount; num += 1U)
			{
				outArray.SetValueAtIndex(num, this.GetValueAtIndex(num + start));
			}
			uint num2 = oldLength - start - deleteCount;
			if (newLength < oldLength)
			{
				for (uint num3 = 0U; num3 < num2; num3 += 1U)
				{
					this.SetValueAtIndex(num3 + start + (uint)args.Length, this.GetValueAtIndex(num3 + start + deleteCount));
				}
				this.SetLength((ulong)newLength);
			}
			else
			{
				if (newLength > oldLength)
				{
					this.SetLength((ulong)newLength);
				}
				for (uint num4 = num2; num4 > 0U; num4 -= 1U)
				{
					this.SetValueAtIndex(num4 + start + (uint)args.Length - 1U, this.GetValueAtIndex(num4 + start + deleteCount - 1U));
				}
			}
			int num5 = ((args == null) ? 0 : args.Length);
			uint num6 = 0U;
			while ((ulong)num6 < (ulong)((long)num5))
			{
				this.SetValueAtIndex(num6 + start, args[(int)((UIntPtr)num6)]);
				num6 += 1U;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005D00 File Offset: 0x00004D00
		internal override void SwapValues(uint pi, uint qi)
		{
			if (pi > qi)
			{
				this.SwapValues(qi, pi);
				return;
			}
			if (pi >= this.denseArrayLength)
			{
				base.SwapValues(pi, qi);
				return;
			}
			object obj = this.denseArray[(int)pi];
			this.denseArray[(int)pi] = this.GetValueAtIndex(qi);
			if (obj == Missing.Value)
			{
				this.DeleteValueAtIndex(qi);
				return;
			}
			this.SetValueAtIndex(qi, obj);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005D60 File Offset: 0x00004D60
		internal virtual object[] ToArray()
		{
			int num = (int)this.len;
			if (num == 0)
			{
				return new object[0];
			}
			if ((long)num == (long)((ulong)this.denseArrayLength))
			{
				return this.denseArray;
			}
			if ((long)num < (long)((ulong)this.denseArrayLength))
			{
				object[] array = new object[num];
				ArrayObject.Copy(this.denseArray, 0, array, 0, num);
				return array;
			}
			object[] array2 = new object[num];
			ArrayObject.Copy(this.denseArray, 0, array2, 0, (int)this.denseArrayLength);
			uint num2 = this.denseArrayLength;
			while ((ulong)num2 < (ulong)((long)num))
			{
				array2[(int)((UIntPtr)num2)] = this.GetValueAtIndex(num2);
				num2 += 1U;
			}
			return array2;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005DF0 File Offset: 0x00004DF0
		internal virtual Array ToNativeArray(Type elementType)
		{
			uint num = this.len;
			if (num > 2147483647U)
			{
				throw new JScriptException(JSError.OutOfMemory);
			}
			if (elementType == null)
			{
				elementType = typeof(object);
			}
			uint num2 = this.denseArrayLength;
			if (num2 > num)
			{
				num2 = num;
			}
			Array array = Array.CreateInstance(elementType, (int)num);
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num2))
			{
				array.SetValue(Convert.CoerceT(this.denseArray[num3], elementType), num3);
				num3++;
			}
			int num4 = (int)num2;
			while ((long)num4 < (long)((ulong)num))
			{
				array.SetValue(Convert.CoerceT(this.GetValueAtIndex((uint)num4), elementType), num4);
				num4++;
			}
			return array;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005E83 File Offset: 0x00004E83
		internal static void Copy(object[] source, object[] target, int n)
		{
			ArrayObject.Copy(source, 0, target, 0, n);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005E90 File Offset: 0x00004E90
		internal static void Copy(object[] source, int i, object[] target, int j, int n)
		{
			if (i < j)
			{
				for (int k = n - 1; k >= 0; k--)
				{
					target[j + k] = source[i + k];
				}
				return;
			}
			for (int l = 0; l < n; l++)
			{
				target[j + l] = source[i + l];
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005ED4 File Offset: 0x00004ED4
		internal virtual ArrayObject Unshift(object[] args)
		{
			uint num = this.len;
			int num2 = args.Length;
			ulong num3 = (ulong)num + (ulong)((long)num2);
			this.SetLength(num3);
			if (num3 <= (ulong)this.denseArrayLength)
			{
				for (int i = (int)(num - 1U); i >= 0; i--)
				{
					this.denseArray[i + num2] = this.denseArray[i];
				}
				ArrayObject.Copy(args, 0, this.denseArray, 0, args.Length);
			}
			else
			{
				for (long num4 = (long)((ulong)(num - 1U)); num4 >= 0L; num4 -= 1L)
				{
					this.SetValueAtIndex((uint)(num4 + (long)num2), this.GetValueAtIndex((uint)num4));
				}
				uint num5 = 0U;
				while ((ulong)num5 < (ulong)((long)num2))
				{
					this.SetValueAtIndex(num5, args[(int)((UIntPtr)num5)]);
					num5 += 1U;
				}
			}
			return this;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005F80 File Offset: 0x00004F80
		internal DebugArrayFieldEnumerator DebugGetEnumerator()
		{
			return new DebugArrayFieldEnumerator(new ScriptObjectPropertyEnumerator(this), this);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005F8E File Offset: 0x00004F8E
		internal object DebugGetValueAtIndex(int index)
		{
			return this.GetValueAtIndex((uint)index);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005F97 File Offset: 0x00004F97
		internal void DebugSetValueAtIndex(int index, object value)
		{
			this.SetValueAtIndex((uint)index, value);
		}

		// Token: 0x04000032 RID: 50
		internal const int MaxIndex = 100000;

		// Token: 0x04000033 RID: 51
		internal const int MinDenseSize = 128;

		// Token: 0x04000034 RID: 52
		internal uint len;

		// Token: 0x04000035 RID: 53
		internal object[] denseArray;

		// Token: 0x04000036 RID: 54
		internal uint denseArrayLength;
	}
}
