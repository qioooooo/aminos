using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200002A RID: 42
	internal class OptimizerPatterns : IQilAnnotation
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x0000D4B0 File Offset: 0x0000C4B0
		public static OptimizerPatterns Read(QilNode nd)
		{
			XmlILAnnotation xmlILAnnotation = nd.Annotation as XmlILAnnotation;
			OptimizerPatterns optimizerPatterns = ((xmlILAnnotation != null) ? xmlILAnnotation.Patterns : null);
			if (optimizerPatterns == null)
			{
				if (!nd.XmlType.MaybeMany)
				{
					if (OptimizerPatterns.ZeroOrOneDefault == null)
					{
						optimizerPatterns = new OptimizerPatterns();
						optimizerPatterns.AddPattern(OptimizerPatternName.IsDocOrderDistinct);
						optimizerPatterns.AddPattern(OptimizerPatternName.SameDepth);
						optimizerPatterns.isReadOnly = true;
						OptimizerPatterns.ZeroOrOneDefault = optimizerPatterns;
					}
					else
					{
						optimizerPatterns = OptimizerPatterns.ZeroOrOneDefault;
					}
				}
				else if (nd.XmlType.IsDod)
				{
					if (OptimizerPatterns.DodDefault == null)
					{
						optimizerPatterns = new OptimizerPatterns();
						optimizerPatterns.AddPattern(OptimizerPatternName.IsDocOrderDistinct);
						optimizerPatterns.isReadOnly = true;
						OptimizerPatterns.DodDefault = optimizerPatterns;
					}
					else
					{
						optimizerPatterns = OptimizerPatterns.DodDefault;
					}
				}
				else if (OptimizerPatterns.MaybeManyDefault == null)
				{
					optimizerPatterns = new OptimizerPatterns();
					optimizerPatterns.isReadOnly = true;
					OptimizerPatterns.MaybeManyDefault = optimizerPatterns;
				}
				else
				{
					optimizerPatterns = OptimizerPatterns.MaybeManyDefault;
				}
			}
			return optimizerPatterns;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000D578 File Offset: 0x0000C578
		public static OptimizerPatterns Write(QilNode nd)
		{
			XmlILAnnotation xmlILAnnotation = XmlILAnnotation.Write(nd);
			OptimizerPatterns optimizerPatterns = xmlILAnnotation.Patterns;
			if (optimizerPatterns == null || optimizerPatterns.isReadOnly)
			{
				optimizerPatterns = new OptimizerPatterns();
				xmlILAnnotation.Patterns = optimizerPatterns;
				if (!nd.XmlType.MaybeMany)
				{
					optimizerPatterns.AddPattern(OptimizerPatternName.IsDocOrderDistinct);
					optimizerPatterns.AddPattern(OptimizerPatternName.SameDepth);
				}
				else if (nd.XmlType.IsDod)
				{
					optimizerPatterns.AddPattern(OptimizerPatternName.IsDocOrderDistinct);
				}
			}
			return optimizerPatterns;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000D5E0 File Offset: 0x0000C5E0
		public static void Inherit(QilNode ndSrc, QilNode ndDst, OptimizerPatternName pattern)
		{
			OptimizerPatterns optimizerPatterns = OptimizerPatterns.Read(ndSrc);
			if (optimizerPatterns.MatchesPattern(pattern))
			{
				OptimizerPatterns optimizerPatterns2 = OptimizerPatterns.Write(ndDst);
				optimizerPatterns2.AddPattern(pattern);
				switch (pattern)
				{
				case OptimizerPatternName.DodReverse:
				case OptimizerPatternName.JoinAndDod:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.ElementQName, optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					return;
				case OptimizerPatternName.EqualityIndex:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.StepNode, optimizerPatterns.GetArgument(OptimizerPatternArgument.StepNode));
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.StepInput, optimizerPatterns.GetArgument(OptimizerPatternArgument.StepInput));
					return;
				case OptimizerPatternName.FilterAttributeKind:
				case OptimizerPatternName.IsDocOrderDistinct:
				case OptimizerPatternName.IsPositional:
				case OptimizerPatternName.SameDepth:
					break;
				case OptimizerPatternName.FilterContentKind:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.ElementQName, optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					return;
				case OptimizerPatternName.FilterElements:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.ElementQName, optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					return;
				case OptimizerPatternName.MaxPosition:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.ElementQName, optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					return;
				case OptimizerPatternName.Step:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.StepNode, optimizerPatterns.GetArgument(OptimizerPatternArgument.StepNode));
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.StepInput, optimizerPatterns.GetArgument(OptimizerPatternArgument.StepInput));
					return;
				case OptimizerPatternName.SingleTextRtf:
					optimizerPatterns2.AddArgument(OptimizerPatternArgument.ElementQName, optimizerPatterns.GetArgument(OptimizerPatternArgument.ElementQName));
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000D6D0 File Offset: 0x0000C6D0
		public void AddArgument(OptimizerPatternArgument argId, object arg)
		{
			switch (argId)
			{
			case OptimizerPatternArgument.StepNode:
				this.arg0 = arg;
				return;
			case OptimizerPatternArgument.StepInput:
				this.arg1 = arg;
				return;
			case OptimizerPatternArgument.ElementQName:
				this.arg2 = arg;
				return;
			default:
				return;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000D70C File Offset: 0x0000C70C
		public object GetArgument(OptimizerPatternArgument argNum)
		{
			object obj = null;
			switch (argNum)
			{
			case OptimizerPatternArgument.StepNode:
				obj = this.arg0;
				break;
			case OptimizerPatternArgument.StepInput:
				obj = this.arg1;
				break;
			case OptimizerPatternArgument.ElementQName:
				obj = this.arg2;
				break;
			}
			return obj;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D74B File Offset: 0x0000C74B
		public void AddPattern(OptimizerPatternName pattern)
		{
			this.patterns |= 1 << (int)pattern;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D760 File Offset: 0x0000C760
		public bool MatchesPattern(OptimizerPatternName pattern)
		{
			return (this.patterns & (1 << (int)pattern)) != 0;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000D775 File Offset: 0x0000C775
		public virtual string Name
		{
			get
			{
				return "Patterns";
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D77C File Offset: 0x0000C77C
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < OptimizerPatterns.PatternCount; i++)
			{
				if (this.MatchesPattern((OptimizerPatternName)i))
				{
					if (text.Length != 0)
					{
						text += ", ";
					}
					text += ((OptimizerPatternName)i).ToString();
				}
			}
			return text;
		}

		// Token: 0x0400027D RID: 637
		private static readonly int PatternCount = Enum.GetValues(typeof(OptimizerPatternName)).Length;

		// Token: 0x0400027E RID: 638
		private int patterns;

		// Token: 0x0400027F RID: 639
		private bool isReadOnly;

		// Token: 0x04000280 RID: 640
		private object arg0;

		// Token: 0x04000281 RID: 641
		private object arg1;

		// Token: 0x04000282 RID: 642
		private object arg2;

		// Token: 0x04000283 RID: 643
		private static OptimizerPatterns ZeroOrOneDefault;

		// Token: 0x04000284 RID: 644
		private static OptimizerPatterns MaybeManyDefault;

		// Token: 0x04000285 RID: 645
		private static OptimizerPatterns DodDefault;
	}
}
