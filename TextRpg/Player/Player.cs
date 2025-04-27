using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Spectre.Console;
namespace TextRpg
{

    public class Player
    {
        public string playerName;
        public string playerClass;
        public int level = GameConstance.Player.INITIAL_LEVEL;
        public double exp = GameConstance.Player.INITIAL_EXP;
        public int maxExp = GameConstance.Player.INITIAL_MAX_EXP;
        public int gold = GameConstance.Player.INITIAL_GOLD;
        
        // 스탯
        public int hp;
        public int mana;
        public int maxHp;
        public int maxMp;
        public float power;
        public int defense;
        public int agility;
        public int criticalChance;
        
        // 직업 및 진행 정보
        public Job SelectedJob;
        public int highestClearedStage = 0; // 최고 스테이지 저장
        
        // 아이템
        public ConsumableItem HealthPotion = ItemFactory.CreateConsumableItem(GameConstance.Item.HEALTH_POTION_NAME, 3);
        public ConsumableItem ManaPotion = ItemFactory.CreateConsumableItem(GameConstance.Item.MANA_POTION_NAME, 3);
        
        public Item EquippedWeapon;
        public Item EquippedArmor;

        public string JobName // TODO: job 이름 중복 사용으로 playerName 제거 필요
        {
            get { return SelectedJob.Name; }
        }
        
        // 계산된 속성 - 무기 보너스를 포함한 총 공격력
        public float totalPower
        {
            get
            {
                float weaponBonus = EquippedWeapon is Weapon w ? w.Attack : 0;
                return power + weaponBonus;
            }
        }

        // 계산된 속성 - 방어구 보너스를 포함한 총 방어력
        public int totalDefense
        {
            get
            {
                int armorBonus = EquippedArmor is Armor a ? a.Defense : 0;
                return defense + armorBonus;
            }
        }

        public void SetPlayer()
        {
            List<string> playerNameSaveMenu = new List<string>() { "1. 저장", "2. 취소" };
            playerName = DecidePlayerName();
            
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"입력하신 이름은 [bold]'{playerName}'[/]입니다.\n");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("원하시는 행동을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(playerNameSaveMenu)
                    .WrapAround());

            int index = int.Parse(choice.Split('.')[0]);

            switch (index)
            {
                case 1:
                    break;
                case 2:
                    Console.Clear();
                    playerName = DecidePlayerName(); // 취소 시 다시 이름 설정
                    break;
            }
            
            SelectJob();

            // 새로 시작할 때 퀘스트 데이터 초기화 필요
            Program.quest.InitQuestData();
        }

        private string DecidePlayerName() {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("스파르타 던전에 오신 여러분 환영합니다.");
            var name = AnsiConsole.Prompt(
                new TextPrompt<string>("[bold]원하시는 이름을 설정해주세요:[/]")
                .PromptStyle("white")
                .DefaultValue("[dim gray]예) 스파르타 전사[/]")
                .AllowEmpty());
            
            return string.IsNullOrEmpty(name) ? "스파르타 전사" : name;
        }

        // 직업 선택
        private void SelectJob()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("스파르타 던전에 오신 여러분 환영합니다.");
            AnsiConsole.MarkupLine("원하시는 직업을 설정해주세요.\n");

            // JobType 목록 가져오기
            List<JobType> jobTypes = Job.GetJobTypes();
            
            // 선택 프롬프트 생성
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<JobType>()
                    .Title("원하시는 직업을 선택해주세요.")
                    .PageSize(10)
                    .AddChoices(jobTypes)
                    .UseConverter(jobType => {
                        // 직업 이름 가져오기 (Description 어트리뷰트 사용)
                        string jobName = EnumUtils.GetDescription(jobType);
                        // 인덱스 계산
                        int index = Array.IndexOf(Enum.GetValues(typeof(JobType)), jobType) + 1;
                        return $"{index}. {jobName}";
                    }) 
                    .WrapAround());

            // 해당 JobType으로 Job 할당
            SelectedJob = Job.JobList[selection];
            
            InitializeJobStats();
        }

        // 직업 기반 캐릭터 스탯 초기화
        private void InitializeJobStats()
        {
            if (SelectedJob == null) return;

            playerClass = SelectedJob.Name;
            hp = SelectedJob.Hp;
            mana = SelectedJob.Mana;
            maxHp = SelectedJob.Hp;
            maxMp = SelectedJob.Mana;
            power = SelectedJob.Power;
            defense = SelectedJob.Defense;
            agility = SelectedJob.Agility;
            criticalChance = SelectedJob.CriticalChance;
        }

        // 체력 포션 사용
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

        // 데미지를 받았을 때 체력 감소
        public void Hit(int hp_decrease)
        {
            hp = Math.Clamp(hp - hp_decrease, 0, maxHp);
        }

        // 경험치 획득
        public void AddExp(double expAmount)
        {
            exp += expAmount;
            LevelUp(); // 레벨업 체크
        }

        // 레벨업시 변화
        public void LevelUp()
        {
            while (exp >= maxExp)
            {
                level++;
                exp -= maxExp;

                // 필요 경험치 증가량 로직 계산
                int increase = 25 + (level - 2) * 5;
                maxExp += increase;

                // 레벨업 시 능력치 증가
                ApplyLevelUpBonuses();

                // 레벨 달성 퀘스트 판정
                Program.quest.QuestRenewal(2, level);

                // 레벨업 메세지
                AnsiConsole.MarkupLine($"[yellow]{playerName}[/] 의 레벨이 [yellow]{level}[/] 로 올랐습니다.");
            }
        }

        // 레벨업 시 능력치 증가 적용
        private void ApplyLevelUpBonuses()
        {
            power += GameConstance.Player.LEVEL_UP_POWER_INCREASE;
            defense += GameConstance.Player.LEVEL_UP_DEFENSE_INCREASE;
        }

        // 스킬 데미지 계산
        public int SkillPower(Skill skill)
        {
            return (int)(totalPower * skill.PowerMultiplier);
        }
        
        // 크리티컬 데미지 계산
        public int CriticalDamage()
        {
            return (int)(totalPower * GameConstance.Player.CRITICAL_DAMAGE_MULTIPLIER);
        }

        public string PrintPlayer()
        {
            AnsiConsole.Write(new Rule("[green]캐릭터의 정보를 볼수 있는 상태창입니다[/]"));

            // 테이블 생성 및 설정
            var statTable = new Table();
                statTable.Border = TableBorder.Rounded;
                
                // 테이블 컬럼 추가
                statTable.AddColumn(new TableColumn("[yellow]특성[/]").Centered());
                statTable.AddColumn(new TableColumn("[yellow]수치[/]").Centered());
                
                // 테이블에 행 추가
                statTable.AddRow("[white]이름[/]", $"{playerName}");
                statTable.AddRow("[white]직업[/]", $"{playerClass}");
                statTable.AddRow("[white]레벨[/]", $"{level}");
                statTable.AddRow("[white]경험치[/]", $"{(int)exp}/{maxExp}");
                statTable.AddRow("[white]체력[/]", $"{hp}/{maxHp}");
                statTable.AddRow("[white]마력[/]", $"{mana}/{maxMp}");
                statTable.AddRow("[white]공격력[/]", $"{totalPower}");
                statTable.AddRow("[white]방어력[/]", $"{totalDefense}");
                statTable.AddRow("[white]민첩[/]", $"{agility}");
                statTable.AddRow("[white]치명타확률[/]", $"{criticalChance}%");
                statTable.AddRow("[white]골드[/]", $"{gold} G");
                statTable.AddRow("[white]최고 스테이지[/]",$"{highestClearedStage}단계");
                AnsiConsole.Write(statTable);

                // 장비 테이블 생성
                var equipmentTable = new Table();
                equipmentTable.Border = TableBorder.Rounded;

                // 장비 테이블 컬럼 추가
                equipmentTable.AddColumn(new TableColumn("[yellow]장비 이름[/]").Centered());
                equipmentTable.AddColumn(new TableColumn("[yellow]장비 타입[/]").Centered());
                
                // 장비 테이블에 행 추가
                equipmentTable.AddRow("[cyan]장착 무기[/]", $"{(EquippedWeapon != null ? EquippedWeapon.ItemName : "없음")}");
                equipmentTable.AddRow("[cyan]장착 방어구[/]", $"{(EquippedArmor != null ? EquippedArmor.ItemName : "없음")}");
                AnsiConsole.Write(equipmentTable);
                
                var skillTable = new Table();
                skillTable.Border = TableBorder.Rounded;
                
                // 스킬 테이블 컬럼 추가
                skillTable.AddColumn(new TableColumn("[yellow]스킬 이름[/]").Centered());
                skillTable.AddColumn(new TableColumn("[yellow]데미지 배율[/]").Centered());
                skillTable.AddColumn(new TableColumn("[yellow]마나 소모량[/]").Centered());
                skillTable.AddColumn(new TableColumn("[yellow]유형[/]").Centered());

                var skills = SkillFactory.GetSkillsByJob(playerClass);
                foreach (var skill in skills)
                {
                    skillTable.AddRow($"[cyan]{skill.Name}[/]", $"{skill.PowerMultiplier}x", $"{skill.ManaCost}", $"{skill.Type}");
                }
                AnsiConsole.Write(skillTable);
                return "";
        }
    }
}
