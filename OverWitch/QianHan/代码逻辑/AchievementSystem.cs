using System;
using System.Collections.Generic;
using Achievements;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Usite.AchievementSystem;

namespace Usite 
{
    public class AchievementSystem : MonoBehaviour
    {
        [Serializable]
        public class Achievement
        {
            public string id;
            public string displyName;
            public string description;
            public bool isUnlocked;
            public bool isOntTime;
            public Sprite icon;
            public Achievement(string id, string name, string desc, bool oneTime = false)
            {
                this.id = id.ToLower();
                this.displyName = name;
                this.description = desc;
                this.isUnlocked = false;
                this.isOntTime = oneTime;
            }
            private static AchievementSystem instance;
            private Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
            private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
            {
                if (scene.name == "主界面")
                {
                    autoUnlockPresetAchievements();
                }
            }
            private void autoUnlockPresetAchievements()
            {
                string[] presetAchievements = { " Rain Bow Sea", " Star Travels - Decisive War Age ", " Time Will Give The Answer " };
                foreach (string id in presetAchievements)
                {
                    if (achievements.TryGetValue(id, out Achievement ach))
                    {
                        if (!ach.isUnlocked || !ach.isOntTime)
                        {
                            UnlockAchievement(id);
                        }
                    }
                }
            }
            public void UnlockAchievement(string achievementId)
            {
                achievementId = achievementId.ToLower();
                if (achievements.TryGetValue(achievementId, out Achievement achievement))
                {
                    if (achievement.isUnlocked && achievement.isOntTime)
                    {
                        Debug.Log($"成就{achievement.displyName}为一次性成就，不可重复解锁");
                        return;
                    }
                    achievement.isUnlocked = true;
                    onAchievementUnlocked(achievement);
                }
            }
            private void onAchievementUnlocked(Achievement achievement)
            {
                Debug.Log($"成就解锁：{achievement.displyName}");
                AchievementEventBus.publish(new AchievementUnlockedEvent(achievement));
            }
        }
    }
}
namespace Achievements
{
    public static class AchievementEventBus
    {
        private static readonly Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();
        public static void publish<T>(T eventInstance)where T : class
        {
            Type type=typeof(T);
            if(eventListeners.TryGetValue(type,out var listeners))
            {
                foreach(var listener in listeners.ToArray())
                {
                    listener?.Invoke(eventInstance);
                }
            }
        }
    }
    public class AchievementUnlockedEvent
    {
        public Achievement Achievement { get; }
        public AchievementUnlockedEvent(Achievement achievement)
        {
            this.Achievement = achievement;
        }
    }
}
