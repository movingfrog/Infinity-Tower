using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceUIManager : MonoBehaviour
{
    public Color[] rarityColor = { Color.white, Color.cyan, Color.yellow };

    public static SpaceUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject infoUIPrefab;
    private List<Transform> itemTransform = new List<Transform>();
    private List<Transform> uiTranform = new List<Transform>();
    private Transform camTransform;

    private void Awake()
    {
        Instance = this;
        camTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < itemTransform.Count; i++)
            uiTranform[i].position = itemTransform[i].position;
    }

    public GameObject CreateItemUI(GameObject targetItem)
    {
        GameObject newUI = Instantiate(infoUIPrefab, this.transform);
        itemTransform.Add(targetItem.transform);
        uiTranform.Add(newUI.transform);
        return newUI;
    }

    public void ChangeItemTransform(GameObject targetItem, GameObject originItem)
    {
        int index = itemTransform.IndexOf(originItem.transform);
        if (index != -1)
        {
            itemTransform[index] = targetItem.transform;
        }
    }

    public void RemoveItemUI(GameObject uiObject, GameObject itemObject)
    {
        if (uiTranform.Contains(uiObject.transform))
        {
            uiTranform.Remove(uiObject.transform);
        }
        if (itemTransform.Contains(itemObject.transform))
        {
            itemTransform.Remove(itemObject.transform);
        }
        Destroy(uiObject);
    }

    private void LateUpdate()
    {
        if (uiTranform.Count == 0)
            return;

        Quaternion targetRotation = camTransform.rotation;

        for (int i = 0; i < uiTranform.Count; i++)
        {
            uiTranform[i].transform.rotation = targetRotation;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
