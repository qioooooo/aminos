using System;
using System.Collections;
using System.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200018F RID: 399
	public class AxImporter
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x0003D68F File Offset: 0x0003C68F
		public AxImporter(AxImporter.Options options)
		{
			this.options = options;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0003D6A0 File Offset: 0x0003C6A0
		public string[] GeneratedAssemblies
		{
			get
			{
				if (this.genAssems == null || this.genAssems.Count <= 0)
				{
					return new string[0];
				}
				string[] array = new string[this.genAssems.Count];
				for (int i = 0; i < this.genAssems.Count; i++)
				{
					array[i] = (string)this.genAssems[i];
				}
				return array;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0003D708 File Offset: 0x0003C708
		public global::System.Runtime.InteropServices.TYPELIBATTR[] GeneratedTypeLibAttributes
		{
			get
			{
				if (this.tlbAttrs == null)
				{
					return new global::System.Runtime.InteropServices.TYPELIBATTR[0];
				}
				global::System.Runtime.InteropServices.TYPELIBATTR[] array = new global::System.Runtime.InteropServices.TYPELIBATTR[this.tlbAttrs.Count];
				for (int i = 0; i < this.tlbAttrs.Count; i++)
				{
					array[i] = (global::System.Runtime.InteropServices.TYPELIBATTR)this.tlbAttrs[i];
				}
				return array;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0003D76C File Offset: 0x0003C76C
		public string[] GeneratedSources
		{
			get
			{
				if (this.options.genSources)
				{
					string[] array = new string[this.generatedSources.Count];
					for (int i = 0; i < this.generatedSources.Count; i++)
					{
						array[i] = (string)this.generatedSources[i];
					}
					return array;
				}
				return null;
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0003D7C4 File Offset: 0x0003C7C4
		private void AddDependentAssemblies(Assembly assem, string assemPath)
		{
			AssemblyName[] referencedAssemblies = assem.GetReferencedAssemblies();
			foreach (AssemblyName assemblyName in referencedAssemblies)
			{
				if (!string.Equals(assemblyName.Name, "mscorlib", StringComparison.OrdinalIgnoreCase))
				{
					string text = this.GetComReference(assemblyName);
					if (text == null)
					{
						Assembly assembly = null;
						try
						{
							assembly = Assembly.Load(assemblyName);
						}
						catch (FileNotFoundException)
						{
							if (assemblyName.CodeBase != null)
							{
								throw;
							}
							AssemblyName assemblyName2 = AssemblyName.GetAssemblyName(Path.Combine(Path.GetDirectoryName(assemPath), assemblyName.Name + ".dll"));
							assembly = Assembly.Load(assemblyName2);
						}
						text = assembly.EscapedCodeBase;
						if (text != null)
						{
							text = this.GetLocalPath(text);
						}
					}
					this.AddReferencedAssembly(text);
				}
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0003D888 File Offset: 0x0003C888
		private void AddReferencedAssembly(string assem)
		{
			if (this.refAssems == null)
			{
				this.refAssems = new ArrayList();
			}
			this.refAssems.Add(assem);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0003D8AA File Offset: 0x0003C8AA
		private void AddGeneratedAssembly(string assem)
		{
			if (this.genAssems == null)
			{
				this.genAssems = new ArrayList();
			}
			this.genAssems.Add(assem);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0003D8CC File Offset: 0x0003C8CC
		internal void AddRCW(ITypeLib typeLib, Assembly assem)
		{
			if (this.rcwCache == null)
			{
				this.rcwCache = new Hashtable();
			}
			IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
			typeLib.GetLibAttr(out invalidIntPtr);
			try
			{
				if (invalidIntPtr != NativeMethods.InvalidIntPtr)
				{
					global::System.Runtime.InteropServices.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.TYPELIBATTR));
					this.rcwCache.Add(typelibattr.guid, assem);
				}
			}
			finally
			{
				typeLib.ReleaseTLibAttr(invalidIntPtr);
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0003D950 File Offset: 0x0003C950
		internal Assembly FindRCW(ITypeLib typeLib)
		{
			if (this.rcwCache == null)
			{
				return null;
			}
			IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
			typeLib.GetLibAttr(out invalidIntPtr);
			try
			{
				if (invalidIntPtr != NativeMethods.InvalidIntPtr)
				{
					global::System.Runtime.InteropServices.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.TYPELIBATTR));
					return (Assembly)this.rcwCache[typelibattr.guid];
				}
			}
			finally
			{
				typeLib.ReleaseTLibAttr(invalidIntPtr);
			}
			return null;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0003D9D4 File Offset: 0x0003C9D4
		private void AddTypeLibAttr(ITypeLib typeLib)
		{
			if (this.tlbAttrs == null)
			{
				this.tlbAttrs = new ArrayList();
			}
			IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
			typeLib.GetLibAttr(out invalidIntPtr);
			if (invalidIntPtr != NativeMethods.InvalidIntPtr)
			{
				global::System.Runtime.InteropServices.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.TYPELIBATTR));
				this.tlbAttrs.Add(typelibattr);
				typeLib.ReleaseTLibAttr(invalidIntPtr);
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0003DA3E File Offset: 0x0003CA3E
		private string GetAxReference(ITypeLib typeLib)
		{
			if (this.options.references == null)
			{
				return null;
			}
			return this.options.references.ResolveActiveXReference((UCOMITypeLib)typeLib);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0003DA68 File Offset: 0x0003CA68
		private string GetReferencedAssembly(string assemName)
		{
			if (this.refAssems == null || this.refAssems.Count <= 0)
			{
				return null;
			}
			foreach (object obj in this.refAssems)
			{
				string text = (string)obj;
				if (string.Equals(text, assemName, StringComparison.OrdinalIgnoreCase))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0003DAE4 File Offset: 0x0003CAE4
		private string GetComReference(ITypeLib typeLib)
		{
			if (this.options.references == null)
			{
				return null;
			}
			return this.options.references.ResolveComReference((UCOMITypeLib)typeLib);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0003DB0B File Offset: 0x0003CB0B
		private string GetComReference(AssemblyName name)
		{
			if (this.options.references == null)
			{
				return name.EscapedCodeBase;
			}
			return this.options.references.ResolveComReference(name);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0003DB32 File Offset: 0x0003CB32
		private string GetManagedReference(string assemName)
		{
			if (this.options.references == null)
			{
				return assemName + ".dll";
			}
			return this.options.references.ResolveManagedReference(assemName);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0003DB60 File Offset: 0x0003CB60
		private string GetAxTypeFromAssembly(string fileName, Guid clsid)
		{
			/*
An exception occurred when decompiling this method (06000EDC)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Windows.Forms.Design.AxImporter::GetAxTypeFromAssembly(System.String,System.Guid)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.NRefactory.AbstractAnnotatable.AddAnnotation(Object annotation) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\NRefactory\ICSharpCode.NRefactory\IAnnotatable.cs:line 152
   at ICSharpCode.NRefactory.CSharp.AstNode.AddAnnotation(Object annotation) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\NRefactory\ICSharpCode.NRefactory.CSharp\Ast\AstNode.cs:line 1024
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 418
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 268
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 282
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0003DC00 File Offset: 0x0003CC00
		private Assembly GetCopiedAssembly(string fileName, bool loadPdb, bool isPIA)
		{
			if (!File.Exists(fileName))
			{
				return null;
			}
			string text = fileName.ToUpper(CultureInfo.InvariantCulture);
			if (this.copiedAssems == null)
			{
				this.copiedAssems = new Hashtable();
			}
			else if (this.copiedAssems.Contains(text))
			{
				return (Assembly)this.copiedAssems[text];
			}
			Assembly assembly;
			if (!isPIA)
			{
				Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				int num = (int)stream.Length;
				byte[] array = new byte[num];
				stream.Read(array, 0, num);
				stream.Close();
				byte[] array2 = null;
				if (loadPdb)
				{
					string text2 = Path.ChangeExtension(fileName, "pdb");
					if (File.Exists(text2))
					{
						stream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.Read);
						num = (int)stream.Length;
						array2 = new byte[num];
						stream.Read(array2, 0, num);
						stream.Close();
					}
				}
				if (array2 == null)
				{
					assembly = Assembly.Load(array);
				}
				else
				{
					assembly = Assembly.Load(array, array2);
				}
			}
			else
			{
				assembly = Assembly.LoadFrom(fileName);
			}
			this.copiedAssems.Add(text, assembly);
			return assembly;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0003DD04 File Offset: 0x0003CD04
		private static string GetFileOfTypeLib(ITypeLib typeLib)
		{
			IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
			typeLib.GetLibAttr(out invalidIntPtr);
			if (invalidIntPtr != NativeMethods.InvalidIntPtr)
			{
				global::System.Runtime.InteropServices.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.TYPELIBATTR));
				try
				{
					return AxImporter.GetFileOfTypeLib(ref typelibattr);
				}
				finally
				{
					typeLib.ReleaseTLibAttr(invalidIntPtr);
				}
			}
			return null;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0003DD68 File Offset: 0x0003CD68
		public static string GetFileOfTypeLib(ref global::System.Runtime.InteropServices.TYPELIBATTR tlibattr)
		{
			string text = NativeMethods.QueryPathOfRegTypeLib(ref tlibattr.guid, tlibattr.wMajorVerNum, tlibattr.wMinorVerNum, tlibattr.lcid);
			if (text.Length > 0)
			{
				int num = text.IndexOf('\0');
				if (num > -1)
				{
					text = text.Substring(0, num);
				}
				if (!File.Exists(text))
				{
					int num2 = text.LastIndexOf(Path.DirectorySeparatorChar);
					if (num2 != -1)
					{
						bool flag = true;
						for (int i = num2 + 1; i < text.Length; i++)
						{
							if (text[i] != '\0' && !char.IsDigit(text[i]))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							text = text.Substring(0, num2);
							if (!File.Exists(text))
							{
								text = null;
							}
						}
						else
						{
							text = null;
						}
					}
					else
					{
						text = null;
					}
				}
			}
			return text;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0003DE24 File Offset: 0x0003CE24
		private string GetLocalPath(string fileName)
		{
			Uri uri = new Uri(fileName);
			return uri.LocalPath + uri.Fragment;
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0003DE4C File Offset: 0x0003CE4C
		internal string GenerateFromActiveXClsid(Guid clsid)
		{
			string text = "CLSID\\{" + clsid.ToString() + "}";
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
			if (registryKey == null)
			{
				throw new ArgumentException(SR.GetString("AXNotRegistered", new object[] { text.ToString() }));
			}
			ITypeLib typeLib = null;
			Guid guid = Guid.Empty;
			RegistryKey registryKey2 = registryKey.OpenSubKey("TypeLib");
			if (registryKey2 != null)
			{
				RegistryKey registryKey3 = registryKey.OpenSubKey("Version");
				string text2 = (string)registryKey3.GetValue("");
				int num = text2.IndexOf('.');
				short num2;
				short num3;
				if (num == -1)
				{
					num2 = short.Parse(text2, CultureInfo.InvariantCulture);
					num3 = 0;
				}
				else
				{
					num2 = short.Parse(text2.Substring(0, num), CultureInfo.InvariantCulture);
					num3 = short.Parse(text2.Substring(num + 1, text2.Length - num - 1), CultureInfo.InvariantCulture);
				}
				registryKey3.Close();
				object value = registryKey2.GetValue("");
				guid = new Guid((string)value);
				registryKey2.Close();
				try
				{
					typeLib = NativeMethods.LoadRegTypeLib(ref guid, num2, num3, Application.CurrentCulture.LCID);
				}
				catch (Exception)
				{
				}
			}
			if (typeLib == null)
			{
				RegistryKey registryKey4 = registryKey.OpenSubKey("InprocServer32");
				if (registryKey4 != null)
				{
					string text3 = (string)registryKey4.GetValue("");
					registryKey4.Close();
					typeLib = NativeMethods.LoadTypeLib(text3);
				}
			}
			registryKey.Close();
			if (typeLib != null)
			{
				try
				{
					return this.GenerateFromTypeLibrary((UCOMITypeLib)typeLib, clsid);
				}
				finally
				{
					Marshal.ReleaseComObject(typeLib);
				}
			}
			throw new ArgumentException(SR.GetString("AXNotRegistered", new object[] { text.ToString() }));
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0003E02C File Offset: 0x0003D02C
		public string GenerateFromFile(FileInfo file)
		{
			this.typeLibName = file.FullName;
			ITypeLib typeLib = null;
			typeLib = NativeMethods.LoadTypeLib(this.typeLibName);
			if (typeLib == null)
			{
				throw new Exception(SR.GetString("AXCannotLoadTypeLib", new object[] { this.typeLibName }));
			}
			string text;
			try
			{
				text = this.GenerateFromTypeLibrary((UCOMITypeLib)typeLib);
			}
			finally
			{
				if (typeLib != null)
				{
					Marshal.ReleaseComObject(typeLib);
				}
			}
			return text;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0003E0A4 File Offset: 0x0003D0A4
		public string GenerateFromTypeLibrary(UCOMITypeLib typeLib)
		{
			bool flag = false;
			int typeInfoCount = ((ITypeLib)typeLib).GetTypeInfoCount();
			for (int i = 0; i < typeInfoCount; i++)
			{
				ITypeInfo typeInfo;
				((ITypeLib)typeLib).GetTypeInfo(i, out typeInfo);
				IntPtr zero;
				typeInfo.GetTypeAttr(out zero);
				global::System.Runtime.InteropServices.ComTypes.TYPEATTR typeattr = (global::System.Runtime.InteropServices.ComTypes.TYPEATTR)Marshal.PtrToStructure(zero, typeof(global::System.Runtime.InteropServices.ComTypes.TYPEATTR));
				if (typeattr.typekind == global::System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_COCLASS)
				{
					Guid guid = typeattr.guid;
					string text = "CLSID\\{" + guid.ToString() + "}\\Control";
					RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
					if (registryKey != null)
					{
						flag = true;
					}
				}
				typeInfo.ReleaseTypeAttr(zero);
				zero = IntPtr.Zero;
				Marshal.ReleaseComObject(typeInfo);
				typeInfo = null;
			}
			if (flag)
			{
				return this.GenerateFromTypeLibrary(typeLib, Guid.Empty);
			}
			string text2 = SR.GetString("AXNoActiveXControls", new object[] { (this.typeLibName != null) ? this.typeLibName : Marshal.GetTypeLibName((ITypeLib)typeLib) });
			if (this.options.msBuildErrors)
			{
				text2 = "AxImp: error aximp000: " + text2;
			}
			throw new Exception(text2);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0003E1C4 File Offset: 0x0003D1C4
		public string GenerateFromTypeLibrary(UCOMITypeLib typeLib, Guid clsid)
		{
			string text = null;
			string text2 = null;
			Assembly assembly = null;
			text = this.GetAxReference((ITypeLib)typeLib);
			if (text != null && clsid != Guid.Empty)
			{
				text2 = this.GetAxTypeFromAssembly(text, clsid);
			}
			if (text == null)
			{
				string text3 = Marshal.GetTypeLibName((ITypeLib)typeLib);
				string text4 = Path.Combine(this.options.outputDirectory, text3 + ".dll");
				this.AddReferencedAssembly(this.GetManagedReference("System.Windows.Forms"));
				this.AddReferencedAssembly(this.GetManagedReference("System.Drawing"));
				this.AddReferencedAssembly(this.GetManagedReference("System"));
				string text5 = this.GetComReference((ITypeLib)typeLib);
				if (text5 != null)
				{
					this.AddReferencedAssembly(text5);
					assembly = this.GetCopiedAssembly(text5, false, false);
					this.AddDependentAssemblies(assembly, text5);
				}
				else
				{
					TypeLibConverter typeLibConverter = new TypeLibConverter();
					assembly = this.GetPrimaryInteropAssembly((ITypeLib)typeLib, typeLibConverter);
					if (assembly != null)
					{
						text5 = this.GetLocalPath(assembly.EscapedCodeBase);
						this.AddDependentAssemblies(assembly, text5);
					}
					else
					{
						AssemblyBuilder assemblyBuilder = typeLibConverter.ConvertTypeLibToAssembly((ITypeLib)typeLib, text4, TypeLibImporterFlags.None, new AxImporter.ImporterCallback(this), this.options.publicKey, this.options.keyPair, null, null);
						if (text5 == null)
						{
							text5 = this.SaveAssemblyBuilder((ITypeLib)typeLib, assemblyBuilder, text4);
							assembly = assemblyBuilder;
						}
					}
				}
				int num = 0;
				string[] array = new string[this.refAssems.Count];
				foreach (object obj in this.refAssems)
				{
					string text6 = (string)obj;
					string text7 = text6;
					text7 = text7.Replace("%20", " ");
					array[num++] = text7;
				}
				if (text2 == null)
				{
					string fileOfTypeLib = AxImporter.GetFileOfTypeLib((ITypeLib)typeLib);
					DateTime dateTime = ((fileOfTypeLib == null) ? DateTime.Now : File.GetLastWriteTime(fileOfTypeLib));
					ResolveEventHandler resolveEventHandler = new ResolveEventHandler(this.OnAssemblyResolve);
					AppDomain.CurrentDomain.AssemblyResolve += resolveEventHandler;
					AppDomain.CurrentDomain.TypeResolve += this.OnTypeResolve;
					try
					{
						if (this.options.genSources)
						{
							AxWrapperGen.GeneratedSources = new ArrayList();
						}
						if (this.options.outputName == null)
						{
							this.options.outputName = "Ax" + text3 + ".dll";
						}
						text2 = AxWrapperGen.GenerateWrappers(this, clsid, assembly, array, dateTime, out text);
						if (this.options.genSources)
						{
							this.generatedSources = AxWrapperGen.GeneratedSources;
						}
					}
					finally
					{
						AppDomain.CurrentDomain.AssemblyResolve -= resolveEventHandler;
						AppDomain.CurrentDomain.TypeResolve -= this.OnTypeResolve;
					}
					if (text2 == null)
					{
						string text8 = SR.GetString("AXNoActiveXControls", new object[] { (this.typeLibName != null) ? this.typeLibName : text3 });
						if (this.options.msBuildErrors)
						{
							text8 = "AxImp: error aximp000: " + text8;
						}
						throw new Exception(text8);
					}
				}
				if (text2 != null)
				{
					this.AddReferencedAssembly(text);
					this.AddTypeLibAttr((ITypeLib)typeLib);
					this.AddGeneratedAssembly(text);
				}
			}
			return text2;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0003E4EC File Offset: 0x0003D4EC
		internal Assembly GetPrimaryInteropAssembly(ITypeLib typeLib, TypeLibConverter tlbConverter)
		{
			Assembly assembly = this.FindRCW(typeLib);
			if (assembly != null)
			{
				return assembly;
			}
			IntPtr invalidIntPtr = NativeMethods.InvalidIntPtr;
			typeLib.GetLibAttr(out invalidIntPtr);
			if (invalidIntPtr != NativeMethods.InvalidIntPtr)
			{
				global::System.Runtime.InteropServices.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.TYPELIBATTR)Marshal.PtrToStructure(invalidIntPtr, typeof(global::System.Runtime.InteropServices.TYPELIBATTR));
				string text = null;
				string text2 = null;
				try
				{
					tlbConverter.GetPrimaryInteropAssembly(typelibattr.guid, (int)typelibattr.wMajorVerNum, (int)typelibattr.wMinorVerNum, typelibattr.lcid, out text, out text2);
					if (text != null && text2 == null)
					{
						try
						{
							assembly = Assembly.Load(text);
							text2 = this.GetLocalPath(assembly.EscapedCodeBase);
							goto IL_00A4;
						}
						catch (Exception)
						{
							goto IL_00A4;
						}
					}
					if (text2 != null)
					{
						text2 = this.GetLocalPath(text2);
						assembly = Assembly.LoadFrom(text2);
					}
					IL_00A4:
					if (assembly != null)
					{
						this.AddRCW(typeLib, assembly);
						this.AddReferencedAssembly(text2);
					}
				}
				finally
				{
					typeLib.ReleaseTLibAttr(invalidIntPtr);
				}
			}
			return assembly;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0003E5D8 File Offset: 0x0003D5D8
		private Assembly OnAssemblyResolve(object sender, ResolveEventArgs e)
		{
			string name = e.Name;
			if (this.rcwCache != null)
			{
				foreach (object obj in this.rcwCache.Values)
				{
					Assembly assembly = (Assembly)obj;
					if (assembly.FullName == name)
					{
						return assembly;
					}
				}
			}
			if (this.copiedAssems == null)
			{
				this.copiedAssems = new Hashtable();
			}
			else
			{
				Assembly assembly2 = (Assembly)this.copiedAssems[name];
				if (assembly2 != null)
				{
					return assembly2;
				}
			}
			if (this.refAssems == null || this.refAssems.Count == 0)
			{
				return null;
			}
			foreach (object obj2 in this.refAssems)
			{
				string text = (string)obj2;
				Assembly copiedAssembly = this.GetCopiedAssembly(text, false, false);
				if (copiedAssembly != null)
				{
					string fullName = copiedAssembly.FullName;
					if (fullName == name)
					{
						return copiedAssembly;
					}
				}
			}
			return null;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0003E714 File Offset: 0x0003D714
		private Assembly OnTypeResolve(object sender, ResolveEventArgs e)
		{
			try
			{
				string name = e.Name;
				if (this.refAssems == null || this.refAssems.Count == 0)
				{
					return null;
				}
				foreach (object obj in this.refAssems)
				{
					string text = (string)obj;
					Assembly copiedAssembly = this.GetCopiedAssembly(text, false, false);
					if (copiedAssembly != null && copiedAssembly.GetType(name, false) != null)
					{
						return copiedAssembly;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0003E7BC File Offset: 0x0003D7BC
		private string SaveAssemblyBuilder(ITypeLib typeLib, AssemblyBuilder asmBldr, string rcwName)
		{
			FileInfo fileInfo = new FileInfo(rcwName);
			string fullName = fileInfo.FullName;
			string name = fileInfo.Name;
			if (fileInfo.Exists)
			{
				if (!this.options.overwriteRCW)
				{
					goto IL_00D9;
				}
				if (this.typeLibName != null && string.Equals(this.typeLibName, fileInfo.FullName, StringComparison.OrdinalIgnoreCase))
				{
					throw new Exception(SR.GetString("AXCannotOverwriteFile", new object[] { fileInfo.FullName }));
				}
				if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					throw new Exception(SR.GetString("AXReadOnlyFile", new object[] { fileInfo.FullName }));
				}
				try
				{
					fileInfo.Delete();
					asmBldr.Save(name);
					goto IL_00D9;
				}
				catch (Exception)
				{
					throw new Exception(SR.GetString("AXCannotOverwriteFile", new object[] { fileInfo.FullName }));
				}
			}
			asmBldr.Save(name);
			IL_00D9:
			string fullName2 = fileInfo.FullName;
			this.AddReferencedAssembly(fullName2);
			this.AddTypeLibAttr(typeLib);
			this.AddGeneratedAssembly(fullName2);
			return fullName2;
		}

		// Token: 0x04000F8C RID: 3980
		internal AxImporter.Options options;

		// Token: 0x04000F8D RID: 3981
		internal string typeLibName;

		// Token: 0x04000F8E RID: 3982
		private ArrayList refAssems;

		// Token: 0x04000F8F RID: 3983
		private ArrayList genAssems;

		// Token: 0x04000F90 RID: 3984
		private ArrayList tlbAttrs;

		// Token: 0x04000F91 RID: 3985
		private ArrayList generatedSources;

		// Token: 0x04000F92 RID: 3986
		private Hashtable copiedAssems;

		// Token: 0x04000F93 RID: 3987
		private Hashtable rcwCache;

		// Token: 0x02000190 RID: 400
		private class ImporterCallback : ITypeLibImporterNotifySink
		{
			// Token: 0x06000EE9 RID: 3817 RVA: 0x0003E8D0 File Offset: 0x0003D8D0
			public ImporterCallback(AxImporter importer)
			{
				this.importer = importer;
				this.options = importer.options;
			}

			// Token: 0x06000EEA RID: 3818 RVA: 0x0003E8EB File Offset: 0x0003D8EB
			void ITypeLibImporterNotifySink.ReportEvent(ImporterEventKind EventKind, int EventCode, string EventMsg)
			{
			}

			// Token: 0x06000EEB RID: 3819 RVA: 0x0003E8F0 File Offset: 0x0003D8F0
			Assembly ITypeLibImporterNotifySink.ResolveRef(object typeLib)
			{
				Assembly assembly2;
				try
				{
					string comReference = this.importer.GetComReference((ITypeLib)typeLib);
					if (comReference != null)
					{
						this.importer.AddReferencedAssembly(comReference);
					}
					Assembly assembly = this.importer.FindRCW((ITypeLib)typeLib);
					if (assembly != null)
					{
						assembly2 = assembly;
					}
					else
					{
						try
						{
							string typeLibName = Marshal.GetTypeLibName((ITypeLib)typeLib);
							string text = Path.Combine(this.options.outputDirectory, typeLibName + ".dll");
							if (this.importer.GetReferencedAssembly(text) != null)
							{
								assembly2 = this.importer.GetCopiedAssembly(text, false, false);
							}
							else
							{
								TypeLibConverter typeLibConverter = new TypeLibConverter();
								assembly = this.importer.GetPrimaryInteropAssembly((ITypeLib)typeLib, typeLibConverter);
								if (assembly != null)
								{
									assembly2 = assembly;
								}
								else
								{
									AssemblyBuilder assemblyBuilder = typeLibConverter.ConvertTypeLibToAssembly(typeLib, text, TypeLibImporterFlags.None, new AxImporter.ImporterCallback(this.importer), this.options.publicKey, this.options.keyPair, null, null);
									if (comReference == null)
									{
										this.importer.SaveAssemblyBuilder((ITypeLib)typeLib, assemblyBuilder, text);
										this.importer.AddRCW((ITypeLib)typeLib, assemblyBuilder);
										assembly2 = assemblyBuilder;
									}
									else
									{
										assembly2 = this.importer.GetCopiedAssembly(comReference, false, false);
									}
								}
							}
						}
						catch
						{
							assembly2 = null;
						}
					}
				}
				finally
				{
					Marshal.ReleaseComObject(typeLib);
				}
				return assembly2;
			}

			// Token: 0x04000F94 RID: 3988
			private AxImporter importer;

			// Token: 0x04000F95 RID: 3989
			private AxImporter.Options options;
		}

		// Token: 0x02000191 RID: 401
		public sealed class Options
		{
			// Token: 0x04000F96 RID: 3990
			public string outputName;

			// Token: 0x04000F97 RID: 3991
			public string outputDirectory;

			// Token: 0x04000F98 RID: 3992
			public byte[] publicKey;

			// Token: 0x04000F99 RID: 3993
			public StrongNameKeyPair keyPair;

			// Token: 0x04000F9A RID: 3994
			public string keyFile;

			// Token: 0x04000F9B RID: 3995
			public string keyContainer;

			// Token: 0x04000F9C RID: 3996
			public bool genSources;

			// Token: 0x04000F9D RID: 3997
			public bool msBuildErrors;

			// Token: 0x04000F9E RID: 3998
			public bool noLogo;

			// Token: 0x04000F9F RID: 3999
			public bool silentMode;

			// Token: 0x04000FA0 RID: 4000
			public bool verboseMode;

			// Token: 0x04000FA1 RID: 4001
			public bool delaySign;

			// Token: 0x04000FA2 RID: 4002
			public bool overwriteRCW;

			// Token: 0x04000FA3 RID: 4003
			public AxImporter.IReferenceResolver references;
		}

		// Token: 0x02000192 RID: 402
		public interface IReferenceResolver
		{
			// Token: 0x06000EED RID: 3821
			string ResolveManagedReference(string assemName);

			// Token: 0x06000EEE RID: 3822
			string ResolveComReference(UCOMITypeLib typeLib);

			// Token: 0x06000EEF RID: 3823
			string ResolveComReference(AssemblyName name);

			// Token: 0x06000EF0 RID: 3824
			string ResolveActiveXReference(UCOMITypeLib typeLib);
		}
	}
}
