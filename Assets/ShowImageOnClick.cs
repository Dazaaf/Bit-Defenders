using UnityEngine;
using UnityEngine.UI;

public class ShowImageOnClick : MonoBehaviour
{
    public GameObject imageToShow;

    public void ToggleImage()
    {
        if (imageToShow != null)
        {
            imageToShow.SetActive(!imageToShow.activeSelf);
        }
    }
}
