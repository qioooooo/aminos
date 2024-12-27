using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020002AF RID: 687
	[Serializable]
	internal class StackFrameHelper
	{
		// Token: 0x06001B1E RID: 6942 RVA: 0x0004738C File Offset: 0x0004638C
		public StackFrameHelper(bool fNeedFileLineColInfo, Thread target)
		{
			this.targetThread = target;
			this.rgMethodBase = null;
			this.rgMethodHandle = null;
			this.rgiOffset = null;
			this.rgiILOffset = null;
			this.rgFilename = null;
			this.rgiLineNumber = null;
			this.rgiColumnNumber = null;
			this.dynamicMethods = null;
			this.iFrameCount = 512;
			this.fNeedFileInfo = fNeedFileLineColInfo;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000473F0 File Offset: 0x000463F0
		public virtual MethodBase GetMethodBase(int i)
		{
			RuntimeMethodHandle runtimeMethodHandle = this.rgMethodHandle[i];
			if (runtimeMethodHandle.IsNullHandle())
			{
				return null;
			}
			runtimeMethodHandle = runtimeMethodHandle.GetTypicalMethodDefinition();
			return RuntimeType.GetMethodBase(runtimeMethodHandle);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00047428 File Offset: 0x00046428
		public virtual int GetOffset(int i)
		{
			return this.rgiOffset[i];
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00047432 File Offset: 0x00046432
		public virtual int GetILOffset(int i)
		{
			return this.rgiILOffset[i];
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0004743C File Offset: 0x0004643C
		public virtual string GetFilename(int i)
		{
			return this.rgFilename[i];
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00047446 File Offset: 0x00046446
		public virtual int GetLineNumber(int i)
		{
			return this.rgiLineNumber[i];
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00047450 File Offset: 0x00046450
		public virtual int GetColumnNumber(int i)
		{
			return this.rgiColumnNumber[i];
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0004745A File Offset: 0x0004645A
		public virtual int GetNumberOfFrames()
		{
			return this.iFrameCount;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00047462 File Offset: 0x00046462
		public virtual void SetNumberOfFrames(int i)
		{
			this.iFrameCount = i;
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0004746C File Offset: 0x0004646C
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			this.rgMethodBase = ((this.rgMethodHandle == null) ? null : new MethodBase[this.rgMethodHandle.Length]);
			if (this.rgMethodHandle != null)
			{
				for (int i = 0; i < this.rgMethodHandle.Length; i++)
				{
					if (!this.rgMethodHandle[i].IsNullHandle())
					{
						this.rgMethodBase[i] = RuntimeType.GetMethodBase(this.rgMethodHandle[i]);
					}
				}
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000474E3 File Offset: 0x000464E3
		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			this.rgMethodBase = null;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000474EC File Offset: 0x000464EC
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			this.rgMethodHandle = ((this.rgMethodBase == null) ? null : new RuntimeMethodHandle[this.rgMethodBase.Length]);
			if (this.rgMethodBase != null)
			{
				for (int i = 0; i < this.rgMethodBase.Length; i++)
				{
					if (this.rgMethodBase[i] != null)
					{
						this.rgMethodHandle[i] = this.rgMethodBase[i].MethodHandle;
					}
				}
			}
			this.rgMethodBase = null;
		}

		// Token: 0x04000A3B RID: 2619
		[NonSerialized]
		private Thread targetThread;

		// Token: 0x04000A3C RID: 2620
		private int[] rgiOffset;

		// Token: 0x04000A3D RID: 2621
		private int[] rgiILOffset;

		// Token: 0x04000A3E RID: 2622
		private MethodBase[] rgMethodBase;

		// Token: 0x04000A3F RID: 2623
		private object dynamicMethods;

		// Token: 0x04000A40 RID: 2624
		[NonSerialized]
		private RuntimeMethodHandle[] rgMethodHandle;

		// Token: 0x04000A41 RID: 2625
		private string[] rgFilename;

		// Token: 0x04000A42 RID: 2626
		private int[] rgiLineNumber;

		// Token: 0x04000A43 RID: 2627
		private int[] rgiColumnNumber;

		// Token: 0x04000A44 RID: 2628
		private int iFrameCount;

		// Token: 0x04000A45 RID: 2629
		private bool fNeedFileInfo;
	}
}
