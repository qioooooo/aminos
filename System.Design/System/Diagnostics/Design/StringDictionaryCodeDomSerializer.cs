using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Diagnostics.Design
{
	internal class StringDictionaryCodeDomSerializer : CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			return null;
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			object obj = null;
			StringDictionary stringDictionary = value as StringDictionary;
			if (stringDictionary != null)
			{
				object obj2 = manager.Context.Current;
				ExpressionContext expressionContext = obj2 as ExpressionContext;
				if (expressionContext != null && expressionContext.Owner == value)
				{
					obj2 = expressionContext.Expression;
				}
				CodePropertyReferenceExpression codePropertyReferenceExpression = obj2 as CodePropertyReferenceExpression;
				if (codePropertyReferenceExpression != null)
				{
					object obj3 = base.DeserializeExpression(manager, null, codePropertyReferenceExpression.TargetObject);
					if (obj3 != null)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj3)[codePropertyReferenceExpression.PropertyName];
						if (propertyDescriptor != null)
						{
							CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
							CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codePropertyReferenceExpression, "Add");
							foreach (object obj4 in stringDictionary)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj4;
								CodeExpression codeExpression = base.SerializeToExpression(manager, dictionaryEntry.Key);
								CodeExpression codeExpression2 = base.SerializeToExpression(manager, dictionaryEntry.Value);
								if (codeExpression != null && codeExpression2 != null)
								{
									codeStatementCollection.Add(new CodeMethodInvokeExpression
									{
										Method = codeMethodReferenceExpression,
										Parameters = { codeExpression, codeExpression2 }
									});
								}
							}
							obj = codeStatementCollection;
						}
					}
				}
			}
			return obj;
		}
	}
}
