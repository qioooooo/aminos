using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000322 RID: 802
	[ComVisible(true)]
	public class LocalVariableInfo
	{
		// Token: 0x06001F58 RID: 8024 RVA: 0x0004F5A8 File Offset: 0x0004E5A8
		internal LocalVariableInfo()
		{
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0004F5B0 File Offset: 0x0004E5B0
		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				this.LocalType.ToString(),
				" (",
				this.LocalIndex,
				")"
			});
			if (this.IsPinned)
			{
				text += " (pinned)";
			}
			return text;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x0004F60C File Offset: 0x0004E60C
		public virtual Type LocalType
		{
			get
			{
				return this.m_typeHandle.GetRuntimeType();
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x0004F619 File Offset: 0x0004E619
		public virtual bool IsPinned
		{
			get
			{
				return this.m_isPinned != 0;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x0004F627 File Offset: 0x0004E627
		public virtual int LocalIndex
		{
			get
			{
				return this.m_localIndex;
			}
		}

		// Token: 0x04000D4D RID: 3405
		private int m_isPinned;

		// Token: 0x04000D4E RID: 3406
		private int m_localIndex;

		// Token: 0x04000D4F RID: 3407
		private RuntimeTypeHandle m_typeHandle;
	}
}
