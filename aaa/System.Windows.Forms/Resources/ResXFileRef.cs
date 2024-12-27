using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Resources
{
	// Token: 0x02000145 RID: 325
	[TypeConverter(typeof(ResXFileRef.Converter))]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[Serializable]
	public class ResXFileRef
	{
		// Token: 0x060004ED RID: 1261 RVA: 0x0000C6A3 File Offset: 0x0000B6A3
		public ResXFileRef(string fileName, string typeName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this.fileName = fileName;
			this.typeName = typeName;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000C6D5 File Offset: 0x0000B6D5
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.textFileEncoding = null;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000C6DE File Offset: 0x0000B6DE
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000C6E0 File Offset: 0x0000B6E0
		public ResXFileRef(string fileName, string typeName, Encoding textFileEncoding)
			: this(fileName, typeName)
		{
			this.textFileEncoding = textFileEncoding;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000C6F1 File Offset: 0x0000B6F1
		internal ResXFileRef Clone()
		{
			return new ResXFileRef(this.fileName, this.typeName, this.textFileEncoding);
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0000C70A File Offset: 0x0000B70A
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000C712 File Offset: 0x0000B712
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000C71A File Offset: 0x0000B71A
		public Encoding TextFileEncoding
		{
			get
			{
				return this.textFileEncoding;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000C724 File Offset: 0x0000B724
		private static string PathDifference(string path1, string path2, bool compareCase)
		{
			int num = -1;
			int i = 0;
			while (i < path1.Length && i < path2.Length && (path1[i] == path2[i] || (!compareCase && char.ToLower(path1[i], CultureInfo.InvariantCulture) == char.ToLower(path2[i], CultureInfo.InvariantCulture))))
			{
				if (path1[i] == Path.DirectorySeparatorChar)
				{
					num = i;
				}
				i++;
			}
			if (i == 0)
			{
				return path2;
			}
			if (i == path1.Length && i == path2.Length)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (i < path1.Length)
			{
				if (path1[i] == Path.DirectorySeparatorChar)
				{
					stringBuilder.Append(".." + Path.DirectorySeparatorChar);
				}
				i++;
			}
			return stringBuilder.ToString() + path2.Substring(num + 1);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000C803 File Offset: 0x0000B803
		internal void MakeFilePathRelative(string basePath)
		{
			if (basePath == null || basePath.Length == 0)
			{
				return;
			}
			this.fileName = ResXFileRef.PathDifference(basePath, this.fileName, false);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000C824 File Offset: 0x0000B824
		public override string ToString()
		{
			string text = "";
			if (this.fileName.IndexOf(";") != -1 || this.fileName.IndexOf("\"") != -1)
			{
				text = text + "\"" + this.fileName + "\";";
			}
			else
			{
				text = text + this.fileName + ";";
			}
			text += this.typeName;
			if (this.textFileEncoding != null)
			{
				text = text + ";" + this.textFileEncoding.WebName;
			}
			return text;
		}

		// Token: 0x04000EF5 RID: 3829
		private string fileName;

		// Token: 0x04000EF6 RID: 3830
		private string typeName;

		// Token: 0x04000EF7 RID: 3831
		[OptionalField(VersionAdded = 2)]
		private Encoding textFileEncoding;

		// Token: 0x02000146 RID: 326
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		public class Converter : TypeConverter
		{
			// Token: 0x060004F8 RID: 1272 RVA: 0x0000C8B5 File Offset: 0x0000B8B5
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}

			// Token: 0x060004F9 RID: 1273 RVA: 0x0000C8C7 File Offset: 0x0000B8C7
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}

			// Token: 0x060004FA RID: 1274 RVA: 0x0000C8DC File Offset: 0x0000B8DC
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				object obj = null;
				if (destinationType == typeof(string))
				{
					obj = ((ResXFileRef)value).ToString();
				}
				return obj;
			}

			// Token: 0x060004FB RID: 1275 RVA: 0x0000C908 File Offset: 0x0000B908
			internal static string[] ParseResxFileRefString(string stringValue)
			{
				string[] array = null;
				if (stringValue != null)
				{
					stringValue = stringValue.Trim();
					string text;
					string text2;
					if (stringValue.StartsWith("\""))
					{
						int num = stringValue.LastIndexOf("\"");
						if (num - 1 < 0)
						{
							throw new ArgumentException("value");
						}
						text = stringValue.Substring(1, num - 1);
						if (num + 2 > stringValue.Length)
						{
							throw new ArgumentException("value");
						}
						text2 = stringValue.Substring(num + 2);
					}
					else
					{
						int num2 = stringValue.IndexOf(";");
						if (num2 == -1)
						{
							throw new ArgumentException("value");
						}
						text = stringValue.Substring(0, num2);
						if (num2 + 1 > stringValue.Length)
						{
							throw new ArgumentException("value");
						}
						text2 = stringValue.Substring(num2 + 1);
					}
					string[] array2 = text2.Split(new char[] { ';' });
					if (array2.Length > 1)
					{
						array = new string[]
						{
							text,
							array2[0],
							array2[1]
						};
					}
					else if (array2.Length > 0)
					{
						array = new string[]
						{
							text,
							array2[0]
						};
					}
					else
					{
						array = new string[] { text };
					}
				}
				return array;
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x0000CA38 File Offset: 0x0000BA38
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				object obj = null;
				string text = value as string;
				if (text != null)
				{
					string[] array = ResXFileRef.Converter.ParseResxFileRefString(text);
					string text2 = array[0];
					Type type = Type.GetType(array[1], true);
					if (type.Equals(typeof(string)))
					{
						Encoding encoding = Encoding.Default;
						if (array.Length > 2)
						{
							encoding = Encoding.GetEncoding(array[2]);
						}
						using (StreamReader streamReader = new StreamReader(text2, encoding))
						{
							return streamReader.ReadToEnd();
						}
					}
					byte[] array2 = null;
					using (FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						array2 = new byte[fileStream.Length];
						fileStream.Read(array2, 0, (int)fileStream.Length);
					}
					if (type.Equals(typeof(byte[])))
					{
						obj = array2;
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream(array2);
						if (type.Equals(typeof(MemoryStream)))
						{
							return memoryStream;
						}
						if (type.Equals(typeof(Bitmap)) && text2.EndsWith(".ico"))
						{
							Icon icon = new Icon(memoryStream);
							obj = icon.ToBitmap();
						}
						else
						{
							obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] { memoryStream }, null);
						}
					}
				}
				return obj;
			}
		}
	}
}
