using TMPro;
using UnityEngine;
using Zenject;

public class ScoreViewer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textScore;
   
   private Score _score;
   
   [Inject]
   public void Construct(Score score)
   {
      _score = score;
   }

   private void OnEnable()
   {
      _score.OnValueChange += Change;
   }

   private void Start()
   {
      _textScore.text = "Score: " + _score.Value;
   }

   private void OnDisable()
   {
      _score.OnValueChange -= Change;
   }

   private void Change(int value)
   {
      _textScore.text = "Score: " + value;
   }
}
