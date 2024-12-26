using System;
using System.Configuration.Assemblies;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000048 RID: 72
	internal sealed class CompilerGlobals
	{
		// Token: 0x060002ED RID: 749 RVA: 0x00015ACC File Offset: 0x00014ACC
		internal CompilerGlobals(VsaEngine engine, string assemName, string assemblyFileName, PEFileKinds PEFileKind, bool save, bool run, bool debugOn, bool isCLSCompliant, Version version, Globals globals)
		{
			string text = null;
			string text2 = null;
			if (assemblyFileName != null)
			{
				try
				{
					text2 = Path.GetDirectoryName(Path.GetFullPath(assemblyFileName));
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.AssemblyNameInvalid, assemblyFileName, ex);
				}
				catch
				{
					throw new JScriptException(JSError.NonClsException);
				}
				text = Path.GetFileName(assemblyFileName);
				if (assemName == null || string.Empty == assemName)
				{
					assemName = Path.GetFileName(assemblyFileName);
					if (Path.HasExtension(assemName))
					{
						assemName = assemName.Substring(0, assemName.Length - Path.GetExtension(assemName).Length);
					}
				}
			}
			if (assemName == null || assemName == string.Empty)
			{
				assemName = "JScriptAssembly";
			}
			if (text == null)
			{
				if (PEFileKind == PEFileKinds.Dll)
				{
					text = "JScriptModule.dll";
				}
				else
				{
					text = "JScriptModule.exe";
				}
			}
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.CodeBase = assemblyFileName;
			if (globals.assemblyCulture != null)
			{
				assemblyName.CultureInfo = globals.assemblyCulture;
			}
			assemblyName.Flags = AssemblyNameFlags.None;
			if ((globals.assemblyFlags & AssemblyFlags.PublicKey) != AssemblyFlags.SideBySideCompatible)
			{
				assemblyName.Flags = AssemblyNameFlags.PublicKey;
			}
			AssemblyFlags assemblyFlags = globals.assemblyFlags & AssemblyFlags.CompatibilityMask;
			if (assemblyFlags != AssemblyFlags.NonSideBySideAppDomain)
			{
				if (assemblyFlags != AssemblyFlags.NonSideBySideProcess)
				{
					if (assemblyFlags != AssemblyFlags.NonSideBySideMachine)
					{
						assemblyName.VersionCompatibility = (AssemblyVersionCompatibility)0;
					}
					else
					{
						assemblyName.VersionCompatibility = AssemblyVersionCompatibility.SameMachine;
					}
				}
				else
				{
					assemblyName.VersionCompatibility = AssemblyVersionCompatibility.SameProcess;
				}
			}
			else
			{
				assemblyName.VersionCompatibility = AssemblyVersionCompatibility.SameDomain;
			}
			assemblyName.HashAlgorithm = globals.assemblyHashAlgorithm;
			if (globals.assemblyKeyFileName != null)
			{
				try
				{
					using (FileStream fileStream = new FileStream(globals.assemblyKeyFileName, FileMode.Open, FileAccess.Read))
					{
						StrongNameKeyPair strongNameKeyPair = new StrongNameKeyPair(fileStream);
						if (globals.assemblyDelaySign)
						{
							if (fileStream.Length == 160L)
							{
								byte[] array = new byte[160];
								fileStream.Seek(0L, SeekOrigin.Begin);
								fileStream.Read(array, 0, 160);
								assemblyName.SetPublicKey(array);
							}
							else
							{
								assemblyName.SetPublicKey(strongNameKeyPair.PublicKey);
							}
						}
						else
						{
							byte[] publicKey = strongNameKeyPair.PublicKey;
							assemblyName.KeyPair = strongNameKeyPair;
						}
					}
					goto IL_025A;
				}
				catch
				{
					globals.assemblyKeyFileNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyFileName);
					goto IL_025A;
				}
			}
			if (globals.assemblyKeyName != null)
			{
				try
				{
					StrongNameKeyPair strongNameKeyPair2 = new StrongNameKeyPair(globals.assemblyKeyName);
					byte[] publicKey2 = strongNameKeyPair2.PublicKey;
					assemblyName.KeyPair = strongNameKeyPair2;
				}
				catch
				{
					globals.assemblyKeyNameContext.HandleError(JSError.InvalidAssemblyKeyFile, globals.assemblyKeyName);
				}
			}
			IL_025A:
			assemblyName.Name = assemName;
			if (version != null)
			{
				assemblyName.Version = version;
			}
			else if (globals.assemblyVersion != null)
			{
				assemblyName.Version = globals.assemblyVersion;
			}
			AssemblyBuilderAccess assemblyBuilderAccess = (save ? (run ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Save) : AssemblyBuilderAccess.Run);
			if (engine.ReferenceLoaderAPI == LoaderAPI.ReflectionOnlyLoadFrom)
			{
				assemblyBuilderAccess = AssemblyBuilderAccess.ReflectionOnly;
			}
			if (globals.engine.genStartupClass)
			{
				this.assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, assemblyBuilderAccess, text2, globals.engine.Evidence);
			}
			else
			{
				this.assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, assemblyBuilderAccess, text2);
			}
			if (save)
			{
				this.module = this.assemblyBuilder.DefineDynamicModule("JScript Module", text, debugOn);
			}
			else
			{
				this.module = this.assemblyBuilder.DefineDynamicModule("JScript Module", debugOn);
			}
			if (isCLSCompliant)
			{
				this.module.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { isCLSCompliant }));
			}
			if (debugOn)
			{
				ConstructorInfo constructor = Typeob.DebuggableAttribute.GetConstructor(new Type[]
				{
					Typeob.Boolean,
					Typeob.Boolean
				});
				this.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[]
				{
					(globals.assemblyFlags & AssemblyFlags.EnableJITcompileTracking) != AssemblyFlags.SideBySideCompatible,
					(globals.assemblyFlags & AssemblyFlags.DisableJITcompileOptimizer) != AssemblyFlags.SideBySideCompatible
				}));
			}
			this.compilationEvidence = globals.engine.Evidence;
			this.classwriter = null;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00015F0C File Offset: 0x00014F0C
		internal static MethodInfo constructArrayMethod
		{
			get
			{
				return Globals.TypeRefs.constructArrayMethod;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00015F18 File Offset: 0x00014F18
		internal static MethodInfo isMissingMethod
		{
			get
			{
				return Globals.TypeRefs.isMissingMethod;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00015F24 File Offset: 0x00014F24
		internal static ConstructorInfo bitwiseBinaryConstructor
		{
			get
			{
				return Globals.TypeRefs.bitwiseBinaryConstructor;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00015F30 File Offset: 0x00014F30
		internal static MethodInfo evaluateBitwiseBinaryMethod
		{
			get
			{
				return Globals.TypeRefs.evaluateBitwiseBinaryMethod;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00015F3C File Offset: 0x00014F3C
		internal static ConstructorInfo breakOutOfFinallyConstructor
		{
			get
			{
				return Globals.TypeRefs.breakOutOfFinallyConstructor;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00015F48 File Offset: 0x00014F48
		internal static ConstructorInfo closureConstructor
		{
			get
			{
				return Globals.TypeRefs.closureConstructor;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00015F54 File Offset: 0x00014F54
		internal static ConstructorInfo continueOutOfFinallyConstructor
		{
			get
			{
				return Globals.TypeRefs.continueOutOfFinallyConstructor;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00015F60 File Offset: 0x00014F60
		internal static MethodInfo checkIfDoubleIsIntegerMethod
		{
			get
			{
				return Globals.TypeRefs.checkIfDoubleIsIntegerMethod;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00015F6C File Offset: 0x00014F6C
		internal static MethodInfo checkIfSingleIsIntegerMethod
		{
			get
			{
				return Globals.TypeRefs.checkIfSingleIsIntegerMethod;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00015F78 File Offset: 0x00014F78
		internal static MethodInfo coerce2Method
		{
			get
			{
				return Globals.TypeRefs.coerce2Method;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00015F84 File Offset: 0x00014F84
		internal static MethodInfo coerceTMethod
		{
			get
			{
				return Globals.TypeRefs.coerceTMethod;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00015F90 File Offset: 0x00014F90
		internal static MethodInfo throwTypeMismatch
		{
			get
			{
				return Globals.TypeRefs.throwTypeMismatch;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00015F9C File Offset: 0x00014F9C
		internal static MethodInfo doubleToBooleanMethod
		{
			get
			{
				return Globals.TypeRefs.doubleToBooleanMethod;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00015FA8 File Offset: 0x00014FA8
		internal static MethodInfo toBooleanMethod
		{
			get
			{
				return Globals.TypeRefs.toBooleanMethod;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00015FB4 File Offset: 0x00014FB4
		internal static MethodInfo toForInObjectMethod
		{
			get
			{
				return Globals.TypeRefs.toForInObjectMethod;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00015FC0 File Offset: 0x00014FC0
		internal static MethodInfo toInt32Method
		{
			get
			{
				return Globals.TypeRefs.toInt32Method;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00015FCC File Offset: 0x00014FCC
		internal static MethodInfo toNativeArrayMethod
		{
			get
			{
				return Globals.TypeRefs.toNativeArrayMethod;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00015FD8 File Offset: 0x00014FD8
		internal static MethodInfo toNumberMethod
		{
			get
			{
				return Globals.TypeRefs.toNumberMethod;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00015FE4 File Offset: 0x00014FE4
		internal static MethodInfo toObjectMethod
		{
			get
			{
				return Globals.TypeRefs.toObjectMethod;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00015FF0 File Offset: 0x00014FF0
		internal static MethodInfo toObject2Method
		{
			get
			{
				return Globals.TypeRefs.toObject2Method;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00015FFC File Offset: 0x00014FFC
		internal static MethodInfo doubleToStringMethod
		{
			get
			{
				return Globals.TypeRefs.doubleToStringMethod;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00016008 File Offset: 0x00015008
		internal static MethodInfo toStringMethod
		{
			get
			{
				return Globals.TypeRefs.toStringMethod;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000304 RID: 772 RVA: 0x00016014 File Offset: 0x00015014
		internal static FieldInfo undefinedField
		{
			get
			{
				return Globals.TypeRefs.undefinedField;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00016020 File Offset: 0x00015020
		internal static ConstructorInfo equalityConstructor
		{
			get
			{
				return Globals.TypeRefs.equalityConstructor;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0001602C File Offset: 0x0001502C
		internal static MethodInfo evaluateEqualityMethod
		{
			get
			{
				return Globals.TypeRefs.evaluateEqualityMethod;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00016038 File Offset: 0x00015038
		internal static MethodInfo jScriptEqualsMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptEqualsMethod;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00016044 File Offset: 0x00015044
		internal static MethodInfo jScriptEvaluateMethod1
		{
			get
			{
				return Globals.TypeRefs.jScriptEvaluateMethod1;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00016050 File Offset: 0x00015050
		internal static MethodInfo jScriptEvaluateMethod2
		{
			get
			{
				return Globals.TypeRefs.jScriptEvaluateMethod2;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0001605C File Offset: 0x0001505C
		internal static MethodInfo jScriptGetEnumeratorMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptGetEnumeratorMethod;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00016068 File Offset: 0x00015068
		internal static MethodInfo jScriptFunctionDeclarationMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptFunctionDeclarationMethod;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00016074 File Offset: 0x00015074
		internal static MethodInfo jScriptFunctionExpressionMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptFunctionExpressionMethod;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00016080 File Offset: 0x00015080
		internal static FieldInfo contextEngineField
		{
			get
			{
				return Globals.TypeRefs.contextEngineField;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0001608C File Offset: 0x0001508C
		internal static MethodInfo fastConstructArrayLiteralMethod
		{
			get
			{
				return Globals.TypeRefs.fastConstructArrayLiteralMethod;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00016098 File Offset: 0x00015098
		internal static ConstructorInfo globalScopeConstructor
		{
			get
			{
				return Globals.TypeRefs.globalScopeConstructor;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000160A4 File Offset: 0x000150A4
		internal static MethodInfo getDefaultThisObjectMethod
		{
			get
			{
				return Globals.TypeRefs.getDefaultThisObjectMethod;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000160B0 File Offset: 0x000150B0
		internal static MethodInfo getFieldMethod
		{
			get
			{
				return Globals.TypeRefs.getFieldMethod;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000160BC File Offset: 0x000150BC
		internal static MethodInfo getGlobalScopeMethod
		{
			get
			{
				return Globals.TypeRefs.getGlobalScopeMethod;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000160C8 File Offset: 0x000150C8
		internal static MethodInfo getMemberValueMethod
		{
			get
			{
				return Globals.TypeRefs.getMemberValueMethod;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000160D4 File Offset: 0x000150D4
		internal static MethodInfo jScriptImportMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptImportMethod;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000315 RID: 789 RVA: 0x000160E0 File Offset: 0x000150E0
		internal static MethodInfo jScriptInMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptInMethod;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000160EC File Offset: 0x000150EC
		internal static MethodInfo getEngineMethod
		{
			get
			{
				return Globals.TypeRefs.getEngineMethod;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000317 RID: 791 RVA: 0x000160F8 File Offset: 0x000150F8
		internal static MethodInfo setEngineMethod
		{
			get
			{
				return Globals.TypeRefs.setEngineMethod;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00016104 File Offset: 0x00015104
		internal static MethodInfo jScriptInstanceofMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptInstanceofMethod;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00016110 File Offset: 0x00015110
		internal static ConstructorInfo scriptExceptionConstructor
		{
			get
			{
				return Globals.TypeRefs.scriptExceptionConstructor;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0001611C File Offset: 0x0001511C
		internal static ConstructorInfo jsFunctionAttributeConstructor
		{
			get
			{
				return Globals.TypeRefs.jsFunctionAttributeConstructor;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00016128 File Offset: 0x00015128
		internal static ConstructorInfo jsLocalFieldConstructor
		{
			get
			{
				return Globals.TypeRefs.jsLocalFieldConstructor;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00016134 File Offset: 0x00015134
		internal static MethodInfo setMemberValue2Method
		{
			get
			{
				return Globals.TypeRefs.setMemberValue2Method;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00016140 File Offset: 0x00015140
		internal static ConstructorInfo lateBindingConstructor2
		{
			get
			{
				return Globals.TypeRefs.lateBindingConstructor2;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0001614C File Offset: 0x0001514C
		internal static ConstructorInfo lateBindingConstructor
		{
			get
			{
				return Globals.TypeRefs.lateBindingConstructor;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00016158 File Offset: 0x00015158
		internal static FieldInfo objectField
		{
			get
			{
				return Globals.TypeRefs.objectField;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00016164 File Offset: 0x00015164
		internal static MethodInfo callMethod
		{
			get
			{
				return Globals.TypeRefs.callMethod;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00016170 File Offset: 0x00015170
		internal static MethodInfo callValueMethod
		{
			get
			{
				return Globals.TypeRefs.callValueMethod;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0001617C File Offset: 0x0001517C
		internal static MethodInfo callValue2Method
		{
			get
			{
				return Globals.TypeRefs.callValue2Method;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00016188 File Offset: 0x00015188
		internal static MethodInfo deleteMethod
		{
			get
			{
				return Globals.TypeRefs.deleteMethod;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00016194 File Offset: 0x00015194
		internal static MethodInfo deleteMemberMethod
		{
			get
			{
				return Globals.TypeRefs.deleteMemberMethod;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000161A0 File Offset: 0x000151A0
		internal static MethodInfo getNonMissingValueMethod
		{
			get
			{
				return Globals.TypeRefs.getNonMissingValueMethod;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000326 RID: 806 RVA: 0x000161AC File Offset: 0x000151AC
		internal static MethodInfo getValue2Method
		{
			get
			{
				return Globals.TypeRefs.getValue2Method;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000327 RID: 807 RVA: 0x000161B8 File Offset: 0x000151B8
		internal static MethodInfo setIndexedPropertyValueStaticMethod
		{
			get
			{
				return Globals.TypeRefs.setIndexedPropertyValueStaticMethod;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000328 RID: 808 RVA: 0x000161C4 File Offset: 0x000151C4
		internal static MethodInfo setValueMethod
		{
			get
			{
				return Globals.TypeRefs.setValueMethod;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000329 RID: 809 RVA: 0x000161D0 File Offset: 0x000151D0
		internal static FieldInfo missingField
		{
			get
			{
				return Globals.TypeRefs.missingField;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000161DC File Offset: 0x000151DC
		internal static MethodInfo getNamespaceMethod
		{
			get
			{
				return Globals.TypeRefs.getNamespaceMethod;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600032B RID: 811 RVA: 0x000161E8 File Offset: 0x000151E8
		internal static ConstructorInfo numericBinaryConstructor
		{
			get
			{
				return Globals.TypeRefs.numericBinaryConstructor;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000161F4 File Offset: 0x000151F4
		internal static MethodInfo numericbinaryDoOpMethod
		{
			get
			{
				return Globals.TypeRefs.numericbinaryDoOpMethod;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00016200 File Offset: 0x00015200
		internal static MethodInfo evaluateNumericBinaryMethod
		{
			get
			{
				return Globals.TypeRefs.evaluateNumericBinaryMethod;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001620C File Offset: 0x0001520C
		internal static ConstructorInfo numericUnaryConstructor
		{
			get
			{
				return Globals.TypeRefs.numericUnaryConstructor;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00016218 File Offset: 0x00015218
		internal static MethodInfo evaluateUnaryMethod
		{
			get
			{
				return Globals.TypeRefs.evaluateUnaryMethod;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00016224 File Offset: 0x00015224
		internal static MethodInfo constructObjectMethod
		{
			get
			{
				return Globals.TypeRefs.constructObjectMethod;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00016230 File Offset: 0x00015230
		internal static MethodInfo jScriptPackageMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptPackageMethod;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0001623C File Offset: 0x0001523C
		internal static ConstructorInfo plusConstructor
		{
			get
			{
				return Globals.TypeRefs.plusConstructor;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00016248 File Offset: 0x00015248
		internal static MethodInfo plusDoOpMethod
		{
			get
			{
				return Globals.TypeRefs.plusDoOpMethod;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00016254 File Offset: 0x00015254
		internal static MethodInfo evaluatePlusMethod
		{
			get
			{
				return Globals.TypeRefs.evaluatePlusMethod;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00016260 File Offset: 0x00015260
		internal static ConstructorInfo postOrPrefixConstructor
		{
			get
			{
				return Globals.TypeRefs.postOrPrefixConstructor;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0001626C File Offset: 0x0001526C
		internal static MethodInfo evaluatePostOrPrefixOperatorMethod
		{
			get
			{
				return Globals.TypeRefs.evaluatePostOrPrefixOperatorMethod;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00016278 File Offset: 0x00015278
		internal static ConstructorInfo referenceAttributeConstructor
		{
			get
			{
				return Globals.TypeRefs.referenceAttributeConstructor;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00016284 File Offset: 0x00015284
		internal static MethodInfo regExpConstructMethod
		{
			get
			{
				return Globals.TypeRefs.regExpConstructMethod;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00016290 File Offset: 0x00015290
		internal static ConstructorInfo relationalConstructor
		{
			get
			{
				return Globals.TypeRefs.relationalConstructor;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0001629C File Offset: 0x0001529C
		internal static MethodInfo evaluateRelationalMethod
		{
			get
			{
				return Globals.TypeRefs.evaluateRelationalMethod;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000162A8 File Offset: 0x000152A8
		internal static MethodInfo jScriptCompareMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptCompareMethod;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000162B4 File Offset: 0x000152B4
		internal static ConstructorInfo returnOutOfFinallyConstructor
		{
			get
			{
				return Globals.TypeRefs.returnOutOfFinallyConstructor;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600033D RID: 829 RVA: 0x000162C0 File Offset: 0x000152C0
		internal static MethodInfo doubleToInt64
		{
			get
			{
				return Globals.TypeRefs.doubleToInt64;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600033E RID: 830 RVA: 0x000162CC File Offset: 0x000152CC
		internal static MethodInfo uncheckedDecimalToInt64Method
		{
			get
			{
				return Globals.TypeRefs.uncheckedDecimalToInt64Method;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000162D8 File Offset: 0x000152D8
		internal static FieldInfo engineField
		{
			get
			{
				return Globals.TypeRefs.engineField;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000162E4 File Offset: 0x000152E4
		internal static MethodInfo getParentMethod
		{
			get
			{
				return Globals.TypeRefs.getParentMethod;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000341 RID: 833 RVA: 0x000162F0 File Offset: 0x000152F0
		internal static MethodInfo writeMethod
		{
			get
			{
				return Globals.TypeRefs.writeMethod;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000162FC File Offset: 0x000152FC
		internal static MethodInfo writeLineMethod
		{
			get
			{
				return Globals.TypeRefs.writeLineMethod;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00016308 File Offset: 0x00015308
		internal static ConstructorInfo hashtableCtor
		{
			get
			{
				return Globals.TypeRefs.hashtableCtor;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00016314 File Offset: 0x00015314
		internal static MethodInfo hashtableGetItem
		{
			get
			{
				return Globals.TypeRefs.hashtableGetItem;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00016320 File Offset: 0x00015320
		internal static MethodInfo hashTableGetEnumerator
		{
			get
			{
				return Globals.TypeRefs.hashTableGetEnumerator;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0001632C File Offset: 0x0001532C
		internal static MethodInfo hashtableRemove
		{
			get
			{
				return Globals.TypeRefs.hashtableRemove;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00016338 File Offset: 0x00015338
		internal static MethodInfo hashtableSetItem
		{
			get
			{
				return Globals.TypeRefs.hashtableSetItem;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00016344 File Offset: 0x00015344
		internal static FieldInfo closureInstanceField
		{
			get
			{
				return Globals.TypeRefs.closureInstanceField;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00016350 File Offset: 0x00015350
		internal static FieldInfo localVarsField
		{
			get
			{
				return Globals.TypeRefs.localVarsField;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0001635C File Offset: 0x0001535C
		internal static MethodInfo pushStackFrameForMethod
		{
			get
			{
				return Globals.TypeRefs.pushStackFrameForMethod;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00016368 File Offset: 0x00015368
		internal static MethodInfo pushStackFrameForStaticMethod
		{
			get
			{
				return Globals.TypeRefs.pushStackFrameForStaticMethod;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00016374 File Offset: 0x00015374
		internal static MethodInfo jScriptStrictEqualsMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptStrictEqualsMethod;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00016380 File Offset: 0x00015380
		internal static MethodInfo jScriptThrowMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptThrowMethod;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0001638C File Offset: 0x0001538C
		internal static MethodInfo jScriptExceptionValueMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptExceptionValueMethod;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00016398 File Offset: 0x00015398
		internal static MethodInfo jScriptTypeofMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptTypeofMethod;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000350 RID: 848 RVA: 0x000163A4 File Offset: 0x000153A4
		internal static ConstructorInfo vsaEngineConstructor
		{
			get
			{
				return Globals.TypeRefs.vsaEngineConstructor;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000351 RID: 849 RVA: 0x000163B0 File Offset: 0x000153B0
		internal static MethodInfo createVsaEngine
		{
			get
			{
				return Globals.TypeRefs.createVsaEngine;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000352 RID: 850 RVA: 0x000163BC File Offset: 0x000153BC
		internal static MethodInfo createVsaEngineWithType
		{
			get
			{
				return Globals.TypeRefs.createVsaEngineWithType;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000353 RID: 851 RVA: 0x000163C8 File Offset: 0x000153C8
		internal static MethodInfo getOriginalArrayConstructorMethod
		{
			get
			{
				return Globals.TypeRefs.getOriginalArrayConstructorMethod;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000354 RID: 852 RVA: 0x000163D4 File Offset: 0x000153D4
		internal static MethodInfo getOriginalObjectConstructorMethod
		{
			get
			{
				return Globals.TypeRefs.getOriginalObjectConstructorMethod;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000355 RID: 853 RVA: 0x000163E0 File Offset: 0x000153E0
		internal static MethodInfo getOriginalRegExpConstructorMethod
		{
			get
			{
				return Globals.TypeRefs.getOriginalRegExpConstructorMethod;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000356 RID: 854 RVA: 0x000163EC File Offset: 0x000153EC
		internal static MethodInfo popScriptObjectMethod
		{
			get
			{
				return Globals.TypeRefs.popScriptObjectMethod;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000357 RID: 855 RVA: 0x000163F8 File Offset: 0x000153F8
		internal static MethodInfo pushScriptObjectMethod
		{
			get
			{
				return Globals.TypeRefs.pushScriptObjectMethod;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00016404 File Offset: 0x00015404
		internal static MethodInfo scriptObjectStackTopMethod
		{
			get
			{
				return Globals.TypeRefs.scriptObjectStackTopMethod;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00016410 File Offset: 0x00015410
		internal static MethodInfo getLenientGlobalObjectMethod
		{
			get
			{
				return Globals.TypeRefs.getLenientGlobalObjectMethod;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0001641C File Offset: 0x0001541C
		internal static MethodInfo jScriptWithMethod
		{
			get
			{
				return Globals.TypeRefs.jScriptWithMethod;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00016428 File Offset: 0x00015428
		internal static ConstructorInfo clsCompliantAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.clsCompliantAttributeCtor;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00016434 File Offset: 0x00015434
		internal static MethodInfo getEnumeratorMethod
		{
			get
			{
				return Globals.TypeRefs.getEnumeratorMethod;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00016440 File Offset: 0x00015440
		internal static MethodInfo moveNextMethod
		{
			get
			{
				return Globals.TypeRefs.moveNextMethod;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0001644C File Offset: 0x0001544C
		internal static MethodInfo getCurrentMethod
		{
			get
			{
				return Globals.TypeRefs.getCurrentMethod;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00016458 File Offset: 0x00015458
		internal static ConstructorInfo contextStaticAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.contextStaticAttributeCtor;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00016464 File Offset: 0x00015464
		internal static MethodInfo changeTypeMethod
		{
			get
			{
				return Globals.TypeRefs.changeTypeMethod;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00016470 File Offset: 0x00015470
		internal static MethodInfo convertCharToStringMethod
		{
			get
			{
				return Globals.TypeRefs.convertCharToStringMethod;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0001647C File Offset: 0x0001547C
		internal static ConstructorInfo dateTimeConstructor
		{
			get
			{
				return Globals.TypeRefs.dateTimeConstructor;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00016488 File Offset: 0x00015488
		internal static MethodInfo dateTimeToStringMethod
		{
			get
			{
				return Globals.TypeRefs.dateTimeToStringMethod;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00016494 File Offset: 0x00015494
		internal static MethodInfo dateTimeToInt64Method
		{
			get
			{
				return Globals.TypeRefs.dateTimeToInt64Method;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000164A0 File Offset: 0x000154A0
		internal static ConstructorInfo decimalConstructor
		{
			get
			{
				return Globals.TypeRefs.decimalConstructor;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000366 RID: 870 RVA: 0x000164AC File Offset: 0x000154AC
		internal static FieldInfo decimalZeroField
		{
			get
			{
				return Globals.TypeRefs.decimalZeroField;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000164B8 File Offset: 0x000154B8
		internal static MethodInfo decimalCompare
		{
			get
			{
				return Globals.TypeRefs.decimalCompare;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000164C4 File Offset: 0x000154C4
		internal static MethodInfo doubleToDecimalMethod
		{
			get
			{
				return Globals.TypeRefs.doubleToDecimalMethod;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000164D0 File Offset: 0x000154D0
		internal static MethodInfo int32ToDecimalMethod
		{
			get
			{
				return Globals.TypeRefs.int32ToDecimalMethod;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000164DC File Offset: 0x000154DC
		internal static MethodInfo int64ToDecimalMethod
		{
			get
			{
				return Globals.TypeRefs.int64ToDecimalMethod;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000164E8 File Offset: 0x000154E8
		internal static MethodInfo uint32ToDecimalMethod
		{
			get
			{
				return Globals.TypeRefs.uint32ToDecimalMethod;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000164F4 File Offset: 0x000154F4
		internal static MethodInfo uint64ToDecimalMethod
		{
			get
			{
				return Globals.TypeRefs.uint64ToDecimalMethod;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00016500 File Offset: 0x00015500
		internal static MethodInfo decimalToDoubleMethod
		{
			get
			{
				return Globals.TypeRefs.decimalToDoubleMethod;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0001650C File Offset: 0x0001550C
		internal static MethodInfo decimalToInt32Method
		{
			get
			{
				return Globals.TypeRefs.decimalToInt32Method;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00016518 File Offset: 0x00015518
		internal static MethodInfo decimalToInt64Method
		{
			get
			{
				return Globals.TypeRefs.decimalToInt64Method;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00016524 File Offset: 0x00015524
		internal static MethodInfo decimalToStringMethod
		{
			get
			{
				return Globals.TypeRefs.decimalToStringMethod;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00016530 File Offset: 0x00015530
		internal static MethodInfo decimalToUInt32Method
		{
			get
			{
				return Globals.TypeRefs.decimalToUInt32Method;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0001653C File Offset: 0x0001553C
		internal static MethodInfo decimalToUInt64Method
		{
			get
			{
				return Globals.TypeRefs.decimalToUInt64Method;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00016548 File Offset: 0x00015548
		internal static MethodInfo debugBreak
		{
			get
			{
				return Globals.TypeRefs.debugBreak;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00016554 File Offset: 0x00015554
		internal static ConstructorInfo debuggerHiddenAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.debuggerHiddenAttributeCtor;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00016560 File Offset: 0x00015560
		internal static ConstructorInfo debuggerStepThroughAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.debuggerStepThroughAttributeCtor;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0001656C File Offset: 0x0001556C
		internal static MethodInfo int32ToStringMethod
		{
			get
			{
				return Globals.TypeRefs.int32ToStringMethod;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00016578 File Offset: 0x00015578
		internal static MethodInfo int64ToStringMethod
		{
			get
			{
				return Globals.TypeRefs.int64ToStringMethod;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00016584 File Offset: 0x00015584
		internal static MethodInfo equalsMethod
		{
			get
			{
				return Globals.TypeRefs.equalsMethod;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00016590 File Offset: 0x00015590
		internal static ConstructorInfo defaultMemberAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.defaultMemberAttributeCtor;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0001659C File Offset: 0x0001559C
		internal static MethodInfo getFieldValueMethod
		{
			get
			{
				return Globals.TypeRefs.getFieldValueMethod;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600037B RID: 891 RVA: 0x000165A8 File Offset: 0x000155A8
		internal static MethodInfo setFieldValueMethod
		{
			get
			{
				return Globals.TypeRefs.setFieldValueMethod;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600037C RID: 892 RVA: 0x000165B4 File Offset: 0x000155B4
		internal static FieldInfo systemReflectionMissingField
		{
			get
			{
				return Globals.TypeRefs.systemReflectionMissingField;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000165C0 File Offset: 0x000155C0
		internal static ConstructorInfo compilerGlobalScopeAttributeCtor
		{
			get
			{
				return Globals.TypeRefs.compilerGlobalScopeAttributeCtor;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600037E RID: 894 RVA: 0x000165CC File Offset: 0x000155CC
		internal static MethodInfo stringConcatArrMethod
		{
			get
			{
				return Globals.TypeRefs.stringConcatArrMethod;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600037F RID: 895 RVA: 0x000165D8 File Offset: 0x000155D8
		internal static MethodInfo stringConcat4Method
		{
			get
			{
				return Globals.TypeRefs.stringConcat4Method;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000380 RID: 896 RVA: 0x000165E4 File Offset: 0x000155E4
		internal static MethodInfo stringConcat3Method
		{
			get
			{
				return Globals.TypeRefs.stringConcat3Method;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000381 RID: 897 RVA: 0x000165F0 File Offset: 0x000155F0
		internal static MethodInfo stringConcat2Method
		{
			get
			{
				return Globals.TypeRefs.stringConcat2Method;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000382 RID: 898 RVA: 0x000165FC File Offset: 0x000155FC
		internal static MethodInfo stringEqualsMethod
		{
			get
			{
				return Globals.TypeRefs.stringEqualsMethod;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00016608 File Offset: 0x00015608
		internal static MethodInfo stringLengthMethod
		{
			get
			{
				return Globals.TypeRefs.stringLengthMethod;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00016614 File Offset: 0x00015614
		internal static MethodInfo getMethodMethod
		{
			get
			{
				return Globals.TypeRefs.getMethodMethod;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00016620 File Offset: 0x00015620
		internal static MethodInfo getTypeMethod
		{
			get
			{
				return Globals.TypeRefs.getTypeMethod;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001662C File Offset: 0x0001562C
		internal static MethodInfo getTypeFromHandleMethod
		{
			get
			{
				return Globals.TypeRefs.getTypeFromHandleMethod;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00016638 File Offset: 0x00015638
		internal static MethodInfo uint32ToStringMethod
		{
			get
			{
				return Globals.TypeRefs.uint32ToStringMethod;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00016644 File Offset: 0x00015644
		internal static MethodInfo uint64ToStringMethod
		{
			get
			{
				return Globals.TypeRefs.uint64ToStringMethod;
			}
		}

		// Token: 0x040001BD RID: 445
		internal Stack BreakLabelStack = new Stack();

		// Token: 0x040001BE RID: 446
		internal Stack ContinueLabelStack = new Stack();

		// Token: 0x040001BF RID: 447
		internal bool InsideProtectedRegion;

		// Token: 0x040001C0 RID: 448
		internal bool InsideFinally;

		// Token: 0x040001C1 RID: 449
		internal int FinallyStackTop;

		// Token: 0x040001C2 RID: 450
		internal ModuleBuilder module;

		// Token: 0x040001C3 RID: 451
		internal AssemblyBuilder assemblyBuilder;

		// Token: 0x040001C4 RID: 452
		internal TypeBuilder classwriter;

		// Token: 0x040001C5 RID: 453
		internal TypeBuilder globalScopeClassWriter;

		// Token: 0x040001C6 RID: 454
		internal SimpleHashtable documents = new SimpleHashtable(8U);

		// Token: 0x040001C7 RID: 455
		internal SimpleHashtable usedNames = new SimpleHashtable(32U);

		// Token: 0x040001C8 RID: 456
		internal Evidence compilationEvidence;
	}
}
