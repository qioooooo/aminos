using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources
{
	// Token: 0x02000141 RID: 321
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[Serializable]
	public sealed class ResXDataNode : ISerializable
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x0000B2F9 File Offset: 0x0000A2F9
		private ResXDataNode()
		{
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000B304 File Offset: 0x0000A304
		internal ResXDataNode DeepClone()
		{
			return new ResXDataNode
			{
				nodeInfo = ((this.nodeInfo != null) ? this.nodeInfo.Clone() : null),
				name = this.name,
				comment = this.comment,
				typeName = this.typeName,
				fileRefFullPath = this.fileRefFullPath,
				fileRefType = this.fileRefType,
				fileRefTextEncoding = this.fileRefTextEncoding,
				value = this.value,
				fileRef = ((this.fileRef != null) ? this.fileRef.Clone() : null)
			};
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000B3A4 File Offset: 0x0000A3A4
		public ResXDataNode(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			Type type = ((value == null) ? typeof(object) : value.GetType());
			if (value != null && !type.IsSerializable)
			{
				throw new InvalidOperationException(SR.GetString("NotSerializableType", new object[] { name, type.FullName }));
			}
			if (value != null)
			{
				this.typeName = type.AssemblyQualifiedName;
			}
			this.name = name;
			this.value = value;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000B43C File Offset: 0x0000A43C
		public ResXDataNode(string name, ResXFileRef fileRef)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (fileRef == null)
			{
				throw new ArgumentNullException("fileRef");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			this.name = name;
			this.fileRef = fileRef;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000B48C File Offset: 0x0000A48C
		internal ResXDataNode(DataNodeInfo nodeInfo, string basePath)
		{
			this.nodeInfo = nodeInfo;
			this.InitializeDataNode(basePath);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000B4A4 File Offset: 0x0000A4A4
		private void InitializeDataNode(string basePath)
		{
			Type type = null;
			if (!string.IsNullOrEmpty(this.nodeInfo.TypeName))
			{
				type = ResXDataNode.internalTypeResolver.GetType(this.nodeInfo.TypeName, false, true);
			}
			if (type != null && type.Equals(typeof(ResXFileRef)))
			{
				string[] array = ResXFileRef.Converter.ParseResxFileRefString(this.nodeInfo.ValueData);
				if (array != null && array.Length > 1)
				{
					if (!Path.IsPathRooted(array[0]) && basePath != null)
					{
						this.fileRefFullPath = Path.Combine(basePath, array[0]);
					}
					else
					{
						this.fileRefFullPath = array[0];
					}
					this.fileRefType = array[1];
					if (array.Length > 2)
					{
						this.fileRefTextEncoding = array[2];
					}
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0000B54C File Offset: 0x0000A54C
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x0000B581 File Offset: 0x0000A581
		public string Comment
		{
			get
			{
				string text = this.comment;
				if (text == null && this.nodeInfo != null)
				{
					text = this.nodeInfo.Comment;
				}
				if (text != null)
				{
					return text;
				}
				return "";
			}
			set
			{
				this.comment = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000B58C File Offset: 0x0000A58C
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x0000B5B8 File Offset: 0x0000A5B8
		public string Name
		{
			get
			{
				string text = this.name;
				if (text == null && this.nodeInfo != null)
				{
					text = this.nodeInfo.Name;
				}
				return text;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Name");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("Name");
				}
				this.name = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x0000B5E4 File Offset: 0x0000A5E4
		public ResXFileRef FileRef
		{
			get
			{
				if (this.FileRefFullPath == null)
				{
					return null;
				}
				if (this.fileRef == null)
				{
					if (string.IsNullOrEmpty(this.fileRefTextEncoding))
					{
						this.fileRef = new ResXFileRef(this.FileRefFullPath, this.FileRefType);
					}
					else
					{
						this.fileRef = new ResXFileRef(this.FileRefFullPath, this.FileRefType, Encoding.GetEncoding(this.FileRefTextEncoding));
					}
				}
				return this.fileRef;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0000B654 File Offset: 0x0000A654
		private string FileRefFullPath
		{
			get
			{
				string text = ((this.fileRef == null) ? null : this.fileRef.FileName);
				if (text == null)
				{
					text = this.fileRefFullPath;
				}
				return text;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0000B684 File Offset: 0x0000A684
		private string FileRefType
		{
			get
			{
				string text = ((this.fileRef == null) ? null : this.fileRef.TypeName);
				if (text == null)
				{
					text = this.fileRefType;
				}
				return text;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000B6B4 File Offset: 0x0000A6B4
		private string FileRefTextEncoding
		{
			get
			{
				string text = ((this.fileRef == null) ? null : ((this.fileRef.TextFileEncoding == null) ? null : this.fileRef.TextFileEncoding.BodyName));
				if (text == null)
				{
					text = this.fileRefTextEncoding;
				}
				return text;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000B6F8 File Offset: 0x0000A6F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private IFormatter CreateSoapFormatter()
		{
			return new SoapFormatter();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000B700 File Offset: 0x0000A700
		private static string ToBase64WrappedString(byte[] data)
		{
			string text = Convert.ToBase64String(data);
			if (text.Length > 80)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / 80 * 3);
				int i;
				for (i = 0; i < text.Length - 80; i += 80)
				{
					stringBuilder.Append("\r\n");
					stringBuilder.Append("        ");
					stringBuilder.Append(text, i, 80);
				}
				stringBuilder.Append("\r\n");
				stringBuilder.Append("        ");
				stringBuilder.Append(text, i, text.Length - i);
				stringBuilder.Append("\r\n");
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000B7B0 File Offset: 0x0000A7B0
		private void FillDataNodeInfoFromObject(DataNodeInfo nodeInfo, object value)
		{
			if (value is string)
			{
				nodeInfo.ValueData = (string)value;
				if (value == null)
				{
					nodeInfo.TypeName = typeof(ResXNullRef).AssemblyQualifiedName;
					return;
				}
			}
			else
			{
				if (value is byte[])
				{
					nodeInfo.ValueData = ResXDataNode.ToBase64WrappedString((byte[])value);
					nodeInfo.TypeName = typeof(byte[]).AssemblyQualifiedName;
					return;
				}
				Type type = ((value == null) ? typeof(object) : value.GetType());
				if (value != null && !type.IsSerializable)
				{
					throw new InvalidOperationException(SR.GetString("NotSerializableType", new object[] { this.name, type.FullName }));
				}
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				bool flag = converter.CanConvertTo(typeof(string));
				bool flag2 = converter.CanConvertFrom(typeof(string));
				try
				{
					if (flag && flag2)
					{
						nodeInfo.ValueData = converter.ConvertToInvariantString(value);
						nodeInfo.TypeName = type.AssemblyQualifiedName;
						return;
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				catch
				{
				}
				bool flag3 = converter.CanConvertTo(typeof(byte[]));
				bool flag4 = converter.CanConvertFrom(typeof(byte[]));
				if (flag3 && flag4)
				{
					byte[] array = (byte[])converter.ConvertTo(value, typeof(byte[]));
					string text = ResXDataNode.ToBase64WrappedString(array);
					nodeInfo.ValueData = text;
					nodeInfo.MimeType = ResXResourceWriter.ByteArraySerializedObjectMimeType;
					nodeInfo.TypeName = type.AssemblyQualifiedName;
				}
				else
				{
					if (value == null)
					{
						nodeInfo.ValueData = string.Empty;
						nodeInfo.TypeName = typeof(ResXNullRef).AssemblyQualifiedName;
						return;
					}
					if (this.binaryFormatter == null)
					{
						this.binaryFormatter = new BinaryFormatter();
					}
					MemoryStream memoryStream = new MemoryStream();
					if (this.binaryFormatter == null)
					{
						this.binaryFormatter = new BinaryFormatter();
					}
					IFormatter formatter = this.binaryFormatter;
					formatter.Serialize(memoryStream, value);
					string text2 = ResXDataNode.ToBase64WrappedString(memoryStream.ToArray());
					nodeInfo.ValueData = text2;
					nodeInfo.MimeType = ResXResourceWriter.DefaultSerializedObjectMimeType;
					return;
				}
				return;
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000B9DC File Offset: 0x0000A9DC
		private object GenerateObjectFromDataNodeInfo(DataNodeInfo dataNodeInfo, ITypeResolutionService typeResolver)
		{
			object obj = null;
			string mimeType = dataNodeInfo.MimeType;
			string text = ((dataNodeInfo.TypeName == null || dataNodeInfo.TypeName.Length == 0) ? typeof(string).AssemblyQualifiedName : dataNodeInfo.TypeName);
			if (mimeType != null && mimeType.Length > 0)
			{
				if (string.Equals(mimeType, ResXResourceWriter.BinSerializedObjectMimeType) || string.Equals(mimeType, ResXResourceWriter.Beta2CompatSerializedObjectMimeType) || string.Equals(mimeType, ResXResourceWriter.CompatBinSerializedObjectMimeType))
				{
					string valueData = dataNodeInfo.ValueData;
					byte[] array = ResXDataNode.FromBase64WrappedString(valueData);
					if (this.binaryFormatter == null)
					{
						this.binaryFormatter = new BinaryFormatter();
						this.binaryFormatter.Binder = new ResXSerializationBinder(typeResolver);
					}
					IFormatter formatter = this.binaryFormatter;
					if (array != null && array.Length > 0)
					{
						obj = formatter.Deserialize(new MemoryStream(array));
						if (obj is ResXNullRef)
						{
							obj = null;
						}
					}
				}
				else if (string.Equals(mimeType, ResXResourceWriter.SoapSerializedObjectMimeType) || string.Equals(mimeType, ResXResourceWriter.CompatSoapSerializedObjectMimeType))
				{
					string valueData2 = dataNodeInfo.ValueData;
					byte[] array2 = ResXDataNode.FromBase64WrappedString(valueData2);
					if (array2 != null && array2.Length > 0)
					{
						IFormatter formatter2 = this.CreateSoapFormatter();
						obj = formatter2.Deserialize(new MemoryStream(array2));
						if (obj is ResXNullRef)
						{
							obj = null;
						}
					}
				}
				else if (string.Equals(mimeType, ResXResourceWriter.ByteArraySerializedObjectMimeType) && text != null && text.Length > 0)
				{
					Type type = this.ResolveType(text, typeResolver);
					if (type == null)
					{
						string @string = SR.GetString("TypeLoadException", new object[]
						{
							text,
							dataNodeInfo.ReaderPosition.Y,
							dataNodeInfo.ReaderPosition.X
						});
						XmlException ex = new XmlException(@string, null, dataNodeInfo.ReaderPosition.Y, dataNodeInfo.ReaderPosition.X);
						TypeLoadException ex2 = new TypeLoadException(@string, ex);
						throw ex2;
					}
					TypeConverter converter = TypeDescriptor.GetConverter(type);
					if (converter.CanConvertFrom(typeof(byte[])))
					{
						string valueData3 = dataNodeInfo.ValueData;
						byte[] array3 = ResXDataNode.FromBase64WrappedString(valueData3);
						if (array3 != null)
						{
							obj = converter.ConvertFrom(array3);
						}
					}
				}
			}
			else if (text != null && text.Length > 0)
			{
				Type type2 = this.ResolveType(text, typeResolver);
				if (type2 != null)
				{
					if (type2 == typeof(ResXNullRef))
					{
						return null;
					}
					if (text.IndexOf("System.Byte[]") != -1 && text.IndexOf("mscorlib") != -1)
					{
						return ResXDataNode.FromBase64WrappedString(dataNodeInfo.ValueData);
					}
					TypeConverter converter2 = TypeDescriptor.GetConverter(type2);
					if (!converter2.CanConvertFrom(typeof(string)))
					{
						return obj;
					}
					string valueData4 = dataNodeInfo.ValueData;
					try
					{
						return converter2.ConvertFromInvariantString(valueData4);
					}
					catch (NotSupportedException ex3)
					{
						string string2 = SR.GetString("NotSupported", new object[]
						{
							text,
							dataNodeInfo.ReaderPosition.Y,
							dataNodeInfo.ReaderPosition.X,
							ex3.Message
						});
						XmlException ex4 = new XmlException(string2, ex3, dataNodeInfo.ReaderPosition.Y, dataNodeInfo.ReaderPosition.X);
						NotSupportedException ex5 = new NotSupportedException(string2, ex4);
						throw ex5;
					}
				}
				string string3 = SR.GetString("TypeLoadException", new object[]
				{
					text,
					dataNodeInfo.ReaderPosition.Y,
					dataNodeInfo.ReaderPosition.X
				});
				XmlException ex6 = new XmlException(string3, null, dataNodeInfo.ReaderPosition.Y, dataNodeInfo.ReaderPosition.X);
				TypeLoadException ex7 = new TypeLoadException(string3, ex6);
				throw ex7;
			}
			return obj;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000BDB0 File Offset: 0x0000ADB0
		internal DataNodeInfo GetDataNodeInfo()
		{
			bool flag = true;
			if (this.nodeInfo != null)
			{
				flag = false;
			}
			else
			{
				this.nodeInfo = new DataNodeInfo();
			}
			this.nodeInfo.Name = this.Name;
			this.nodeInfo.Comment = this.Comment;
			if (flag || this.FileRefFullPath != null)
			{
				if (this.FileRefFullPath != null)
				{
					this.nodeInfo.ValueData = this.FileRef.ToString();
					this.nodeInfo.MimeType = null;
					this.nodeInfo.TypeName = typeof(ResXFileRef).AssemblyQualifiedName;
				}
				else
				{
					this.FillDataNodeInfoFromObject(this.nodeInfo, this.value);
				}
			}
			return this.nodeInfo;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000BE64 File Offset: 0x0000AE64
		public Point GetNodePosition()
		{
			if (this.nodeInfo == null)
			{
				return default(Point);
			}
			return this.nodeInfo.ReaderPosition;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000BE90 File Offset: 0x0000AE90
		public string GetValueTypeName(ITypeResolutionService typeResolver)
		{
			if (this.typeName == null || this.typeName.Length <= 0)
			{
				string text = this.FileRefType;
				Type type = null;
				if (text != null)
				{
					type = this.ResolveType(this.FileRefType, typeResolver);
				}
				else if (this.nodeInfo != null)
				{
					text = this.nodeInfo.TypeName;
					if (text == null || text.Length == 0)
					{
						if (this.nodeInfo.MimeType != null && this.nodeInfo.MimeType.Length > 0)
						{
							object obj = null;
							try
							{
								obj = this.GenerateObjectFromDataNodeInfo(this.nodeInfo, typeResolver);
							}
							catch (Exception ex)
							{
								if (ClientUtils.IsCriticalException(ex))
								{
									throw;
								}
								text = typeof(object).AssemblyQualifiedName;
							}
							catch
							{
								throw;
							}
							if (obj != null)
							{
								text = obj.GetType().AssemblyQualifiedName;
							}
						}
						else
						{
							text = typeof(string).AssemblyQualifiedName;
						}
					}
					else
					{
						type = this.ResolveType(this.nodeInfo.TypeName, typeResolver);
					}
				}
				if (type != null)
				{
					if (type == typeof(ResXNullRef))
					{
						text = typeof(object).AssemblyQualifiedName;
					}
					else
					{
						text = type.AssemblyQualifiedName;
					}
				}
				return text;
			}
			if (this.typeName.Equals(typeof(ResXNullRef).AssemblyQualifiedName))
			{
				return typeof(object).AssemblyQualifiedName;
			}
			return this.typeName;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000BFF8 File Offset: 0x0000AFF8
		public string GetValueTypeName(AssemblyName[] names)
		{
			return this.GetValueTypeName(new AssemblyNamesTypeResolutionService(names));
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000C008 File Offset: 0x0000B008
		public object GetValue(ITypeResolutionService typeResolver)
		{
			if (this.value != null)
			{
				return this.value;
			}
			object obj = null;
			if (this.FileRefFullPath != null)
			{
				Type type = this.ResolveType(this.FileRefType, typeResolver);
				if (type == null)
				{
					string @string = SR.GetString("TypeLoadExceptionShort", new object[] { this.FileRefType });
					TypeLoadException ex = new TypeLoadException(@string);
					throw ex;
				}
				if (this.FileRefTextEncoding != null)
				{
					this.fileRef = new ResXFileRef(this.FileRefFullPath, this.FileRefType, Encoding.GetEncoding(this.FileRefTextEncoding));
				}
				else
				{
					this.fileRef = new ResXFileRef(this.FileRefFullPath, this.FileRefType);
				}
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(ResXFileRef));
				obj = converter.ConvertFrom(this.fileRef.ToString());
			}
			else
			{
				if (obj != null || this.nodeInfo.ValueData == null)
				{
					return null;
				}
				obj = this.GenerateObjectFromDataNodeInfo(this.nodeInfo, typeResolver);
			}
			return obj;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000C0F6 File Offset: 0x0000B0F6
		public object GetValue(AssemblyName[] names)
		{
			return this.GetValue(new AssemblyNamesTypeResolutionService(names));
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000C104 File Offset: 0x0000B104
		private static byte[] FromBase64WrappedString(string text)
		{
			if (text.IndexOfAny(ResXDataNode.SpecialChars) != -1)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length);
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					if (c != '\n' && c != '\r' && c != ' ')
					{
						stringBuilder.Append(text[i]);
					}
				}
				return Convert.FromBase64String(stringBuilder.ToString());
			}
			return Convert.FromBase64String(text);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000C174 File Offset: 0x0000B174
		private Type ResolveType(string typeName, ITypeResolutionService typeResolver)
		{
			Type type = null;
			if (typeResolver != null)
			{
				type = typeResolver.GetType(typeName, false);
				if (type == null)
				{
					string[] array = typeName.Split(new char[] { ',' });
					if (array != null && array.Length >= 2)
					{
						string text = array[0].Trim();
						string text2 = array[1].Trim();
						text = text + ", " + text2;
						type = typeResolver.GetType(text, false);
					}
				}
			}
			if (type == null)
			{
				type = Type.GetType(typeName, false);
			}
			return type;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000C1E8 File Offset: 0x0000B1E8
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			DataNodeInfo dataNodeInfo = this.GetDataNodeInfo();
			si.AddValue("Name", dataNodeInfo.Name, typeof(string));
			si.AddValue("Comment", dataNodeInfo.Comment, typeof(string));
			si.AddValue("TypeName", dataNodeInfo.TypeName, typeof(string));
			si.AddValue("MimeType", dataNodeInfo.MimeType, typeof(string));
			si.AddValue("ValueData", dataNodeInfo.ValueData, typeof(string));
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000C284 File Offset: 0x0000B284
		private ResXDataNode(SerializationInfo info, StreamingContext context)
		{
			this.nodeInfo = new DataNodeInfo
			{
				Name = (string)info.GetValue("Name", typeof(string)),
				Comment = (string)info.GetValue("Comment", typeof(string)),
				TypeName = (string)info.GetValue("TypeName", typeof(string)),
				MimeType = (string)info.GetValue("MimeType", typeof(string)),
				ValueData = (string)info.GetValue("ValueData", typeof(string))
			};
			this.InitializeDataNode(null);
		}

		// Token: 0x04000EE0 RID: 3808
		private static readonly char[] SpecialChars = new char[] { ' ', '\r', '\n' };

		// Token: 0x04000EE1 RID: 3809
		private DataNodeInfo nodeInfo;

		// Token: 0x04000EE2 RID: 3810
		private string name;

		// Token: 0x04000EE3 RID: 3811
		private string comment;

		// Token: 0x04000EE4 RID: 3812
		private string typeName;

		// Token: 0x04000EE5 RID: 3813
		private string fileRefFullPath;

		// Token: 0x04000EE6 RID: 3814
		private string fileRefType;

		// Token: 0x04000EE7 RID: 3815
		private string fileRefTextEncoding;

		// Token: 0x04000EE8 RID: 3816
		private object value;

		// Token: 0x04000EE9 RID: 3817
		private ResXFileRef fileRef;

		// Token: 0x04000EEA RID: 3818
		private IFormatter binaryFormatter;

		// Token: 0x04000EEB RID: 3819
		private static ITypeResolutionService internalTypeResolver = new AssemblyNamesTypeResolutionService(new AssemblyName[]
		{
			new AssemblyName("System.Windows.Forms")
		});
	}
}
