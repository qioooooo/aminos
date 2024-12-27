using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000163 RID: 355
	internal class PrimitiveCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0003426A File Offset: 0x0003326A
		internal new static PrimitiveCodeDomSerializer Default
		{
			get
			{
				if (PrimitiveCodeDomSerializer.defaultSerializer == null)
				{
					PrimitiveCodeDomSerializer.defaultSerializer = new PrimitiveCodeDomSerializer();
				}
				return PrimitiveCodeDomSerializer.defaultSerializer;
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00034284 File Offset: 0x00033284
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			using (CodeDomSerializerBase.TraceScope("PrimitiveCodeDomSerializer::Serialize"))
			{
			}
			CodeExpression codeExpression = new CodePrimitiveExpression(value);
			if (value != null)
			{
				if (value is bool || value is char || value is int || value is float || value is double)
				{
					CodeDomProvider codeDomProvider = manager.GetService(typeof(CodeDomProvider)) as CodeDomProvider;
					if (codeDomProvider != null && string.Equals(codeDomProvider.FileExtension, PrimitiveCodeDomSerializer.JSharpFileExtension))
					{
						ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
						if (expressionContext != null && expressionContext.ExpressionType == typeof(object))
						{
							codeExpression = new CodeCastExpression(value.GetType(), codeExpression);
							codeExpression.UserData.Add("CastIsBoxing", true);
						}
					}
				}
				else if (value is string)
				{
					string text = value as string;
					if (text != null && text.Length > 200)
					{
						codeExpression = base.SerializeToResourceExpression(manager, text);
					}
				}
				else
				{
					codeExpression = new CodeCastExpression(new CodeTypeReference(value.GetType()), codeExpression);
				}
			}
			return codeExpression;
		}

		// Token: 0x04000EF5 RID: 3829
		private static readonly string JSharpFileExtension = ".jsl";

		// Token: 0x04000EF6 RID: 3830
		private static PrimitiveCodeDomSerializer defaultSerializer;
	}
}
