using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terraria;
using Terraria.ID;
using Terraria.IO;

namespace BannerCollector
{
    internal static class PlayerAssist
    {
        // 데이터 저장
        public static void SavePlayerBannerData()
        {
            Player player = Main.LocalPlayer; // 현재 로컬 플레이어 가져오기
            string playerName = player.name; // 플레이어 이름 가져오기
            string path = GetUserDataFilePath(playerName);
            string jsonData = File.ReadAllText(path);
            List<BannerInfo> jsonBannerList = JsonConvert.DeserializeObject<List<BannerInfo>>(jsonData);
            List<BannerInfo> saveBannerList = BannerLoad.BannerDict.Values.ToList();

            //저장할 리스트 jason에 덮어쓰기
            for (int i = 0; i < saveBannerList.Count; i++)
            {
                int index = jsonBannerList.FindIndex(banner => banner.ItemId == saveBannerList[i].ItemId);
                if (index == -1) //json에 없던 데이터 이므로 새로 추가
                {
                    jsonBannerList.Add(saveBannerList[i]);
                }
                else
                {
                    jsonBannerList[index] = saveBannerList[i];
                }
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(jsonBannerList, Formatting.Indented));
        }

        //새로운 플레이어의 경우 파일을 생성하고 bannerCount가 모두 0 인 리스트 json에 넣으려고 만든 메서드
        public static void SavePlayerBannerDefaultData(string filePath)
        {
            List<BannerInfo> bannerList = BannerLoad.BannerDict.Values.ToList();
            // 리스트를 JSON 형식으로 직렬화
            string jsonData = JsonConvert.SerializeObject(bannerList, Formatting.Indented);

            // JSON 데이터를 파일에 쓰기
            File.WriteAllText(filePath, jsonData);
        }

        public static void LoadPlayerBannerData()
        {
            Player player = Main.LocalPlayer; // 현재 로컬 플레이어 가져오기
            string playerName = player.name; // 플레이어 이름 가져오기
            string filePath = GetUserDataFilePath(playerName);
            //파일이 존재 하면 데이터 불러오기
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                List<BannerInfo> bannerList = JsonConvert.DeserializeObject<List<BannerInfo>>(jsonData);

                //json에서 불러온 리스트 BannerLoad.BannerDict에 넣기
                BannerLoad.LoadBanners();
                foreach (BannerInfo banner in bannerList)
                {
                    // Restore only the collected count from the save. All static metadata
                    // (Index, ModName, ItemName, IsHardMode, UseItemIcon) is always taken
                    // from code so that mod updates (e.g. Calamity reordering its banner
                    // atlas) can never desync banners loaded from an older save file.
                    if (BannerLoad.BannerDict.ContainsKey(banner.ItemId))
                        BannerLoad.BannerDict[banner.ItemId].BannerCount = banner.BannerCount;
                }
            }
            else //파일 없으면(새로운 플레이어인 경우) 파일 생성하고 디폴트값 넣기
            {
                // 디렉토리 생성
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath); // 경로가 없으면 생성
                }
                File.Create(filePath).Close();
                SavePlayerBannerDefaultData(filePath);


            }

        }
        private static string GetUserDataFilePath(string playerName)
        {
            // Documents 폴더 경로 가져오기
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            // 유저 데이터 경로 생성
            string userDataPath = Path.Combine(documentsPath, "My Games", "Terraria", "tModLoader", "Players", "BannerData", playerName + ".json");

            return userDataPath;
        }
    }
}
