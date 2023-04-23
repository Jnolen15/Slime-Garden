using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMenu : MonoBehaviour
{
    [SerializeField] private Transform hiddenContent;
    [SerializeField] private Transform shownContent;
    [SerializeField] private int numPerPage;
    [SerializeField] private int curPage;
    [SerializeField] private int totalPages;

    [SerializeField]
    private List<Transform> contentList = new List<Transform>();
    [SerializeField]
    private List<Transform> trashCollection = new List<Transform>();


    // ============== Setup / Updating ==============
    public void UpdateContent(string type, GameObject contentUIPrefab, InventoryManager inv, MenuManager menus)
    {
        // Clear previous content
        ClearContent();

        // Add new content
        if (type == "placeable")
        {
            foreach (PlaceableObjectSO so in inv.availablePlaceables)
            {
                var element = Instantiate(contentUIPrefab, hiddenContent);
                element.GetComponent<placeableUIContent>().Setup(so, menus);
                Debug.Log("Added content from inventory");
            }
        }
        else if (type == "seed")
        {
            foreach (CropSO so in inv.availableCrops)
            {
                var element = Instantiate(contentUIPrefab, hiddenContent);
                element.GetComponent<SeedUIContent>().Setup(so, menus);
            }
        }
        else if (type == "crop")
        {
            foreach (InventoryManager.CropInventroyEntry crop in inv.inventoryList)
            {
                var element = Instantiate(contentUIPrefab, hiddenContent);
                element.GetComponent<CropUIContent>().Setup(crop, menus);
            }
        }
        else 
        {
            Debug.LogError("Content type not specified");
        }

        Debug.Log($"Children instantiaded to hidden content {hiddenContent.childCount}");

        // Set up pages
        PageSetup();
    }

    public void ClearContent()
    {
        Debug.Log($"Children in hidden content pre clear {hiddenContent.childCount}");

        // Add objects to list to be destroyed
        foreach(Transform child in hiddenContent)
            trashCollection.Add(child);

        foreach (Transform child in shownContent)
            trashCollection.Add(child);

        // Unparent old objects
        hiddenContent.DetachChildren();
        shownContent.DetachChildren();

        // Destroy them
        foreach (Transform child in trashCollection)
        {
            Destroy(child.gameObject);
            Debug.Log("Child in trashCollection destroyed");
        }
        trashCollection.Clear();

        Debug.Log($"Children in hidden content post clear {hiddenContent.childCount}");

        // NOTE: The reason they are unparented and then destroyed is because
        // the destruction happens at the end of the frame and so it will result
        // in refrences being set then destroyed
    }

    public void PageSetup()
    {
        Debug.Log($"Content list size {contentList.Count}");

        contentList.Clear();
        curPage = 0;

        Debug.Log($"Content list size post clear {contentList.Count}");

        Debug.Log($"Children in hidden content {hiddenContent.childCount}");

        foreach (Transform child in hiddenContent)
            contentList.Add(child);

        totalPages = contentList.Count / numPerPage;
        if ((contentList.Count % numPerPage) > 0)
            totalPages++;

        Debug.Log($"Content list size post fill from hidden {contentList.Count}");

        UpdatePage();
    }

    // ============== Buttons ==============
    public void UpButton()
    {
        curPage--;

        if (curPage < 0)
            curPage = totalPages-1;

        UpdatePage();
    }

    public void DownButton()
    {
        curPage++;

        if (curPage > totalPages-1)
            curPage = 0;

        UpdatePage();
    }

    public void UpdatePage()
    {
        // Clear shown content
        for (int i = shownContent.childCount; i > 0; i--)
        {
            shownContent.GetChild(i-1).SetParent(hiddenContent);
        }

        // Fill it with next page of content
        for (int i = 0; i < numPerPage; i++)
        {
            var curNum = i + (curPage * numPerPage);

            if (curNum >= contentList.Count)
                break;

            contentList[curNum].SetParent(shownContent);
        }
    }
}
