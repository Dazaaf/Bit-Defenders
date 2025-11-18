using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddCoinsButton : MonoBehaviour
{
    public Button button;
    public float cooldownTime = 5f;  
    private bool isCooldown = false;

    public void GiveCoins()
    {
        if (isCooldown) return;

        
        CurrencySystem.Instance.AddCoins(50);  

        
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        button.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        button.interactable = true;
        isCooldown = false;
    }
}

