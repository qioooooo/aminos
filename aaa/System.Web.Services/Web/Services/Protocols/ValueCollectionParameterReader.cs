using System;
using System.Collections.Specialized;
using System.Reflection;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000036 RID: 54
	public abstract class ValueCollectionParameterReader : MimeParameterReader
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00005345 File Offset: 0x00004345
		public override void Initialize(object o)
		{
			this.paramInfos = (ParameterInfo[])o;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005353 File Offset: 0x00004353
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			if (!ValueCollectionParameterReader.IsSupported(methodInfo))
			{
				return null;
			}
			return methodInfo.InParameters;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005368 File Offset: 0x00004368
		protected object[] Read(NameValueCollection collection)
		{
			object[] array = new object[this.paramInfos.Length];
			for (int i = 0; i < this.paramInfos.Length; i++)
			{
				ParameterInfo parameterInfo = this.paramInfos[i];
				if (parameterInfo.ParameterType.IsArray)
				{
					string[] values = collection.GetValues(parameterInfo.Name);
					Type elementType = parameterInfo.ParameterType.GetElementType();
					Array array2 = Array.CreateInstance(elementType, values.Length);
					for (int j = 0; j < values.Length; j++)
					{
						string text = values[j];
						array2.SetValue(ScalarFormatter.FromString(text, elementType), j);
					}
					array[i] = array2;
				}
				else
				{
					string text2 = collection[parameterInfo.Name];
					if (text2 == null)
					{
						throw new InvalidOperationException(Res.GetString("WebMissingParameter", new object[] { parameterInfo.Name }));
					}
					array[i] = ScalarFormatter.FromString(text2, parameterInfo.ParameterType);
				}
			}
			return array;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005450 File Offset: 0x00004450
		public static bool IsSupported(LogicalMethodInfo methodInfo)
		{
			if (methodInfo.OutParameters.Length > 0)
			{
				return false;
			}
			ParameterInfo[] inParameters = methodInfo.InParameters;
			for (int i = 0; i < inParameters.Length; i++)
			{
				if (!ValueCollectionParameterReader.IsSupported(inParameters[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000548C File Offset: 0x0000448C
		public static bool IsSupported(ParameterInfo paramInfo)
		{
			Type type = paramInfo.ParameterType;
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			return ScalarFormatter.IsTypeSupported(type);
		}

		// Token: 0x0400027B RID: 635
		private ParameterInfo[] paramInfos;
	}
}
