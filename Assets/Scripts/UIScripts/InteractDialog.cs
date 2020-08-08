using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InteractDialog : MonoBehaviour
{
    [SerializeField]
    private float hidePosY;
    [SerializeField]
    private float moveDialogTime;
    [SerializeField]
    private GameObject dialogPanel;
    private Image dialogPanelImage;
    private Button dialogPanelButton;
    private Sequence lastSequence;
    private void Awake()
    {
        hidePosY = -gameObject.GetComponent<RectTransform>().rect.height;
        dialogPanelImage = dialogPanel.GetComponent<Image>();
        dialogPanelButton = dialogPanel.GetComponent<Button>();
        dialogPanelButton.onClick.AddListener(() => MoveDialog(false));
    }
    public void MoveDialog(bool isUp)
    {
        lastSequence?.Kill();
        if (isUp)
        {
            lastSequence = DOTween.Sequence()
                                  .OnStart(() =>
                                  {
                                      dialogPanelImage.canvasRenderer.SetAlpha(0);
                                      dialogPanel.SetActive(true);
                                      dialogPanelButton.interactable = true;
                                      dialogPanelImage.CrossFadeAlpha(0.5f, moveDialogTime, false);
                                  })
                                  .Join(transform.DOMoveY(0, moveDialogTime))
                                  .Play();
        }
        else
        {
            dialogPanelButton.interactable = false;
            lastSequence = DOTween.Sequence()
                                  .OnStart(() => dialogPanelImage.CrossFadeAlpha(0, moveDialogTime, false))
                                  .Join(transform.DOMoveY(hidePosY, moveDialogTime))
                                  .OnComplete(() => dialogPanel.SetActive(false))
                                  .Play();
        }
    }
}