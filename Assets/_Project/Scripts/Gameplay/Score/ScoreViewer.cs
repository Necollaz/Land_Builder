using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class ScoreViewer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textScore;
   
   private CompositeDisposable _disposables = new ();
   private Score _score;
   
   [Inject]
   public void Construct(Score score)
   {
      _score = score;
   }

   private void Start()
   {
      _score.Value
         .Subscribe(Change)
         .AddTo(_disposables);
      
      Change(_score.Value.Value);
   }

   private void OnDestroy()
   {
      _disposables.Dispose();
   }

   private void Change(int value)
   {
      _textScore.text = "Score: " + value;
   }
}
