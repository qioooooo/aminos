using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using Microsoft.CSharp;

namespace System.Xml.Serialization
{
	// Token: 0x020002B7 RID: 695
	internal class Compiler
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x0009D924 File Offset: 0x0009C924
		protected string[] Imports
		{
			get
			{
				string[] array = new string[this.imports.Values.Count];
				this.imports.Values.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0009D95C File Offset: 0x0009C95C
		internal void AddImport(Type type, Hashtable types)
		{
			if (type == null)
			{
				return;
			}
			if (TypeScope.IsKnownType(type))
			{
				return;
			}
			if (types[type] != null)
			{
				return;
			}
			types[type] = type;
			Type baseType = type.BaseType;
			if (baseType != null)
			{
				this.AddImport(baseType, types);
			}
			Type declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				this.AddImport(declaringType, types);
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				this.AddImport(type2, types);
			}
			ConstructorInfo[] constructors = type.GetConstructors();
			for (int j = 0; j < constructors.Length; j++)
			{
				ParameterInfo[] parameters = constructors[j].GetParameters();
				for (int k = 0; k < parameters.Length; k++)
				{
					this.AddImport(parameters[k].ParameterType, types);
				}
			}
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int l = 0; l < genericArguments.Length; l++)
				{
					this.AddImport(genericArguments[l], types);
				}
			}
			TempAssembly.FileIOPermission.Assert();
			Module module = type.Module;
			Assembly assembly = module.Assembly;
			if (DynamicAssemblies.IsTypeDynamic(type))
			{
				DynamicAssemblies.Add(assembly);
				return;
			}
			this.imports[assembly] = assembly.Location;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x0009DA8B File Offset: 0x0009CA8B
		internal void AddImport(Assembly assembly)
		{
			TempAssembly.FileIOPermission.Assert();
			this.imports[assembly] = assembly.Location;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x0009DAA9 File Offset: 0x0009CAA9
		internal TextWriter Source
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0009DAB1 File Offset: 0x0009CAB1
		internal void Close()
		{
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0009DAB4 File Offset: 0x0009CAB4
		internal static string GetTempAssemblyPath(string baseDir, Assembly assembly, string defaultNamespace)
		{
			if (assembly.Location == null || assembly.Location.Length == 0)
			{
				throw new InvalidOperationException(Res.GetString("XmlPregenAssemblyDynamic", new object[] { assembly.Location }));
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
			permissionSet.Assert();
			try
			{
				if (baseDir != null && baseDir.Length > 0)
				{
					if (!Directory.Exists(baseDir))
					{
						throw new UnauthorizedAccessException(Res.GetString("XmlPregenMissingDirectory", new object[] { baseDir }));
					}
				}
				else
				{
					baseDir = Path.GetTempPath();
					if (!Directory.Exists(baseDir))
					{
						throw new UnauthorizedAccessException(Res.GetString("XmlPregenMissingTempDirectory"));
					}
				}
				if (baseDir.EndsWith("\\", StringComparison.Ordinal))
				{
					baseDir += Compiler.GetTempAssemblyName(assembly.GetName(), defaultNamespace);
				}
				else
				{
					baseDir = baseDir + "\\" + Compiler.GetTempAssemblyName(assembly.GetName(), defaultNamespace);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return baseDir + ".dll";
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0009DBD0 File Offset: 0x0009CBD0
		internal static string GetTempAssemblyName(AssemblyName parent, string ns)
		{
			return parent.Name + ".XmlSerializers" + ((ns == null || ns.Length == 0) ? "" : ("." + ns.GetHashCode()));
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0009DC0C File Offset: 0x0009CC0C
		internal Assembly Compile(Assembly parent, string ns, XmlSerializerCompilerParameters xmlParameters, Evidence evidence)
		{
			CodeDomProvider codeDomProvider = new CSharpCodeProvider();
			CompilerParameters codeDomParameters = xmlParameters.CodeDomParameters;
			codeDomParameters.ReferencedAssemblies.AddRange(this.Imports);
			if (this.debugEnabled)
			{
				codeDomParameters.GenerateInMemory = false;
				codeDomParameters.IncludeDebugInformation = true;
				codeDomParameters.TempFiles.KeepFiles = true;
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			if (xmlParameters.IsNeedTempDirAccess)
			{
				permissionSet.AddPermission(TempAssembly.FileIOPermission);
			}
			permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.ControlEvidence));
			permissionSet.Assert();
			if (parent != null && (codeDomParameters.OutputAssembly == null || codeDomParameters.OutputAssembly.Length == 0))
			{
				string text = Compiler.AssemblyNameFromOptions(codeDomParameters.CompilerOptions);
				if (text == null)
				{
					text = Compiler.GetTempAssemblyPath(codeDomParameters.TempFiles.TempDir, parent, ns);
				}
				codeDomParameters.OutputAssembly = text;
			}
			if (codeDomParameters.CompilerOptions == null || codeDomParameters.CompilerOptions.Length == 0)
			{
				codeDomParameters.CompilerOptions = "/nostdlib";
			}
			else
			{
				CompilerParameters compilerParameters = codeDomParameters;
				compilerParameters.CompilerOptions += " /nostdlib";
			}
			CompilerParameters compilerParameters2 = codeDomParameters;
			compilerParameters2.CompilerOptions += " /D:_DYNAMIC_XMLSERIALIZER_COMPILATION";
			codeDomParameters.Evidence = evidence;
			CompilerResults compilerResults = null;
			Assembly assembly = null;
			try
			{
				compilerResults = codeDomProvider.CompileAssemblyFromSource(codeDomParameters, new string[] { this.writer.ToString() });
				if (compilerResults.Errors.Count > 0)
				{
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					stringWriter.WriteLine(Res.GetString("XmlCompilerError", new object[] { compilerResults.NativeCompilerReturnValue.ToString(CultureInfo.InvariantCulture) }));
					bool flag = false;
					foreach (object obj in compilerResults.Errors)
					{
						CompilerError compilerError = (CompilerError)obj;
						compilerError.FileName = "";
						if (!compilerError.IsWarning || compilerError.ErrorNumber == "CS1595")
						{
							flag = true;
							stringWriter.WriteLine(compilerError.ToString());
						}
					}
					if (flag)
					{
						throw new InvalidOperationException(stringWriter.ToString());
					}
				}
				assembly = compilerResults.CompiledAssembly;
			}
			catch (UnauthorizedAccessException)
			{
				string currentUser = Compiler.GetCurrentUser();
				if (currentUser == null || currentUser.Length == 0)
				{
					throw new UnauthorizedAccessException(Res.GetString("XmlSerializerAccessDenied"));
				}
				throw new UnauthorizedAccessException(Res.GetString("XmlIdentityAccessDenied", new object[] { currentUser }));
			}
			catch (FileLoadException ex)
			{
				throw new InvalidOperationException(Res.GetString("XmlSerializerCompileFailed"), ex);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (assembly == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlInternalError"));
			}
			return assembly;
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0009DF24 File Offset: 0x0009CF24
		private static string AssemblyNameFromOptions(string options)
		{
			if (options == null || options.Length == 0)
			{
				return null;
			}
			string text = null;
			string[] array = options.ToLower(CultureInfo.InvariantCulture).Split(null);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (text2.StartsWith("/out:", StringComparison.Ordinal))
				{
					text = text2.Substring(5);
				}
			}
			return text;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0009DF80 File Offset: 0x0009CF80
		internal static string GetCurrentUser()
		{
			try
			{
				WindowsIdentity current = WindowsIdentity.GetCurrent();
				if (current != null && current.Name != null)
				{
					return current.Name;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
			catch
			{
			}
			return "";
		}

		// Token: 0x04001447 RID: 5191
		private bool debugEnabled = DiagnosticsSwitches.KeepTempFiles.Enabled;

		// Token: 0x04001448 RID: 5192
		private Hashtable imports = new Hashtable();

		// Token: 0x04001449 RID: 5193
		private StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
	}
}
