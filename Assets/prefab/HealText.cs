using UnityEngine;
using TMPro;

public class HealText : MonoBehaviour
{
    public float lifetime = 0.7f;          // Thời gian tồn tại của văn bản
    public float floatSpeed = 0.3f;        // Tốc độ di chuyển của văn bản
    public TextMeshPro textMesh;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Hủy đối tượng sau một thời gian tồn tại
    }

    private void Update()
    {
        // Di chuyển văn bản lên trên
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }

    public void SetText(string text)
    {
        if (textMesh != null)
        {
            textMesh.text = text;
        }
    }
}
