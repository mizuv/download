using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Download {
    public interface ICursorEventListener {
        public void OnHoverEnter() { }
        public void OnHoverExit() { }

        public void OnClickEnter() { }
        public void OnClickExit() { }

        public void OnSubbuttonClickEnter() { }
        public void OnSubbuttonClickExit() { }
    }
}
