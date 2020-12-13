using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor;

    public int distanceToEnd;

    public bool includeShop;
    public int minDistToShop;
    public int maxDistToShop;

    public Transform generatorPoint;

    public enum Directions
    { up, right, down, left };
    public Directions selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask roomLayoutLayer;

    private GameObject endRoom, shopRoom;

    private List<GameObject> roomLayoutObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop;
    public RoomCenter[] potentialCenters;



    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        //selectedDirection = (Directions)Random.Range(0, 4);
        //MoveGeneratonPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            selectedDirection = (Directions)Random.Range(0, 4);
            MoveGeneratonPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, roomLayoutLayer))
            {
                MoveGeneratonPoint();
            }

            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            roomLayoutObjects.Add(newRoom);
            if (i + 1 == distanceToEnd)
            {
                roomLayoutObjects.RemoveAt(roomLayoutObjects.Count - 1);
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                endRoom = newRoom;
            }

        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistToShop, maxDistToShop + 1);
            shopRoom = roomLayoutObjects[shopSelector];
            roomLayoutObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        //create room outline
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in roomLayoutObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        foreach (GameObject roomOutline in generatedOutlines)
        {
            bool generateCenter = true;
            if (roomOutline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, roomOutline.transform.position, roomOutline.transform.rotation).theRoom = roomOutline.GetComponent<Room>();

                generateCenter = false;
            }

            if (roomOutline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, roomOutline.transform.position, roomOutline.transform.rotation).theRoom = roomOutline.GetComponent<Room>();
                generateCenter = false;
            }

            if(includeShop)
            {
                if (roomOutline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, roomOutline.transform.position, roomOutline.transform.rotation).theRoom = roomOutline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], roomOutline.transform.position, roomOutline.transform.rotation).theRoom = roomOutline.GetComponent<Room>();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif
    }

    public void MoveGeneratonPoint()
    {
        switch (selectedDirection)
        {
            case Directions.up:
                {
                    generatorPoint.position += new Vector3(0f, yOffset, 0f);
                    break;
                }
            case Directions.down:
                {
                    generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                    break;
                }
            case Directions.right:
                {
                    generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                    break;
                }
            case Directions.left:
                {
                    generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                    break;
                }
        }
    }


    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 02f, roomLayoutLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 02f, roomLayoutLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 02f, roomLayoutLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 02f, roomLayoutLayer);

        int directionCount = 0;

        if (roomAbove)
        { directionCount++; }
        if (roomBelow)
        { directionCount++; }
        if (roomLeft)
        { directionCount++; }
        if (roomRight)
        { directionCount++; }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exist!");
                break;
            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;

            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownRight, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                break;

            case 3:
                if (roomAbove && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownLeft, roomPosition, transform.rotation));
                }
                if (roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownRight, roomPosition, transform.rotation));
                }
                if (roomLeft && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightLeftDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightLeftUp, roomPosition, transform.rotation));
                }
                break;

            case 4:
                if (roomLeft && roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
                }
                break;
        }
    }


}



[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleLeft, singleRight, singleDown, singleUp,
        doubleLeftRight, doubleUpDown, doubleUpLeft, doubleUpRight, doubleDownLeft, doubleDownRight,
        tripleUpDownLeft, tripleUpDownRight, tripleRightLeftDown, tripleRightLeftUp,
        fourWay;
}
