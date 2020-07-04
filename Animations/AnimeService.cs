using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Animations;

namespace PBFramework.Animations
{
    /// <summary>
    /// Provides IAnime instances the ability to update and mutate its internal states.
    /// </summary>
    public class AnimeService : MonoBehaviour
    {
        private static AnimeService I;

        /// <summary>
        /// The list of all anime instances to be updated.
        /// </summary>
        private List<IAnime> animes = new List<IAnime>();


        private static AnimeService Instance => I ?? (I = new GameObject("_AnimeService").AddComponent<AnimeService>());


        /// <summary>
        /// Manually initializes the servicer instance.
        /// </summary>
        public static void Initialize()
        {
            if (Instance != null) return;
        }

        /// <summary>
        /// Adds the specified anime instance from update service.
        /// </summary>
        public static void AddAnime(IAnime anime)
        {
            Instance.animes.Add(anime);
        }

        /// <summary>
        /// Removes the specified anime instance from update service.
        /// </summary>
        public static void RemoveAnime(IAnime anime)
        {
            var animes = Instance.animes;
            for (int i = 0; i < animes.Count; i++)
            {
                if (animes[i] == anime)
                {
                    animes[i] = null;
                    break;
                }
            }
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < animes.Count; i++)
            {
                var anime = animes[i];

                // Update animation
                if (anime != null)
                {
                    // Continue if not finished yet.
                    if(anime.Update(deltaTime))
                        continue;
                }

                // Animation is either finished or removed.
                animes.RemoveAt(i);
                i--;
            }
        }
    }
}