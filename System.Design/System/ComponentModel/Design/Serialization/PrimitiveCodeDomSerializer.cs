using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace System.ComponentModel.Design.Serialization
{
	internal class PrimitiveCodeDomSerializer : CodeDomSerializer
	{
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

		private static readonly string JSharpFileExtension = ".jsl";

		private static PrimitiveCodeDomSerializer defaultSerializer;
	}
}
