using UnityEngine;
using TMPro;
using UniRx;
using Zenject;

public class ScoreViewer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textScore;
   
   private readonly CompositeDisposable disposables = new ();
   
   private Score _score;
   
   [Inject]
   public void Construct(Score score)
   {
      _score = score;
   }

   private void Start()
   {
      if (_score == null)
      {
         Debug.LogError($"{nameof(ScoreViewer)}: Score is not injected. Subscription skipped.");
         return;
      }
      
      _score.Value.Subscribe(Change).AddTo(disposables);
      
      Change(_score.Value.Value);
   }

   private void OnDestroy()
   {
      disposables.Dispose();
   }

   private void Change(int value)
   {
      _textScore.text = value.ToString();
   }
}