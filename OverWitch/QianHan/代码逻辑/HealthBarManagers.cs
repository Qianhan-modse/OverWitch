using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.lang.logine;
using UnityEngine;
using UnityEngine.UI;

namespace HealthManager
{
    [RequireComponent(typeof(Image))]
    public class HealthBarManagers : MonoBehaviour
    {
        public EntityLivingBase target;
        public Slider healthSlider;
        public float animatorSpeed=3.0F;
        private float currentDisplay;
        private void Start()
        {
            currentDisplay = target.getHealth();
            healthSlider.maxValue = target.getMaxHealth();
            healthSlider.value = currentDisplay;
        }
        public void Update()
        {
            float currentHealth = target.getHealth();
            currentDisplay = Super.Clamped(Mathf.Lerp(currentDisplay, currentHealth, Time.deltaTime * animatorSpeed),0f,target.getMaxHealth());
            healthSlider.value = currentDisplay;

        }
    }
}
