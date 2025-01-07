using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public sealed class MyGroupCollectionAttribute : Attribute
	{
		public MyGroupCollectionAttribute(string typeToCollect, string createInstanceMethodName, string disposeInstanceMethodName, string defaultInstanceAlias)
		{
			this.m_NameOfBaseTypeToCollect = typeToCollect;
			this.m_NameOfCreateMethod = createInstanceMethodName;
			this.m_NameOfDisposeMethod = disposeInstanceMethodName;
			this.m_DefaultInstanceAlias = defaultInstanceAlias;
		}

		public string MyGroupName
		{
			get
			{
				return this.m_NameOfBaseTypeToCollect;
			}
		}

		public string CreateMethod
		{
			get
			{
				return this.m_NameOfCreateMethod;
			}
		}

		public string DisposeMethod
		{
			get
			{
				return this.m_NameOfDisposeMethod;
			}
		}

		public string DefaultInstanceAlias
		{
			get
			{
				return this.m_DefaultInstanceAlias;
			}
		}

		private string m_NameOfBaseTypeToCollect;

		private string m_NameOfCreateMethod;

		private string m_NameOfDisposeMethod;

		private string m_DefaultInstanceAlias;
	}
}
