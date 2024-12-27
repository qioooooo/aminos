using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System
{
	// Token: 0x02000030 RID: 48
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Exception))]
	[Serializable]
	public class Exception : ISerializable, _Exception
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0000AC3C File Offset: 0x00009C3C
		public Exception()
		{
			this._message = null;
			this._stackTrace = null;
			this._dynamicMethods = null;
			this.HResult = -2146233088;
			this._xcode = -532459699;
			this._xptrs = (IntPtr)0;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000AC7B File Offset: 0x00009C7B
		public Exception(string message)
		{
			this._message = message;
			this._stackTrace = null;
			this._dynamicMethods = null;
			this.HResult = -2146233088;
			this._xcode = -532459699;
			this._xptrs = (IntPtr)0;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000ACBC File Offset: 0x00009CBC
		public Exception(string message, Exception innerException)
		{
			this._message = message;
			this._stackTrace = null;
			this._dynamicMethods = null;
			this._innerException = innerException;
			this.HResult = -2146233088;
			this._xcode = -532459699;
			this._xptrs = (IntPtr)0;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000AD10 File Offset: 0x00009D10
		protected Exception(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this._className = info.GetString("ClassName");
			this._message = info.GetString("Message");
			this._data = (IDictionary)info.GetValueNoThrow("Data", typeof(IDictionary));
			this._innerException = (Exception)info.GetValue("InnerException", typeof(Exception));
			this._helpURL = info.GetString("HelpURL");
			this._stackTraceString = info.GetString("StackTraceString");
			this._remoteStackTraceString = info.GetString("RemoteStackTraceString");
			this._remoteStackIndex = info.GetInt32("RemoteStackIndex");
			this._exceptionMethodString = (string)info.GetValue("ExceptionMethod", typeof(string));
			this.HResult = info.GetInt32("HResult");
			this._source = info.GetString("Source");
			if (this._className == null || this.HResult == 0)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
			if (context.State == StreamingContextStates.CrossAppDomain)
			{
				this._remoteStackTraceString += this._stackTraceString;
				this._stackTraceString = null;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000AE68 File Offset: 0x00009E68
		public virtual string Message
		{
			get
			{
				if (this._message == null)
				{
					if (this._className == null)
					{
						this._className = this.GetClassName();
					}
					return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Exception_WasThrown"), new object[] { this._className });
				}
				return this._message;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000AEBD File Offset: 0x00009EBD
		public virtual IDictionary Data
		{
			get
			{
				return this.GetDataInternal();
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000AEC5 File Offset: 0x00009EC5
		internal IDictionary GetDataInternal()
		{
			if (this._data == null)
			{
				if (Exception.IsImmutableAgileException(this))
				{
					this._data = new EmptyReadOnlyDictionaryInternal();
				}
				else
				{
					this._data = new ListDictionaryInternal();
				}
			}
			return this._data;
		}

		// Token: 0x0600027E RID: 638
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsImmutableAgileException(Exception e);

		// Token: 0x0600027F RID: 639
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetClassName();

		// Token: 0x06000280 RID: 640 RVA: 0x0000AEF8 File Offset: 0x00009EF8
		public virtual Exception GetBaseException()
		{
			Exception ex = this.InnerException;
			Exception ex2 = this;
			while (ex != null)
			{
				ex2 = ex;
				ex = ex.InnerException;
			}
			return ex2;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000AF1D File Offset: 0x00009F1D
		public Exception InnerException
		{
			get
			{
				return this._innerException;
			}
		}

		// Token: 0x06000282 RID: 642
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* _InternalGetMethod(object stackTrace);

		// Token: 0x06000283 RID: 643 RVA: 0x0000AF25 File Offset: 0x00009F25
		private static RuntimeMethodHandle InternalGetMethod(object stackTrace)
		{
			return new RuntimeMethodHandle(Exception._InternalGetMethod(stackTrace));
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000AF32 File Offset: 0x00009F32
		public MethodBase TargetSite
		{
			get
			{
				return this.GetTargetSiteInternal();
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000AF3C File Offset: 0x00009F3C
		private MethodBase GetTargetSiteInternal()
		{
			if (this._exceptionMethod != null)
			{
				return this._exceptionMethod;
			}
			if (this._stackTrace == null)
			{
				return null;
			}
			if (this._exceptionMethodString != null)
			{
				this._exceptionMethod = this.GetExceptionMethodFromString();
			}
			else
			{
				RuntimeMethodHandle typicalMethodDefinition = Exception.InternalGetMethod(this._stackTrace).GetTypicalMethodDefinition();
				this._exceptionMethod = RuntimeType.GetMethodBase(typicalMethodDefinition);
			}
			return this._exceptionMethod;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000AFA0 File Offset: 0x00009FA0
		public virtual string StackTrace
		{
			get
			{
				if (this._stackTraceString != null)
				{
					return this._remoteStackTraceString + this._stackTraceString;
				}
				if (this._stackTrace == null)
				{
					return this._remoteStackTraceString;
				}
				string stackTrace = Environment.GetStackTrace(this, true);
				return this._remoteStackTraceString + stackTrace;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000AFEA File Offset: 0x00009FEA
		internal void SetErrorCode(int hr)
		{
			this.HResult = hr;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000AFF3 File Offset: 0x00009FF3
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000AFFB File Offset: 0x00009FFB
		public virtual string HelpLink
		{
			get
			{
				return this._helpURL;
			}
			set
			{
				this._helpURL = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000B004 File Offset: 0x0000A004
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000B055 File Offset: 0x0000A055
		public virtual string Source
		{
			get
			{
				if (this._source == null)
				{
					StackTrace stackTrace = new StackTrace(this, true);
					if (stackTrace.FrameCount > 0)
					{
						StackFrame frame = stackTrace.GetFrame(0);
						MethodBase method = frame.GetMethod();
						this._source = method.Module.Assembly.nGetSimpleName();
					}
				}
				return this._source;
			}
			set
			{
				this._source = value;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000B060 File Offset: 0x0000A060
		public override string ToString()
		{
			string message = this.Message;
			if (this._className == null)
			{
				this._className = this.GetClassName();
			}
			string text;
			if (message == null || message.Length <= 0)
			{
				text = this._className;
			}
			else
			{
				text = this._className + ": " + message;
			}
			if (this._innerException != null)
			{
				text = string.Concat(new string[]
				{
					text,
					" ---> ",
					this._innerException.ToString(),
					Environment.NewLine,
					"   ",
					Environment.GetResourceString("Exception_EndOfInnerExceptionStack")
				});
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			return text;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B118 File Offset: 0x0000A118
		private string GetExceptionMethodString()
		{
			MethodBase targetSiteInternal = this.GetTargetSiteInternal();
			if (targetSiteInternal == null)
			{
				return null;
			}
			if (targetSiteInternal is DynamicMethod.RTDynamicMethod)
			{
				return null;
			}
			char c = '\n';
			StringBuilder stringBuilder = new StringBuilder();
			if (targetSiteInternal is ConstructorInfo)
			{
				RuntimeConstructorInfo runtimeConstructorInfo = (RuntimeConstructorInfo)targetSiteInternal;
				Type reflectedType = runtimeConstructorInfo.ReflectedType;
				stringBuilder.Append(1);
				stringBuilder.Append(c);
				stringBuilder.Append(runtimeConstructorInfo.Name);
				if (reflectedType != null)
				{
					stringBuilder.Append(c);
					stringBuilder.Append(reflectedType.Assembly.FullName);
					stringBuilder.Append(c);
					stringBuilder.Append(reflectedType.FullName);
				}
				stringBuilder.Append(c);
				stringBuilder.Append(runtimeConstructorInfo.ToString());
			}
			else
			{
				RuntimeMethodInfo runtimeMethodInfo = (RuntimeMethodInfo)targetSiteInternal;
				Type declaringType = runtimeMethodInfo.DeclaringType;
				stringBuilder.Append(8);
				stringBuilder.Append(c);
				stringBuilder.Append(runtimeMethodInfo.Name);
				stringBuilder.Append(c);
				stringBuilder.Append(runtimeMethodInfo.Module.Assembly.FullName);
				stringBuilder.Append(c);
				if (declaringType != null)
				{
					stringBuilder.Append(declaringType.FullName);
					stringBuilder.Append(c);
				}
				stringBuilder.Append(runtimeMethodInfo.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B24C File Offset: 0x0000A24C
		private MethodBase GetExceptionMethodFromString()
		{
			string[] array = this._exceptionMethodString.Split(new char[] { '\0', '\n' });
			if (array.Length != 5)
			{
				throw new SerializationException();
			}
			SerializationInfo serializationInfo = new SerializationInfo(typeof(MemberInfoSerializationHolder), new FormatterConverter());
			serializationInfo.AddValue("MemberType", int.Parse(array[0], CultureInfo.InvariantCulture), typeof(int));
			serializationInfo.AddValue("Name", array[1], typeof(string));
			serializationInfo.AddValue("AssemblyName", array[2], typeof(string));
			serializationInfo.AddValue("ClassName", array[3]);
			serializationInfo.AddValue("Signature", array[4]);
			StreamingContext streamingContext = new StreamingContext(StreamingContextStates.All);
			MethodBase methodBase;
			try
			{
				methodBase = (MethodBase)new MemberInfoSerializationHolder(serializationInfo, streamingContext).GetRealObject(streamingContext);
			}
			catch (SerializationException)
			{
				methodBase = null;
			}
			return methodBase;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000B340 File Offset: 0x0000A340
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			string text = this._stackTraceString;
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this._className == null)
			{
				this._className = this.GetClassName();
			}
			if (this._stackTrace != null)
			{
				if (text == null)
				{
					text = Environment.GetStackTrace(this, true);
				}
				if (this._exceptionMethod == null)
				{
					RuntimeMethodHandle typicalMethodDefinition = Exception.InternalGetMethod(this._stackTrace).GetTypicalMethodDefinition();
					this._exceptionMethod = RuntimeType.GetMethodBase(typicalMethodDefinition);
				}
			}
			if (this._source == null)
			{
				this._source = this.Source;
			}
			info.AddValue("ClassName", this._className, typeof(string));
			info.AddValue("Message", this._message, typeof(string));
			info.AddValue("Data", this._data, typeof(IDictionary));
			info.AddValue("InnerException", this._innerException, typeof(Exception));
			info.AddValue("HelpURL", this._helpURL, typeof(string));
			info.AddValue("StackTraceString", text, typeof(string));
			info.AddValue("RemoteStackTraceString", this._remoteStackTraceString, typeof(string));
			info.AddValue("RemoteStackIndex", this._remoteStackIndex, typeof(int));
			info.AddValue("ExceptionMethod", this.GetExceptionMethodString(), typeof(string));
			info.AddValue("HResult", this.HResult);
			info.AddValue("Source", this._source, typeof(string));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B4E4 File Offset: 0x0000A4E4
		internal Exception PrepForRemoting()
		{
			string text;
			if (this._remoteStackIndex == 0)
			{
				text = string.Concat(new object[]
				{
					Environment.NewLine,
					"Server stack trace: ",
					Environment.NewLine,
					this.StackTrace,
					Environment.NewLine,
					Environment.NewLine,
					"Exception rethrown at [",
					this._remoteStackIndex,
					"]: ",
					Environment.NewLine
				});
			}
			else
			{
				text = string.Concat(new object[]
				{
					this.StackTrace,
					Environment.NewLine,
					Environment.NewLine,
					"Exception rethrown at [",
					this._remoteStackIndex,
					"]: ",
					Environment.NewLine
				});
			}
			this._remoteStackTraceString = text;
			this._remoteStackIndex++;
			return this;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B5C8 File Offset: 0x0000A5C8
		internal void InternalPreserveStackTrace()
		{
			string stackTrace = this.StackTrace;
			if (stackTrace != null && stackTrace.Length > 0)
			{
				this._remoteStackTraceString = stackTrace + Environment.NewLine;
			}
			this._stackTrace = null;
			this._stackTraceString = null;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000B607 File Offset: 0x0000A607
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000B60F File Offset: 0x0000A60F
		protected int HResult
		{
			get
			{
				return this._HResult;
			}
			set
			{
				this._HResult = value;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B618 File Offset: 0x0000A618
		internal virtual string InternalToString()
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy);
				securityPermission.Assert();
			}
			catch
			{
			}
			return this.ToString();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B650 File Offset: 0x0000A650
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000B658 File Offset: 0x0000A658
		internal bool IsTransient
		{
			get
			{
				return Exception.nIsTransient(this._HResult);
			}
		}

		// Token: 0x06000297 RID: 663
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nIsTransient(int hr);

		// Token: 0x06000298 RID: 664
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetMessageFromNativeResources(Exception.ExceptionMessageKind kind);

		// Token: 0x040000B4 RID: 180
		private const int _COMPlusExceptionCode = -532459699;

		// Token: 0x040000B5 RID: 181
		private string _className;

		// Token: 0x040000B6 RID: 182
		private MethodBase _exceptionMethod;

		// Token: 0x040000B7 RID: 183
		private string _exceptionMethodString;

		// Token: 0x040000B8 RID: 184
		internal string _message;

		// Token: 0x040000B9 RID: 185
		private IDictionary _data;

		// Token: 0x040000BA RID: 186
		private Exception _innerException;

		// Token: 0x040000BB RID: 187
		private string _helpURL;

		// Token: 0x040000BC RID: 188
		private object _stackTrace;

		// Token: 0x040000BD RID: 189
		private string _stackTraceString;

		// Token: 0x040000BE RID: 190
		private string _remoteStackTraceString;

		// Token: 0x040000BF RID: 191
		private int _remoteStackIndex;

		// Token: 0x040000C0 RID: 192
		private object _dynamicMethods;

		// Token: 0x040000C1 RID: 193
		internal int _HResult;

		// Token: 0x040000C2 RID: 194
		private string _source;

		// Token: 0x040000C3 RID: 195
		private IntPtr _xptrs;

		// Token: 0x040000C4 RID: 196
		private int _xcode;

		// Token: 0x02000031 RID: 49
		internal enum ExceptionMessageKind
		{
			// Token: 0x040000C6 RID: 198
			ThreadAbort = 1,
			// Token: 0x040000C7 RID: 199
			ThreadInterrupted,
			// Token: 0x040000C8 RID: 200
			OutOfMemory
		}
	}
}
