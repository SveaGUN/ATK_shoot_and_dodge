using System;
using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤー入力の受け取りと通知を行う
public class PlayerInputHandler : MonoBehaviour
{
    private InputActionMap _map = null;

    public event Action<Vector2> OnMove;

    /// <summary>
    /// InputMapとActionの作成
    /// </summary>
    public void Init()
    {
        //Playerという名前でActionMapを作成する
        _map = new InputActionMap("Player");

        //MoveのActionを作成
        var moveAction = _map.AddAction("Move", InputActionType.Value);

        //moveActionのBindingを設定
        moveAction.AddBinding("<Gamepad>/leftStick", groups: "Gamepad");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w", groups: "Keyboard")
            .With("Down", "<Keyboard>/s", groups: "Keyboard")
            .With("Left", "<Keyboard>/a", groups: "Keyboard")
            .With("Right", "<Keyboard>/d", groups: "Keyboard");


        _map.Enable();

        //イベントの登録
        moveAction.performed += context => OnMove.Invoke(context.ReadValue<Vector2>());
        moveAction.canceled += context => OnMove.Invoke(new Vector2(0, 0));
    }

    private void OnDisable()
    {
        _map.Disable();
    }
}
