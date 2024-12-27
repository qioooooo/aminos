using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000CD RID: 205
	public sealed class MimeTextMatch
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001B302 File Offset: 0x0001A302
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x0001B318 File Offset: 0x0001A318
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001B321 File Offset: 0x0001A321
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0001B337 File Offset: 0x0001A337
		[XmlAttribute("type")]
		public string Type
		{
			get
			{
				if (this.type != null)
				{
					return this.type;
				}
				return string.Empty;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001B340 File Offset: 0x0001A340
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0001B348 File Offset: 0x0001A348
		[DefaultValue(1)]
		[XmlAttribute("group")]
		public int Group
		{
			get
			{
				return this.group;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("WebNegativeValue", new object[] { "group" }));
				}
				this.group = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001B380 File Offset: 0x0001A380
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0001B388 File Offset: 0x0001A388
		[DefaultValue(0)]
		[XmlAttribute("capture")]
		public int Capture
		{
			get
			{
				return this.capture;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("WebNegativeValue", new object[] { "capture" }));
				}
				this.capture = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001B3C0 File Offset: 0x0001A3C0
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0001B3C8 File Offset: 0x0001A3C8
		[XmlIgnore]
		public int Repeats
		{
			get
			{
				return this.repeats;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("WebNegativeValue", new object[] { "repeats" }));
				}
				this.repeats = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001B400 File Offset: 0x0001A400
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x0001B425 File Offset: 0x0001A425
		[XmlAttribute("repeats")]
		[DefaultValue("1")]
		public string RepeatsString
		{
			get
			{
				if (this.repeats != 2147483647)
				{
					return this.repeats.ToString(CultureInfo.InvariantCulture);
				}
				return "*";
			}
			set
			{
				if (value == "*")
				{
					this.repeats = int.MaxValue;
					return;
				}
				this.Repeats = int.Parse(value, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001B451 File Offset: 0x0001A451
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0001B467 File Offset: 0x0001A467
		[XmlAttribute("pattern")]
		public string Pattern
		{
			get
			{
				if (this.pattern != null)
				{
					return this.pattern;
				}
				return string.Empty;
			}
			set
			{
				this.pattern = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001B470 File Offset: 0x0001A470
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001B478 File Offset: 0x0001A478
		[XmlAttribute("ignoreCase")]
		public bool IgnoreCase
		{
			get
			{
				return this.ignoreCase;
			}
			set
			{
				this.ignoreCase = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001B481 File Offset: 0x0001A481
		[XmlElement("match")]
		public MimeTextMatchCollection Matches
		{
			get
			{
				return this.matches;
			}
		}

		// Token: 0x0400041D RID: 1053
		private string name;

		// Token: 0x0400041E RID: 1054
		private string type;

		// Token: 0x0400041F RID: 1055
		private int repeats = 1;

		// Token: 0x04000420 RID: 1056
		private string pattern;

		// Token: 0x04000421 RID: 1057
		private int group = 1;

		// Token: 0x04000422 RID: 1058
		private int capture;

		// Token: 0x04000423 RID: 1059
		private bool ignoreCase;

		// Token: 0x04000424 RID: 1060
		private MimeTextMatchCollection matches = new MimeTextMatchCollection();
	}
}
