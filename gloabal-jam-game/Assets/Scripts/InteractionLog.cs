using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Interactions
{
    Moved,
    Faced,
    Switched,
    Created,
}

public class InteractionLog : MonoBehaviour
{
    public Dictionary<string, string> NormalInversePairs = new Dictionary<string, string>()
    {
        
    };
    public struct DataLog
    {
        public Interactions interaction;
        public Vector2Int position;
        public GameObject createdObject;
        public GameObject destroyedObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
