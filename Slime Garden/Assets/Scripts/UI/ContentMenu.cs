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

    public void UpdateContent()
    {
        foreach (Transform child in hiddenContent)
            contentList.Add(child);

        totalPages = contentList.Count / numPerPage;
        if ((contentList.Count % numPerPage) > 0)
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
