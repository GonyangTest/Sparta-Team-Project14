using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
namespace TextRpg
{

    class Player
    {
        public string playerName;
        public string playerClass;
        public int level = GameConstance.Player.INITIAL_LEVEL;
        public int exp = GameConstance.Player.INITIAL_EXP;
        public int maxExp = GameConstance.Player.INITIAL_MAX_EXP;
        public int gold = GameConstance.Player.INITIAL_GOLD;
        public int hp;
        public int mana;
        public int maxHp;
        public int maxMp;
        public float power;
        public int defense;
        public int agility;
        public int criticalChance;
        public Job SelectedJob;

        public ConsumableItem HealthPotion = new ConsumableItem("체력포션", "체력을 회복하는 포션", 50, ConsumableItem.OptionType.Health, 30, 3);
        public ConsumableItem ManaPotion = new ConsumableItem("마나포션", "마나를 회복하는 포션", 100, ConsumableItem.OptionType.Mana, 30, 3);

        public Item EquippedWeapon;
        public Item EquippedArmor;

        public void SetPlayer()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("스파르타 던전에 오신 여러분 환영합니다.");
            var userName = AnsiConsole.Prompt(
                new TextPrompt<string>("[bold]원하시는 이름을 설정해주세요:[/]")
                .PromptStyle("white")
                .DefaultValue("[dim gray]예) 스파르타 전사[/]")
                .AllowEmpty());

            playerName = userName;
            List<string> userNameList = new List<string>()
            {
                "1. 저장",
                "2. 취소"
            };



            Console.Clear();
            AnsiConsole.MarkupLine($"입력하신 이름은 [bold]'{playerName}'[/]입니다.\n");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("원하시는 행동을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(userNameList)
                    .WrapAround());

            int index = int.Parse(choice.Split('.')[0]);


            switch (index)
            {
                case 1:
                    playerName = userName;
                    break;
                case 2:
                    Console.Clear();
                    break;
            }

            List<string> jobList = new List<string>()
            {
                "1. 전사",
                "2. 도적",
                "3. 궁수",
                "4. 마법사"
            };

            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 직업을 설정해주세요.\n");
            
            choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("원하시는 직업을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(jobList)
                    .WrapAround());

            index = int.Parse(choice.Split('.')[0]);

            SelectedJob = Job.JobList[index];
            // TODO: 나중에 구조 변경 필요
            playerClass = SelectedJob.Name;
            hp = SelectedJob.Hp;
            mana = SelectedJob.Mana;
            maxHp = SelectedJob.MaxHp;
            maxMp = SelectedJob.MaxMp;
            power = SelectedJob.Power;
            defense = SelectedJob.Defense;
            agility = SelectedJob.Agility;
            criticalChance = SelectedJob.CriticalChance;

            // 새로 시작할 때 퀘스트 데이터 초기화 필요
            Program.quest.InitQuestData();
        }

        public bool UseHealthPotion()
        {
            if (hp >= maxHp)
            {
                Console.WriteLine("\n이미 최대체력입니다.");
                return false;
            }

            return HealthPotion.Use(this);
        }

        // 마나 포션 사용
        public bool UseManaPotion()
        {
            if (mana >= maxMp)
            {
                Console.WriteLine("\n이미 최대마나입니다.");
                return false;
            }

            return ManaPotion.Use(this);
        }

        public void Hit(int hp_decrease)
        {
            hp = Math.Clamp(hp - hp_decrease, 0, maxHp);
        }

        public void AddExp(int expAmount)
        {
            exp += expAmount; // 매개변수만큼 경험치를 더하고
            LevelUp(); // 레벨업 체크
        }

        // 레벨업시 변화
        public void LevelUp()
        {
            while (exp >= maxExp)
            {
                level++;
                exp -= maxExp;

                // 필요 경험치 증가량 로직 생성
                int increase = 25 + (level - 2) * 5;
                maxExp += increase;

                // 레벨업시 공격력 0.5증가 방어력 1증가
                power += GameConstance.Player.LEVEL_UP_POWER_INCREASE;
                defense += GameConstance.Player.LEVEL_UP_DEFENSE_INCREASE;

                // 레벨 달성 퀘스트 판정
                Program.quest.QuestRenewal(2, level);

                // 레벨업 메세지
                AnsiConsole.MarkupLine($"[yellow]{playerName}[/] 의 레벨이 [yellow]{level}[/] 로 올랐습니다.");
            }
        }
        public float totalPower
        {
            get
            {
                float weaponBonus = EquippedWeapon is Weapon w ? w.attack : 0;
                return power + weaponBonus;
            }
        }

        public int totalDefense
        {
            get
            {
                int armorBonus = EquippedArmor is Armor a ? a.defense : 0;
                return defense + armorBonus;
            }
        }
        //스킬 데미지
        public int SkillPower(Skill skill)
        {
            int skillPower = (int)(totalPower * skill.PowerMultiplier);
            return skillPower;
        }
        //크리티컬 데미지
        public int CriticalDamage()
        {
            int criticalDamage = (int)(totalPower * GameConstance.Player.CRITICAL_DAMAGE_MULTIPLIER);
            return criticalDamage;
        }

        public string PrintPlayer()
        {
                Console.WriteLine("상태보기\n");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                return $"[플레이어 정보]\n" +
                $"이름: {playerName}\n" +
                $"직업: {playerClass}\n" +
                $"레벨: {level}\n" +
                $"경험치: {exp}/{maxExp}\n" +
                $"체력: {hp}/{maxHp}\n" +
                $"마력: {mana}/{maxMp}\n" +
                $"공격력: {totalPower}\n" +
                $"방어력: {totalDefense}\n" +
                $"민첩: {agility}\n" +
                $"치명타확률: {criticalChance}\n" +
                $"장착 무기: {(EquippedWeapon != null ? EquippedWeapon.itemName : "없음")}\n" +
                $"장착 방어구: {(EquippedArmor != null ? EquippedArmor.itemName : "없음")}\n" +
                $"골드 : {gold} G\n" +
                $"[스킬목록]\n{string.Join("\n", SkillFactory.GetSkillsByJob(playerClass).Select(skill => skill.ToString()))}";
        }
    }
}
