using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent(typeof(CarController))]
public class CarUserControl : MonoBehaviour {
    private CarController m_Car; // the car controller we want to use
    private Fuel m_Fuel;

    private void Awake() {
        // get the car controller
        m_Car = GetComponent<CarController>();
        m_Fuel = GetComponent<Fuel>();
    }

    private void FixedUpdate() {
        // pass the input to the car!
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        if (m_Fuel.Value <= 0) {
            v = 0;
        }
#if !MOBILE_INPUT
        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
        m_Car.Move(h, v, v, handbrake);
#else
        m_Car.Move(h, v, v, 0f);
#endif
    }
}
