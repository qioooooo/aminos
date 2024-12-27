using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200029D RID: 669
	internal class TableLayoutControlCollectionCodeDomSerializer : CollectionCodeDomSerializer
	{
		// Token: 0x060018DB RID: 6363 RVA: 0x00084D70 File Offset: 0x00083D70
		protected override object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
		{
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(targetExpression, "Add");
			TableLayoutControlCollection tableLayoutControlCollection = (TableLayoutControlCollection)originalCollection;
			if (valuesToSerialize.Count > 0)
			{
				bool flag = false;
				ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
				if (expressionContext != null && expressionContext.Expression == targetExpression)
				{
					IComponent component = expressionContext.Owner as IComponent;
					if (component != null)
					{
						InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(component)[typeof(InheritanceAttribute)];
						flag = inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel == InheritanceLevel.Inherited;
					}
				}
				foreach (object obj in valuesToSerialize)
				{
					bool flag2 = !(obj is IComponent);
					if (!flag2)
					{
						InheritanceAttribute inheritanceAttribute2 = (InheritanceAttribute)TypeDescriptor.GetAttributes(obj)[typeof(InheritanceAttribute)];
						flag2 = inheritanceAttribute2 == null || (inheritanceAttribute2.InheritanceLevel != InheritanceLevel.InheritedReadOnly && (inheritanceAttribute2.InheritanceLevel != InheritanceLevel.Inherited || !flag));
					}
					if (flag2)
					{
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
						codeMethodInvokeExpression.Method = codeMethodReferenceExpression;
						CodeExpression codeExpression = base.SerializeToExpression(manager, obj);
						if (codeExpression != null && !typeof(Control).IsAssignableFrom(obj.GetType()))
						{
							codeExpression = new CodeCastExpression(typeof(Control), codeExpression);
						}
						if (codeExpression != null)
						{
							int column = tableLayoutControlCollection.Container.GetColumn((Control)obj);
							int row = tableLayoutControlCollection.Container.GetRow((Control)obj);
							codeMethodInvokeExpression.Parameters.Add(codeExpression);
							if (column != -1 || row != -1)
							{
								codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(column));
								codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(row));
							}
							codeStatementCollection.Add(codeMethodInvokeExpression);
						}
					}
				}
			}
			return codeStatementCollection;
		}
	}
}
