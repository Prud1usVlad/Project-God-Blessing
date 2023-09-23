using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public interface IBuildState
    {
        void EndState();
        void OnAction(Vector3 position,
            Action<Action<Vector3>, Vector3> confirm = null);
        void UpdateState(Vector3 position);
    }
}
