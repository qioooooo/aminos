using System;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace System.Data.Design
{
	// Token: 0x020000AD RID: 173
	internal class PropertyReferenceSerializer
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x00012B30 File Offset: 0x00011B30
		private PropertyReferenceSerializer()
		{
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00012B38 File Offset: 0x00011B38
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

		// Token: 0x060007FA RID: 2042 RVA: 0x00012B60 File Offset: 0x00011B60
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

		// Token: 0x060007FB RID: 2043 RVA: 0x00012BE0 File Offset: 0x00011BE0
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

		// Token: 0x060007FC RID: 2044 RVA: 0x00012C4C File Offset: 0x00011C4C
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

		// Token: 0x060007FD RID: 2045 RVA: 0x00012CD0 File Offset: 0x00011CD0
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

		// Token: 0x060007FE RID: 2046 RVA: 0x00012D8C File Offset: 0x00011D8C
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

		// Token: 0x060007FF RID: 2047 RVA: 0x00012E64 File Offset: 0x00011E64
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

		// Token: 0x06000800 RID: 2048 RVA: 0x00012FB8 File Offset: 0x00011FB8
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

		// Token: 0x06000801 RID: 2049 RVA: 0x0001306C File Offset: 0x0001206C
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

		// Token: 0x04000BD4 RID: 3028
		private const string applicationSettingsPrefix = "ApplicationSettings";

		// Token: 0x04000BD5 RID: 3029
		private const string appConfigPrefix = "AppConfig";
	}
}
