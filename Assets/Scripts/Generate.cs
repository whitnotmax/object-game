using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generate : MonoBehaviour
{
    private Dictionary<GameObject, ObjectController> objectControllers = new Dictionary<GameObject, ObjectController>();

    public static Generate instance;
    public float radius;

    [SerializeField]
    private int numberToGenerate;

    [Header("Properties for generated objects")]
    public float maxSize;
    public float speed;
    public float scaleSpeedReduce;
    public float spawnScaleSpeedIncrease;
    public float despawnScaleSpeedIncrease;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < numberToGenerate; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.tag = "Object";
            obj.transform.localPosition = Random.insideUnitCircle * 15;
            obj.AddComponent<Rigidbody>().isKinematic = true;
            objectControllers.Add(obj, obj.AddComponent<ObjectController>());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            objectControllers[other.gameObject].expandState = ExpandState.Despawning;
        }
    }
}
