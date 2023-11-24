using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoodlesUtilities
{
    public class OnVisibility : MonoBehaviour
    {
        public Action OnBecomeVisible;
        public Action OnBecomeInvisible;
        public Action OnVisible;
        public Action OnInvisible;
        
        public bool IsVisible { get; private set; }

        private Renderer _renderer;
        private readonly List<Vector3> _points = new List<Vector3>();
        
        private Camera _camera;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            var bounds = _renderer.bounds;
            _points.Add(bounds.center);
            _points.Add(bounds.max);
            _points.Add(bounds.max - Vector3.forward * bounds.extents.z * 2);
            _points.Add(bounds.min);
            _points.Add(bounds.min + Vector3.forward * bounds.extents.z * 2);
            
            _camera = Camera.main;
        }
        
        private void Update()
        {
            CheckVisibility();
        }
        
        private void CheckVisibility()
        {
            // Check all bounds/centre to see if any are in camera view
            var isObjectVisible = _points.Select(point => _camera.WorldToViewportPoint(point))
                .Any(screenPoint => screenPoint is { z: > 0, x: > 0 and < 1, y: > 0 and < 1 });


            if (isObjectVisible)
            {
                OnVisible?.Invoke();
                
                if (!IsVisible)
                {
                    OnBecomeVisible?.Invoke();
                }
            }
            else
            {
                OnInvisible?.Invoke();
                
                if (IsVisible)
                {
                    OnBecomeInvisible?.Invoke();
                }
            }

            IsVisible = isObjectVisible;
        }
    }
}
