using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Download {
    public abstract class CursorEventListener : MonoBehaviour {
        public virtual void OnHoverEnter() { }
        public virtual void OnHoverExit() { }

        public virtual void OnClickEnter() { }
        public virtual void OnClickExit() { }

        public virtual void OnSubbuttonClickEnter() { }
        public virtual void OnSubbuttonClickExit() { }
    }
}
