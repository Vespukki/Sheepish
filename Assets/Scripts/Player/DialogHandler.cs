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
        textBox.text = dialog.text;

        textBox.maxVisibleCharacters = 0;
        textBox.ForceMeshUpdate();

        List<TextModifier> activeEvents = new();

        List<TextEvent> tEvents = new();

        foreach (TMP_LinkInfo link in textBox.textInfo.linkInfo)
        {
            Debug.Log(link.GetLinkID());
            int first = (link.linkTextfirstCharacterIndex);
            int last = link.GetLinkText().Length + first;

            tEvents.Add(new TextEvent(dialog.events[int.Parse(link.GetLinkID())].modifer, first, last));
        }

        for (int i = 0; i < textBox.GetParsedText().Length; i++)
        {
            float textSpeed = .04f;

            foreach(TextEvent tEvent in tEvents)
            {
                if(tEvent.startIndex == i)
                {
                    activeEvents.Add(tEvent.modifer);
                }
                else if(tEvent.endIndex == i)
                {
                    activeEvents.Remove(tEvent.modifer);
                }

                
            }

            foreach (TextModifier modifier in activeEvents)
            {
                switch (modifier)
                {
                    case TextModifier.slow:
                        textSpeed *= 10;
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(textSpeed);
            textBox.maxVisibleCharacters++;
        }
        yield return new WaitUntil(() => next.IsPressed());

        List<TMP_LinkInfo> links = new(textBox.textInfo.linkInfo);
        links.Clear();
        textBox.textInfo.linkInfo = links.ToArray();
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
    [HideInInspector] public int startIndex = 0;
    [HideInInspector] public int endIndex = 1000;

    public TextEvent()
    {

    }

    public TextEvent(TextModifier _modifier, int _startIndex, int _endIndex)
    {
        modifer = _modifier;
        startIndex = _startIndex;
        endIndex = _endIndex;
    }
}


public enum TextModifier { none, slow }

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
