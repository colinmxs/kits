using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{   
    [SerializeField] private int totalHealth = 100;    
    [SerializeField] private int cooldownPeriod = 2;

    [SerializeField]
    public int remainingHealth
    {
        get
        {
            return _remainingHealth;
        }
        set
        {
            if (!_lockRemainingHealth)
            {
                if (value == 0) OnDeath.Invoke();
                else if (value < _remainingHealth)
                {
                    _remainingHealth = value;
                    StartCoroutine(LockRemainingHealth());
                }
            }
        }
    }
    private int _remainingHealth;

    public UnityEvent OnDeath = new UnityEvent();
    public Canvas HealthCanvas;
    private bool _lockRemainingHealth = false;

    private void Awake()
    {
        _remainingHealth = totalHealth;
        var texts = HealthCanvas.GetComponentsInChildren<Text>();
        texts.Single(t => t.name == "AvailableHealth").text = string.Concat(Enumerable.Repeat("|", remainingHealth));
        texts.Single(t => t.name == "TotalHealth").text = string.Concat(Enumerable.Repeat("|", totalHealth));
    }

    private void Update()
    {
        var texts = HealthCanvas.GetComponentsInChildren<Text>();
        texts.Single(t => t.name == "AvailableHealth").text = string.Concat(Enumerable.Repeat("|", remainingHealth));        
    }

    public IEnumerator LockRemainingHealth()
    {
        _lockRemainingHealth = true;
        yield return new WaitForSeconds(cooldownPeriod);
        _lockRemainingHealth = false;
    }
}
