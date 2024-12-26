using System;

namespace Microsoft.JScript
{
	// Token: 0x02000117 RID: 279
	internal sealed class Stack
	{
		// Token: 0x06000B83 RID: 2947 RVA: 0x0005798F File Offset: 0x0005698F
		internal Stack()
		{
			this.elements = new object[32];
			this.top = -1;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000579AC File Offset: 0x000569AC
		internal void GuardedPush(object item)
		{
			if (this.top > 500)
			{
				throw new JScriptException(JSError.OutOfStack);
			}
			if (++this.top >= this.elements.Length)
			{
				object[] array = new object[this.elements.Length + 32];
				ArrayObject.Copy(this.elements, array, this.elements.Length);
				this.elements = array;
			}
			this.elements[this.top] = item;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00057A24 File Offset: 0x00056A24
		internal void Push(object item)
		{
			if (++this.top >= this.elements.Length)
			{
				object[] array = new object[this.elements.Length + 32];
				ArrayObject.Copy(this.elements, array, this.elements.Length);
				this.elements = array;
			}
			this.elements[this.top] = item;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00057A88 File Offset: 0x00056A88
		internal object Pop()
		{
			object obj = this.elements[this.top];
			this.elements[this.top--] = null;
			return obj;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00057ABD File Offset: 0x00056ABD
		internal ScriptObject Peek()
		{
			if (this.top < 0)
			{
				return null;
			}
			return (ScriptObject)this.elements[this.top];
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00057ADC File Offset: 0x00056ADC
		internal object Peek(int i)
		{
			return this.elements[this.top - i];
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00057AED File Offset: 0x00056AED
		internal int Size()
		{
			return this.top + 1;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00057AF7 File Offset: 0x00056AF7
		internal void TrimToSize(int i)
		{
			this.top = i - 1;
		}

		// Token: 0x040006F0 RID: 1776
		private object[] elements;

		// Token: 0x040006F1 RID: 1777
		private int top;
	}
}
