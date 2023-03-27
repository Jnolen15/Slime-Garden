using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMenu : MonoBehaviour
{
    [SerializeField] private Transform hiddenContent;
    [SerializeField] private Transform shownContent;
    [SerializeField] private int curPage;
    [SerializeField] private int totalPages;

    [SerializeField]
    private List<Transform> contentList = new List<Transform>();

    public void UpdateContent()
    {
        foreach (Transform child in hiddenContent)
            contentList.Add(child);

        totalPages = contentList.Count / 8;
        if ((contentList.Count % 8) > 0)
            totalPages++;

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
        Debug.Log("Next Page " + curPage);

        // Clear shown content
        for (int i = shownContent.childCount; i > 0; i--)
        {
            shownContent.GetChild(i-1).SetParent(hiddenContent);
        }

        // Fill it with next page of content
        for (int i = 0; i < 8; i++)
        {
            var curNum = i + (curPage * 8);

            if (curNum >= contentList.Count)
                break;

            Debug.Log("Showing "  + curNum);

            contentList[curNum].SetParent(shownContent);
        }
    }
}
