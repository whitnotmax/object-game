using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] availableObjects;
    
    private Dictionary<GameObject, ObjectController> objectControllers = new Dictionary<GameObject, ObjectController>();
    private List<GameObject> targets = new List<GameObject>();
    private int wave = 1;

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
        if (targets.Count == 0)
        {
            wave++;
            foreach (var pair in objectControllers)
            {
                RemoveObject(pair.Key, false);
            }
            objectControllers.Clear();
            GenerateObjects();
            numberToGenerate *= 2;
            speed *= 1.5f;
        }
    }

    private void GenerateObjects()
    {
        for (int i = 0; i < numberToGenerate; i++)
        {
            GameObject randomObject = availableObjects[Random.Range(0, availableObjects.Length)];
            GameObject obj = Instantiate(randomObject);
            obj.tag = "Object";
            obj.name = randomObject.name; // so that we can see if the object is the right one when it is clicked lmfaooo
            obj.transform.localPosition = Random.insideUnitCircle * 15;
            obj.AddComponent<Rigidbody>().isKinematic = true;
            obj.SetActive(true);
            objectControllers.Add(obj, obj.AddComponent<ObjectController>());
            if (obj.name == correctObject.name)
            {
                targets.Add(obj);
            }

        }
    }

    public void RemoveObject(GameObject obj, bool addScoreIfCorrect)
    {
        try
        {
            if (obj.name == correctObject.name)
            {
                if (addScoreIfCorrect)
                {
                    score++;

                }
                targets.Remove(obj);
            }
            objectControllers[obj].expandState = ExpandState.Despawning;
        } 
        catch
        {
            // lol
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            RemoveObject(other.gameObject, false);
        }
        objectControllers.Remove(other.gameObject);
    }
}
