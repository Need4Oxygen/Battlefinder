using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinder2e;
using Pathfinder2e.Character;
using Pathfinder2e.Containers;
using UnityEngine;
using UnityEngine.UI;

public class SkillPlanner : MonoBehaviour
{
    [SerializeField] private CharacterCreation creation = null;

    [Header("SkillPlanner")]
    [SerializeField] private Window window = null;
    [SerializeField] private List<Toggle> toggles = null;

    [HideInInspector] public bool isOpen = false;

    private RuleElement keyRule = null;
    private int maxActiveToggles = 0;
    private bool[] initialState = new bool[16];

    void Awake()
    {
        AssignTogglesListeners();
    }

    public void OpenWithRule(RuleElement rule)
    {
        keyRule = rule;

        PlayerIntoToggles();
        Open_SkillPlanner();
    }

    private void Open_SkillPlanner()
    {
        for (int i = 0; i < toggles.Count; i++)
            initialState[i] = toggles[i].isOn;

        isOpen = true;
        window.OpenWindow();
    }

    private void Close_SkillPlanner(bool save)
    {
        bool somethingChanged = false;
        for (int i = 0; i < toggles.Count; i++)
            if (initialState[i] != toggles[i].isOn) { somethingChanged = true; break; }

        isOpen = false;
        window.CloseWindow();

        if (save && somethingChanged)
        {
            TogglesIntoPlayer();
            creation.RefreshPlayerIntoPanel();
        }

        keyRule = null;
    }

    private void PlayerIntoToggles()
    {
        List<string> canTrain = GetTraineables(keyRule.proficiency); // Make toggles interactive or not
        List<string> alrTraining = new List<string>(); // Make toggles interactive and checked or not
        switch (keyRule.key)
        {
            case "skill_static":
                maxActiveToggles = 1;
                if (!RuleElement.IsEmpty(creation.currentPlayer.skill_unspent[keyRule]))
                    alrTraining.Add(creation.currentPlayer.skill_unspent[keyRule].selector);
                break;
            case "skill_choice":
                List<string> choices = keyRule.value_list.ConvertAll(x => x.value);
                choices.RemoveAll(x => !canTrain.Contains(x)); // Remove the non traineables from choice options
                canTrain = choices;
                maxActiveToggles = 1;
                if (!RuleElement.IsEmpty(creation.currentPlayer.skill_choice[keyRule]))
                    alrTraining.Add(creation.currentPlayer.skill_choice[keyRule].selector);
                break;
            case "skill_free":
                maxActiveToggles = int.Parse(keyRule.value);
                foreach (var element in creation.currentPlayer.skill_free[keyRule])
                    if (!RuleElement.IsEmpty(element))
                        alrTraining.Add(element.selector);
                break;
            case "skill_improve":
                maxActiveToggles = 1;
                if (!RuleElement.IsEmpty(creation.currentPlayer.skill_improve[keyRule]))
                    alrTraining.Add(creation.currentPlayer.skill_improve[keyRule].selector);
                break;
            default:
                break;
        }

        foreach (var toggle in toggles)
        {
            toggle.SetIsOnWithoutNotify(false);
            toggle.interactable = false;
        }

        foreach (var selector in canTrain)
        {
            Toggle toggle = toggles[DB.Skl_Full2Int(selector)];
            toggle.interactable = true;
        }

        foreach (var selector in alrTraining)
        {
            Toggle toggle = toggles[DB.Skl_Full2Int(selector)];
            toggle.interactable = true;
            toggle.SetIsOnWithoutNotify(true);
        }
    }

    private void TogglesIntoPlayer()
    {
        List<RuleElement> newTraining = new List<RuleElement>();

        for (int i = 0; i < toggles.Count; i++)
            if (toggles[i].isOn)
            {
                RuleElement newRule = new RuleElement();
                newRule.from = keyRule.from;
                newRule.key = "skill_static";
                newRule.selector = DB.Skl_Int2Full(i);
                newRule.level = keyRule.level;
                newRule.proficiency = keyRule.proficiency;
                newTraining.Add(newRule);
            }

        creation.currentPlayer.Skill_Set(keyRule, newTraining);
        creation.RefreshPlayerIntoPanel();
    }

    private List<string> GetTraineables(string prof)
    {
        List<string> list = new List<string>();
        foreach (var sklName in DB.SkillNames)
            if (creation.currentPlayer.Skill_TraineableTo(sklName, prof))
                list.Add(sklName);
        return list;
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LISTENERS
    private void AssignTogglesListeners()
    {
        for (int i = 0; i < toggles.Count; i++)
            toggles[i].onValueChanged.RemoveAllListeners();

        // Putting this inside a for breaks it, still don't know why
        toggles[0].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[0], v); });
        toggles[1].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[1], v); });
        toggles[2].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[2], v); });
        toggles[3].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[3], v); });
        toggles[4].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[4], v); });
        toggles[5].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[5], v); });
        toggles[6].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[6], v); });
        toggles[7].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[7], v); });
        toggles[8].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[8], v); });
        toggles[9].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[9], v); });
        toggles[10].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[10], v); });
        toggles[11].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[11], v); });
        toggles[12].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[12], v); });
        toggles[13].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[13], v); });
        toggles[14].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[14], v); });
        toggles[15].onValueChanged.AddListener((v) => { OnValueChangedToggle(toggles[15], v); });
    }

    private void OnValueChangedToggle(Toggle toggle, bool value)
    {
        if (toggles.Where(ctx => ctx.isOn == true).Count() <= maxActiveToggles && value)
            toggle.SetIsOnWithoutNotify(true);
        else
            toggle.SetIsOnWithoutNotify(false);
    }


    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- EXIT
    public void OnClick_AcceptButton()
    {
        Close_SkillPlanner(true);
    }

    public void OnClick_CancelButton()
    {
        Close_SkillPlanner(false);
    }

}
