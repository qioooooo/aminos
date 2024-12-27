using System;

namespace System.Reflection.Emit
{
	// Token: 0x0200081C RID: 2076
	internal struct __ExceptionInstance
	{
		// Token: 0x06004A8E RID: 19086 RVA: 0x00103DAF File Offset: 0x00102DAF
		internal __ExceptionInstance(int start, int end, int filterAddr, int handle, int handleEnd, int type, int exceptionClass)
		{
			this.m_startAddress = start;
			this.m_endAddress = end;
			this.m_filterAddress = filterAddr;
			this.m_handleAddress = handle;
			this.m_handleEndAddress = handleEnd;
			this.m_type = type;
			this.m_exceptionClass = exceptionClass;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x00103DE8 File Offset: 0x00102DE8
		public override bool Equals(object obj)
		{
			if (obj != null && obj is __ExceptionInstance)
			{
				__ExceptionInstance _ExceptionInstance = (__ExceptionInstance)obj;
				return _ExceptionInstance.m_exceptionClass == this.m_exceptionClass && _ExceptionInstance.m_startAddress == this.m_startAddress && _ExceptionInstance.m_endAddress == this.m_endAddress && _ExceptionInstance.m_filterAddress == this.m_filterAddress && _ExceptionInstance.m_handleAddress == this.m_handleAddress && _ExceptionInstance.m_handleEndAddress == this.m_handleEndAddress;
			}
			return false;
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x00103E65 File Offset: 0x00102E65
		public override int GetHashCode()
		{
			return this.m_exceptionClass ^ this.m_startAddress ^ this.m_endAddress ^ this.m_filterAddress ^ this.m_handleAddress ^ this.m_handleEndAddress ^ this.m_type;
		}

		// Token: 0x040025FF RID: 9727
		internal int m_exceptionClass;

		// Token: 0x04002600 RID: 9728
		internal int m_startAddress;

		// Token: 0x04002601 RID: 9729
		internal int m_endAddress;

		// Token: 0x04002602 RID: 9730
		internal int m_filterAddress;

		// Token: 0x04002603 RID: 9731
		internal int m_handleAddress;

		// Token: 0x04002604 RID: 9732
		internal int m_handleEndAddress;

		// Token: 0x04002605 RID: 9733
		internal int m_type;
	}
}
