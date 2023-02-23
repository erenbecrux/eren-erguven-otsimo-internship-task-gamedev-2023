using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    // player variables
    Transform playerTransform;
    Vector2 playerPosition;
    Vector2 playerNextPosition;
    [SerializeField] int remainingMoves;

    // other objects variables
    [SerializeField] GameObject rockObject;
    [SerializeField] GameObject lavaObject;
    [SerializeField] GameObject keyObject;
    Vector2 rockPosition;

    // grid varibles
    [SerializeField] List<GameObject> gridCells;
    [SerializeField] List<bool> availableGridCells;
    List<Vector2> gridCellPositions = new List<Vector2>();

    // game controller
    GameController gameController;
    private bool isGameOver = false;
    private bool hasKey = false;

    // UI variables
    [SerializeField] TextMeshProUGUI remainingMoveText; 

    private void Start() 
    {
        playerTransform = this.transform;
        playerPosition = playerTransform.position;
        playerNextPosition = playerPosition;
        
        gameController = FindObjectOfType<GameController>();

        remainingMoveText.text = "Remaining Move: " + remainingMoves;

        // get gridcell positions
        foreach(GameObject gridCell in gridCells)
        {
            gridCellPositions.Add(gridCell.transform.position);
        }

        // randomly place blocks
        PlaceLavaBlock();
        PlaceRockBlock();
        PlaceKeyBlock();
    }

    private void Update() 
    {
        if(Input.GetKeyDown("up"))
        {
            playerNextPosition = playerPosition + new Vector2 (0,1);
            if(CanMove())
            {
                remainingMoves--;
            }
        }
        if(Input.GetKeyDown("down"))
        {
            playerNextPosition = playerPosition + new Vector2 (0,-1);
            if(CanMove())
            {
                remainingMoves--;
            }
        }
        if(Input.GetKeyDown("right"))
        {
            playerNextPosition = playerPosition + new Vector2 (1,0);
            if(CanMove())
            {
                remainingMoves--;
            }
        }
        if(Input.GetKeyDown("left"))
        {
            playerNextPosition = playerPosition + new Vector2 (-1,0);
            if(CanMove())
            {
                remainingMoves--;
            }
        }   

        if(CanMove())
        {
            MoveNextPosition();
        }

        // update remaining move text
        remainingMoveText.text = "Remaining Move: " + remainingMoves;

        // check for remaining moves
        if(remainingMoves <= 0)
        {
            isGameOver = true;
            gameController.GameOver();
        } 
    }

    // check next position for movement
    private bool CanMove()
    {

        // check for game over
        if(isGameOver)
        {
            return false;
        }

        // check for grid border
        foreach(Vector2 gridCellPosition in gridCellPositions)
        {
            if(playerNextPosition == gridCellPosition)
            {
                // check for obstacles
                if(playerNextPosition == rockPosition)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    // player moves to next position
    private void MoveNextPosition()
    {
        this.transform.position = playerNextPosition;
        playerPosition = playerNextPosition; // update player's current position
    }

    // checks for available grid cells, then places lava block
    private void PlaceLavaBlock()
    {
        bool isLavaPlaced = false;
        int lavaIndex;
        while(!isLavaPlaced)
        {
            lavaIndex = Random.Range(0,availableGridCells.Count);
            if(availableGridCells[lavaIndex])
            {
                isLavaPlaced = true;
                lavaObject.transform.position = gridCellPositions[lavaIndex];
                availableGridCells[lavaIndex] = false;
            }
        }
    }

    // checks for available grid cells, then places rock block
    private void PlaceRockBlock()
    {
        bool isRockPlaced = false;
        int rockIndex;
        while(!isRockPlaced)
        {
            rockIndex = Random.Range(0,availableGridCells.Count);
            if(availableGridCells[rockIndex])
            {
                isRockPlaced = true;
                rockObject.transform.position = gridCellPositions[rockIndex];
                availableGridCells[rockIndex] = false;
                rockPosition = rockObject.transform.position;
            }
        }
    }

    // checks for available grid cells, then places key
    private void PlaceKeyBlock()
    {
        bool isKeyPlaced = false;
        int keyIndex;
        while(!isKeyPlaced)
        {
            keyIndex = Random.Range(0,availableGridCells.Count);
            if(availableGridCells[keyIndex])
            {
                isKeyPlaced = true;
                keyObject.transform.position = gridCellPositions[keyIndex];
                availableGridCells[keyIndex] = false;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if player enters lava
        if(other.tag == "Lava")
        {
            isGameOver = true;
            gameController.GameOver();
        }

        // if player enters finish
        if(other.tag == "Finish")
        {
            if(hasKey)
            {
                isGameOver = true;
                gameController.GameSuccess();
            } 
        }

        // if player enters key
        if(other.tag == "Key")
        {
            hasKey = true;
            Destroy(keyObject);
        }
    }


}
