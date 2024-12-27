using System;
using System.CodeDom;
using System.Collections;
using System.Design;
using System.Globalization;

namespace System.Data.Design
{
	// Token: 0x020000A2 RID: 162
	internal sealed class ExtendedPropertiesHandler
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x0000F917 File Offset: 0x0000E917
		private ExtendedPropertiesHandler()
		{
		}

		// Token: 0x170000FC RID: 252
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0000F91F File Offset: 0x0000E91F
		internal static TypedDataSourceCodeGenerator CodeGenerator
		{
			set
			{
				ExtendedPropertiesHandler.codeGenerator = value;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000F928 File Offset: 0x0000E928
		internal static void AddExtendedProperties(DataSourceComponent targetObj, CodeExpression addTarget, IList statementCollection, Hashtable extendedProperties)
		{
			if (extendedProperties == null)
			{
				return;
			}
			if (addTarget == null)
			{
				throw new InternalException("ExtendedPropertiesHandler.AddExtendedProperties: addTarget cannot be null");
			}
			if (statementCollection == null)
			{
				throw new InternalException("ExtendedPropertiesHandler.AddExtendedProperties: statementCollection cannot be null");
			}
			if (ExtendedPropertiesHandler.codeGenerator == null)
			{
				throw new InternalException("ExtendedPropertiesHandler.AddExtendedProperties: codeGenerator cannot be null");
			}
			if (targetObj == null)
			{
				throw new InternalException("ExtendedPropertiesHandler.AddExtendedProperties: targetObject cannot be null");
			}
			ExtendedPropertiesHandler.targetObject = targetObj;
			if (ExtendedPropertiesHandler.codeGenerator.GenerateExtendedProperties)
			{
				ExtendedPropertiesHandler.GenerateProperties(addTarget, statementCollection, extendedProperties);
				return;
			}
			SortedList sortedList = new SortedList(new Comparer(CultureInfo.InvariantCulture));
			foreach (string text in ExtendedPropertiesHandler.targetObject.NamingPropertyNames)
			{
				string text2 = extendedProperties[text] as string;
				if (!StringUtil.Empty(text2))
				{
					sortedList.Add(text, text2);
				}
			}
			ExtendedPropertiesHandler.GenerateProperties(addTarget, statementCollection, sortedList);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000FA0C File Offset: 0x0000EA0C
		private static void GenerateProperties(CodeExpression addTarget, IList statementCollection, ICollection extendedProperties)
		{
			if (extendedProperties != null)
			{
				IDictionaryEnumerator dictionaryEnumerator = (IDictionaryEnumerator)extendedProperties.GetEnumerator();
				if (dictionaryEnumerator != null)
				{
					dictionaryEnumerator.Reset();
					while (dictionaryEnumerator.MoveNext())
					{
						string text = dictionaryEnumerator.Key as string;
						string text2 = dictionaryEnumerator.Value as string;
						if (text == null || text2 == null)
						{
							ExtendedPropertiesHandler.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_UnableToReadExtProperties"), ProblemSeverity.NonFatalError, ExtendedPropertiesHandler.targetObject));
						}
						else
						{
							statementCollection.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(addTarget, "ExtendedProperties"), "Add", new CodeExpression[]
							{
								CodeGenHelper.Primitive(text),
								CodeGenHelper.Primitive(text2)
							})));
						}
					}
				}
			}
		}

		// Token: 0x04000B94 RID: 2964
		private static TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000B95 RID: 2965
		private static DataSourceComponent targetObject;
	}
}
