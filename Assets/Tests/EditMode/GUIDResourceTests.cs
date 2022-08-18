using System.Linq;
using IGUIDResources;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class GUIDResourceTests
    {
        [Test]
        public void AllValidGUIDs()
        {
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            foreach (BikeModel bikeModel in resources.Bikes.Models)
            {
                foreach (Skin skin in bikeModel.AllSkins)
                {
                    Assert.True(GUIDs.IsValidGUID(skin.GetGUID()), $"Invalid skin guid: {skin.GetGUID()}, for bike: {bikeModel.GetGUID()}");
                }
                Assert.True(GUIDs.IsValidGUID(bikeModel.GetGUID()));
                foreach (Level level in resources.Career)
                {
                    if(level is null) continue;
                    Assert.True(GUIDs.IsValidGUID(level.GetGUID()));
                }
            }
        }

        [Test]
        public void AllSkinsUsed()
        {
            Skin[] allSkins = Resources.LoadAll<Skin>("GUIDResources/Bikes");
            BikeModel[] bikeModels = Resources.FindObjectsOfTypeAll<BikeModel>();
            foreach (Skin skin in allSkins)
            {
                Assert.True(SkinUsed(skin), $"Skin: {skin.GetGUID()} is never used");
            }
            
            bool SkinUsed(Skin skin)
            {
                foreach (BikeModel bike in bikeModels)
                {
                    if (bike.AllSkins.Contains(skin)) return true;
                }
                return false;
            }
        }

        [Test]
        public void AllBikesUsed()
        {
            BikeModel[] bikeModels = Resources.LoadAll<BikeModel>("GUIDResources/Bikes");
            BikeModels modelList = GUIDResourceLocator.Initialize().Bikes;
            foreach (BikeModel model in bikeModels)
            {
                Assert.Contains(model, modelList.Models, $"Bike: {model.GetGUID()} is never used");
            }
        }
        
        [Test]
        public void AllLevelsUsed()
        {
            Level[] levels = Resources.LoadAll<Level>("GUIDResources/Career");
            Career career = GUIDResourceLocator.Initialize().Career;
            if(levels is null || levels.Length < 1) Assert.Fail("No levels found");
            foreach (Level level in levels)
            {
                if(!LevelUsed(level)) Assert.Fail( $"Level: {level.GetGUID()} is never used");
            }
            Assert.Pass();

            bool LevelUsed(Level searchedLevel)
            {
                foreach (Level careerLevel in career)
                {
                    if (searchedLevel == careerLevel) return true;
                }

                return false;
            }
        }

        [Test]
        public void AllChaptersFilled()
        {
            Chapter[] chapters = GUIDResourceLocator.Initialize().Career.Chapters;
            foreach (Chapter chapter in chapters)
            {
                Assert.True(IsFilled(chapter), $"Chapter: {chapter.name} is not filled");
            }
            
            bool IsFilled(Chapter chapter)
            {
                for (int i = 0; i < chapters.Length; i++)
                {
                    if (chapter[i] is null) return false;
                }

                return true;
            }
        }

        [Test]
        public void AllBikesHaveAtLeastOneSkin()
        {
            BikeModel[] bikeModels = Resources.LoadAll<BikeModel>("GUIDResources/Bikes");
            foreach (BikeModel bikeModel in bikeModels)
            {
                if (bikeModel.AllSkins is null || bikeModel.AllSkins.Length < 1) Assert.Fail();
            }
        }
    }
}
