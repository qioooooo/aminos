using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	[Editor("System.Windows.Forms.Design.DesignBindingEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	internal class DesignBinding
	{
		public DesignBinding(object dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
		}

		public bool IsNull
		{
			get
			{
				return this.dataSource == null;
			}
		}

		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
		}

		public string DataField
		{
			get
			{
				if (string.IsNullOrEmpty(this.dataMember))
				{
					return string.Empty;
				}
				int num = this.dataMember.LastIndexOf(".");
				if (num == -1)
				{
					return this.dataMember;
				}
				return this.dataMember.Substring(num + 1);
			}
		}

		public bool Equals(object dataSource, string dataMember)
		{
			return dataSource == this.dataSource && string.Equals(dataMember, this.dataMember, StringComparison.OrdinalIgnoreCase);
		}

		private object dataSource;

		private string dataMember;

		public static DesignBinding Null = new DesignBinding(null, null);
	}
}
