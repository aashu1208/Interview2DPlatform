using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;
        public int totalCoins;  // New variable to store the total number of coins

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;
        public PlayerController pc;
        public GameObject gameOverPanal;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();

            // Set the total number of coins based on your level design
            totalCoins = CalculateTotalCoins();  // Implement a method to calculate or set the total number of coins
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString();
            if (player.deathState == true)
            {
                //pc.PlaySound(pc.deathSound);
                Invoke("GameOver", 2.0f);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);

                player.deathState = false;
                Invoke("GameOver", 3.5f);
                //Time.timeScale = 1;
            }
        }

        private void GameOver()
        {
            //Application.LoadLevel(Application.loadedLevel);
            //Time.timeScale = 0;
            PauseGame();
            gameOverPanal.SetActive(true);
        }

        public void GameOverPanalActiveFalse()
        {
            gameOverPanal.SetActive(false);
        }

        public void Level1Reload()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level1");
        }

        public void Level2Reload()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level2");
        }

        public void Level3Relaod()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level3");
        }

        // Example method to calculate or set the total number of coins
        private int CalculateTotalCoins()
        {
            int totalCoins = 0;

            // Find all objects with the "Coin" tag in the scene
            GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");

            // Count the number of coins
            totalCoins = coinObjects.Length;

            return totalCoins;
        }

        private void PauseGame()
        {
            Time.timeScale = 0;  // Pause the game
        }

    }
}
