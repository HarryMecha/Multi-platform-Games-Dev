/* ACKNOWLEDGMENTS
 * Code has been modified from ScrollingTextExample script found at: https://www.youtube.com/watch?v=Z8efnBeXHeQ&ab_channel=SpeedTutor
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{

    [SerializeField][TextArea] private List<string> itemInfo = new List<string>();
    [SerializeField] private float textSpeed = 0.01f;
    [SerializeField] private TextMeshProUGUI itemTextInfo;
    private int currentDisplayingText = 0;
    public bool isFinished = false;

    public void ActivateText()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        for (int i = 0; i < itemInfo[currentDisplayingText].Length + 1; i++)
        {
            itemTextInfo.text = itemInfo[currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
        isFinished = true;
        yield break;
    }

    public void addToItemInfo(string itemName)
    {
        itemInfo.Add(itemName);
    }

    public void incrimentText()
    {
        currentDisplayingText++;
    }
}
   
