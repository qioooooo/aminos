using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020002B2 RID: 690
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[Serializable]
	public class StackFrame
	{
		// Token: 0x06001B3D RID: 6973 RVA: 0x00047BF7 File Offset: 0x00046BF7
		internal void InitMembers()
		{
			this.method = null;
			this.offset = -1;
			this.ILOffset = -1;
			this.strFileName = null;
			this.iLineNumber = 0;
			this.iColumnNumber = 0;
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00047C23 File Offset: 0x00046C23
		public StackFrame()
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00047C39 File Offset: 0x00046C39
		public StackFrame(bool fNeedFileInfo)
		{
			this.InitMembers();
			this.BuildStackFrame(0, fNeedFileInfo);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00047C4F File Offset: 0x00046C4F
		public StackFrame(int skipFrames)
		{
			this.InitMembers();
			this.BuildStackFrame(skipFrames, false);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00047C65 File Offset: 0x00046C65
		public StackFrame(int skipFrames, bool fNeedFileInfo)
		{
			this.InitMembers();
			this.BuildStackFrame(skipFrames, fNeedFileInfo);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00047C7B File Offset: 0x00046C7B
		internal StackFrame(bool DummyFlag1, bool DummyFlag2)
		{
			this.InitMembers();
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00047C89 File Offset: 0x00046C89
		public StackFrame(string fileName, int lineNumber)
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
			this.strFileName = fileName;
			this.iLineNumber = lineNumber;
			this.iColumnNumber = 0;
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00047CB4 File Offset: 0x00046CB4
		public StackFrame(string fileName, int lineNumber, int colNumber)
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
			this.strFileName = fileName;
			this.iLineNumber = lineNumber;
			this.iColumnNumber = colNumber;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00047CDF File Offset: 0x00046CDF
		internal virtual void SetMethodBase(MethodBase mb)
		{
			this.method = mb;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00047CE8 File Offset: 0x00046CE8
		internal virtual void SetOffset(int iOffset)
		{
			this.offset = iOffset;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00047CF1 File Offset: 0x00046CF1
		internal virtual void SetILOffset(int iOffset)
		{
			this.ILOffset = iOffset;
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00047CFA File Offset: 0x00046CFA
		internal virtual void SetFileName(string strFName)
		{
			this.strFileName = strFName;
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00047D03 File Offset: 0x00046D03
		internal virtual void SetLineNumber(int iLine)
		{
			this.iLineNumber = iLine;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00047D0C File Offset: 0x00046D0C
		internal virtual void SetColumnNumber(int iCol)
		{
			this.iColumnNumber = iCol;
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x00047D15 File Offset: 0x00046D15
		public virtual MethodBase GetMethod()
		{
			return this.method;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00047D1D File Offset: 0x00046D1D
		public virtual int GetNativeOffset()
		{
			return this.offset;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x00047D25 File Offset: 0x00046D25
		public virtual int GetILOffset()
		{
			return this.ILOffset;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00047D30 File Offset: 0x00046D30
		public virtual string GetFileName()
		{
			if (this.strFileName != null)
			{
				new FileIOPermission(PermissionState.None)
				{
					AllFiles = FileIOPermissionAccess.PathDiscovery
				}.Demand();
			}
			return this.strFileName;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00047D5F File Offset: 0x00046D5F
		public virtual int GetFileLineNumber()
		{
			return this.iLineNumber;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00047D67 File Offset: 0x00046D67
		public virtual int GetFileColumnNumber()
		{
			return this.iColumnNumber;
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00047D70 File Offset: 0x00046D70
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			if (this.method != null)
			{
				stringBuilder.Append(this.method.Name);
				if (this.method is MethodInfo && ((MethodInfo)this.method).IsGenericMethod)
				{
					Type[] genericArguments = ((MethodInfo)this.method).GetGenericArguments();
					stringBuilder.Append("<");
					int i = 0;
					bool flag = true;
					while (i < genericArguments.Length)
					{
						if (!flag)
						{
							stringBuilder.Append(",");
						}
						else
						{
							flag = false;
						}
						stringBuilder.Append(genericArguments[i].Name);
						i++;
					}
					stringBuilder.Append(">");
				}
				stringBuilder.Append(" at offset ");
				if (this.offset == -1)
				{
					stringBuilder.Append("<offset unknown>");
				}
				else
				{
					stringBuilder.Append(this.offset);
				}
				stringBuilder.Append(" in file:line:column ");
				bool flag2 = this.strFileName != null;
				if (flag2)
				{
					try
					{
						new FileIOPermission(PermissionState.None)
						{
							AllFiles = FileIOPermissionAccess.PathDiscovery
						}.Demand();
					}
					catch (SecurityException)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					stringBuilder.Append("<filename unknown>");
				}
				else
				{
					stringBuilder.Append(this.strFileName);
				}
				stringBuilder.Append(":");
				stringBuilder.Append(this.iLineNumber);
				stringBuilder.Append(":");
				stringBuilder.Append(this.iColumnNumber);
			}
			else
			{
				stringBuilder.Append("<null>");
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00047F10 File Offset: 0x00046F10
		private void BuildStackFrame(int skipFrames, bool fNeedFileInfo)
		{
			StackFrameHelper stackFrameHelper = new StackFrameHelper(fNeedFileInfo, null);
			StackTrace.GetStackFramesInternal(stackFrameHelper, 0, null);
			int numberOfFrames = stackFrameHelper.GetNumberOfFrames();
			skipFrames += StackTrace.CalculateFramesToSkip(stackFrameHelper, numberOfFrames);
			if (numberOfFrames - skipFrames > 0)
			{
				this.method = stackFrameHelper.GetMethodBase(skipFrames);
				this.offset = stackFrameHelper.GetOffset(skipFrames);
				this.ILOffset = stackFrameHelper.GetILOffset(skipFrames);
				if (fNeedFileInfo)
				{
					this.strFileName = stackFrameHelper.GetFilename(skipFrames);
					this.iLineNumber = stackFrameHelper.GetLineNumber(skipFrames);
					this.iColumnNumber = stackFrameHelper.GetColumnNumber(skipFrames);
				}
			}
		}

		// Token: 0x04000A4E RID: 2638
		public const int OFFSET_UNKNOWN = -1;

		// Token: 0x04000A4F RID: 2639
		private MethodBase method;

		// Token: 0x04000A50 RID: 2640
		private int offset;

		// Token: 0x04000A51 RID: 2641
		private int ILOffset;

		// Token: 0x04000A52 RID: 2642
		private string strFileName;

		// Token: 0x04000A53 RID: 2643
		private int iLineNumber;

		// Token: 0x04000A54 RID: 2644
		private int iColumnNumber;
	}
}
