using System.Globalization;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class CardView : AbstractView<GameLogicService.Card>
    {
        [SerializeField] private TMP_Text _flipText = null;
        [SerializeField] private Button _button = null;

        protected override void OnInitialize(GameLogicService.Card data)
        {
            _flipText.text = data.Symbol.ToString(NumberFormatInfo.InvariantInfo);
        }

        protected override void OnDeinitialize() {}
    }
}