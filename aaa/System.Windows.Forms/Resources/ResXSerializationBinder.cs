using System;
using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x02000143 RID: 323
	internal class ResXSerializationBinder : SerializationBinder
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x0000C415 File Offset: 0x0000B415
		internal ResXSerializationBinder(ITypeResolutionService typeResolver)
		{
			this.typeResolver = typeResolver;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000C424 File Offset: 0x0000B424
		public override Type BindToType(string assemblyName, string typeName)
		{
			if (this.typeResolver == null)
			{
				return null;
			}
			typeName = typeName + ", " + assemblyName;
			Type type = this.typeResolver.GetType(typeName);
			if (type == null)
			{
				string[] array = typeName.Split(new char[] { ',' });
				if (array != null && array.Length > 2)
				{
					string text = array[0].Trim();
					for (int i = 1; i < array.Length; i++)
					{
						string text2 = array[i].Trim();
						if (!text2.StartsWith("Version=") && !text2.StartsWith("version="))
						{
							text = text + ", " + text2;
						}
					}
					type = this.typeResolver.GetType(text);
					if (type == null)
					{
						type = this.typeResolver.GetType(array[0].Trim());
					}
				}
			}
			return type;
		}

		// Token: 0x04000EF2 RID: 3826
		private ITypeResolutionService typeResolver;
	}
}
