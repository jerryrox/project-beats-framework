using PBFramework.Animations.Sections;

namespace PBFramework.Animations
{
    /// <summary>
    /// IAnime instance as seen by the internal components.
    /// </summary>
    public interface IAnimeEditor {

        /// <summary>
        /// Event called from ISection when it has finished building.
        /// </summary>
        void OnBuildSection(ISection section);
    }
}