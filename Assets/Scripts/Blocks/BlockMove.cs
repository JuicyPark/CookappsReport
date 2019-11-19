using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Blocks
{
    public class BlockMove : MonoBehaviour
    {
        [SerializeField] Block block;

        Transform midTransform;
        Transform rightTransform;
        Transform leftTransform;
        Frame nextFrame;
        public Frame currentFrame;
        bool isMove;

        void Update()
        {
            if (StateManager.Instance.state.Equals(State.Generate))
                ApplyGravity();
        }

        public bool IsEmptyTransform(Transform targetTransform)
        {
            RaycastHit2D hit = Physics2D.Raycast(targetTransform.position, Vector2.up, 0.1f, LayerInformation.Instance.frame);
            if (!hit)
                return false;
            nextFrame = hit.transform.GetComponent<Frame>();

            if (!nextFrame)
                return false;

            if (!nextFrame.block)
                return true;
            return false;
        }
        public void ApplyDownTransform(Transform mid, Transform right, Transform left)
        {
            midTransform = mid;
            rightTransform = right;
            leftTransform = left;
        }
        public void ApplyGravity()
        {
            if (isMove)
                return;
            if (IsEmptyTransform(midTransform))
            {
                if (currentFrame)
                    currentFrame.block = null;
                currentFrame = nextFrame;
                currentFrame.block = block;
                StartCoroutine(CMove(0, -0.2f));
            }
            else if (IsEmptyTransform(rightTransform))
            {
                if (currentFrame)
                    currentFrame.block = null;
                currentFrame = nextFrame;
                currentFrame.block = block;
                StartCoroutine(CMove(0.15f, -0.1f));
            }
            else if (IsEmptyTransform(leftTransform))
            {
                if (currentFrame)
                    currentFrame.block = null;
                currentFrame = nextFrame;
                currentFrame.block = block;
                StartCoroutine(CMove(-0.15f, -0.1f));
            }
        }
        public void Boom() => currentFrame.ParticleBoom();
        public void MoveToIndex(int index)
        {
            if (index == 0)
                StartCoroutine(CMove(0, 0.2f));
            else if (index == 1)
                StartCoroutine(CMove(0.15f, 0.1f));
            else if (index == 2)
                StartCoroutine(CMove(0.15f, -0.1f));
            else if (index == 3)
                StartCoroutine(CMove(0, -0.2f));
            else if (index == 4)
                StartCoroutine(CMove(-0.15f, -0.1f));
            else if (index == 5)
                StartCoroutine(CMove(-0.15f, 0.1f));
        }
        IEnumerator CMove(float x, float y)
        {
            isMove = true;
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.01f);
                transform.Translate(x, y, 0);
            }
            isMove = false;
            ApplyGravity();
        }
    }
}
