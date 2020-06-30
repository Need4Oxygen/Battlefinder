# Contributing to Battlefinder

Contribution applications are open via our [Discord Server](https://discord.gg/9F6dmbV). It is necessary to be on the server to contribute to this project, since it's going to be the main channel of communication between Contributors and Main Devs.


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* [Unity 2019.3.9f1](https://unity3d.com/get-unity/download/archive) - Unity version
* Download all the assets in the list at the bottom of this document.
* Once you have all needed assets, contact us at [contact@need4oxygen.com](contact@need4oxygen.com), we will give you access to a submodule with all the configuration files.

### How To Collaborate
* Fork the project from the link above & clone locally.
* Update the Third Party Assets submodule.
* Sync your local copy before you branch.
* Branch for each separate piece of work.
* Do the work, write good commit messages.
* Push to your origin repository.
* Create a new Pull Request towards our repository.
* Respond to any code review feedback.

## Branch Rules

### Main Branches

* "master" - Only updated for releases.
* "develop" - Main branch. Push your work towards this branch.

Theoretically, no bug should ever enter "develop". Practically, no bug should ever enter "master".

### Naming

Branch rules are applied by checking for specific strings contained in their name.

* Branches containing "master" will be heavily protected as they will adquire the protections corresponding to the releases branch.
* Branches containing "develop" will be heavily protected as they will adquire the protections corresponding to the main branch.
* Branches containing "BF-\*" will be protected as they correspond to main developer tasks.

Please try to avoid naming your branches in a way that contains any of the aforementioned strings.
Contributor branches should be something like "C-[Affected_System]-[Purpose]" for example:

* C-DataBase-Update
* C-Grid_Shader-Refactor

## Coding Style

Mildly condensed, highly readable.

When declarating variables, prioritize inspector legibility:

```c#
    [Header("Initial Ability Boosts")]
    [SerializeField] public CanvasGroup iAblBoostsPanel = null;
    [SerializeField] private Transform ancestryContainer = null;
    [SerializeField] public Transform backgroundContainer = null;
    [SerializeField] private Toggle[] level1Toggles = null;
```

Hierarchical operations might be condensed as c# allows it:

```c#
    foreach (var item in ancestry.abilityBoosts)
        alreadyBoosted.Add(PF2E_DataBase.AbilityToFullName(item.Value.target));
    foreach (var item in ancestry.abilityFlaws)
        alreadyBoosted.Add(PF2E_DataBase.AbilityToFullName(item.Value.target));
        
    if (options != null)
        foreach (var item in options)
            list.Add(new TMP_Dropdown.OptionData(PF2E_DataBase.AbilityToFullName(item)));    
```

Always group functionality regardless of accessibility:

```c#
    //------------------ANCESTRIES ASSIGMENT------------------
    private void AssignAncestryBoosts() {}
    public void OnValueChangedAncestryDropdown(int value) {}
    public void SaveAncestryOptions() {}
    
    //------------------BACKGROUND ASSIGMENT------------------
    private void AssignBackgroundBoosts() {}
    public void OnValueChangedBackgroundDropdown(int value) {}
    public void SaveBackgroundOptions() {}
```

Always try to comment public functions, especially if they are called via UI because those references wont't appear when you look for them in your code editor.

```c#
    <summary> Called by X button instantiated on Y system to make Z </summary>
    public void OnValueChangedBackgroundDropdown(int value) {}
```
## Third Party Assets

* https://www.youtube.com/watch?v=LBpKUIyOHdo&t=
* https://www.textures.com/download/3dscans0055/127567
* https://www.textures.com/download/pbr0194/133232
* https://www.textures.com/download/pbr0579/138828
* https://www.textures.com/download/pbr0007/133043
* https://www.textures.com/download/pbr0183/133222
* https://www.textures.com/download/pbr0433/138073
* https://www.textures.com/download/substance0111/131745
* https://www.textures.com/download/pbr0139/133174
* https://paizo.com/community/communityuse/package
* https://freesound.org/people/KToppMod/sounds/512282/
* https://assetstore.unity.com/packages/3d/props/furniture/medieval-tavern-pack-112546
* https://assetstore.unity.com/packages/3d/environments/fantasy/blacksmith-s-forge-17785
* https://assetstore.unity.com/packages/3d/environments/wooden-bridge-64399
* https://assetstore.unity.com/packages/3d/environments/fantasy/medieval-tent-small-18736
* https://assetstore.unity.com/packages/3d/environments/historic/medieval-barrows-and-wagons-33411
* https://assetstore.unity.com/packages/3d/environments/historic/modular-medieval-lanterns-85527
* https://assetstore.unity.com/packages/3d/props/free-medieval-props-asset-pack-131420
* https://assetstore.unity.com/packages/3d/environments/free-medieval-room-131004#reviews
* https://assetstore.unity.com/packages/3d/environments/fantasy/mega-fantasy-props-pack-87811
