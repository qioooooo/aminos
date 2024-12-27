using System;

namespace System.Xml
{
	// Token: 0x020000EB RID: 235
	public class XmlNotation : XmlNode
	{
		// Token: 0x06000E73 RID: 3699 RVA: 0x00040531 File Offset: 0x0003F531
		internal XmlNotation(string name, string publicId, string systemId, XmlDocument doc)
			: base(doc)
		{
			this.name = doc.NameTable.Add(name);
			this.publicId = publicId;
			this.systemId = systemId;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0004055C File Offset: 0x0003F55C
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00040564 File Offset: 0x0003F564
		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0004056C File Offset: 0x0003F56C
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Notation;
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00040570 File Offset: 0x0003F570
		public override XmlNode CloneNode(bool deep)
		{
			throw new InvalidOperationException(Res.GetString("Xdom_Node_Cloning"));
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00040581 File Offset: 0x0003F581
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00040584 File Offset: 0x0003F584
		public string PublicId
		{
			get
			{
				return this.publicId;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0004058C File Offset: 0x0003F58C
		public string SystemId
		{
			get
			{
				return this.systemId;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00040594 File Offset: 0x0003F594
		public override string OuterXml
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0004059B File Offset: 0x0003F59B
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x000405A2 File Offset: 0x0003F5A2
		public override string InnerXml
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Set_InnerXml"));
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000405B3 File Offset: 0x0003F5B3
		public override void WriteTo(XmlWriter w)
		{
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000405B5 File Offset: 0x0003F5B5
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x040009A2 RID: 2466
		private string publicId;

		// Token: 0x040009A3 RID: 2467
		private string systemId;

		// Token: 0x040009A4 RID: 2468
		private string name;
	}
}
