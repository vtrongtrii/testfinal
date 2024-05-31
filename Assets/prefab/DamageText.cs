using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float lifetime = 1f;
    public float floatSpeed = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the text after the lifetime
    }

    void Update()
    {
        // Make the text float upwards
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text;
    }
}
