using System;
using System.Collections;

namespace System.Security.Policy
{
	// Token: 0x0200049C RID: 1180
	internal sealed class CodeGroupStack
	{
		// Token: 0x06002F81 RID: 12161 RVA: 0x000A3A6D File Offset: 0x000A2A6D
		internal CodeGroupStack()
		{
			this.m_array = new ArrayList();
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000A3A80 File Offset: 0x000A2A80
		internal void Push(CodeGroupStackFrame element)
		{
			this.m_array.Add(element);
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000A3A90 File Offset: 0x000A2A90
		internal CodeGroupStackFrame Pop()
		{
			if (this.IsEmpty())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			int count = this.m_array.Count;
			CodeGroupStackFrame codeGroupStackFrame = (CodeGroupStackFrame)this.m_array[count - 1];
			this.m_array.RemoveAt(count - 1);
			return codeGroupStackFrame;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000A3AE4 File Offset: 0x000A2AE4
		internal bool IsEmpty()
		{
			return this.m_array.Count == 0;
		}

		// Token: 0x040017FE RID: 6142
		private ArrayList m_array;
	}
}
