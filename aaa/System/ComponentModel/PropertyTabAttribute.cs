using System;
using System.Reflection;

namespace System.ComponentModel
{
	// Token: 0x02000195 RID: 405
	[AttributeUsage(AttributeTargets.All)]
	public class PropertyTabAttribute : Attribute
	{
		// Token: 0x06000CBB RID: 3259 RVA: 0x000295B7 File Offset: 0x000285B7
		public PropertyTabAttribute()
		{
			this.tabScopes = new PropertyTabScope[0];
			this.tabClassNames = new string[0];
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x000295D7 File Offset: 0x000285D7
		public PropertyTabAttribute(Type tabClass)
			: this(tabClass, PropertyTabScope.Component)
		{
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000295E1 File Offset: 0x000285E1
		public PropertyTabAttribute(string tabClassName)
			: this(tabClassName, PropertyTabScope.Component)
		{
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x000295EC File Offset: 0x000285EC
		public PropertyTabAttribute(Type tabClass, PropertyTabScope tabScope)
		{
			this.tabClasses = new Type[] { tabClass };
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"), "tabScope");
			}
			this.tabScopes = new PropertyTabScope[] { tabScope };
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002963C File Offset: 0x0002863C
		public PropertyTabAttribute(string tabClassName, PropertyTabScope tabScope)
		{
			this.tabClassNames = new string[] { tabClassName };
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"), "tabScope");
			}
			this.tabScopes = new PropertyTabScope[] { tabScope };
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0002968C File Offset: 0x0002868C
		public Type[] TabClasses
		{
			get
			{
				if (this.tabClasses == null && this.tabClassNames != null)
				{
					this.tabClasses = new Type[this.tabClassNames.Length];
					for (int i = 0; i < this.tabClassNames.Length; i++)
					{
						int num = this.tabClassNames[i].IndexOf(',');
						string text = null;
						string text2;
						if (num != -1)
						{
							text2 = this.tabClassNames[i].Substring(0, num).Trim();
							text = this.tabClassNames[i].Substring(num + 1).Trim();
						}
						else
						{
							text2 = this.tabClassNames[i];
						}
						this.tabClasses[i] = Type.GetType(text2, false);
						if (this.tabClasses[i] == null)
						{
							if (text == null)
							{
								throw new TypeLoadException(SR.GetString("PropertyTabAttributeTypeLoadException", new object[] { text2 }));
							}
							Assembly assembly = Assembly.Load(text);
							if (assembly != null)
							{
								this.tabClasses[i] = assembly.GetType(text2, true);
							}
						}
					}
				}
				return this.tabClasses;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00029789 File Offset: 0x00028789
		protected string[] TabClassNames
		{
			get
			{
				if (this.tabClassNames != null)
				{
					return (string[])this.tabClassNames.Clone();
				}
				return null;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000297A5 File Offset: 0x000287A5
		public PropertyTabScope[] TabScopes
		{
			get
			{
				return this.tabScopes;
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000297AD File Offset: 0x000287AD
		public override bool Equals(object other)
		{
			return other is PropertyTabAttribute && this.Equals((PropertyTabAttribute)other);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000297C8 File Offset: 0x000287C8
		public bool Equals(PropertyTabAttribute other)
		{
			if (other == this)
			{
				return true;
			}
			if (other.TabClasses.Length != this.TabClasses.Length || other.TabScopes.Length != this.TabScopes.Length)
			{
				return false;
			}
			for (int i = 0; i < this.TabClasses.Length; i++)
			{
				if (this.TabClasses[i] != other.TabClasses[i] || this.TabScopes[i] != other.TabScopes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002983B File Offset: 0x0002883B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00029843 File Offset: 0x00028843
		protected void InitializeArrays(string[] tabClassNames, PropertyTabScope[] tabScopes)
		{
			this.InitializeArrays(tabClassNames, null, tabScopes);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0002984E File Offset: 0x0002884E
		protected void InitializeArrays(Type[] tabClasses, PropertyTabScope[] tabScopes)
		{
			this.InitializeArrays(null, tabClasses, tabScopes);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002985C File Offset: 0x0002885C
		private void InitializeArrays(string[] tabClassNames, Type[] tabClasses, PropertyTabScope[] tabScopes)
		{
			if (tabClasses != null)
			{
				if (tabScopes != null && tabClasses.Length != tabScopes.Length)
				{
					throw new ArgumentException(SR.GetString("PropertyTabAttributeArrayLengthMismatch"));
				}
				this.tabClasses = (Type[])tabClasses.Clone();
			}
			else if (tabClassNames != null)
			{
				if (tabScopes != null && tabClasses.Length != tabScopes.Length)
				{
					throw new ArgumentException(SR.GetString("PropertyTabAttributeArrayLengthMismatch"));
				}
				this.tabClassNames = (string[])tabClassNames.Clone();
				this.tabClasses = null;
			}
			else if (this.tabClasses == null && this.tabClassNames == null)
			{
				throw new ArgumentException(SR.GetString("PropertyTabAttributeParamsBothNull"));
			}
			if (tabScopes != null)
			{
				for (int i = 0; i < tabScopes.Length; i++)
				{
					if (tabScopes[i] < PropertyTabScope.Document)
					{
						throw new ArgumentException(SR.GetString("PropertyTabAttributeBadPropertyTabScope"));
					}
				}
				this.tabScopes = (PropertyTabScope[])tabScopes.Clone();
				return;
			}
			this.tabScopes = new PropertyTabScope[tabClasses.Length];
			for (int j = 0; j < this.TabScopes.Length; j++)
			{
				this.tabScopes[j] = PropertyTabScope.Component;
			}
		}

		// Token: 0x04000AE7 RID: 2791
		private PropertyTabScope[] tabScopes;

		// Token: 0x04000AE8 RID: 2792
		private Type[] tabClasses;

		// Token: 0x04000AE9 RID: 2793
		private string[] tabClassNames;
	}
}
