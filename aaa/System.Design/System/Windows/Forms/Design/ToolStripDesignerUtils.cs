using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002BC RID: 700
	internal sealed class ToolStripDesignerUtils
	{
		// Token: 0x06001A45 RID: 6725 RVA: 0x0008E8D5 File Offset: 0x0008D8D5
		private ToolStripDesignerUtils()
		{
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0008E8E0 File Offset: 0x0008D8E0
		public static void GetAdjustedBounds(ToolStripItem item, ref Rectangle r)
		{
			if (!(item is ToolStripControlHost) || !item.IsOnDropDown)
			{
				if (item is ToolStripMenuItem && item.IsOnDropDown)
				{
					r.Inflate(-3, -2);
					r.Width++;
					return;
				}
				if (item is ToolStripControlHost && !item.IsOnDropDown)
				{
					r.Inflate(0, -2);
					return;
				}
				if (item is ToolStripMenuItem && !item.IsOnDropDown)
				{
					r.Inflate(-3, -3);
					return;
				}
				r.Inflate(-1, -1);
			}
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0008E964 File Offset: 0x0008D964
		private static ToolStrip GetToolStripFromComponent(IComponent component)
		{
			ToolStripItem toolStripItem = component as ToolStripItem;
			ToolStrip toolStrip;
			if (toolStripItem != null)
			{
				if (!(toolStripItem is ToolStripDropDownItem))
				{
					toolStrip = toolStripItem.Owner;
				}
				else
				{
					toolStrip = ((ToolStripDropDownItem)toolStripItem).DropDown;
				}
			}
			else
			{
				toolStrip = component as ToolStrip;
			}
			return toolStrip;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0008E9A4 File Offset: 0x0008D9A4
		private static ToolboxItem GetCachedToolboxItem(Type itemType)
		{
			ToolboxItem toolboxItem = null;
			if (ToolStripDesignerUtils.CachedToolboxItems == null)
			{
				ToolStripDesignerUtils.CachedToolboxItems = new Dictionary<Type, ToolboxItem>();
			}
			else if (ToolStripDesignerUtils.CachedToolboxItems.ContainsKey(itemType))
			{
				return ToolStripDesignerUtils.CachedToolboxItems[itemType];
			}
			if (toolboxItem == null)
			{
				toolboxItem = ToolboxService.GetToolboxItem(itemType);
				if (toolboxItem == null)
				{
					toolboxItem = new ToolboxItem(itemType);
				}
			}
			ToolStripDesignerUtils.CachedToolboxItems[itemType] = toolboxItem;
			if (ToolStripDesignerUtils.CustomToolStripItemCount > 0 && ToolStripDesignerUtils.CustomToolStripItemCount * 2 < ToolStripDesignerUtils.CachedToolboxItems.Count)
			{
				ToolStripDesignerUtils.CachedToolboxItems.Clear();
			}
			return toolboxItem;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0008EA28 File Offset: 0x0008DA28
		private static Bitmap GetKnownToolboxBitmap(Type itemType)
		{
			if (ToolStripDesignerUtils.CachedWinformsImages == null)
			{
				ToolStripDesignerUtils.CachedWinformsImages = new Dictionary<Type, Bitmap>();
			}
			if (!ToolStripDesignerUtils.CachedWinformsImages.ContainsKey(itemType))
			{
				Bitmap bitmap = ToolboxBitmapAttribute.GetImageFromResource(itemType, null, false) as Bitmap;
				ToolStripDesignerUtils.CachedWinformsImages[itemType] = bitmap;
				return bitmap;
			}
			return ToolStripDesignerUtils.CachedWinformsImages[itemType];
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0008EA7C File Offset: 0x0008DA7C
		public static Bitmap GetToolboxBitmap(Type itemType)
		{
			if (itemType.Namespace == ToolStripDesignerUtils.systemWindowsFormsNamespace)
			{
				return ToolStripDesignerUtils.GetKnownToolboxBitmap(itemType);
			}
			ToolboxItem cachedToolboxItem = ToolStripDesignerUtils.GetCachedToolboxItem(itemType);
			if (cachedToolboxItem != null)
			{
				return cachedToolboxItem.Bitmap;
			}
			return ToolStripDesignerUtils.GetKnownToolboxBitmap(typeof(Component));
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0008EAC4 File Offset: 0x0008DAC4
		public static string GetToolboxDescription(Type itemType)
		{
			string text = null;
			ToolboxItem cachedToolboxItem = ToolStripDesignerUtils.GetCachedToolboxItem(itemType);
			if (cachedToolboxItem != null)
			{
				text = cachedToolboxItem.DisplayName;
			}
			if (text == null)
			{
				text = itemType.Name;
			}
			if (text.StartsWith("ToolStrip"))
			{
				return text.Substring(9);
			}
			return text;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0008EB08 File Offset: 0x0008DB08
		public static Type[] GetStandardItemTypes(IComponent component)
		{
			ToolStrip toolStripFromComponent = ToolStripDesignerUtils.GetToolStripFromComponent(component);
			if (toolStripFromComponent is MenuStrip)
			{
				return ToolStripDesignerUtils.NewItemTypesForMenuStrip;
			}
			if (toolStripFromComponent is ToolStripDropDownMenu)
			{
				return ToolStripDesignerUtils.NewItemTypesForToolStripDropDownMenu;
			}
			if (toolStripFromComponent is StatusStrip)
			{
				return ToolStripDesignerUtils.NewItemTypesForStatusStrip;
			}
			return ToolStripDesignerUtils.NewItemTypesForToolStrip;
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x0008EB4C File Offset: 0x0008DB4C
		private static ToolStripItemDesignerAvailability GetDesignerVisibility(ToolStrip toolStrip)
		{
			ToolStripItemDesignerAvailability toolStripItemDesignerAvailability;
			if (toolStrip is StatusStrip)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.StatusStrip;
			}
			else if (toolStrip is MenuStrip)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.MenuStrip;
			}
			else if (toolStrip is ToolStripDropDownMenu)
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.ContextMenuStrip;
			}
			else
			{
				toolStripItemDesignerAvailability = ToolStripItemDesignerAvailability.ToolStrip;
			}
			return toolStripItemDesignerAvailability;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0008EB84 File Offset: 0x0008DB84
		public static Type[] GetCustomItemTypes(IComponent component, IServiceProvider serviceProvider)
		{
			ITypeDiscoveryService typeDiscoveryService = null;
			if (serviceProvider != null)
			{
				typeDiscoveryService = serviceProvider.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
			}
			return ToolStripDesignerUtils.GetCustomItemTypes(component, typeDiscoveryService);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0008EBB4 File Offset: 0x0008DBB4
		public static Type[] GetCustomItemTypes(IComponent component, ITypeDiscoveryService discoveryService)
		{
			if (discoveryService != null)
			{
				ICollection types = discoveryService.GetTypes(ToolStripDesignerUtils.toolStripItemType, false);
				ToolStrip toolStripFromComponent = ToolStripDesignerUtils.GetToolStripFromComponent(component);
				ToolStripItemDesignerAvailability designerVisibility = ToolStripDesignerUtils.GetDesignerVisibility(toolStripFromComponent);
				Type[] standardItemTypes = ToolStripDesignerUtils.GetStandardItemTypes(component);
				if (designerVisibility != ToolStripItemDesignerAvailability.None)
				{
					ArrayList arrayList = new ArrayList(types.Count);
					foreach (object obj in types)
					{
						Type type = (Type)obj;
						if (!type.IsAbstract && (type.IsPublic || type.IsNestedPublic) && !type.ContainsGenericParameters)
						{
							ConstructorInfo constructor = type.GetConstructor(new Type[0]);
							if (constructor != null)
							{
								ToolStripItemDesignerAvailabilityAttribute toolStripItemDesignerAvailabilityAttribute = (ToolStripItemDesignerAvailabilityAttribute)TypeDescriptor.GetAttributes(type)[typeof(ToolStripItemDesignerAvailabilityAttribute)];
								if (toolStripItemDesignerAvailabilityAttribute != null && (toolStripItemDesignerAvailabilityAttribute.ItemAdditionVisibility & designerVisibility) == designerVisibility)
								{
									bool flag = false;
									foreach (Type type2 in standardItemTypes)
									{
										if (type2 == type)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										arrayList.Add(type);
									}
								}
							}
						}
					}
					if (arrayList.Count > 0)
					{
						Type[] array2 = new Type[arrayList.Count];
						arrayList.CopyTo(array2, 0);
						ToolStripDesignerUtils.CustomToolStripItemCount = arrayList.Count;
						return array2;
					}
				}
			}
			ToolStripDesignerUtils.CustomToolStripItemCount = 0;
			return new Type[0];
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0008ED2C File Offset: 0x0008DD2C
		public static ToolStripItem[] GetStandardItemMenuItems(IComponent component, EventHandler onClick, bool convertTo)
		{
			Type[] standardItemTypes = ToolStripDesignerUtils.GetStandardItemTypes(component);
			ToolStripItem[] array = new ToolStripItem[standardItemTypes.Length];
			for (int i = 0; i < standardItemTypes.Length; i++)
			{
				ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = new ItemTypeToolStripMenuItem(standardItemTypes[i]);
				itemTypeToolStripMenuItem.ConvertTo = convertTo;
				if (onClick != null)
				{
					itemTypeToolStripMenuItem.Click += onClick;
				}
				array[i] = itemTypeToolStripMenuItem;
			}
			return array;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0008ED78 File Offset: 0x0008DD78
		public static ToolStripItem[] GetCustomItemMenuItems(IComponent component, EventHandler onClick, bool convertTo, IServiceProvider serviceProvider)
		{
			Type[] customItemTypes = ToolStripDesignerUtils.GetCustomItemTypes(component, serviceProvider);
			ToolStripItem[] array = new ToolStripItem[customItemTypes.Length];
			for (int i = 0; i < customItemTypes.Length; i++)
			{
				ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = new ItemTypeToolStripMenuItem(customItemTypes[i]);
				itemTypeToolStripMenuItem.ConvertTo = convertTo;
				if (onClick != null)
				{
					itemTypeToolStripMenuItem.Click += onClick;
				}
				array[i] = itemTypeToolStripMenuItem;
			}
			return array;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0008EDC4 File Offset: 0x0008DDC4
		public static ToolStripDropDown GetNewItemDropDown(IComponent component, ToolStripItem currentItem, EventHandler onClick, bool convertTo, IServiceProvider serviceProvider)
		{
			NewItemsContextMenuStrip newItemsContextMenuStrip = new NewItemsContextMenuStrip(component, currentItem, onClick, convertTo, serviceProvider);
			newItemsContextMenuStrip.GroupOrdering.Add("StandardList");
			newItemsContextMenuStrip.GroupOrdering.Add("CustomList");
			foreach (ToolStripItem toolStripItem in ToolStripDesignerUtils.GetStandardItemMenuItems(component, onClick, convertTo))
			{
				newItemsContextMenuStrip.Groups["StandardList"].Items.Add(toolStripItem);
				if (convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = toolStripItem as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem != null && currentItem != null && itemTypeToolStripMenuItem.ItemType == currentItem.GetType())
					{
						itemTypeToolStripMenuItem.Enabled = false;
					}
				}
			}
			foreach (ToolStripItem toolStripItem2 in ToolStripDesignerUtils.GetCustomItemMenuItems(component, onClick, convertTo, serviceProvider))
			{
				newItemsContextMenuStrip.Groups["CustomList"].Items.Add(toolStripItem2);
				if (convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem2 = toolStripItem2 as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem2 != null && currentItem != null && itemTypeToolStripMenuItem2.ItemType == currentItem.GetType())
					{
						itemTypeToolStripMenuItem2.Enabled = false;
					}
				}
			}
			IUIService iuiservice = serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				newItemsContextMenuStrip.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
				newItemsContextMenuStrip.Font = (Font)iuiservice.Styles["DialogFont"];
			}
			newItemsContextMenuStrip.Populate();
			return newItemsContextMenuStrip;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0008EF28 File Offset: 0x0008DF28
		public static void InvalidateSelection(ArrayList originalSelComps, ToolStripItem nextSelection, IServiceProvider provider, bool shiftPressed)
		{
			if (nextSelection == null || provider == null)
			{
				return;
			}
			Region region = null;
			Region region2 = null;
			int num = 1;
			int num2 = 2;
			bool flag = false;
			try
			{
				Rectangle rectangle = Rectangle.Empty;
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					foreach (object obj in originalSelComps)
					{
						Component component = (Component)obj;
						ToolStripItem toolStripItem = component as ToolStripItem;
						if (toolStripItem != null && (originalSelComps.Count > 1 || (originalSelComps.Count == 1 && toolStripItem.GetCurrentParent() != nextSelection.GetCurrentParent()) || toolStripItem is ToolStripSeparator || toolStripItem is ToolStripControlHost || !toolStripItem.IsOnDropDown || toolStripItem.IsOnOverflow))
						{
							ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(toolStripItem) as ToolStripItemDesigner;
							if (toolStripItemDesigner != null)
							{
								rectangle = toolStripItemDesigner.GetGlyphBounds();
								ToolStripDesignerUtils.GetAdjustedBounds(toolStripItem, ref rectangle);
								rectangle.Inflate(num, num);
								if (region == null)
								{
									region = new Region(rectangle);
									rectangle.Inflate(-num2, -num2);
									region.Exclude(rectangle);
								}
								else
								{
									region2 = new Region(rectangle);
									rectangle.Inflate(-num2, -num2);
									region2.Exclude(rectangle);
									region.Union(region2);
								}
							}
							else if (toolStripItem is DesignerToolStripControlHost)
							{
								flag = true;
							}
						}
					}
				}
				if (region != null || flag || shiftPressed)
				{
					BehaviorService behaviorService = (BehaviorService)provider.GetService(typeof(BehaviorService));
					if (behaviorService != null)
					{
						if (region != null)
						{
							behaviorService.Invalidate(region);
						}
						ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(nextSelection) as ToolStripItemDesigner;
						if (toolStripItemDesigner != null)
						{
							rectangle = toolStripItemDesigner.GetGlyphBounds();
							ToolStripDesignerUtils.GetAdjustedBounds(nextSelection, ref rectangle);
							rectangle.Inflate(num, num);
							region = new Region(rectangle);
							rectangle.Inflate(-num2, -num2);
							region.Exclude(rectangle);
							behaviorService.Invalidate(region);
						}
					}
				}
			}
			finally
			{
				if (region != null)
				{
					region.Dispose();
				}
				if (region2 != null)
				{
					region2.Dispose();
				}
			}
		}

		// Token: 0x04001504 RID: 5380
		private const int TOOLSTRIPCHARCOUNT = 9;

		// Token: 0x04001505 RID: 5381
		private static Type toolStripItemType = typeof(ToolStripItem);

		// Token: 0x04001506 RID: 5382
		[ThreadStatic]
		private static Dictionary<Type, ToolboxItem> CachedToolboxItems;

		// Token: 0x04001507 RID: 5383
		[ThreadStatic]
		private static int CustomToolStripItemCount = 0;

		// Token: 0x04001508 RID: 5384
		public static ArrayList originalSelComps;

		// Token: 0x04001509 RID: 5385
		[ThreadStatic]
		private static Dictionary<Type, Bitmap> CachedWinformsImages;

		// Token: 0x0400150A RID: 5386
		private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;

		// Token: 0x0400150B RID: 5387
		private static readonly Type[] NewItemTypesForToolStrip = new Type[]
		{
			typeof(ToolStripButton),
			typeof(ToolStripLabel),
			typeof(ToolStripSplitButton),
			typeof(ToolStripDropDownButton),
			typeof(ToolStripSeparator),
			typeof(ToolStripComboBox),
			typeof(ToolStripTextBox),
			typeof(ToolStripProgressBar)
		};

		// Token: 0x0400150C RID: 5388
		private static readonly Type[] NewItemTypesForStatusStrip = new Type[]
		{
			typeof(ToolStripStatusLabel),
			typeof(ToolStripProgressBar),
			typeof(ToolStripDropDownButton),
			typeof(ToolStripSplitButton)
		};

		// Token: 0x0400150D RID: 5389
		private static readonly Type[] NewItemTypesForMenuStrip = new Type[]
		{
			typeof(ToolStripMenuItem),
			typeof(ToolStripComboBox),
			typeof(ToolStripTextBox)
		};

		// Token: 0x0400150E RID: 5390
		private static readonly Type[] NewItemTypesForToolStripDropDownMenu = new Type[]
		{
			typeof(ToolStripMenuItem),
			typeof(ToolStripComboBox),
			typeof(ToolStripSeparator),
			typeof(ToolStripTextBox)
		};

		// Token: 0x020002BD RID: 701
		internal static class DisplayInformation
		{
			// Token: 0x06001A55 RID: 6741 RVA: 0x0008F2AD File Offset: 0x0008E2AD
			static DisplayInformation()
			{
				SystemEvents.UserPreferenceChanged += ToolStripDesignerUtils.DisplayInformation.UserPreferenceChanged;
				SystemEvents.DisplaySettingsChanged += ToolStripDesignerUtils.DisplayInformation.DisplaySettingChanged;
			}

			// Token: 0x17000481 RID: 1153
			// (get) Token: 0x06001A56 RID: 6742 RVA: 0x0008F2D4 File Offset: 0x0008E2D4
			public static short BitsPerPixel
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.bitsPerPixel == 0)
					{
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						try
						{
							foreach (Screen screen in Screen.AllScreens)
							{
								if (ToolStripDesignerUtils.DisplayInformation.bitsPerPixel == 0)
								{
									ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = (short)screen.BitsPerPixel;
								}
								else
								{
									ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = (short)Math.Min(screen.BitsPerPixel, (int)ToolStripDesignerUtils.DisplayInformation.bitsPerPixel);
								}
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					return ToolStripDesignerUtils.DisplayInformation.bitsPerPixel;
				}
			}

			// Token: 0x17000482 RID: 1154
			// (get) Token: 0x06001A57 RID: 6743 RVA: 0x0008F354 File Offset: 0x0008E354
			public static bool LowResolution
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.lowResSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.lowRes;
					}
					ToolStripDesignerUtils.DisplayInformation.lowRes = ToolStripDesignerUtils.DisplayInformation.BitsPerPixel <= 8;
					ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.lowRes;
				}
			}

			// Token: 0x17000483 RID: 1155
			// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0008F37E File Offset: 0x0008E37E
			public static bool HighContrast
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.highContrast;
					}
					ToolStripDesignerUtils.DisplayInformation.highContrast = SystemInformation.HighContrast;
					ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.highContrast;
				}
			}

			// Token: 0x17000484 RID: 1156
			// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0008F3A2 File Offset: 0x0008E3A2
			public static bool IsDropShadowEnabled
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled;
					}
					ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled = SystemInformation.IsDropShadowEnabled;
					ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.dropShadowEnabled;
				}
			}

			// Token: 0x17000485 RID: 1157
			// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0008F3C6 File Offset: 0x0008E3C6
			public static bool TerminalServer
			{
				get
				{
					if (ToolStripDesignerUtils.DisplayInformation.terminalSettingValid)
					{
						return ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession;
					}
					ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession = SystemInformation.TerminalServerSession;
					ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = true;
					return ToolStripDesignerUtils.DisplayInformation.isTerminalServerSession;
				}
			}

			// Token: 0x06001A5B RID: 6747 RVA: 0x0008F3EA File Offset: 0x0008E3EA
			private static void DisplaySettingChanged(object obj, EventArgs ea)
			{
				ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = false;
			}

			// Token: 0x06001A5C RID: 6748 RVA: 0x0008F404 File Offset: 0x0008E404
			private static void UserPreferenceChanged(object obj, UserPreferenceChangedEventArgs ea)
			{
				ToolStripDesignerUtils.DisplayInformation.highContrastSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.lowResSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.terminalSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.dropShadowSettingValid = false;
				ToolStripDesignerUtils.DisplayInformation.bitsPerPixel = 0;
			}

			// Token: 0x0400150F RID: 5391
			private static bool highContrast;

			// Token: 0x04001510 RID: 5392
			private static bool lowRes;

			// Token: 0x04001511 RID: 5393
			private static bool isTerminalServerSession;

			// Token: 0x04001512 RID: 5394
			private static bool highContrastSettingValid;

			// Token: 0x04001513 RID: 5395
			private static bool lowResSettingValid;

			// Token: 0x04001514 RID: 5396
			private static bool terminalSettingValid;

			// Token: 0x04001515 RID: 5397
			private static short bitsPerPixel;

			// Token: 0x04001516 RID: 5398
			private static bool dropShadowSettingValid;

			// Token: 0x04001517 RID: 5399
			private static bool dropShadowEnabled;
		}
	}
}
