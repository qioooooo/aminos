using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000AE RID: 174
	internal class JSInProcCompiler
	{
		// Token: 0x060007EB RID: 2027 RVA: 0x00035F30 File Offset: 0x00034F30
		private void AddAssemblyReference(IVsaEngine engine, string filename)
		{
			IVsaReferenceItem vsaReferenceItem = (IVsaReferenceItem)engine.Items.CreateItem(filename, VsaItemType.Reference, VsaItemFlag.None);
			vsaReferenceItem.AssemblyName = filename;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00035F58 File Offset: 0x00034F58
		private void AddDefinition(string def, Hashtable definitions, VsaEngine engine)
		{
			int num = def.IndexOf("=");
			object obj = null;
			string text;
			if (num == -1)
			{
				text = def.Trim();
				obj = true;
			}
			else
			{
				text = def.Substring(0, num).Trim();
				string text2 = def.Substring(num + 1).Trim();
				if (string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
				{
					obj = true;
				}
				else if (string.Compare(text2, "false", StringComparison.OrdinalIgnoreCase) == 0)
				{
					obj = false;
				}
				else
				{
					try
					{
						obj = int.Parse(text2, CultureInfo.InvariantCulture);
					}
					catch
					{
						throw new CmdLineException(CmdLineError.InvalidDefinition, text, engine.ErrorCultureInfo);
					}
				}
			}
			if (text.Length == 0)
			{
				throw new CmdLineException(CmdLineError.MissingDefineArgument, engine.ErrorCultureInfo);
			}
			definitions[text] = obj;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0003602C File Offset: 0x0003502C
		private void AddResourceFile(ResInfo resinfo, Hashtable resources, Hashtable resourceFiles, VsaEngine engine)
		{
			if (!File.Exists(resinfo.fullpath))
			{
				throw new CmdLineException(CmdLineError.ManagedResourceNotFound, resinfo.filename, engine.ErrorCultureInfo);
			}
			if (resourceFiles[resinfo.fullpath] != null)
			{
				throw new CmdLineException(CmdLineError.DuplicateResourceFile, resinfo.filename, engine.ErrorCultureInfo);
			}
			if (resources[resinfo.name] != null)
			{
				throw new CmdLineException(CmdLineError.DuplicateResourceName, resinfo.name, engine.ErrorCultureInfo);
			}
			resources[resinfo.name] = resinfo;
			resourceFiles[resinfo.fullpath] = resinfo;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000360C4 File Offset: 0x000350C4
		private void AddSourceFile(VsaEngine engine, string filename)
		{
			string text = "$SourceFile_" + this.codeItemCounter++;
			IVsaCodeItem vsaCodeItem = (IVsaCodeItem)engine.Items.CreateItem(text, VsaItemType.Code, VsaItemFlag.None);
			vsaCodeItem.SetOption("codebase", filename);
			vsaCodeItem.SourceText = this.ReadFile(filename, engine);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00036120 File Offset: 0x00035120
		internal int Compile(CompilerParameters options, string partialCmdLine, string[] sourceFiles, string outputFile)
		{
			StreamWriter streamWriter = null;
			int num = 0;
			try
			{
				streamWriter = new StreamWriter(outputFile);
				streamWriter.AutoFlush = true;
				if (options.IncludeDebugInformation)
				{
					this.PrintOptions(streamWriter, options);
					this.debugCommandLine = partialCmdLine;
				}
				VsaEngine vsaEngine = null;
				try
				{
					vsaEngine = this.CreateAndInitEngine(options, sourceFiles, outputFile, streamWriter);
				}
				catch (CmdLineException ex)
				{
					streamWriter.WriteLine(ex.Message);
					num = 10;
				}
				catch (Exception ex2)
				{
					streamWriter.WriteLine("fatal error JS2999: " + ex2);
					num = 10;
				}
				catch
				{
					streamWriter.WriteLine("fatal error JS2999");
					num = 10;
				}
				if (vsaEngine == null)
				{
					return num;
				}
				if (options.IncludeDebugInformation)
				{
					StringBuilder stringBuilder = new StringBuilder(this.debugCommandLine);
					foreach (string text in sourceFiles)
					{
						stringBuilder.Append(" \"");
						stringBuilder.Append(text);
						stringBuilder.Append("\"");
					}
					this.debugCommandLine = stringBuilder.ToString();
					string text2 = options.TempFiles.AddExtension("cmdline");
					StreamWriter streamWriter2 = null;
					try
					{
						streamWriter2 = new StreamWriter(text2);
						streamWriter2.WriteLine(this.debugCommandLine);
						streamWriter2.Flush();
					}
					finally
					{
						if (streamWriter2 != null)
						{
							streamWriter2.Close();
						}
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(JScriptException.Localize("CmdLine helper", CultureInfo.CurrentUICulture));
					stringBuilder2.Append(":");
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append("    ");
					stringBuilder2.Append(options.TempFiles.TempDir);
					stringBuilder2.Append("> jsc.exe @\"");
					stringBuilder2.Append(text2);
					stringBuilder2.Append("\"");
					stringBuilder2.Append(Environment.NewLine);
					streamWriter.WriteLine(stringBuilder2.ToString());
					this.PrintBanner(vsaEngine, streamWriter);
				}
				try
				{
					if (!vsaEngine.Compile())
					{
						num = 10;
					}
					else
					{
						num = 0;
					}
				}
				catch (VsaException ex3)
				{
					if (ex3.ErrorCode == VsaError.AssemblyExpected)
					{
						if (ex3.InnerException != null && ex3.InnerException is BadImageFormatException)
						{
							CmdLineException ex4 = new CmdLineException(CmdLineError.InvalidAssembly, ex3.Message, vsaEngine.ErrorCultureInfo);
							streamWriter.WriteLine(ex4.Message);
						}
						else if (ex3.InnerException != null && ex3.InnerException is FileNotFoundException)
						{
							CmdLineException ex5 = new CmdLineException(CmdLineError.AssemblyNotFound, ex3.Message, vsaEngine.ErrorCultureInfo);
							streamWriter.WriteLine(ex5.Message);
						}
						else
						{
							CmdLineException ex6 = new CmdLineException(CmdLineError.InvalidAssembly, vsaEngine.ErrorCultureInfo);
							streamWriter.WriteLine(ex6.Message);
						}
					}
					else if (ex3.ErrorCode == VsaError.SaveCompiledStateFailed)
					{
						CmdLineException ex7 = new CmdLineException(CmdLineError.ErrorSavingCompiledState, ex3.Message, vsaEngine.ErrorCultureInfo);
						streamWriter.WriteLine(ex7.Message);
					}
					else
					{
						streamWriter.WriteLine(JScriptException.Localize("INTERNAL COMPILER ERROR", vsaEngine.ErrorCultureInfo));
						streamWriter.WriteLine(ex3);
					}
					num = 10;
				}
				catch (Exception ex8)
				{
					streamWriter.WriteLine(JScriptException.Localize("INTERNAL COMPILER ERROR", vsaEngine.ErrorCultureInfo));
					streamWriter.WriteLine(ex8);
					num = 10;
				}
				catch
				{
					streamWriter.WriteLine(JScriptException.Localize("INTERNAL COMPILER ERROR", vsaEngine.ErrorCultureInfo));
					num = 10;
				}
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
			return num;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00036538 File Offset: 0x00035538
		private VsaEngine CreateAndInitEngine(CompilerParameters options, string[] sourceFiles, string outputFile, TextWriter output)
		{
			VsaEngine vsaEngine = new VsaEngine(true);
			VsaSite vsaSite = new VsaSite(output);
			vsaEngine.InitVsaEngine("JSCodeGenerator://Microsoft.JScript.Vsa.VsaEngine", vsaSite);
			this.ValidateOptions(options, vsaEngine);
			vsaEngine.GenerateDebugInfo = options.IncludeDebugInformation;
			vsaEngine.SetOption("referenceLoaderAPI", "LoadFile");
			vsaEngine.SetOption("fast", true);
			vsaEngine.SetOption("print", false);
			vsaEngine.SetOption("VersionSafe", false);
			vsaEngine.SetOption("output", options.OutputAssembly);
			if (options.GenerateExecutable)
			{
				vsaEngine.SetOption("PEFileKind", PEFileKinds.ConsoleApplication);
			}
			else
			{
				vsaEngine.SetOption("PEFileKind", PEFileKinds.Dll);
			}
			vsaSite.treatWarningsAsErrors = options.TreatWarningsAsErrors;
			vsaEngine.SetOption("warnaserror", options.TreatWarningsAsErrors);
			vsaSite.warningLevel = options.WarningLevel;
			vsaEngine.SetOption("WarningLevel", options.WarningLevel);
			if (options.Win32Resource != null && options.Win32Resource.Length > 0)
			{
				vsaEngine.SetOption("win32resource", options.Win32Resource);
			}
			bool flag = false;
			foreach (string text in options.ReferencedAssemblies)
			{
				if (string.Compare(Path.GetFileName(text), "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
				}
				this.AddAssemblyReference(vsaEngine, text);
			}
			if (!flag)
			{
				this.AddAssemblyReference(vsaEngine, "mscorlib.dll");
			}
			StringCollection stringCollection = this.SplitCmdLineArguments(options.CompilerOptions);
			this.ParseCompilerOptions(vsaEngine, stringCollection, output, options.GenerateExecutable);
			for (int i = 0; i < sourceFiles.Length; i++)
			{
				this.AddSourceFile(vsaEngine, sourceFiles[i]);
			}
			return vsaEngine;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00036714 File Offset: 0x00035714
		private void GetAllDefines(string definitionList, Hashtable defines, VsaEngine engine)
		{
			int num = 0;
			int argumentSeparatorIndex;
			do
			{
				argumentSeparatorIndex = this.GetArgumentSeparatorIndex(definitionList, num);
				string text;
				if (argumentSeparatorIndex == -1)
				{
					text = definitionList.Substring(num);
				}
				else
				{
					text = definitionList.Substring(num, argumentSeparatorIndex - num);
				}
				this.AddDefinition(text, defines, engine);
				num = argumentSeparatorIndex + 1;
			}
			while (argumentSeparatorIndex > -1);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00036758 File Offset: 0x00035758
		private int GetArgumentSeparatorIndex(string argList, int startIndex)
		{
			int num = argList.IndexOf(",", startIndex);
			int num2 = argList.IndexOf(";", startIndex);
			if (num == -1)
			{
				return num2;
			}
			if (num2 == -1)
			{
				return num;
			}
			if (num < num2)
			{
				return num;
			}
			return num2;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00036794 File Offset: 0x00035794
		private void ParseCompilerOptions(VsaEngine engine, StringCollection args, TextWriter output, bool generateExe)
		{
			string text = Environment.GetEnvironmentVariable("LIB");
			bool flag = false;
			Hashtable hashtable = new Hashtable(10);
			Hashtable hashtable2 = new Hashtable(10);
			Hashtable hashtable3 = new Hashtable(10);
			bool flag2 = false;
			StringBuilder stringBuilder = null;
			if (this.debugCommandLine != null)
			{
				stringBuilder = new StringBuilder(this.debugCommandLine);
			}
			string text2 = ((Path.DirectorySeparatorChar == '/') ? "-" : "/");
			int i = 0;
			int count = args.Count;
			while (i < count)
			{
				string text3 = args[i];
				if (text3 != null && text3.Length != 0)
				{
					if (text3[0] == '@')
					{
						throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "@<filename>", engine.ErrorCultureInfo);
					}
					if ('-' == text3[0] || ('/' == text3[0] && Path.DirectorySeparatorChar != '/'))
					{
						string text4 = text3.Substring(1);
						if (text4.Length > 0)
						{
							char c = text4[0];
							switch (c)
							{
							case '?':
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/?", engine.ErrorCultureInfo);
							case '@':
							case 'B':
							case 'E':
							case 'G':
							case 'H':
							case 'I':
							case 'J':
							case 'K':
							case 'M':
							case 'Q':
							case 'S':
								goto IL_09B4;
							case 'A':
								break;
							case 'C':
								goto IL_0223;
							case 'D':
								goto IL_024E;
							case 'F':
								goto IL_02F5;
							case 'L':
								goto IL_033A;
							case 'N':
								goto IL_0488;
							case 'O':
								goto IL_04DB;
							case 'P':
								goto IL_0506;
							case 'R':
								goto IL_0631;
							case 'T':
								goto IL_06AD;
							case 'U':
								goto IL_07C8;
							case 'V':
								goto IL_07F3;
							case 'W':
								goto IL_0838;
							default:
								switch (c)
								{
								case 'a':
									break;
								case 'b':
								case 'e':
								case 'g':
								case 'h':
								case 'i':
								case 'j':
								case 'k':
								case 'm':
								case 'q':
								case 's':
									goto IL_09B4;
								case 'c':
									goto IL_0223;
								case 'd':
									goto IL_024E;
								case 'f':
									goto IL_02F5;
								case 'l':
									goto IL_033A;
								case 'n':
									goto IL_0488;
								case 'o':
									goto IL_04DB;
								case 'p':
									goto IL_0506;
								case 'r':
									goto IL_0631;
								case 't':
									goto IL_06AD;
								case 'u':
									goto IL_07C8;
								case 'v':
									goto IL_07F3;
								case 'w':
									goto IL_0838;
								default:
									goto IL_09B4;
								}
								break;
							}
							object obj = CmdLineOptionParser.IsBooleanOption(text4, "autoref");
							if (obj == null)
							{
								goto IL_09B4;
							}
							engine.SetOption("autoref", obj);
							if (stringBuilder != null)
							{
								stringBuilder.Append(text3);
								stringBuilder.Append(" ");
								goto IL_09C7;
							}
							goto IL_09C7;
							IL_024E:
							obj = CmdLineOptionParser.IsBooleanOption(text4, "debug");
							if (obj != null)
							{
								engine.GenerateDebugInfo = (bool)obj;
								if (stringBuilder != null)
								{
									stringBuilder.Append(text3);
									stringBuilder.Append(" ");
									goto IL_09C7;
								}
								goto IL_09C7;
							}
							else
							{
								obj = CmdLineOptionParser.IsArgumentOption(text4, "d", "define");
								if (obj == null)
								{
									goto IL_09B4;
								}
								this.GetAllDefines((string)obj, hashtable, engine);
								if (stringBuilder != null)
								{
									stringBuilder.Append(text2 + "d:\"");
									stringBuilder.Append((string)obj);
									stringBuilder.Append("\" ");
									goto IL_09C7;
								}
								goto IL_09C7;
							}
							IL_02F5:
							obj = CmdLineOptionParser.IsBooleanOption(text4, "fast");
							if (obj == null)
							{
								goto IL_09B4;
							}
							engine.SetOption("fast", obj);
							if (stringBuilder != null)
							{
								stringBuilder.Append(text3);
								stringBuilder.Append(" ");
								goto IL_09C7;
							}
							goto IL_09C7;
							IL_033A:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "lcid");
							if (obj != null)
							{
								if (((string)obj).Length == 0)
								{
									throw new CmdLineException(CmdLineError.NoLocaleID, text3, engine.ErrorCultureInfo);
								}
								try
								{
									engine.LCID = int.Parse((string)obj, CultureInfo.InvariantCulture);
									goto IL_09C7;
								}
								catch
								{
									throw new CmdLineException(CmdLineError.InvalidLocaleID, (string)obj, engine.ErrorCultureInfo);
								}
							}
							obj = CmdLineOptionParser.IsArgumentOption(text4, "lib");
							if (obj != null)
							{
								string text5 = (string)obj;
								if (text5.Length == 0)
								{
									throw new CmdLineException(CmdLineError.MissingLibArgument, engine.ErrorCultureInfo);
								}
								text5 = text5.Replace(',', Path.PathSeparator);
								text = text5 + Path.PathSeparator + text;
								if (stringBuilder != null)
								{
									stringBuilder.Append(text2 + "lib:\"");
									stringBuilder.Append((string)obj);
									stringBuilder.Append("\" ");
									goto IL_09C7;
								}
								goto IL_09C7;
							}
							else
							{
								obj = CmdLineOptionParser.IsArgumentOption(text4, "linkres", "linkresource");
								if (obj != null)
								{
									try
									{
										ResInfo resInfo = new ResInfo((string)obj, true);
										this.AddResourceFile(resInfo, hashtable2, hashtable3, engine);
										goto IL_09C7;
									}
									catch (CmdLineException)
									{
										throw;
									}
									catch
									{
										throw new CmdLineException(CmdLineError.ManagedResourceNotFound, engine.ErrorCultureInfo);
									}
									goto IL_0488;
								}
								goto IL_09B4;
							}
							IL_0506:
							obj = CmdLineOptionParser.IsBooleanOption(text4, "print");
							if (obj != null)
							{
								engine.SetOption("print", obj);
								if (stringBuilder != null)
								{
									stringBuilder.Append(text3);
									stringBuilder.Append(" ");
									goto IL_09C7;
								}
								goto IL_09C7;
							}
							else
							{
								obj = CmdLineOptionParser.IsArgumentOption(text4, "platform");
								if (obj == null)
								{
									goto IL_09B4;
								}
								string text6 = (string)obj;
								PortableExecutableKinds portableExecutableKinds;
								ImageFileMachine imageFileMachine;
								if (string.Compare(text6, "x86", StringComparison.OrdinalIgnoreCase) == 0)
								{
									portableExecutableKinds = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit;
									imageFileMachine = ImageFileMachine.I386;
								}
								else if (string.Compare(text6, "Itanium", StringComparison.OrdinalIgnoreCase) == 0)
								{
									portableExecutableKinds = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
									imageFileMachine = ImageFileMachine.IA64;
								}
								else if (string.Compare(text6, "x64", StringComparison.OrdinalIgnoreCase) == 0)
								{
									portableExecutableKinds = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
									imageFileMachine = ImageFileMachine.AMD64;
								}
								else
								{
									if (string.Compare(text6, "anycpu", StringComparison.OrdinalIgnoreCase) != 0)
									{
										throw new CmdLineException(CmdLineError.InvalidPlatform, (string)obj, engine.ErrorCultureInfo);
									}
									portableExecutableKinds = PortableExecutableKinds.ILOnly;
									imageFileMachine = ImageFileMachine.I386;
								}
								engine.SetOption("PortableExecutableKind", portableExecutableKinds);
								engine.SetOption("ImageFileMachine", imageFileMachine);
								if (stringBuilder != null)
								{
									stringBuilder.Append(text3);
									stringBuilder.Append(" ");
									goto IL_09C7;
								}
								goto IL_09C7;
							}
							IL_0631:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "r", "reference");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/r[eference]:<file list>", engine.ErrorCultureInfo);
							}
							obj = CmdLineOptionParser.IsArgumentOption(text4, "res", "resource");
							if (obj == null)
							{
								goto IL_09B4;
							}
							try
							{
								ResInfo resInfo2 = new ResInfo((string)obj, false);
								this.AddResourceFile(resInfo2, hashtable2, hashtable3, engine);
								goto IL_09C7;
							}
							catch (CmdLineException)
							{
								throw;
							}
							catch
							{
								throw new CmdLineException(CmdLineError.ManagedResourceNotFound, engine.ErrorCultureInfo);
							}
							IL_06AD:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "t", "target");
							if (obj == null)
							{
								goto IL_09B4;
							}
							if (string.Compare((string)obj, "exe", StringComparison.OrdinalIgnoreCase) == 0)
							{
								if (!generateExe)
								{
									throw new CmdLineException(CmdLineError.IncompatibleTargets, text3, engine.ErrorCultureInfo);
								}
								if (flag2)
								{
									throw new CmdLineException(CmdLineError.MultipleTargets, engine.ErrorCultureInfo);
								}
								flag2 = true;
								goto IL_09C7;
							}
							else if (string.Compare((string)obj, "winexe", StringComparison.OrdinalIgnoreCase) == 0)
							{
								if (!generateExe)
								{
									throw new CmdLineException(CmdLineError.IncompatibleTargets, text3, engine.ErrorCultureInfo);
								}
								if (flag2)
								{
									throw new CmdLineException(CmdLineError.MultipleTargets, engine.ErrorCultureInfo);
								}
								engine.SetOption("PEFileKind", PEFileKinds.WindowApplication);
								flag = true;
								flag2 = true;
								goto IL_09C7;
							}
							else
							{
								if (string.Compare((string)obj, "library", StringComparison.OrdinalIgnoreCase) != 0)
								{
									throw new CmdLineException(CmdLineError.InvalidTarget, (string)obj, engine.ErrorCultureInfo);
								}
								if (generateExe)
								{
									throw new CmdLineException(CmdLineError.IncompatibleTargets, engine.ErrorCultureInfo);
								}
								if (flag2)
								{
									throw new CmdLineException(CmdLineError.MultipleTargets, engine.ErrorCultureInfo);
								}
								flag2 = true;
								goto IL_09C7;
							}
							IL_07F3:
							obj = CmdLineOptionParser.IsBooleanOption(text4, "VersionSafe");
							if (obj == null)
							{
								goto IL_09B4;
							}
							engine.SetOption("VersionSafe", obj);
							if (stringBuilder != null)
							{
								stringBuilder.Append(text3);
								stringBuilder.Append(" ");
								goto IL_09C7;
							}
							goto IL_09C7;
							IL_0838:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "w", "warn");
							if (obj != null)
							{
								if (((string)obj).Length == 0)
								{
									throw new CmdLineException(CmdLineError.NoWarningLevel, text3, engine.ErrorCultureInfo);
								}
								if (((string)obj).Length == 1)
								{
									if (stringBuilder != null)
									{
										stringBuilder.Append(text3);
										stringBuilder.Append(" ");
									}
									switch (((string)obj)[0])
									{
									case '0':
										engine.SetOption("WarningLevel", 0);
										goto IL_09C7;
									case '1':
										engine.SetOption("WarningLevel", 1);
										goto IL_09C7;
									case '2':
										engine.SetOption("WarningLevel", 2);
										goto IL_09C7;
									case '3':
										engine.SetOption("WarningLevel", 3);
										goto IL_09C7;
									case '4':
										engine.SetOption("WarningLevel", 4);
										goto IL_09C7;
									}
								}
								throw new CmdLineException(CmdLineError.InvalidWarningLevel, text3, engine.ErrorCultureInfo);
							}
							else
							{
								obj = CmdLineOptionParser.IsBooleanOption(text4, "warnaserror");
								if (obj != null)
								{
									engine.SetOption("warnaserror", obj);
									if (stringBuilder != null)
									{
										stringBuilder.Append(text3);
										stringBuilder.Append(" ");
										goto IL_09C7;
									}
									goto IL_09C7;
								}
								else
								{
									obj = CmdLineOptionParser.IsArgumentOption(text4, "win32res");
									if (obj != null)
									{
										throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/win32res:<filename>", engine.ErrorCultureInfo);
									}
									goto IL_09B4;
								}
							}
							IL_0223:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "codepage");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/codepage:<id>", engine.ErrorCultureInfo);
							}
							goto IL_09B4;
							IL_0488:
							obj = CmdLineOptionParser.IsBooleanOption(text4, "nologo");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/nologo[+|-]", engine.ErrorCultureInfo);
							}
							obj = CmdLineOptionParser.IsBooleanOption(text4, "nostdlib");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/nostdlib[+|-]", engine.ErrorCultureInfo);
							}
							goto IL_09B4;
							IL_04DB:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "out");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/out:<filename>", engine.ErrorCultureInfo);
							}
							goto IL_09B4;
							IL_07C8:
							obj = CmdLineOptionParser.IsArgumentOption(text4, "utf8output");
							if (obj != null)
							{
								throw new CmdLineException(CmdLineError.InvalidForCompilerOptions, "/utf8output[+|-]", engine.ErrorCultureInfo);
							}
						}
						IL_09B4:
						throw new CmdLineException(CmdLineError.UnknownOption, text3, engine.ErrorCultureInfo);
					}
					break;
				}
				IL_09C7:
				i++;
			}
			if (stringBuilder != null)
			{
				if (generateExe)
				{
					if (flag)
					{
						stringBuilder.Append(text2 + "t:winexe ");
					}
					else
					{
						stringBuilder.Append(text2 + "t:exe ");
					}
				}
				else
				{
					stringBuilder.Append(text2 + "t:library ");
				}
				this.debugCommandLine = stringBuilder.ToString();
			}
			engine.SetOption("libpath", text);
			engine.SetOption("defines", hashtable);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00037228 File Offset: 0x00036228
		internal void PrintBanner(VsaEngine engine, TextWriter output)
		{
			string text = string.Concat(new string[]
			{
				8.ToString(CultureInfo.InvariantCulture),
				".",
				0.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
				".",
				50727.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')
			});
			Version version = Environment.Version;
			string text2 = string.Concat(new string[]
			{
				version.Major.ToString(CultureInfo.InvariantCulture),
				".",
				version.Minor.ToString(CultureInfo.InvariantCulture),
				".",
				version.Build.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')
			});
			output.WriteLine(string.Format(engine.ErrorCultureInfo, JScriptException.Localize("Banner line 1", engine.ErrorCultureInfo), new object[] { text }));
			output.WriteLine(string.Format(engine.ErrorCultureInfo, JScriptException.Localize("Banner line 2", engine.ErrorCultureInfo), new object[] { text2 }));
			output.WriteLine(JScriptException.Localize("Banner line 3", engine.ErrorCultureInfo) + Environment.NewLine);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00037398 File Offset: 0x00036398
		private void PrintOptions(TextWriter output, CompilerParameters options)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CompilerParameters.CompilerOptions        : \"");
			stringBuilder.Append(options.CompilerOptions);
			stringBuilder.Append("\"");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.GenerateExecutable     : ");
			stringBuilder.Append(options.GenerateExecutable.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.GenerateInMemory       : ");
			stringBuilder.Append(options.GenerateInMemory.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.IncludeDebugInformation: ");
			stringBuilder.Append(options.IncludeDebugInformation.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.MainClass              : \"");
			stringBuilder.Append(options.MainClass);
			stringBuilder.Append("\"");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.OutputAssembly         : \"");
			stringBuilder.Append(options.OutputAssembly);
			stringBuilder.Append("\"");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.ReferencedAssemblies   : ");
			foreach (string text in options.ReferencedAssemblies)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("        \"");
				stringBuilder.Append(text);
				stringBuilder.Append("\"");
			}
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.TreatWarningsAsErrors  : ");
			stringBuilder.Append(options.TreatWarningsAsErrors.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.WarningLevel           : ");
			stringBuilder.Append(options.WarningLevel.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("CompilerParameters.Win32Resource          : \"");
			stringBuilder.Append(options.Win32Resource);
			stringBuilder.Append("\"");
			stringBuilder.Append(Environment.NewLine);
			output.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00037604 File Offset: 0x00036604
		protected string ReadFile(string fileName, VsaEngine engine)
		{
			string text = "";
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (ArgumentException)
			{
				throw new CmdLineException(CmdLineError.InvalidCharacters, fileName, engine.ErrorCultureInfo);
			}
			catch (FileNotFoundException)
			{
				throw new CmdLineException(CmdLineError.SourceNotFound, fileName, engine.ErrorCultureInfo);
			}
			try
			{
				if (fileStream.Length != 0L)
				{
					StreamReader streamReader = new StreamReader(fileStream, true);
					try
					{
						text = streamReader.ReadToEnd();
					}
					finally
					{
						streamReader.Close();
					}
				}
			}
			finally
			{
				fileStream.Close();
			}
			return text;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000376AC File Offset: 0x000366AC
		private StringCollection SplitCmdLineArguments(string argumentString)
		{
			StringCollection stringCollection = new StringCollection();
			if (argumentString == null || argumentString.Length == 0)
			{
				return stringCollection;
			}
			string text = "\\s*([^\\s\\\"]|(\\\"[^\\\"\\n]*\\\"))+";
			Regex regex = new Regex(text);
			MatchCollection matchCollection = regex.Matches(argumentString);
			if (matchCollection != null && matchCollection.Count != 0)
			{
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					string text2 = match.ToString().Trim();
					int num = 0;
					while ((num = text2.IndexOf("\"", num)) != -1)
					{
						if (num == 0)
						{
							text2 = text2.Substring(1);
						}
						else if (text2[num - 1] == '\\')
						{
							num++;
						}
						else
						{
							text2 = text2.Remove(num, 1);
						}
					}
					stringCollection.Add(text2);
				}
			}
			return stringCollection;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000377A0 File Offset: 0x000367A0
		private void ValidateOptions(CompilerParameters options, VsaEngine engine)
		{
			string outputAssembly = options.OutputAssembly;
			try
			{
				if (Path.GetFileName(outputAssembly).Length == 0)
				{
					throw new CmdLineException(CmdLineError.NoFileName, outputAssembly, engine.ErrorCultureInfo);
				}
			}
			catch (ArgumentException)
			{
				throw new CmdLineException(CmdLineError.NoFileName, engine.ErrorCultureInfo);
			}
			if (Path.GetExtension(outputAssembly).Length == 0)
			{
				throw new CmdLineException(CmdLineError.MissingExtension, outputAssembly, engine.ErrorCultureInfo);
			}
			if (options.WarningLevel == -1)
			{
				options.WarningLevel = 4;
			}
			if (options.WarningLevel < 0 || options.WarningLevel > 4)
			{
				throw new CmdLineException(CmdLineError.InvalidWarningLevel, options.WarningLevel.ToString(CultureInfo.InvariantCulture), engine.ErrorCultureInfo);
			}
			if (options.Win32Resource != null && options.Win32Resource.Length > 0 && !File.Exists(options.Win32Resource))
			{
				throw new CmdLineException(CmdLineError.ResourceNotFound, options.Win32Resource, engine.ErrorCultureInfo);
			}
		}

		// Token: 0x04000444 RID: 1092
		private int codeItemCounter;

		// Token: 0x04000445 RID: 1093
		private string debugCommandLine;
	}
}
