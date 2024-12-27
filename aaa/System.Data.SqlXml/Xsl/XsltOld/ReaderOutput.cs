using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Utils;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200018D RID: 397
	internal class ReaderOutput : XmlReader, RecordOutput
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x00050D6A File Offset: 0x0004FD6A
		internal ReaderOutput(Processor processor)
		{
			this.processor = processor;
			this.nameTable = processor.NameTable;
			this.Reset();
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00050DA1 File Offset: 0x0004FDA1
		public override XmlNodeType NodeType
		{
			get
			{
				return this.currentInfo.NodeType;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x00050DB0 File Offset: 0x0004FDB0
		public override string Name
		{
			get
			{
				string prefix = this.Prefix;
				string localName = this.LocalName;
				if (prefix == null || prefix.Length <= 0)
				{
					return localName;
				}
				if (localName.Length > 0)
				{
					return prefix + ":" + localName;
				}
				return prefix;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x00050DF0 File Offset: 0x0004FDF0
		public override string LocalName
		{
			get
			{
				return this.currentInfo.LocalName;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00050DFD File Offset: 0x0004FDFD
		public override string NamespaceURI
		{
			get
			{
				return this.currentInfo.NamespaceURI;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x00050E0A File Offset: 0x0004FE0A
		public override string Prefix
		{
			get
			{
				return this.currentInfo.Prefix;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00050E17 File Offset: 0x0004FE17
		public override bool HasValue
		{
			get
			{
				return XmlReader.HasValueInternal(this.NodeType);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x00050E24 File Offset: 0x0004FE24
		public override string Value
		{
			get
			{
				return this.currentInfo.Value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00050E31 File Offset: 0x0004FE31
		public override int Depth
		{
			get
			{
				return this.currentInfo.Depth;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x00050E3E File Offset: 0x0004FE3E
		public override string BaseURI
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00050E45 File Offset: 0x0004FE45
		public override bool IsEmptyElement
		{
			get
			{
				return this.currentInfo.IsEmptyTag;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00050E52 File Offset: 0x0004FE52
		public override char QuoteChar
		{
			get
			{
				return this.encoder.QuoteChar;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00050E5F File Offset: 0x0004FE5F
		public override bool IsDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x00050E62 File Offset: 0x0004FE62
		public override XmlSpace XmlSpace
		{
			get
			{
				if (this.manager == null)
				{
					return XmlSpace.None;
				}
				return this.manager.XmlSpace;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x00050E79 File Offset: 0x0004FE79
		public override string XmlLang
		{
			get
			{
				if (this.manager == null)
				{
					return string.Empty;
				}
				return this.manager.XmlLang;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00050E94 File Offset: 0x0004FE94
		public override int AttributeCount
		{
			get
			{
				return this.attributeCount;
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00050E9C File Offset: 0x0004FE9C
		public override string GetAttribute(string name)
		{
			int num;
			if (this.FindAttribute(name, out num))
			{
				return ((BuilderInfo)this.attributeList[num]).Value;
			}
			return null;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00050ECC File Offset: 0x0004FECC
		public override string GetAttribute(string localName, string namespaceURI)
		{
			int num;
			if (this.FindAttribute(localName, namespaceURI, out num))
			{
				return ((BuilderInfo)this.attributeList[num]).Value;
			}
			return null;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00050F00 File Offset: 0x0004FF00
		public override string GetAttribute(int i)
		{
			BuilderInfo builderInfo = this.GetBuilderInfo(i);
			return builderInfo.Value;
		}

		// Token: 0x1700029E RID: 670
		public override string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}

		// Token: 0x1700029F RID: 671
		public override string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		// Token: 0x170002A0 RID: 672
		public override string this[string name, string namespaceURI]
		{
			get
			{
				return this.GetAttribute(name, namespaceURI);
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00050F38 File Offset: 0x0004FF38
		public override bool MoveToAttribute(string name)
		{
			int num;
			if (this.FindAttribute(name, out num))
			{
				this.SetAttribute(num);
				return true;
			}
			return false;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00050F5C File Offset: 0x0004FF5C
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			int num;
			if (this.FindAttribute(localName, namespaceURI, out num))
			{
				this.SetAttribute(num);
				return true;
			}
			return false;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00050F7F File Offset: 0x0004FF7F
		public override void MoveToAttribute(int i)
		{
			if (i < 0 || this.attributeCount <= i)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			this.SetAttribute(i);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00050FA0 File Offset: 0x0004FFA0
		public override bool MoveToFirstAttribute()
		{
			if (this.attributeCount <= 0)
			{
				return false;
			}
			this.SetAttribute(0);
			return true;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00050FB5 File Offset: 0x0004FFB5
		public override bool MoveToNextAttribute()
		{
			if (this.currentIndex + 1 < this.attributeCount)
			{
				this.SetAttribute(this.currentIndex + 1);
				return true;
			}
			return false;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00050FD8 File Offset: 0x0004FFD8
		public override bool MoveToElement()
		{
			if (this.NodeType == XmlNodeType.Attribute || this.currentInfo == this.attributeValue)
			{
				this.SetMainNode();
				return true;
			}
			return false;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00050FFC File Offset: 0x0004FFFC
		public override bool Read()
		{
			if (this.state != ReadState.Interactive)
			{
				if (this.state != ReadState.Initial)
				{
					return false;
				}
				this.state = ReadState.Interactive;
			}
			for (;;)
			{
				if (this.haveRecord)
				{
					this.processor.ResetOutput();
					this.haveRecord = false;
				}
				this.processor.Execute();
				if (!this.haveRecord)
				{
					goto IL_00A0;
				}
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Text)
				{
					if (nodeType != XmlNodeType.Whitespace)
					{
						break;
					}
				}
				else
				{
					if (!this.xmlCharType.IsOnlyWhitespace(this.Value))
					{
						break;
					}
					this.currentInfo.NodeType = XmlNodeType.Whitespace;
				}
				if (this.Value.Length != 0)
				{
					goto Block_8;
				}
			}
			goto IL_00AD;
			Block_8:
			if (this.XmlSpace == XmlSpace.Preserve)
			{
				this.currentInfo.NodeType = XmlNodeType.SignificantWhitespace;
				goto IL_00AD;
			}
			goto IL_00AD;
			IL_00A0:
			this.state = ReadState.EndOfFile;
			this.Reset();
			IL_00AD:
			return this.haveRecord;
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x000510BC File Offset: 0x000500BC
		public override bool EOF
		{
			get
			{
				return this.state == ReadState.EndOfFile;
			}
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000510C7 File Offset: 0x000500C7
		public override void Close()
		{
			this.processor = null;
			this.state = ReadState.Closed;
			this.Reset();
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x000510DD File Offset: 0x000500DD
		public override ReadState ReadState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000510E8 File Offset: 0x000500E8
		public override string ReadString()
		{
			string text = string.Empty;
			if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.Attribute || this.currentInfo == this.attributeValue)
			{
				if (this.mainNode.IsEmptyTag)
				{
					return text;
				}
				if (!this.Read())
				{
					throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
				}
			}
			StringBuilder stringBuilder = null;
			bool flag = true;
			for (;;)
			{
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Text)
				{
					switch (nodeType)
					{
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						goto IL_006F;
					}
					break;
				}
				IL_006F:
				if (flag)
				{
					text = this.Value;
					flag = false;
				}
				else
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(text);
					}
					stringBuilder.Append(this.Value);
				}
				if (!this.Read())
				{
					goto Block_9;
				}
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return text;
			Block_9:
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000511AC File Offset: 0x000501AC
		public override string ReadInnerXml()
		{
			if (this.ReadState == ReadState.Interactive)
			{
				if (this.NodeType == XmlNodeType.Element && !this.IsEmptyElement)
				{
					StringOutput stringOutput = new StringOutput(this.processor);
					stringOutput.OmitXmlDecl();
					int i = this.Depth;
					this.Read();
					while (i < this.Depth)
					{
						stringOutput.RecordDone(this.builder);
						this.Read();
					}
					this.Read();
					stringOutput.TheEnd();
					return stringOutput.Result;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.encoder.AtributeInnerXml(this.Value);
				}
				this.Read();
			}
			return string.Empty;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00051250 File Offset: 0x00050250
		public override string ReadOuterXml()
		{
			if (this.ReadState == ReadState.Interactive)
			{
				if (this.NodeType == XmlNodeType.Element)
				{
					StringOutput stringOutput = new StringOutput(this.processor);
					stringOutput.OmitXmlDecl();
					bool isEmptyElement = this.IsEmptyElement;
					int i = this.Depth;
					stringOutput.RecordDone(this.builder);
					this.Read();
					while (i < this.Depth)
					{
						stringOutput.RecordDone(this.builder);
						this.Read();
					}
					if (!isEmptyElement)
					{
						stringOutput.RecordDone(this.builder);
						this.Read();
					}
					stringOutput.TheEnd();
					return stringOutput.Result;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.encoder.AtributeOuterXml(this.Name, this.Value);
				}
				this.Read();
			}
			return string.Empty;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x00051316 File Offset: 0x00050316
		public override XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0005131E File Offset: 0x0005031E
		public override string LookupNamespace(string prefix)
		{
			prefix = this.nameTable.Get(prefix);
			if (this.manager != null && prefix != null)
			{
				return this.manager.ResolveNamespace(prefix);
			}
			return null;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00051347 File Offset: 0x00050347
		public override void ResolveEntity()
		{
			if (this.NodeType != XmlNodeType.EntityReference)
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00051364 File Offset: 0x00050364
		public override bool ReadAttributeValue()
		{
			if (this.ReadState != ReadState.Interactive || this.NodeType != XmlNodeType.Attribute)
			{
				return false;
			}
			if (this.attributeValue == null)
			{
				this.attributeValue = new BuilderInfo();
				this.attributeValue.NodeType = XmlNodeType.Text;
			}
			if (this.currentInfo == this.attributeValue)
			{
				return false;
			}
			this.attributeValue.Value = this.currentInfo.Value;
			this.attributeValue.Depth = this.currentInfo.Depth + 1;
			this.currentInfo = this.attributeValue;
			return true;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x000513F0 File Offset: 0x000503F0
		public Processor.OutputResult RecordDone(RecordBuilder record)
		{
			this.builder = record;
			this.mainNode = record.MainNode;
			this.attributeList = record.AttributeList;
			this.attributeCount = record.AttributeCount;
			this.manager = record.Manager;
			this.haveRecord = true;
			this.SetMainNode();
			return Processor.OutputResult.Interrupt;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00051442 File Offset: 0x00050442
		public void TheEnd()
		{
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00051444 File Offset: 0x00050444
		private void SetMainNode()
		{
			this.currentIndex = -1;
			this.currentInfo = this.mainNode;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00051459 File Offset: 0x00050459
		private void SetAttribute(int attrib)
		{
			this.currentIndex = attrib;
			this.currentInfo = (BuilderInfo)this.attributeList[attrib];
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00051479 File Offset: 0x00050479
		private BuilderInfo GetBuilderInfo(int attrib)
		{
			if (attrib < 0 || this.attributeCount <= attrib)
			{
				throw new ArgumentOutOfRangeException("attrib");
			}
			return (BuilderInfo)this.attributeList[attrib];
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x000514A4 File Offset: 0x000504A4
		private bool FindAttribute(string localName, string namespaceURI, out int attrIndex)
		{
			if (namespaceURI == null)
			{
				namespaceURI = string.Empty;
			}
			if (localName == null)
			{
				localName = string.Empty;
			}
			for (int i = 0; i < this.attributeCount; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[i];
				if (builderInfo.NamespaceURI == namespaceURI && builderInfo.LocalName == localName)
				{
					attrIndex = i;
					return true;
				}
			}
			attrIndex = -1;
			return false;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00051510 File Offset: 0x00050510
		private bool FindAttribute(string name, out int attrIndex)
		{
			if (name == null)
			{
				name = string.Empty;
			}
			for (int i = 0; i < this.attributeCount; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[i];
				if (builderInfo.Name == name)
				{
					attrIndex = i;
					return true;
				}
			}
			attrIndex = -1;
			return false;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00051561 File Offset: 0x00050561
		private void Reset()
		{
			this.currentIndex = -1;
			this.currentInfo = ReaderOutput.s_DefaultInfo;
			this.mainNode = ReaderOutput.s_DefaultInfo;
			this.manager = null;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00051587 File Offset: 0x00050587
		[Conditional("DEBUG")]
		private void CheckCurrentInfo()
		{
		}

		// Token: 0x04000B26 RID: 2854
		private Processor processor;

		// Token: 0x04000B27 RID: 2855
		private XmlNameTable nameTable;

		// Token: 0x04000B28 RID: 2856
		private RecordBuilder builder;

		// Token: 0x04000B29 RID: 2857
		private BuilderInfo mainNode;

		// Token: 0x04000B2A RID: 2858
		private ArrayList attributeList;

		// Token: 0x04000B2B RID: 2859
		private int attributeCount;

		// Token: 0x04000B2C RID: 2860
		private BuilderInfo attributeValue;

		// Token: 0x04000B2D RID: 2861
		private OutputScopeManager manager;

		// Token: 0x04000B2E RID: 2862
		private int currentIndex;

		// Token: 0x04000B2F RID: 2863
		private BuilderInfo currentInfo;

		// Token: 0x04000B30 RID: 2864
		private ReadState state;

		// Token: 0x04000B31 RID: 2865
		private bool haveRecord;

		// Token: 0x04000B32 RID: 2866
		private static BuilderInfo s_DefaultInfo = new BuilderInfo();

		// Token: 0x04000B33 RID: 2867
		private ReaderOutput.XmlEncoder encoder = new ReaderOutput.XmlEncoder();

		// Token: 0x04000B34 RID: 2868
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x0200018E RID: 398
		private class XmlEncoder
		{
			// Token: 0x060010E9 RID: 4329 RVA: 0x00051595 File Offset: 0x00050595
			private void Init()
			{
				this.buffer = new StringBuilder();
				this.encoder = new XmlTextEncoder(new StringWriter(this.buffer, CultureInfo.InvariantCulture));
			}

			// Token: 0x060010EA RID: 4330 RVA: 0x000515C0 File Offset: 0x000505C0
			public string AtributeInnerXml(string value)
			{
				if (this.encoder == null)
				{
					this.Init();
				}
				this.buffer.Length = 0;
				this.encoder.StartAttribute(false);
				this.encoder.Write(value);
				this.encoder.EndAttribute();
				return this.buffer.ToString();
			}

			// Token: 0x060010EB RID: 4331 RVA: 0x00051618 File Offset: 0x00050618
			public string AtributeOuterXml(string name, string value)
			{
				if (this.encoder == null)
				{
					this.Init();
				}
				this.buffer.Length = 0;
				this.buffer.Append(name);
				this.buffer.Append('=');
				this.buffer.Append(this.QuoteChar);
				this.encoder.StartAttribute(false);
				this.encoder.Write(value);
				this.encoder.EndAttribute();
				this.buffer.Append(this.QuoteChar);
				return this.buffer.ToString();
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x060010EC RID: 4332 RVA: 0x000516AC File Offset: 0x000506AC
			public char QuoteChar
			{
				get
				{
					return '"';
				}
			}

			// Token: 0x04000B35 RID: 2869
			private StringBuilder buffer;

			// Token: 0x04000B36 RID: 2870
			private XmlTextEncoder encoder;
		}
	}
}
