using System;
using System.CodeDom;
using System.Collections;
using System.Design;
using System.Globalization;

namespace System.Data.Design
{
	internal sealed class ExtendedPropertiesHandler
	{
		private ExtendedPropertiesHandler()
		{
		}

		internal static TypedDataSourceCodeGenerator CodeGenerator
		{
			set
			{
				ExtendedPropertiesHandler.codeGenerator = value;
			}
		}

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

		private static TypedDataSourceCodeGenerator codeGenerator;

		private static DataSourceComponent targetObject;
	}
}
