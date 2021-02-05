using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinder2e.Containers;
using Pathfinder2e.GameData;
using UnityEngine.UI;

namespace Pathfinder2e.Player
{

    public enum E_Searcher_Type { None, Default, Heritage, AncestryFeat, ClassFeat, Dedication, ArchetypeFeat, SkillFeat, GeneralFeat }
    public enum E_Searcher_Sort { None, Default, ABC, LVL }

    public class Searcher : MonoBehaviour
    {
        [SerializeField] private CharacterCreation creation = null;

        [Header("Searcher")]
        [SerializeField] private Window window = null;
        [SerializeField] private TMP_InputField searchField = null;
        [SerializeField] private TMP_Dropdown typeDropdown = null;
        [SerializeField] private TMP_Dropdown sortDropdown = null;
        [SerializeField] private Transform resultButtonContainer = null;
        [SerializeField] private GameObject resultButtonPrefab = null;

        [Header("Feat Displayer")]
        [SerializeField] private Window featDisplayerWindow = null;
        [SerializeField] private VerticalLayoutGroup verticalContainer = null;
        [SerializeField] private TMP_Text featLvl = null;
        [SerializeField] private TMP_Text featName = null;
        [SerializeField] private Image featActionCostImg = null;
        [SerializeField] private TMP_Text featCost = null;
        [SerializeField] private TMP_Text featFrequency = null;
        [SerializeField] private TMP_Text featPrerequisites = null;
        [SerializeField] private TMP_Text featTrigger = null;
        [SerializeField] private TMP_Text featRequirement = null;
        [SerializeField] private TMP_Text featDescription = null;
        [SerializeField] private TMP_Text featSource = null;
        [SerializeField] private Transform traitsContainer = null;
        [SerializeField] private GameObject traitPrefab = null;

        [Header("General")]
        [Space(15)]
        [SerializeField] private Sprite[] actionCostImages = null;

        [HideInInspector] public bool isOpen;

        private E_Searcher_Type searchingType = 0;
        private E_Searcher_Sort searchingSort = 0;
        private List<Feat> query = new List<Feat>();
        private List<Feat> queryFiltered = new List<Feat>();
        private List<ResultButton> resultButtons = new List<ResultButton>();
        private List<TraitButton> traitButtons = new List<TraitButton>();

        private Color acceptedPrereqColor;
        private Color failedPrereqColor;

        void Start()
        {
            ObjectPooler.CreatePool(resultButtonPrefab, resultButtonContainer, 100);
            ObjectPooler.CreatePool(traitPrefab, traitsContainer, 6);

            searchField.onValueChanged.RemoveAllListeners();
            searchField.onValueChanged.AddListener((v) => { OnValueChanged_SearchField(v); });

            typeDropdown.onValueChanged.RemoveAllListeners();
            typeDropdown.interactable = false;
            typeDropdown.onValueChanged.AddListener((v) => { OnValueChanged_TypeDropdown(v); });

            sortDropdown.onValueChanged.RemoveAllListeners();
            sortDropdown.options = new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData("LVL"), new TMP_Dropdown.OptionData("ABC") };
            sortDropdown.SetValueWithoutNotify(0);
            sortDropdown.onValueChanged.AddListener((v) => { OnValueChanged_SortDropdown(v); });

            acceptedPrereqColor = resultButtonPrefab.GetComponent<ResultButton>().mainText.color;
            failedPrereqColor = Color.red;

            ClearFeatDisplayer();
        }

        public void OpenSearcher()
        {
            isOpen = true;
            window.OpenWindow();
        }

        public void CloseSearcher()
        {
            isOpen = false;
            window.CloseWindow();

            ClearQueries();
            Invoke("ClearResultButtons", 0.1f);

            CloseFeatDisplayer();
        }

        // Called by Searcher input field whenever is edited
        public void OnValueChanged_SearchField(string value)
        {
            queryFiltered = SearchFilter(query, value);
            Sort(ref queryFiltered);
            resultButtons = SpawnResultButtons(queryFiltered);
        }

        // Called by Type dropdown when user select another type of feat to search for
        public void OnValueChanged_TypeDropdown(int value)
        {
            switch (typeDropdown.options[value].text)
            {
                case "Class Feats": SearchPrivate(E_Searcher_Type.ClassFeat); break;
                case "Dedications": SearchPrivate(E_Searcher_Type.Dedication); break;
                case "Archetype Feats": SearchPrivate(E_Searcher_Type.ArchetypeFeat); break;
                default: SearchPrivate(E_Searcher_Type.ClassFeat); break;
            }
        }

        // Called by Back button on searcher bar
        public void OnClick_BackButton()
        {
            CloseSearcher();
            CloseFeatDisplayer();
        }

        // Called by Sort dropwdown when user select another sort system
        public void OnValueChanged_SortDropdown(int value)
        {
            switch (sortDropdown.options[sortDropdown.value].text)
            {
                case "LVL": searchingSort = E_Searcher_Sort.LVL; break;
                case "ABC": searchingSort = E_Searcher_Sort.ABC; break;
                default: searchingSort = E_Searcher_Sort.LVL; break;
            }

            Sort(ref queryFiltered);
            resultButtons = SpawnResultButtons(queryFiltered);
        }

        private void Sort(ref List<Feat> feats)
        {
            if (searchingSort == E_Searcher_Sort.ABC)
                feats = feats.OrderBy(x => x.name).ThenBy(x => x.level).ToList();
            else
                feats = feats.OrderBy(x => x.level).ThenBy(x => x.name).ToList();
        }

        // Called by build buttons to search for feats
        public void Search(E_Searcher_Type featType)
        {
            OpenSearcher();

            if (featType == E_Searcher_Type.ClassFeat)
            {
                typeDropdown.options = new List<TMP_Dropdown.OptionData>{
                    new TMP_Dropdown.OptionData("Class Feats"),
                    new TMP_Dropdown.OptionData("Dedications"),
                    new TMP_Dropdown.OptionData("Archetype Feats")};
                typeDropdown.SetValueWithoutNotify(0);
                typeDropdown.interactable = true;
            }
            else
            {
                typeDropdown.options = new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData("Type") };
                typeDropdown.SetValueWithoutNotify(0);
                typeDropdown.interactable = false;
            }

            SearchPrivate(featType);
        }

        // Called by this script to separate searchs from build buttons (public, requires OpenSearcher) and searchs from searcher dropdowns (searcher already opened)
        private void SearchPrivate(E_Searcher_Type featType)
        {
            searchingType = featType;
            query = new List<Feat>(SearchDB(featType));
            queryFiltered = SearchFilter(query);
            Sort(ref queryFiltered);
            resultButtons = SpawnResultButtons(queryFiltered);
        }

        // Returns feat list given a feat type
        private List<Feat> SearchDB(E_Searcher_Type featType)
        {
            switch (featType)
            {
                case E_Searcher_Type.Heritage: return DB.AncestryHeritages.Find(creation.currentPlayer.ancestry);
                case E_Searcher_Type.AncestryFeat: return DB.AncestryFeats.Find(creation.currentPlayer.ancestry);
                case E_Searcher_Type.ClassFeat: return DB.ClassFeats.Find(creation.currentPlayer.class_name);
                case E_Searcher_Type.Dedication: return DB.Dedications;
                case E_Searcher_Type.ArchetypeFeat: return DB.ArchetypeFeats;
                case E_Searcher_Type.SkillFeat: return DB.SkillFeats.Find("skill feats");
                case E_Searcher_Type.GeneralFeat: return DB.SkillFeats.Find("general feats");
                default: return new List<Feat>();
            }
        }

        private List<Feat> SearchFilter(List<Feat> feats) { return SearchFilter(feats, ""); }
        private List<Feat> SearchFilter(List<Feat> feats, string value)
        {
            List<Feat> filteredFeats = new List<Feat>();

            // Look if inputed value is a trait
            List<Trait> detectedTraits = new List<Trait>();
            string[] valueSplit = value.Split(new char[] { ' ', ',' });
            if (!string.IsNullOrEmpty(value))
                foreach (var item in valueSplit)
                {
                    Trait trait = DB.Traits.Find(ctx => ctx.name.Equals(item, StringComparison.OrdinalIgnoreCase));
                    if (trait != null)
                        detectedTraits.Add(trait);
                }
            else
            {
                return feats;
            }

            // Add matches to results
            filteredFeats = feats.FindAll(feat => feat.name.Contains(value, StringComparison.OrdinalIgnoreCase));

            // Add traits to results
            if (detectedTraits.Count > 0)
            {
                // filteredFeats = feats.FindAll(feat => feat.traits.Any(trait => detectedTraits.Any(searchedTrait => searchedTrait.name.Equals(trait, StringComparison.OrdinalIgnoreCase))));

                foreach (var feat in feats)
                {
                    int matchesCount = 0;
                    foreach (var item in detectedTraits)
                        if (feat.traits.Contains(item.name))
                            matchesCount++;
                        else
                            break;

                    if (matchesCount == detectedTraits.Count && !filteredFeats.Contains(feat))
                        filteredFeats.Add(feat);
                }
            }

            return filteredFeats;
        }

        private List<ResultButton> SpawnResultButtons(List<Feat> feats)
        {
            ClearResultButtons();

            List<ResultButton> buttons = new List<ResultButton>(feats.Count);

            foreach (var feat in feats)
            {
                ResultButton newButton = ObjectPooler.Spawn(resultButtonPrefab, resultButtonContainer).GetComponent<ResultButton>();
                newButton.transform.SetAsLastSibling();
                newButton.mainText.text = feat.name;
                newButton.levelText.text = feat.level.ToString();
                if (feat.actioncost != null)
                {
                    newButton.actionCostImage.sprite = ActionCostImage(feat.actioncost.ToString());
                    newButton.actionCostImage.enabled = true;
                }
                else
                {
                    newButton.actionCostImage.sprite = null;
                    newButton.actionCostImage.enabled = false;
                }

                newButton.mainText.color = acceptedPrereqColor;

                if (feat.prerequisites != null)
                    if (!PrerequisitesSolver.Check_Feat(feat, ref creation.currentPlayer).isValidated)
                        newButton.mainText.color = failedPrereqColor;


                newButton.button.onClick.AddListener(() => { OnClick_ResultButton(feat); });

                newButton.gameObject.SetActive(true);

                buttons.Add(newButton);
            }

            return buttons;
        }

        private void OnClick_ResultButton(Feat feat)
        {
            DisplayFeat(feat);
        }

        private void ClearResultButtons()
        {
            foreach (var item in resultButtons)
                item.Destroy();

            resultButtons.Clear();
        }

        private void ClearQueries()
        {
            query.Clear();
            queryFiltered.Clear();
        }

        //-------------------------------------------------FEAT DISPLAYER-------------------------------------------------

        private void OpenFeatDisplayer()
        {
            featDisplayerWindow.OpenWindow();
        }

        private void CloseFeatDisplayer()
        {
            featDisplayerWindow.CloseWindow();

            Invoke("ClearFeatDisplayer", 0.1f);
        }

        private void ClearFeatDisplayer()
        {
            featLvl.text = null;
            featName.text = null;
            featActionCostImg.sprite = null;
            featCost.text = null;
            featFrequency.text = null;
            featPrerequisites.text = null;
            featTrigger.text = null;
            featRequirement.text = null;
            featDescription.text = null;
            featSource.text = null;
            if (traitButtons.Count != 0)
                foreach (var item in traitButtons)
                    item.Destroy();
        }

        private void DisplayFeat(Feat feat)
        {
            featLvl.text = feat.level.ToString();
            featName.text = feat.name;

            if (feat.actioncost != null)
            {
                featActionCostImg.sprite = ActionCostImage(feat.actioncost.ToString());
                featActionCostImg.gameObject.SetActive(true);
            }
            else
            {
                featActionCostImg.gameObject.SetActive(false);
            }

            if (feat.cost != null)
            {
                featCost.text = $"<b><color=#C59D6B>Cost:</color></b> {feat.cost}";
                featCost.gameObject.SetActive(true);
            }
            else
            {
                featCost.gameObject.SetActive(false);
            }

            if (feat.frequency != null)
            {
                featFrequency.text = $"<b><color=#C59D6B>Frequency:</color></b> {feat.frequency}";
                featFrequency.gameObject.SetActive(true);
            }
            else
            {
                featFrequency.gameObject.SetActive(false);
            }

            if (feat.trigger != null)
            {
                featTrigger.text = $"<b><color=#C59D6B>Trigger:</color></b> {feat.trigger}";
                featTrigger.gameObject.SetActive(true);
            }
            else
            {
                featTrigger.gameObject.SetActive(false);
            }

            if (feat.requirement != null)
            {
                featRequirement.text = $"<b><color=#C59D6B>Requirement:</color></b> {feat.requirement}";
                featRequirement.gameObject.SetActive(true);
            }
            else
            {
                featRequirement.gameObject.SetActive(false);
            }

            if (feat.descr != null)
            {
                featDescription.text = $"{feat.descr}";
                featDescription.gameObject.SetActive(true);
            }
            else
            {
                featDescription.gameObject.SetActive(false);
            }

            if (feat.prerequisites != null)
            {
                string[] prerequisites = new string[feat.prerequisites.Count];
                for (int i = 0; i < feat.prerequisites.Count; i++)
                    prerequisites[i] = feat.prerequisites[i].descr;
                featPrerequisites.text = $"<b><color=#C59D6B>Prerequisites:</color></b> {string.Join(" ,", prerequisites)}";
            }
            else
            {
                featPrerequisites.gameObject.SetActive(false);
            }

            if (feat.source != null)
            {
                string sourceString = "";

                for (int i = 0; i < feat.source.Count; i++)
                {
                    if (i > 0) sourceString += "\n";

                    SourceInfo source = DB.Sources.Find(x => feat.source[i].abbr == x.abbr);

                    if (source != null)
                        sourceString += $"<b><color=#C59D6B>{source.short_name}</color></b> <size=15>pg.{feat.source[i].page_start}-{feat.source[i].page_stop}</size>";
                    else
                        sourceString += $"<b><color=#C59D6B>{feat.source[i].abbr}</color></b>\n<size=15>pg.{feat.source[i].page_start}-{feat.source[i].page_stop}</size>";
                }
                featSource.text = sourceString;
            }
            else
            {
                featSource.gameObject.SetActive(false);
            }

            if (traitButtons.Count != 0)
                foreach (var item in traitButtons)
                    item.Destroy();
            if (feat.traits != null)
                foreach (var item in feat.traits)
                {
                    GameObject trait = ObjectPooler.Spawn(traitPrefab, traitsContainer);
                    trait.transform.SetAsLastSibling();
                    TraitButton traitScript = trait.GetComponent<TraitButton>();
                    traitScript.text.text = item;
                    traitButtons.Add(traitScript);
                    trait.gameObject.SetActive(true);
                }

            OpenFeatDisplayer();
        }

        public void OnClick_AcceptButton()
        {
            // Add feat to build
        }

        public void OnClick_CancelButton()
        {
            CloseFeatDisplayer();
        }

        IEnumerator UpdateVerticalLayoutGroup()
        {
            verticalContainer.enabled = false;
            yield return null;
            verticalContainer.enabled = true;
        }

        //-------------------------------------------------GENERAL-------------------------------------------------
        private Sprite ActionCostImage(string name)
        {
            switch (name)
            {
                case "1": return actionCostImages[0];
                case "2": return actionCostImages[1];
                case "3": return actionCostImages[2];
                case "R": return actionCostImages[3];
                case "F": return actionCostImages[4];
                default: return actionCostImages[0];
            }
        }

    }

}
