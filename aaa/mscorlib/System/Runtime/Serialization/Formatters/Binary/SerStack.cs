using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D6 RID: 2006
	internal sealed class SerStack
	{
		// Token: 0x06004753 RID: 18259 RVA: 0x000F567A File Offset: 0x000F467A
		internal SerStack()
		{
			this.stackId = "System";
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x000F56A0 File Offset: 0x000F46A0
		internal SerStack(string stackId)
		{
			this.stackId = stackId;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x000F56C4 File Offset: 0x000F46C4
		internal void Push(object obj)
		{
			if (this.top == this.objects.Length - 1)
			{
				this.IncreaseCapacity();
			}
			this.objects[++this.top] = obj;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x000F5704 File Offset: 0x000F4704
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

		// Token: 0x06004757 RID: 18263 RVA: 0x000F5744 File Offset: 0x000F4744
		internal void IncreaseCapacity()
		{
			int num = this.objects.Length * 2;
			object[] array = new object[num];
			Array.Copy(this.objects, 0, array, 0, this.objects.Length);
			this.objects = array;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x000F5780 File Offset: 0x000F4780
		internal object Peek()
		{
			if (this.top < 0)
			{
				return null;
			}
			return this.objects[this.top];
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x000F579A File Offset: 0x000F479A
		internal object PeekPeek()
		{
			if (this.top < 1)
			{
				return null;
			}
			return this.objects[this.top - 1];
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x000F57B6 File Offset: 0x000F47B6
		internal int Count()
		{
			return this.top + 1;
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x000F57C0 File Offset: 0x000F47C0
		internal bool IsEmpty()
		{
			return this.top <= 0;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x000F57D0 File Offset: 0x000F47D0
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
			for (int i = 0; i < this.Count(); i++)
			{
				object obj = this.objects[i];
			}
		}

		// Token: 0x04002434 RID: 9268
		internal object[] objects = new object[5];

		// Token: 0x04002435 RID: 9269
		internal string stackId;

		// Token: 0x04002436 RID: 9270
		internal int top = -1;

		// Token: 0x04002437 RID: 9271
		internal int next;
	}
}
