using UnityEngine;
using UnityEngine.UI;

public class GridAutoScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public RectTransform parentContainer;
    public int padding = 5; // Padding for scaling

    public int rows = 1;

    void Start(){
        gridLayout = GetComponent<GridLayoutGroup>();
        parentContainer = GetComponent<RectTransform>();
    }

    private void Update()
    {
        ScaleGridCells();
    }

    private void ScaleGridCells()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Calculate grid dimensions
        // int rows = Mathf.CeilToInt(Mathf.Sqrt(childCount));
        int columns = childCount;

        // Calculate the cell size
        float cellWidth = (parentContainer.rect.width - padding * (columns - 1)) / columns;
        float cellHeight = (parentContainer.rect.height - padding * (rows - 1)) / rows;
        if (cellWidth > 100){
            cellWidth = 100;
        }
        if (cellHeight > 100){
            cellHeight = 100;
        }

        // Set cell size in Grid Layout
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}