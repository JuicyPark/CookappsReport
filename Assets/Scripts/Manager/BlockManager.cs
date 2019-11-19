using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blocks;
using System;

namespace Manager
{
    public class BlockManager : Singleton<BlockManager>
    {
        List<Block> blocks = new List<Block>();

        public Queue<Block> destroyBlockBuffer = new Queue<Block>();
        [SerializeField] Block blockPrefab;
        [SerializeField] int maxBlockCount;
        public event Action check;
        public int destroyCount;

        void Start()
        {
            check += AllFindNeighborBlock;
            check += AllFindEqualBlock;
            check += DestroyEqualBlock;
            check += AllVisitBlockInit;
            check += ChangeStateToReady;

            GenerateBlock(maxBlockCount);
        }
        public void InvokeCheck() => check?.Invoke();

        public void DestroyBlock(Block destroyBlock)
        {
            blocks.Remove(destroyBlock);
            destroyBlock.BoomParticle();
            Destroy(destroyBlock.gameObject);
        }

        public void DestroyEqualBlock()
        {
            Debug.Log("<color=red>삭제</color>");
            destroyCount = destroyBlockBuffer.Count;
            while (destroyBlockBuffer.Count != 0)
            {
                Block currentBlock = destroyBlockBuffer.Dequeue();
                if(currentBlock.isTop==0)
                    currentBlock.SpinTop();
                DestroyBlock(currentBlock);
            }
            if(destroyCount>0)
            {
                SoundManager.Instance.Destroy();
                GenerateBlock(destroyCount);
            }
        }

        public void ChangeStateToReady()
        {
            // State Change!!!
            if (destroyCount == 0)
                StateManager.Instance.state = State.Ready;
        }
        void GenerateBlock(int number) => StartCoroutine(CGenerateBlock(number));

        public void AllFindNeighborBlock()
        {
            Debug.Log("<color=green>인접 블록 찾기</color>");
            for (int i = 0; i < blocks.Count; i++)
                blocks[i].FindNeighborBlock();
        }

        void AllVisitBlockInit()
        {
            for (int i = 0; i < blocks.Count; i++)
                blocks[i].visited = false;
        }

        public void AllFindEqualBlock()
        {
            Debug.Log("<color=blue>같은 블록 찾기</color>");
            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].isTop > 0)
                    continue;

                blocks[i].FindEqualBlock(0, 3);
                blocks[i].FindEqualBlock(1, 4);
                blocks[i].FindEqualBlock(2, 5);
            }
        }

        IEnumerator CGenerateBlock(int number)
        {
            for (int i = 0; i < number; i++)
            {
                yield return new WaitForSeconds(0.1f);
                SoundManager.Instance.Generate();
                blocks.Add(Instantiate(blockPrefab));
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            InvokeCheck();
        }
    }
}