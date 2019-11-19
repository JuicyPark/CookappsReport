using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class StageManager : Singleton<StageManager>
    {
        public int moveCount = 10;
        public int topCount = 15;
        [SerializeField] Text moveText;
        [SerializeField] Text topText;
        [SerializeField] GameObject endPanel;
        [SerializeField] GameObject exitPanel;
        [SerializeField] Text endText;
        bool gameEnd;

        void Start()
        {
            Time.timeScale = 1;
        }

        void Update()
        {
            moveText.text = moveCount.ToString();
            topText.text = topCount.ToString();

            if (!gameEnd)
            {
                if (topCount <= 0)
                {
                    gameEnd = true;
                    Time.timeScale = 0;
                    SoundManager.Instance.Clear();
                    endPanel.SetActive(true);
                    endText.text = "CLEAR :D";
                }
                else if (moveCount <= 0)
                {
                    gameEnd = true;
                    Time.timeScale = 0;
                    SoundManager.Instance.Failed();
                    endPanel.SetActive(true);
                    endText.text = "FAILED :(";
                }
            }
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        
        public void OnExitPanel()
        {
            if(exitPanel.activeSelf==true)
            {
                exitPanel.SetActive(false);
            }
            else
            {
                exitPanel.SetActive(true);
            }
        }
        
        public void Exit()
        {
            Application.Quit();
        }
    }
}