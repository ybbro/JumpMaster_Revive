using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, descriptionText;

    public void InitInfos(string _name, string _description)
    {
        nameText.text = _name;
        descriptionText.text = _description;
    }
}
