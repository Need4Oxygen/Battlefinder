public enum E_PF2E_Size
{
    None, Default, Tiny, Small, Medium, Large, Huge, Gargantuan
}
public enum E_PF2E_Saves
{
    None, Default, Fortitude, Reflex, Will
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

public enum E_PF2E_CharacterTraits
{
    None, Default, Humanoid, Dwarf, Elf
}

public enum E_PF2E_Ability
{
    None, Default, Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma, Free
}

public enum E_PF2E_Skill
{
    None, Default, Acrobatics, Arcana, Athletics, Crafting, Deception, Diplomacy,
    Intimidation, Medicine, Nature, Occultism, Performance, Religion, Society, Stealth,
    Survival, Thievery
}

public enum E_PF2E_Proficiency
{
    None, Default, Untrained, Trained, Expert, Master, Lengend
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


// -----------------------------EFFECTS-----------------------------
// Effects are buffs or detriments directed towards an specific stat.
public enum E_PF2E_FeatType
{
    None, Default, GeneralSkillFeat, SkillFeat, ClassFeat, ClassFeature, Heritage, AncestryFeat, AncestryFeature,
}
public enum E_PF2E_EffectAplication
{
    None, Default, Complex, Both
}
public enum E_PF2E_EffectTarget
{
    None = 0, Default = 0,
    HP = 1, AC = 2, Perception = 3,
    STR = 10, DEX = 11, CON = 12, INT = 13, WIS = 14, CHA = 15,
    SavesFortitude = 20, SavesReflex = 21, SavesWill = 22,
    AttackMelee = 30, AttackRange = 31, AttackSpell = 32, AttackDCs = 33,
    DamageMelee = 40, DamageRange = 41, DamageSpell = 42,
    ResistanceFire = 50, ResistanceCold = 51, ResistanceSlash = 52, ResistanceBlunt = 53, ResistancePiercing = 54,
    speedBase = 60, speedBasePenalty = 61, speedClimb = 62, speedClimbPenalty = 63, speedFly = 64, speedFlyPenalty = 65, speedSwim = 66, speedSwimPenalty = 67, speedBurrow = 68, speedBurrowPenalty = 69,
    Acrobatics = 80, Arcana = 81, Athletics = 82, Crafting = 83, Deception = 84, Diplomacy = 85, Intimidation = 86, Medicine = 87, Nature = 88, Occultism = 89, Performance = 90, Religion = 91, Society = 92, Stealth = 93, Survival = 94, Thievery = 95,
    ArmorSpeedPenalty = 100,
}
public enum E_PF2E_EffectType
{
    None, Default, Circumstance, Status, Item, Untyped
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
