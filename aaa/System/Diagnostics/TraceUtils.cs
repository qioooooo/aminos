using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Diagnostics
{
	// Token: 0x020001DF RID: 479
	internal static class TraceUtils
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x00030E5C File Offset: 0x0002FE5C
		internal static object GetRuntimeObject(string className, Type baseType, string initializeData)
		{
			object obj = null;
			if (className.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("EmptyTypeName_NotAllowed"));
			}
			Type type = Type.GetType(className);
			if (type == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_find_type", new object[] { className }));
			}
			if (!baseType.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Incorrect_base_type", new object[] { className, baseType.FullName }));
			}
			Exception ex = null;
			try
			{
				if (string.IsNullOrEmpty(initializeData))
				{
					if (TraceUtils.IsOwnedTextWriterTL(type))
					{
						throw new ConfigurationErrorsException(SR.GetString("TextWriterTL_DefaultConstructor_NotSupported"));
					}
					ConstructorInfo constructor = type.GetConstructor(new Type[0]);
					if (constructor == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Could_not_get_constructor", new object[] { className }));
					}
					obj = constructor.Invoke(new object[0]);
				}
				else
				{
					ConstructorInfo constructor2 = type.GetConstructor(new Type[] { typeof(string) });
					if (constructor2 != null)
					{
						if (TraceUtils.IsOwnedTextWriterTL(type) && initializeData[0] != Path.DirectorySeparatorChar && initializeData[0] != Path.AltDirectorySeparatorChar && !Path.IsPathRooted(initializeData))
						{
							string configFilePath = DiagnosticsConfiguration.ConfigFilePath;
							if (!string.IsNullOrEmpty(configFilePath))
							{
								string directoryName = Path.GetDirectoryName(configFilePath);
								if (directoryName != null)
								{
									initializeData = Path.Combine(directoryName, initializeData);
								}
							}
						}
						obj = constructor2.Invoke(new object[] { initializeData });
					}
					else
					{
						ConstructorInfo[] constructors = type.GetConstructors();
						if (constructors == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Could_not_get_constructor", new object[] { className }));
						}
						for (int i = 0; i < constructors.Length; i++)
						{
							ParameterInfo[] parameters = constructors[i].GetParameters();
							if (parameters.Length == 1)
							{
								Type parameterType = parameters[0].ParameterType;
								try
								{
									object obj2 = TraceUtils.ConvertToBaseTypeOrEnum(initializeData, parameterType);
									obj = constructors[i].Invoke(new object[] { obj2 });
									break;
								}
								catch (TargetInvocationException ex2)
								{
									ex = ex2.InnerException;
								}
								catch (Exception ex3)
								{
									ex = ex3;
								}
								catch
								{
								}
							}
						}
					}
				}
			}
			catch (TargetInvocationException ex4)
			{
				ex = ex4.InnerException;
			}
			if (obj != null)
			{
				return obj;
			}
			if (ex != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_create_type_instance", new object[] { className }), ex);
			}
			throw new ConfigurationErrorsException(SR.GetString("Could_not_create_type_instance", new object[] { className }));
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00031128 File Offset: 0x00030128
		internal static bool IsOwnedTextWriterTL(Type type)
		{
			return typeof(XmlWriterTraceListener) == type || typeof(DelimitedListTraceListener) == type || typeof(TextWriterTraceListener) == type;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00031153 File Offset: 0x00030153
		private static object ConvertToBaseTypeOrEnum(string value, Type type)
		{
			if (type.IsEnum)
			{
				return Enum.Parse(type, value, false);
			}
			return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00031174 File Offset: 0x00030174
		internal static void VerifyAttributes(IDictionary attributes, string[] supportedAttributes, object parent)
		{
			foreach (object obj in attributes.Keys)
			{
				string text = (string)obj;
				bool flag = false;
				if (supportedAttributes != null)
				{
					for (int i = 0; i < supportedAttributes.Length; i++)
					{
						if (supportedAttributes[i] == text)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					throw new ConfigurationErrorsException(SR.GetString("AttributeNotSupported", new object[]
					{
						text,
						parent.GetType().FullName
					}));
				}
			}
		}
	}
}
