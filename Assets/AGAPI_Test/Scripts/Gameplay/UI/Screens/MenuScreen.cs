using System.Collections;
using System.Collections.Generic;
using AGAPI.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : UIScreenBehaviour
{

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private TMP_Text continueGameResultText;

    private int _selectedIndex;


    private List<Vector2Int> AvailableBoardSizes => ScreenManager.BoardConfig.AvailableBoardSizes;
    private Vector2Int SelectedSize => AvailableBoardSizes[_selectedIndex];
    private IUIInputHandler UiInputHandler => ScreenManager.UiInputHandler;

    public override void OnConfigured()
    {
        Setup();
        SetupButtons();
    }


    //-----------private methode----------

    private void SetupButtons()
    {
        newGameButton.onClick.AddListener(() => UiInputHandler.StartNewGame(SelectedSize));
        continueGameButton.onClick.AddListener(OnContinueGameButtonClicked);
    }
    private void OnContinueGameButtonClicked()
    {
        bool canContinue = UiInputHandler.TryContinueGame();
        continueGameResultText.text = canContinue ? "Continue Success" : "No Saved Game";
    }
    private void Setup()
    {
        continueGameResultText.text = "";
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void PopulateDropdown()
    {
        dropdown.ClearOptions();

        if (AvailableBoardSizes == null || AvailableBoardSizes.Count == 0)
        {
            dropdown.AddOptions(new List<string> { "No sizes" });
            dropdown.interactable = false;
            _selectedIndex = 0;
            return;
        }

        var options = new List<string>(AvailableBoardSizes.Count);
        for (int i = 0; i < AvailableBoardSizes.Count; i++)
        {
            var s = AvailableBoardSizes[i];
            options.Add($"{s.x} x {s.y}");
        }

        dropdown.AddOptions(options);

        // Default selection (0) or restore from saved index if you have one
        _selectedIndex = Mathf.Clamp(_selectedIndex, 0, AvailableBoardSizes.Count - 1);
        dropdown.SetValueWithoutNotify(_selectedIndex);
        dropdown.RefreshShownValue();
    }

    private void OnDropdownChanged(int newIndex)
    {
        _selectedIndex = Mathf.Clamp(newIndex, 0, AvailableBoardSizes.Count - 1);
    }
}
