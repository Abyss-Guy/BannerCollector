using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BannerCollector
{
    internal static partial class BannerLoad
    {
        // Diagnostic: curated banner names that did not resolve to an item via Mod.TryFind
        // during the last load (e.g. a texture-vs-class name mismatch). Surfaced by the
        // "/discoverbanners" command; does not affect loading.
        internal static readonly List<string> SkippedBanners = new List<string>();

        // Reverse map (banner item type -> engine banner number) built from the game's own banner
        // registry, used to resolve each banner's real buff index once at load. See
        // BuildBannerItemToBannerId; it is built in LoadBanners before this file's banners register.
        private static readonly Dictionary<int, int> BannerItemToBannerId = new Dictionary<int, int>();

        // Banner tile type -> the set of distinct banner numbers whose canonical item sits on that
        // tile. A tile with exactly one number is a single-enemy banner tile, so EVERY banner item
        // placed on it grants that enemy's buff - this resolves the extra "head/variant" items a mod
        // ships for one enemy (e.g. the three Hydra heads all give the Lernean Hydra buff) without
        // any hardcoding. Built together with BannerItemToBannerId.
        private static readonly Dictionary<int, HashSet<int>> TileToBannerIds = new Dictionary<int, HashSet<int>>();

        public static void LoadModBanners()
        {
            SkippedBanners.Clear();

            // Calamity banners are drawn from each item's own icon (UseItemIcon) instead of
            // the Calamity banner atlas, so banner sprites never desync when Calamity adds or
            // removes banners. The name list is matched against the loaded mod at runtime.
            #region CalamityMod
            AddItemIconBanners("CalamityMod", CalamityBanners, CalamityHardModeBanners, CalamityPostMoonLordBanners);
            #endregion

            // Calamity Fables is a Calamity add-on, so it is kept right next to Calamity.
            #region CalamityFables
            AddItemIconBanners("CalamityFables", CalamityFablesBanners);
            #endregion

            // Catalyst, like Calamity, is rendered from each item's own icon (UseItemIcon)
            // instead of the Catalyst banner atlas, so banner sprites stay correct if
            // Catalyst ever changes its banner set.
            #region CatalystMod
            AddItemIconBanners("CatalystMod", CatalystBanners, CatalystHardModeBanners);
            #endregion

            #region ThoriumMod
            AddItemIconBanners("ThoriumMod", ThoriumBanners, ThoriumHardModeBanners);
            #endregion

            #region SpiritMod
            AddItemIconBanners("SpiritMod", SpiritBanners, SpiritHardModeBanners);
            #endregion

            #region SpiritReforged
            AddItemIconBanners("SpiritReforged", SpiritReforgedBanners, SpiritReforgedHardModeBanners);
            #endregion

            #region Consolaria
            AddItemIconBanners("Consolaria", ConsolariaBanners, ConsolariaHardModeBanners);
            #endregion

            #region VitalityMod
            AddItemIconBanners("VitalityMod", VitalityBanners, VitalityHardModeBanners);
            #endregion

            #region ContinentOfJourney
            // Hardmode set is every banner except the post-Moon-Lord ones, so the tiers stay exclusive.
            AddItemIconBanners("ContinentOfJourney", ContinentOfJourneyBanners,
                new HashSet<string>(ContinentOfJourneyBanners.Except(ContinentOfJourneyPostMoonLordBanners)),
                ContinentOfJourneyPostMoonLordBanners);
            #endregion

            #region Split
            AddItemIconBanners("Split", SplitBanners, SplitHardModeBanners);
            #endregion

            #region ElementsAwoken
            AddItemIconBanners("ElementsAwoken", ElementsAwokenBanners, ElementsAwokenHardModeBanners, ElementsAwokenPostMoonLordBanners);
            #endregion

            #region Redemption
            AddItemIconBanners("Redemption", RedemptionBanners, RedemptionHardModeBanners);
            #endregion

            #region SOTS
            AddItemIconBanners("SOTS", SOTSBanners, SOTSHardModeBanners);
            #endregion
        }

        /// <summary>
        /// Registers a mod's banners so the UI draws each one from its own item icon
        /// (<see cref="BannerInfo.UseItemIcon"/>) rather than from a packed banner atlas.
        /// This means no <see cref="BannerInfo.Index"/> and no mod banner atlas texture are
        /// needed, and banner sprites can never desync when a mod adds, removes or reorders
        /// its banners. Used by Calamity, Catalyst, Thorium, Spirit and Spirit Reforged.
        /// </summary>
        /// <param name="modName">Internal name of the source mod.</param>
        /// <param name="bannerNames">Internal item names of the banners to register.</param>
        /// <param name="hardModeNames">
        /// Optional set of banner names that drop from hardmode enemies, used for the
        /// hardmode/pre-hardmode filter. Names not in the set (or when null) are treated
        /// as pre-hardmode.
        /// </param>
        /// <param name="postMoonLordNames">
        /// Optional set of post-Moon-Lord banner names, kept out of <paramref name="hardModeNames"/>
        /// so the progression tiers stay mutually exclusive.
        /// </param>
        private static void AddItemIconBanners(string modName, string[] bannerNames, HashSet<string> hardModeNames = null, HashSet<string> postMoonLordNames = null)
        {
            if (!ModList.Contains(modName))
                return;

            Mod mod = ModLoader.GetMod(modName);
            foreach (string bannerName in bannerNames)
            {
                if (!mod.TryFind(bannerName, out ModItem item))
                {
                    SkippedBanners.Add($"{modName}/{bannerName}");
                    continue;
                }

                // Read the banner's tile + style from the fully-initialized item sample so the
                // UI can draw the real banner tile (authentic 16x48 look) with no hardcoded
                // atlas indices. createTile is -1 for the rare banner with no placeable tile,
                // in which case the UI falls back to the item icon.
                Item sample = ContentSamples.ItemsByType[item.Type];

                // Resolve the banner's real buff number once, here at load, so granting the
                // nearby-banner buff later needs no per-frame lookup. Prefer the game's own banner
                // registry (authoritative); fall back to the NPC matched by name and its own banner
                // number for banners whose item is a non-canonical sibling. -1 means no buff.
                int bannerId = ResolveBannerNumber(item.Type);
                if (bannerId < 0
                    && mod.TryFind(GetBannerNpcName(item.Name), out ModNPC fallbackNpc)
                    && ContentSamples.NpcsByNetId.TryGetValue(fallbackNpc.Type, out NPC fallbackSample))
                {
                    int b = Item.NPCtoBanner(fallbackSample.BannerID());
                    if (b > 0)
                        bannerId = b;
                }

                if (bannerId < 0)
                    SkippedBanners.Add($"{modName}/{bannerName} [NPC not resolved - banner gives no buff]");

                BannerDict[item.Type] = new BannerInfo
                {
                    ItemId = item.Type,
                    ItemName = item.Name,
                    BannerCount = 0,
                    IsHardMode = hardModeNames != null && hardModeNames.Contains(bannerName),
                    IsPostMoonLord = postMoonLordNames != null && postMoonLordNames.Contains(bannerName),
                    ModName = modName,
                    UseItemIcon = true,
                    TileType = sample.createTile,
                    Index = sample.placeStyle,
                    BannerId = bannerId,
                };
            }
        }

        /// <summary>
        /// Builds <see cref="BannerItemToBannerId"/> (banner item type -> engine banner number)
        /// from the game's own banner registry, i.e. the exact chain the engine uses when an enemy
        /// drops its banner: NPC -> <see cref="Item.NPCtoBanner"/> -> <see cref="Item.BannerToItem"/>.
        /// The stored value is the banner number itself - the authoritative index into
        /// <see cref="Terraria.SceneMetrics.NPCBannerBuff"/> - so the buff is always granted for the
        /// banner's real enemy, for vanilla and any mod alike. Every entry satisfies
        /// <c>BannerToItem(value) == key</c> by construction, so a resolved banner is provably correct.
        /// Called once from LoadBanners (unconditionally, so vanilla works with no mods loaded).
        /// </summary>
        private static void BuildBannerItemToBannerId()
        {
            BannerItemToBannerId.Clear();
            TileToBannerIds.Clear();
            // Iterate every net id, not just 0..NPCCount: many vanilla enemies (the coloured
            // slimes, Pinky, etc.) are negative-net-id variants of a base type and would be
            // skipped by a type-range walk, losing their banner entirely.
            foreach (NPC npc in ContentSamples.NpcsByNetId.Values)
            {
                int banner = Item.NPCtoBanner(npc.BannerID());
                if (banner <= 0)
                    continue;

                int itemType = Item.BannerToItem(banner);
                if (itemType <= 0)
                    continue;

                if (!BannerItemToBannerId.ContainsKey(itemType))
                    BannerItemToBannerId[itemType] = banner;

                // Record which banner tile this banner's canonical item sits on, so a banner item
                // that is not itself in the registry can still be resolved when its tile is dedicated
                // to a single enemy.
                if (ContentSamples.ItemsByType.TryGetValue(itemType, out Item itemSample) && itemSample.createTile >= 0)
                {
                    if (!TileToBannerIds.TryGetValue(itemSample.createTile, out HashSet<int> ids))
                        TileToBannerIds[itemSample.createTile] = ids = new HashSet<int>();
                    ids.Add(banner);
                }
            }
        }

        /// <summary>
        /// Resolves a banner item to its engine banner number (the index written to
        /// <see cref="Terraria.SceneMetrics.NPCBannerBuff"/>). Prefers the authoritative
        /// registry; if the item is not itself a registered banner, it falls back to the banner of
        /// its tile when that tile is dedicated to a single enemy (so the extra head/variant items a
        /// mod ships for one enemy still grant that enemy's buff). Returns -1 if unresolved.
        /// </summary>
        private static int ResolveBannerNumber(int itemType)
        {
            if (BannerItemToBannerId.TryGetValue(itemType, out int banner))
                return banner;

            if (ContentSamples.ItemsByType.TryGetValue(itemType, out Item sample) && sample.createTile >= 0
                && TileToBannerIds.TryGetValue(sample.createTile, out HashSet<int> ids) && ids.Count == 1)
                return ids.First();

            return -1;
        }

        /// <summary>
        /// Derives the likely NPC internal name from a banner item's internal name by removing
        /// the trailing "BannerItem" (Spirit Reforged) or "Banner" suffix. Used only as a
        /// fallback when the game's banner registry (<see cref="BannerItemToBannerId"/>)
        /// does not cover a banner.
        /// </summary>
        private static string GetBannerNpcName(string itemName)
        {
            if (itemName.EndsWith("BannerItem"))
                return itemName.Substring(0, itemName.Length - "BannerItem".Length);
            if (itemName.EndsWith("Banner"))
                return itemName.Substring(0, itemName.Length - "Banner".Length);
            return itemName;
        }

        // Internal banner item names extracted from each mod's assets
        // (Items/Banners/*Banner for Thorium & Spirit, *BannerItem for Spirit Reforged).
        // The list is matched against the loaded mod at runtime via Mod.TryFind, so any
        // name missing from a given mod version is simply skipped.

        #region Calamity Banners
        private static readonly string[] CalamityBanners =
        {
            "AcidEelBanner", "AeroSlimeBanner", "AmberCrawlerBanner", "AmethystCrawlerBanner",
            "AndroombaBanner", "AnthozoanCrabBanner", "AquaticUrchinBanner", "AriesBanner",
            "AstraglomerateBanner", "AstralachneaBanner", "AstralProbeBanner", "AstralSlimeBanner",
            "AtlasBanner", "AuroraSpiritBanner", "BabyCannonballJellyfishBanner", "BabyFlakCrabBanner",
            "BabyGhostBellBanner", "BelchingCoralBanner", "BlindedAnglerBanner", "BloatfishBanner",
            "BloomSlimeBanner", "BobbitWormBanner", "BohldohrBanner", "BoxJellyfishBanner",
            "BurrowerBanner", "CalamityEyeBanner", "CannonballJellyfishBanner", "ChaoticPufferBanner",
            "CladCrabBanner", "ClamBanner", "CloudElementalBanner", "CnidrionBanner",
            "ColossalSquidBanner", "CrimulanBlightSlimeBanner", "CryonBanner", "CryoSlimeBanner",
            "CrystalCrawlerBanner", "CuttlefishBanner", "DespairStoneBanner", "DevilFishBanner",
            "DiamondCrawlerBanner", "DraconicSwarmerBanner", "EarthElementalBanner", "EbonianBlightSlimeBanner",
            "EidolistBanner", "EidolonWyrmJuvenileBanner", "EmeraldCrawlerBanner", "EutrophicRayBanner",
            "FearlessGoldfishWarriorBanner", "FlakCrabBanner", "FrogfishBanner", "FusionFeederBanner",
            "GammaSlimeBanner", "GhostBellBanner", "GiantSquidBanner", "GnasherBanner",
            "GulperEelBanner", "HadarianBanner", "HeatSpiritBanner", "HorribleHogBanner", "IceClasperBanner",
            "ImpiousImmolatorBanner", "InfernalCongealmentBanner", "IrradiatedSlimeBanner", "LaserfishBanner",
            "LuminousCorvinaBanner", "MantisBanner", "MantisShrimpBanner", "MelterBanner",
            "MirageJellyBanner", "MorayEelBanner", "NovaBanner", "NuclearToadBanner",
            "OarfishBanner", "OrthoceraBanner", "OverloadedSoldierBanner", "PerennialSlimeBanner",
            "PestilentSlimeBanner", "PhantomSpiritBanner", "PiggyBanner", "PlaguebringerBanner",
            "PlagueChargerBanner", "PlagueshellBanner", "PrismBackBanner", "ProfanedEnergyBanner",
            "RadiatorBanner", "ReaperSharkBanner", "RenegadeWarlockBanner", "RepairUnitBanner",
            "RimehoundBanner", "RotdogBanner", "RubyCrawlerBanner", "SapphireCrawlerBanner",
            "ScornEaterBanner", "ScryllarBanner", "SeaFloatyBanner", "SeaMinnowBanner",
            "SeaSerpentBanner", "ShockstormShuttleBanner", "ShroombleBanner", "SightseerColliderBanner",
            "SightseerSpitterBanner", "SkyfinBanner", "SlabCrabBanner", "SoulSlurperBanner",
            "StellarCulexBanner", "StormlionBanner", "SulflounderBanner", "SulphurousSkaterBanner",
            "SunskaterBanner", "TopazCrawlerBanner", "ToxicatfishBanner", "ToxicMinnowBanner",
            "TrasherBanner", "TrilobiteBanner", "ViperfishBanner", "VirulingBanner",
            "WulfrumAmplifierBanner", "WulfrumDroneBanner", "WulfrumGyratorBanner", "WulfrumHovercraftBanner",
            "WulfrumRoverBanner",
        };

        // Calamity banners that drop from hardmode enemies (preserved from the original
        // hand-maintained data) so the hardmode/pre-hardmode filter keeps working.
        private static readonly HashSet<string> CalamityHardModeBanners = new HashSet<string>
        {
            "AnthozoanCrabBanner", "AriesBanner", "AstralProbeBanner", "AstralSlimeBanner",
            "AstralachneaBanner", "AtlasBanner", "BelchingCoralBanner", "BlindedAnglerBanner",
            "BohldohrBanner", "ChaoticPufferBanner", "CloudElementalBanner", "CryoSlimeBanner",
            "CryonBanner", "DevilFishBanner", "EarthElementalBanner", "EidolistBanner",
            "FlakCrabBanner", "FusionFeederBanner", "GiantSquidBanner", "GulperEelBanner",
            "HadarianBanner", "IceClasperBanner", "InfernalCongealmentBanner", "IrradiatedSlimeBanner",
            "MantisBanner", "MantisShrimpBanner", "MelterBanner", "MirageJellyBanner",
            "NovaBanner", "OrthoceraBanner", "OverloadedSoldierBanner", "PerennialSlimeBanner",
            "PestilentSlimeBanner", "PlagueChargerBanner", "PlaguebringerBanner", "PlagueshellBanner",
            "RenegadeWarlockBanner", "SeaSerpentBanner", "ShockstormShuttleBanner", "SightseerColliderBanner",
            "SightseerSpitterBanner", "StellarCulexBanner", "SulphurousSkaterBanner", "TrilobiteBanner",
            "VirulingBanner",
            // Added in Calamity 2.1.2 (Burrower, CladCrab and Shromble are pre-hardmode)
            "AstraglomerateBanner", "AuroraSpiritBanner",
        };

        // Calamity banners from post-Moon-Lord enemies.
        private static readonly HashSet<string> CalamityPostMoonLordBanners = new HashSet<string>
        {
            "BloatfishBanner", "BloomSlimeBanner", "BobbitWormBanner", "ColossalSquidBanner",
            "DraconicSwarmerBanner", "EidolonWyrmJuvenileBanner", "GammaSlimeBanner", "ImpiousImmolatorBanner",
            "PhantomSpiritBanner", "ProfanedEnergyBanner", "ReaperSharkBanner", "ScornEaterBanner",
        };

        #endregion

        #region Calamity Fables Banners
        // All banners are pre-hardmode (no hardmode banner enemies).
        private static readonly string[] CalamityFablesBanners =
        {
            "BlizzardSpriteBannerItem", "CloudSpriteBannerItem", "GeodeCrawlerBannerItem", "SandstormSpriteBannerItem",
            "SpelunkerScarabBannerItem", "StormlionBannerItem", "StormlionLarvaBannerItem", "WulfrumGrapplerBannerItem",
            "WulfrumMagnetizerBannerItem", "WulfrumMortarBannerItem", "WulfrumNexusBannerItem", "WulfrumRollerBannerItem",
            "WulfrumRoverBannerItem",
        };

        #endregion

        #region Catalyst Banners
        private static readonly string[] CatalystBanners =
        {
            "AscendedAstralSlimeBanner", "MetanovaSlimeBanner", "WulfrumMineBanner", "WulfrumSlimeBanner",
        };

        // Catalyst banners that drop from hardmode enemies (preserved from the original
        // hand-maintained data) so the hardmode/pre-hardmode filter keeps working.
        private static readonly HashSet<string> CatalystHardModeBanners = new HashSet<string>
        {
            "AscendedAstralSlimeBanner", "MetanovaSlimeBanner",
        };

        #endregion

        #region Thorium Banners
        private static readonly string[] ThoriumBanners =
        {
            "AbominationBanner", "AbyssalAnglerBanner", "AncientArcherBanner", "AncientChargerBanner",
            "AncientPhalanxBanner", "ArmyAntBanner", "AstroBeetleBanner", "BarracudaBanner", "BiterBanner",
            "BlackWidowBanner", "BlisterPodBanner", "BlizzardBatBanner", "BlobfishBanner", "BloodMageBanner",
            "BloodyWargBanner", "BlowfishBanner", "BoneFlayerBanner", "BrownRecluseBanner", "ChilledSpitterBanner",
            "CoinBagBanner", "ColdlingBanner", "CoolmeraBanner", "CrownofThornsBanner", "DarksteelKnightBanner",
            "DissonanceSeerBanner", "EarthenBatBanner", "EarthenGolemBanner", "EngorgedEyeBanner", "EpiDermonBanner",
            "FeedingFrenzyBanner", "FlamekinCasterBanner", "FreezerBanner", "FrostBurntBanner", "FrostFangBanner",
            "FrostWurmBanner", "FrozenFaceBanner", "FrozenGrossBanner", "GelatinousCubeBanner", "GigaClamBanner",
            "GlitteringGolemBanner", "GoblinDrummerBanner", "GoblinSpiritGuideBanner", "GoblinTrapperBanner",
            "GoldBatBanner", "GoldLycanBanner", "GoldSlimeBanner", "GraniteEradicatorBanner", "GraniteFusedSlimeBanner",
            "GraniteSurgerBanner", "GraveLimbBanner", "HallucinationBanner", "HammerHeadBanner", "HellBringerMimicBanner",
            "HoppingSpiderBanner", "HorrificChargerBanner", "InfernalHoundBanner", "KrakenBanner", "LeFantomeBanner",
            "LifeCrystalMimicBanner", "LihzardMimicBanner", "LihzardPotMimicBanner", "LivingHemorrhageBanner",
            "LycanBanner", "MahoganyEntBanner", "ManofWarBanner", "MartianScoutBanner", "MartianSentryBanner",
            "MoltenMortarBanner", "MorayBanner", "MossWaspBanner", "MudManBanner", "MyceliumMimicBanner",
            "NecroPotBanner", "NestlingBanner", "OctopusBanner", "PutridSerpentBanner", "RagingMinotaurBanner",
            "ScissorStalkerBanner", "SeaShantySingerBanner", "ShamblerBanner", "SharptoothBanner", "SmotheringShadeBanner",
            "SnowballBanner", "SnowEaterBanner", "SnowElementalBanner", "SnowFlinxMatriarchBanner", "SnowSingaBanner",
            "SnowyOwlBanner", "SoulCorrupterBanner", "SpaceSlimeBanner", "SpectrumiteBanner", "StrangeBulbBanner",
            "SubmergedMimicBanner", "SunPriestessBanner", "TarantulaBanner", "TheInnocentBanner", "TheStarvedBanner",
            "UFOBanner", "UnderworldPotBanner", "VampireSquidBanner", "VileFloaterBanner", "VoltEelBanner",
            "WindElementalBanner",
        };

        // Thorium banners that drop from hardmode enemies (verified against
        // thoriummod.wiki.gg/wiki/Enemy_list). Everything else is pre-hardmode.
        private static readonly HashSet<string> ThoriumHardModeBanners = new HashSet<string>
        {
            "AbyssalAnglerBanner", "AstroBeetleBanner", "BlackWidowBanner", "BlisterPodBanner",
            "BlizzardBatBanner", "BlobfishBanner", "BloodMageBanner", "BloodyWargBanner",
            "BoneFlayerBanner", "BrownRecluseBanner", "ChilledSpitterBanner", "CoinBagBanner",
            "ColdlingBanner", "CrownofThornsBanner", "DissonanceSeerBanner", "EpiDermonBanner",
            "FeedingFrenzyBanner", "FreezerBanner", "FrostBurntBanner", "FrostFangBanner",
            "FrozenGrossBanner", "GlitteringGolemBanner", "GoblinSpiritGuideBanner", "HallucinationBanner",
            "HellBringerMimicBanner", "HorrificChargerBanner", "InfernalHoundBanner", "KrakenBanner",
            "LeFantomeBanner", "LihzardMimicBanner", "LihzardPotMimicBanner", "LycanBanner",
            "MartianScoutBanner", "MartianSentryBanner", "MoltenMortarBanner", "MossWaspBanner",
            "MyceliumMimicBanner", "NecroPotBanner", "PutridSerpentBanner", "ScissorStalkerBanner",
            "SeaShantySingerBanner", "SmotheringShadeBanner", "SnowElementalBanner", "SnowFlinxMatriarchBanner",
            "SnowSingaBanner", "SnowyOwlBanner", "SoulCorrupterBanner", "SpectrumiteBanner",
            "SubmergedMimicBanner", "SunPriestessBanner", "TarantulaBanner", "TheStarvedBanner",
            "UnderworldPotBanner", "VampireSquidBanner", "VileFloaterBanner", "VoltEelBanner",
        };

        #endregion

        #region Spirit Classic Banners
        private static readonly string[] SpiritBanners =
        {
            "AlienBanner", "AncientApostleBanner", "AncientSpectreBanner", "AntlionAssassinBanner", "ArachmatonBanner",
            "ArterialGrasperBanner", "AstralAdventurerBanner", "AstralAmalgamBanner", "BeholderBanner",
            "BlazingSkullBanner", "BlizzardBanditBanner", "BlizzardNimbusBanner", "BloaterBanner", "BloatfishBanner",
            "BloomshroomBanner", "BlossomHoundBanner", "BlueDungeonCubeBanner", "BottomFeederBanner",
            "BoulderBehemothBanner", "BriarthornSlimeBanner", "BubbleBruteBanner", "CavernBanditBanner",
            "CavernCrawlerBanner", "ChestZombieBanner", "CoconutSlimeBanner", "CracklingCoreBanner", "CrocosaurBanner",
            "CrystalDrifterBanner", "CystalBanner", "DarkAlchemistBanner", "DeadeyeMarksmanBanner", "DiseasedSlimeBanner",
            "DraugrBanner", "ElectricEelBanner", "FallenAngelBanner", "FallingAsteroidBanner", "FesterflyBanner",
            "FleshHoundBanner", "FurnaceMawBanner", "GhastBanner", "GiantJellyBanner", "GladeWraithBanner",
            "GladiatorSpiritBanner", "GlitterflyBanner", "GloopBanner", "GlowToadBanner", "GluttonousDevourerBanner",
            "GoblinGrenadierBanner", "GoldCrateMimicBanner", "GranitecTurretBanner", "GraniteSlimeBanner",
            "GreenDungeonCubeBanner", "HauntedTomeBanner", "HemaphoraBanner", "HydraGreenBanner",
            "HydraPurpleBanner", "HydraRedBanner", "IronCrateMimicBanner",
            "KakamoraBanner", "KakamoraBruteBanner", "KakamoraGliderBanner", "KakamoraShamanBanner",
            "KakamoraShielderBanner", "KakamoraShielderBanner1", "KakamoraThrowerBanner", "LostMimeBanner",
            "LumantisBanner", "LunarSlimeBanner", "MadHatterBanner", "MangoWarBanner", "MangroveDefenderBanner",
            "MasticatorBanner", "MechromancerBanner", "MoltenCoreBanner", "MoonlightPreserverBanner",
            "MoonlightRupturerBanner", "MyceliumBotanistBanner", "NetherbaneBanner", "OccultistBanner", "OrbititeBanner",
            "PhantomBanner", "PhantomSamuraiBanner", "PinkDungeonCubeBanner", "PirateLobberBanner", "PokeyBanner",
            "PutromaBanner", "ReachmanBanner", "RlyehianBanner", "ScreechOwlBanner", "ShockhopperBanner",
            "SkeletonBruteBanner", "SoulCrusherBanner", "SpiritBatBanner", "SpiritFloaterBanner", "SpiritGhoulBanner",
            "SpiritMummyBanner", "SpiritSkullBanner", "SpiritTomeBanner", "SporeWheezerBanner", "StardancerBanner",
            "StymphalianBatBanner", "ThornStalkerBanner", "TrochmatonBanner", "ValkyrieBanner", "WanderingSoulBanner",
            "WheezerBanner", "WildwoodWatcherBanner", "WinterbornBanner", "WinterbornHeraldBanner", "WoodCrateMimicBanner",
            "YureiBanner",
        };

        // Spirit banners that drop from hardmode enemies (verified against
        // spiritmod.wiki.gg/wiki/Enemies). The Spirit-biome enemies Shadow Ghoul, Dusk Mummy
        // and Ancient Tome use the internal names SpiritGhoul/SpiritMummy/SpiritTome.
        private static readonly HashSet<string> SpiritHardModeBanners = new HashSet<string>
        {
            "AncientSpectreBanner", "ArachmatonBanner", "BoulderBehemothBanner", "FallenAngelBanner",
            "GranitecTurretBanner", "HydraGreenBanner", "HydraPurpleBanner", "HydraRedBanner",
            "MangroveDefenderBanner", "NetherbaneBanner", "PhantomBanner", "SoulCrusherBanner",
            "SpiritBatBanner", "SpiritFloaterBanner", "SpiritGhoulBanner", "SpiritMummyBanner",
            "SpiritSkullBanner", "SpiritTomeBanner", "StymphalianBatBanner", "TrochmatonBanner",
            "WanderingSoulBanner",
        };

        #endregion

        #region Spirit Reforged Banners
        private static readonly string[] SpiritReforgedBanners =
        {
            "CorruptStactusBannerItem", "CrimsonStactusBannerItem", "DecrepitMummyBannerItem",
            "DesertStactusBannerItem", "DunceCrabBannerItem", "GraverobberBannerItem", "HallowedStactusBannerItem",
            "HyenaBannerItem", "MossSlimeBannerItem", "OceanSlimeBannerItem", "OstrichBannerItem",
            "PeevedTumblerBannerItem", "WheezerBannerItem", "WispBannerItem",
        };

        // Spirit Reforged banners that drop from hardmode enemies (verified against
        // spiritmod.wiki.gg/wiki/Spirit_Reforged/Enemies). Only Hallowed Stactus is hardmode.
        private static readonly HashSet<string> SpiritReforgedHardModeBanners = new HashSet<string>
        {
            "HallowedStactusBannerItem",
        };

        #endregion

        #region Consolaria Banners
        private static readonly string[] ConsolariaBanners =
        {
            "AlbinoAntlionBanner", "AlbinoChargerBanner", "AlbinoSwarmerBanner", "ArchDemonBanner",
            "ArchWyvernBanner", "DisasterBunnyBanner", "DragonHornetBanner", "DragonSkullBanner",
            "DragonSnatcherBanner", "FleshAxeBanner", "FleshMummyBanner", "FleshSlimeBanner",
            "MythicalWyvernBanner", "OrcaBanner", "ShadowHammerBanner", "ShadowMummyBanner",
            "ShadowSlimeBanner", "SpectralElementalBanner", "SpectralGastropodBanner", "SpectralMummyBanner",
            "VampireMinerBanner",
        };

        // Consolaria banners that drop from hardmode enemies (verified against
        // terrariamods.fandom.com/wiki/Consolaria/Enemies).
        private static readonly HashSet<string> ConsolariaHardModeBanners = new HashSet<string>
        {
            "ArchWyvernBanner", "FleshAxeBanner", "FleshMummyBanner", "FleshSlimeBanner",
            "MythicalWyvernBanner", "ShadowHammerBanner", "ShadowMummyBanner", "ShadowSlimeBanner",
            "SpectralElementalBanner", "SpectralGastropodBanner", "SpectralMummyBanner",
        };

        #endregion

        #region Vitality Banners
        private static readonly string[] VitalityBanners =
        {
            "AmberSlimeBanner", "AmethystSlimeBanner", "AquaticMermanBanner", "BigEaterBanner",
            "BoneDemonBanner", "ChickenBanner", "CrystalBasiliskBanner", "DecayingBatBanner",
            "DiamondSlimeBanner", "DynamiteSnowmanBanner", "EmeraldSlimeBanner", "ForestSpiritBanner",
            "FungalTortoiseBanner", "GladiatorBanner", "HeatwaveZombieBanner", "IceChimeBanner",
            "LeechingBatBanner", "MagmaSharkBanner", "MossMothBanner", "MossSkeletonBanner",
            "PufferfishBanner", "RattlingArcherBanner", "RottenAntlionBanner", "RubySlimeBanner",
            "SapphireSlimeBanner", "ScubaZombieBanner", "SheddedAntlionBanner", "StrangeSaucerBanner",
            "TaintedBasiliskBanner", "TopazSlimeBanner", "UmbrellaZombieBanner", "VileBasiliskBanner",
        };

        // Vitality banners that drop from hardmode enemies (verified against
        // terrariamods.fandom.com/wiki/Vitality_Mod/Enemies).
        private static readonly HashSet<string> VitalityHardModeBanners = new HashSet<string>
        {
            "BoneDemonBanner", "CrystalBasiliskBanner", "DynamiteSnowmanBanner", "FungalTortoiseBanner",
            "IceChimeBanner", "MagmaSharkBanner", "StrangeSaucerBanner", "TaintedBasiliskBanner",
            "VileBasiliskBanner",
        };

        #endregion

        #region Homeward Journey Banners
        // Every banner enemy is hardmode.
        private static readonly string[] ContinentOfJourneyBanners =
        {
            "AbsoluteZeroBanner", "AbyssSkullBanner", "ChewingThingBanner",
            "CursedLingererBanner", "DeepWeedBanner", "DesertMimicBanner", "DrillerBanner",
            "DrownedBanner", "EnchantedVineBanner",
            "EyeholeBanner", "FluorescentAnglerBanner", "FluorescentMelonBanner", "FluorescentSlimeBanner",
            "FlyingCoffinBanner", "ForbiddenJellyBanner", "ForgottenWarhammerBanner", "GoldenEyeBanner",
            "GoldenShadowBanner", "ImpMageBanner", "KeyStandBanner", "LinkPustuleBanner",
            "LivingNestBanner", "LostVikingsBanner", "MagmackerelBanner", "MagmaFlowerBanner",
            "MeatPotBanner", "MicrorganBanner", "MissleTurtleBanner", "MonarchButterflyBanner",
            "PharaohsHandBanner", "PolarMimicBanner", "PrehistoricVirusBanner", "PrototypeSlimeBanner",
            "PurpleShadowBanner", "RoseGardenBanner", "ScarletShadowBanner", "ScreenDoorZombieBanner",
            "ShadowArmorBanner", "ShadowChomperBanner", "ShadowDiggerBanner", "ShadowEaterBanner",
            "ShadowEyeBanner", "ShadowOozeBanner", "ShadowSlimeBanner", "ShadowZombieBanner",
            "ShyGhostBanner", "SlimySpiritBanner", "SolenopsisBanner", "SoulstareBanner",
            "SunlightDiscipleBanner", "TempleMimicBanner", "ToothyBanner", "ValkyrieBanner",
            "VoidWeaverBanner", "WardenEyeBanner", "WhiteCultistBanner", "WindElementalBanner",
        };

        // Homeward Journey banners from post-Moon-Lord enemies.
        private static readonly HashSet<string> ContinentOfJourneyPostMoonLordBanners = new HashSet<string>
        {
            "ChewingThingBanner", "CursedLingererBanner", "DesertMimicBanner", "DrillerBanner",
            "EyeholeBanner", "FlyingCoffinBanner", "ForbiddenJellyBanner", "GoldenEyeBanner",
            "ImpMageBanner", "LinkPustuleBanner", "LivingNestBanner", "LostVikingsBanner",
            "MagmaFlowerBanner", "MagmackerelBanner", "MeatPotBanner", "MicrorganBanner",
            "MissleTurtleBanner", "MonarchButterflyBanner", "PharaohsHandBanner", "PolarMimicBanner",
            "PrehistoricVirusBanner", "PrototypeSlimeBanner", "RoseGardenBanner", "ScreenDoorZombieBanner",
            "ShyGhostBanner", "SoulstareBanner", "TempleMimicBanner", "ToothyBanner",
            "ValkyrieBanner", "WindElementalBanner", "EnchantedVineBanner", "AbsoluteZeroBanner",
            "SlimySpiritBanner", "SolenopsisBanner", "WhiteCultistBanner", "SunlightDiscipleBanner",
        };

        #endregion

        #region Split Banners
        private static readonly string[] SplitBanners =
        {
            "CaveImpBanner", "ColossusBanner", "CombusterBanner", "CrabcannonBanner", "DarknutBanner",
            "EchoBanner", "FacelessPirateBanner", "FairyflyBanner", "FootballZombieBanner", "FortressBanner",
            "FunkySoulBanner", "GarghoulBanner", "GlassButterflyBanner", "GoldenDroneBanner", "GreatToxicSludgeBanner",
            "HauntedAnchorBanner", "HeadlessPirateBanner", "HobgoblinBanner", "HuskBanner", "IdlerBanner",
            "JumpkinBanner", "KnockerBanner", "LatopusBanner", "MagnetoBanner", "MindFlayerBanner",
            "MoonwalkerBanner", "MuskeletonBanner", "PossessedFrostArmorBanner", "PossessedShadowArmorBanner",
            "PurpleDroneBanner", "RebelliousWispBanner", "SarraceniaBanner", "SavageBanner", "SentinelBanner",
            "ShadowCasterBanner", "SheepNPCBanner", "ShinyPixieBanner", "SkeletonJesterBanner",
            "SpikedLavaSlimeBanner", "ThreaterBanner", "ThrillerBanner", "TrollBanner", "WitnessBanner",
        };

        private static readonly HashSet<string> SplitHardModeBanners = new HashSet<string>
        {
            "DarknutBanner", "GreatToxicSludgeBanner", "HauntedAnchorBanner", "IdlerBanner", "LatopusBanner",
            "MoonwalkerBanner", "MuskeletonBanner", "SavageBanner", "ShinyPixieBanner", "SkeletonJesterBanner",
            "ThrillerBanner",
        };

        #endregion

        #region Elements Awoken Banners
        private static readonly string[] ElementsAwokenBanners =
        {
            "DesertElementalBanner", "DragonBatBanner", "DragonSlimeBanner", "DragonWarriorBanner",
            "DrakoniteElementalBanner", "EtherealHunterBanner", "FireElementalBanner", "FlyingJawBanner",
            "FrostElementalBanner", "GiantTickBanner", "GiantVampireBatBanner", "ImmolatorBanner",
            "InfernoSpiritBanner", "MortemWalkerBanner", "PebleerBanner", "PetalClasperBanner",
            "ReaverSlimeBanner", "SkyCrawlerBanner", "SkyElementalBanner", "StellarBatBanner",
            "StellarEntityBanner", "VampireBatBanner", "VoidCrawlerBanner", "VoidElementalBanner",
            "VoidGolemBanner", "VoidKnightBanner", "WaterElementalBanner", "ZergCasterBanner",
        };

        private static readonly HashSet<string> ElementsAwokenHardModeBanners = new HashSet<string>
        {
            "FrostElementalBanner", "GiantVampireBatBanner", "SkyElementalBanner", "WaterElementalBanner",
        };

        // Elements Awoken banners from post-Moon-Lord enemies.
        private static readonly HashSet<string> ElementsAwokenPostMoonLordBanners = new HashSet<string>
        {
            "EtherealHunterBanner", "GiantTickBanner", "ImmolatorBanner", "InfernoSpiritBanner",
            "MortemWalkerBanner", "ReaverSlimeBanner", "SkyCrawlerBanner", "StellarBatBanner",
            "StellarEntityBanner", "VoidCrawlerBanner", "VoidElementalBanner", "VoidGolemBanner",
            "VoidKnightBanner", "ZergCasterBanner",
        };

        #endregion

        #region Redemption Banners
        private static readonly string[] RedemptionBanners =
        {
            "AncientGladestoneGolemBanner", "AndroidBanner", "BlisteredScientistBanner", "BloatedClingerBanner",
            "BloatedDiggerBanner", "BloatedFaceMonsterBanner", "BloatedGhoulBanner", "BloatedGoldfishBanner",
            "BloatedScientistBanner", "BloatedSwarmerBanner", "BlobbleBanner", "BobTheBlobBanner",
            "BoneSpiderBanner", "ChickenBanner", "ChickenBomberBanner", "ChickenScratcherBanner",
            "CoastScarabBanner", "CorpseWalkerPriestBanner", "CorruptChickenBanner", "DevilsTongueBanner",
            "EpidotrianSkeletonBanner", "ForestNymphBanner", "ForretBanner", "GrandLarvaBanner",
            "HaymakerBanner", "HazmatZombieBanner", "HeadlessChickenBanner", "JollyMadmanBanner",
            "KabucraBanner", "LivingBloomBanner", "MoonflareBatBanner", "MoonflareSkeletonBanner",
            "MutatedLivingBloomBanner", "NuclearShadowBanner", "NuclearSlimeBanner", "OozeBlobBanner",
            "OozingScientistBanner", "PrototypeSilverBanner", "RadioactiveJellyBanner", "RadioactiveSlimeBanner",
            "RoosterBoosterBanner", "SandskinSpiderBanner", "SickenedBunnyBanner", "SickenedDemonEyeBanner",
            "SicklyPenguinBanner", "SicklyWolfBanner", "SkeletonAssassinBanner", "SkeletonDuelistBanner",
            "SkeletonFlagbearerBanner", "SkeletonNobleBanner", "SkeletonWandererBanner", "SkeletonWardenBanner",
            "SneezyFlinxBanner", "SpacePaladinBanner", "TreeBugBanner", "VagrantSpiritBanner",
            "ViciousChickenBanner",
        };

        private static readonly HashSet<string> RedemptionHardModeBanners = new HashSet<string>
        {
            "AndroidBanner", "BlisteredScientistBanner", "BloatedClingerBanner", "BloatedDiggerBanner",
            "BloatedFaceMonsterBanner", "BloatedGhoulBanner", "BloatedGoldfishBanner", "BloatedScientistBanner",
            "BloatedSwarmerBanner", "BobTheBlobBanner", "HazmatZombieBanner", "MutatedLivingBloomBanner",
            "NuclearSlimeBanner", "OozeBlobBanner", "OozingScientistBanner", "PrototypeSilverBanner",
            "RadioactiveJellyBanner", "RadioactiveSlimeBanner", "SickenedBunnyBanner", "SickenedDemonEyeBanner",
            "SicklyPenguinBanner", "SicklyWolfBanner", "SneezyFlinxBanner", "SpacePaladinBanner",
        };

        #endregion

        #region Secrets Of The Shadows Banners
        private static readonly string[] SOTSBanners =
        {
            "ArcticGoblinBanner", "BallOGutsBanner", "BallOWormsBanner", "BigPhantarayBanner",
            "BleedingGhastBanner", "BlueSlimerBanner", "ChimeraBanner", "CoalCartBanner",
            "CorpsebloomBanner", "CorruptionTreasureSlimeBanner", "CrimsonTreasureSlimeBanner",
            "DungeonTreasureSlimeBanner", "EarthenGizmoBanner", "FamishedBanner", "FistfullBanner",
            "FlamingGhastBanner", "FluxSlimeBanner", "FrozenTreasureSlimeBanner", "FurnaceBanner",
            "GhastBanner", "GoldenTreasureSlimeBanner", "HallowTreasureSlimeBanner", "HoloEyeBanner",
            "HoloSlimeBanner", "HoloSwordBanner", "JungleTreasureSlimeBanner", "LesserWispBanner",
            "LostSoulBanner", "MaligmorBanner", "MutagenTreasureSlimeBanner", "NatureSlimeBanner",
            "PhaseAssaulterBanner", "PhaseSpeederBanner", "PlanetoidBanner", "PupaBanner",
            "PupaFlyBanner", "PyramidTreasureSlimeBanner", "RotWalkerBanner", "ShadowTreasureSlimeBanner",
            "SittingMushroomBanner", "SmallPhantarayBanner", "SnakeBanner", "SnakePotBanner",
            "TeratomaBanner", "ThroeBanner", "TreasureSlimeBanner", "TwilightDevilBanner",
            "TwilightScouterBanner", "UltracapBanner", "VoidTreasureSlimeBanner", "WallMimicBanner",
        };

        private static readonly HashSet<string> SOTSHardModeBanners = new HashSet<string>
        {
            "BleedingGhastBanner", "ChimeraBanner", "FlamingGhastBanner", "HallowTreasureSlimeBanner",
            "PhaseAssaulterBanner", "PhaseSpeederBanner", "VoidTreasureSlimeBanner",
        };
        #endregion
    }
}

