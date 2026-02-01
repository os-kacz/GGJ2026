using UnityEngine;

public class dmgNumberLogic : MonoBehaviour
{
    public float destroyTime = 0.5f;
    private Animation textBounce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
        var animToPlay = Random.Range(0, 2);
        textBounce = GetComponent<Animation>();
        if (animToPlay == 0)
        {
            textBounce.Play("DamageNumberPositive");
        }
        else
        {
            textBounce.Play("DamageNumberNegative");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
