using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E6 RID: 1254
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class DllImportAttribute : Attribute
	{
		// Token: 0x06003131 RID: 12593 RVA: 0x000A90EC File Offset: 0x000A80EC
		internal static Attribute GetCustomAttribute(RuntimeMethodInfo method)
		{
			if ((method.Attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope)
			{
				return null;
			}
			MetadataImport metadataImport = method.Module.ModuleHandle.GetMetadataImport();
			string text = null;
			int metadataToken = method.MetadataToken;
			PInvokeAttributes pinvokeAttributes = PInvokeAttributes.CharSetNotSpec;
			string text2;
			metadataImport.GetPInvokeMap(metadataToken, out pinvokeAttributes, out text2, out text);
			CharSet charSet = CharSet.None;
			switch (pinvokeAttributes & PInvokeAttributes.CharSetMask)
			{
			case PInvokeAttributes.CharSetNotSpec:
				charSet = CharSet.None;
				break;
			case PInvokeAttributes.CharSetAnsi:
				charSet = CharSet.Ansi;
				break;
			case PInvokeAttributes.CharSetUnicode:
				charSet = CharSet.Unicode;
				break;
			case PInvokeAttributes.CharSetMask:
				charSet = CharSet.Auto;
				break;
			}
			CallingConvention callingConvention = CallingConvention.Cdecl;
			PInvokeAttributes pinvokeAttributes2 = pinvokeAttributes & PInvokeAttributes.CallConvMask;
			if (pinvokeAttributes2 <= PInvokeAttributes.CallConvCdecl)
			{
				if (pinvokeAttributes2 != PInvokeAttributes.CallConvWinapi)
				{
					if (pinvokeAttributes2 == PInvokeAttributes.CallConvCdecl)
					{
						callingConvention = CallingConvention.Cdecl;
					}
				}
				else
				{
					callingConvention = CallingConvention.Winapi;
				}
			}
			else if (pinvokeAttributes2 != PInvokeAttributes.CallConvStdcall)
			{
				if (pinvokeAttributes2 != PInvokeAttributes.CallConvThiscall)
				{
					if (pinvokeAttributes2 == PInvokeAttributes.CallConvFastcall)
					{
						callingConvention = CallingConvention.FastCall;
					}
				}
				else
				{
					callingConvention = CallingConvention.ThisCall;
				}
			}
			else
			{
				callingConvention = CallingConvention.StdCall;
			}
			bool flag = (pinvokeAttributes & PInvokeAttributes.NoMangle) != PInvokeAttributes.CharSetNotSpec;
			bool flag2 = (pinvokeAttributes & PInvokeAttributes.SupportsLastError) != PInvokeAttributes.CharSetNotSpec;
			bool flag3 = (pinvokeAttributes & PInvokeAttributes.BestFitMask) == PInvokeAttributes.BestFitEnabled;
			bool flag4 = (pinvokeAttributes & PInvokeAttributes.ThrowOnUnmappableCharMask) == PInvokeAttributes.ThrowOnUnmappableCharEnabled;
			bool flag5 = (method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) != MethodImplAttributes.IL;
			return new DllImportAttribute(text, text2, charSet, flag, flag2, flag5, callingConvention, flag3, flag4);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000A9234 File Offset: 0x000A8234
		internal static bool IsDefined(RuntimeMethodInfo method)
		{
			return (method.Attributes & MethodAttributes.PinvokeImpl) != MethodAttributes.PrivateScope;
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000A9248 File Offset: 0x000A8248
		internal DllImportAttribute(string dllName, string entryPoint, CharSet charSet, bool exactSpelling, bool setLastError, bool preserveSig, CallingConvention callingConvention, bool bestFitMapping, bool throwOnUnmappableChar)
		{
			this._val = dllName;
			this.EntryPoint = entryPoint;
			this.CharSet = charSet;
			this.ExactSpelling = exactSpelling;
			this.SetLastError = setLastError;
			this.PreserveSig = preserveSig;
			this.CallingConvention = callingConvention;
			this.BestFitMapping = bestFitMapping;
			this.ThrowOnUnmappableChar = throwOnUnmappableChar;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000A92A0 File Offset: 0x000A82A0
		public DllImportAttribute(string dllName)
		{
			this._val = dllName;
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x000A92AF File Offset: 0x000A82AF
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001941 RID: 6465
		internal string _val;

		// Token: 0x04001942 RID: 6466
		public string EntryPoint;

		// Token: 0x04001943 RID: 6467
		public CharSet CharSet;

		// Token: 0x04001944 RID: 6468
		public bool SetLastError;

		// Token: 0x04001945 RID: 6469
		public bool ExactSpelling;

		// Token: 0x04001946 RID: 6470
		public bool PreserveSig;

		// Token: 0x04001947 RID: 6471
		public CallingConvention CallingConvention;

		// Token: 0x04001948 RID: 6472
		public bool BestFitMapping;

		// Token: 0x04001949 RID: 6473
		public bool ThrowOnUnmappableChar;
	}
}
