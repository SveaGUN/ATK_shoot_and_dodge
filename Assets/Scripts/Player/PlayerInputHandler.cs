using System;
using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤー入力の受け取りと通知を行う
public class PlayerInputHandler : MonoBehaviour
{
    private InputActionMap _map = null;

    public event Action<Vector2> OnMove;
    public event Action<Vector2, string> OnLook;

    /// <summary>
    /// InputMapとActionの作成
    /// </summary>
    public void Init()
    {
        //Playerという名前でActionMapを作成する
        _map = new InputActionMap("Player");

        //MoveのActionを作成
        var moveAction = _map.AddAction("Move", InputActionType.Value);
        //LookのActionを作成
        var lookAction = _map.AddAction("Look", InputActionType.Value);

        //moveActionのBindingを設定
        moveAction.AddBinding("<Gamepad>/leftStick", groups: "Gamepad");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w", groups: "Keyboard")
            .With("Down", "<Keyboard>/s", groups: "Keyboard")
            .With("Left", "<Keyboard>/a", groups: "Keyboard")
            .With("Right", "<Keyboard>/d", groups: "Keyboard");

        //lookActionのBindingを設定
        lookAction.AddBinding("<Gamepad>/rightStick", groups: "Gamepad");
        lookAction.AddBinding("<Mouse>/position", groups: "Keyboard");

        _map.Enable();

        //イベントの登録
        moveAction.performed += context => OnMove.Invoke(context.ReadValue<Vector2>());
        moveAction.canceled += context => OnMove.Invoke(new Vector2(0, 0));
        lookAction.performed += context =>
        {
            //現在操作しているデバイスを取得
            var device = context.control.device;

            var controlScheme = "Unknow";

            //デバイスの種類からcontrolSchemeを指定する
            if (device is Mouse) { controlScheme = "Mouse"; }
            else { controlScheme = "Gamepad"; }

            OnLook.Invoke(context.ReadValue<Vector2>(), controlScheme);
        };
        lookAction.canceled += context => OnLook.Invoke(new Vector2(0, 0), "");
    }

    private void OnDisable()
    {
        _map.Disable();
    }
}
