using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndCard : MonoBehaviour
{
    [SerializeField] Text text;
    public void ShowEndCard(string content)
    {
        text.text = content;
    }
}
