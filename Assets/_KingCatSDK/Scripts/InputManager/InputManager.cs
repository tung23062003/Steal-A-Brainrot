using System.Collections.Generic;
using System.Linq;
using KingCat.Base;
using UnityEngine;

namespace KingCat.Base.Clickable
{
    public interface IClickable
    {
        void OnClickDown();
        void OnClickUp();
        void OnClickHold();
        void OnClick();
    }

    public class InputManager : MonoSingleton<InputManager>
    {
        void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            var targets = TryGetInputTargets();
            if (targets == null) return;


            
            if (Input.GetMouseButtonDown(0)) // Left mouse button down
            {
                foreach (IClickable target in targets)
                {
                    target.OnClickDown();
                }
            }
            else if (Input.GetMouseButtonUp(0)) // Left mouse button up
            {
                foreach (IClickable target in targets)
                {
                    target.OnClickUp();
                    target.OnClick(); // Confirm click action
                }
            }
            else if (Input.GetMouseButton(0)) // Holding the button
            {
                foreach (IClickable target in targets)
                {
                    target.OnClickHold();
                }
            }

            targets.Clear();
        }

        private List<IClickable> TryGetInputTargets()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length > 0)
            {
                var results = new List<IClickable>();
                foreach (var hit in hits)
                {
                    var clickableComp = hit.collider.GetComponent<IClickable>();
                    if (clickableComp!=null) results.Add(clickableComp);
                }
                return results;
            }

            return null;
        }
    }

}
