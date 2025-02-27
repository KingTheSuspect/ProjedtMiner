using System.Collections;
using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TriggerManager : MonoBehaviour
{
    private Transform Target;
    public bool isResource;
    private TextMeshPro carry;
    [SerializeField] private TextMeshPro moneyAdded;
    private Transform Base;
    [SerializeField] private int mineStrength;
    [SerializeField] TextMeshProUGUI MoneyUI;
    [SerializeField] TextMeshProUGUI RareOreUI;
    private CollectionManager collectionManager;
  

    
    void Start()
    {
        
        
        collectionManager = GameObject.Find("Base").GetComponent<CollectionManager>();
        Base = GameObject.Find("Base").GetComponent<Transform>();
       
       
    }

    private void OnTriggerStay(Collider other)
    {
        NPCController gravityManager = other.GetComponent<NPCController>();

        if (other.CompareTag("Player") && !gravityManager.isMining)
        {
            if (isResource) //resource trigger
            {
                TextMeshPro carry = other.GetComponentInChildren<TextMeshPro>();
                
                if (carry.text == "20/20")
                {
                    gravityManager.moveToTarget = Base; // Set the target to the base or any other target
                    gravityManager.isMoving = true; // Start moving the NPC towards the target
                    Debug.Log("Carry is full! Moving to base.");
                    return;
                }
                
                gravityManager.cycleFinished = false;
                StartCoroutine(PerformAction(other, carry));
            }
            else //base trigger
            {
                TextMeshPro carry = other.GetComponentInChildren<TextMeshPro>();
                if(carry.text == "0/20") return;

                gravityManager.cycleFinished = false;
                StartCoroutine(PerformAction(other, carry));
            }
        }
    }

    IEnumerator PerformAction(Collider other, TextMeshPro carryText)
    {
        NPCController gravityManager = other.GetComponent<NPCController>();
        Animator animator = other.GetComponentInChildren<Animator>();

        if (gravityManager != null && animator != null)
        {
            gravityManager.isMining = true;
            animator.SetBool("mine", true);

            yield return StartCoroutine(IncreaseNumber(() =>
            {
                gravityManager.isMining = false;
                animator.SetBool("mine", false);
                gravityManager.moveToTarget = Target;

            }, gravityManager, carryText));
        }
    }

    IEnumerator IncreaseNumber(System.Action onCompletion, NPCController npc, TextMeshPro carryText)
    {
        
        if (isResource)
        {
            for (int i = 1; i <= 20; i++)
            {
                int num = Random.Range(1, 100);
                
                if(num > 95)
                {
                    collectionManager.Rare_ore += 1;
                    RareOreUI.SetText("Rare ore: " + collectionManager.Rare_ore);
                }
                
                carryText.SetText(i + "/20");

                yield return new WaitForSeconds(1f / mineStrength); 
            }
            
            Target = GameObject.Find("baseTrigger").GetComponent<Transform>();
            //Debug.Log(npc.name + " is moving");
            npc.isMoving = true;
        }
        else
        {
            for (int i = 20; i >= 0; i--)
            {
                carryText.SetText(i + "/20");

                yield return new WaitForSeconds(0.2f); 
            }

            StartCoroutine(SpawnAndFade());
            collectionManager.Money += 100;
            MoneyUI.SetText(collectionManager.Money + "$");
            
            npc.cycleFinished = true;
            npc.isMoving = false;
        }
        
        onCompletion?.Invoke();
    }
    
    
        IEnumerator SpawnAndFade()
        {
           
            TextMeshPro textMeshPro = Instantiate(moneyAdded, Base.position, Quaternion.identity);
            textMeshPro.transform.LookAt(Camera.main.transform);
            textMeshPro.transform.Rotate(0, 180, 0);

            
            Color originalColor = textMeshPro.color;
            originalColor.a = 1.0f;
            textMeshPro.color = originalColor;

           
            textMeshPro.transform.position = Base.position;

           
            float riseDuration = 1.5f; 
            float elapsedTime = 0f;
            while (elapsedTime < riseDuration)
            {
                textMeshPro.transform.position += Vector3.up * Time.deltaTime * 2;
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            
            float fadeDuration = 0.8f;
            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                
                originalColor.a = alpha;
                textMeshPro.color = originalColor;

                yield return null;
                elapsedTime += Time.deltaTime;
            }

            
            DestroyImmediate(textMeshPro.gameObject);


        }
    
}

