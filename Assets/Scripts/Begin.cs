using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Begin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private int difficulty = 0;
    public Texture2D cursorTexture;
    private GameController gc;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);
    private void Start()
    {
        gc = GetComponentInParent<GameController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        GameObject dr = GameObject.Find("Difficulty Rememberer");
        DiffMember dm = dr.GetComponent<DiffMember>();
        dm.difficulty = difficulty;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
