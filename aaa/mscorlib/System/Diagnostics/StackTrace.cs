using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020002B0 RID: 688
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[Serializable]
	public class StackTrace
	{
		// Token: 0x06001B2A RID: 6954 RVA: 0x00047561 File Offset: 0x00046561
		public StackTrace()
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, false, null, null);
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00047581 File Offset: 0x00046581
		public StackTrace(bool fNeedFileInfo)
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, fNeedFileInfo, null, null);
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000475A1 File Offset: 0x000465A1
		public StackTrace(int skipFrames)
		{
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, false, null, null);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000475DA File Offset: 0x000465DA
		public StackTrace(int skipFrames, bool fNeedFileInfo)
		{
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, fNeedFileInfo, null, null);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x00047613 File Offset: 0x00046613
		public StackTrace(Exception e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, false, null, e);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00047641 File Offset: 0x00046641
		public StackTrace(Exception e, bool fNeedFileInfo)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, fNeedFileInfo, null, e);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00047670 File Offset: 0x00046670
		public StackTrace(Exception e, int skipFrames)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, false, null, e);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000476C4 File Offset: 0x000466C4
		public StackTrace(Exception e, int skipFrames, bool fNeedFileInfo)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, fNeedFileInfo, null, e);
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x00047716 File Offset: 0x00046716
		public StackTrace(StackFrame frame)
		{
			this.frames = new StackFrame[1];
			this.frames[0] = frame;
			this.m_iMethodsToSkip = 0;
			this.m_iNumOfFrames = 1;
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00047741 File Offset: 0x00046741
		public StackTrace(Thread targetThread, bool needFileInfo)
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, needFileInfo, targetThread, null);
		}

		// Token: 0x06001B34 RID: 6964
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetStackFramesInternal(StackFrameHelper sfh, int iSkip, Exception e);

		// Token: 0x06001B35 RID: 6965 RVA: 0x00047764 File Offset: 0x00046764
		internal static int CalculateFramesToSkip(StackFrameHelper StackF, int iNumFrames)
		{
			int num = 0;
			string text = "System.Diagnostics";
			for (int i = 0; i < iNumFrames; i++)
			{
				MethodBase methodBase = StackF.GetMethodBase(i);
				if (methodBase != null)
				{
					Type declaringType = methodBase.DeclaringType;
					if (declaringType == null)
					{
						break;
					}
					string @namespace = declaringType.Namespace;
					if (@namespace == null || string.Compare(@namespace, text, StringComparison.Ordinal) != 0)
					{
						break;
					}
				}
				num++;
			}
			return num;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000477BC File Offset: 0x000467BC
		private void CaptureStackTrace(int iSkip, bool fNeedFileInfo, Thread targetThread, Exception e)
		{
			this.m_iMethodsToSkip += iSkip;
			StackFrameHelper stackFrameHelper = new StackFrameHelper(fNeedFileInfo, targetThread);
			StackTrace.GetStackFramesInternal(stackFrameHelper, 0, e);
			this.m_iNumOfFrames = stackFrameHelper.GetNumberOfFrames();
			if (this.m_iMethodsToSkip > this.m_iNumOfFrames)
			{
				this.m_iMethodsToSkip = this.m_iNumOfFrames;
			}
			if (this.m_iNumOfFrames != 0)
			{
				this.frames = new StackFrame[this.m_iNumOfFrames];
				for (int i = 0; i < this.m_iNumOfFrames; i++)
				{
					bool flag = true;
					bool flag2 = true;
					StackFrame stackFrame = new StackFrame(flag, flag2);
					stackFrame.SetMethodBase(stackFrameHelper.GetMethodBase(i));
					stackFrame.SetOffset(stackFrameHelper.GetOffset(i));
					stackFrame.SetILOffset(stackFrameHelper.GetILOffset(i));
					if (fNeedFileInfo)
					{
						stackFrame.SetFileName(stackFrameHelper.GetFilename(i));
						stackFrame.SetLineNumber(stackFrameHelper.GetLineNumber(i));
						stackFrame.SetColumnNumber(stackFrameHelper.GetColumnNumber(i));
					}
					this.frames[i] = stackFrame;
				}
				if (e == null)
				{
					this.m_iMethodsToSkip += StackTrace.CalculateFramesToSkip(stackFrameHelper, this.m_iNumOfFrames);
				}
				this.m_iNumOfFrames -= this.m_iMethodsToSkip;
				if (this.m_iNumOfFrames < 0)
				{
					this.m_iNumOfFrames = 0;
					return;
				}
			}
			else
			{
				this.frames = null;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x000478F1 File Offset: 0x000468F1
		public virtual int FrameCount
		{
			get
			{
				return this.m_iNumOfFrames;
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000478F9 File Offset: 0x000468F9
		public virtual StackFrame GetFrame(int index)
		{
			if (this.frames != null && index < this.m_iNumOfFrames && index >= 0)
			{
				return this.frames[index + this.m_iMethodsToSkip];
			}
			return null;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00047924 File Offset: 0x00046924
		[ComVisible(false)]
		public virtual StackFrame[] GetFrames()
		{
			if (this.frames == null || this.m_iNumOfFrames <= 0)
			{
				return null;
			}
			StackFrame[] array = new StackFrame[this.m_iNumOfFrames];
			Array.Copy(this.frames, this.m_iMethodsToSkip, array, 0, this.m_iNumOfFrames);
			return array;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0004796A File Offset: 0x0004696A
		public override string ToString()
		{
			return this.ToString(StackTrace.TraceFormat.TrailingNewLine);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00047974 File Offset: 0x00046974
		internal string ToString(StackTrace.TraceFormat traceFormat)
		{
			string text = "at";
			string text2 = "in {0}:line {1}";
			if (traceFormat != StackTrace.TraceFormat.NoResourceLookup)
			{
				text = Environment.GetResourceString("Word_At");
				text2 = Environment.GetResourceString("StackTrace_InFileLineNumber");
			}
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder(255);
			for (int i = 0; i < this.m_iNumOfFrames; i++)
			{
				StackFrame frame = this.GetFrame(i);
				MethodBase method = frame.GetMethod();
				if (method != null)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "   {0} ", new object[] { text });
					Type declaringType = method.DeclaringType;
					if (declaringType != null)
					{
						stringBuilder.Append(declaringType.FullName.Replace('+', '.'));
						stringBuilder.Append(".");
					}
					stringBuilder.Append(method.Name);
					if (method is MethodInfo && ((MethodInfo)method).IsGenericMethod)
					{
						Type[] genericArguments = ((MethodInfo)method).GetGenericArguments();
						stringBuilder.Append("[");
						int j = 0;
						bool flag2 = true;
						while (j < genericArguments.Length)
						{
							if (!flag2)
							{
								stringBuilder.Append(",");
							}
							else
							{
								flag2 = false;
							}
							stringBuilder.Append(genericArguments[j].Name);
							j++;
						}
						stringBuilder.Append("]");
					}
					stringBuilder.Append("(");
					ParameterInfo[] parameters = method.GetParameters();
					bool flag3 = true;
					for (int k = 0; k < parameters.Length; k++)
					{
						if (!flag3)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							flag3 = false;
						}
						string text3 = "<UnknownType>";
						if (parameters[k].ParameterType != null)
						{
							text3 = parameters[k].ParameterType.Name;
						}
						stringBuilder.Append(text3 + " " + parameters[k].Name);
					}
					stringBuilder.Append(")");
					if (frame.GetILOffset() != -1)
					{
						string text4 = null;
						try
						{
							text4 = frame.GetFileName();
						}
						catch (SecurityException)
						{
						}
						if (text4 != null)
						{
							stringBuilder.Append(' ');
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, text2, new object[]
							{
								text4,
								frame.GetFileLineNumber()
							});
						}
					}
				}
			}
			if (traceFormat == StackTrace.TraceFormat.TrailingNewLine)
			{
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00047BDC File Offset: 0x00046BDC
		private static string GetManagedStackTraceStringHelper(bool fNeedFileInfo)
		{
			StackTrace stackTrace = new StackTrace(0, fNeedFileInfo);
			return stackTrace.ToString();
		}

		// Token: 0x04000A46 RID: 2630
		public const int METHODS_TO_SKIP = 0;

		// Token: 0x04000A47 RID: 2631
		private StackFrame[] frames;

		// Token: 0x04000A48 RID: 2632
		private int m_iNumOfFrames;

		// Token: 0x04000A49 RID: 2633
		private int m_iMethodsToSkip;

		// Token: 0x020002B1 RID: 689
		internal enum TraceFormat
		{
			// Token: 0x04000A4B RID: 2635
			Normal,
			// Token: 0x04000A4C RID: 2636
			TrailingNewLine,
			// Token: 0x04000A4D RID: 2637
			NoResourceLookup
		}
	}
}
