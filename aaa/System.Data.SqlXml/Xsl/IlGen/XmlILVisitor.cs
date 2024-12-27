using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Schema;
using System.Xml.Utils;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200002F RID: 47
	internal class XmlILVisitor : QilVisitor
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000E8AC File Offset: 0x0000D8AC
		public void Visit(QilExpression qil, GenerateHelper helper, MethodInfo methRoot)
		{
			this.qil = qil;
			this.helper = helper;
			this.iterNested = null;
			this.indexId = 0;
			this.PrepareGlobalValues(qil.GlobalParameterList);
			this.PrepareGlobalValues(qil.GlobalVariableList);
			this.VisitGlobalValues(qil.GlobalParameterList);
			this.VisitGlobalValues(qil.GlobalVariableList);
			foreach (QilNode qilNode in qil.FunctionList)
			{
				QilFunction qilFunction = (QilFunction)qilNode;
				this.Function(qilFunction);
			}
			this.helper.MethodBegin(methRoot, null, true);
			this.StartNestedIterator(qil.Root);
			this.Visit(qil.Root);
			this.EndNestedIterator(qil.Root);
			this.helper.MethodEnd();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000E988 File Offset: 0x0000D988
		private void PrepareGlobalValues(QilList globalIterators)
		{
			foreach (QilNode qilNode in globalIterators)
			{
				QilIterator qilIterator = (QilIterator)qilNode;
				MethodInfo functionBinding = XmlILAnnotation.Write(qilIterator).FunctionBinding;
				IteratorDescriptor iteratorDescriptor = new IteratorDescriptor(this.helper);
				iteratorDescriptor.Storage = StorageDescriptor.Global(functionBinding, this.GetItemStorageType(qilIterator), !qilIterator.XmlType.IsSingleton);
				XmlILAnnotation.Write(qilIterator).CachedIteratorDescriptor = iteratorDescriptor;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000EA14 File Offset: 0x0000DA14
		private void VisitGlobalValues(QilList globalIterators)
		{
			foreach (QilNode qilNode in globalIterators)
			{
				QilIterator qilIterator = (QilIterator)qilNode;
				QilParameter qilParameter = qilIterator as QilParameter;
				MethodInfo globalLocation = XmlILAnnotation.Write(qilIterator).CachedIteratorDescriptor.Storage.GlobalLocation;
				bool flag = !qilIterator.XmlType.IsSingleton;
				int num = this.helper.StaticData.DeclareGlobalValue(qilIterator.DebugName);
				this.helper.MethodBegin(globalLocation, qilIterator.SourceLine, false);
				Label label = this.helper.DefineLabel();
				Label label2 = this.helper.DefineLabel();
				this.helper.LoadQueryRuntime();
				this.helper.LoadInteger(num);
				this.helper.Call(XmlILMethods.GlobalComputed);
				this.helper.Emit(OpCodes.Brtrue, label);
				this.StartNestedIterator(qilIterator);
				if (qilParameter != null)
				{
					LocalBuilder localBuilder = this.helper.DeclareLocal("$$$param", typeof(object));
					this.helper.CallGetParameter(qilParameter.Name.LocalName, qilParameter.Name.NamespaceUri);
					this.helper.Emit(OpCodes.Stloc, localBuilder);
					this.helper.Emit(OpCodes.Ldloc, localBuilder);
					this.helper.Emit(OpCodes.Brfalse, label2);
					this.helper.LoadQueryRuntime();
					this.helper.LoadInteger(num);
					this.helper.LoadQueryRuntime();
					this.helper.LoadInteger(this.helper.StaticData.DeclareXmlType(XmlQueryTypeFactory.ItemS));
					this.helper.Emit(OpCodes.Ldloc, localBuilder);
					this.helper.Call(XmlILMethods.ChangeTypeXsltResult);
					this.helper.CallSetGlobalValue(typeof(object));
					this.helper.EmitUnconditionalBranch(OpCodes.Br, label);
				}
				this.helper.MarkLabel(label2);
				if (qilIterator.Binding != null)
				{
					this.helper.LoadQueryRuntime();
					this.helper.LoadInteger(num);
					this.NestedVisitEnsureStack(qilIterator.Binding, this.GetItemStorageType(qilIterator), flag);
					this.helper.CallSetGlobalValue(this.GetStorageType(qilIterator));
				}
				else
				{
					this.helper.LoadQueryRuntime();
					this.helper.Emit(OpCodes.Ldstr, Res.GetString("XmlIl_UnknownParam", new string[]
					{
						qilParameter.Name.LocalName,
						qilParameter.Name.NamespaceUri
					}));
					this.helper.Call(XmlILMethods.ThrowException);
				}
				this.EndNestedIterator(qilIterator);
				this.helper.MarkLabel(label);
				this.helper.CallGetGlobalValue(num, this.GetStorageType(qilIterator));
				this.helper.MethodEnd();
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000ED20 File Offset: 0x0000DD20
		private void Function(QilFunction ndFunc)
		{
			foreach (QilNode qilNode in ndFunc.Arguments)
			{
				QilIterator qilIterator = (QilIterator)qilNode;
				IteratorDescriptor iteratorDescriptor = new IteratorDescriptor(this.helper);
				int num = XmlILAnnotation.Write(qilIterator).ArgumentPosition + 1;
				iteratorDescriptor.Storage = StorageDescriptor.Parameter(num, this.GetItemStorageType(qilIterator), !qilIterator.XmlType.IsSingleton);
				XmlILAnnotation.Write(qilIterator).CachedIteratorDescriptor = iteratorDescriptor;
			}
			MethodInfo functionBinding = XmlILAnnotation.Write(ndFunc).FunctionBinding;
			bool flag = XmlILConstructInfo.Read(ndFunc).ConstructMethod == XmlILConstructMethod.Writer;
			this.helper.MethodBegin(functionBinding, ndFunc.SourceLine, flag);
			foreach (QilNode qilNode2 in ndFunc.Arguments)
			{
				QilIterator qilIterator2 = (QilIterator)qilNode2;
				if (this.qil.IsDebug && qilIterator2.SourceLine != null)
				{
					this.helper.DebugSequencePoint(qilIterator2.SourceLine);
				}
				if (qilIterator2.Binding != null)
				{
					int num = (qilIterator2.Annotation as XmlILAnnotation).ArgumentPosition + 1;
					Label label = this.helper.DefineLabel();
					this.helper.LoadQueryRuntime();
					this.helper.LoadParameter(num);
					this.helper.LoadInteger(29);
					this.helper.Call(XmlILMethods.SeqMatchesCode);
					this.helper.Emit(OpCodes.Brfalse, label);
					this.StartNestedIterator(qilIterator2);
					this.NestedVisitEnsureStack(qilIterator2.Binding, this.GetItemStorageType(qilIterator2), !qilIterator2.XmlType.IsSingleton);
					this.EndNestedIterator(qilIterator2);
					this.helper.SetParameter(num);
					this.helper.MarkLabel(label);
				}
			}
			this.StartNestedIterator(ndFunc);
			if (flag)
			{
				this.NestedVisit(ndFunc.Definition);
			}
			else
			{
				this.NestedVisitEnsureStack(ndFunc.Definition, this.GetItemStorageType(ndFunc), !ndFunc.XmlType.IsSingleton);
			}
			this.EndNestedIterator(ndFunc);
			this.helper.MethodEnd();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000EF80 File Offset: 0x0000DF80
		protected override QilNode Visit(QilNode nd)
		{
			if (nd == null)
			{
				return null;
			}
			if (this.qil.IsDebug && nd.SourceLine != null && !(nd is QilIterator))
			{
				this.helper.DebugSequencePoint(nd.SourceLine);
			}
			switch (XmlILConstructInfo.Read(nd).ConstructMethod)
			{
			case XmlILConstructMethod.WriterThenIterator:
				this.NestedConstruction(nd);
				return nd;
			case XmlILConstructMethod.IteratorThenWriter:
				this.CopySequence(nd);
				return nd;
			}
			base.Visit(nd);
			return nd;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000EFFF File Offset: 0x0000DFFF
		protected override QilNode VisitChildren(QilNode parent)
		{
			return parent;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000F002 File Offset: 0x0000E002
		private void NestedConstruction(QilNode nd)
		{
			this.helper.CallStartSequenceConstruction();
			base.Visit(nd);
			this.helper.CallEndSequenceConstruction();
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathItem), true);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000F040 File Offset: 0x0000E040
		private void CopySequence(QilNode nd)
		{
			XmlQueryType xmlType = nd.XmlType;
			bool flag;
			Label label;
			this.StartWriterLoop(nd, out flag, out label);
			if (xmlType.IsSingleton)
			{
				this.helper.LoadQueryOutput();
				base.Visit(nd);
				this.iterCurr.EnsureItemStorageType(nd.XmlType, typeof(XPathItem));
			}
			else
			{
				base.Visit(nd);
				this.iterCurr.EnsureItemStorageType(nd.XmlType, typeof(XPathItem));
				this.iterCurr.EnsureNoStackNoCache("$$$copyTemp");
				this.helper.LoadQueryOutput();
			}
			this.iterCurr.EnsureStackNoCache();
			this.helper.Call(XmlILMethods.WriteItem);
			this.EndWriterLoop(nd, flag, label);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000F0FC File Offset: 0x0000E0FC
		protected override QilNode VisitDataSource(QilDataSource ndSrc)
		{
			this.helper.LoadQueryContext();
			this.NestedVisitEnsureStack(ndSrc.Name);
			this.NestedVisitEnsureStack(ndSrc.BaseUri);
			this.helper.Call(XmlILMethods.GetDataSource);
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$navDoc", typeof(XPathNavigator));
			this.helper.Emit(OpCodes.Stloc, localBuilder);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.Emit(OpCodes.Brfalse, this.iterCurr.GetLabelNext());
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, typeof(XPathNavigator), false);
			return ndSrc;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000F1B1 File Offset: 0x0000E1B1
		protected override QilNode VisitNop(QilUnary ndNop)
		{
			return this.Visit(ndNop.Child);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000F1BF File Offset: 0x0000E1BF
		protected override QilNode VisitOptimizeBarrier(QilUnary ndBarrier)
		{
			return this.Visit(ndBarrier.Child);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F1D0 File Offset: 0x0000E1D0
		protected override QilNode VisitError(QilUnary ndErr)
		{
			this.helper.LoadQueryRuntime();
			this.NestedVisitEnsureStack(ndErr.Child);
			this.helper.Call(XmlILMethods.ThrowException);
			if (XmlILConstructInfo.Read(ndErr).ConstructMethod == XmlILConstructMethod.Writer)
			{
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				this.helper.Emit(OpCodes.Ldnull);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathItem), false);
			}
			return ndErr;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F250 File Offset: 0x0000E250
		protected override QilNode VisitWarning(QilUnary ndWarning)
		{
			this.helper.LoadQueryRuntime();
			this.NestedVisitEnsureStack(ndWarning.Child);
			this.helper.Call(XmlILMethods.SendMessage);
			if (XmlILConstructInfo.Read(ndWarning).ConstructMethod == XmlILConstructMethod.Writer)
			{
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				this.VisitEmpty(ndWarning);
			}
			return ndWarning;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F2AC File Offset: 0x0000E2AC
		protected override QilNode VisitTrue(QilNode ndTrue)
		{
			if (this.iterCurr.CurrentBranchingContext != BranchingContext.None)
			{
				this.helper.EmitUnconditionalBranch((this.iterCurr.CurrentBranchingContext == BranchingContext.OnTrue) ? OpCodes.Brtrue : OpCodes.Brfalse, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				this.helper.LoadBoolean(true);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
			}
			return ndTrue;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F330 File Offset: 0x0000E330
		protected override QilNode VisitFalse(QilNode ndFalse)
		{
			if (this.iterCurr.CurrentBranchingContext != BranchingContext.None)
			{
				this.helper.EmitUnconditionalBranch((this.iterCurr.CurrentBranchingContext == BranchingContext.OnFalse) ? OpCodes.Brtrue : OpCodes.Brfalse, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				this.helper.LoadBoolean(false);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
			}
			return ndFalse;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F3B4 File Offset: 0x0000E3B4
		protected override QilNode VisitLiteralString(QilLiteral ndStr)
		{
			this.helper.Emit(OpCodes.Ldstr, ndStr);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
			return ndStr;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F3E8 File Offset: 0x0000E3E8
		protected override QilNode VisitLiteralInt32(QilLiteral ndInt)
		{
			this.helper.LoadInteger(ndInt);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(int), false);
			return ndInt;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F417 File Offset: 0x0000E417
		protected override QilNode VisitLiteralInt64(QilLiteral ndLong)
		{
			this.helper.Emit(OpCodes.Ldc_I8, ndLong);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(long), false);
			return ndLong;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F44B File Offset: 0x0000E44B
		protected override QilNode VisitLiteralDouble(QilLiteral ndDbl)
		{
			this.helper.Emit(OpCodes.Ldc_R8, ndDbl);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(double), false);
			return ndDbl;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F480 File Offset: 0x0000E480
		protected override QilNode VisitLiteralDecimal(QilLiteral ndDec)
		{
			this.helper.ConstructLiteralDecimal(ndDec);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(decimal), false);
			return ndDec;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F4AF File Offset: 0x0000E4AF
		protected override QilNode VisitLiteralQName(QilName ndQName)
		{
			this.helper.ConstructLiteralQName(ndQName.LocalName, ndQName.NamespaceUri);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XmlQualifiedName), false);
			return ndQName;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F4E4 File Offset: 0x0000E4E4
		protected override QilNode VisitAnd(QilBinary ndAnd)
		{
			IteratorDescriptor iteratorDescriptor = this.iterCurr;
			this.StartNestedIterator(ndAnd.Left);
			Label label = this.StartConjunctiveTests(iteratorDescriptor.CurrentBranchingContext, iteratorDescriptor.LabelBranch);
			this.Visit(ndAnd.Left);
			this.EndNestedIterator(ndAnd.Left);
			this.StartNestedIterator(ndAnd.Right);
			this.StartLastConjunctiveTest(iteratorDescriptor.CurrentBranchingContext, iteratorDescriptor.LabelBranch, label);
			this.Visit(ndAnd.Right);
			this.EndNestedIterator(ndAnd.Right);
			this.EndConjunctiveTests(iteratorDescriptor.CurrentBranchingContext, iteratorDescriptor.LabelBranch, label);
			return ndAnd;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F57C File Offset: 0x0000E57C
		private Label StartConjunctiveTests(BranchingContext brctxt, Label lblBranch)
		{
			if (brctxt == BranchingContext.OnFalse)
			{
				this.iterCurr.SetBranching(BranchingContext.OnFalse, lblBranch);
				return lblBranch;
			}
			Label label = this.helper.DefineLabel();
			this.iterCurr.SetBranching(BranchingContext.OnFalse, label);
			return label;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F5B8 File Offset: 0x0000E5B8
		private void StartLastConjunctiveTest(BranchingContext brctxt, Label lblBranch, Label lblOnFalse)
		{
			if (brctxt == BranchingContext.OnTrue)
			{
				this.iterCurr.SetBranching(BranchingContext.OnTrue, lblBranch);
				return;
			}
			this.iterCurr.SetBranching(BranchingContext.OnFalse, lblOnFalse);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F5E8 File Offset: 0x0000E5E8
		private void EndConjunctiveTests(BranchingContext brctxt, Label lblBranch, Label lblOnFalse)
		{
			switch (brctxt)
			{
			case BranchingContext.None:
				this.helper.ConvBranchToBool(lblOnFalse, false);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
				return;
			case BranchingContext.OnTrue:
				this.helper.MarkLabel(lblOnFalse);
				break;
			case BranchingContext.OnFalse:
				break;
			default:
				return;
			}
			this.iterCurr.Storage = StorageDescriptor.None();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F650 File Offset: 0x0000E650
		protected override QilNode VisitOr(QilBinary ndOr)
		{
			Label label = default(Label);
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.OnTrue:
				this.NestedVisitWithBranch(ndOr.Left, BranchingContext.OnTrue, this.iterCurr.LabelBranch);
				break;
			case BranchingContext.OnFalse:
				label = this.helper.DefineLabel();
				this.NestedVisitWithBranch(ndOr.Left, BranchingContext.OnTrue, label);
				break;
			default:
				label = this.helper.DefineLabel();
				this.NestedVisitWithBranch(ndOr.Left, BranchingContext.OnTrue, label);
				break;
			}
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.OnTrue:
				this.NestedVisitWithBranch(ndOr.Right, BranchingContext.OnTrue, this.iterCurr.LabelBranch);
				break;
			case BranchingContext.OnFalse:
				this.NestedVisitWithBranch(ndOr.Right, BranchingContext.OnFalse, this.iterCurr.LabelBranch);
				break;
			default:
				this.NestedVisitWithBranch(ndOr.Right, BranchingContext.OnTrue, label);
				break;
			}
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.None:
				this.helper.ConvBranchToBool(label, true);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
				return ndOr;
			case BranchingContext.OnTrue:
				break;
			case BranchingContext.OnFalse:
				this.helper.MarkLabel(label);
				break;
			default:
				return ndOr;
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndOr;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F79C File Offset: 0x0000E79C
		protected override QilNode VisitNot(QilUnary ndNot)
		{
			Label label = default(Label);
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.OnTrue:
				this.NestedVisitWithBranch(ndNot.Child, BranchingContext.OnFalse, this.iterCurr.LabelBranch);
				break;
			case BranchingContext.OnFalse:
				this.NestedVisitWithBranch(ndNot.Child, BranchingContext.OnTrue, this.iterCurr.LabelBranch);
				break;
			default:
				label = this.helper.DefineLabel();
				this.NestedVisitWithBranch(ndNot.Child, BranchingContext.OnTrue, label);
				break;
			}
			if (this.iterCurr.CurrentBranchingContext == BranchingContext.None)
			{
				this.helper.ConvBranchToBool(label, false);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
			}
			else
			{
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			return ndNot;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F868 File Offset: 0x0000E868
		protected override QilNode VisitConditional(QilTernary ndCond)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(ndCond);
			if (xmlILConstructInfo.ConstructMethod == XmlILConstructMethod.Writer)
			{
				Label label = this.helper.DefineLabel();
				this.NestedVisitWithBranch(ndCond.Left, BranchingContext.OnFalse, label);
				this.NestedVisit(ndCond.Center);
				if (ndCond.Right.NodeType == QilNodeType.Sequence && ndCond.Right.Count == 0)
				{
					this.helper.MarkLabel(label);
					this.NestedVisit(ndCond.Right);
				}
				else
				{
					Label label2 = this.helper.DefineLabel();
					this.helper.EmitUnconditionalBranch(OpCodes.Br, label2);
					this.helper.MarkLabel(label);
					this.NestedVisit(ndCond.Right);
					this.helper.MarkLabel(label2);
				}
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				LocalBuilder localBuilder = null;
				LocalBuilder localBuilder2 = null;
				Type itemStorageType = this.GetItemStorageType(ndCond);
				Label label3 = this.helper.DefineLabel();
				if (ndCond.XmlType.IsSingleton)
				{
					this.NestedVisitWithBranch(ndCond.Left, BranchingContext.OnFalse, label3);
				}
				else
				{
					localBuilder2 = this.helper.DeclareLocal("$$$cond", itemStorageType);
					localBuilder = this.helper.DeclareLocal("$$$boolResult", typeof(bool));
					this.NestedVisitEnsureLocal(ndCond.Left, localBuilder);
					this.helper.Emit(OpCodes.Ldloc, localBuilder);
					this.helper.Emit(OpCodes.Brfalse, label3);
				}
				this.ConditionalBranch(ndCond.Center, itemStorageType, localBuilder2);
				IteratorDescriptor iteratorDescriptor = this.iterNested;
				Label label4 = this.helper.DefineLabel();
				this.helper.EmitUnconditionalBranch(OpCodes.Br, label4);
				this.helper.MarkLabel(label3);
				this.ConditionalBranch(ndCond.Right, itemStorageType, localBuilder2);
				if (!ndCond.XmlType.IsSingleton)
				{
					this.helper.EmitUnconditionalBranch(OpCodes.Brtrue, label4);
					Label label5 = this.helper.DefineLabel();
					this.helper.MarkLabel(label5);
					this.helper.Emit(OpCodes.Ldloc, localBuilder);
					this.helper.Emit(OpCodes.Brtrue, iteratorDescriptor.GetLabelNext());
					this.helper.EmitUnconditionalBranch(OpCodes.Br, this.iterNested.GetLabelNext());
					this.iterCurr.SetIterator(label5, StorageDescriptor.Local(localBuilder2, itemStorageType, false));
				}
				this.helper.MarkLabel(label4);
			}
			return ndCond;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000FAD0 File Offset: 0x0000EAD0
		private void ConditionalBranch(QilNode ndBranch, Type itemStorageType, LocalBuilder locResult)
		{
			if (locResult != null)
			{
				this.NestedVisit(ndBranch, this.iterCurr.GetLabelNext());
				this.iterCurr.EnsureItemStorageType(ndBranch.XmlType, itemStorageType);
				this.iterCurr.EnsureLocalNoCache(locResult);
				return;
			}
			if (this.iterCurr.IsBranching)
			{
				this.NestedVisitWithBranch(ndBranch, this.iterCurr.CurrentBranchingContext, this.iterCurr.LabelBranch);
				return;
			}
			this.NestedVisitEnsureStack(ndBranch, itemStorageType, false);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000FB48 File Offset: 0x0000EB48
		protected override QilNode VisitChoice(QilChoice ndChoice)
		{
			this.NestedVisit(ndChoice.Expression);
			QilNode branches = ndChoice.Branches;
			int num = branches.Count - 1;
			Label[] array = new Label[num];
			int i;
			for (i = 0; i < num; i++)
			{
				array[i] = this.helper.DefineLabel();
			}
			Label label = this.helper.DefineLabel();
			Label label2 = this.helper.DefineLabel();
			this.helper.Emit(OpCodes.Switch, array);
			this.helper.EmitUnconditionalBranch(OpCodes.Br, label);
			for (i = 0; i < num; i++)
			{
				this.helper.MarkLabel(array[i]);
				this.NestedVisit(branches[i]);
				this.helper.EmitUnconditionalBranch(OpCodes.Br, label2);
			}
			this.helper.MarkLabel(label);
			this.NestedVisit(branches[i]);
			this.helper.MarkLabel(label2);
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndChoice;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000FC5C File Offset: 0x0000EC5C
		protected override QilNode VisitLength(QilUnary ndSetLen)
		{
			Label label = this.helper.DefineLabel();
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndSetLen);
			if (this.CachesResult(ndSetLen.Child))
			{
				this.NestedVisitEnsureStack(ndSetLen.Child);
				this.helper.CallCacheCount(this.iterNested.Storage.ItemStorageType);
			}
			else
			{
				this.helper.Emit(OpCodes.Ldc_I4_0);
				this.StartNestedIterator(ndSetLen.Child, label);
				this.Visit(ndSetLen.Child);
				this.iterCurr.EnsureNoCache();
				this.iterCurr.DiscardStack();
				this.helper.Emit(OpCodes.Ldc_I4_1);
				this.helper.Emit(OpCodes.Add);
				if (optimizerPatterns.MatchesPattern(OptimizerPatternName.MaxPosition))
				{
					this.helper.Emit(OpCodes.Dup);
					this.helper.LoadInteger((int)optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					this.helper.Emit(OpCodes.Bgt, label);
				}
				this.iterCurr.LoopToEnd(label);
				this.EndNestedIterator(ndSetLen.Child);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(int), false);
			return ndSetLen;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000FD90 File Offset: 0x0000ED90
		protected override QilNode VisitSequence(QilList ndSeq)
		{
			if (XmlILConstructInfo.Read(ndSeq).ConstructMethod == XmlILConstructMethod.Writer)
			{
				using (IEnumerator<QilNode> enumerator = ndSeq.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QilNode qilNode = enumerator.Current;
						this.NestedVisit(qilNode);
					}
					return ndSeq;
				}
			}
			if (ndSeq.Count == 0)
			{
				this.VisitEmpty(ndSeq);
			}
			else
			{
				this.Sequence(ndSeq);
			}
			return ndSeq;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000FE00 File Offset: 0x0000EE00
		private void VisitEmpty(QilNode nd)
		{
			this.helper.EmitUnconditionalBranch(OpCodes.Brtrue, this.iterCurr.GetLabelNext());
			this.helper.Emit(OpCodes.Ldnull);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathItem), false);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000FE54 File Offset: 0x0000EE54
		private void Sequence(QilList ndSeq)
		{
			Label label = default(Label);
			Type itemStorageType = this.GetItemStorageType(ndSeq);
			if (ndSeq.XmlType.IsSingleton)
			{
				foreach (QilNode qilNode in ndSeq)
				{
					if (qilNode.XmlType.IsSingleton)
					{
						this.NestedVisitEnsureStack(qilNode);
					}
					else
					{
						label = this.helper.DefineLabel();
						this.NestedVisit(qilNode, label);
						this.iterCurr.DiscardStack();
						this.helper.MarkLabel(label);
					}
				}
				this.iterCurr.Storage = StorageDescriptor.Stack(itemStorageType, false);
				return;
			}
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$itemList", itemStorageType);
			LocalBuilder localBuilder2 = this.helper.DeclareLocal("$$$idxList", typeof(int));
			Label[] array = new Label[ndSeq.Count];
			Label label2 = this.helper.DefineLabel();
			for (int i = 0; i < ndSeq.Count; i++)
			{
				if (i != 0)
				{
					this.helper.MarkLabel(label);
				}
				if (i == ndSeq.Count - 1)
				{
					label = this.iterCurr.GetLabelNext();
				}
				else
				{
					label = this.helper.DefineLabel();
				}
				this.helper.LoadInteger(i);
				this.helper.Emit(OpCodes.Stloc, localBuilder2);
				this.NestedVisit(ndSeq[i], label);
				this.iterCurr.EnsureItemStorageType(ndSeq[i].XmlType, itemStorageType);
				this.iterCurr.EnsureLocalNoCache(localBuilder);
				array[i] = this.iterNested.GetLabelNext();
				this.helper.EmitUnconditionalBranch(OpCodes.Brtrue, label2);
			}
			Label label3 = this.helper.DefineLabel();
			this.helper.MarkLabel(label3);
			this.helper.Emit(OpCodes.Ldloc, localBuilder2);
			this.helper.Emit(OpCodes.Switch, array);
			this.helper.MarkLabel(label2);
			this.iterCurr.SetIterator(label3, StorageDescriptor.Local(localBuilder, itemStorageType, false));
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0001008C File Offset: 0x0000F08C
		protected override QilNode VisitUnion(QilBinary ndUnion)
		{
			return this.CreateSetIterator(ndUnion, "$$$iterUnion", typeof(UnionIterator), XmlILMethods.UnionCreate, XmlILMethods.UnionNext);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000100AE File Offset: 0x0000F0AE
		protected override QilNode VisitIntersection(QilBinary ndInter)
		{
			return this.CreateSetIterator(ndInter, "$$$iterInter", typeof(IntersectIterator), XmlILMethods.InterCreate, XmlILMethods.InterNext);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000100D0 File Offset: 0x0000F0D0
		protected override QilNode VisitDifference(QilBinary ndDiff)
		{
			return this.CreateSetIterator(ndDiff, "$$$iterDiff", typeof(DifferenceIterator), XmlILMethods.DiffCreate, XmlILMethods.DiffNext);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000100F4 File Offset: 0x0000F0F4
		private QilNode CreateSetIterator(QilBinary ndSet, string iterName, Type iterType, MethodInfo methCreate, MethodInfo methNext)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal(iterName, iterType);
			LocalBuilder localBuilder2 = this.helper.DeclareLocal("$$$navSet", typeof(XPathNavigator));
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.helper.LoadQueryRuntime();
			this.helper.Call(methCreate);
			Label label = this.helper.DefineLabel();
			Label label2 = this.helper.DefineLabel();
			Label label3 = this.helper.DefineLabel();
			this.NestedVisit(ndSet.Left, label);
			Label labelNext = this.iterNested.GetLabelNext();
			this.iterCurr.EnsureLocal(localBuilder2);
			this.helper.EmitUnconditionalBranch(OpCodes.Brtrue, label2);
			this.helper.MarkLabel(label3);
			this.NestedVisit(ndSet.Right, label);
			Label labelNext2 = this.iterNested.GetLabelNext();
			this.iterCurr.EnsureLocal(localBuilder2);
			this.helper.EmitUnconditionalBranch(OpCodes.Brtrue, label2);
			this.helper.MarkLabel(label);
			this.helper.Emit(OpCodes.Ldnull);
			this.helper.Emit(OpCodes.Stloc, localBuilder2);
			this.helper.MarkLabel(label2);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.helper.Emit(OpCodes.Ldloc, localBuilder2);
			this.helper.Call(methNext);
			if (ndSet.XmlType.IsSingleton)
			{
				this.helper.Emit(OpCodes.Switch, new Label[] { label3, labelNext, labelNext2 });
				this.iterCurr.Storage = StorageDescriptor.Current(localBuilder, typeof(XPathNavigator));
			}
			else
			{
				this.helper.Emit(OpCodes.Switch, new Label[]
				{
					this.iterCurr.GetLabelNext(),
					label3,
					labelNext,
					labelNext2
				});
				this.iterCurr.SetIterator(label, StorageDescriptor.Current(localBuilder, typeof(XPathNavigator)));
			}
			return ndSet;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00010348 File Offset: 0x0000F348
		protected override QilNode VisitAverage(QilUnary ndAvg)
		{
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[this.GetItemStorageType(ndAvg)];
			return this.CreateAggregator(ndAvg, "$$$aggAvg", xmlILStorageMethods, xmlILStorageMethods.AggAvg, xmlILStorageMethods.AggAvgResult);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00010380 File Offset: 0x0000F380
		protected override QilNode VisitSum(QilUnary ndSum)
		{
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[this.GetItemStorageType(ndSum)];
			return this.CreateAggregator(ndSum, "$$$aggSum", xmlILStorageMethods, xmlILStorageMethods.AggSum, xmlILStorageMethods.AggSumResult);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000103B8 File Offset: 0x0000F3B8
		protected override QilNode VisitMinimum(QilUnary ndMin)
		{
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[this.GetItemStorageType(ndMin)];
			return this.CreateAggregator(ndMin, "$$$aggMin", xmlILStorageMethods, xmlILStorageMethods.AggMin, xmlILStorageMethods.AggMinResult);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000103F0 File Offset: 0x0000F3F0
		protected override QilNode VisitMaximum(QilUnary ndMax)
		{
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[this.GetItemStorageType(ndMax)];
			return this.CreateAggregator(ndMax, "$$$aggMax", xmlILStorageMethods, xmlILStorageMethods.AggMax, xmlILStorageMethods.AggMaxResult);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00010428 File Offset: 0x0000F428
		private QilNode CreateAggregator(QilUnary ndAgg, string aggName, XmlILStorageMethods methods, MethodInfo methAgg, MethodInfo methResult)
		{
			Label label = this.helper.DefineLabel();
			Type declaringType = methAgg.DeclaringType;
			LocalBuilder localBuilder = this.helper.DeclareLocal(aggName, declaringType);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.helper.Call(methods.AggCreate);
			this.StartNestedIterator(ndAgg.Child, label);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.Visit(ndAgg.Child);
			this.iterCurr.EnsureStackNoCache();
			this.iterCurr.EnsureItemStorageType(ndAgg.XmlType, this.GetItemStorageType(ndAgg));
			this.helper.Call(methAgg);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.iterCurr.LoopToEnd(label);
			this.EndNestedIterator(ndAgg.Child);
			if (ndAgg.XmlType.MaybeEmpty)
			{
				this.helper.Call(methods.AggIsEmpty);
				this.helper.Emit(OpCodes.Brtrue, this.iterCurr.GetLabelNext());
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
			}
			this.helper.Call(methResult);
			this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(ndAgg), false);
			return ndAgg;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0001056D File Offset: 0x0000F56D
		protected override QilNode VisitNegate(QilUnary ndNeg)
		{
			this.NestedVisitEnsureStack(ndNeg.Child);
			this.helper.CallArithmeticOp(QilNodeType.Negate, ndNeg.XmlType.TypeCode);
			this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(ndNeg), false);
			return ndNeg;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000105AC File Offset: 0x0000F5AC
		protected override QilNode VisitAdd(QilBinary ndPlus)
		{
			return this.ArithmeticOp(ndPlus);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000105B5 File Offset: 0x0000F5B5
		protected override QilNode VisitSubtract(QilBinary ndMinus)
		{
			return this.ArithmeticOp(ndMinus);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000105BE File Offset: 0x0000F5BE
		protected override QilNode VisitMultiply(QilBinary ndMul)
		{
			return this.ArithmeticOp(ndMul);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000105C7 File Offset: 0x0000F5C7
		protected override QilNode VisitDivide(QilBinary ndDiv)
		{
			return this.ArithmeticOp(ndDiv);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000105D0 File Offset: 0x0000F5D0
		protected override QilNode VisitModulo(QilBinary ndMod)
		{
			return this.ArithmeticOp(ndMod);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000105DC File Offset: 0x0000F5DC
		private QilNode ArithmeticOp(QilBinary ndOp)
		{
			this.NestedVisitEnsureStack(ndOp.Left, ndOp.Right);
			this.helper.CallArithmeticOp(ndOp.NodeType, ndOp.XmlType.TypeCode);
			this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(ndOp), false);
			return ndOp;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00010630 File Offset: 0x0000F630
		protected override QilNode VisitStrLength(QilUnary ndLen)
		{
			this.NestedVisitEnsureStack(ndLen.Child);
			this.helper.Call(XmlILMethods.StrLen);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(int), false);
			return ndLen;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001066C File Offset: 0x0000F66C
		protected override QilNode VisitStrConcat(QilStrConcat ndStrConcat)
		{
			QilNode qilNode = ndStrConcat.Delimiter;
			if (qilNode.NodeType == QilNodeType.LiteralString && ((QilLiteral)qilNode).Length == 0)
			{
				qilNode = null;
			}
			QilNode values = ndStrConcat.Values;
			bool flag;
			if (values.NodeType == QilNodeType.Sequence && values.Count < 5)
			{
				flag = true;
				using (IEnumerator<QilNode> enumerator = values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QilNode qilNode2 = enumerator.Current;
						if (!qilNode2.XmlType.IsSingleton)
						{
							flag = false;
						}
					}
					goto IL_007D;
				}
			}
			flag = false;
			IL_007D:
			if (flag)
			{
				foreach (QilNode qilNode3 in values)
				{
					this.NestedVisitEnsureStack(qilNode3);
				}
				this.helper.CallConcatStrings(values.Count);
			}
			else
			{
				LocalBuilder localBuilder = this.helper.DeclareLocal("$$$strcat", typeof(StringConcat));
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
				this.helper.Call(XmlILMethods.StrCatClear);
				if (qilNode != null)
				{
					this.helper.Emit(OpCodes.Ldloca, localBuilder);
					this.NestedVisitEnsureStack(qilNode);
					this.helper.Call(XmlILMethods.StrCatDelim);
				}
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
				if (values.NodeType == QilNodeType.Sequence)
				{
					using (IEnumerator<QilNode> enumerator3 = values.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							QilNode qilNode4 = enumerator3.Current;
							this.GenerateConcat(qilNode4, localBuilder);
						}
						goto IL_0185;
					}
				}
				this.GenerateConcat(values, localBuilder);
				IL_0185:
				this.helper.Call(XmlILMethods.StrCatResult);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
			return ndStrConcat;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00010854 File Offset: 0x0000F854
		private void GenerateConcat(QilNode ndStr, LocalBuilder locStringConcat)
		{
			Label label = this.helper.DefineLabel();
			this.StartNestedIterator(ndStr, label);
			this.Visit(ndStr);
			this.iterCurr.EnsureStackNoCache();
			this.iterCurr.EnsureItemStorageType(ndStr.XmlType, typeof(string));
			this.helper.Call(XmlILMethods.StrCatCat);
			this.helper.Emit(OpCodes.Ldloca, locStringConcat);
			this.iterCurr.LoopToEnd(label);
			this.EndNestedIterator(ndStr);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000108D7 File Offset: 0x0000F8D7
		protected override QilNode VisitStrParseQName(QilBinary ndParsedTagName)
		{
			this.VisitStrParseQName(ndParsedTagName, false);
			return ndParsedTagName;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000108E4 File Offset: 0x0000F8E4
		private void VisitStrParseQName(QilBinary ndParsedTagName, bool preservePrefix)
		{
			if (!preservePrefix)
			{
				this.helper.LoadQueryRuntime();
			}
			this.NestedVisitEnsureStack(ndParsedTagName.Left);
			if (ndParsedTagName.Right.XmlType.TypeCode == XmlTypeCode.String)
			{
				this.NestedVisitEnsureStack(ndParsedTagName.Right);
				if (!preservePrefix)
				{
					this.helper.CallParseTagName(GenerateNameType.TagNameAndNamespace);
				}
			}
			else
			{
				if (ndParsedTagName.Right.NodeType == QilNodeType.Sequence)
				{
					this.helper.LoadInteger(this.helper.StaticData.DeclarePrefixMappings(ndParsedTagName.Right));
				}
				else
				{
					this.helper.LoadInteger(this.helper.StaticData.DeclarePrefixMappings(new QilNode[] { ndParsedTagName.Right }));
				}
				if (!preservePrefix)
				{
					this.helper.CallParseTagName(GenerateNameType.TagNameAndMappings);
				}
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XmlQualifiedName), false);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000109C4 File Offset: 0x0000F9C4
		protected override QilNode VisitNe(QilBinary ndNe)
		{
			this.Compare(ndNe);
			return ndNe;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000109CE File Offset: 0x0000F9CE
		protected override QilNode VisitEq(QilBinary ndEq)
		{
			this.Compare(ndEq);
			return ndEq;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000109D8 File Offset: 0x0000F9D8
		protected override QilNode VisitGt(QilBinary ndGt)
		{
			this.Compare(ndGt);
			return ndGt;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000109E2 File Offset: 0x0000F9E2
		protected override QilNode VisitGe(QilBinary ndGe)
		{
			this.Compare(ndGe);
			return ndGe;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000109EC File Offset: 0x0000F9EC
		protected override QilNode VisitLt(QilBinary ndLt)
		{
			this.Compare(ndLt);
			return ndLt;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000109F6 File Offset: 0x0000F9F6
		protected override QilNode VisitLe(QilBinary ndLe)
		{
			this.Compare(ndLe);
			return ndLe;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010A00 File Offset: 0x0000FA00
		private void Compare(QilBinary ndComp)
		{
			QilNodeType nodeType = ndComp.NodeType;
			if (nodeType == QilNodeType.Eq || nodeType == QilNodeType.Ne)
			{
				if (this.TryZeroCompare(nodeType, ndComp.Left, ndComp.Right))
				{
					return;
				}
				if (this.TryZeroCompare(nodeType, ndComp.Right, ndComp.Left))
				{
					return;
				}
				if (this.TryNameCompare(nodeType, ndComp.Left, ndComp.Right))
				{
					return;
				}
				if (this.TryNameCompare(nodeType, ndComp.Right, ndComp.Left))
				{
					return;
				}
			}
			this.NestedVisitEnsureStack(ndComp.Left, ndComp.Right);
			XmlTypeCode typeCode = ndComp.Left.XmlType.TypeCode;
			XmlTypeCode xmlTypeCode = typeCode;
			if (xmlTypeCode <= XmlTypeCode.QName)
			{
				switch (xmlTypeCode)
				{
				case XmlTypeCode.String:
				case XmlTypeCode.Decimal:
					break;
				case XmlTypeCode.Boolean:
				case XmlTypeCode.Double:
					goto IL_010F;
				case XmlTypeCode.Float:
					return;
				default:
					if (xmlTypeCode != XmlTypeCode.QName)
					{
						return;
					}
					break;
				}
				if (nodeType == QilNodeType.Eq || nodeType == QilNodeType.Ne)
				{
					this.helper.CallCompareEquals(typeCode);
					this.ZeroCompare((nodeType == QilNodeType.Eq) ? QilNodeType.Ne : QilNodeType.Eq, true);
					return;
				}
				this.helper.CallCompare(typeCode);
				this.helper.Emit(OpCodes.Ldc_I4_0);
				this.ClrCompare(nodeType, typeCode);
				return;
			}
			else if (xmlTypeCode != XmlTypeCode.Integer && xmlTypeCode != XmlTypeCode.Int)
			{
				return;
			}
			IL_010F:
			this.ClrCompare(nodeType, typeCode);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010B24 File Offset: 0x0000FB24
		protected override QilNode VisitIs(QilBinary ndIs)
		{
			this.NestedVisitEnsureStack(ndIs.Left, ndIs.Right);
			this.helper.Call(XmlILMethods.NavSamePos);
			this.ZeroCompare(QilNodeType.Ne, true);
			return ndIs;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010B52 File Offset: 0x0000FB52
		protected override QilNode VisitBefore(QilBinary ndBefore)
		{
			this.ComparePosition(ndBefore);
			return ndBefore;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00010B5C File Offset: 0x0000FB5C
		protected override QilNode VisitAfter(QilBinary ndAfter)
		{
			this.ComparePosition(ndAfter);
			return ndAfter;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010B68 File Offset: 0x0000FB68
		private void ComparePosition(QilBinary ndComp)
		{
			this.helper.LoadQueryRuntime();
			this.NestedVisitEnsureStack(ndComp.Left, ndComp.Right);
			this.helper.Call(XmlILMethods.CompPos);
			this.helper.LoadInteger(0);
			this.ClrCompare((ndComp.NodeType == QilNodeType.Before) ? QilNodeType.Lt : QilNodeType.Gt, XmlTypeCode.String);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010BC8 File Offset: 0x0000FBC8
		protected override QilNode VisitFor(QilIterator ndFor)
		{
			IteratorDescriptor cachedIteratorDescriptor = XmlILAnnotation.Write(ndFor).CachedIteratorDescriptor;
			this.iterCurr.Storage = cachedIteratorDescriptor.Storage;
			if (this.iterCurr.Storage.Location == ItemLocation.Global)
			{
				this.iterCurr.EnsureStack();
			}
			return ndFor;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010C14 File Offset: 0x0000FC14
		protected override QilNode VisitLet(QilIterator ndLet)
		{
			return this.VisitFor(ndLet);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00010C1D File Offset: 0x0000FC1D
		protected override QilNode VisitParameter(QilParameter ndParameter)
		{
			return this.VisitFor(ndParameter);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00010C28 File Offset: 0x0000FC28
		protected override QilNode VisitLoop(QilLoop ndLoop)
		{
			bool flag;
			Label label;
			this.StartWriterLoop(ndLoop, out flag, out label);
			this.StartBinding(ndLoop.Variable);
			this.Visit(ndLoop.Body);
			this.EndBinding(ndLoop.Variable);
			this.EndWriterLoop(ndLoop, flag, label);
			return ndLoop;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00010C70 File Offset: 0x0000FC70
		protected override QilNode VisitFilter(QilLoop ndFilter)
		{
			if (this.HandleFilterPatterns(ndFilter))
			{
				return ndFilter;
			}
			this.StartBinding(ndFilter.Variable);
			this.iterCurr.SetIterator(this.iterNested);
			this.StartNestedIterator(ndFilter.Body);
			this.iterCurr.SetBranching(BranchingContext.OnFalse, this.iterCurr.ParentIterator.GetLabelNext());
			this.Visit(ndFilter.Body);
			this.EndNestedIterator(ndFilter.Body);
			this.EndBinding(ndFilter.Variable);
			return ndFilter;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00010CF4 File Offset: 0x0000FCF4
		private bool HandleFilterPatterns(QilLoop ndFilter)
		{
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndFilter);
			bool flag = optimizerPatterns.MatchesPattern(OptimizerPatternName.FilterElements);
			if (flag || optimizerPatterns.MatchesPattern(OptimizerPatternName.FilterContentKind))
			{
				XmlNodeKindFlags xmlNodeKindFlags;
				QilName qilName;
				if (flag)
				{
					xmlNodeKindFlags = XmlNodeKindFlags.Element;
					qilName = (QilName)optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName);
				}
				else
				{
					xmlNodeKindFlags = ((XmlQueryType)optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName)).NodeKinds;
					qilName = null;
				}
				QilNode qilNode = (QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.StepNode);
				QilNode qilNode2 = (QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.StepInput);
				QilNodeType nodeType = qilNode.NodeType;
				switch (nodeType)
				{
				case QilNodeType.Content:
					if (flag)
					{
						LocalBuilder localBuilder = this.helper.DeclareLocal("$$$iterElemContent", typeof(ElementContentIterator));
						this.helper.Emit(OpCodes.Ldloca, localBuilder);
						this.NestedVisitEnsureStack(qilNode2);
						this.helper.CallGetAtomizedName(this.helper.StaticData.DeclareName(qilName.LocalName));
						this.helper.CallGetAtomizedName(this.helper.StaticData.DeclareName(qilName.NamespaceUri));
						this.helper.Call(XmlILMethods.ElemContentCreate);
						this.GenerateSimpleIterator(typeof(XPathNavigator), localBuilder, XmlILMethods.ElemContentNext);
					}
					else if (xmlNodeKindFlags == XmlNodeKindFlags.Content)
					{
						this.CreateSimpleIterator(qilNode2, "$$$iterContent", typeof(ContentIterator), XmlILMethods.ContentCreate, XmlILMethods.ContentNext);
					}
					else
					{
						LocalBuilder localBuilder = this.helper.DeclareLocal("$$$iterContent", typeof(NodeKindContentIterator));
						this.helper.Emit(OpCodes.Ldloca, localBuilder);
						this.NestedVisitEnsureStack(qilNode2);
						this.helper.LoadInteger((int)this.QilXmlToXPathNodeType(xmlNodeKindFlags));
						this.helper.Call(XmlILMethods.KindContentCreate);
						this.GenerateSimpleIterator(typeof(XPathNavigator), localBuilder, XmlILMethods.KindContentNext);
					}
					return true;
				case QilNodeType.Attribute:
				case QilNodeType.Root:
				case QilNodeType.XmlContext:
					break;
				case QilNodeType.Parent:
					this.CreateFilteredIterator(qilNode2, "$$$iterPar", typeof(ParentIterator), XmlILMethods.ParentCreate, XmlILMethods.ParentNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
					return true;
				case QilNodeType.Descendant:
				case QilNodeType.DescendantOrSelf:
					this.CreateFilteredIterator(qilNode2, "$$$iterDesc", typeof(DescendantIterator), XmlILMethods.DescCreate, XmlILMethods.DescNext, xmlNodeKindFlags, qilName, (qilNode.NodeType == QilNodeType.Descendant) ? TriState.False : TriState.True, null);
					return true;
				case QilNodeType.Ancestor:
				case QilNodeType.AncestorOrSelf:
					this.CreateFilteredIterator(qilNode2, "$$$iterAnc", typeof(AncestorIterator), XmlILMethods.AncCreate, XmlILMethods.AncNext, xmlNodeKindFlags, qilName, (qilNode.NodeType == QilNodeType.Ancestor) ? TriState.False : TriState.True, null);
					return true;
				case QilNodeType.Preceding:
					this.CreateFilteredIterator(qilNode2, "$$$iterPrec", typeof(PrecedingIterator), XmlILMethods.PrecCreate, XmlILMethods.PrecNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
					return true;
				case QilNodeType.FollowingSibling:
					this.CreateFilteredIterator(qilNode2, "$$$iterFollSib", typeof(FollowingSiblingIterator), XmlILMethods.FollSibCreate, XmlILMethods.FollSibNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
					return true;
				case QilNodeType.PrecedingSibling:
					this.CreateFilteredIterator(qilNode2, "$$$iterPreSib", typeof(PrecedingSiblingIterator), XmlILMethods.PreSibCreate, XmlILMethods.PreSibNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
					return true;
				case QilNodeType.NodeRange:
					this.CreateFilteredIterator(qilNode2, "$$$iterRange", typeof(NodeRangeIterator), XmlILMethods.NodeRangeCreate, XmlILMethods.NodeRangeNext, xmlNodeKindFlags, qilName, TriState.Unknown, ((QilBinary)qilNode).Right);
					return true;
				default:
					switch (nodeType)
					{
					case QilNodeType.XPathFollowing:
						this.CreateFilteredIterator(qilNode2, "$$$iterFoll", typeof(XPathFollowingIterator), XmlILMethods.XPFollCreate, XmlILMethods.XPFollNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
						return true;
					case QilNodeType.XPathPreceding:
						this.CreateFilteredIterator(qilNode2, "$$$iterPrec", typeof(XPathPrecedingIterator), XmlILMethods.XPPrecCreate, XmlILMethods.XPPrecNext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
						return true;
					}
					break;
				}
			}
			else
			{
				if (optimizerPatterns.MatchesPattern(OptimizerPatternName.FilterAttributeKind))
				{
					QilNode qilNode2 = (QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.StepInput);
					this.CreateSimpleIterator(qilNode2, "$$$iterAttr", typeof(AttributeIterator), XmlILMethods.AttrCreate, XmlILMethods.AttrNext);
					return true;
				}
				if (optimizerPatterns.MatchesPattern(OptimizerPatternName.EqualityIndex))
				{
					Label label = this.helper.DefineLabel();
					Label label2 = this.helper.DefineLabel();
					QilIterator qilIterator = (QilIterator)optimizerPatterns.GetArgument(OptimizerPatternArgument.StepNode);
					QilNode qilNode3 = (QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.StepInput);
					LocalBuilder localBuilder2 = this.helper.DeclareLocal("$$$index", typeof(XmlILIndex));
					this.helper.LoadQueryRuntime();
					this.helper.Emit(OpCodes.Ldarg_1);
					this.helper.LoadInteger(this.indexId);
					this.helper.Emit(OpCodes.Ldloca, localBuilder2);
					this.helper.Call(XmlILMethods.FindIndex);
					this.helper.Emit(OpCodes.Brtrue, label2);
					this.helper.LoadQueryRuntime();
					this.helper.Emit(OpCodes.Ldarg_1);
					this.helper.LoadInteger(this.indexId);
					this.helper.Emit(OpCodes.Ldloc, localBuilder2);
					this.StartNestedIterator(qilIterator, label);
					this.StartBinding(qilIterator);
					this.Visit(qilNode3);
					this.iterCurr.EnsureStackNoCache();
					this.VisitFor(qilIterator);
					this.iterCurr.EnsureStackNoCache();
					this.iterCurr.EnsureItemStorageType(qilIterator.XmlType, typeof(XPathNavigator));
					this.helper.Call(XmlILMethods.IndexAdd);
					this.helper.Emit(OpCodes.Ldloc, localBuilder2);
					this.iterCurr.LoopToEnd(label);
					this.EndBinding(qilIterator);
					this.EndNestedIterator(qilIterator);
					this.helper.Call(XmlILMethods.AddNewIndex);
					this.helper.MarkLabel(label2);
					this.helper.Emit(OpCodes.Ldloc, localBuilder2);
					this.helper.Emit(OpCodes.Ldarg_2);
					this.helper.Call(XmlILMethods.IndexLookup);
					this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathNavigator), true);
					this.indexId++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000112D0 File Offset: 0x000102D0
		private void StartBinding(QilIterator ndIter)
		{
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndIter);
			if (this.qil.IsDebug && ndIter.SourceLine != null)
			{
				this.helper.DebugSequencePoint(ndIter.SourceLine);
			}
			if (ndIter.NodeType == QilNodeType.For || ndIter.XmlType.IsSingleton)
			{
				this.StartForBinding(ndIter, optimizerPatterns);
			}
			else
			{
				this.StartLetBinding(ndIter);
			}
			XmlILAnnotation.Write(ndIter).CachedIteratorDescriptor = this.iterNested;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00011344 File Offset: 0x00010344
		private void StartForBinding(QilIterator ndFor, OptimizerPatterns patt)
		{
			LocalBuilder localBuilder = null;
			if (this.iterCurr.HasLabelNext)
			{
				this.StartNestedIterator(ndFor.Binding, this.iterCurr.GetLabelNext());
			}
			else
			{
				this.StartNestedIterator(ndFor.Binding);
			}
			if (patt.MatchesPattern(OptimizerPatternName.IsPositional))
			{
				localBuilder = this.helper.DeclareLocal("$$$pos", typeof(int));
				this.helper.Emit(OpCodes.Ldc_I4_0);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
			}
			this.Visit(ndFor.Binding);
			if (this.qil.IsDebug && ndFor.DebugName != null)
			{
				this.helper.DebugStartScope();
				this.iterCurr.EnsureLocalNoCache("$$$for");
				this.iterCurr.Storage.LocalLocation.SetLocalSymInfo(ndFor.DebugName);
			}
			else
			{
				this.iterCurr.EnsureNoStackNoCache("$$$for");
			}
			if (patt.MatchesPattern(OptimizerPatternName.IsPositional))
			{
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.helper.Emit(OpCodes.Ldc_I4_1);
				this.helper.Emit(OpCodes.Add);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
				if (patt.MatchesPattern(OptimizerPatternName.MaxPosition))
				{
					this.helper.Emit(OpCodes.Ldloc, localBuilder);
					this.helper.LoadInteger((int)patt.GetArgument(OptimizerPatternArgument.ElementQName));
					this.helper.Emit(OpCodes.Bgt, this.iterCurr.ParentIterator.GetLabelNext());
				}
				this.iterCurr.LocalPosition = localBuilder;
			}
			this.EndNestedIterator(ndFor.Binding);
			this.iterCurr.SetIterator(this.iterNested);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00011500 File Offset: 0x00010500
		public void StartLetBinding(QilIterator ndLet)
		{
			this.StartNestedIterator(ndLet);
			this.NestedVisit(ndLet.Binding, this.GetItemStorageType(ndLet), !ndLet.XmlType.IsSingleton);
			if (this.qil.IsDebug && ndLet.DebugName != null)
			{
				this.helper.DebugStartScope();
				this.iterCurr.EnsureLocal("$$$cache");
				this.iterCurr.Storage.LocalLocation.SetLocalSymInfo(ndLet.DebugName);
			}
			else
			{
				this.iterCurr.EnsureNoStack("$$$cache");
			}
			this.EndNestedIterator(ndLet);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0001159C File Offset: 0x0001059C
		private void EndBinding(QilIterator ndIter)
		{
			if (this.qil.IsDebug && ndIter.DebugName != null)
			{
				this.helper.DebugEndScope();
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000115C0 File Offset: 0x000105C0
		protected override QilNode VisitPositionOf(QilUnary ndPos)
		{
			QilIterator qilIterator = ndPos.Child as QilIterator;
			LocalBuilder localPosition = XmlILAnnotation.Write(qilIterator).CachedIteratorDescriptor.LocalPosition;
			this.iterCurr.Storage = StorageDescriptor.Local(localPosition, typeof(int), false);
			return ndPos;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00011608 File Offset: 0x00010608
		protected override QilNode VisitSort(QilLoop ndSort)
		{
			Type itemStorageType = this.GetItemStorageType(ndSort);
			Label label = this.helper.DefineLabel();
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[itemStorageType];
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$cache", xmlILStorageMethods.SeqType);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.CallToken(xmlILStorageMethods.SeqReuse);
			this.helper.Emit(OpCodes.Stloc, localBuilder);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			LocalBuilder localBuilder2 = this.helper.DeclareLocal("$$$keys", typeof(XmlSortKeyAccumulator));
			this.helper.Emit(OpCodes.Ldloca, localBuilder2);
			this.helper.Call(XmlILMethods.SortKeyCreate);
			this.StartNestedIterator(ndSort.Variable, label);
			this.StartBinding(ndSort.Variable);
			this.iterCurr.EnsureStackNoCache();
			this.iterCurr.EnsureItemStorageType(ndSort.Variable.XmlType, this.GetItemStorageType(ndSort.Variable));
			this.helper.Call(xmlILStorageMethods.SeqAdd);
			this.helper.Emit(OpCodes.Ldloca, localBuilder2);
			foreach (QilNode qilNode in ndSort.Body)
			{
				QilSortKey qilSortKey = (QilSortKey)qilNode;
				this.VisitSortKey(qilSortKey, localBuilder2);
			}
			this.helper.Call(XmlILMethods.SortKeyFinish);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.iterCurr.LoopToEnd(label);
			this.helper.Emit(OpCodes.Pop);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.Emit(OpCodes.Ldloca, localBuilder2);
			this.helper.Call(XmlILMethods.SortKeyKeys);
			this.helper.Call(xmlILStorageMethods.SeqSortByKeys);
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, itemStorageType, true);
			this.EndBinding(ndSort.Variable);
			this.EndNestedIterator(ndSort.Variable);
			this.iterCurr.SetIterator(this.iterNested);
			return ndSort;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00011844 File Offset: 0x00010844
		private void VisitSortKey(QilSortKey ndKey, LocalBuilder locKeys)
		{
			this.helper.Emit(OpCodes.Ldloca, locKeys);
			if (ndKey.Collation.NodeType == QilNodeType.LiteralString)
			{
				this.helper.CallGetCollation(this.helper.StaticData.DeclareCollation((QilLiteral)ndKey.Collation));
			}
			else
			{
				this.helper.LoadQueryRuntime();
				this.NestedVisitEnsureStack(ndKey.Collation);
				this.helper.Call(XmlILMethods.CreateCollation);
			}
			if (ndKey.XmlType.IsSingleton)
			{
				this.NestedVisitEnsureStack(ndKey.Key);
				this.helper.AddSortKey(ndKey.Key.XmlType);
				return;
			}
			Label label = this.helper.DefineLabel();
			this.StartNestedIterator(ndKey.Key, label);
			this.Visit(ndKey.Key);
			this.iterCurr.EnsureStackNoCache();
			this.iterCurr.EnsureItemStorageType(ndKey.Key.XmlType, this.GetItemStorageType(ndKey.Key));
			this.helper.AddSortKey(ndKey.Key.XmlType);
			Label label2 = this.helper.DefineLabel();
			this.helper.EmitUnconditionalBranch(OpCodes.Br_S, label2);
			this.helper.MarkLabel(label);
			this.helper.AddSortKey(null);
			this.helper.MarkLabel(label2);
			this.EndNestedIterator(ndKey.Key);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000119AC File Offset: 0x000109AC
		protected override QilNode VisitDocOrderDistinct(QilUnary ndDod)
		{
			if (ndDod.XmlType.IsSingleton)
			{
				return this.Visit(ndDod.Child);
			}
			if (this.HandleDodPatterns(ndDod))
			{
				return ndDod;
			}
			this.helper.LoadQueryRuntime();
			this.NestedVisitEnsureCache(ndDod.Child, typeof(XPathNavigator));
			this.iterCurr.EnsureStack();
			this.helper.Call(XmlILMethods.DocOrder);
			return ndDod;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00011A1C File Offset: 0x00010A1C
		private bool HandleDodPatterns(QilUnary ndDod)
		{
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndDod);
			bool flag = optimizerPatterns.MatchesPattern(OptimizerPatternName.JoinAndDod);
			if (flag || optimizerPatterns.MatchesPattern(OptimizerPatternName.DodReverse))
			{
				OptimizerPatterns optimizerPatterns2 = OptimizerPatterns.Read((QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
				XmlNodeKindFlags xmlNodeKindFlags;
				QilName qilName;
				if (optimizerPatterns2.MatchesPattern(OptimizerPatternName.FilterElements))
				{
					xmlNodeKindFlags = XmlNodeKindFlags.Element;
					qilName = (QilName)optimizerPatterns2.GetArgument(OptimizerPatternArgument.ElementQName);
				}
				else if (optimizerPatterns2.MatchesPattern(OptimizerPatternName.FilterContentKind))
				{
					xmlNodeKindFlags = ((XmlQueryType)optimizerPatterns2.GetArgument(OptimizerPatternArgument.ElementQName)).NodeKinds;
					qilName = null;
				}
				else
				{
					xmlNodeKindFlags = (((ndDod.XmlType.NodeKinds & XmlNodeKindFlags.Attribute) != XmlNodeKindFlags.None) ? XmlNodeKindFlags.Any : XmlNodeKindFlags.Content);
					qilName = null;
				}
				QilNode qilNode = (QilNode)optimizerPatterns2.GetArgument(OptimizerPatternArgument.StepNode);
				if (flag)
				{
					QilNodeType nodeType = qilNode.NodeType;
					if (nodeType <= QilNodeType.DescendantOrSelf)
					{
						if (nodeType == QilNodeType.Content)
						{
							this.CreateContainerIterator(ndDod, "$$$iterContent", typeof(ContentMergeIterator), XmlILMethods.ContentMergeCreate, XmlILMethods.ContentMergeNext, xmlNodeKindFlags, qilName, TriState.Unknown);
							return true;
						}
						switch (nodeType)
						{
						case QilNodeType.Descendant:
						case QilNodeType.DescendantOrSelf:
							this.CreateContainerIterator(ndDod, "$$$iterDesc", typeof(DescendantMergeIterator), XmlILMethods.DescMergeCreate, XmlILMethods.DescMergeNext, xmlNodeKindFlags, qilName, (qilNode.NodeType == QilNodeType.Descendant) ? TriState.False : TriState.True);
							return true;
						}
					}
					else
					{
						if (nodeType == QilNodeType.FollowingSibling)
						{
							this.CreateContainerIterator(ndDod, "$$$iterFollSib", typeof(FollowingSiblingMergeIterator), XmlILMethods.FollSibMergeCreate, XmlILMethods.FollSibMergeNext, xmlNodeKindFlags, qilName, TriState.Unknown);
							return true;
						}
						switch (nodeType)
						{
						case QilNodeType.XPathFollowing:
							this.CreateContainerIterator(ndDod, "$$$iterFoll", typeof(XPathFollowingMergeIterator), XmlILMethods.XPFollMergeCreate, XmlILMethods.XPFollMergeNext, xmlNodeKindFlags, qilName, TriState.Unknown);
							return true;
						case QilNodeType.XPathPreceding:
							this.CreateContainerIterator(ndDod, "$$$iterPrec", typeof(XPathPrecedingMergeIterator), XmlILMethods.XPPrecMergeCreate, XmlILMethods.XPPrecMergeNext, xmlNodeKindFlags, qilName, TriState.Unknown);
							return true;
						}
					}
				}
				else
				{
					QilNode qilNode2 = (QilNode)optimizerPatterns2.GetArgument(OptimizerPatternArgument.StepInput);
					QilNodeType nodeType2 = qilNode.NodeType;
					switch (nodeType2)
					{
					case QilNodeType.Ancestor:
					case QilNodeType.AncestorOrSelf:
						this.CreateFilteredIterator(qilNode2, "$$$iterAnc", typeof(AncestorDocOrderIterator), XmlILMethods.AncDOCreate, XmlILMethods.AncDONext, xmlNodeKindFlags, qilName, (qilNode.NodeType == QilNodeType.Ancestor) ? TriState.False : TriState.True, null);
						return true;
					case QilNodeType.Preceding:
					case QilNodeType.FollowingSibling:
						break;
					case QilNodeType.PrecedingSibling:
						this.CreateFilteredIterator(qilNode2, "$$$iterPreSib", typeof(PrecedingSiblingDocOrderIterator), XmlILMethods.PreSibDOCreate, XmlILMethods.PreSibDONext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
						return true;
					default:
						if (nodeType2 == QilNodeType.XPathPreceding)
						{
							this.CreateFilteredIterator(qilNode2, "$$$iterPrec", typeof(XPathPrecedingDocOrderIterator), XmlILMethods.XPPrecDOCreate, XmlILMethods.XPPrecDONext, xmlNodeKindFlags, qilName, TriState.Unknown, null);
							return true;
						}
						break;
					}
				}
			}
			else if (optimizerPatterns.MatchesPattern(OptimizerPatternName.DodMerge))
			{
				LocalBuilder localBuilder = this.helper.DeclareLocal("$$$dodMerge", typeof(DodSequenceMerge));
				Label label = this.helper.DefineLabel();
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
				this.helper.LoadQueryRuntime();
				this.helper.Call(XmlILMethods.DodMergeCreate);
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
				this.StartNestedIterator(ndDod.Child, label);
				this.Visit(ndDod.Child);
				this.iterCurr.EnsureStack();
				this.helper.Call(XmlILMethods.DodMergeAdd);
				this.helper.Emit(OpCodes.Ldloca, localBuilder);
				this.iterCurr.LoopToEnd(label);
				this.EndNestedIterator(ndDod.Child);
				this.helper.Call(XmlILMethods.DodMergeSeq);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathNavigator), true);
				return true;
			}
			return false;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00011DA0 File Offset: 0x00010DA0
		protected override QilNode VisitInvoke(QilInvoke ndInvoke)
		{
			QilFunction function = ndInvoke.Function;
			MethodInfo functionBinding = XmlILAnnotation.Write(function).FunctionBinding;
			bool flag = XmlILConstructInfo.Read(function).ConstructMethod == XmlILConstructMethod.Writer;
			this.helper.LoadQueryRuntime();
			for (int i = 0; i < ndInvoke.Arguments.Count; i++)
			{
				QilNode qilNode = ndInvoke.Arguments[i];
				QilNode qilNode2 = ndInvoke.Function.Arguments[i];
				this.NestedVisitEnsureStack(qilNode, this.GetItemStorageType(qilNode2), !qilNode2.XmlType.IsSingleton);
			}
			if (OptimizerPatterns.Read(ndInvoke).MatchesPattern(OptimizerPatternName.TailCall))
			{
				this.helper.TailCall(functionBinding);
			}
			else
			{
				this.helper.Call(functionBinding);
			}
			if (!flag)
			{
				this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(ndInvoke), !ndInvoke.XmlType.IsSingleton);
			}
			else
			{
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			return ndInvoke;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00011E94 File Offset: 0x00010E94
		protected override QilNode VisitContent(QilUnary ndContent)
		{
			this.CreateSimpleIterator(ndContent.Child, "$$$iterAttrContent", typeof(AttributeContentIterator), XmlILMethods.AttrContentCreate, XmlILMethods.AttrContentNext);
			return ndContent;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00011EBC File Offset: 0x00010EBC
		protected override QilNode VisitAttribute(QilBinary ndAttr)
		{
			QilName qilName = ndAttr.Right as QilName;
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$navAttr", typeof(XPathNavigator));
			this.SyncToNavigator(localBuilder, ndAttr.Left);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.CallGetAtomizedName(this.helper.StaticData.DeclareName(qilName.LocalName));
			this.helper.CallGetAtomizedName(this.helper.StaticData.DeclareName(qilName.NamespaceUri));
			this.helper.Call(XmlILMethods.NavMoveAttr);
			this.helper.Emit(OpCodes.Brfalse, this.iterCurr.GetLabelNext());
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, typeof(XPathNavigator), false);
			return ndAttr;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00011F98 File Offset: 0x00010F98
		protected override QilNode VisitParent(QilUnary ndParent)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$navParent", typeof(XPathNavigator));
			this.SyncToNavigator(localBuilder, ndParent.Child);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.Call(XmlILMethods.NavMoveParent);
			this.helper.Emit(OpCodes.Brfalse, this.iterCurr.GetLabelNext());
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, typeof(XPathNavigator), false);
			return ndParent;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00012028 File Offset: 0x00011028
		protected override QilNode VisitRoot(QilUnary ndRoot)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$navRoot", typeof(XPathNavigator));
			this.SyncToNavigator(localBuilder, ndRoot.Child);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.Call(XmlILMethods.NavMoveRoot);
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, typeof(XPathNavigator), false);
			return ndRoot;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0001209B File Offset: 0x0001109B
		protected override QilNode VisitXmlContext(QilNode ndCtxt)
		{
			this.helper.LoadQueryContext();
			this.helper.Call(XmlILMethods.GetDefaultDataSource);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathNavigator), false);
			return ndCtxt;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000120D4 File Offset: 0x000110D4
		protected override QilNode VisitDescendant(QilUnary ndDesc)
		{
			this.CreateFilteredIterator(ndDesc.Child, "$$$iterDesc", typeof(DescendantIterator), XmlILMethods.DescCreate, XmlILMethods.DescNext, XmlNodeKindFlags.Any, null, TriState.False, null);
			return ndDesc;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001210C File Offset: 0x0001110C
		protected override QilNode VisitDescendantOrSelf(QilUnary ndDesc)
		{
			this.CreateFilteredIterator(ndDesc.Child, "$$$iterDesc", typeof(DescendantIterator), XmlILMethods.DescCreate, XmlILMethods.DescNext, XmlNodeKindFlags.Any, null, TriState.True, null);
			return ndDesc;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00012144 File Offset: 0x00011144
		protected override QilNode VisitAncestor(QilUnary ndAnc)
		{
			this.CreateFilteredIterator(ndAnc.Child, "$$$iterAnc", typeof(AncestorIterator), XmlILMethods.AncCreate, XmlILMethods.AncNext, XmlNodeKindFlags.Any, null, TriState.False, null);
			return ndAnc;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001217C File Offset: 0x0001117C
		protected override QilNode VisitAncestorOrSelf(QilUnary ndAnc)
		{
			this.CreateFilteredIterator(ndAnc.Child, "$$$iterAnc", typeof(AncestorIterator), XmlILMethods.AncCreate, XmlILMethods.AncNext, XmlNodeKindFlags.Any, null, TriState.True, null);
			return ndAnc;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000121B4 File Offset: 0x000111B4
		protected override QilNode VisitPreceding(QilUnary ndPrec)
		{
			this.CreateFilteredIterator(ndPrec.Child, "$$$iterPrec", typeof(PrecedingIterator), XmlILMethods.PrecCreate, XmlILMethods.PrecNext, XmlNodeKindFlags.Any, null, TriState.Unknown, null);
			return ndPrec;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000121EC File Offset: 0x000111EC
		protected override QilNode VisitFollowingSibling(QilUnary ndFollSib)
		{
			this.CreateFilteredIterator(ndFollSib.Child, "$$$iterFollSib", typeof(FollowingSiblingIterator), XmlILMethods.FollSibCreate, XmlILMethods.FollSibNext, XmlNodeKindFlags.Any, null, TriState.Unknown, null);
			return ndFollSib;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00012224 File Offset: 0x00011224
		protected override QilNode VisitPrecedingSibling(QilUnary ndPreSib)
		{
			this.CreateFilteredIterator(ndPreSib.Child, "$$$iterPreSib", typeof(PrecedingSiblingIterator), XmlILMethods.PreSibCreate, XmlILMethods.PreSibNext, XmlNodeKindFlags.Any, null, TriState.Unknown, null);
			return ndPreSib;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001225C File Offset: 0x0001125C
		protected override QilNode VisitNodeRange(QilBinary ndRange)
		{
			this.CreateFilteredIterator(ndRange.Left, "$$$iterRange", typeof(NodeRangeIterator), XmlILMethods.NodeRangeCreate, XmlILMethods.NodeRangeNext, XmlNodeKindFlags.Any, null, TriState.Unknown, ndRange.Right);
			return ndRange;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0001229C File Offset: 0x0001129C
		protected override QilNode VisitDeref(QilBinary ndDeref)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$iterId", typeof(IdIterator));
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.NestedVisitEnsureStack(ndDeref.Left);
			this.NestedVisitEnsureStack(ndDeref.Right);
			this.helper.Call(XmlILMethods.IdCreate);
			this.GenerateSimpleIterator(typeof(XPathNavigator), localBuilder, XmlILMethods.IdNext);
			return ndDeref;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00012314 File Offset: 0x00011314
		protected override QilNode VisitElementCtor(QilBinary ndElem)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(ndElem);
			bool flag = this.CheckWithinContent(xmlILConstructInfo) || !xmlILConstructInfo.IsNamespaceInScope || this.ElementCachesAttributes(xmlILConstructInfo);
			if (XmlILConstructInfo.Read(ndElem.Right).FinalStates == PossibleXmlStates.Any)
			{
				flag = true;
			}
			if (xmlILConstructInfo.FinalStates == PossibleXmlStates.Any)
			{
				flag = true;
			}
			if (!flag)
			{
				this.BeforeStartChecks(ndElem);
			}
			GenerateNameType generateNameType = this.LoadNameAndType(XPathNodeType.Element, ndElem.Left, true, flag);
			this.helper.CallWriteStartElement(generateNameType, flag);
			this.NestedVisit(ndElem.Right);
			if (XmlILConstructInfo.Read(ndElem.Right).FinalStates == PossibleXmlStates.EnumAttrs && !flag)
			{
				this.helper.CallStartElementContent();
			}
			generateNameType = this.LoadNameAndType(XPathNodeType.Element, ndElem.Left, false, flag);
			this.helper.CallWriteEndElement(generateNameType, flag);
			if (!flag)
			{
				this.AfterEndChecks(ndElem);
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndElem;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000123F0 File Offset: 0x000113F0
		protected override QilNode VisitAttributeCtor(QilBinary ndAttr)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(ndAttr);
			bool flag = this.CheckEnumAttrs(xmlILConstructInfo) || !xmlILConstructInfo.IsNamespaceInScope;
			if (!flag)
			{
				this.BeforeStartChecks(ndAttr);
			}
			GenerateNameType generateNameType = this.LoadNameAndType(XPathNodeType.Attribute, ndAttr.Left, true, flag);
			this.helper.CallWriteStartAttribute(generateNameType, flag);
			this.NestedVisit(ndAttr.Right);
			this.helper.CallWriteEndAttribute(flag);
			if (!flag)
			{
				this.AfterEndChecks(ndAttr);
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndAttr;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00012474 File Offset: 0x00011474
		protected override QilNode VisitCommentCtor(QilUnary ndComment)
		{
			this.helper.CallWriteStartComment();
			this.NestedVisit(ndComment.Child);
			this.helper.CallWriteEndComment();
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndComment;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000124AC File Offset: 0x000114AC
		protected override QilNode VisitPICtor(QilBinary ndPI)
		{
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndPI.Left);
			this.helper.CallWriteStartPI();
			this.NestedVisit(ndPI.Right);
			this.helper.CallWriteEndPI();
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndPI;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00012503 File Offset: 0x00011503
		protected override QilNode VisitTextCtor(QilUnary ndText)
		{
			return this.VisitTextCtor(ndText, false);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001250D File Offset: 0x0001150D
		protected override QilNode VisitRawTextCtor(QilUnary ndText)
		{
			return this.VisitTextCtor(ndText, true);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00012518 File Offset: 0x00011518
		private QilNode VisitTextCtor(QilUnary ndText, bool disableOutputEscaping)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(ndText);
			bool flag;
			switch (xmlILConstructInfo.InitialStates)
			{
			case PossibleXmlStates.WithinAttr:
			case PossibleXmlStates.WithinComment:
			case PossibleXmlStates.WithinPI:
				flag = false;
				break;
			default:
				flag = this.CheckWithinContent(xmlILConstructInfo);
				break;
			}
			if (!flag)
			{
				this.BeforeStartChecks(ndText);
			}
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndText.Child);
			switch (xmlILConstructInfo.InitialStates)
			{
			case PossibleXmlStates.WithinAttr:
				this.helper.CallWriteString(false, flag);
				break;
			case PossibleXmlStates.WithinComment:
				this.helper.Call(XmlILMethods.CommentText);
				break;
			case PossibleXmlStates.WithinPI:
				this.helper.Call(XmlILMethods.PIText);
				break;
			default:
				this.helper.CallWriteString(disableOutputEscaping, flag);
				break;
			}
			if (!flag)
			{
				this.AfterEndChecks(ndText);
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndText;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000125EE File Offset: 0x000115EE
		protected override QilNode VisitDocumentCtor(QilUnary ndDoc)
		{
			this.helper.CallWriteStartRoot();
			this.NestedVisit(ndDoc.Child);
			this.helper.CallWriteEndRoot();
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndDoc;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00012624 File Offset: 0x00011624
		protected override QilNode VisitNamespaceDecl(QilBinary ndNmsp)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(ndNmsp);
			bool flag = this.CheckEnumAttrs(xmlILConstructInfo) || this.MightHaveNamespacesAfterAttributes(xmlILConstructInfo);
			if (!flag)
			{
				this.BeforeStartChecks(ndNmsp);
			}
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndNmsp.Left);
			this.NestedVisitEnsureStack(ndNmsp.Right);
			this.helper.CallWriteNamespaceDecl(flag);
			if (!flag)
			{
				this.AfterEndChecks(ndNmsp);
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndNmsp;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000126A0 File Offset: 0x000116A0
		protected override QilNode VisitRtfCtor(QilBinary ndRtf)
		{
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndRtf);
			string text = (QilLiteral)ndRtf.Right;
			if (optimizerPatterns.MatchesPattern(OptimizerPatternName.SingleTextRtf))
			{
				this.helper.LoadQueryRuntime();
				this.NestedVisitEnsureStack((QilNode)optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
				this.helper.Emit(OpCodes.Ldstr, text);
				this.helper.Call(XmlILMethods.RtfConstr);
			}
			else
			{
				this.helper.CallStartRtfConstruction(text);
				this.NestedVisit(ndRtf.Left);
				this.helper.CallEndRtfConstruction();
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathNavigator), false);
			return ndRtf;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001274E File Offset: 0x0001174E
		protected override QilNode VisitNameOf(QilUnary ndName)
		{
			return this.VisitNodeProperty(ndName);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00012757 File Offset: 0x00011757
		protected override QilNode VisitLocalNameOf(QilUnary ndName)
		{
			return this.VisitNodeProperty(ndName);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00012760 File Offset: 0x00011760
		protected override QilNode VisitNamespaceUriOf(QilUnary ndName)
		{
			return this.VisitNodeProperty(ndName);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00012769 File Offset: 0x00011769
		protected override QilNode VisitPrefixOf(QilUnary ndName)
		{
			return this.VisitNodeProperty(ndName);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00012774 File Offset: 0x00011774
		private QilNode VisitNodeProperty(QilUnary ndProp)
		{
			this.NestedVisitEnsureStack(ndProp.Child);
			switch (ndProp.NodeType)
			{
			case QilNodeType.NameOf:
				this.helper.Emit(OpCodes.Dup);
				this.helper.Call(XmlILMethods.NavLocalName);
				this.helper.Call(XmlILMethods.NavNmsp);
				this.helper.Construct(XmlILConstructors.QName);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XmlQualifiedName), false);
				break;
			case QilNodeType.LocalNameOf:
				this.helper.Call(XmlILMethods.NavLocalName);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
				break;
			case QilNodeType.NamespaceUriOf:
				this.helper.Call(XmlILMethods.NavNmsp);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
				break;
			case QilNodeType.PrefixOf:
				this.helper.Call(XmlILMethods.NavPrefix);
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
				break;
			}
			return ndProp;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00012898 File Offset: 0x00011898
		protected override QilNode VisitTypeAssert(QilTargetType ndTypeAssert)
		{
			if (!ndTypeAssert.Source.XmlType.IsSingleton && ndTypeAssert.XmlType.IsSingleton && !this.iterCurr.HasLabelNext)
			{
				Label label = this.helper.DefineLabel();
				this.helper.MarkLabel(label);
				this.NestedVisit(ndTypeAssert.Source, label);
			}
			else
			{
				this.Visit(ndTypeAssert.Source);
			}
			this.iterCurr.EnsureItemStorageType(ndTypeAssert.Source.XmlType, this.GetItemStorageType(ndTypeAssert));
			return ndTypeAssert;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00012924 File Offset: 0x00011924
		protected override QilNode VisitIsType(QilTargetType ndIsType)
		{
			XmlQueryType xmlType = ndIsType.Source.XmlType;
			XmlQueryType targetType = ndIsType.TargetType;
			if (xmlType.IsSingleton && object.Equals(targetType, XmlQueryTypeFactory.Node))
			{
				this.NestedVisitEnsureStack(ndIsType.Source);
				this.helper.Call(XmlILMethods.ItemIsNode);
				this.ZeroCompare(QilNodeType.Ne, true);
				return ndIsType;
			}
			if (this.MatchesNodeKinds(ndIsType, xmlType, targetType))
			{
				return ndIsType;
			}
			XmlTypeCode xmlTypeCode;
			if (object.Equals(targetType, XmlQueryTypeFactory.Double))
			{
				xmlTypeCode = XmlTypeCode.Double;
			}
			else if (object.Equals(targetType, XmlQueryTypeFactory.String))
			{
				xmlTypeCode = XmlTypeCode.String;
			}
			else if (object.Equals(targetType, XmlQueryTypeFactory.Boolean))
			{
				xmlTypeCode = XmlTypeCode.Boolean;
			}
			else if (object.Equals(targetType, XmlQueryTypeFactory.Node))
			{
				xmlTypeCode = XmlTypeCode.Node;
			}
			else
			{
				xmlTypeCode = XmlTypeCode.None;
			}
			if (xmlTypeCode != XmlTypeCode.None)
			{
				this.helper.LoadQueryRuntime();
				this.NestedVisitEnsureStack(ndIsType.Source, typeof(XPathItem), !xmlType.IsSingleton);
				this.helper.LoadInteger((int)xmlTypeCode);
				this.helper.Call(xmlType.IsSingleton ? XmlILMethods.ItemMatchesCode : XmlILMethods.SeqMatchesCode);
				this.ZeroCompare(QilNodeType.Ne, true);
				return ndIsType;
			}
			this.helper.LoadQueryRuntime();
			this.NestedVisitEnsureStack(ndIsType.Source, typeof(XPathItem), !xmlType.IsSingleton);
			this.helper.LoadInteger(this.helper.StaticData.DeclareXmlType(targetType));
			this.helper.Call(xmlType.IsSingleton ? XmlILMethods.ItemMatchesType : XmlILMethods.SeqMatchesType);
			this.ZeroCompare(QilNodeType.Ne, true);
			return ndIsType;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00012AA8 File Offset: 0x00011AA8
		private bool MatchesNodeKinds(QilTargetType ndIsType, XmlQueryType typDerived, XmlQueryType typBase)
		{
			bool flag = true;
			if (!typBase.IsNode || !typBase.IsSingleton)
			{
				return false;
			}
			if (!typDerived.IsNode || !typDerived.IsSingleton || !typDerived.IsNotRtf)
			{
				return false;
			}
			XmlNodeKindFlags xmlNodeKindFlags = XmlNodeKindFlags.None;
			foreach (XmlQueryType xmlQueryType in typBase)
			{
				if (object.Equals(xmlQueryType, XmlQueryTypeFactory.Element))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.Element;
				}
				else if (object.Equals(xmlQueryType, XmlQueryTypeFactory.Attribute))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.Attribute;
				}
				else if (object.Equals(xmlQueryType, XmlQueryTypeFactory.Text))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.Text;
				}
				else if (object.Equals(xmlQueryType, XmlQueryTypeFactory.Document))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.Document;
				}
				else if (object.Equals(xmlQueryType, XmlQueryTypeFactory.Comment))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.Comment;
				}
				else if (object.Equals(xmlQueryType, XmlQueryTypeFactory.PI))
				{
					xmlNodeKindFlags |= XmlNodeKindFlags.PI;
				}
				else
				{
					if (!object.Equals(xmlQueryType, XmlQueryTypeFactory.Namespace))
					{
						return false;
					}
					xmlNodeKindFlags |= XmlNodeKindFlags.Namespace;
				}
			}
			xmlNodeKindFlags = typDerived.NodeKinds & xmlNodeKindFlags;
			if (!Bits.ExactlyOne((uint)xmlNodeKindFlags))
			{
				xmlNodeKindFlags = ~xmlNodeKindFlags & XmlNodeKindFlags.Any;
				flag = !flag;
			}
			XmlNodeKindFlags xmlNodeKindFlags2 = xmlNodeKindFlags;
			XPathNodeType xpathNodeType;
			if (xmlNodeKindFlags2 <= XmlNodeKindFlags.Comment)
			{
				switch (xmlNodeKindFlags2)
				{
				case XmlNodeKindFlags.Document:
					xpathNodeType = XPathNodeType.Root;
					goto IL_017B;
				case XmlNodeKindFlags.Element:
					xpathNodeType = XPathNodeType.Element;
					goto IL_017B;
				case XmlNodeKindFlags.Document | XmlNodeKindFlags.Element:
					break;
				case XmlNodeKindFlags.Attribute:
					xpathNodeType = XPathNodeType.Attribute;
					goto IL_017B;
				default:
					if (xmlNodeKindFlags2 == XmlNodeKindFlags.Comment)
					{
						xpathNodeType = XPathNodeType.Comment;
						goto IL_017B;
					}
					break;
				}
			}
			else
			{
				if (xmlNodeKindFlags2 == XmlNodeKindFlags.PI)
				{
					xpathNodeType = XPathNodeType.ProcessingInstruction;
					goto IL_017B;
				}
				if (xmlNodeKindFlags2 == XmlNodeKindFlags.Namespace)
				{
					xpathNodeType = XPathNodeType.Namespace;
					goto IL_017B;
				}
			}
			this.helper.Emit(OpCodes.Ldc_I4_1);
			xpathNodeType = XPathNodeType.All;
			IL_017B:
			this.NestedVisitEnsureStack(ndIsType.Source);
			this.helper.Call(XmlILMethods.NavType);
			if (xpathNodeType == XPathNodeType.All)
			{
				this.helper.Emit(OpCodes.Shl);
				int num = 0;
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Document) != XmlNodeKindFlags.None)
				{
					num |= 1;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Element) != XmlNodeKindFlags.None)
				{
					num |= 2;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Attribute) != XmlNodeKindFlags.None)
				{
					num |= 4;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Text) != XmlNodeKindFlags.None)
				{
					num |= 112;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Comment) != XmlNodeKindFlags.None)
				{
					num |= 256;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.PI) != XmlNodeKindFlags.None)
				{
					num |= 128;
				}
				if ((xmlNodeKindFlags & XmlNodeKindFlags.Namespace) != XmlNodeKindFlags.None)
				{
					num |= 8;
				}
				this.helper.LoadInteger(num);
				this.helper.Emit(OpCodes.And);
				this.ZeroCompare(flag ? QilNodeType.Ne : QilNodeType.Eq, false);
			}
			else
			{
				this.helper.LoadInteger((int)xpathNodeType);
				this.ClrCompare(flag ? QilNodeType.Eq : QilNodeType.Ne, XmlTypeCode.Int);
			}
			return true;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00012D10 File Offset: 0x00011D10
		protected override QilNode VisitIsEmpty(QilUnary ndIsEmpty)
		{
			if (this.CachesResult(ndIsEmpty.Child))
			{
				this.NestedVisitEnsureStack(ndIsEmpty.Child);
				this.helper.CallCacheCount(this.iterNested.Storage.ItemStorageType);
				switch (this.iterCurr.CurrentBranchingContext)
				{
				case BranchingContext.OnTrue:
					this.helper.TestAndBranch(0, this.iterCurr.LabelBranch, OpCodes.Beq);
					break;
				case BranchingContext.OnFalse:
					this.helper.TestAndBranch(0, this.iterCurr.LabelBranch, OpCodes.Bne_Un);
					break;
				default:
				{
					Label label = this.helper.DefineLabel();
					this.helper.Emit(OpCodes.Brfalse_S, label);
					this.helper.ConvBranchToBool(label, true);
					break;
				}
				}
			}
			else
			{
				Label label2 = this.helper.DefineLabel();
				IteratorDescriptor iteratorDescriptor = this.iterCurr;
				if (iteratorDescriptor.CurrentBranchingContext == BranchingContext.OnTrue)
				{
					this.StartNestedIterator(ndIsEmpty.Child, this.iterCurr.LabelBranch);
				}
				else
				{
					this.StartNestedIterator(ndIsEmpty.Child, label2);
				}
				this.Visit(ndIsEmpty.Child);
				this.iterCurr.EnsureNoCache();
				this.iterCurr.DiscardStack();
				switch (iteratorDescriptor.CurrentBranchingContext)
				{
				case BranchingContext.None:
					this.helper.ConvBranchToBool(label2, true);
					break;
				case BranchingContext.OnFalse:
					this.helper.EmitUnconditionalBranch(OpCodes.Br, iteratorDescriptor.LabelBranch);
					this.helper.MarkLabel(label2);
					break;
				}
				this.EndNestedIterator(ndIsEmpty.Child);
			}
			if (this.iterCurr.IsBranching)
			{
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			else
			{
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
			}
			return ndIsEmpty;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00012EE4 File Offset: 0x00011EE4
		protected override QilNode VisitXPathNodeValue(QilUnary ndVal)
		{
			if (ndVal.Child.XmlType.IsSingleton)
			{
				this.NestedVisitEnsureStack(ndVal.Child, typeof(XPathNavigator), false);
				this.helper.Call(XmlILMethods.Value);
			}
			else
			{
				Label label = this.helper.DefineLabel();
				this.StartNestedIterator(ndVal.Child, label);
				this.Visit(ndVal.Child);
				this.iterCurr.EnsureStackNoCache();
				this.helper.Call(XmlILMethods.Value);
				Label label2 = this.helper.DefineLabel();
				this.helper.EmitUnconditionalBranch(OpCodes.Br, label2);
				this.helper.MarkLabel(label);
				this.helper.Emit(OpCodes.Ldstr, "");
				this.helper.MarkLabel(label2);
				this.EndNestedIterator(ndVal.Child);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
			return ndVal;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00012FE4 File Offset: 0x00011FE4
		protected override QilNode VisitXPathFollowing(QilUnary ndFoll)
		{
			this.CreateFilteredIterator(ndFoll.Child, "$$$iterFoll", typeof(XPathFollowingIterator), XmlILMethods.XPFollCreate, XmlILMethods.XPFollNext, XmlNodeKindFlags.Any, null, TriState.Unknown, null);
			return ndFoll;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0001301C File Offset: 0x0001201C
		protected override QilNode VisitXPathPreceding(QilUnary ndPrec)
		{
			this.CreateFilteredIterator(ndPrec.Child, "$$$iterPrec", typeof(XPathPrecedingIterator), XmlILMethods.XPPrecCreate, XmlILMethods.XPPrecNext, XmlNodeKindFlags.Any, null, TriState.Unknown, null);
			return ndPrec;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00013054 File Offset: 0x00012054
		protected override QilNode VisitXPathNamespace(QilUnary ndNmsp)
		{
			this.CreateSimpleIterator(ndNmsp.Child, "$$$iterNmsp", typeof(NamespaceIterator), XmlILMethods.NmspCreate, XmlILMethods.NmspNext);
			return ndNmsp;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001307C File Offset: 0x0001207C
		protected override QilNode VisitXsltGenerateId(QilUnary ndGenId)
		{
			this.helper.LoadQueryRuntime();
			if (ndGenId.Child.XmlType.IsSingleton)
			{
				this.NestedVisitEnsureStack(ndGenId.Child, typeof(XPathNavigator), false);
				this.helper.Call(XmlILMethods.GenId);
			}
			else
			{
				Label label = this.helper.DefineLabel();
				this.StartNestedIterator(ndGenId.Child, label);
				this.Visit(ndGenId.Child);
				this.iterCurr.EnsureStackNoCache();
				this.iterCurr.EnsureItemStorageType(ndGenId.Child.XmlType, typeof(XPathNavigator));
				this.helper.Call(XmlILMethods.GenId);
				Label label2 = this.helper.DefineLabel();
				this.helper.EmitUnconditionalBranch(OpCodes.Br, label2);
				this.helper.MarkLabel(label);
				this.helper.Emit(OpCodes.Pop);
				this.helper.Emit(OpCodes.Ldstr, "");
				this.helper.MarkLabel(label2);
				this.EndNestedIterator(ndGenId.Child);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(string), false);
			return ndGenId;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000131B8 File Offset: 0x000121B8
		protected override QilNode VisitXsltInvokeLateBound(QilInvokeLateBound ndInvoke)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$args", typeof(IList<XPathItem>[]));
			QilName name = ndInvoke.Name;
			this.helper.LoadQueryContext();
			this.helper.Emit(OpCodes.Ldstr, name.LocalName);
			this.helper.Emit(OpCodes.Ldstr, name.NamespaceUri);
			this.helper.LoadInteger(ndInvoke.Arguments.Count);
			this.helper.Emit(OpCodes.Newarr, typeof(IList<XPathItem>));
			this.helper.Emit(OpCodes.Stloc, localBuilder);
			for (int i = 0; i < ndInvoke.Arguments.Count; i++)
			{
				QilNode qilNode = ndInvoke.Arguments[i];
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.helper.LoadInteger(i);
				this.helper.Emit(OpCodes.Ldelema, typeof(IList<XPathItem>));
				this.NestedVisitEnsureCache(qilNode, typeof(XPathItem));
				this.iterCurr.EnsureStack();
				this.helper.Emit(OpCodes.Stobj, typeof(IList<XPathItem>));
			}
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			this.helper.Call(XmlILMethods.InvokeXsltLate);
			this.iterCurr.Storage = StorageDescriptor.Stack(typeof(XPathItem), true);
			return ndInvoke;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00013330 File Offset: 0x00012330
		protected override QilNode VisitXsltInvokeEarlyBound(QilInvokeEarlyBound ndInvoke)
		{
			QilName name = ndInvoke.Name;
			XmlExtensionFunction xmlExtensionFunction = new XmlExtensionFunction(name.LocalName, name.NamespaceUri, ndInvoke.ClrMethod);
			Type clrReturnType = xmlExtensionFunction.ClrReturnType;
			Type storageType = this.GetStorageType(ndInvoke);
			if (clrReturnType != storageType && !ndInvoke.XmlType.IsEmpty)
			{
				this.helper.LoadQueryRuntime();
				this.helper.LoadInteger(this.helper.StaticData.DeclareXmlType(ndInvoke.XmlType));
			}
			if (!xmlExtensionFunction.Method.IsStatic)
			{
				if (name.NamespaceUri.Length == 0)
				{
					this.helper.LoadXsltLibrary();
				}
				else
				{
					this.helper.CallGetEarlyBoundObject(this.helper.StaticData.DeclareEarlyBound(name.NamespaceUri, xmlExtensionFunction.Method.DeclaringType), xmlExtensionFunction.Method.DeclaringType);
				}
			}
			for (int i = 0; i < ndInvoke.Arguments.Count; i++)
			{
				QilNode qilNode = ndInvoke.Arguments[i];
				XmlQueryType xmlArgumentType = xmlExtensionFunction.GetXmlArgumentType(i);
				Type clrArgumentType = xmlExtensionFunction.GetClrArgumentType(i);
				if (name.NamespaceUri.Length == 0)
				{
					Type itemStorageType = this.GetItemStorageType(qilNode);
					if (clrArgumentType == XmlILMethods.StorageMethods[itemStorageType].IListType)
					{
						this.NestedVisitEnsureStack(qilNode, itemStorageType, true);
					}
					else if (clrArgumentType == XmlILMethods.StorageMethods[typeof(XPathItem)].IListType)
					{
						this.NestedVisitEnsureStack(qilNode, typeof(XPathItem), true);
					}
					else if ((qilNode.XmlType.IsSingleton && clrArgumentType == itemStorageType) || qilNode.XmlType.TypeCode == XmlTypeCode.None)
					{
						this.NestedVisitEnsureStack(qilNode, clrArgumentType, false);
					}
					else if (qilNode.XmlType.IsSingleton && clrArgumentType == typeof(XPathItem))
					{
						this.NestedVisitEnsureStack(qilNode, typeof(XPathItem), false);
					}
				}
				else
				{
					Type storageType2 = this.GetStorageType(xmlArgumentType);
					if (xmlArgumentType.TypeCode == XmlTypeCode.Item || !clrArgumentType.IsAssignableFrom(storageType2))
					{
						this.helper.LoadQueryRuntime();
						this.helper.LoadInteger(this.helper.StaticData.DeclareXmlType(xmlArgumentType));
						this.NestedVisitEnsureStack(qilNode, this.GetItemStorageType(xmlArgumentType), !xmlArgumentType.IsSingleton);
						this.helper.TreatAs(storageType2, typeof(object));
						this.helper.LoadType(clrArgumentType);
						this.helper.Call(XmlILMethods.ChangeTypeXsltArg);
						this.helper.TreatAs(typeof(object), clrArgumentType);
					}
					else
					{
						this.NestedVisitEnsureStack(qilNode, this.GetItemStorageType(xmlArgumentType), !xmlArgumentType.IsSingleton);
					}
				}
			}
			this.helper.Call(xmlExtensionFunction.Method);
			if (ndInvoke.XmlType.IsEmpty)
			{
				this.helper.Emit(OpCodes.Ldsfld, XmlILMethods.StorageMethods[typeof(XPathItem)].SeqEmpty);
			}
			else if (clrReturnType != storageType)
			{
				this.helper.TreatAs(clrReturnType, typeof(object));
				this.helper.Call(XmlILMethods.ChangeTypeXsltResult);
				this.helper.TreatAs(typeof(object), storageType);
			}
			else if (name.NamespaceUri.Length != 0 && !clrReturnType.IsValueType)
			{
				Label label = this.helper.DefineLabel();
				this.helper.Emit(OpCodes.Dup);
				this.helper.Emit(OpCodes.Brtrue, label);
				this.helper.LoadQueryRuntime();
				this.helper.Emit(OpCodes.Ldstr, Res.GetString("Xslt_ItemNull"));
				this.helper.Call(XmlILMethods.ThrowException);
				this.helper.MarkLabel(label);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(ndInvoke), !ndInvoke.XmlType.IsSingleton);
			return ndInvoke;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00013734 File Offset: 0x00012734
		protected override QilNode VisitXsltCopy(QilBinary ndCopy)
		{
			Label label = this.helper.DefineLabel();
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndCopy.Left);
			this.helper.Call(XmlILMethods.StartCopy);
			this.helper.Emit(OpCodes.Brfalse, label);
			this.NestedVisit(ndCopy.Right);
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndCopy.Left);
			this.helper.Call(XmlILMethods.EndCopy);
			this.helper.MarkLabel(label);
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndCopy;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000137D5 File Offset: 0x000127D5
		protected override QilNode VisitXsltCopyOf(QilUnary ndCopyOf)
		{
			this.helper.LoadQueryOutput();
			this.NestedVisitEnsureStack(ndCopyOf.Child);
			this.helper.Call(XmlILMethods.CopyOf);
			this.iterCurr.Storage = StorageDescriptor.None();
			return ndCopyOf;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013810 File Offset: 0x00012810
		protected override QilNode VisitXsltConvert(QilTargetType ndConv)
		{
			XmlQueryType xmlType = ndConv.Source.XmlType;
			XmlQueryType targetType = ndConv.TargetType;
			MethodInfo methodInfo;
			if (this.GetXsltConvertMethod(xmlType, targetType, out methodInfo))
			{
				this.NestedVisitEnsureStack(ndConv.Source);
			}
			else
			{
				this.NestedVisitEnsureStack(ndConv.Source, typeof(XPathItem), !xmlType.IsSingleton);
				this.GetXsltConvertMethod(xmlType.IsSingleton ? XmlQueryTypeFactory.Item : XmlQueryTypeFactory.ItemS, targetType, out methodInfo);
			}
			if (methodInfo != null)
			{
				this.helper.Call(methodInfo);
			}
			this.iterCurr.Storage = StorageDescriptor.Stack(this.GetItemStorageType(targetType), !targetType.IsSingleton);
			return ndConv;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000138B8 File Offset: 0x000128B8
		private bool GetXsltConvertMethod(XmlQueryType typSrc, XmlQueryType typDst, out MethodInfo meth)
		{
			meth = null;
			if (object.Equals(typDst, XmlQueryTypeFactory.BooleanX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.Item))
				{
					meth = XmlILMethods.ItemToBool;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.ItemS))
				{
					meth = XmlILMethods.ItemsToBool;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.DateTimeX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.StringX))
				{
					meth = XmlILMethods.StrToDT;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.DecimalX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.DoubleX))
				{
					meth = XmlILMethods.DblToDec;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.DoubleX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.DecimalX))
				{
					meth = XmlILMethods.DecToDbl;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.IntX))
				{
					meth = XmlILMethods.IntToDbl;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.Item))
				{
					meth = XmlILMethods.ItemToDbl;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.ItemS))
				{
					meth = XmlILMethods.ItemsToDbl;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.LongX))
				{
					meth = XmlILMethods.LngToDbl;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.StringX))
				{
					meth = XmlILMethods.StrToDbl;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.IntX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.DoubleX))
				{
					meth = XmlILMethods.DblToInt;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.LongX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.DoubleX))
				{
					meth = XmlILMethods.DblToLng;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.NodeNotRtf))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.Item))
				{
					meth = XmlILMethods.ItemToNode;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.ItemS))
				{
					meth = XmlILMethods.ItemsToNode;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.NodeDodS) || object.Equals(typDst, XmlQueryTypeFactory.NodeNotRtfS))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.Item))
				{
					meth = XmlILMethods.ItemToNodes;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.ItemS))
				{
					meth = XmlILMethods.ItemsToNodes;
				}
			}
			else if (object.Equals(typDst, XmlQueryTypeFactory.StringX))
			{
				if (object.Equals(typSrc, XmlQueryTypeFactory.DateTimeX))
				{
					meth = XmlILMethods.DTToStr;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.DoubleX))
				{
					meth = XmlILMethods.DblToStr;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.Item))
				{
					meth = XmlILMethods.ItemToStr;
				}
				else if (object.Equals(typSrc, XmlQueryTypeFactory.ItemS))
				{
					meth = XmlILMethods.ItemsToStr;
				}
			}
			return meth != null;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00013B49 File Offset: 0x00012B49
		private void SyncToNavigator(LocalBuilder locNav, QilNode ndCtxt)
		{
			this.helper.Emit(OpCodes.Ldloc, locNav);
			this.NestedVisitEnsureStack(ndCtxt);
			this.helper.CallSyncToNavigator();
			this.helper.Emit(OpCodes.Stloc, locNav);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00013B80 File Offset: 0x00012B80
		private void CreateSimpleIterator(QilNode ndCtxt, string iterName, Type iterType, MethodInfo methCreate, MethodInfo methNext)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal(iterName, iterType);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.NestedVisitEnsureStack(ndCtxt);
			this.helper.Call(methCreate);
			this.GenerateSimpleIterator(typeof(XPathNavigator), localBuilder, methNext);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00013BD4 File Offset: 0x00012BD4
		private void CreateFilteredIterator(QilNode ndCtxt, string iterName, Type iterType, MethodInfo methCreate, MethodInfo methNext, XmlNodeKindFlags kinds, QilName ndName, TriState orSelf, QilNode ndEnd)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal(iterName, iterType);
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.NestedVisitEnsureStack(ndCtxt);
			this.LoadSelectFilter(kinds, ndName);
			if (orSelf != TriState.Unknown)
			{
				this.helper.LoadBoolean(orSelf == TriState.True);
			}
			if (ndEnd != null)
			{
				this.NestedVisitEnsureStack(ndEnd);
			}
			this.helper.Call(methCreate);
			this.GenerateSimpleIterator(typeof(XPathNavigator), localBuilder, methNext);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00013C54 File Offset: 0x00012C54
		private void CreateContainerIterator(QilUnary ndDod, string iterName, Type iterType, MethodInfo methCreate, MethodInfo methNext, XmlNodeKindFlags kinds, QilName ndName, TriState orSelf)
		{
			LocalBuilder localBuilder = this.helper.DeclareLocal(iterName, iterType);
			QilLoop qilLoop = (QilLoop)ndDod.Child;
			this.helper.Emit(OpCodes.Ldloca, localBuilder);
			this.LoadSelectFilter(kinds, ndName);
			if (orSelf != TriState.Unknown)
			{
				this.helper.LoadBoolean(orSelf == TriState.True);
			}
			this.helper.Call(methCreate);
			Label label = this.helper.DefineLabel();
			this.StartNestedIterator(qilLoop, label);
			this.StartBinding(qilLoop.Variable);
			this.EndBinding(qilLoop.Variable);
			this.EndNestedIterator(qilLoop.Variable);
			this.iterCurr.Storage = this.iterNested.Storage;
			this.GenerateContainerIterator(ndDod, localBuilder, label, methNext, typeof(XPathNavigator));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00013D1C File Offset: 0x00012D1C
		private void GenerateSimpleIterator(Type itemStorageType, LocalBuilder locIter, MethodInfo methNext)
		{
			Label label = this.helper.DefineLabel();
			this.helper.MarkLabel(label);
			this.helper.Emit(OpCodes.Ldloca, locIter);
			this.helper.Call(methNext);
			this.helper.Emit(OpCodes.Brfalse, this.iterCurr.GetLabelNext());
			this.iterCurr.SetIterator(label, StorageDescriptor.Current(locIter, itemStorageType));
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00013D8C File Offset: 0x00012D8C
		private void GenerateContainerIterator(QilNode nd, LocalBuilder locIter, Label lblOnEndNested, MethodInfo methNext, Type itemStorageType)
		{
			Label label = this.helper.DefineLabel();
			this.iterCurr.EnsureNoStackNoCache(nd.XmlType.IsNode ? "$$$navInput" : "$$$itemInput");
			this.helper.Emit(OpCodes.Ldloca, locIter);
			this.iterCurr.PushValue();
			this.helper.EmitUnconditionalBranch(OpCodes.Br, label);
			this.helper.MarkLabel(lblOnEndNested);
			this.helper.Emit(OpCodes.Ldloca, locIter);
			this.helper.Emit(OpCodes.Ldnull);
			this.helper.MarkLabel(label);
			this.helper.Call(methNext);
			if (nd.XmlType.IsSingleton)
			{
				this.helper.LoadInteger(1);
				this.helper.Emit(OpCodes.Beq, this.iterNested.GetLabelNext());
				this.iterCurr.Storage = StorageDescriptor.Current(locIter, itemStorageType);
				return;
			}
			this.helper.Emit(OpCodes.Switch, new Label[]
			{
				this.iterCurr.GetLabelNext(),
				this.iterNested.GetLabelNext()
			});
			this.iterCurr.SetIterator(lblOnEndNested, StorageDescriptor.Current(locIter, itemStorageType));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00013EE0 File Offset: 0x00012EE0
		private GenerateNameType LoadNameAndType(XPathNodeType nodeType, QilNode ndName, bool isStart, bool callChk)
		{
			this.helper.LoadQueryOutput();
			GenerateNameType generateNameType = GenerateNameType.StackName;
			if (ndName.NodeType == QilNodeType.LiteralQName)
			{
				if (isStart || !callChk)
				{
					QilName qilName = ndName as QilName;
					string prefix = qilName.Prefix;
					string localName = qilName.LocalName;
					string namespaceUri = qilName.NamespaceUri;
					if (qilName.NamespaceUri.Length == 0)
					{
						this.helper.Emit(OpCodes.Ldstr, qilName.LocalName);
						return GenerateNameType.LiteralLocalName;
					}
					if (!ValidateNames.ValidateName(prefix, localName, namespaceUri, nodeType, ValidateNames.Flags.CheckPrefixMapping))
					{
						if (isStart)
						{
							this.helper.Emit(OpCodes.Ldstr, localName);
							this.helper.Emit(OpCodes.Ldstr, namespaceUri);
							this.helper.Construct(XmlILConstructors.QName);
							generateNameType = GenerateNameType.QName;
						}
					}
					else
					{
						this.helper.Emit(OpCodes.Ldstr, prefix);
						this.helper.Emit(OpCodes.Ldstr, localName);
						this.helper.Emit(OpCodes.Ldstr, namespaceUri);
						generateNameType = GenerateNameType.LiteralName;
					}
				}
			}
			else if (isStart)
			{
				if (ndName.NodeType == QilNodeType.NameOf)
				{
					this.NestedVisitEnsureStack((ndName as QilUnary).Child);
					generateNameType = GenerateNameType.CopiedName;
				}
				else if (ndName.NodeType == QilNodeType.StrParseQName)
				{
					this.VisitStrParseQName(ndName as QilBinary, true);
					if ((ndName as QilBinary).Right.XmlType.TypeCode == XmlTypeCode.String)
					{
						generateNameType = GenerateNameType.TagNameAndNamespace;
					}
					else
					{
						generateNameType = GenerateNameType.TagNameAndMappings;
					}
				}
				else
				{
					this.NestedVisitEnsureStack(ndName);
					generateNameType = GenerateNameType.QName;
				}
			}
			return generateNameType;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00014040 File Offset: 0x00013040
		private bool TryZeroCompare(QilNodeType relOp, QilNode ndFirst, QilNode ndSecond)
		{
			switch (ndFirst.NodeType)
			{
			case QilNodeType.True:
				relOp = ((relOp == QilNodeType.Eq) ? QilNodeType.Ne : QilNodeType.Eq);
				goto IL_0055;
			case QilNodeType.False:
				goto IL_0055;
			case QilNodeType.LiteralInt32:
				if ((QilLiteral)ndFirst != 0)
				{
					return false;
				}
				goto IL_0055;
			case QilNodeType.LiteralInt64:
				if ((QilLiteral)ndFirst != 0)
				{
					return false;
				}
				goto IL_0055;
			}
			return false;
			IL_0055:
			this.NestedVisitEnsureStack(ndSecond);
			this.ZeroCompare(relOp, ndSecond.XmlType.TypeCode == XmlTypeCode.Boolean);
			return true;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000140C0 File Offset: 0x000130C0
		private bool TryNameCompare(QilNodeType relOp, QilNode ndFirst, QilNode ndSecond)
		{
			if (ndFirst.NodeType == QilNodeType.NameOf)
			{
				QilNodeType nodeType = ndSecond.NodeType;
				if (nodeType == QilNodeType.LiteralQName || nodeType == QilNodeType.NameOf)
				{
					this.helper.LoadQueryRuntime();
					this.NestedVisitEnsureStack((ndFirst as QilUnary).Child);
					if (ndSecond.NodeType == QilNodeType.LiteralQName)
					{
						QilName qilName = ndSecond as QilName;
						this.helper.LoadInteger(this.helper.StaticData.DeclareName(qilName.LocalName));
						this.helper.LoadInteger(this.helper.StaticData.DeclareName(qilName.NamespaceUri));
						this.helper.Call(XmlILMethods.QNameEqualLit);
					}
					else
					{
						this.NestedVisitEnsureStack(ndSecond);
						this.helper.Call(XmlILMethods.QNameEqualNav);
					}
					this.ZeroCompare((relOp == QilNodeType.Eq) ? QilNodeType.Ne : QilNodeType.Eq, true);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001419C File Offset: 0x0001319C
		private void ClrCompare(QilNodeType relOp, XmlTypeCode code)
		{
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.OnTrue:
			{
				OpCode opCode;
				switch (relOp)
				{
				case QilNodeType.Ne:
					opCode = OpCodes.Bne_Un;
					break;
				case QilNodeType.Eq:
					opCode = OpCodes.Beq;
					break;
				case QilNodeType.Gt:
					opCode = OpCodes.Bgt;
					break;
				case QilNodeType.Ge:
					opCode = OpCodes.Bge;
					break;
				case QilNodeType.Lt:
					opCode = OpCodes.Blt;
					break;
				case QilNodeType.Le:
					opCode = OpCodes.Ble;
					break;
				default:
					opCode = OpCodes.Nop;
					break;
				}
				this.helper.Emit(opCode, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
				return;
			}
			case BranchingContext.OnFalse:
			{
				OpCode opCode;
				if (code == XmlTypeCode.Double || code == XmlTypeCode.Float)
				{
					switch (relOp)
					{
					case QilNodeType.Ne:
						opCode = OpCodes.Beq;
						break;
					case QilNodeType.Eq:
						opCode = OpCodes.Bne_Un;
						break;
					case QilNodeType.Gt:
						opCode = OpCodes.Ble_Un;
						break;
					case QilNodeType.Ge:
						opCode = OpCodes.Blt_Un;
						break;
					case QilNodeType.Lt:
						opCode = OpCodes.Bge_Un;
						break;
					case QilNodeType.Le:
						opCode = OpCodes.Bgt_Un;
						break;
					default:
						opCode = OpCodes.Nop;
						break;
					}
				}
				else
				{
					switch (relOp)
					{
					case QilNodeType.Ne:
						opCode = OpCodes.Beq;
						break;
					case QilNodeType.Eq:
						opCode = OpCodes.Bne_Un;
						break;
					case QilNodeType.Gt:
						opCode = OpCodes.Ble;
						break;
					case QilNodeType.Ge:
						opCode = OpCodes.Blt;
						break;
					case QilNodeType.Lt:
						opCode = OpCodes.Bge;
						break;
					case QilNodeType.Le:
						opCode = OpCodes.Bgt;
						break;
					default:
						opCode = OpCodes.Nop;
						break;
					}
				}
				this.helper.Emit(opCode, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
				return;
			}
			default:
			{
				switch (relOp)
				{
				case QilNodeType.Eq:
					this.helper.Emit(OpCodes.Ceq);
					goto IL_0255;
				case QilNodeType.Gt:
					this.helper.Emit(OpCodes.Cgt);
					goto IL_0255;
				case QilNodeType.Lt:
					this.helper.Emit(OpCodes.Clt);
					goto IL_0255;
				}
				OpCode opCode;
				if (relOp != QilNodeType.Ne)
				{
					switch (relOp)
					{
					case QilNodeType.Ge:
						opCode = OpCodes.Bge_S;
						goto IL_022F;
					case QilNodeType.Le:
						opCode = OpCodes.Ble_S;
						goto IL_022F;
					}
					opCode = OpCodes.Nop;
				}
				else
				{
					opCode = OpCodes.Bne_Un_S;
				}
				IL_022F:
				Label label = this.helper.DefineLabel();
				this.helper.Emit(opCode, label);
				this.helper.ConvBranchToBool(label, true);
				IL_0255:
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
				return;
			}
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001441C File Offset: 0x0001341C
		private void ZeroCompare(QilNodeType relOp, bool isBoolVal)
		{
			switch (this.iterCurr.CurrentBranchingContext)
			{
			case BranchingContext.OnTrue:
				this.helper.Emit((relOp == QilNodeType.Eq) ? OpCodes.Brfalse : OpCodes.Brtrue, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
				return;
			case BranchingContext.OnFalse:
				this.helper.Emit((relOp == QilNodeType.Eq) ? OpCodes.Brtrue : OpCodes.Brfalse, this.iterCurr.LabelBranch);
				this.iterCurr.Storage = StorageDescriptor.None();
				return;
			default:
				if (!isBoolVal || relOp == QilNodeType.Eq)
				{
					Label label = this.helper.DefineLabel();
					this.helper.Emit((relOp == QilNodeType.Eq) ? OpCodes.Brfalse : OpCodes.Brtrue, label);
					this.helper.ConvBranchToBool(label, true);
				}
				this.iterCurr.Storage = StorageDescriptor.Stack(typeof(bool), false);
				return;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00014510 File Offset: 0x00013510
		private void StartWriterLoop(QilNode nd, out bool hasOnEnd, out Label lblOnEnd)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(nd);
			hasOnEnd = false;
			lblOnEnd = default(Label);
			if (!xmlILConstructInfo.PushToWriterLast || nd.XmlType.IsSingleton)
			{
				return;
			}
			if (!this.iterCurr.HasLabelNext)
			{
				hasOnEnd = true;
				lblOnEnd = this.helper.DefineLabel();
				this.iterCurr.SetIterator(lblOnEnd, StorageDescriptor.None());
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001457C File Offset: 0x0001357C
		private void EndWriterLoop(QilNode nd, bool hasOnEnd, Label lblOnEnd)
		{
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Read(nd);
			if (!xmlILConstructInfo.PushToWriterLast)
			{
				return;
			}
			this.iterCurr.Storage = StorageDescriptor.None();
			if (nd.XmlType.IsSingleton)
			{
				return;
			}
			if (hasOnEnd)
			{
				this.iterCurr.LoopToEnd(lblOnEnd);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000145C6 File Offset: 0x000135C6
		private bool MightHaveNamespacesAfterAttributes(XmlILConstructInfo info)
		{
			if (info != null)
			{
				info = info.ParentElementInfo;
			}
			return info == null || info.MightHaveNamespacesAfterAttributes;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000145DE File Offset: 0x000135DE
		private bool ElementCachesAttributes(XmlILConstructInfo info)
		{
			return info.MightHaveDuplicateAttributes || info.MightHaveNamespacesAfterAttributes;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000145F0 File Offset: 0x000135F0
		private void BeforeStartChecks(QilNode ndCtor)
		{
			switch (XmlILConstructInfo.Read(ndCtor).InitialStates)
			{
			case PossibleXmlStates.WithinSequence:
				this.helper.CallStartTree(this.QilConstructorToNodeType(ndCtor.NodeType));
				return;
			case PossibleXmlStates.EnumAttrs:
				switch (ndCtor.NodeType)
				{
				case QilNodeType.ElementCtor:
				case QilNodeType.CommentCtor:
				case QilNodeType.PICtor:
				case QilNodeType.TextCtor:
				case QilNodeType.RawTextCtor:
					this.helper.CallStartElementContent();
					break;
				case QilNodeType.AttributeCtor:
					break;
				default:
					return;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00014666 File Offset: 0x00013666
		private void AfterEndChecks(QilNode ndCtor)
		{
			if (XmlILConstructInfo.Read(ndCtor).FinalStates == PossibleXmlStates.WithinSequence)
			{
				this.helper.CallEndTree();
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00014684 File Offset: 0x00013684
		private bool CheckWithinContent(XmlILConstructInfo info)
		{
			switch (info.InitialStates)
			{
			case PossibleXmlStates.WithinSequence:
			case PossibleXmlStates.EnumAttrs:
			case PossibleXmlStates.WithinContent:
				return false;
			default:
				return true;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000146B4 File Offset: 0x000136B4
		private bool CheckEnumAttrs(XmlILConstructInfo info)
		{
			switch (info.InitialStates)
			{
			case PossibleXmlStates.WithinSequence:
			case PossibleXmlStates.EnumAttrs:
				return false;
			default:
				return true;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000146E0 File Offset: 0x000136E0
		private XPathNodeType QilXmlToXPathNodeType(XmlNodeKindFlags xmlTypes)
		{
			switch (xmlTypes)
			{
			case XmlNodeKindFlags.Element:
				return XPathNodeType.Element;
			case XmlNodeKindFlags.Document | XmlNodeKindFlags.Element:
				break;
			case XmlNodeKindFlags.Attribute:
				return XPathNodeType.Attribute;
			default:
				if (xmlTypes == XmlNodeKindFlags.Text)
				{
					return XPathNodeType.Text;
				}
				if (xmlTypes == XmlNodeKindFlags.Comment)
				{
					return XPathNodeType.Comment;
				}
				break;
			}
			return XPathNodeType.ProcessingInstruction;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00014718 File Offset: 0x00013718
		private XPathNodeType QilConstructorToNodeType(QilNodeType typ)
		{
			switch (typ)
			{
			case QilNodeType.ElementCtor:
				return XPathNodeType.Element;
			case QilNodeType.AttributeCtor:
				return XPathNodeType.Attribute;
			case QilNodeType.CommentCtor:
				return XPathNodeType.Comment;
			case QilNodeType.PICtor:
				return XPathNodeType.ProcessingInstruction;
			case QilNodeType.TextCtor:
				return XPathNodeType.Text;
			case QilNodeType.RawTextCtor:
				return XPathNodeType.Text;
			case QilNodeType.DocumentCtor:
				return XPathNodeType.Root;
			case QilNodeType.NamespaceDecl:
				return XPathNodeType.Namespace;
			default:
				return XPathNodeType.All;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00014764 File Offset: 0x00013764
		private void LoadSelectFilter(XmlNodeKindFlags xmlTypes, QilName ndName)
		{
			if (ndName != null)
			{
				this.helper.CallGetNameFilter(this.helper.StaticData.DeclareNameFilter(ndName.LocalName, ndName.NamespaceUri));
				return;
			}
			bool flag = XmlILVisitor.IsNodeTypeUnion(xmlTypes);
			if (!flag)
			{
				this.helper.CallGetTypeFilter(this.QilXmlToXPathNodeType(xmlTypes));
				return;
			}
			if ((xmlTypes & XmlNodeKindFlags.Attribute) != XmlNodeKindFlags.None)
			{
				this.helper.CallGetTypeFilter(XPathNodeType.All);
				return;
			}
			this.helper.CallGetTypeFilter(XPathNodeType.Attribute);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000147D8 File Offset: 0x000137D8
		private static bool IsNodeTypeUnion(XmlNodeKindFlags xmlTypes)
		{
			return (xmlTypes & (xmlTypes - 1)) != XmlNodeKindFlags.None;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000147E8 File Offset: 0x000137E8
		private void StartNestedIterator(QilNode nd)
		{
			IteratorDescriptor iteratorDescriptor = this.iterCurr;
			if (iteratorDescriptor == null)
			{
				this.iterCurr = new IteratorDescriptor(this.helper);
			}
			else
			{
				this.iterCurr = new IteratorDescriptor(iteratorDescriptor);
			}
			this.iterNested = null;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00014825 File Offset: 0x00013825
		private void StartNestedIterator(QilNode nd, Label lblOnEnd)
		{
			this.StartNestedIterator(nd);
			this.iterCurr.SetIterator(lblOnEnd, StorageDescriptor.None());
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00014840 File Offset: 0x00013840
		private void EndNestedIterator(QilNode nd)
		{
			if (this.iterCurr.IsBranching && this.iterCurr.Storage.Location != ItemLocation.None)
			{
				this.iterCurr.EnsureItemStorageType(nd.XmlType, typeof(bool));
				this.iterCurr.EnsureStackNoCache();
				if (this.iterCurr.CurrentBranchingContext == BranchingContext.OnTrue)
				{
					this.helper.Emit(OpCodes.Brtrue, this.iterCurr.LabelBranch);
				}
				else
				{
					this.helper.Emit(OpCodes.Brfalse, this.iterCurr.LabelBranch);
				}
				this.iterCurr.Storage = StorageDescriptor.None();
			}
			this.iterNested = this.iterCurr;
			this.iterCurr = this.iterCurr.ParentIterator;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001490C File Offset: 0x0001390C
		private void NestedVisit(QilNode nd, Type itemStorageType, bool isCached)
		{
			if (XmlILConstructInfo.Read(nd).PushToWriterLast)
			{
				this.StartNestedIterator(nd);
				this.Visit(nd);
				this.EndNestedIterator(nd);
				this.iterCurr.Storage = StorageDescriptor.None();
				return;
			}
			if (!isCached && nd.XmlType.IsSingleton)
			{
				this.StartNestedIterator(nd);
				this.Visit(nd);
				this.iterCurr.EnsureNoCache();
				this.iterCurr.EnsureItemStorageType(nd.XmlType, itemStorageType);
				this.EndNestedIterator(nd);
				this.iterCurr.Storage = this.iterNested.Storage;
				return;
			}
			this.NestedVisitEnsureCache(nd, itemStorageType);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000149AF File Offset: 0x000139AF
		private void NestedVisit(QilNode nd)
		{
			this.NestedVisit(nd, this.GetItemStorageType(nd), !nd.XmlType.IsSingleton);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000149D0 File Offset: 0x000139D0
		private void NestedVisit(QilNode nd, Label lblOnEnd)
		{
			this.StartNestedIterator(nd, lblOnEnd);
			this.Visit(nd);
			this.iterCurr.EnsureNoCache();
			this.iterCurr.EnsureItemStorageType(nd.XmlType, this.GetItemStorageType(nd));
			this.EndNestedIterator(nd);
			this.iterCurr.Storage = this.iterNested.Storage;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00014A2D File Offset: 0x00013A2D
		private void NestedVisitEnsureStack(QilNode nd)
		{
			this.NestedVisit(nd);
			this.iterCurr.EnsureStack();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00014A41 File Offset: 0x00013A41
		private void NestedVisitEnsureStack(QilNode ndLeft, QilNode ndRight)
		{
			this.NestedVisitEnsureStack(ndLeft);
			this.NestedVisitEnsureStack(ndRight);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00014A51 File Offset: 0x00013A51
		private void NestedVisitEnsureStack(QilNode nd, Type itemStorageType, bool isCached)
		{
			this.NestedVisit(nd, itemStorageType, isCached);
			this.iterCurr.EnsureStack();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00014A67 File Offset: 0x00013A67
		private void NestedVisitEnsureLocal(QilNode nd, LocalBuilder loc)
		{
			this.NestedVisit(nd);
			this.iterCurr.EnsureLocal(loc);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00014A7C File Offset: 0x00013A7C
		private void NestedVisitWithBranch(QilNode nd, BranchingContext brctxt, Label lblBranch)
		{
			this.StartNestedIterator(nd);
			this.iterCurr.SetBranching(brctxt, lblBranch);
			this.Visit(nd);
			this.EndNestedIterator(nd);
			this.iterCurr.Storage = StorageDescriptor.None();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00014AB4 File Offset: 0x00013AB4
		private void NestedVisitEnsureCache(QilNode nd, Type itemStorageType)
		{
			bool flag = this.CachesResult(nd);
			Label label = this.helper.DefineLabel();
			if (flag)
			{
				this.StartNestedIterator(nd);
				this.Visit(nd);
				this.EndNestedIterator(nd);
				this.iterCurr.Storage = this.iterNested.Storage;
				if (this.iterCurr.Storage.ItemStorageType == itemStorageType)
				{
					return;
				}
				if (this.iterCurr.Storage.ItemStorageType == typeof(XPathNavigator) || itemStorageType == typeof(XPathNavigator))
				{
					this.iterCurr.EnsureItemStorageType(nd.XmlType, itemStorageType);
					return;
				}
				this.iterCurr.EnsureNoStack("$$$cacheResult");
			}
			Type type = ((this.GetItemStorageType(nd) == typeof(XPathNavigator)) ? typeof(XPathNavigator) : itemStorageType);
			XmlILStorageMethods xmlILStorageMethods = XmlILMethods.StorageMethods[type];
			LocalBuilder localBuilder = this.helper.DeclareLocal("$$$cache", xmlILStorageMethods.SeqType);
			this.helper.Emit(OpCodes.Ldloc, localBuilder);
			if (nd.XmlType.IsSingleton)
			{
				this.NestedVisitEnsureStack(nd, type, false);
				this.helper.CallToken(xmlILStorageMethods.SeqReuseSgl);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
			}
			else
			{
				this.helper.CallToken(xmlILStorageMethods.SeqReuse);
				this.helper.Emit(OpCodes.Stloc, localBuilder);
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.StartNestedIterator(nd, label);
				if (flag)
				{
					this.iterCurr.Storage = this.iterCurr.ParentIterator.Storage;
				}
				else
				{
					this.Visit(nd);
				}
				this.iterCurr.EnsureItemStorageType(nd.XmlType, type);
				this.iterCurr.EnsureStackNoCache();
				this.helper.Call(xmlILStorageMethods.SeqAdd);
				this.helper.Emit(OpCodes.Ldloc, localBuilder);
				this.iterCurr.LoopToEnd(label);
				this.EndNestedIterator(nd);
				this.helper.Emit(OpCodes.Pop);
			}
			this.iterCurr.Storage = StorageDescriptor.Local(localBuilder, itemStorageType, true);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00014CE0 File Offset: 0x00013CE0
		private bool CachesResult(QilNode nd)
		{
			QilNodeType nodeType = nd.NodeType;
			if (nodeType <= QilNodeType.Filter)
			{
				switch (nodeType)
				{
				case QilNodeType.Let:
				case QilNodeType.Parameter:
					break;
				default:
				{
					if (nodeType != QilNodeType.Filter)
					{
						return false;
					}
					OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(nd);
					return optimizerPatterns.MatchesPattern(OptimizerPatternName.EqualityIndex);
				}
				}
			}
			else
			{
				switch (nodeType)
				{
				case QilNodeType.DocOrderDistinct:
				{
					if (nd.XmlType.IsSingleton)
					{
						return false;
					}
					OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(nd);
					return !optimizerPatterns.MatchesPattern(OptimizerPatternName.JoinAndDod) && !optimizerPatterns.MatchesPattern(OptimizerPatternName.DodReverse);
				}
				case QilNodeType.Function:
					return false;
				case QilNodeType.Invoke:
					break;
				default:
					if (nodeType == QilNodeType.TypeAssert)
					{
						QilTargetType qilTargetType = (QilTargetType)nd;
						return this.CachesResult(qilTargetType.Source) && this.GetItemStorageType(qilTargetType.Source) == this.GetItemStorageType(qilTargetType);
					}
					switch (nodeType)
					{
					case QilNodeType.XsltInvokeLateBound:
					case QilNodeType.XsltInvokeEarlyBound:
						break;
					default:
						return false;
					}
					break;
				}
			}
			return !nd.XmlType.IsSingleton;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00014DB9 File Offset: 0x00013DB9
		private Type GetStorageType(QilNode nd)
		{
			return XmlILTypeHelper.GetStorageType(nd.XmlType);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00014DC6 File Offset: 0x00013DC6
		private Type GetStorageType(XmlQueryType typ)
		{
			return XmlILTypeHelper.GetStorageType(typ);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00014DCE File Offset: 0x00013DCE
		private Type GetItemStorageType(QilNode nd)
		{
			return XmlILTypeHelper.GetStorageType(nd.XmlType.Prime);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00014DE0 File Offset: 0x00013DE0
		private Type GetItemStorageType(XmlQueryType typ)
		{
			return XmlILTypeHelper.GetStorageType(typ.Prime);
		}

		// Token: 0x0400028F RID: 655
		private QilExpression qil;

		// Token: 0x04000290 RID: 656
		private GenerateHelper helper;

		// Token: 0x04000291 RID: 657
		private IteratorDescriptor iterCurr;

		// Token: 0x04000292 RID: 658
		private IteratorDescriptor iterNested;

		// Token: 0x04000293 RID: 659
		private int indexId;
	}
}
