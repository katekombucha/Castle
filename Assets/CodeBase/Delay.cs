using System;
using System.Collections;
using UnityEngine;

namespace CodeBase
{
    public class Delay : MonoBehaviour
    {
        public static Delay Instance;

        private void Awake() => 
            Instance = this;

        public void Invoke(float delay, Action action) => 
            StartCoroutine(InvokeWithDelay(delay, action));

        private IEnumerator InvokeWithDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
