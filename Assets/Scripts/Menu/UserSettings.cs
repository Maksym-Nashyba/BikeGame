using UnityEngine;

namespace Menu
{
    public static class UserSettings
    {
        private const string _effectsKey = "effects";
        private const string _ambientKey = "music";
        private const string _graphicsKey = "graphics";

        public static float GetEffectsVolume()
        {
            return PlayerPrefs.GetFloat(_effectsKey, 1f);
        }

        public static void SetEffectsVolume(float value)
        {
            PlayerPrefs.SetFloat(_effectsKey, value);
            PlayerPrefs.Save();
        }
        
        public static float GetAmbientVolume()
        {
            return PlayerPrefs.GetFloat(_ambientKey, 1f);
        }

        public static void SetAmbientVolume(float value)
        {
            PlayerPrefs.SetFloat(_effectsKey, value);
            PlayerPrefs.Save();
        }

        public static GraphicsTier GetGraphicsTier()
        {
            return (GraphicsTier)PlayerPrefs.GetInt(_graphicsKey, 0);
        }

        public static void SetGraphicsTier(GraphicsTier graphicsTier)
        {
            PlayerPrefs.SetInt(_graphicsKey, (int)graphicsTier);
            PlayerPrefs.Save();
        }
        
        public enum GraphicsTier
        {
            Low,
            High
        }
    }
}