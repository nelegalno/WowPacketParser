using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V5_4_8_18291.Enums;

namespace WowPacketParserModule.V5_4_8_18291.Parsers
{
    public static class ItemHandler
    {
        [HasSniffData]
        [Parser(Opcode.SMSG_DB_REPLY)]
        public static void HandleDBReply(Packet packet)
        {
            var id = packet.ReadUInt32("Entry");
            var type = packet.ReadUInt32("Type"); // See DB2Hash enum. Left like this for now to see some numbers pop. ^^
            packet.ReadTime("Hotfix date");
            var size = packet.ReadUInt32("Size");

            if (size == 0 || id < 0)
                return;

            var HashType = (DB2Hash)type;
            var itemId = (uint)id;

            switch (HashType)
            {
                case DB2Hash.Item:    // Items
                {
                    var item = Storage.ItemTemplates.ContainsKey(itemId) ? Storage.ItemTemplates[itemId].Item1 : new ItemTemplate();

                    packet.ReadEntryWithName<UInt32>(StoreNameType.Item, "Entry");
                    item.Class = packet.ReadEnum<ItemClass>("Class", TypeCode.Int32);
                    item.SubClass = packet.ReadUInt32("Sub Class");
                    item.SoundOverrideSubclass = packet.ReadInt32("Sound Override Subclass");
                    item.Material = packet.ReadEnum<Material>("Material", TypeCode.Int32);
                    item.DisplayId = packet.ReadUInt32("Display ID");
                    item.InventoryType = packet.ReadEnum<InventoryType>("Inventory Type", TypeCode.UInt32);
                    item.SheathType = packet.ReadEnum<SheathType>("Sheath Type", TypeCode.Int32);

                    Storage.ItemTemplates.Add(itemId, item, packet.TimeSpan);
                    break;
                }
                case DB2Hash.Item_sparse:    // Item-sparse
                {
                    var item = Storage.ItemTemplates.ContainsKey(itemId) ? Storage.ItemTemplates[itemId].Item1 : new ItemTemplate();

                    packet.ReadEntryWithName<UInt32>(StoreNameType.Item, "Entry");
                    item.Quality = packet.ReadEnum<ItemQuality>("Quality", TypeCode.Int32);
                    item.Flags = packet.ReadEnum<ItemProtoFlags>("Flags", TypeCode.UInt32);
                    item.ExtraFlags = packet.ReadEnum<ItemFlagExtra>("Extra Flags", TypeCode.Int32);
                    item.Unk430_1 = packet.ReadSingle("Unk430_1");
                    item.Unk430_2 = packet.ReadSingle("Unk430_2");
                    item.BuyCount = packet.ReadUInt32("Buy count");
                    item.BuyPrice = packet.ReadUInt32("Buy Price");
                    item.SellPrice = packet.ReadUInt32("Sell Price");
                    item.InventoryType = packet.ReadEnum<InventoryType>("Inventory Type", TypeCode.Int32);
                    item.AllowedClasses = packet.ReadEnum<ClassMask>("Allowed Classes", TypeCode.Int32);
                    item.AllowedRaces = packet.ReadEnum<RaceMask>("Allowed Races", TypeCode.Int32);
                    item.ItemLevel = packet.ReadUInt32("Item Level");
                    item.RequiredLevel = packet.ReadUInt32("Required Level");
                    item.RequiredSkillId = packet.ReadUInt32("Required Skill ID");
                    item.RequiredSkillLevel = packet.ReadUInt32("Required Skill Level");
                    item.RequiredSpell = (uint)packet.ReadEntryWithName<Int32>(StoreNameType.Spell, "Required Spell");
                    item.RequiredHonorRank = packet.ReadUInt32("Required Honor Rank");
                    item.RequiredCityRank = packet.ReadUInt32("Required City Rank");
                    item.RequiredRepFaction = packet.ReadUInt32("Required Rep Faction");
                    item.RequiredRepValue = packet.ReadUInt32("Required Rep Value");
                    item.MaxCount = packet.ReadInt32("Max Count");
                    item.MaxStackSize = packet.ReadInt32("Max Stack Size");
                    item.ContainerSlots = packet.ReadUInt32("Container Slots");

                    item.StatTypes = new ItemModType[10];
                    for (var i = 0; i < 10; i++)
                    {
                        var statType = packet.ReadEnum<ItemModType>("Stat Type", TypeCode.Int32, i);
                        item.StatTypes[i] = statType == ItemModType.None ? ItemModType.Mana : statType; // TDB
                    }

                    item.StatValues = new int[10];
                    for (var i = 0; i < 10; i++)
                        item.StatValues[i] = packet.ReadInt32("Stat Value", i);

                    item.StatUnk1 = new int[10];
                    for (var i = 0; i < 10; i++)
                        item.StatUnk1[i] = packet.ReadInt32("Unk UInt32 1", i);

                    item.StatUnk2 = new int[10];
                    for (var i = 0; i < 10; i++)
                        item.StatUnk2[i] = packet.ReadInt32("Unk UInt32 2", i);

                    item.ScalingStatDistribution = packet.ReadInt32("Scaling Stat Distribution");
                    item.DamageType = packet.ReadEnum<DamageType>("Damage Type", TypeCode.Int32);
                    item.Delay = packet.ReadUInt32("Delay");
                    item.RangedMod = packet.ReadSingle("Ranged Mod");

                    item.TriggeredSpellIds = new int[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellIds[i] = packet.ReadEntryWithName<Int32>(StoreNameType.Spell, "Triggered Spell ID", i);

                    item.TriggeredSpellTypes = new ItemSpellTriggerType[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellTypes[i] = packet.ReadEnum<ItemSpellTriggerType>("Trigger Spell Type", TypeCode.Int32, i);

                    item.TriggeredSpellCharges = new int[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellCharges[i] = packet.ReadInt32("Triggered Spell Charges", i);

                    item.TriggeredSpellCooldowns = new int[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellCooldowns[i] = packet.ReadInt32("Triggered Spell Cooldown", i);

                    item.TriggeredSpellCategories = new uint[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellCategories[i] = packet.ReadUInt32("Triggered Spell Category", i);

                    item.TriggeredSpellCategoryCooldowns = new int[5];
                    for (var i = 0; i < 5; i++)
                        item.TriggeredSpellCategoryCooldowns[i] = packet.ReadInt32("Triggered Spell Category Cooldown", i);

                    item.Bonding = packet.ReadEnum<ItemBonding>("Bonding", TypeCode.Int32);

                    if (packet.ReadUInt16() > 0)
                        item.Name = packet.ReadCString("Name", 0);

                    for (var i = 1; i < 4; ++i)
                        if (packet.ReadUInt16() > 0)
                            packet.ReadCString("Name", i);

                    if (packet.ReadUInt16() > 0)
                        item.Description = packet.ReadCString("Description");

                    item.PageText = packet.ReadUInt32("Page Text");
                    item.Language = packet.ReadEnum<Language>("Language", TypeCode.Int32);
                    item.PageMaterial = packet.ReadEnum<PageMaterial>("Page Material", TypeCode.Int32);
                    item.StartQuestId = (uint)packet.ReadEntryWithName<Int32>(StoreNameType.Quest, "Start Quest");
                    item.LockId = packet.ReadUInt32("Lock ID");
                    item.Material = packet.ReadEnum<Material>("Material", TypeCode.Int32);
                    item.SheathType = packet.ReadEnum<SheathType>("Sheath Type", TypeCode.Int32);
                    item.RandomPropery = packet.ReadInt32("Random Property");
                    item.RandomSuffix = packet.ReadUInt32("Random Suffix");
                    item.ItemSet = packet.ReadUInt32("Item Set");
                    item.AreaId = (uint)packet.ReadEntryWithName<UInt32>(StoreNameType.Area, "Area");
                    // In this single (?) case, map 0 means no map
                    var map = packet.ReadInt32();
                    item.MapId = map;
                    packet.WriteLine("Map ID: " + (map != 0 ? StoreGetters.GetName(StoreNameType.Map, map) : map + " (No map)"));
                    item.BagFamily = packet.ReadEnum<BagFamilyMask>("Bag Family", TypeCode.Int32);
                    item.TotemCategory = packet.ReadEnum<TotemCategory>("Totem Category", TypeCode.Int32);

                    item.ItemSocketColors = new ItemSocketColor[3];
                    for (var i = 0; i < 3; i++)
                        item.ItemSocketColors[i] = packet.ReadEnum<ItemSocketColor>("Socket Color", TypeCode.Int32, i);

                    item.SocketContent = new uint[3];
                    for (var i = 0; i < 3; i++)
                        item.SocketContent[i] = packet.ReadUInt32("Socket Item", i);

                    item.SocketBonus = packet.ReadInt32("Socket Bonus");
                    item.GemProperties = packet.ReadInt32("Gem Properties");
                    item.ArmorDamageModifier = packet.ReadSingle("Armor Damage Modifier");
                    item.Duration = packet.ReadUInt32("Duration");
                    item.ItemLimitCategory = packet.ReadInt32("Limit Category");
                    item.HolidayId = packet.ReadEnum<Holiday>("Holiday", TypeCode.Int32);
                    item.StatScalingFactor = packet.ReadSingle("Stat Scaling Factor");
                    item.CurrencySubstitutionId = packet.ReadUInt32("Currency Substitution Id");
                    item.CurrencySubstitutionCount = packet.ReadUInt32("Currency Substitution Count");

                    Storage.ObjectNames.Add(itemId, new ObjectName { ObjectType = ObjectType.Item, Name = item.Name }, packet.TimeSpan);
                    break;
                }
                case DB2Hash.KeyChain: // KeyChain
                {
                    packet.ReadUInt32("Key Chain Id");
                    packet.WriteLine("Key: {0}", Utilities.ByteArrayToHexString(packet.ReadBytes(32)));
                    break;
                }

                // Cases need correction, the other DB2's need implementation etc.
                default: break;
            }

            if (HashType == DB2Hash.Item || HashType == DB2Hash.Item_sparse) // Add item data.
                packet.AddSniffData(StoreNameType.Item, (int)itemId, "DB_REPLY");

            packet.ReadToEnd();
        }
    }
}
