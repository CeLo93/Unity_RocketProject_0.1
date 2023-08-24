using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform player; // Referência ao jogador

    [SerializeField] private float distanceX = 0f; // Distância da câmera para o jogador
    [SerializeField] private float distanceY = 5.0f; // Distância da câmera para o jogador
    [SerializeField] private float distanceZ = 5.0f; // Distância da câmera para o jogador

    [SerializeField] private float sensitivityX = 2.0f; // Sensibilidade de rotação horizontal
    [SerializeField] private float sensitivityY = 2.0f; // Sensibilidade de rotação vertical
    [SerializeField] private float minYAngle = -60.0f; // Ângulo mínimo de rotação vertical
    [SerializeField] private float maxYAngle = 60.0f; // Ângulo máximo de rotação vertical

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor para dentro da janela do jogo
    }

    private void Update()
    {
        // Verifica se o botão direito do mouse está pressionado
        bool isRotating = Input.GetMouseButton(1);

        if (isRotating)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(distanceX, distanceY, -distanceZ);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * dir;
        transform.LookAt(player.position);
    }
}