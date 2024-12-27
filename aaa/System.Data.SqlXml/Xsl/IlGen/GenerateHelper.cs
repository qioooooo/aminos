using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000022 RID: 34
	internal class GenerateHelper
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000BC1B File Offset: 0x0000AC1B
		public GenerateHelper(XmlILModule module, bool isDebug)
		{
			this.isDebug = isDebug;
			this.module = module;
			this.staticData = new StaticDataManager();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000BC3C File Offset: 0x0000AC3C
		public void MethodBegin(MethodBase methInfo, ISourceLineInfo sourceInfo, bool initWriters)
		{
			this.methInfo = methInfo;
			this.ilgen = XmlILModule.DefineMethodBody(methInfo);
			this.lastSourceInfo = null;
			if (this.isDebug)
			{
				this.DebugStartScope();
				if (sourceInfo != null)
				{
					this.MarkSequencePoint(sourceInfo);
					this.Emit(OpCodes.Nop);
				}
			}
			this.initWriters = false;
			if (initWriters)
			{
				this.EnsureWriter();
				this.LoadQueryRuntime();
				this.Call(XmlILMethods.GetOutput);
				this.Emit(OpCodes.Stloc, this.locXOut);
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000BCB8 File Offset: 0x0000ACB8
		public void MethodEnd()
		{
			this.Emit(OpCodes.Ret);
			if (this.isDebug)
			{
				this.DebugEndScope();
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000BCD3 File Offset: 0x0000ACD3
		public void CallSyncToNavigator()
		{
			if (this.methSyncToNav == null)
			{
				this.methSyncToNav = this.module.FindMethod("SyncToNavigator");
			}
			this.Call(this.methSyncToNav);
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000BCFF File Offset: 0x0000ACFF
		public StaticDataManager StaticData
		{
			get
			{
				return this.staticData;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000BD08 File Offset: 0x0000AD08
		public void LoadInteger(int intVal)
		{
			if (intVal >= -1 && intVal < 9)
			{
				OpCode opCode;
				switch (intVal)
				{
				case -1:
					opCode = OpCodes.Ldc_I4_M1;
					break;
				case 0:
					opCode = OpCodes.Ldc_I4_0;
					break;
				case 1:
					opCode = OpCodes.Ldc_I4_1;
					break;
				case 2:
					opCode = OpCodes.Ldc_I4_2;
					break;
				case 3:
					opCode = OpCodes.Ldc_I4_3;
					break;
				case 4:
					opCode = OpCodes.Ldc_I4_4;
					break;
				case 5:
					opCode = OpCodes.Ldc_I4_5;
					break;
				case 6:
					opCode = OpCodes.Ldc_I4_6;
					break;
				case 7:
					opCode = OpCodes.Ldc_I4_7;
					break;
				case 8:
					opCode = OpCodes.Ldc_I4_8;
					break;
				default:
					return;
				}
				this.Emit(opCode);
				return;
			}
			if (intVal >= -128 && intVal <= 127)
			{
				this.Emit(OpCodes.Ldc_I4_S, (sbyte)intVal);
				return;
			}
			this.Emit(OpCodes.Ldc_I4, intVal);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000BDD1 File Offset: 0x0000ADD1
		public void LoadBoolean(bool boolVal)
		{
			this.Emit(boolVal ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000BDE8 File Offset: 0x0000ADE8
		public void LoadType(Type clrTyp)
		{
			this.Emit(OpCodes.Ldtoken, clrTyp);
			this.Call(XmlILMethods.GetTypeFromHandle);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000BE04 File Offset: 0x0000AE04
		public LocalBuilder DeclareLocal(string name, Type type)
		{
			return this.ilgen.DeclareLocal(type);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000BE1F File Offset: 0x0000AE1F
		public void LoadQueryRuntime()
		{
			this.Emit(OpCodes.Ldarg_0);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000BE2C File Offset: 0x0000AE2C
		public void LoadQueryContext()
		{
			this.Emit(OpCodes.Ldarg_0);
			this.Call(XmlILMethods.Context);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000BE44 File Offset: 0x0000AE44
		public void LoadXsltLibrary()
		{
			this.Emit(OpCodes.Ldarg_0);
			this.Call(XmlILMethods.XsltLib);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000BE5C File Offset: 0x0000AE5C
		public void LoadQueryOutput()
		{
			this.Emit(OpCodes.Ldloc, this.locXOut);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000BE70 File Offset: 0x0000AE70
		public void LoadParameter(int paramPos)
		{
			switch (paramPos)
			{
			case 0:
				this.Emit(OpCodes.Ldarg_0);
				return;
			case 1:
				this.Emit(OpCodes.Ldarg_1);
				return;
			case 2:
				this.Emit(OpCodes.Ldarg_2);
				return;
			case 3:
				this.Emit(OpCodes.Ldarg_3);
				return;
			default:
				if (paramPos <= 255)
				{
					this.Emit(OpCodes.Ldarg_S, (byte)paramPos);
					return;
				}
				if (paramPos <= 65535)
				{
					this.Emit(OpCodes.Ldarg, paramPos);
					return;
				}
				throw new XslTransformException("XmlIl_TooManyParameters");
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000BEFC File Offset: 0x0000AEFC
		public void SetParameter(object paramId)
		{
			int num = (int)paramId;
			if (num <= 255)
			{
				this.Emit(OpCodes.Starg_S, (byte)num);
				return;
			}
			if (num <= 65535)
			{
				this.Emit(OpCodes.Starg, num);
				return;
			}
			throw new XslTransformException("XmlIl_TooManyParameters");
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000BF45 File Offset: 0x0000AF45
		public void BranchAndMark(Label lblBranch, Label lblMark)
		{
			if (!lblBranch.Equals(lblMark))
			{
				this.EmitUnconditionalBranch(OpCodes.Br, lblBranch);
			}
			this.MarkLabel(lblMark);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000BF64 File Offset: 0x0000AF64
		public void TestAndBranch(int i4, Label lblBranch, OpCode opcodeBranch)
		{
			if (i4 == 0)
			{
				if (opcodeBranch.Value == OpCodes.Beq.Value)
				{
					opcodeBranch = OpCodes.Brfalse;
					goto IL_008A;
				}
				if (opcodeBranch.Value == OpCodes.Beq_S.Value)
				{
					opcodeBranch = OpCodes.Brfalse_S;
					goto IL_008A;
				}
				if (opcodeBranch.Value == OpCodes.Bne_Un.Value)
				{
					opcodeBranch = OpCodes.Brtrue;
					goto IL_008A;
				}
				if (opcodeBranch.Value == OpCodes.Bne_Un_S.Value)
				{
					opcodeBranch = OpCodes.Brtrue_S;
					goto IL_008A;
				}
			}
			this.LoadInteger(i4);
			IL_008A:
			this.Emit(opcodeBranch, lblBranch);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000C004 File Offset: 0x0000B004
		public void ConvBranchToBool(Label lblBranch, bool isTrueBranch)
		{
			Label label = this.DefineLabel();
			this.Emit(isTrueBranch ? OpCodes.Ldc_I4_0 : OpCodes.Ldc_I4_1);
			this.EmitUnconditionalBranch(OpCodes.Br_S, label);
			this.MarkLabel(lblBranch);
			this.Emit(isTrueBranch ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
			this.MarkLabel(label);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000C05C File Offset: 0x0000B05C
		public void TailCall(MethodInfo meth)
		{
			this.Emit(OpCodes.Tailcall);
			this.Call(meth);
			this.Emit(OpCodes.Ret);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000C07B File Offset: 0x0000B07B
		[Conditional("DEBUG")]
		private void TraceCall(OpCode opcode, MethodInfo meth)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000C080 File Offset: 0x0000B080
		public void Call(MethodInfo meth)
		{
			OpCode opCode = ((meth.IsVirtual || meth.IsAbstract) ? OpCodes.Callvirt : OpCodes.Call);
			this.ilgen.Emit(opCode, meth);
			if (this.lastSourceInfo != null)
			{
				this.MarkSequencePoint(SourceLineInfo.NoSource);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000C0CC File Offset: 0x0000B0CC
		public void CallToken(MethodInfo meth)
		{
			MethodBuilder methodBuilder = this.methInfo as MethodBuilder;
			if (methodBuilder != null)
			{
				OpCode opCode = ((meth.IsVirtual || meth.IsAbstract) ? OpCodes.Callvirt : OpCodes.Call);
				this.ilgen.Emit(opCode, ((ModuleBuilder)methodBuilder.GetModule()).GetMethodToken(meth).Token);
				if (this.lastSourceInfo != null)
				{
					this.MarkSequencePoint(SourceLineInfo.NoSource);
					return;
				}
			}
			else
			{
				this.Call(meth);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000C145 File Offset: 0x0000B145
		public void Construct(ConstructorInfo constr)
		{
			this.Emit(OpCodes.Newobj, constr);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000C154 File Offset: 0x0000B154
		public void CallConcatStrings(int cStrings)
		{
			switch (cStrings)
			{
			case 0:
				this.Emit(OpCodes.Ldstr, "");
				return;
			case 1:
				break;
			case 2:
				this.Call(XmlILMethods.StrCat2);
				return;
			case 3:
				this.Call(XmlILMethods.StrCat3);
				return;
			case 4:
				this.Call(XmlILMethods.StrCat4);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000C1B4 File Offset: 0x0000B1B4
		public void TreatAs(Type clrTypeSrc, Type clrTypeDst)
		{
			if (clrTypeSrc == clrTypeDst)
			{
				return;
			}
			if (clrTypeSrc.IsValueType)
			{
				this.Emit(OpCodes.Box, clrTypeSrc);
				return;
			}
			if (clrTypeDst.IsValueType)
			{
				this.Emit(OpCodes.Unbox, clrTypeDst);
				this.Emit(OpCodes.Ldobj, clrTypeDst);
				return;
			}
			if (clrTypeDst != typeof(object))
			{
				this.Emit(OpCodes.Castclass, clrTypeDst);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000C218 File Offset: 0x0000B218
		public void ConstructLiteralDecimal(decimal dec)
		{
			if (dec >= -2147483648m && dec <= 2147483647m && decimal.Truncate(dec) == dec)
			{
				this.LoadInteger((int)dec);
				this.Construct(XmlILConstructors.DecFromInt32);
				return;
			}
			int[] bits = decimal.GetBits(dec);
			this.LoadInteger(bits[0]);
			this.LoadInteger(bits[1]);
			this.LoadInteger(bits[2]);
			this.LoadBoolean(bits[3] < 0);
			this.LoadInteger(bits[3] >> 16);
			this.Construct(XmlILConstructors.DecFromParts);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000C2B4 File Offset: 0x0000B2B4
		public void ConstructLiteralQName(string localName, string namespaceName)
		{
			this.Emit(OpCodes.Ldstr, localName);
			this.Emit(OpCodes.Ldstr, namespaceName);
			this.Construct(XmlILConstructors.QName);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000C2DC File Offset: 0x0000B2DC
		public void CallArithmeticOp(QilNodeType opType, XmlTypeCode code)
		{
			MethodInfo methodInfo = null;
			switch (code)
			{
			case XmlTypeCode.Decimal:
				switch (opType)
				{
				case QilNodeType.Negate:
					methodInfo = XmlILMethods.DecNeg;
					break;
				case QilNodeType.Add:
					methodInfo = XmlILMethods.DecAdd;
					break;
				case QilNodeType.Subtract:
					methodInfo = XmlILMethods.DecSub;
					break;
				case QilNodeType.Multiply:
					methodInfo = XmlILMethods.DecMul;
					break;
				case QilNodeType.Divide:
					methodInfo = XmlILMethods.DecDiv;
					break;
				case QilNodeType.Modulo:
					methodInfo = XmlILMethods.DecRem;
					break;
				}
				this.Call(methodInfo);
				return;
			case XmlTypeCode.Float:
			case XmlTypeCode.Double:
				break;
			default:
				if (code != XmlTypeCode.Integer && code != XmlTypeCode.Int)
				{
					return;
				}
				break;
			}
			switch (opType)
			{
			case QilNodeType.Negate:
				this.Emit(OpCodes.Neg);
				return;
			case QilNodeType.Add:
				this.Emit(OpCodes.Add);
				return;
			case QilNodeType.Subtract:
				this.Emit(OpCodes.Sub);
				return;
			case QilNodeType.Multiply:
				this.Emit(OpCodes.Mul);
				return;
			case QilNodeType.Divide:
				this.Emit(OpCodes.Div);
				return;
			case QilNodeType.Modulo:
				this.Emit(OpCodes.Rem);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000C3D4 File Offset: 0x0000B3D4
		public void CallCompareEquals(XmlTypeCode code)
		{
			MethodInfo methodInfo = null;
			switch (code)
			{
			case XmlTypeCode.String:
				methodInfo = XmlILMethods.StrEq;
				break;
			case XmlTypeCode.Boolean:
				break;
			case XmlTypeCode.Decimal:
				methodInfo = XmlILMethods.DecEq;
				break;
			default:
				if (code == XmlTypeCode.QName)
				{
					methodInfo = XmlILMethods.QNameEq;
				}
				break;
			}
			this.Call(methodInfo);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000C420 File Offset: 0x0000B420
		public void CallCompare(XmlTypeCode code)
		{
			MethodInfo methodInfo = null;
			switch (code)
			{
			case XmlTypeCode.String:
				methodInfo = XmlILMethods.StrCmp;
				break;
			case XmlTypeCode.Decimal:
				methodInfo = XmlILMethods.DecCmp;
				break;
			}
			this.Call(methodInfo);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000C45D File Offset: 0x0000B45D
		public void CallStartRtfConstruction(string baseUri)
		{
			this.EnsureWriter();
			this.LoadQueryRuntime();
			this.Emit(OpCodes.Ldstr, baseUri);
			this.Emit(OpCodes.Ldloca, this.locXOut);
			this.Call(XmlILMethods.StartRtfConstr);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000C493 File Offset: 0x0000B493
		public void CallEndRtfConstruction()
		{
			this.LoadQueryRuntime();
			this.Emit(OpCodes.Ldloca, this.locXOut);
			this.Call(XmlILMethods.EndRtfConstr);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000C4B7 File Offset: 0x0000B4B7
		public void CallStartSequenceConstruction()
		{
			this.EnsureWriter();
			this.LoadQueryRuntime();
			this.Emit(OpCodes.Ldloca, this.locXOut);
			this.Call(XmlILMethods.StartSeqConstr);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000C4E1 File Offset: 0x0000B4E1
		public void CallEndSequenceConstruction()
		{
			this.LoadQueryRuntime();
			this.Emit(OpCodes.Ldloca, this.locXOut);
			this.Call(XmlILMethods.EndSeqConstr);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000C505 File Offset: 0x0000B505
		public void CallGetEarlyBoundObject(int idxObj, Type clrType)
		{
			this.LoadQueryRuntime();
			this.LoadInteger(idxObj);
			this.Call(XmlILMethods.GetEarly);
			this.TreatAs(typeof(object), clrType);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000C530 File Offset: 0x0000B530
		public void CallGetAtomizedName(int idxName)
		{
			this.LoadQueryRuntime();
			this.LoadInteger(idxName);
			this.Call(XmlILMethods.GetAtomizedName);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000C54A File Offset: 0x0000B54A
		public void CallGetNameFilter(int idxFilter)
		{
			this.LoadQueryRuntime();
			this.LoadInteger(idxFilter);
			this.Call(XmlILMethods.GetNameFilter);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000C564 File Offset: 0x0000B564
		public void CallGetTypeFilter(XPathNodeType nodeType)
		{
			this.LoadQueryRuntime();
			this.LoadInteger((int)nodeType);
			this.Call(XmlILMethods.GetTypeFilter);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000C57E File Offset: 0x0000B57E
		public void CallParseTagName(GenerateNameType nameType)
		{
			if (nameType == GenerateNameType.TagNameAndMappings)
			{
				this.Call(XmlILMethods.TagAndMappings);
				return;
			}
			this.Call(XmlILMethods.TagAndNamespace);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000C59B File Offset: 0x0000B59B
		public void CallGetGlobalValue(int idxValue, Type clrType)
		{
			this.LoadQueryRuntime();
			this.LoadInteger(idxValue);
			this.Call(XmlILMethods.GetGlobalValue);
			this.TreatAs(typeof(object), clrType);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000C5C6 File Offset: 0x0000B5C6
		public void CallSetGlobalValue(Type clrType)
		{
			this.TreatAs(clrType, typeof(object));
			this.Call(XmlILMethods.SetGlobalValue);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000C5E4 File Offset: 0x0000B5E4
		public void CallGetCollation(int idxName)
		{
			this.LoadQueryRuntime();
			this.LoadInteger(idxName);
			this.Call(XmlILMethods.GetCollation);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000C5FE File Offset: 0x0000B5FE
		private void EnsureWriter()
		{
			if (!this.initWriters)
			{
				this.locXOut = this.DeclareLocal("$$$xwrtChk", typeof(XmlQueryOutput));
				this.initWriters = true;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000C62A File Offset: 0x0000B62A
		public void CallGetParameter(string localName, string namespaceUri)
		{
			this.LoadQueryContext();
			this.Emit(OpCodes.Ldstr, localName);
			this.Emit(OpCodes.Ldstr, namespaceUri);
			this.Call(XmlILMethods.GetParam);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000C655 File Offset: 0x0000B655
		public void CallStartTree(XPathNodeType rootType)
		{
			this.LoadQueryOutput();
			this.LoadInteger((int)rootType);
			this.Call(XmlILMethods.StartTree);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000C66F File Offset: 0x0000B66F
		public void CallEndTree()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.EndTree);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000C682 File Offset: 0x0000B682
		public void CallWriteStartRoot()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.StartRoot);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000C695 File Offset: 0x0000B695
		public void CallWriteEndRoot()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.EndRoot);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000C6A8 File Offset: 0x0000B6A8
		public void CallWriteStartElement(GenerateNameType nameType, bool callChk)
		{
			MethodInfo methodInfo = null;
			if (callChk)
			{
				switch (nameType)
				{
				case GenerateNameType.LiteralLocalName:
					methodInfo = XmlILMethods.StartElemLocName;
					break;
				case GenerateNameType.LiteralName:
					methodInfo = XmlILMethods.StartElemLitName;
					break;
				case GenerateNameType.CopiedName:
					methodInfo = XmlILMethods.StartElemCopyName;
					break;
				case GenerateNameType.TagNameAndMappings:
					methodInfo = XmlILMethods.StartElemMapName;
					break;
				case GenerateNameType.TagNameAndNamespace:
					methodInfo = XmlILMethods.StartElemNmspName;
					break;
				case GenerateNameType.QName:
					methodInfo = XmlILMethods.StartElemQName;
					break;
				}
			}
			else
			{
				switch (nameType)
				{
				case GenerateNameType.LiteralLocalName:
					methodInfo = XmlILMethods.StartElemLocNameUn;
					break;
				case GenerateNameType.LiteralName:
					methodInfo = XmlILMethods.StartElemLitNameUn;
					break;
				}
			}
			this.Call(methodInfo);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000C734 File Offset: 0x0000B734
		public void CallWriteEndElement(GenerateNameType nameType, bool callChk)
		{
			MethodInfo methodInfo = null;
			if (callChk)
			{
				methodInfo = XmlILMethods.EndElemStackName;
			}
			else
			{
				switch (nameType)
				{
				case GenerateNameType.LiteralLocalName:
					methodInfo = XmlILMethods.EndElemLocNameUn;
					break;
				case GenerateNameType.LiteralName:
					methodInfo = XmlILMethods.EndElemLitNameUn;
					break;
				}
			}
			this.Call(methodInfo);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000C775 File Offset: 0x0000B775
		public void CallStartElementContent()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.StartContentUn);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000C788 File Offset: 0x0000B788
		public void CallWriteStartAttribute(GenerateNameType nameType, bool callChk)
		{
			MethodInfo methodInfo = null;
			if (callChk)
			{
				switch (nameType)
				{
				case GenerateNameType.LiteralLocalName:
					methodInfo = XmlILMethods.StartAttrLocName;
					break;
				case GenerateNameType.LiteralName:
					methodInfo = XmlILMethods.StartAttrLitName;
					break;
				case GenerateNameType.CopiedName:
					methodInfo = XmlILMethods.StartAttrCopyName;
					break;
				case GenerateNameType.TagNameAndMappings:
					methodInfo = XmlILMethods.StartAttrMapName;
					break;
				case GenerateNameType.TagNameAndNamespace:
					methodInfo = XmlILMethods.StartAttrNmspName;
					break;
				case GenerateNameType.QName:
					methodInfo = XmlILMethods.StartAttrQName;
					break;
				}
			}
			else
			{
				switch (nameType)
				{
				case GenerateNameType.LiteralLocalName:
					methodInfo = XmlILMethods.StartAttrLocNameUn;
					break;
				case GenerateNameType.LiteralName:
					methodInfo = XmlILMethods.StartAttrLitNameUn;
					break;
				}
			}
			this.Call(methodInfo);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000C813 File Offset: 0x0000B813
		public void CallWriteEndAttribute(bool callChk)
		{
			this.LoadQueryOutput();
			if (callChk)
			{
				this.Call(XmlILMethods.EndAttr);
				return;
			}
			this.Call(XmlILMethods.EndAttrUn);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000C835 File Offset: 0x0000B835
		public void CallWriteNamespaceDecl(bool callChk)
		{
			if (callChk)
			{
				this.Call(XmlILMethods.NamespaceDecl);
				return;
			}
			this.Call(XmlILMethods.NamespaceDeclUn);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000C851 File Offset: 0x0000B851
		public void CallWriteString(bool disableOutputEscaping, bool callChk)
		{
			if (callChk)
			{
				if (disableOutputEscaping)
				{
					this.Call(XmlILMethods.NoEntText);
					return;
				}
				this.Call(XmlILMethods.Text);
				return;
			}
			else
			{
				if (disableOutputEscaping)
				{
					this.Call(XmlILMethods.NoEntTextUn);
					return;
				}
				this.Call(XmlILMethods.TextUn);
				return;
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000C88B File Offset: 0x0000B88B
		public void CallWriteStartPI()
		{
			this.Call(XmlILMethods.StartPI);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000C898 File Offset: 0x0000B898
		public void CallWriteEndPI()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.EndPI);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000C8AB File Offset: 0x0000B8AB
		public void CallWriteStartComment()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.StartComment);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000C8BE File Offset: 0x0000B8BE
		public void CallWriteEndComment()
		{
			this.LoadQueryOutput();
			this.Call(XmlILMethods.EndComment);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000C8D4 File Offset: 0x0000B8D4
		public void CallCacheCount(Type itemStorageType)
		{
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[itemStorageType];
			this.Call(xmlILStorageMethods.IListCount);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000C8F9 File Offset: 0x0000B8F9
		public void CallCacheItem(Type itemStorageType)
		{
			this.Call(XmlILMethods.StorageMethods[itemStorageType].IListItem);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000C914 File Offset: 0x0000B914
		public void CallValueAs(Type clrType)
		{
			MethodInfo valueAs = XmlILMethods.StorageMethods[clrType].ValueAs;
			if (valueAs == null)
			{
				this.LoadType(clrType);
				this.Emit(OpCodes.Ldnull);
				this.Call(XmlILMethods.ValueAsAny);
				this.TreatAs(typeof(object), clrType);
				return;
			}
			this.Call(valueAs);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000C96C File Offset: 0x0000B96C
		public void AddSortKey(XmlQueryType keyType)
		{
			MethodInfo methodInfo = null;
			if (keyType == null)
			{
				methodInfo = XmlILMethods.SortKeyEmpty;
			}
			else
			{
				XmlTypeCode typeCode = keyType.TypeCode;
				if (typeCode <= XmlTypeCode.DateTime)
				{
					if (typeCode != XmlTypeCode.None)
					{
						switch (typeCode)
						{
						case XmlTypeCode.AnyAtomicType:
							return;
						case XmlTypeCode.String:
							methodInfo = XmlILMethods.SortKeyString;
							break;
						case XmlTypeCode.Boolean:
							methodInfo = XmlILMethods.SortKeyInt;
							break;
						case XmlTypeCode.Decimal:
							methodInfo = XmlILMethods.SortKeyDecimal;
							break;
						case XmlTypeCode.Double:
							methodInfo = XmlILMethods.SortKeyDouble;
							break;
						case XmlTypeCode.DateTime:
							methodInfo = XmlILMethods.SortKeyDateTime;
							break;
						}
					}
					else
					{
						this.Emit(OpCodes.Pop);
						methodInfo = XmlILMethods.SortKeyEmpty;
					}
				}
				else if (typeCode != XmlTypeCode.Integer)
				{
					if (typeCode == XmlTypeCode.Int)
					{
						methodInfo = XmlILMethods.SortKeyInt;
					}
				}
				else
				{
					methodInfo = XmlILMethods.SortKeyInteger;
				}
			}
			this.Call(methodInfo);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000CA2D File Offset: 0x0000BA2D
		public void DebugStartScope()
		{
			this.ilgen.BeginScope();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000CA3A File Offset: 0x0000BA3A
		public void DebugEndScope()
		{
			this.ilgen.EndScope();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000CA47 File Offset: 0x0000BA47
		public void DebugSequencePoint(ISourceLineInfo sourceInfo)
		{
			this.Emit(OpCodes.Nop);
			this.MarkSequencePoint(sourceInfo);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000CA5C File Offset: 0x0000BA5C
		private string GetFileName(ISourceLineInfo sourceInfo)
		{
			string uri = sourceInfo.Uri;
			if (uri == this.lastUriString)
			{
				return this.lastFileName;
			}
			this.lastUriString = uri;
			this.lastFileName = SourceLineInfo.GetFileName(uri);
			return this.lastFileName;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000CA9C File Offset: 0x0000BA9C
		private void MarkSequencePoint(ISourceLineInfo sourceInfo)
		{
			if (sourceInfo.IsNoSource && this.lastSourceInfo != null && this.lastSourceInfo.IsNoSource)
			{
				return;
			}
			string fileName = this.GetFileName(sourceInfo);
			ISymbolDocumentWriter symbolDocumentWriter = this.module.AddSourceDocument(fileName);
			this.ilgen.MarkSequencePoint(symbolDocumentWriter, sourceInfo.StartLine, sourceInfo.StartPos, sourceInfo.EndLine, sourceInfo.EndPos);
			this.lastSourceInfo = sourceInfo;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000CB08 File Offset: 0x0000BB08
		public Label DefineLabel()
		{
			return this.ilgen.DefineLabel();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000CB22 File Offset: 0x0000BB22
		public void MarkLabel(Label lbl)
		{
			if (this.lastSourceInfo != null && !this.lastSourceInfo.IsNoSource)
			{
				this.DebugSequencePoint(SourceLineInfo.NoSource);
			}
			this.ilgen.MarkLabel(lbl);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000CB50 File Offset: 0x0000BB50
		public void Emit(OpCode opcode)
		{
			this.ilgen.Emit(opcode);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000CB5E File Offset: 0x0000BB5E
		public void Emit(OpCode opcode, byte byteVal)
		{
			this.ilgen.Emit(opcode, byteVal);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000CB6D File Offset: 0x0000BB6D
		public void Emit(OpCode opcode, ConstructorInfo constrInfo)
		{
			this.ilgen.Emit(opcode, constrInfo);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000CB7C File Offset: 0x0000BB7C
		public void Emit(OpCode opcode, double dblVal)
		{
			this.ilgen.Emit(opcode, dblVal);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000CB8B File Offset: 0x0000BB8B
		public void Emit(OpCode opcode, float fltVal)
		{
			this.ilgen.Emit(opcode, fltVal);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000CB9A File Offset: 0x0000BB9A
		public void Emit(OpCode opcode, FieldInfo fldInfo)
		{
			this.ilgen.Emit(opcode, fldInfo);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000CBA9 File Offset: 0x0000BBA9
		public void Emit(OpCode opcode, short shrtVal)
		{
			this.ilgen.Emit(opcode, shrtVal);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000CBB8 File Offset: 0x0000BBB8
		public void Emit(OpCode opcode, int intVal)
		{
			this.ilgen.Emit(opcode, intVal);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000CBC7 File Offset: 0x0000BBC7
		public void Emit(OpCode opcode, long longVal)
		{
			this.ilgen.Emit(opcode, longVal);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000CBD6 File Offset: 0x0000BBD6
		public void Emit(OpCode opcode, Label lblVal)
		{
			this.ilgen.Emit(opcode, lblVal);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000CBE5 File Offset: 0x0000BBE5
		public void Emit(OpCode opcode, Label[] arrLabels)
		{
			this.ilgen.Emit(opcode, arrLabels);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000CBF4 File Offset: 0x0000BBF4
		public void Emit(OpCode opcode, LocalBuilder locBldr)
		{
			this.ilgen.Emit(opcode, locBldr);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000CC03 File Offset: 0x0000BC03
		public void Emit(OpCode opcode, MethodInfo methInfo)
		{
			this.ilgen.Emit(opcode, methInfo);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000CC12 File Offset: 0x0000BC12
		public void Emit(OpCode opcode, sbyte sbyteVal)
		{
			this.ilgen.Emit(opcode, sbyteVal);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000CC21 File Offset: 0x0000BC21
		public void Emit(OpCode opcode, string strVal)
		{
			this.ilgen.Emit(opcode, strVal);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000CC30 File Offset: 0x0000BC30
		public void Emit(OpCode opcode, Type typVal)
		{
			this.ilgen.Emit(opcode, typVal);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000CC40 File Offset: 0x0000BC40
		public void EmitUnconditionalBranch(OpCode opcode, Label lblTarget)
		{
			if (!opcode.Equals(OpCodes.Br) && !opcode.Equals(OpCodes.Br_S))
			{
				this.Emit(OpCodes.Ldc_I4_1);
			}
			this.ilgen.Emit(opcode, lblTarget);
			if (this.lastSourceInfo != null && (opcode.Equals(OpCodes.Br) || opcode.Equals(OpCodes.Br_S)))
			{
				this.MarkSequencePoint(SourceLineInfo.NoSource);
			}
		}

		// Token: 0x0400023F RID: 575
		private MethodBase methInfo;

		// Token: 0x04000240 RID: 576
		private ILGenerator ilgen;

		// Token: 0x04000241 RID: 577
		private LocalBuilder locXOut;

		// Token: 0x04000242 RID: 578
		private XmlILModule module;

		// Token: 0x04000243 RID: 579
		private bool isDebug;

		// Token: 0x04000244 RID: 580
		private bool initWriters;

		// Token: 0x04000245 RID: 581
		private StaticDataManager staticData;

		// Token: 0x04000246 RID: 582
		private ISourceLineInfo lastSourceInfo;

		// Token: 0x04000247 RID: 583
		private MethodInfo methSyncToNav;

		// Token: 0x04000248 RID: 584
		private string lastUriString;

		// Token: 0x04000249 RID: 585
		private string lastFileName;
	}
}
