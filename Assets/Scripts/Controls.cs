using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text dialogText;
    private int index = 0;
    
    [TextArea(3, 10)]
    [SerializeField] string[] textBlock;

    void Start()
    {
        StartCoroutine(RunStudyDialogMoving());
    }

    IEnumerator RunStudyDialogMoving()
    {
        dialogPanel.SetActive(true);
        //Debug.Log(textBlock[index]);
        dialogText.text = textBlock[index];
        index++;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W));

        StartCoroutine(RunStudyDialogCamera());
    }

    IEnumerator RunStudyDialogCamera()
    {
        //Debug.Log(textBlock[index]);
        dialogText.text = textBlock[index];
        index++;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        StartCoroutine(RunStudyDialogInteraction());
    }

    IEnumerator RunStudyDialogInteraction()
    {
        //Debug.Log(textBlock[index]);
        dialogText.text = textBlock[index];
        index++;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        StartCoroutine(RunStudyDialogPick());
    }

    IEnumerator RunStudyDialogPick()
    {
        //Debug.Log(textBlock[index]);
        dialogText.text = textBlock[index];
        index++;

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(RunStudyDialogChecklist());
    }

    IEnumerator RunStudyDialogChecklist()
    {
        //Debug.Log(textBlock[index]);
        dialogText.text = textBlock[index];

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));

        dialogPanel.SetActive(false);
    }
}
