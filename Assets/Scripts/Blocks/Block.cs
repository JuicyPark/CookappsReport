using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Blocks
{
    public enum Color { RED, YELLOW, GREEN, PURPLE, BLUE, TOP }

    public class Block : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] BlockMove blockMove;
        [SerializeField] Animator animator;
        [SerializeField] Transform[] sideTransform = new Transform[6];
        // 0 : normalBlock // 1 : stopTop // 2 : spinTop
        public int isTop;
        Color color;
        public bool visited = false;
        Block[] sideBlock = new Block[6];

        void Start()
        {
            SetColor();
            blockMove.ApplyDownTransform(sideTransform[3], sideTransform[2], sideTransform[4]);
            blockMove.ApplyGravity();
        }
        public void BoomParticle() => blockMove.Boom();
        void SetColor()
        {
            color = (Color)UnityEngine.Random.Range(0, 6);
            spriteRenderer.sprite = Resources.Load<Sprite>(color.ToString());
            if (color.Equals(Color.TOP))
                isTop = 1;
        }
        public void SpinTop()
        {
            for (int i = 0; i < 6; i++)
            {
                if (sideBlock[i])
                {
                    if (sideBlock[i].isTop == 1)
                    {
                        sideBlock[i].visited = true;
                        sideBlock[i].isTop = 2;
                        sideBlock[i].animator.SetTrigger("IsSpin");
                    }
                    else if (sideBlock[i].isTop == 2)
                    {
                        if (sideBlock[i].visited == false)
                        {
                            sideBlock[i].visited = true;
                            BlockManager.Instance.destroyCount++;
                            StageManager.Instance.topCount--;
                            BlockManager.Instance.destroyBlockBuffer.Enqueue(sideBlock[i]);
                        }
                    }
                }
            }
        }
        public void FindNeighborBlock()
        {
            for (int i = 0; i < sideBlock.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(sideTransform[i].position, Vector2.up, 0.1f, LayerInformation.Instance.block);
                if (hit)
                    sideBlock[i] = hit.transform.GetComponent<Block>();
                else
                    sideBlock[i] = null;
            }
        }
        public void FindEqualBlock(int side1, int side2)
        {
            if (sideBlock[side1] && sideBlock[side2])
            {
                if (sideBlock[side1].color == color && sideBlock[side2].color == color)
                {
                    if (!visited)
                    {
                        visited = true;
                        BlockManager.Instance.destroyBlockBuffer.Enqueue(this);
                    }
                    if (!sideBlock[side1].visited)
                    {
                        sideBlock[side1].visited = true;
                        BlockManager.Instance.destroyBlockBuffer.Enqueue(sideBlock[side1]);
                    }
                    if (!sideBlock[side2].visited)
                    {
                        sideBlock[side2].visited = true;
                        BlockManager.Instance.destroyBlockBuffer.Enqueue(sideBlock[side2]);
                    }
                }
            }
        }
        public void Swipe(float direction)
        {
            if (direction >= 0 && direction < 60)
                StartCoroutine(ChangePosition(1));
            else if (direction >= 60 && direction < 120)
                StartCoroutine(ChangePosition(0));
            else if (direction >= 120 && direction < 180)
                StartCoroutine(ChangePosition(5));
            else if (direction <= 0 && direction > -60)
                StartCoroutine(ChangePosition(2));
            else if (direction <= -60 && direction > -120)
                StartCoroutine(ChangePosition(3));
            else if (direction <= -120 && direction > -180)
                StartCoroutine(ChangePosition(4));
        }
        IEnumerator ChangePosition(int index)
        {
            if (sideBlock[index])
            {
                // State Change!!
                StateManager.Instance.state = State.Generate;
                SoundManager.Instance.Swipe();

                Frame tempFrame = blockMove.currentFrame;
                blockMove.currentFrame = sideBlock[index].blockMove.currentFrame;
                sideBlock[index].blockMove.currentFrame = tempFrame;

                Block tempBlock = blockMove.currentFrame.block;
                blockMove.currentFrame.block = sideBlock[index].blockMove.currentFrame.block;
                sideBlock[index].blockMove.currentFrame.block = tempBlock;

                blockMove.MoveToIndex(index);
                sideBlock[index].blockMove.MoveToIndex((index + 3) % 6);

                yield return new WaitForSeconds(0.3f);
                BlockManager.Instance.AllFindNeighborBlock();
                BlockManager.Instance.AllFindEqualBlock();
                Debug.Log(BlockManager.Instance.destroyCount);

                if (BlockManager.Instance.destroyBlockBuffer.Count == 0)
                {
                    BlockManager.Instance.InvokeCheck();
                    StateManager.Instance.state = State.Generate;
                    tempFrame = blockMove.currentFrame;
                    blockMove.currentFrame = sideBlock[(index + 3) % 6].blockMove.currentFrame;
                    sideBlock[(index + 3) % 6].blockMove.currentFrame = tempFrame;

                    tempBlock = blockMove.currentFrame.block;
                    blockMove.currentFrame.block = sideBlock[(index + 3) % 6].blockMove.currentFrame.block;
                    sideBlock[(index + 3) % 6].blockMove.currentFrame.block = tempBlock;

                    blockMove.MoveToIndex((index + 3) % 6);
                    sideBlock[(index + 3) % 6].blockMove.MoveToIndex(index);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                    StageManager.Instance.moveCount--;
                BlockManager.Instance.InvokeCheck();
            }
        }
    }
}