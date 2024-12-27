using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

namespace System.Resources
{
	// Token: 0x02000144 RID: 324
	internal class AssemblyNamesTypeResolutionService : ITypeResolutionService
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x0000C4EC File Offset: 0x0000B4EC
		internal AssemblyNamesTypeResolutionService(AssemblyName[] names)
		{
			this.names = names;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000C4FB File Offset: 0x0000B4FB
		public Assembly GetAssembly(AssemblyName name)
		{
			return this.GetAssembly(name, true);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000C508 File Offset: 0x0000B508
		public Assembly GetAssembly(AssemblyName name, bool throwOnError)
		{
			Assembly assembly = null;
			if (this.cachedAssemblies == null)
			{
				this.cachedAssemblies = new Hashtable();
			}
			if (this.cachedAssemblies.Contains(name))
			{
				assembly = this.cachedAssemblies[name] as Assembly;
			}
			if (assembly == null)
			{
				assembly = Assembly.LoadWithPartialName(name.FullName);
				if (assembly != null)
				{
					this.cachedAssemblies[name] = assembly;
				}
				else if (this.names != null)
				{
					for (int i = 0; i < this.names.Length; i++)
					{
						if (name.Equals(this.names[i]))
						{
							try
							{
								assembly = Assembly.LoadFrom(this.GetPathOfAssembly(name));
								if (assembly != null)
								{
									this.cachedAssemblies[name] = assembly;
								}
							}
							catch
							{
								if (throwOnError)
								{
									throw;
								}
							}
						}
					}
				}
			}
			return assembly;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000C5D0 File Offset: 0x0000B5D0
		public string GetPathOfAssembly(AssemblyName name)
		{
			return name.CodeBase;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000C5D8 File Offset: 0x0000B5D8
		public Type GetType(string name)
		{
			return this.GetType(name, true);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000C5E2 File Offset: 0x0000B5E2
		public Type GetType(string name, bool throwOnError)
		{
			return this.GetType(name, throwOnError, false);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000C5F0 File Offset: 0x0000B5F0
		public Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			Type type = null;
			if (name.IndexOf(',') != -1)
			{
				type = Type.GetType(name, false, ignoreCase);
			}
			if (type == null && this.names != null)
			{
				for (int i = 0; i < this.names.Length; i++)
				{
					Assembly assembly = this.GetAssembly(this.names[i], throwOnError);
					type = assembly.GetType(name, false, ignoreCase);
					if (type == null)
					{
						int num = name.IndexOf(",");
						if (num != -1)
						{
							string text = name.Substring(0, num);
							type = assembly.GetType(text, false, ignoreCase);
						}
					}
					if (type != null)
					{
						break;
					}
				}
			}
			if (type == null && throwOnError)
			{
				throw new ArgumentException(SR.GetString("InvalidResXNoType", new object[] { name }));
			}
			return type;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000C69C File Offset: 0x0000B69C
		public void ReferenceAssembly(AssemblyName name)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000EF3 RID: 3827
		private AssemblyName[] names;

		// Token: 0x04000EF4 RID: 3828
		private Hashtable cachedAssemblies;
	}
}
