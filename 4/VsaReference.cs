using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000141 RID: 321
	internal class VsaReference : VsaItem, IVsaReferenceItem, IVsaItem
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x000631E4 File Offset: 0x000621E4
		internal VsaReference(VsaEngine engine, string itemName)
			: base(engine, itemName, VsaItemType.Reference, VsaItemFlag.None)
		{
			this.assemblyName = itemName;
			this.assembly = null;
			this.loadFailed = false;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00063205 File Offset: 0x00062205
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x00063220 File Offset: 0x00062220
		public string AssemblyName
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.assemblyName;
			}
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				this.assemblyName = value;
				this.isDirty = true;
				this.engine.IsDirty = true;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0006324F File Offset: 0x0006224F
		internal Assembly Assembly
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.assembly;
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0006326C File Offset: 0x0006226C
		internal Type GetType(string typeName)
		{
			if (this.assembly == null)
			{
				if (!this.loadFailed)
				{
					try
					{
						this.Load();
					}
					catch
					{
						this.loadFailed = true;
					}
				}
				if (this.assembly == null)
				{
					return null;
				}
			}
			Type type = this.assembly.GetType(typeName, false);
			if (type != null && (!type.IsPublic || CustomAttribute.IsDefined(type, typeof(RequiredAttributeAttribute), true)))
			{
				type = null;
			}
			return type;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000632E4 File Offset: 0x000622E4
		internal override void Compile()
		{
			this.Compile(true);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000632F0 File Offset: 0x000622F0
		internal bool Compile(bool throwOnFileNotFound)
		{
			try
			{
				string fileName = Path.GetFileName(this.assemblyName);
				string text = fileName + ".dll";
				if (string.Compare(fileName, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = typeof(object).Assembly;
				}
				if (string.Compare(fileName, "microsoft.jscript.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "microsoft.jscript.dll", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = this.engine.JScriptModule.Assembly;
				}
				else if (string.Compare(fileName, "microsoft.vsa.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "microsoft.vsa.dll", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = this.engine.VsaModule.Assembly;
				}
				else if (this.engine.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom && (string.Compare(fileName, "system.dll", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "system.dll", StringComparison.OrdinalIgnoreCase) == 0))
				{
					this.assembly = typeof(Regex).Module.Assembly;
				}
				if (this.assembly == null)
				{
					string text2 = this.engine.FindAssembly(this.assemblyName);
					if (text2 == null)
					{
						text = this.assemblyName + ".dll";
						bool flag = false;
						foreach (object obj in this.engine.Items)
						{
							if (obj is VsaReference && string.Compare(((VsaReference)obj).AssemblyName, text, StringComparison.OrdinalIgnoreCase) == 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							text2 = this.engine.FindAssembly(text);
							if (text2 != null)
							{
								this.assemblyName = text;
							}
						}
					}
					if (text2 == null)
					{
						if (throwOnFileNotFound)
						{
							throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, new FileNotFoundException());
						}
						return false;
					}
					else
					{
						switch (this.engine.ReferenceLoaderAPI)
						{
						case LoaderAPI.LoadFrom:
							this.assembly = Assembly.LoadFrom(text2);
							break;
						case LoaderAPI.LoadFile:
							this.assembly = Assembly.LoadFile(text2);
							break;
						case LoaderAPI.ReflectionOnlyLoadFrom:
							this.assembly = Assembly.ReflectionOnlyLoadFrom(text2);
							break;
						}
						this.CheckCompatibility();
					}
				}
			}
			catch (VsaException)
			{
				throw;
			}
			catch (BadImageFormatException ex)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex);
			}
			catch (FileNotFoundException ex2)
			{
				if (throwOnFileNotFound)
				{
					throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex2);
				}
				return false;
			}
			catch (FileLoadException ex3)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex3);
			}
			catch (ArgumentException ex4)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex4);
			}
			catch (Exception ex5)
			{
				throw new VsaException(VsaError.InternalCompilerError, ex5.ToString(), ex5);
			}
			catch
			{
				throw new VsaException(VsaError.InternalCompilerError);
			}
			if (this.assembly != null)
			{
				return true;
			}
			if (throwOnFileNotFound)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName);
			}
			return false;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00063684 File Offset: 0x00062684
		private void Load()
		{
			try
			{
				if (string.Compare(this.assemblyName, "mscorlib", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = typeof(object).Module.Assembly;
				}
				else if (string.Compare(this.assemblyName, "Microsoft.JScript", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = typeof(VsaEngine).Module.Assembly;
				}
				else if (string.Compare(this.assemblyName, "Microsoft.Vsa", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = typeof(IVsaEngine).Module.Assembly;
				}
				else if (string.Compare(this.assemblyName, "System", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.assembly = typeof(Regex).Module.Assembly;
				}
				else
				{
					this.assembly = Assembly.Load(this.assemblyName);
				}
			}
			catch (BadImageFormatException ex)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex);
			}
			catch (FileNotFoundException ex2)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex2);
			}
			catch (ArgumentException ex3)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName, ex3);
			}
			catch (Exception ex4)
			{
				throw new VsaException(VsaError.InternalCompilerError, ex4.ToString(), ex4);
			}
			catch
			{
				throw new VsaException(VsaError.InternalCompilerError);
			}
			if (this.assembly == null)
			{
				throw new VsaException(VsaError.AssemblyExpected, this.assemblyName);
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00063814 File Offset: 0x00062814
		private void CheckCompatibility()
		{
			PortableExecutableKinds portableExecutableKinds;
			ImageFileMachine imageFileMachine;
			this.assembly.ManifestModule.GetPEKind(out portableExecutableKinds, out imageFileMachine);
			if (imageFileMachine == ImageFileMachine.I386 && PortableExecutableKinds.ILOnly == (portableExecutableKinds & (PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit)))
			{
				return;
			}
			PortableExecutableKinds pekindFlags = this.engine.PEKindFlags;
			ImageFileMachine pemachineArchitecture = this.engine.PEMachineArchitecture;
			if (pemachineArchitecture == ImageFileMachine.I386 && PortableExecutableKinds.ILOnly == (pekindFlags & (PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit)))
			{
				return;
			}
			if (imageFileMachine != pemachineArchitecture)
			{
				JScriptException ex = new JScriptException(JSError.IncompatibleAssemblyReference);
				ex.value = this.assemblyName;
				this.engine.OnCompilerError(ex);
			}
		}

		// Token: 0x040007DD RID: 2013
		private string assemblyName;

		// Token: 0x040007DE RID: 2014
		private Assembly assembly;

		// Token: 0x040007DF RID: 2015
		private bool loadFailed;
	}
}
