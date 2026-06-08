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

        public static void LoadModBanners()
        {
            SkippedBanners.Clear();

            // Calamity banners are drawn from each item's own icon (UseItemIcon) instead of
            // the Calamity banner atlas, so banner sprites never desync when Calamity adds or
            // removes banners. The name list is matched against the loaded mod at runtime.
            #region CalamityMod
            AddItemIconBanners("CalamityMod", CalamityBanners, CalamityHardModeBanners);
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

            #region CalamityFables
            AddItemIconBanners("CalamityFables", CalamityFablesBanners);
            #endregion

            #region ContinentOfJourney
            // Homeward Journey: every banner enemy is hardmode (the only pre-hardmode enemy,
            // Bucket Zombie, has no banner), so the whole list is the hardmode set.
            AddItemIconBanners("ContinentOfJourney", ContinentOfJourneyBanners, new HashSet<string>(ContinentOfJourneyBanners));
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
        private static void AddItemIconBanners(string modName, string[] bannerNames, HashSet<string> hardModeNames = null)
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

                BannerDict[item.Type] = new BannerInfo
                {
                    ItemId = item.Type,
                    ItemName = item.Name,
                    BannerCount = 0,
                    IsHardMode = hardModeNames != null && hardModeNames.Contains(bannerName),
                    ModName = modName,
                    UseItemIcon = true,
                    TileType = sample.createTile,
                    Index = sample.placeStyle,
                };
            }
        }

        // Internal banner item names extracted from each mod's assets
        // (Items/Banners/*Banner for Thorium & Spirit, *BannerItem for Spirit Reforged).
        // The list is matched against the loaded mod at runtime via Mod.TryFind, so any
        // name missing from a given mod version is simply skipped.

        // ===========================================================================
        //  CALAMITY
        // ===========================================================================
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
            "GulperEelBanner", "HadarianBanner", "HeatSpiritBanner", "IceClasperBanner",
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
            "BloatfishBanner", "BloomSlimeBanner", "BobbitWormBanner", "BohldohrBanner",
            "ChaoticPufferBanner", "CloudElementalBanner", "ColossalSquidBanner", "CryoSlimeBanner",
            "CryonBanner", "DevilFishBanner", "EarthElementalBanner", "EidolistBanner",
            "EidolonWyrmJuvenileBanner", "FlakCrabBanner", "FusionFeederBanner", "GammaSlimeBanner",
            "GiantSquidBanner", "GulperEelBanner", "HadarianBanner", "IceClasperBanner",
            "ImpiousImmolatorBanner", "InfernalCongealmentBanner", "IrradiatedSlimeBanner", "MantisBanner",
            "MantisShrimpBanner", "MelterBanner", "MirageJellyBanner", "NovaBanner",
            "OrthoceraBanner", "OverloadedSoldierBanner", "PerennialSlimeBanner", "PestilentSlimeBanner",
            "PhantomSpiritBanner", "PlagueChargerBanner", "PlaguebringerBanner", "PlagueshellBanner",
            "ProfanedEnergyBanner", "ReaperSharkBanner", "RenegadeWarlockBanner", "ScornEaterBanner",
            "SeaSerpentBanner", "ShockstormShuttleBanner", "SightseerColliderBanner", "SightseerSpitterBanner",
            "StellarCulexBanner", "SulphurousSkaterBanner", "TrilobiteBanner", "VirulingBanner",
            // Added in Calamity 2.1.2 (Burrower, CladCrab and Shromble are pre-hardmode)
            "AstraglomerateBanner", "AuroraSpiritBanner", "DraconicSwarmerBanner",
        };

        // ===========================================================================
        //  CATALYST
        // ===========================================================================
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

        // ===========================================================================
        //  THORIUM
        // ===========================================================================
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

        // ===========================================================================
        //  SPIRIT
        // ===========================================================================
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

        // ===========================================================================
        //  SPIRIT REFORGED
        // ===========================================================================
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

        // ===========================================================================
        //  CONSOLARIA
        // ===========================================================================
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

        // ===========================================================================
        //  VITALITY
        // ===========================================================================
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

        // ===========================================================================
        //  CALAMITY FABLES
        // ===========================================================================
        // All banners are pre-hardmode (no hardmode banner enemies).
        private static readonly string[] CalamityFablesBanners =
        {
            "BlizzardSpriteBannerItem", "CloudSpriteBannerItem", "GeodeCrawlerBannerItem", "SandstormSpriteBannerItem",
            "SpelunkerScarabBannerItem", "StormlionBannerItem", "StormlionLarvaBannerItem", "WulfrumGrapplerBannerItem",
            "WulfrumMagnetizerBannerItem", "WulfrumMortarBannerItem", "WulfrumNexusBannerItem", "WulfrumRollerBannerItem",
            "WulfrumRoverBannerItem",
        };

        // ===========================================================================
        //  HOMEWARD JOURNEY  (ContinentOfJourney)
        // ===========================================================================
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
    }
}

