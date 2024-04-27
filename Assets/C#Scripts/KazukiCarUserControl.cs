using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.InputSystem;
using Autohand;

namespace KazukiSample
{
    public class KazukiCarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use

        float verticalValue;
        float handbrakeValue;

        // Hand Component of Auto Hand
        // オートハンドのハンドコンポネント
        public Hand r_Hand;
        public Hand l_Hand;

        [SerializeField]
        AutoHandControl autoHandControl;

        // Handle hinge joint
        // ハンドルのヒンジジョイント
        public HingeJoint steeringHingeJoint;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();

            //Create and use an instance of the AutoHand input system
            autoHandControl = new AutoHandControl();

            autoHandControl.Enable();
        }

        private void FixedUpdate()
        {
            //The angle at which the steering wheel is turned moves the tires.
            //車の角度
            float angle = steeringHingeJoint.angle;
            if (angle < 1 && angle > -1) angle = 0;
            float mappedValue = MapToNormalized(angle);

            if (autoHandControl.AutoHand.TriggerPressedR.inProgress ) verticalValue = 1; else verticalValue = 0;
            if (autoHandControl.AutoHand.TriggerPressedL.inProgress) handbrakeValue = 1; else handbrakeValue = 0;

            //Move a car using the Move function of a standard asset.
            //車を動かす
            m_Car.Move(-mappedValue, verticalValue, verticalValue, handbrakeValue);
            //Debug.Log($"<color=#ff69b4>Move</color> : 角度{-mappedValue}:Vertical{verticalValue}:ブレーキ{handbrakeValue}");
        }
        private void OnDestroy()
        {
            autoHandControl?.Dispose();
        }

        public float MapToNormalized(float angle)
        {
            // 範囲内の値を正規化する
            float normalizedValue = Mathf.InverseLerp(steeringHingeJoint.limits.min, steeringHingeJoint.limits.max, angle);

            // -1 から 1 の範囲にマッピングする
            float mappedValue = normalizedValue * 2f - 1f;

            return mappedValue;
        }
    }
}
