using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Text
{
	// Token: 0x0200002E RID: 46
	[ComVisible(true)]
	[Serializable]
	public sealed class StringBuilder : ISerializable
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00009786 File Offset: 0x00008786
		public StringBuilder()
			: this(16)
		{
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009790 File Offset: 0x00008790
		public StringBuilder(int capacity)
			: this(string.Empty, capacity)
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000979E File Offset: 0x0000879E
		public StringBuilder(string value)
			: this(value, 16)
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000097A9 File Offset: 0x000087A9
		public StringBuilder(string value, int capacity)
			: this(value, 0, (value != null) ? value.Length : 0, capacity)
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000097C0 File Offset: 0x000087C0
		public StringBuilder(string value, int startIndex, int length, int capacity)
		{
			this.m_currentThread = Thread.InternalGetCurrentThread();
			base..ctor();
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_MustBePositive"), new object[] { "capacity" }));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum"), new object[] { "length" }));
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (value == null)
			{
				value = string.Empty;
			}
			if (startIndex > value.Length - length)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_IndexLength"));
			}
			this.m_MaxCapacity = int.MaxValue;
			if (capacity == 0)
			{
				capacity = 16;
			}
			while (capacity < length)
			{
				capacity *= 2;
				if (capacity < 0)
				{
					capacity = length;
					break;
				}
			}
			this.m_StringValue = string.GetStringForStringBuilder(value, startIndex, length, capacity);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000098C4 File Offset: 0x000088C4
		public StringBuilder(int capacity, int maxCapacity)
		{
			this.m_currentThread = Thread.InternalGetCurrentThread();
			base..ctor();
			if (capacity > maxCapacity)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_Capacity"));
			}
			if (maxCapacity < 1)
			{
				throw new ArgumentOutOfRangeException("maxCapacity", Environment.GetResourceString("ArgumentOutOfRange_SmallMaxCapacity"));
			}
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_MustBePositive"), new object[] { "capacity" }));
			}
			if (capacity == 0)
			{
				capacity = Math.Min(16, maxCapacity);
			}
			this.m_StringValue = string.GetStringForStringBuilder(string.Empty, capacity);
			this.m_MaxCapacity = maxCapacity;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00009970 File Offset: 0x00008970
		private StringBuilder(SerializationInfo info, StreamingContext context)
		{
			this.m_currentThread = Thread.InternalGetCurrentThread();
			base..ctor();
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			int num = 0;
			string text = null;
			int num2 = int.MaxValue;
			bool flag = false;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null)
				{
					if (!(name == "m_MaxCapacity"))
					{
						if (!(name == "m_StringValue"))
						{
							if (name == "Capacity")
							{
								num = info.GetInt32("Capacity");
								flag = true;
							}
						}
						else
						{
							text = info.GetString("m_StringValue");
						}
					}
					else
					{
						num2 = info.GetInt32("m_MaxCapacity");
					}
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if (num2 < 1 || text.Length > num2)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_StringBuilderMaxCapacity"));
			}
			if (!flag)
			{
				num = 16;
				if (num < text.Length)
				{
					num = text.Length;
				}
				if (num > num2)
				{
					num = num2;
				}
			}
			if (num < 0 || num < text.Length || num > num2)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_StringBuilderCapacity"));
			}
			this.m_MaxCapacity = num2;
			this.m_StringValue = string.GetStringForStringBuilder(text, 0, text.Length, num);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00009AA0 File Offset: 0x00008AA0
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("m_MaxCapacity", this.m_MaxCapacity);
			info.AddValue("Capacity", this.Capacity);
			info.AddValue("m_StringValue", this.m_StringValue);
			info.AddValue("m_currentThread", 0);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009AFC File Offset: 0x00008AFC
		[Conditional("_DEBUG")]
		private void VerifyClassInvariant()
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00009B00 File Offset: 0x00008B00
		private string GetThreadSafeString(out IntPtr tid)
		{
			string stringValue = this.m_StringValue;
			tid = Thread.InternalGetCurrentThread();
			if (this.m_currentThread == tid)
			{
				return stringValue;
			}
			return string.GetStringForStringBuilder(stringValue, stringValue.Capacity);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00009B42 File Offset: 0x00008B42
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00009B54 File Offset: 0x00008B54
		public int Capacity
		{
			get
			{
				return this.m_StringValue.Capacity;
			}
			set
			{
				IntPtr intPtr;
				string threadSafeString = this.GetThreadSafeString(out intPtr);
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NegativeCapacity"));
				}
				if (value < threadSafeString.Length)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
				}
				if (value > this.MaxCapacity)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_Capacity"));
				}
				int capacity = threadSafeString.Capacity;
				if (value != capacity)
				{
					string stringForStringBuilder = string.GetStringForStringBuilder(threadSafeString, value);
					this.ReplaceString(intPtr, stringForStringBuilder);
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00009BDA File Offset: 0x00008BDA
		public int MaxCapacity
		{
			get
			{
				return this.m_MaxCapacity;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009BE4 File Offset: 0x00008BE4
		public int EnsureCapacity(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedPosCapacity"));
			}
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			if (!this.NeedsAllocation(threadSafeString, capacity))
			{
				return threadSafeString.Capacity;
			}
			string newString = this.GetNewString(threadSafeString, capacity);
			this.ReplaceString(intPtr, newString);
			return newString.Capacity;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00009C3C File Offset: 0x00008C3C
		public override string ToString()
		{
			string stringValue = this.m_StringValue;
			IntPtr currentThread = this.m_currentThread;
			if (currentThread != Thread.InternalGetCurrentThread())
			{
				return string.InternalCopy(stringValue);
			}
			if (2 * stringValue.Length < stringValue.ArrayLength)
			{
				return string.InternalCopy(stringValue);
			}
			stringValue.ClearPostNullChar();
			this.m_currentThread = IntPtr.Zero;
			return stringValue;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009C96 File Offset: 0x00008C96
		public string ToString(int startIndex, int length)
		{
			return this.m_StringValue.InternalSubStringWithChecks(startIndex, length, true);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00009CA8 File Offset: 0x00008CA8
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00009CB8 File Offset: 0x00008CB8
		public int Length
		{
			get
			{
				return this.m_StringValue.Length;
			}
			set
			{
				IntPtr intPtr;
				string threadSafeString = this.GetThreadSafeString(out intPtr);
				if (value == 0)
				{
					threadSafeString.SetLength(0);
					this.ReplaceString(intPtr, threadSafeString);
					return;
				}
				int length = threadSafeString.Length;
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("newlength", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
				}
				if (value > this.MaxCapacity)
				{
					throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
				}
				if (value == length)
				{
					return;
				}
				if (value <= threadSafeString.Capacity)
				{
					if (value > length)
					{
						for (int i = length; i < value; i++)
						{
							threadSafeString.InternalSetCharNoBoundsCheck(i, '\0');
						}
					}
					threadSafeString.InternalSetCharNoBoundsCheck(value, '\0');
					threadSafeString.SetLength(value);
					this.ReplaceString(intPtr, threadSafeString);
					return;
				}
				int num = ((value > threadSafeString.Capacity) ? value : threadSafeString.Capacity);
				string stringForStringBuilder = string.GetStringForStringBuilder(threadSafeString, num);
				stringForStringBuilder.SetLength(value);
				this.ReplaceString(intPtr, stringForStringBuilder);
			}
		}

		// Token: 0x17000027 RID: 39
		[IndexerName("Chars")]
		public char this[int index]
		{
			get
			{
				return this.m_StringValue[index];
			}
			set
			{
				IntPtr intPtr;
				string threadSafeString = this.GetThreadSafeString(out intPtr);
				threadSafeString.SetChar(index, value);
				this.ReplaceString(intPtr, threadSafeString);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009DCC File Offset: 0x00008DCC
		public StringBuilder Append(char value, int repeatCount)
		{
			if (repeatCount == 0)
			{
				return this;
			}
			if (repeatCount < 0)
			{
				throw new ArgumentOutOfRangeException("repeatCount", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			int num = length + repeatCount;
			if (num < 0)
			{
				throw new OutOfMemoryException();
			}
			if (!this.NeedsAllocation(threadSafeString, num))
			{
				threadSafeString.AppendInPlace(value, repeatCount, length);
				this.ReplaceString(intPtr, threadSafeString);
				return this;
			}
			string newString = this.GetNewString(threadSafeString, num);
			newString.AppendInPlace(value, repeatCount, length);
			this.ReplaceString(intPtr, newString);
			return this;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009E50 File Offset: 0x00008E50
		public StringBuilder Append(char[] value, int startIndex, int charCount)
		{
			if (value == null)
			{
				if (startIndex == 0 && charCount == 0)
				{
					return this;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				if (charCount == 0)
				{
					return this;
				}
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
				}
				if (charCount < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
				}
				if (charCount > value.Length - startIndex)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				IntPtr intPtr;
				string threadSafeString = this.GetThreadSafeString(out intPtr);
				int length = threadSafeString.Length;
				int num = length + charCount;
				if (this.NeedsAllocation(threadSafeString, num))
				{
					string newString = this.GetNewString(threadSafeString, num);
					newString.AppendInPlace(value, startIndex, charCount, length);
					this.ReplaceString(intPtr, newString);
				}
				else
				{
					threadSafeString.AppendInPlace(value, startIndex, charCount, length);
					this.ReplaceString(intPtr, threadSafeString);
				}
				return this;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009F18 File Offset: 0x00008F18
		public StringBuilder Append(string value)
		{
			if (value == null)
			{
				return this;
			}
			string text = this.m_StringValue;
			IntPtr intPtr = Thread.InternalGetCurrentThread();
			if (this.m_currentThread != intPtr)
			{
				text = string.GetStringForStringBuilder(text, text.Capacity);
			}
			int length = text.Length;
			int num = length + value.Length;
			if (this.NeedsAllocation(text, num))
			{
				string newString = this.GetNewString(text, num);
				newString.AppendInPlace(value, length);
				this.ReplaceString(intPtr, newString);
			}
			else
			{
				text.AppendInPlace(value, length);
				this.ReplaceString(intPtr, text);
			}
			return this;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009FA0 File Offset: 0x00008FA0
		internal unsafe StringBuilder Append(char* value, int count)
		{
			if (value == null)
			{
				return this;
			}
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			int num = length + count;
			if (this.NeedsAllocation(threadSafeString, num))
			{
				string newString = this.GetNewString(threadSafeString, num);
				newString.AppendInPlace(value, count, length);
				this.ReplaceString(intPtr, newString);
			}
			else
			{
				threadSafeString.AppendInPlace(value, count, length);
				this.ReplaceString(intPtr, threadSafeString);
			}
			return this;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A003 File Offset: 0x00009003
		private bool NeedsAllocation(string currentString, int requiredLength)
		{
			return currentString.ArrayLength <= requiredLength;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000A014 File Offset: 0x00009014
		private string GetNewString(string currentString, int requiredLength)
		{
			int maxCapacity = this.m_MaxCapacity;
			if (requiredLength < 0)
			{
				throw new OutOfMemoryException();
			}
			if (requiredLength > maxCapacity)
			{
				throw new ArgumentOutOfRangeException("requiredLength", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
			}
			int num = currentString.Capacity * 2;
			if (num < requiredLength)
			{
				num = requiredLength;
			}
			if (num > maxCapacity)
			{
				num = maxCapacity;
			}
			if (num <= 0)
			{
				throw new ArgumentOutOfRangeException("newCapacity", Environment.GetResourceString("ArgumentOutOfRange_NegativeCapacity"));
			}
			return string.GetStringForStringBuilder(currentString, num);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A080 File Offset: 0x00009080
		private void ReplaceString(IntPtr tid, string value)
		{
			this.m_currentThread = tid;
			this.m_StringValue = value;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000A094 File Offset: 0x00009094
		public StringBuilder Append(string value, int startIndex, int count)
		{
			if (value == null)
			{
				if (startIndex == 0 && count == 0)
				{
					return this;
				}
				throw new ArgumentNullException("value");
			}
			else if (count <= 0)
			{
				if (count == 0)
				{
					return this;
				}
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			else
			{
				if (startIndex < 0 || startIndex > value.Length - count)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				IntPtr intPtr;
				string threadSafeString = this.GetThreadSafeString(out intPtr);
				int length = threadSafeString.Length;
				int num = length + count;
				if (this.NeedsAllocation(threadSafeString, num))
				{
					string newString = this.GetNewString(threadSafeString, num);
					newString.AppendInPlace(value, startIndex, count, length);
					this.ReplaceString(intPtr, newString);
				}
				else
				{
					threadSafeString.AppendInPlace(value, startIndex, count, length);
					this.ReplaceString(intPtr, threadSafeString);
				}
				return this;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000A14A File Offset: 0x0000914A
		[ComVisible(false)]
		public StringBuilder AppendLine()
		{
			return this.Append(Environment.NewLine);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000A157 File Offset: 0x00009157
		[ComVisible(false)]
		public StringBuilder AppendLine(string value)
		{
			this.Append(value);
			return this.Append(Environment.NewLine);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000A16C File Offset: 0x0000916C
		[ComVisible(false)]
		public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("Arg_NegativeArgCount"), "count");
			}
			if (destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", new object[] { "destinationIndex" }), "destinationIndex");
			}
			if (destinationIndex > destination.Length - count)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_OffsetOut"));
			}
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			if (sourceIndex < 0 || sourceIndex > length)
			{
				throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (sourceIndex > length - count)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_LongerThanSrcString"));
			}
			if (count == 0)
			{
				return;
			}
			fixed (char* ptr = &destination[destinationIndex], ptr2 = threadSafeString)
			{
				char* ptr3 = ptr2 + sourceIndex;
				Buffer.memcpyimpl((byte*)ptr3, (byte*)ptr, count * 2);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000A260 File Offset: 0x00009260
		public StringBuilder Insert(int index, string value, int count)
		{
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			if (index < 0 || index > length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (value == null || value.Length == 0 || count == 0)
			{
				return this;
			}
			int num;
			try
			{
				num = checked(length + value.Length * count);
			}
			catch (OverflowException)
			{
				throw new OutOfMemoryException();
			}
			if (this.NeedsAllocation(threadSafeString, num))
			{
				string newString = this.GetNewString(threadSafeString, num);
				newString.InsertInPlace(index, value, count, length, num);
				this.ReplaceString(intPtr, newString);
			}
			else
			{
				threadSafeString.InsertInPlace(index, value, count, length, num);
				this.ReplaceString(intPtr, threadSafeString);
			}
			return this;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000A328 File Offset: 0x00009328
		public StringBuilder Remove(int startIndex, int length)
		{
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length2 = threadSafeString.Length;
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (length > length2 - startIndex)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			threadSafeString.RemoveInPlace(startIndex, length, length2);
			this.ReplaceString(intPtr, threadSafeString);
			return this;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A3A4 File Offset: 0x000093A4
		public StringBuilder Append(bool value)
		{
			return this.Append(value.ToString());
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000A3B3 File Offset: 0x000093B3
		[CLSCompliant(false)]
		public StringBuilder Append(sbyte value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000A3C7 File Offset: 0x000093C7
		public StringBuilder Append(byte value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000A3DC File Offset: 0x000093DC
		public StringBuilder Append(char value)
		{
			string text = this.m_StringValue;
			IntPtr intPtr = Thread.InternalGetCurrentThread();
			if (this.m_currentThread != intPtr)
			{
				text = string.GetStringForStringBuilder(text, text.Capacity);
			}
			int length = text.Length;
			if (!this.NeedsAllocation(text, length + 1))
			{
				text.AppendInPlace(value, length);
				this.ReplaceString(intPtr, text);
				return this;
			}
			string newString = this.GetNewString(text, length + 1);
			newString.AppendInPlace(value, length);
			this.ReplaceString(intPtr, newString);
			return this;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000A454 File Offset: 0x00009454
		public StringBuilder Append(short value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000A468 File Offset: 0x00009468
		public StringBuilder Append(int value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000A47C File Offset: 0x0000947C
		public StringBuilder Append(long value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A490 File Offset: 0x00009490
		public StringBuilder Append(float value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000A4A4 File Offset: 0x000094A4
		public StringBuilder Append(double value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000A4B8 File Offset: 0x000094B8
		public StringBuilder Append(decimal value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000A4CC File Offset: 0x000094CC
		[CLSCompliant(false)]
		public StringBuilder Append(ushort value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000A4E0 File Offset: 0x000094E0
		[CLSCompliant(false)]
		public StringBuilder Append(uint value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000A4F4 File Offset: 0x000094F4
		[CLSCompliant(false)]
		public StringBuilder Append(ulong value)
		{
			return this.Append(value.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A508 File Offset: 0x00009508
		public StringBuilder Append(object value)
		{
			if (value == null)
			{
				return this;
			}
			return this.Append(value.ToString());
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A51C File Offset: 0x0000951C
		public StringBuilder Append(char[] value)
		{
			if (value == null)
			{
				return this;
			}
			int num = value.Length;
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			int num2 = length + value.Length;
			if (this.NeedsAllocation(threadSafeString, num2))
			{
				string newString = this.GetNewString(threadSafeString, num2);
				newString.AppendInPlace(value, 0, num, length);
				this.ReplaceString(intPtr, newString);
			}
			else
			{
				threadSafeString.AppendInPlace(value, 0, num, length);
				this.ReplaceString(intPtr, threadSafeString);
			}
			return this;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A588 File Offset: 0x00009588
		public StringBuilder Insert(int index, string value)
		{
			if (value == null)
			{
				return this.Insert(index, value, 0);
			}
			return this.Insert(index, value, 1);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A5A0 File Offset: 0x000095A0
		public StringBuilder Insert(int index, bool value)
		{
			return this.Insert(index, value.ToString(), 1);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A5B1 File Offset: 0x000095B1
		[CLSCompliant(false)]
		public StringBuilder Insert(int index, sbyte value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000A5C7 File Offset: 0x000095C7
		public StringBuilder Insert(int index, byte value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000A5DD File Offset: 0x000095DD
		public StringBuilder Insert(int index, short value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000A5F3 File Offset: 0x000095F3
		public StringBuilder Insert(int index, char value)
		{
			return this.Insert(index, char.ToString(value), 1);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000A603 File Offset: 0x00009603
		public StringBuilder Insert(int index, char[] value)
		{
			if (value == null)
			{
				return this.Insert(index, value, 0, 0);
			}
			return this.Insert(index, value, 0, value.Length);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000A620 File Offset: 0x00009620
		public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
		{
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			if (index < 0 || index > length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (value == null)
			{
				if (startIndex == 0 && charCount == 0)
				{
					return this;
				}
				throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_String"));
			}
			else
			{
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
				}
				if (charCount < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
				}
				if (startIndex > value.Length - charCount)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (charCount == 0)
				{
					return this;
				}
				int num = length + charCount;
				if (this.NeedsAllocation(threadSafeString, num))
				{
					string newString = this.GetNewString(threadSafeString, num);
					newString.InsertInPlace(index, value, startIndex, charCount, length, num);
					this.ReplaceString(intPtr, newString);
				}
				else
				{
					threadSafeString.InsertInPlace(index, value, startIndex, charCount, length, num);
					this.ReplaceString(intPtr, threadSafeString);
				}
				return this;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000A715 File Offset: 0x00009715
		public StringBuilder Insert(int index, int value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000A72B File Offset: 0x0000972B
		public StringBuilder Insert(int index, long value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000A741 File Offset: 0x00009741
		public StringBuilder Insert(int index, float value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000A757 File Offset: 0x00009757
		public StringBuilder Insert(int index, double value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000A76D File Offset: 0x0000976D
		public StringBuilder Insert(int index, decimal value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000A783 File Offset: 0x00009783
		[CLSCompliant(false)]
		public StringBuilder Insert(int index, ushort value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000A799 File Offset: 0x00009799
		[CLSCompliant(false)]
		public StringBuilder Insert(int index, uint value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000A7AF File Offset: 0x000097AF
		[CLSCompliant(false)]
		public StringBuilder Insert(int index, ulong value)
		{
			return this.Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000A7C5 File Offset: 0x000097C5
		public StringBuilder Insert(int index, object value)
		{
			if (value == null)
			{
				return this;
			}
			return this.Insert(index, value.ToString(), 1);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000A7DC File Offset: 0x000097DC
		public StringBuilder AppendFormat(string format, object arg0)
		{
			return this.AppendFormat(null, format, new object[] { arg0 });
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000A800 File Offset: 0x00009800
		public StringBuilder AppendFormat(string format, object arg0, object arg1)
		{
			return this.AppendFormat(null, format, new object[] { arg0, arg1 });
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000A828 File Offset: 0x00009828
		public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
		{
			return this.AppendFormat(null, format, new object[] { arg0, arg1, arg2 });
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000A852 File Offset: 0x00009852
		public StringBuilder AppendFormat(string format, params object[] args)
		{
			return this.AppendFormat(null, format, args);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000A85D File Offset: 0x0000985D
		private static void FormatError()
		{
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000A870 File Offset: 0x00009870
		public StringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (format == null || args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			char[] array = format.ToCharArray(0, format.Length);
			int i = 0;
			int num = array.Length;
			char c = '\0';
			ICustomFormatter customFormatter = null;
			if (provider != null)
			{
				customFormatter = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
			}
			for (;;)
			{
				int num2 = i;
				int num3 = i;
				while (i < num)
				{
					c = array[i];
					i++;
					if (c == '}')
					{
						if (i < num && array[i] == '}')
						{
							i++;
						}
						else
						{
							StringBuilder.FormatError();
						}
					}
					if (c == '{')
					{
						if (i >= num || array[i] != '{')
						{
							i--;
							break;
						}
						i++;
					}
					array[num3++] = c;
				}
				if (num3 > num2)
				{
					this.Append(array, num2, num3 - num2);
				}
				if (i == num)
				{
					return this;
				}
				i++;
				if (i == num || (c = array[i]) < '0' || c > '9')
				{
					StringBuilder.FormatError();
				}
				int num4 = 0;
				do
				{
					num4 = num4 * 10 + (int)c - 48;
					i++;
					if (i == num)
					{
						StringBuilder.FormatError();
					}
					c = array[i];
				}
				while (c >= '0' && c <= '9' && num4 < 1000000);
				if (num4 >= args.Length)
				{
					break;
				}
				while (i < num && (c = array[i]) == ' ')
				{
					i++;
				}
				bool flag = false;
				int num5 = 0;
				if (c == ',')
				{
					i++;
					while (i < num && array[i] == ' ')
					{
						i++;
					}
					if (i == num)
					{
						StringBuilder.FormatError();
					}
					c = array[i];
					if (c == '-')
					{
						flag = true;
						i++;
						if (i == num)
						{
							StringBuilder.FormatError();
						}
						c = array[i];
					}
					if (c < '0' || c > '9')
					{
						StringBuilder.FormatError();
					}
					do
					{
						num5 = num5 * 10 + (int)c - 48;
						i++;
						if (i == num)
						{
							StringBuilder.FormatError();
						}
						c = array[i];
						if (c < '0' || c > '9')
						{
							break;
						}
					}
					while (num5 < 1000000);
				}
				while (i < num && (c = array[i]) == ' ')
				{
					i++;
				}
				object obj = args[num4];
				string text = null;
				if (c == ':')
				{
					i++;
					num2 = i;
					num3 = i;
					for (;;)
					{
						if (i == num)
						{
							StringBuilder.FormatError();
						}
						c = array[i];
						i++;
						if (c == '{')
						{
							if (i < num && array[i] == '{')
							{
								i++;
							}
							else
							{
								StringBuilder.FormatError();
							}
						}
						else if (c == '}')
						{
							if (i >= num || array[i] != '}')
							{
								break;
							}
							i++;
						}
						array[num3++] = c;
					}
					i--;
					if (num3 > num2)
					{
						text = new string(array, num2, num3 - num2);
					}
				}
				if (c != '}')
				{
					StringBuilder.FormatError();
				}
				i++;
				string text2 = null;
				if (customFormatter != null)
				{
					text2 = customFormatter.Format(text, obj, provider);
				}
				if (text2 == null)
				{
					if (obj is IFormattable)
					{
						text2 = ((IFormattable)obj).ToString(text, provider);
					}
					else if (obj != null)
					{
						text2 = obj.ToString();
					}
				}
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				int num6 = num5 - text2.Length;
				if (!flag && num6 > 0)
				{
					this.Append(' ', num6);
				}
				this.Append(text2);
				if (flag && num6 > 0)
				{
					this.Append(' ', num6);
				}
			}
			throw new FormatException(Environment.GetResourceString("Format_IndexOutOfRange"));
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000AB74 File Offset: 0x00009B74
		public StringBuilder Replace(string oldValue, string newValue)
		{
			return this.Replace(oldValue, newValue, 0, this.Length);
		}

		// Token: 0x06000265 RID: 613
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern StringBuilder Replace(string oldValue, string newValue, int startIndex, int count);

		// Token: 0x06000266 RID: 614 RVA: 0x0000AB85 File Offset: 0x00009B85
		public bool Equals(StringBuilder sb)
		{
			return sb != null && (this.Capacity == sb.Capacity && this.MaxCapacity == sb.MaxCapacity) && this.m_StringValue.Equals(sb.m_StringValue);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000ABBF File Offset: 0x00009BBF
		public StringBuilder Replace(char oldChar, char newChar)
		{
			return this.Replace(oldChar, newChar, 0, this.Length);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000ABD0 File Offset: 0x00009BD0
		public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
		{
			IntPtr intPtr;
			string threadSafeString = this.GetThreadSafeString(out intPtr);
			int length = threadSafeString.Length;
			if (startIndex > length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > length - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			threadSafeString.ReplaceCharInPlace(oldChar, newChar, startIndex, count, length);
			this.ReplaceString(intPtr, threadSafeString);
			return this;
		}

		// Token: 0x040000AC RID: 172
		internal const int DefaultCapacity = 16;

		// Token: 0x040000AD RID: 173
		private const string CapacityField = "Capacity";

		// Token: 0x040000AE RID: 174
		private const string MaxCapacityField = "m_MaxCapacity";

		// Token: 0x040000AF RID: 175
		private const string StringValueField = "m_StringValue";

		// Token: 0x040000B0 RID: 176
		private const string ThreadIDField = "m_currentThread";

		// Token: 0x040000B1 RID: 177
		internal IntPtr m_currentThread;

		// Token: 0x040000B2 RID: 178
		internal int m_MaxCapacity;

		// Token: 0x040000B3 RID: 179
		internal volatile string m_StringValue;
	}
}
