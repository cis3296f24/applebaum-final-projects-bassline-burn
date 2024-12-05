using Fusion;
using FusionExamples.Utility;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Managers
{
    public class LevelManager : NetworkSceneManagerDefault
    {
        public const int LAUNCH_SCENE = 0;
        public const int LOBBY_SCENE = 1;
        public const int LEADERBOARD_SCENE = 2; // Add a constant for the leaderboard scene

        [SerializeField] private UIScreen _dummyScreen;
        [SerializeField] private UIScreen _lobbyScreen;
        [SerializeField] private CanvasFader fader;

        public static LevelManager Instance => Singleton<LevelManager>.Instance;

        public static void LoadMenu()
        {
            Instance.Runner.LoadScene(SceneRef.FromIndex(LOBBY_SCENE));
        }

        public static void LoadTrack(int sceneIndex)
        {
            Instance.Runner.LoadScene(SceneRef.FromIndex(sceneIndex));
        }

        public static void LoadLeaderboard()
        {
            Instance.Runner.LoadScene(SceneRef.FromIndex(LEADERBOARD_SCENE)); // Load the leaderboard scene
        }

        protected override System.Collections.IEnumerator LoadSceneCoroutine(SceneRef sceneRef, NetworkLoadSceneParameters sceneParams)
        {
            Debug.Log($"Loading scene {sceneRef}");

            PreLoadScene(sceneRef.AsIndex);

            yield return base.LoadSceneCoroutine(sceneRef, sceneParams);

            // Delay one frame, so we're sure level objects have spawned locally
            yield return null;

            // Handle spawning or special logic for tracks
            if (GameManager.CurrentTrack != null && sceneRef.AsIndex > LOBBY_SCENE)
            {
                if (Runner.GameMode == GameMode.Host)
                {
                    foreach (var player in RoomPlayer.Players)
                    {
                        player.GameState = RoomPlayer.EGameState.GameCutscene;
                        GameManager.CurrentTrack.SpawnPlayer(Runner, player);
                    }
                }
            }

            PostLoadScene();
        }

        private void PreLoadScene(int scene)
        {
            if (scene > LOBBY_SCENE)
            {
                Debug.Log("Showing Dummy");
                UIScreen.Focus(_dummyScreen);
            }
            else if (scene == LOBBY_SCENE)
            {
                foreach (RoomPlayer player in RoomPlayer.Players)
                {
                    player.IsReady = false;
                }
                UIScreen.activeScreen.BackTo(_lobbyScreen);
            }
            else
            {
                UIScreen.BackToInitial();
            }
            fader.gameObject.SetActive(true);
            fader.FadeIn();
        }

        private void PostLoadScene()
        {
            fader.FadeOut();
        }

        public void LocalPlay()
        {
            SceneManager.LoadScene("Local");
        }

        public void EndRaceAndShowLeaderboard(List<PlayerResult> raceResults)
        {
            // Pass race results to a static storage or directly to the leaderboard UI
            RaceResults.results = raceResults;
            LoadLeaderboard(); // Load leaderboard scene
        }
    }
}
