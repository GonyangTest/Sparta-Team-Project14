using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    [Serializable]
    internal class QuestData
    {
        public Dictionary<int, int> QuestProgress { get; set; } = new Dictionary<int, int>();

        public QuestData() { }

        // Quest 객체에서 데이터 추출
        public void FromQuest(Quest quest)
        {
            QuestProgress = new Dictionary<int, int>(quest.Get_questData());
        }

        // QuestData 객체를 Quest 객체에 적용
        public void ToQuest(Quest quest)
        {
            // 현재 진행 중인 퀘스트의 진행도 업데이트
            var questData = quest.Get_questData();
            questData.Clear();

            foreach (var pair in QuestProgress)
            {
                questData.Add(pair.Key, pair.Value);
            }
        }
    }
}
