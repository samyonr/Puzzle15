using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleGame
{
    public class Puzzle : MonoBehaviour
    {

        public Texture2D image;
        public int blocksPerLine = 4;
        public int shuffleLength = 20;
        public float defaultMoveDuration = .2f;
        public float shuffleMoveDuration = .1f;
        
        public enum GameMode
        {
            Game,
            Demo
        }

        GameMode gameMode;

        enum PuzzleState
        {
            Solved,
            Shuffling,
            InPlay
        };

        PuzzleState state;

        Block emptyBlock;
        Block[,] blocks;
        Queue<Block> inputs;
        bool blockIsMoving;
        int shuffleMovesRemaining;
        Vector3Int prevShuffleOffset;

        public void Init(GameMode mode)
        {
            image = ScenePropertirs.GameImage;
            gameMode = mode;
            blocksPerLine = 4;
            shuffleLength = 30;
            defaultMoveDuration = 0.2f;
            shuffleMoveDuration = 0.1f;
            CreatePuzzle();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (gameMode == GameMode.Game)
            {                
                if (state == PuzzleState.Solved && Input.GetKeyDown(KeyCode.Space))
                {
                    StartShuffle();
                }
                if (state == PuzzleState.InPlay && Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            else
            {
                if (state !=  PuzzleState.Shuffling)
                {
                    StartShuffle();
                }              
            }
        }

        public void CreatePuzzle()
        {
            blocks = new Block[blocksPerLine, blocksPerLine];
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
                    block.OnFinishedMoving += OnBlockFinishedMoving;
                    block.Init(new Vector3Int(x, y, 0), imageSlices[blocksPerLine - x - 1, blocksPerLine - y - 1]);
                    blocks[x, y] = block;

                    if (y == 0 && x == blocksPerLine - 1)
                    {
                        emptyBlock = block;
                    }
                }
            }

            Camera.main.transform.position = new Vector3(0, 1, -blocksPerLine * 2f);
            inputs = new Queue<Block>();
        }

        void PlayerMoveBlockInput(Block blockToMove)
        {
            if (state == PuzzleState.InPlay)
            {
                inputs.Enqueue(blockToMove);
                MakeNextPlayerMove();
            }
        }

        void MakeNextPlayerMove()
        {
            while (inputs.Count > 0 && !blockIsMoving)
            {
                MoveBlock(inputs.Dequeue(), defaultMoveDuration);
            }
        }

        void MoveBlock(Block blockToMove, float duration)
        {
            if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
            {
                blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
                blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

                Vector3Int targetCoord = emptyBlock.coord;
                emptyBlock.coord = blockToMove.coord;
                blockToMove.coord = targetCoord;

                Vector3 targetPosition = emptyBlock.transform.position;
                emptyBlock.transform.position = blockToMove.transform.position;
                blockToMove.MoveToPisition(targetPosition, duration);
                blockIsMoving = true;
            }
        }

        void OnBlockFinishedMoving()
        {
            blockIsMoving = false;
            CheckIfSolved();

            if (state == PuzzleState.InPlay)
            {
                MakeNextPlayerMove();
            }
            else if (state == PuzzleState.Shuffling)
            {
                if (shuffleMovesRemaining > 0)
                {
                    MakeNextShuffleMove();
                }
                else
                {
                    state = PuzzleState.InPlay;
                }
            }
        }

        void StartShuffle()
        {  
            state = PuzzleState.Shuffling;

            shuffleMovesRemaining = shuffleLength;

            emptyBlock.gameObject.SetActive(false);

            MakeNextShuffleMove();
        }

        void MakeNextShuffleMove()
        {
            Vector3Int[] offsets = {new Vector3Int(1, 0, 0),
                                    new Vector3Int(-1, 0, 0),
                                    new Vector3Int(0, 1, 0),
                                    new Vector3Int(0, -1, 0) };
            int randomIndex = Random.Range(0, offsets.Length);

            for (int i = 0; i < offsets.Length; i++)
            {
                Vector3Int offset = offsets[(randomIndex + i) % offsets.Length];
                if (offset != prevShuffleOffset * -1)
                {
                    Vector3Int moveBlockCoord = emptyBlock.coord + offset;

                    if (moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine &&
                        moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                    {
                        MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration);
                        shuffleMovesRemaining--;
                        prevShuffleOffset = offset;
                        break;
                    }
                }
            }
        }

        void CheckIfSolved()
        {
            foreach (Block block in blocks)
            {
                if (!block.IsAtStartingCoord())
                {
                    return;
                }
            }
            
            state = PuzzleState.Solved;
            emptyBlock.gameObject.SetActive(true);
        }
        
    }
}