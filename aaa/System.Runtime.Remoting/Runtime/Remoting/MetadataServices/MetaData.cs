﻿using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using Microsoft.CSharp;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200006C RID: 108
	public class MetaData
	{
		// Token: 0x06000364 RID: 868 RVA: 0x000102ED File Offset: 0x0000F2ED
		public static void ConvertTypesToSchemaToFile(Type[] types, SdlType sdlType, string path)
		{
			MetaData.ConvertTypesToSchemaToStream(types, sdlType, File.Create(path));
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000102FC File Offset: 0x0000F2FC
		public static void ConvertTypesToSchemaToStream(Type[] types, SdlType sdlType, Stream outputStream)
		{
			ServiceType[] array = new ServiceType[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				array[i] = new ServiceType(types[i]);
			}
			MetaData.ConvertTypesToSchemaToStream(array, sdlType, outputStream);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00010333 File Offset: 0x0000F333
		public static void ConvertTypesToSchemaToFile(ServiceType[] types, SdlType sdlType, string path)
		{
			MetaData.ConvertTypesToSchemaToStream(types, sdlType, File.Create(path));
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00010344 File Offset: 0x0000F344
		public static void ConvertTypesToSchemaToStream(ServiceType[] serviceTypes, SdlType sdlType, Stream outputStream)
		{
			if (sdlType == SdlType.Sdl)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Sdl generation is not supported"), new object[0]));
			}
			TextWriter textWriter = new StreamWriter(outputStream, new UTF8Encoding(false, true));
			SUDSGenerator sudsgenerator = new SUDSGenerator(serviceTypes, sdlType, textWriter);
			sudsgenerator.Generate();
			textWriter.Flush();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010398 File Offset: 0x0000F398
		public static void RetrieveSchemaFromUrlToStream(string url, Stream outputStream)
		{
			TextWriter textWriter = new StreamWriter(outputStream, new UTF8Encoding(false, true));
			WebRequest webRequest = WebRequest.Create(url);
			WebResponse response = webRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			textWriter.Write(streamReader.ReadToEnd());
			textWriter.Flush();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000103E3 File Offset: 0x0000F3E3
		public static void RetrieveSchemaFromUrlToFile(string url, string path)
		{
			MetaData.RetrieveSchemaFromUrlToStream(url, File.Create(path));
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000103F4 File Offset: 0x0000F3F4
		public static void ConvertSchemaStreamToCodeSourceStream(bool clientProxy, string outputDirectory, Stream inputStream, ArrayList outCodeStreamList, string proxyUrl, string proxyNamespace)
		{
			TextReader textReader = new StreamReader(inputStream);
			SUDSParser sudsparser = new SUDSParser(textReader, outputDirectory, outCodeStreamList, proxyUrl, clientProxy, proxyNamespace);
			sudsparser.Parse();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001041C File Offset: 0x0000F41C
		public static void ConvertSchemaStreamToCodeSourceStream(bool clientProxy, string outputDirectory, Stream inputStream, ArrayList outCodeStreamList, string proxyUrl)
		{
			MetaData.ConvertSchemaStreamToCodeSourceStream(clientProxy, outputDirectory, inputStream, outCodeStreamList, proxyUrl, "");
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0001042E File Offset: 0x0000F42E
		public static void ConvertSchemaStreamToCodeSourceStream(bool clientProxy, string outputDirectory, Stream inputStream, ArrayList outCodeStreamList)
		{
			MetaData.ConvertSchemaStreamToCodeSourceStream(clientProxy, outputDirectory, inputStream, outCodeStreamList, "", "");
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00010444 File Offset: 0x0000F444
		public static void ConvertCodeSourceStreamToAssemblyFile(ArrayList outCodeStreamList, string assemblyPath, string strongNameFilename)
		{
			CompilerResults compilerResults = null;
			string text = "__Sn.cs";
			try
			{
				if (strongNameFilename != null)
				{
					if (assemblyPath != null)
					{
						int num = assemblyPath.LastIndexOf("\\");
						if (num > 0)
						{
							text = assemblyPath.Substring(0, num + 1) + text;
						}
					}
					FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
					StreamWriter streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false, true));
					streamWriter.WriteLine("// CLR Remoting Autogenerated Key file (to create a key file use: sn -k tmp.key)");
					streamWriter.WriteLine("using System;");
					streamWriter.WriteLine("using System.Reflection;");
					streamWriter.WriteLine("[assembly: AssemblyKeyFile(@\"" + strongNameFilename + "\")]");
					streamWriter.WriteLine("[assembly: AssemblyVersion(@\"1.0.0.1\")]");
					streamWriter.Flush();
					streamWriter.Close();
					fileStream.Close();
					outCodeStreamList.Add(text);
				}
				string[] array = new string[outCodeStreamList.Count];
				string[] array2 = new string[outCodeStreamList.Count];
				int num2 = 0;
				for (int i = 0; i < outCodeStreamList.Count; i++)
				{
					bool flag = false;
					Stream stream;
					if (outCodeStreamList[i] is string)
					{
						string text2 = (string)outCodeStreamList[i];
						array2[i] = text2;
						stream = File.OpenRead(text2);
						flag = true;
					}
					else
					{
						if (!(outCodeStreamList[i] is Stream))
						{
							throw new RemotingException(CoreChannel.GetResourceString("Remoting_UnknownObjectInCodeStreamList"));
						}
						stream = (Stream)outCodeStreamList[i];
						array2[i] = "Stream" + num2++;
					}
					StreamReader streamReader = new StreamReader(stream);
					array[i] = streamReader.ReadToEnd();
					if (flag)
					{
						stream.Close();
					}
				}
				string[] array3 = new string[] { "System.dll", "System.Runtime.Remoting.dll", "System.Data.dll", "System.Xml.dll", "System.Web.Services.dll" };
				if (array.Length > 0)
				{
					CodeDomProvider codeDomProvider = new CSharpCodeProvider();
					compilerResults = codeDomProvider.CompileAssemblyFromSource(new CompilerParameters(array3, assemblyPath, true)
					{
						GenerateExecutable = false
					}, array);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			catch
			{
			}
			finally
			{
				File.Delete(text);
			}
			if (compilerResults.Errors.HasErrors)
			{
				CompilerErrorCollection errors = compilerResults.Errors;
				if (errors.Count > 0)
				{
					foreach (object obj in errors)
					{
						CompilerError compilerError = (CompilerError)obj;
						Console.WriteLine(compilerError.ToString());
					}
				}
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00010724 File Offset: 0x0000F724
		public static void ConvertCodeSourceFileToAssemblyFile(string codePath, string assemblyPath, string strongNameFilename)
		{
			MetaData.ConvertCodeSourceStreamToAssemblyFile(new ArrayList { codePath }, assemblyPath, strongNameFilename);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00010748 File Offset: 0x0000F748
		public static void SaveStreamToFile(Stream inputStream, string path)
		{
			Stream stream = File.Create(path);
			TextWriter textWriter = new StreamWriter(stream, new UTF8Encoding(false, true));
			StreamReader streamReader = new StreamReader(inputStream);
			textWriter.Write(streamReader.ReadToEnd());
			textWriter.Flush();
			textWriter.Close();
			stream.Close();
		}
	}
}
