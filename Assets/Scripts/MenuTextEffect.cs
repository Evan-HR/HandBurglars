 using UnityEngine;  
 using System.Collections;  
 using UnityEngine.EventSystems;  
 using UnityEngine.UI;
 
 public class MenuTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
 
     public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
     {
         theText.color = Color.red; //Or however you do your color
        FindObjectOfType<AudioManager>().Play("tick");
        

    }
 
     public void OnPointerExit(PointerEventData eventData)
     {
         theText.color = Color.black; //Or however you do your color
     }
 }
