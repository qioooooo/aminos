using System;
using System.Diagnostics;
using System.Globalization;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000009 RID: 9
	internal sealed class SerStack
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004E04 File Offset: 0x00003E04
		internal SerStack(string stackId)
		{
			this.stackId = stackId;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004E27 File Offset: 0x00003E27
		internal object GetItem(int index)
		{
			return this.objects[index];
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004E31 File Offset: 0x00003E31
		internal void Clear()
		{
			this.top = -1;
			this.next = 0;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004E44 File Offset: 0x00003E44
		internal void Push(object obj)
		{
			if (this.top == this.objects.Length - 1)
			{
				this.IncreaseCapacity();
			}
			this.objects[++this.top] = obj;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004E84 File Offset: 0x00003E84
		internal object Pop()
		{
			if (this.top < 0)
			{
				return null;
			}
			object obj = this.objects[this.top];
			this.objects[this.top--] = null;
			return obj;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004EC4 File Offset: 0x00003EC4
		internal object Next()
		{
			if (this.next > this.top)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_StackRange"), new object[] { this.stackId }));
			}
			return this.objects[this.next++];
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004F24 File Offset: 0x00003F24
		internal void IncreaseCapacity()
		{
			int num = this.objects.Length * 2;
			object[] array = new object[num];
			Array.Copy(this.objects, 0, array, 0, this.objects.Length);
			this.objects = array;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004F60 File Offset: 0x00003F60
		internal object Peek()
		{
			if (this.top < 0)
			{
				return null;
			}
			return this.objects[this.top];
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004F7A File Offset: 0x00003F7A
		internal object PeekPeek()
		{
			if (this.top < 1)
			{
				return null;
			}
			return this.objects[this.top - 1];
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004F96 File Offset: 0x00003F96
		internal int Count()
		{
			return this.top + 1;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004FA0 File Offset: 0x00003FA0
		internal bool IsEmpty()
		{
			return this.top <= 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004FAE File Offset: 0x00003FAE
		internal void Reverse()
		{
			Array.Reverse(this.objects, 0, this.Count());
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004FC4 File Offset: 0x00003FC4
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
			for (int i = 0; i < this.Count(); i++)
			{
				object obj = this.objects[i];
			}
		}

		// Token: 0x0400003A RID: 58
		internal object[] objects = new object[10];

		// Token: 0x0400003B RID: 59
		internal string stackId;

		// Token: 0x0400003C RID: 60
		internal int top = -1;

		// Token: 0x0400003D RID: 61
		internal int next;
	}
}
