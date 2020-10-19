public enum E_PF2E_Size
{
    None, Default, Tiny, Small, Medium, Large, Huge, Gargantuan
}

public enum E_PF2E_Alignment
{
    None, Default, LG, NG, CG, LN, NN, CN, LE, NE, CE
}

public enum E_PF2E_Languages
{
    None, Default, Common, Draconic, Dwarven, Elven, Ganomish, Goblin, Halfling, Jotun, Orcish,
    Sylvan, Undercommon, Abyssal, Aklo, Aquan, Auran, Celestial, Gnoll, Ignan, Infernal, Necril,
    Shadowtongue, Terran
}

public enum E_PF2E_Ability
{
    None, Default, Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma, Free
}

public enum E_PF2E_AbilityBoost
{
    None, Default, Ancestry, Background, Class, Lvl1Boost, Lvl5Boost, Lvl10Boost, Lvl15Boost, Lvl20Boost
}

public enum E_PF2E_Skill
{
    None, Default, Acrobatics, Arcana, Athletics, Crafting, Deception, Diplomacy,
    Intimidation, Medicine, Nature, Occultism, Performance, Religion, Society, Stealth,
    Survival, Thievery
}

public enum E_PF2E_ABC
{
    None, Default, Ancestry, Background, Class
}

public enum E_PF2E_Traineable
{
    None, Default,

    Acrobatics, Arcana, Athletics, Crafting, Deception, Diplomacy, Intimidation, Medicine,
    Nature, Occultism, Performance, Religion, Society, Stealth, Survival, Thievery,

    Fortitude, Reflex, Will,

    Perception,

    Unarmed, SimpleWeapons, MartialWeapons, AdvancedWeapons,
    Unarmored, LightArmor, MediumArmor, HeavyArmor,

    Lore,
}

// -----------------------------BUILD-----------------------------
public enum E_PF2E_FeatType
{
    None, Default, GeneralSkillFeat, SkillFeat, ClassFeat, ClassFeature, Heritage, AncestryFeat, AncestryFeature,
}
public enum E_PF2E_BuildItem
{
    None, Default,
    InitialAbilityBoost, AbilityBoost, Action,
    Heritage, AncestryFeature, AncestryFeat,
    GeneralSkillFeat, SkillFeat, SkillTraining, SkillIncrease,
    ClassFeature, ClassFeat, ClassSkill,

    ResearchField,
    Instinct,
    Muses,
    DeitySkill, ChampionsCode, DeityAndCause, DivineAlly,
    DivineFont, Doctrine,
    DruidicOrder,
    PathToPerfection, SecondPathToPerfection, ThirdPathToPerfection,
    HuntersEdge,
    RoguesRacket,
    Bloodline,
    ArcaneSchool, ArcaneThesis
}


// -----------------------------ABC-----------------------------
// Ancestries, Backgrounds and Classes
public enum E_PF2E_Ancestry
{
    None, Default, Dwarf, Elf, Gnome, Goblin, Halfling, Human
}
public enum E_PF2E_Background
{
    None, Default, Acolyte, Acrobat, AnimalWhisperer, Artisan, Artist, Barkeep, Barrister, BountyHunter, Charlatan,
    Criminal, Detective, Emissary, Entretainer, Farmhand, FieldMedic, FortuneTeller, Gambler, Gladiator, Guard,
    Herbalist, Hermit, Hunter, Laborer, MartialDisciple, Merchant, Miner, Noble, Nomad, Prisioner, Sailor, Shcolar,
    Scout, StreetUrchin, Tinker, Warrior
}
public enum E_PF2E_Class
{
    None, Default, Alchemist, Barbarian, Bard, Champion, Cleric, Druid, Fighter, Monk, Ranger, Rogue, Sorcerer, Wizard
}


// -----------------------------ACTIONS-----------------------------
public enum E_PF2E_ActionType
{
    None, Default, Free, Reaction, AP1, AP2, AP3
}


// -----------------------------SKILLS-----------------------------
public enum E_PF2E_Skill_GFeat
{
    None, Default,
    AdoptedAncestry, ArmorProficiency, BreathControl, CannyAcumen, Diehard, FastRecovery, FeatherStep,
    Fleet, IncredibleInitiative, Ride, ShieldBlock, Toughness, WeaponProficiency, AncestralParagon,
    UntrainedImprovisation, ExpeditiousSearch, IncredibleInvestiture
}
public enum E_PF2E_Skill_Feat
{
    None, Default,
    Assurance, DubiousKnowledge, QuickIdentification, RecognizeSpell, SkillTraining, TrickMagicItem,
    AutomaticKnowledge, MagicalShorthand, QuickRecognition,

    CatFall, QuickSqueeze, SteadyBalance, NimbleCrawl, KipUp,

    ArcaneSense, UnifiedTheory,

    CombatClimber, HeftyHauler, QuickJump, TitanWrestler, UnderwaterMarauder, PowerfulLeap, RapidMantel,
    QuickClimb, QuickSwim, WallJump, CloudJump,

    AlchemicalCrafting, QuickRepair, SnareCrafting, SpecialtyCrafting, MagicalCrafting, ImpeccableCrafting,
    Inventor, CraftAnything,

    CharmingLiar, LengthyDiversion, LieToMe, Confabulator, QuickDisguise, SlipperySecrets,

    BargainHunter, GroupImpression, Hobnobber, GladHand, ShamelessRequest, LegendaryNegotiation,

    GroupCoercion, IntimidatingGlare, QuickCoercion, IntimidatingProwess, LastingCoercion, BattleCry,
    TerrifiedRetreat, ScaretoDeath,

    AdditionalLore, ExperiencedProfessional, UnmistakableLore, LegendaryProfessional,

    BattleMedicine, ContinualRecovery, RobustRecovery, WardMedic, LegendaryMedic,

    NaturalMedicine, TrainAnimal, BondedAnimal,

    OddityIdentification, BizarreMagic,

    FascinatingPerformance, ImpressivePerformance, VirtuosicPerformer, LegendaryPerformer,

    StudentOfTheCanon, DivineGuidance,

    CourtlyGraces, Multilingual, ReadLips, SignLanguage, Streetwise, Connections, LegendaryCodebreaker,
    LegendaryLinguist,

    ExperiencedSmuggler, TerrainStalker, QuietAllies, FoilSenses, SwiftSneak, LegendarySneak,

    ExperiencedTracker, Forager, SurveyWildlife, TerrainExpertise, PlanarSurvival, LegendarySurvivalist,

    Pickpocket, SubtleTheft, WaryDisarmament, QuickUnlock, LegendaryThief,

}
