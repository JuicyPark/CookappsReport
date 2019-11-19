using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blocks;

namespace Manager
{
    public class TouchManager : Singleton<TouchManager>
    {
        Vector2 startTouch;
        Vector2 endTouch;

        [SerializeField] float touchSenstive = 1000f;
        Block block;

        void Update()
        {
            if (StateManager.Instance.state.Equals(State.Ready))
                DragBlock();
            if (Input.GetKeyDown(KeyCode.A))
                BlockManager.Instance.InvokeCheck();
        }

        void DragBlock()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
                SelectBlock();
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (block == null)
                    return;

                endTouch = Input.GetTouch(0).position;
                Vector2 touchVector = endTouch - startTouch;

                if (touchVector.magnitude > touchSenstive)
                {
                    block.Swipe((Mathf.Atan2(touchVector.y, touchVector.x) * Mathf.Rad2Deg));
                    block = null;
                }
            }
        }
        void SelectBlock()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D check = Physics2D.Raycast(ray.origin, Vector2.up, 0.1f, LayerInformation.Instance.block);
            if (check)
            {
                if (check.collider != null)
                    block = check.transform.gameObject.GetComponent<Block>();
            }
        }
    }
}
