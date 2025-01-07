using System;
using System.Text;

namespace System.Xml
{
	public class XmlDeclaration : XmlLinkedNode
	{
		protected internal XmlDeclaration(string version, string encoding, string standalone, XmlDocument doc)
			: base(doc)
		{
			if (version != "1.0")
			{
				throw new ArgumentException(Res.GetString("Xdom_Version"));
			}
			if (standalone != null && standalone.Length > 0 && standalone != "yes" && standalone != "no")
			{
				throw new ArgumentException(Res.GetString("Xdom_standalone", new object[] { standalone }));
			}
			this.Encoding = encoding;
			this.Standalone = standalone;
		}

		public string Version
		{
			get
			{
				return "1.0";
			}
		}

		public string Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = ((value == null) ? string.Empty : value);
			}
		}

		public string Standalone
		{
			get
			{
				return this.standalone;
			}
			set
			{
				if (value == null)
				{
					this.standalone = string.Empty;
					return;
				}
				if (value.Length == 0 || value == "yes" || value == "no")
				{
					this.standalone = value;
					return;
				}
				throw new ArgumentException(Res.GetString("Xdom_standalone", new object[] { value }));
			}
		}

		public override string Value
		{
			get
			{
				return this.InnerText;
			}
			set
			{
				this.InnerText = value;
			}
		}

		public override string InnerText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("version=\"" + this.Version + "\"");
				if (this.Encoding.Length > 0)
				{
					stringBuilder.Append(" encoding=\"");
					stringBuilder.Append(this.Encoding);
					stringBuilder.Append("\"");
				}
				if (this.Standalone.Length > 0)
				{
					stringBuilder.Append(" standalone=\"");
					stringBuilder.Append(this.Standalone);
					stringBuilder.Append("\"");
				}
				return stringBuilder.ToString();
			}
			set
			{
				string text = null;
				string text2 = null;
				string text3 = null;
				string text4 = this.Encoding;
				string text5 = this.Standalone;
				XmlLoader.ParseXmlDeclarationValue(value, out text, out text2, out text3);
				try
				{
					if (text != null && text != "1.0")
					{
						throw new ArgumentException(Res.GetString("Xdom_Version"));
					}
					if (text2 != null)
					{
						this.Encoding = text2;
					}
					if (text3 != null)
					{
						this.Standalone = text3;
					}
				}
				catch
				{
					this.Encoding = text4;
					this.Standalone = text5;
					throw;
				}
			}
		}

		public override string Name
		{
			get
			{
				return "xml";
			}
		}

		public override string LocalName
		{
			get
			{
				return this.Name;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateXmlDeclaration(this.Version, this.Encoding, this.Standalone);
		}

		public override void WriteTo(XmlWriter w)
		{
			w.WriteProcessingInstruction(this.Name, this.InnerText);
		}

		public override void WriteContentTo(XmlWriter w)
		{
		}

		private const string YES = "yes";

		private const string NO = "no";

		private const string VERNUM = "1.0";

		private string encoding;

		private string standalone;
	}
}
