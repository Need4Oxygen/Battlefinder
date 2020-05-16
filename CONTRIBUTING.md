# Contributing to Battlefinder

Contribution applications are open via our [Discord Server](https://discord.gg/9F6dmbV). It is necessary to be on the server to contribute to this project, since it's going to be the main channel of communication between Contributors and Main Devs.

Once your application is approved, you will be added as an external collaborator of the project, which will give you the necessary permissions to create and push new branches. Please read the Branch Rules section.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* [Unity 2019.3.9f1](https://unity3d.com/get-unity/download/archive) - Unity version

### Installing

Just install the correct Unity version, tell it where the project folder is and run it.

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
