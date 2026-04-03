using System.Collections.Generic;
using UnityEngine;

public class SpaceUIManager : MonoBehaviour
{
    public static SpaceUIManager Instance;

    [SerializeField] private GameObject infoUIPrefab;
    private List<Transform> itemTranform = new List<Transform>();
    private List<Transform> uiTranform = new List<Transform>();
    private Transform camTransform;

    private void Awake()
    {
        Instance = this;
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        for(int i = 0;i<itemTranform.Count;i++) uiTranform[i].position = itemTranform[i].position;
    }

    public GameObject CreateItemUI(GameObject targetItem)
    {
        GameObject newUI = Instantiate(infoUIPrefab, this.transform);
        itemTranform.Add(targetItem.transform);
        uiTranform.Add(newUI.transform);
        return newUI;
    }
    public void RemoveItemUI(GameObject uiObject, GameObject itemObject)
    {
        if (uiTranform.Contains(uiObject.transform))
        {
            uiTranform.Remove(uiObject.transform);
        }
        if (itemTranform.Contains(itemObject.transform))
        {
            itemTranform.Remove(itemObject.transform);
        }
        Destroy(uiObject);
    }

    private void LateUpdate()
    {
        if (uiTranform.Count == 0) return;

        Quaternion targetRotation = camTransform.rotation;

        for(int i = 0; i < uiTranform.Count; i++)
        {
            uiTranform[i].transform.rotation = targetRotation;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
