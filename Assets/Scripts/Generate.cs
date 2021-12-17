using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] availableObjects;
    
    private Dictionary<GameObject, ObjectController> objectControllers = new Dictionary<GameObject, ObjectController>();

    public static Generate instance;
    public float radius;
    public GameObject correctObject;
    public int score;

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
        correctObject = availableObjects[Random.Range(0, availableObjects.Length)];
        instance = this;
        GenerateObjects();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateObjects()
    {
        for (int i = 0; i < numberToGenerate; i++)
        {
            GameObject randomObject = availableObjects[Random.Range(0, availableObjects.Length)];
            GameObject obj = Instantiate(randomObject);
            obj.tag = "Object";
            obj.name = randomObject.name; // jank lol
            obj.transform.localPosition = Random.insideUnitCircle * 15;
            obj.AddComponent<Rigidbody>().isKinematic = true;
            obj.SetActive(true);
            objectControllers.Add(obj, obj.AddComponent<ObjectController>());

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            objectControllers[other.gameObject].expandState = ExpandState.Despawning;
            objectControllers.Remove(other.gameObject);

        }
    }
}
