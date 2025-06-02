using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
        [Header("Survival Settings")]
        public float dayLength = 120f; // seconds per day
        public bool autoConsumeDailyFood = true;

        private float dayTimer = 0f;
        private int currentDay = 1;
        private InventoryManager inventory;

        void Start()
        {
            inventory = InventoryManager.Instance;
        }

        void Update()
        {
            if (inventory == null) return;

            dayTimer += Time.deltaTime;

            if (dayTimer >= dayLength)
            {
                dayTimer = 0f;
                currentDay++;

                inventory.ShowStatusMessage($"Day {currentDay} begins");

                if (autoConsumeDailyFood)
                {
                    StartCoroutine(DelayedFoodConsumption());
                }
            }
        }

        private IEnumerator DelayedFoodConsumption()
        {
            yield return new WaitForSeconds(2f); // Wait a bit after day message
            inventory.ConsumeDailyRations();
        }

        public int GetCurrentDay() => currentDay;
        public float GetDayProgress() => dayTimer / dayLength;
    }
