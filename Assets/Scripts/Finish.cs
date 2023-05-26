using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject finishPanel;
    private int counter = 0;
    public Dictionary<int, string> leaderBoard = new Dictionary<int, string>();
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PathFollower>(out PathFollower bot) || other.gameObject.TryGetComponent<P_Controller>(out P_Controller player))
        {
            counter++;
            leaderBoard.Add(counter, other.gameObject.name);
            if (other.gameObject.TryGetComponent<P_Controller>(out P_Controller playerNow))
            {
                finishPanel.SetActive(true);
                string output = "";
                foreach (var pair in leaderBoard)
                {
                    output += pair.Key + "место: " + pair.Value + "\n";
                }
                finishPanel.GetComponentInChildren<TextMeshProUGUI>().text = output;
                Time.timeScale = 0;
            }
        }
    }
}
