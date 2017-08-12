using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputTextField : MonoBehaviour
{
    [SerializeField]
    private Color notValidColor;

    private Text text;
    private Text label;
    private Color labelBaseColor;
    private Color labelBadColor;
    private bool isValid;

    private void Awake()
    {
        text = GetComponentsInChildren<Text>().Single(x => x.name == "Text");
        label = GetComponentsInChildren<Text>().Single(x => x.name == "Label");
        labelBadColor = notValidColor;
    }

    private void Start()
    {
        labelBaseColor = label.color;
    }

    public string Text
    {
        get
        {
            return text.text;
        }
        set
        {
            text.text = value;
        }
    }

    public bool IsValid
    {
        get
        {
            return isValid;
        }
        set
        {
            isValid = value;
            SetValid(isValid);
        }
    }

    private void SetValid(bool value)
    {
        if(value)
        {
            label.color = labelBaseColor;
        }
        else
        {
            label.color = labelBadColor;
        }
    }
}
