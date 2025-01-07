using System;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace System.Data.Design
{
	internal class PropertyReferenceSerializer
	{
		private PropertyReferenceSerializer()
		{
		}

		internal static string Serialize(CodePropertyReferenceExpression expression)
		{
			if (PropertyReferenceSerializer.IsWellKnownApplicationSettingsExpression(expression))
			{
				return PropertyReferenceSerializer.SerializeApplicationSettingsExpression(expression);
			}
			if (PropertyReferenceSerializer.IsWellKnownAppConfigExpression(expression))
			{
				return PropertyReferenceSerializer.SerializeAppConfigExpression(expression);
			}
			return PropertyReferenceSerializer.SerializeWithSoapFormatter(expression);
		}

		internal static CodePropertyReferenceExpression Deserialize(string expressionString)
		{
			string[] array = expressionString.Split(new char[] { '.' });
			if (array != null && array.Length > 0)
			{
				if (StringUtil.EqualValue(array[0], "ApplicationSettings"))
				{
					return PropertyReferenceSerializer.DeserializeApplicationSettingsExpression(array);
				}
				if (StringUtil.EqualValue(array[0], "AppConfig"))
				{
					return PropertyReferenceSerializer.DeserializeAppConfigExpression(array);
				}
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			MemoryStream memoryStream = new MemoryStream(utf8Encoding.GetBytes(expressionString));
			IFormatter formatter = new SoapFormatter();
			return (CodePropertyReferenceExpression)formatter.Deserialize(memoryStream);
		}

		private static string SerializeWithSoapFormatter(CodePropertyReferenceExpression expression)
		{
			MemoryStream memoryStream = new MemoryStream();
			IFormatter formatter = new SoapFormatter();
			formatter.Serialize(memoryStream, expression);
			if (memoryStream.Length > 2147483647L)
			{
				throw new InternalException("Serialized property expression is too long.");
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Position = 0L;
			memoryStream.Read(array, 0, (int)memoryStream.Length);
			return utf8Encoding.GetString(array);
		}

		private static string SerializeApplicationSettingsExpression(CodePropertyReferenceExpression expression)
		{
			string text = expression.PropertyName;
			CodePropertyReferenceExpression codePropertyReferenceExpression = (CodePropertyReferenceExpression)expression.TargetObject;
			text = codePropertyReferenceExpression.PropertyName + "." + text;
			CodeTypeReferenceExpression codeTypeReferenceExpression = (CodeTypeReferenceExpression)codePropertyReferenceExpression.TargetObject;
			text = codeTypeReferenceExpression.Type.Options.ToString() + "." + text;
			text = codeTypeReferenceExpression.Type.BaseType + "." + text;
			return "ApplicationSettings." + text;
		}

		private static string SerializeAppConfigExpression(CodePropertyReferenceExpression expression)
		{
			string text = expression.PropertyName;
			CodeIndexerExpression codeIndexerExpression = (CodeIndexerExpression)expression.TargetObject;
			string text2 = ((CodePrimitiveExpression)codeIndexerExpression.Indices[0]).Value as string;
			text = text2 + "." + text;
			CodePropertyReferenceExpression codePropertyReferenceExpression = (CodePropertyReferenceExpression)codeIndexerExpression.TargetObject;
			text = codePropertyReferenceExpression.PropertyName + "." + text;
			CodeTypeReferenceExpression codeTypeReferenceExpression = (CodeTypeReferenceExpression)codePropertyReferenceExpression.TargetObject;
			text = codeTypeReferenceExpression.Type.Options.ToString() + "." + text;
			text = codeTypeReferenceExpression.Type.BaseType + "." + text;
			return "AppConfig." + text;
		}

		private static bool IsWellKnownApplicationSettingsExpression(CodePropertyReferenceExpression expression)
		{
			if (expression.UserData != null && expression.UserData.Count > 0)
			{
				return false;
			}
			if (!(expression.TargetObject is CodePropertyReferenceExpression))
			{
				return false;
			}
			CodePropertyReferenceExpression codePropertyReferenceExpression = (CodePropertyReferenceExpression)expression.TargetObject;
			if (codePropertyReferenceExpression.UserData != null && codePropertyReferenceExpression.UserData.Count > 0)
			{
				return false;
			}
			if (!(codePropertyReferenceExpression.TargetObject is CodeTypeReferenceExpression))
			{
				return false;
			}
			CodeTypeReferenceExpression codeTypeReferenceExpression = (CodeTypeReferenceExpression)codePropertyReferenceExpression.TargetObject;
			if (codeTypeReferenceExpression.UserData != null && codeTypeReferenceExpression.UserData.Count > 0)
			{
				return false;
			}
			CodeTypeReference type = codeTypeReferenceExpression.Type;
			return (type.UserData == null || type.UserData.Count <= 0) && (type.TypeArguments == null || type.TypeArguments.Count <= 0) && type.ArrayElementType == null && type.ArrayRank <= 0;
		}

		private static bool IsWellKnownAppConfigExpression(CodePropertyReferenceExpression expression)
		{
			if (expression.UserData != null && expression.UserData.Count > 0)
			{
				return false;
			}
			if (!(expression.TargetObject is CodeIndexerExpression))
			{
				return false;
			}
			CodeIndexerExpression codeIndexerExpression = (CodeIndexerExpression)expression.TargetObject;
			if (codeIndexerExpression.UserData != null && codeIndexerExpression.UserData.Count > 0)
			{
				return false;
			}
			if (codeIndexerExpression.Indices == null || codeIndexerExpression.Indices.Count != 1 || !(codeIndexerExpression.Indices[0] is CodePrimitiveExpression))
			{
				return false;
			}
			if (!(((CodePrimitiveExpression)codeIndexerExpression.Indices[0]).Value is string))
			{
				return false;
			}
			if (!(codeIndexerExpression.TargetObject is CodePropertyReferenceExpression))
			{
				return false;
			}
			CodePropertyReferenceExpression codePropertyReferenceExpression = (CodePropertyReferenceExpression)codeIndexerExpression.TargetObject;
			if (codePropertyReferenceExpression.UserData != null && codePropertyReferenceExpression.UserData.Count > 0)
			{
				return false;
			}
			if (!(codePropertyReferenceExpression.TargetObject is CodeTypeReferenceExpression))
			{
				return false;
			}
			CodeTypeReferenceExpression codeTypeReferenceExpression = (CodeTypeReferenceExpression)codePropertyReferenceExpression.TargetObject;
			if (codeTypeReferenceExpression.UserData != null && codeTypeReferenceExpression.UserData.Count > 0)
			{
				return false;
			}
			CodeTypeReference type = codeTypeReferenceExpression.Type;
			return (type.UserData == null || type.UserData.Count <= 0) && (type.TypeArguments == null || type.TypeArguments.Count <= 0) && type.ArrayElementType == null && type.ArrayRank <= 0;
		}

		private static CodePropertyReferenceExpression DeserializeApplicationSettingsExpression(string[] expressionParts)
		{
			int i = expressionParts.Length - 1;
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression();
			codePropertyReferenceExpression.PropertyName = expressionParts[i];
			i--;
			CodePropertyReferenceExpression codePropertyReferenceExpression2 = new CodePropertyReferenceExpression();
			codePropertyReferenceExpression.TargetObject = codePropertyReferenceExpression2;
			codePropertyReferenceExpression2.PropertyName = expressionParts[i];
			i--;
			CodeTypeReferenceExpression codeTypeReferenceExpression = new CodeTypeReferenceExpression();
			codePropertyReferenceExpression2.TargetObject = codeTypeReferenceExpression;
			codeTypeReferenceExpression.Type.Options = (CodeTypeReferenceOptions)Enum.Parse(typeof(CodeTypeReferenceOptions), expressionParts[i]);
			i--;
			codeTypeReferenceExpression.Type.BaseType = expressionParts[i];
			for (i--; i > 0; i--)
			{
				codeTypeReferenceExpression.Type.BaseType = expressionParts[i] + "." + codeTypeReferenceExpression.Type.BaseType;
			}
			return codePropertyReferenceExpression;
		}

		private static CodePropertyReferenceExpression DeserializeAppConfigExpression(string[] expressionParts)
		{
			int i = expressionParts.Length - 1;
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression();
			codePropertyReferenceExpression.PropertyName = expressionParts[i];
			i--;
			CodeIndexerExpression codeIndexerExpression = new CodeIndexerExpression();
			codePropertyReferenceExpression.TargetObject = codeIndexerExpression;
			codeIndexerExpression.Indices.Add(new CodePrimitiveExpression(expressionParts[i]));
			i--;
			CodePropertyReferenceExpression codePropertyReferenceExpression2 = new CodePropertyReferenceExpression();
			codeIndexerExpression.TargetObject = codePropertyReferenceExpression2;
			codePropertyReferenceExpression2.PropertyName = expressionParts[i];
			i--;
			CodeTypeReferenceExpression codeTypeReferenceExpression = new CodeTypeReferenceExpression();
			codePropertyReferenceExpression2.TargetObject = codeTypeReferenceExpression;
			codeTypeReferenceExpression.Type.Options = (CodeTypeReferenceOptions)Enum.Parse(typeof(CodeTypeReferenceOptions), expressionParts[i]);
			i--;
			codeTypeReferenceExpression.Type.BaseType = expressionParts[i];
			for (i--; i > 0; i--)
			{
				codeTypeReferenceExpression.Type.BaseType = expressionParts[i] + "." + codeTypeReferenceExpression.Type.BaseType;
			}
			return codePropertyReferenceExpression;
		}

		private const string applicationSettingsPrefix = "ApplicationSettings";

		private const string appConfigPrefix = "AppConfig";
	}
}
