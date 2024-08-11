using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using AnimalSimulation.Tools;

namespace AnimalSimulation.UI
{
    [RequireComponent(typeof(Animator))]
    public class GameWindow : MonoBehaviour
    {
        private static readonly int VISIBLE = Animator.StringToHash("Visible");
        private const int SHOW_HIDE_DELAY = 500;

        [SerializeField] private Animator _animator = null;
        [Inject] private DiContainer _container;
        [Inject] private Canvas _mainCanvas;

        protected virtual void OnEnable()
        {
            if (_animator != null && _animator.HasParameter(VISIBLE))
            {
                _animator.SetBool(VISIBLE, true);
            }
        }

        protected async Task HideAndOpen(GameObject prefab)
        {
            await HideInner();
            _container.InstantiatePrefab(prefab, _mainCanvas.transform);
            Destroy(gameObject);
        }

        protected async Task HideAndOpen(GameObject prefab, Transform parent)
        {
            await HideInner();
            _container.InstantiatePrefab(prefab, parent);
            Destroy(gameObject);
        }

        protected async Task HideAndCall(Action action)
        {
            await HideInner();
            action.Invoke();
            Destroy(gameObject);
        }

        protected async Task Hide()
        {
            await HideInner();
            Destroy(gameObject);
        }

        private async Task HideInner()
        {
            if (_animator != null && _animator.HasParameter(VISIBLE))
            {
                _animator.SetBool(VISIBLE, false);
                await Task.Delay(SHOW_HIDE_DELAY);
            }
        }

        protected virtual void Reset()
        {
            if (_animator == null)
            {
                TryGetComponent(out Animator _animator);
            }
        }
    }
}