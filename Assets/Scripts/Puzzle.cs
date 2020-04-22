﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

    public Texture2D image;

    public int blocksPerLine = 4;
    Block emptyBlock;

    // Start is called before the first frame update
    void Start()
    {
        CreatePuzzle();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePuzzle()
    {
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);

        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                blockObject.transform.position = -Vector3.one * (blocksPerLine-1) * .5f + new Vector3(x, y, 0);
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.Init(new Vector3Int(x, y, 0), imageSlices[blocksPerLine - x - 1, blocksPerLine - y - 1]);
                //block.coord = new Vector3Int(x, y, 0);

                if (y == 0 && x == blocksPerLine - 1)
                {
                    blockObject.SetActive(false);
                    emptyBlock = block;
                }
            }
        }

        Camera.main.transform.position = new Vector3(0, 1, -blocksPerLine * 2f);
    }

    void PlayerMoveBlockInput(Block blockToMove)
    {
        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            Vector3Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector3 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.transform.position = targetPosition;
        }
    }
}