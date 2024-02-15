using System.Collections;
using TMPro;
using UnityEngine;


public class TriggerManager : MonoBehaviour
{
    private Transform Target;
    public bool isResource;
    private TextMeshPro carry;
    [SerializeField] private TextMeshPro moneyAdded;
    private Transform Base;
    [SerializeField] private int mineStrength;
    [SerializeField] TextMeshProUGUI MoneyUI;
    private CollectionManager collectionManager;
    
    void Start()
    {
        collectionManager = GameObject.Find("Base").GetComponent<CollectionManager>();
        Base = GameObject.Find("Base").GetComponent<Transform>();
       
        Target = isResource ? GameObject.Find("baseTrigger").GetComponent<Transform>() : GameObject.Find("mineTrigger").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            carry = other.GetComponentInChildren<TextMeshPro>();
            PerformAction(other);
            
                
        }
    }

    

    void PerformAction(Collider other)
    {
        NPCController gravityManager = other.GetComponent<NPCController>();
        Animator animator = other.GetComponentInChildren<Animator>();

        if (gravityManager != null && animator != null)
        {
            gravityManager.isMining = true;
            animator.SetBool("mine", true);

            StartCoroutine(IncreaseNumber(() =>
            {
                gravityManager.isMining = false;
                animator.SetBool("mine", false);
                gravityManager.moveToTarget = Target;

                
            }));
        }
    }

    IEnumerator IncreaseNumber(System.Action onCompletion)
    {
        if (isResource)
        {
            for (int i = 1; i <= 20; i++)
            {
                carry.SetText(i + "/20");

                yield return new WaitForSeconds(1f / mineStrength); 
            }
        }
        else
        {
            for (int i = 20; i >= 0; i--)
            {
                carry.SetText(i + "/20");

                yield return new WaitForSeconds(0.2f); 
            }

            StartCoroutine(SpawnAndFade());
            collectionManager.Money += 100;
            MoneyUI.SetText(collectionManager.Money + "$");
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

