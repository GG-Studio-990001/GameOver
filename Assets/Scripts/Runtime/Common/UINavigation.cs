using Runtime.Common.Define;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Common
{
    public class UINavigation : MonoBehaviour
    {
        [SerializeField] private Canvas rootCanvas;
        
        private ViewBase _currentView;
        private Queue<ViewBase> _viewQueue = new Queue<ViewBase>();
        private Stack<ViewBase> _viewStack = new Stack<ViewBase>();

        public void Push(UIType type)
        {
            
        }
    }
}
