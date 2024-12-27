using System;
using System.Collections;
using System.Web.Services.Configuration;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000048 RID: 72
	internal class HttpServerType : ServerType
	{
		// Token: 0x0600017C RID: 380 RVA: 0x000065AC File Offset: 0x000055AC
		internal HttpServerType(Type type)
			: base(type)
		{
			WebServicesSection webServicesSection = WebServicesSection.Current;
			Type[] returnWriterTypes = webServicesSection.ReturnWriterTypes;
			Type[] parameterReaderTypes = webServicesSection.ParameterReaderTypes;
			LogicalMethodInfo[] array = WebMethodReflector.GetMethods(type);
			HttpServerMethod[] array2 = new HttpServerMethod[array.Length];
			object[] array3 = new object[returnWriterTypes.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = MimeFormatter.GetInitializers(returnWriterTypes[i], array);
			}
			for (int j = 0; j < array.Length; j++)
			{
				LogicalMethodInfo logicalMethodInfo = array[j];
				HttpServerMethod httpServerMethod = null;
				if (logicalMethodInfo.ReturnType == typeof(void))
				{
					httpServerMethod = new HttpServerMethod();
				}
				else
				{
					for (int k = 0; k < returnWriterTypes.Length; k++)
					{
						object[] array4 = (object[])array3[k];
						if (array4[j] != null)
						{
							httpServerMethod = new HttpServerMethod();
							httpServerMethod.writerInitializer = array4[j];
							httpServerMethod.writerType = returnWriterTypes[k];
							break;
						}
					}
				}
				if (httpServerMethod != null)
				{
					httpServerMethod.methodInfo = logicalMethodInfo;
					array2[j] = httpServerMethod;
				}
			}
			array3 = new object[parameterReaderTypes.Length];
			for (int l = 0; l < array3.Length; l++)
			{
				array3[l] = MimeFormatter.GetInitializers(parameterReaderTypes[l], array);
			}
			for (int m = 0; m < array.Length; m++)
			{
				HttpServerMethod httpServerMethod2 = array2[m];
				if (httpServerMethod2 != null)
				{
					LogicalMethodInfo logicalMethodInfo2 = array[m];
					if (logicalMethodInfo2.InParameters.Length > 0)
					{
						int num = 0;
						for (int n = 0; n < parameterReaderTypes.Length; n++)
						{
							object[] array5 = (object[])array3[n];
							if (array5[m] != null)
							{
								num++;
							}
						}
						if (num == 0)
						{
							array2[m] = null;
						}
						else
						{
							httpServerMethod2.readerTypes = new Type[num];
							httpServerMethod2.readerInitializers = new object[num];
							num = 0;
							for (int num2 = 0; num2 < parameterReaderTypes.Length; num2++)
							{
								object[] array6 = (object[])array3[num2];
								if (array6[m] != null)
								{
									httpServerMethod2.readerTypes[num] = parameterReaderTypes[num2];
									httpServerMethod2.readerInitializers[num] = array6[m];
									num++;
								}
							}
						}
					}
				}
			}
			foreach (HttpServerMethod httpServerMethod3 in array2)
			{
				if (httpServerMethod3 != null)
				{
					WebMethodAttribute methodAttribute = httpServerMethod3.methodInfo.MethodAttribute;
					httpServerMethod3.name = methodAttribute.MessageName;
					if (httpServerMethod3.name.Length == 0)
					{
						httpServerMethod3.name = httpServerMethod3.methodInfo.Name;
					}
					this.methods.Add(httpServerMethod3.name, httpServerMethod3);
				}
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006829 File Offset: 0x00005829
		internal HttpServerMethod GetMethod(string name)
		{
			return (HttpServerMethod)this.methods[name];
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000683C File Offset: 0x0000583C
		internal HttpServerMethod GetMethodIgnoreCase(string name)
		{
			foreach (object obj in this.methods)
			{
				HttpServerMethod httpServerMethod = (HttpServerMethod)((DictionaryEntry)obj).Value;
				if (string.Compare(httpServerMethod.name, name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return httpServerMethod;
				}
			}
			return null;
		}

		// Token: 0x0400028F RID: 655
		private Hashtable methods = new Hashtable();
	}
}
