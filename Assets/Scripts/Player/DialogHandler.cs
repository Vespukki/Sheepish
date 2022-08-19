using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text textBox;
    CanvasGroup canGroup;

    PlayerInput input;
    Animator animator;

    InputAction next;

    const string clearTag = "<color=#0000>";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        canGroup = canvas.gameObject.GetComponent<CanvasGroup>();

        next = input.actions.FindAction("Next");
    }

    public void StartDialog(Dialog[] dialogs, NPC npc)
    {
        StartCoroutine(RunDialog(dialogs));
    }

    IEnumerator RunDialog(Dialog[] dialogs)
    {
        canGroup.alpha = 1;
        foreach (Dialog dialog in dialogs)
        {
            yield return StartCoroutine(PlayText(dialog));
        }
        yield return new WaitUntil(() => next.IsPressed());
        input.SwitchCurrentActionMap("Player");
        animator.SetTrigger("Reset");
        canGroup.alpha = 0;
    }

    IEnumerator PlayText(Dialog dialog)
    {
        textBox.ForceMeshUpdate();

        //textBox.text = dialog.text;

        textBox.ForceMeshUpdate();

        float textSpeed = .1f;
        List<TextModifier> activeEvents = new();
        for (int i = 0; i < dialog.text.Length; i++)
        {
            string final = dialog.text.Insert(i, clearTag);
            foreach(TextEvent tEvent in dialog.events)
            {
                if(tEvent.startIndex == i)
                {
                    activeEvents.Add(tEvent.modifer);
                    textSpeed *= tEvent.speedMulitplier;
                }
                else if(tEvent.endIndex == i)
                {
                    activeEvents.Remove(tEvent.modifer);
                    textSpeed /= tEvent.speedMulitplier;
                }

                foreach(TextModifier modifier in activeEvents)
                {
                }
            }
            textBox.text = final;
            yield return new WaitForSeconds(textSpeed);
        }
        textBox.text = dialog.text;

        yield return new WaitUntil(() => next.IsPressed());
    }

    public TextModifier Parse(string tag)
    {
        switch (tag)
        {
            default:
                return TextModifier.none;
        }
    }

    IEnumerator PrintDialog(List<string> dialog)
    {
        yield return null;
    }
}

[System.Serializable]
public class Dialog
{
    [TextArea]
    public string text;
    public List<TextEvent> events;
}

[System.Serializable]
public class TextEvent
{
    public TextModifier modifer = TextModifier.none;
    public float speedMulitplier = 1;
    public int startIndex;
    public int endIndex;
}


public enum TextModifier { none }

//save for later
/*foreach (TMP_LinkInfo link in textBox.textInfo.linkInfo)
        {
            TextModifier modifier = Parse(link.GetLinkID());


            int firstChar = (link.linkIdFirstCharacterIndex + link.linkIdLength + 2);

            for (int i = firstChar; i < link.GetLinkText().Length + firstChar; i++)
            {
                TMP_CharacterInfo charInfo = textBox.textInfo.characterInfo[i];
            }

        }*/
