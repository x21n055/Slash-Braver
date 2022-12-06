using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Image image;
    private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;

    [SerializeField] private int id = 0;
    [SerializeField] private float pressOffsetY = 0f;
    [SerializeField] private UnityEvent onClick = null;

    private Transform child;
    private float defaultY;
    private ButtonController[] buttonControllers;

    private bool isPushed = false;

    public string UIname;


    void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
        child = transform.GetChild(0);
        defaultY = child.localPosition.y;

        Transform canvas = GameObject.Find(UIname).transform;
        buttonControllers = canvas.GetComponentsInChildren<ButtonController>();
    }

    void OnEnable()
    {
        ButtonActive(true);
    }

    public void ButtonActive(bool active)
    {
        isPushed = !active;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPushed) return;
        Vector3 pos = child.localPosition;
        pos.y = defaultY - pressOffsetY;
        child.localPosition = pos;
        if (pressedSprite != null) image.sprite = pressedSprite;
        OnButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPushed) return;
        Vector3 pos = child.localPosition;
        pos.y = defaultY;
        child.localPosition = pos;
        image.sprite = defaultSprite;
        OnButtonUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (id != -1)
        {
            foreach (var controller in buttonControllers)
            {
                controller.ButtonActive(controller.id != this.id);
            }
        }

        OnButtonClick();
        onClick?.Invoke();
    }

    public void AllButtonReset()
    {
        foreach (var controller in buttonControllers)
        {
            controller.ButtonActive(true);
        }
    }

    private void OnButtonDown()
    {
        // DownéûÇÃã§í èàóù
    }

    private void OnButtonUp()
    {
        // UpéûÇÃã§í èàóù
    }

    private void OnButtonClick()
    {
        // ClickéûÇÃã§í èàóùÅiSEñ¬ÇÁÇ∑Ç»Ç«Åj
    }
}