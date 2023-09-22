using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers.Hub.BuildMode
{
    public interface IBuildState
    {
        void EndState();
        void OnAction(Vector3 position);
        void UpdateState(Vector3 position);
    }
}
