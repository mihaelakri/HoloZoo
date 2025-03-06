using System.Collections.Generic;
using UnityEngine;
using WPM;

namespace HoloZoo
{
    // This script is somewhat spliced from the 2 Demos of the WPM plugin:
    // 14 Virus Plague and 18 Highlight Continents
    public class WorldMapContinentsHighlight : MonoBehaviour
    {
        public Material circleMat, combineMat;
        public Texture2D earthMask;
        public WorldMapGlobe map;
        public Color oceanColor = new(0f, 0f, 1f, 0.33f), continentColor = new(0f, 1f, 0f, 0.33f);
        RenderTexture[] rtEarth, rtOceans, rtCombined;
        EarthTexture[] earthTextures;
        Material earthMat;
        readonly Dictionary<string, List<Country>> continents = new();

        void Start()
        {
            // Group countries by continent
            foreach (Country country in map.countries)
            {
                if (continents.ContainsKey(country.continent))
                {
                    continents[country.continent].Add(country);
                }
                else
                {
                    continents[country.continent] = new List<Country> { country };
                }
            }

            map.OnCountryEnter += OnCountryEnter;
            map.OnCountryExit += OnCountryExit;
            SetupOceanHighlight();
            UnhighlightOcean();
            HighlightOcean();
        }

        void SetupOceanHighlight()
        {
            earthMat = map.earthMaterial;
            earthTextures = map.earthTextures;
            int numTextures = earthTextures.Length;
            int width = earthTextures[0].texture.width;
            int height = earthTextures[0].texture.height;
            rtEarth = new RenderTexture[numTextures];
            rtOceans = new RenderTexture[numTextures];
            rtCombined = new RenderTexture[numTextures];
            for (int k = 0; k < numTextures; k++)
            {
                rtEarth[k] = new RenderTexture(width, height, 0);
                Graphics.Blit(earthTextures[k].texture, rtEarth[k]);
                rtOceans[k] = new RenderTexture(width, height, 0);
                rtCombined[k] = new RenderTexture(width, height, 0);
            }

            circleMat.SetTexture("_MaskTex", earthMask);
            circleMat.SetColor("_Color", oceanColor);
        }

        void HighlightOcean()
        {
            for (int k = 0; k < earthTextures.Length; k++)
            {
                Vector4 uvRect = earthTextures[k].uvRect;
                circleMat.SetVector("_UVRect", uvRect);
                rtOceans[k].Draw(circleMat);
            }
            ApplyTextures();
        }

        void UnhighlightOcean()
        {
            for (int k = 0; k < rtOceans.Length; k++)
            {
                rtOceans[k].Clear(false, true, Misc.ColorTransparent);
            }
            ApplyTextures();
        }

        void ApplyTextures()
        {
            for (int k = 0; k < rtOceans.Length; k++)
            {
                combineMat.SetTexture("_SecondTex", rtOceans[k]);
                Graphics.Blit(rtEarth[k], rtCombined[k], combineMat);
                earthMat.SetTexture(earthTextures[k].shaderTextureName, rtCombined[k]);
            }
        }

        void OnCountryEnter(int countryIndex, int regionIndex)
        {
            string continent = map.countries[countryIndex].continent;
            if (continents.TryGetValue(continent, out List<Country> continentCountries))
            {
                foreach (Country country in continentCountries)
                {
                    int cindex = map.GetCountryIndex(country.name);
                    map.ToggleCountrySurface(cindex, visible: true, continentColor);
                }
            }
            UnhighlightOcean();
        }

        void OnCountryExit(int countryIndex, int regionIndex)
        {
            string continent = map.countries[countryIndex].continent;
            if (continents.TryGetValue(continent, out List<Country> continentCountries))
            {
                foreach (Country country in continentCountries)
                {
                    int cindex = map.GetCountryIndex(country.name);
                    map.ToggleCountrySurface(cindex, visible: false);
                }
            }

            if (map.mouseIsOver)
                HighlightOcean();
        }
    }

    public static class RenderTextureExt
    {
        public static void Clear(this RenderTexture rt, bool clearDepth, bool clearColor, Color backgroundColor)
        {
            RenderTexture old = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(clearDepth, clearColor, backgroundColor);
            RenderTexture.active = old;
        }

        public static void Draw(this RenderTexture rt, Material mat)
        {
            RenderTexture old = RenderTexture.active;
            RenderTexture.active = rt;
            Graphics.Blit(null, rt, mat);
            RenderTexture.active = old;
        }
    }
}